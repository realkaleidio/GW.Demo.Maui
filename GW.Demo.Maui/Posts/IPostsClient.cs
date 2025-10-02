namespace GW.Demo.Maui.Posts;

using System.Threading;
using System.Threading.Tasks;



public interface IPostsClient
{
    Task<PostsResponse> GetAllAsync(int limit, int skip, CancellationToken ct);

    Task<PostsResponse> GetAsync(string query, int limit, int skip, CancellationToken ct);
}
