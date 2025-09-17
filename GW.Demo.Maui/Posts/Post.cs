namespace GW.Demo.Maui.Posts;

using System.Text.Json.Serialization;



public class Post
{
    [JsonPropertyName("id")]
    public int ID { get; set; }


    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;


    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;


    [JsonPropertyName("userId")]
    public int UserID { get; set; }
}
