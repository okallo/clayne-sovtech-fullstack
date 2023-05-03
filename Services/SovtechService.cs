using System.Net;
using System.Text.Json;
using clayne_sovtech_fullstack.models;

namespace clayne_sovtech_fullstack.services;
public class SovtechService : ISovtechService
{
    private readonly HttpClient _httpClient;
    private readonly string _jokeBaseUrl = "https://api.chucknorris.io/jokes/";
    private readonly string _peopleBaseUrl = "https://swapi.dev/api/people/";

    public SovtechService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public HttpResponseException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }

    public async Task<SearchResult<Joke>> GetRandomJokeAsync(int pageSize = 10, int pageNumber = 1)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_jokeBaseUrl}random");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var joke = JsonSerializer.Deserialize<SearchResult<Joke>>(content);
            if (joke?.Results == null)
            {
                return new SearchResult<Joke>();
            }
            else
            {
                return joke;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }

    }


    public async Task<SearchResult<Joke>> GetJokeByCategoryAsync(string category, int pageSize = 10, int pageNumber = 1)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_jokeBaseUrl}random?category={category}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var joke = JsonSerializer.Deserialize<SearchResult<Joke>>(content);
            if (joke?.Results == null)
            {
                return new SearchResult<Joke>();
            }
            return joke;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }

    }

    public async Task<string[]> GetCategoriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_jokeBaseUrl}categories");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<string[]>(content);
            if (items == null || items.Length == 0 || items.All(item => string.IsNullOrEmpty(item)))
            {
                return new string[0];
                //throw new Exception("Categories not found");
                // throw new HttpResponseException(HttpStatusCode.NotFound, "Categories not found");
            }
            else
            {
                return items;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }

    }

    public async Task<SearchResult<Person>> GetAllPeopleAsync(int pageSize = 10, int pageNumber = 1)
    {
        try
        {
            var url = $"{_peopleBaseUrl}?page={pageNumber}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<SearchResult<Person>>(content, options);
                Console.WriteLine(result);
                if (result?.Results?.Count() > 0)
                {
                    var res = result.Results;
                    return result;
                }
            }
            return new SearchResult<Person>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }


    }

    public async Task<SearchResult<Joke>> SearchJokeAsync(string query, int pageSize = 10, int pageNumber = 1)
    {

        try
        {
            var url = $"{_jokeBaseUrl}search?query={query}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<SearchResult<Joke>>(content, options);
                if (result?.Results?.Count() > 0)
                {
                    var res = result;
                    return res;
                }
            }
            return new SearchResult<Joke>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    public async Task<SearchResult<Person>> SearchPeopleAsync(string query, int pageSize = 10, int pageNumber = 1)
    {
        try
        {
            var peopleUrl = $"{_peopleBaseUrl}?search={query}";
            var peopleResponse = await _httpClient.GetAsync(peopleUrl);

            if (peopleResponse.IsSuccessStatusCode)
            {
                var content = await peopleResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<SearchResult<Person>>(content, options);
                if (result?.Results?.Count() > 0)
                {
                    return result;
                }
            }
            return new SearchResult<Person>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }

    }


}