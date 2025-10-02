namespace GW.Demo.Maui.Posts;

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;



public class PostsClient: IPostsClient
{
    private readonly HttpClient _Http;


    public PostsClient(HttpClient httpClient)
    {
        (_Http = httpClient).BaseAddress = new Uri("https://dummyjson.com/");
    }


    public async Task<PostsResponse> GetAllAsync(int limit, int skip, CancellationToken ct)
    {
        using HttpResponseMessage resp = await _Http.GetAsync($"posts?limit={limit}&skip={skip}", ct);
        resp.EnsureSuccessStatusCode();

        PostsResponse? rval = await resp.Content.ReadFromJsonAsync<PostsResponse>(cancellationToken: ct);
        return rval ?? new PostsResponse();
    }


    public async Task<PostsResponse> GetAsync(string query, int limit, int skip, CancellationToken ct)
    {
        using HttpResponseMessage resp = await _Http.GetAsync($"posts/search?q={Uri.EscapeDataString(query ?? string.Empty)}&limit={limit}&skip={skip}", ct);
        resp.EnsureSuccessStatusCode();

        PostsResponse? rval = await resp.Content.ReadFromJsonAsync<PostsResponse>(cancellationToken: ct);
        return rval ?? new PostsResponse();
    }
}
