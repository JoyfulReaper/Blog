/*
MIT License

Copyright(c) 2022 Kyle Givler
https://github.com/JoyfulReaper

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using BlogApi.Library.Models;
using BlogApi.Library.Repositories.Interfaces;
using BlogApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace BlogApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IPostRepo _postRepo;
    private readonly IMapper _mapper;

    public PostController(IConfiguration config,
        IPostRepo postRepo,
        IMapper mapper)
    {
        _config = config;
        _postRepo = postRepo;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPostAsync(int id)
    {
        var post = await _postRepo.LoadAsync(id);
        if(post is null)
        {
            return NotFound();
        }

        return post;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetPostsAsync()
    {
        return await _postRepo.LoadAllAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Post>> PostAsync(PostCreateRequest postRequest)
    {
        if (postRequest.Password != _config["apiPassword"])
        {
            return Unauthorized();
        }

        var post =_mapper.Map<Post>(postRequest);
        await _postRepo.SaveAsync(post);

        return CreatedAtAction(nameof(GetPostAsync), new { id = post.PostId }, post);
    }
}
