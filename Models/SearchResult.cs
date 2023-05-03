namespace clayne_sovtech_fullstack.models;

public class SearchResult<T>
{
    public int Count { get; set; }
    public string Next { get; set; } = string.Empty;
    public string Previous { get; set; } = string.Empty;
    public int total {get;set;}
    public T[]? result { get; set; }
    public List<T>? Results { get; set; }
}

// public class ChuckSearchResult
// {
//     public int Count { get; set; }
//     public string Next { get; set; } = string.Empty;
//     public string Previous { get; set; } = string.Empty;
//     public IEnumerable<Joke>? Result { get; set; }
// }

// public class SwapiSearchResult<T>
// {
//     public int Count { get; set; }
//     public string Next { get; set; } = string.Empty;
//     public string Previous { get; set; } = string.Empty;
//     public IEnumerable<T>? Results { get; set; }
// }