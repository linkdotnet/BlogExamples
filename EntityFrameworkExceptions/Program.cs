using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

await using var context = new BlogContext();
context.Database.OpenConnection();
context.Database.EnsureCreated();

var blogPostWithNullTitle = new BlogPost
{
    Title = null,
    Content = "Content"
};

context.BlogPosts.Add(blogPostWithNullTitle);
try
{
    await context.SaveChangesAsync();
}
catch (CannotInsertNullException e)
{
    // Do something specific to this exception
    Console.WriteLine(e);
}