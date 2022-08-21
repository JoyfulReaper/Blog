using BlogApi.Client.Interfaces;
using BlogApi.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Client;
public class PostEndpoint : EndPoint, IPostEndpoint
{
    private readonly HttpClient _client;

    public PostEndpoint(HttpClient client)
    {
        _client = client;
    }


    public async Task<IEnumerable<Post>> GetAsync()
    {
        var posts = await _client.GetFromJsonAsync<IEnumerable<Post>>("api/Post");
        ThrowIfNull(posts);
        return posts!;
    }

    public async Task<IEnumerable<Post>> GetAsync(int id)
    {
        var post = await _client.GetFromJsonAsync<IEnumerable<Post>>($"api/Post/{id}");
        ThrowIfNull(post);
        return post!;
    }

    public async Task<Post> PostAsync(Post post)
    {
        using var response = await _client.PostAsJsonAsync("api/Post", post);
        CheckResponse(response);
        var resourceResp = await response.Content.ReadFromJsonAsync<Post>();
        ThrowIfNull(resourceResp);
        return resourceResp!;
    }

}
