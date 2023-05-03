
namespace clayne_sovtech_fullstack.models;
public class Joke
{
    public string id { get; set; } = string.Empty;
    public string value { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;
    public string icon_url { get; set; } = string.Empty;
    public string created_at {get;set;} = string.Empty;
    public string updated_at {get;set;} =string.Empty;
    public string[]? categories {get;set;} 
}