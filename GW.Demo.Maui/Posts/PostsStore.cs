namespace GW.Demo.Maui.Posts;

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;



public class PostsStore
{
    private readonly IPostsClient _Client;
    private readonly string _CacheFile;



    public ObservableCollection<Post> Items
    {
        get; 
    } = new();


    public PostsStore(IPostsClient client)
    {
        _Client = client;
        _CacheFile = Path.Combine(FileSystem.Current.CacheDirectory, "posts-cache.json");
    }


    public async Task LoadAsync(string? query, CancellationToken ct)
    {
        PostsResponse resp = string.IsNullOrWhiteSpace(query) ? await _Client.GetAllAsync(100, 0, ct) : await _Client.GetAsync(query!, 100, 0, ct);

        Items.Clear();
        foreach(Post i in resp.Items) { Items.Add(i); }
    }


    public async Task SaveAsJsonAsync(CancellationToken ct)
    {
        PostsResponse r = new PostsResponse
        {
            Limit = Items.Count,
            Total = Items.Count,
            Items = [.. Items]
        };

        var json = JsonSerializer.Serialize(r, new JsonSerializerOptions { WriteIndented = true });
        using var wr = new StreamWriter(_CacheFile, false, Encoding.UTF8);
        await wr.WriteAsync(json.AsMemory(), ct);
    }


    public async Task<bool> LoadFromJsonAsync(CancellationToken ct)
    {
        if(!File.Exists(_CacheFile))
        {
            return false;
        }

        string json = await File.ReadAllTextAsync(_CacheFile, ct);
        PostsResponse r = JsonSerializer.Deserialize<PostsResponse>(json) ?? new PostsResponse();

        Items.Clear();
        foreach(Post i in r.Items) { Items.Add(i); }

        return true;
    }
}
