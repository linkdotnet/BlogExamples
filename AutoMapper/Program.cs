using System.Threading.Channels;
using AutoMapper;

var blogPost = new BlogPost { Id = 1, Title = "Write your own AutoMapper in C#", PublishedDate = new DateOnly(2023, 3, 18) };
var dto = Mapper.Map<BlogPost, BlogPostDto>(blogPost);
Console.Write($"Blog Post: '{dto.Title}' was published at: {dto.PublishedDate}");