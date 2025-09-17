namespace GW.Demo.Maui.Posts;

using System.Collections.Generic;
using System.Text.Json.Serialization;



public class PostsResponse
{
    [JsonPropertyName("limit")]
    public int Limit { get; set; }


    [JsonPropertyName("skip")]
    public int Skip { get; set; }


    [JsonPropertyName("total")]
    public int Total { get; set; }


    [JsonPropertyName("posts")]
    public List<Post> Items { get; set; } = new();
}
