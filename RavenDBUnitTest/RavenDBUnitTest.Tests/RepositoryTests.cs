using Raven.Client.Documents;
using Raven.TestDriver;
using RavenDBUnitTest.Infrastructure;

namespace RavenDBUnitTest.Tests;

public sealed class BlogPostRepositoryTests : RavenTestDriver
{
    private static bool serverRunning;
    private readonly IDocumentStore store;
    private readonly Repository<BlogPost> sut;

    public BlogPostRepositoryTests()
    {
        StartServerIfNotRunning();
        store = GetDocumentStore();
        sut = new Repository<BlogPost>(store);
    }
    
    public override void Dispose()
    {
        base.Dispose();
        store.Dispose();
    }

    [Fact]
    public async Task ShouldLoadBlogPost()
    {
        var blogPost = new BlogPost { Title = "Title", Content = "Content"};
        await SaveBlogPostAsync(blogPost);

        var blogPostFromRepo = await sut.GetByIdAsync(blogPost.Id);
        
        Assert.Equal("Title", blogPostFromRepo.Title);
        Assert.Equal("Content", blogPostFromRepo.Content);
    }
    
    [Fact]
    public async Task ShouldSaveBlogPost()
    {
        var blogPost = new BlogPost { Title = "Title", Content = "Content"};

        await sut.StoreAsync(blogPost);

        var blogPostFromContext = await GetBlogPostByIdAsync(blogPost.Id);
        Assert.NotNull(blogPostFromContext);
        Assert.Equal("Title", blogPostFromContext.Title);
        Assert.Equal("Content", blogPostFromContext.Content);
    }

    private static void StartServerIfNotRunning()
    {
        if (!serverRunning)
        {
            serverRunning = true;
            ConfigureServer(new TestServerOptions
            {
                DataDirectory = "./RavenDbTest/",
            });
        }
    }

    private async Task SaveBlogPostAsync(params BlogPost[] blogPosts)
    {
        using var session = store.OpenAsyncSession();
        foreach (var blogPost in blogPosts)
        {
            await session.StoreAsync(blogPost);
        }

        await session.SaveChangesAsync();
    }

    private async Task<BlogPost> GetBlogPostByIdAsync(string id)
    {
        using var session = store.OpenAsyncSession();
        return await session.Query<BlogPost>().SingleOrDefaultAsync(s => s.Id == id);
    }
}