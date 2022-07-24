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

using BlogApi.Library.DataAccess;
using BlogApi.Library.Models;
using BlogApi.Library.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Library.Repositories;
public class PostRepo : IPostRepo
{
    private readonly IDataAccess _dataAccess;

    public PostRepo(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }


    public async Task<List<Post>> LoadAllPostsAsync()
    {
        return await _dataAccess.LoadDataAsync<Post, dynamic>("spPost_GetAll", new { }, "BlogApi");
    }

    public async Task SavePostAsync(Post post)
    {
        int id = await _dataAccess.SaveDataAndGetIdAsync("spPost_Upsert", new
        {
            PostId = post.PostId,
            AuthorId = post.AuthorId,
            Title = post.Title,
            Content = post.Content,
            Ready = post.Ready,
            Slug = post.Slug,
            PostImage = post.PostImage,
            PostImageContentType = post.PostImageContentType
        }, "BlogApi");

        post.PostId = id;
    }
}
