using BlogApi.Client.Models;

namespace BlogApi.Client.Interfaces;
public interface IPostEndpoint
{
    Task<IEnumerable<Post>> GetAsync();
    Task<IEnumerable<Post>> GetAsync(int id);
    Task<Post> PostAsync(Post post);
}