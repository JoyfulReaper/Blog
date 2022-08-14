using AutoMapper;
using BlogApi.Dtos;
using BlogApi.Library.Models;

namespace BlogApi;

public class BlogApiProfile : Profile
{
	public BlogApiProfile()
	{
        CreateMap<Post, PostCreateRequest>();
        CreateMap<PostCreateRequest, Post>();
    }
}
