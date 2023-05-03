using clayne_sovtech_fullstack.models;

namespace clayne_sovtech_fullstack.services;
public interface ISovtechService
{
     Task<SearchResult<Joke>> GetRandomJokeAsync(int pageSize = 10, int pageNumber = 1);
    Task<SearchResult<Joke>> GetJokeByCategoryAsync(string category,int pageSize = 10, int pageNumber = 1);
    Task<SearchResult<Joke>> SearchJokeAsync(string query, int pageSize = 10, int pageNumber = 1);

    Task<string[]> GetCategoriesAsync();
    Task<SearchResult<Person>> GetAllPeopleAsync(int pageSize = 10, int pageNumber = 1);
    Task<SearchResult<Person>> SearchPeopleAsync(string query, int pageSize = 10, int pageNumber = 1);

}