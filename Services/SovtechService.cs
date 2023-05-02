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
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var joke = JsonSerializer.Deserialize<Joke>(content);
            if (joke?.Value == null)
            {
                return new Joke();
                //throw new Exception("Chuck Norris Joke not found");
                //throw new HttpResponseException(HttpStatusCode.NotFound, "Chuck Norris Joke not found");
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


    public async Task<Joke> GetJokeByCategoryAsync(string category)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_jokeBaseUrl}random?category={category}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var joke = JsonSerializer.Deserialize<Joke>(content);
            if (joke?.Value == null)
            {
                throw new Exception("Chuck Norris Joke not found");
                //throw new HttpResponseException(HttpStatusCode.NotFound, "Chuck Norris Joke not found");
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

    public async Task<IEnumerable<Person>> GetAllPeopleAsync(int page = 1)
    {
        try
        {
            var url = $"{_peopleBaseUrl}?page={page}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<SearchPeopleResult<Person>>(content, options);
                Console.WriteLine(result);
                if (result?.Results?.Count() > 0)
                {
                    var res = result.Results;
                    return res;
                }
            }
            return Enumerable.Empty<Person>();
            //throw new Exception("Nobody by was found");
            //throw new HttpResponseException(HttpStatusCode.NotFound, "People not found");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }


    }

    public async Task<IEnumerable<Joke>> SearchJokeAsync(string query, int page = 1)
    {

        try
        {
            var url = $"{_jokeBaseUrl}search?query={query}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<ChuckSearchResult>(content, options);
                if (result?.Result?.Count() > 0)
                {
                    var res = result.Result;
                    return res;
                }
            }
            return Enumerable.Empty<Joke>();
            //throw new Exception($"Chuck Norris joke containing {query} not found");
            //throw new HttpResponseException(HttpStatusCode.NotFound, "Chuck Norris joke not found");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }
    }

    public async Task<IEnumerable<object>> SearchPeopleAsync(string query, int page = 1)
    {
        try
        {
            var results = new List<object>();

            // Search for people
            var peopleUrl = $"{_peopleBaseUrl}?search={query}";
            var peopleResponse = await _httpClient.GetAsync(peopleUrl);

            if (peopleResponse.IsSuccessStatusCode)
            {
                var content = await peopleResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<SwapiSearchResult<Person>>(content, options);
                if (result?.Results?.Count() > 0)
                {
                    var res = result.Results;
                    results.AddRange(result.Results);
                    return results;
                }
            }
            return Enumerable.Empty<object>();
            //throw new Exception($"Nobody named {query} not found in swapi");
            //throw new HttpResponseException(HttpStatusCode.NotFound, "Nobody was found");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new HttpResponseException(HttpStatusCode.BadRequest, ex.Message);
        }

    }


}