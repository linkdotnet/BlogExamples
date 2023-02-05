using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;

namespace MultiTenantravenDB.Controllers;

[ApiController]
[Route("blogposts")]
public class BlogPostController : ControllerBase
{
    private readonly ITenantDocumentStore _documentStore;

    public BlogPostController(ITenantDocumentStore documentStore)
    {
        _documentStore = documentStore;
    }

    [HttpGet]
    [Route("list")]
    public async Task<List<BlogPost>> Get()
    {
        using var session = _documentStore.DocumentStore.OpenAsyncSession();
        return await session.Query<BlogPost>().ToListAsync();
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> Add([FromBody] CreateBlogPostRequest request)
    {
        using var session = _documentStore.DocumentStore.OpenAsyncSession();
        await session.StoreAsync(new BlogPost() { Title = request.Title });
        await session.SaveChangesAsync();

        return Ok();
    }
}