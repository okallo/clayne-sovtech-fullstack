using clayne_sovtech_fullstack.models;
using clayne_sovtech_fullstack.services;
using Microsoft.AspNetCore.Mvc;

namespace clayne_sovtech_fullstack.Controllers;
public class SovtechController : ControllerBase
{

    private readonly ILogger<SovtechController> _logger;
    private readonly ISovtechService _sovtechService;

    public SovtechController(ILogger<SovtechController> logger, ISovtechService sovtechService)
    {
        _logger = logger;
        _sovtechService = sovtechService;
    }


    [HttpGet]
    [Route("/chuck/categories")]
    public Task<string[]> Get()
    {
        return _sovtechService.GetCategoriesAsync();
    }
     [HttpGet]
    [Route("/chuck/category/joke")]
    public Task<Joke> GetJokeByCategory(string category)
    {
        return _sovtechService.GetJokeByCategoryAsync(category);
    }
       [HttpGet]
    [Route("/chuck/random/joke")]
    public Task<Joke> GetRandomJoke()
    {
        return _sovtechService.GetRandomJokeAsync();
    }

    [HttpGet]
    [Route("/swapi/people")]
    public async Task<SearchResult<Person>> GetPeople(int pageNumber = 1)
    {
        return await _sovtechService.GetAllPeopleAsync(pageNumber);
    }
    [HttpGet]
    [Route("/search/{query}")]
    public async Task<Dictionary<string, object>> Search(string query)
    {

        var chuckResult = _sovtechService.SearchJokeAsync(query);
        var swapiResult = _sovtechService.SearchPeopleAsync(query);

        await Task.WhenAll(chuckResult, swapiResult);
        var chuckres = await chuckResult;
        var swapires = await swapiResult;
        var results = new Dictionary<string, object>
            {
                { "Jokes", chuckres},
                { "People", swapires}
            };
        return results;


    }

}