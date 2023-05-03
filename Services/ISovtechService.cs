using clayne_sovtech_fullstack.models;

namespace clayne_sovtech_fullstack.services;
public interface ISovtechService
{
     Task<Joke> GetRandomJokeAsync();
    Task<Joke> GetJokeByCategoryAsync(string category);
    Task<SearchResult<Joke>> SearchJokeAsync(string query);

    Task<string[]> GetCategoriesAsync();
    Task<SearchResult<Person>> GetAllPeopleAsync( int pageNumber = 1);
    Task<SearchResult<Person>> SearchPeopleAsync(string query);

}