using clayne_sovtech_fullstack.models;

namespace clayne_sovtech_fullstack.services;
public interface ISovtechService
{
     Task<Joke> GetRandomJokeAsync();
    Task<Joke> GetJokeByCategoryAsync(string category);
    Task<IEnumerable<Joke>> SearchJokeAsync(string query, int limit = 50, int page = 1);

    Task<string[]> GetCategoriesAsync();
    Task<IEnumerable<Person>> GetAllPeopleAsync(int page = 1);
    Task<IEnumerable<object>> SearchPeopleAsync(string query, int limit = 50, int page = 1);

}