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

    public async Task<Joke> GetRandomJokeAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_jokeBaseUrl}random");
            if (response.IsSuccessStatusCode) {

            var content = await response.Content.ReadAsStringAsync();
            var joke = JsonSerializer.Deserialize<Joke>(content);
            if (joke != null)
            {
                return joke;
            }
            }
            return new Joke();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }

    }


    public async Task<Joke> GetJokeByCategoryAsync(string category)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_jokeBaseUrl}random?category={category}");
           if (response.IsSuccessStatusCode) {
               
            var content = await response.Content.ReadAsStringAsync();
            var joke = JsonSerializer.Deserialize<Joke>(content);
            if (joke != null)
            {
                return joke;
            }
            
           }
          
                return new Joke();
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
            if (response.IsSuccessStatusCode) {

            var content = await response.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<string[]>(content);
            if (items != null)
            {
               return items;
            }
            }
             return new string[0];

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }

    }

    public async Task<SearchResult<Person>> GetAllPeopleAsync( int pageNumber = 1)
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

    public async Task<SearchResult<Joke>> SearchJokeAsync(string query)
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
                if (result?.total > 0)
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

    public async Task<SearchResult<Person>> SearchPeopleAsync(string query)
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