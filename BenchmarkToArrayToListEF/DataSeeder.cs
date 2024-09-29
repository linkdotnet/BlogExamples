public static class DataSeeder  
{  
    public static void Seed(BlogContext context, int numberOfPosts)  
    {  
        var blogPosts = new List<BlogPost>();  
        for (var i = 1; i <= numberOfPosts; i++)  
        {  
            blogPosts.Add(new BlogPost  
            {  
                Title = $"Title {i}",  
                Subtitle = $"Subtitle {i}",  
                PublishDate = DateTime.Now.AddDays(-i),  
                Likes = i  
            });  
        }  

        context.BlogPosts.AddRange(blogPosts);  
        context.SaveChanges();  
    }  
}