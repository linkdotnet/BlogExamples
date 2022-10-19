using LiteDB;

using var database = new LiteDatabase("database.db");
int selection;
do
{
    Console.WriteLine("BlogPost Editor");
    Console.WriteLine("1: Write blog post");
    Console.WriteLine("2: Read blog post");
    Console.WriteLine("3: Get count of all published blog posts");
    Console.WriteLine("4: Get blog posts with tag");
    Console.WriteLine("5: Quit");
    if (!int.TryParse(Console.ReadLine(), out selection))
    {
        Console.WriteLine("Could not understand your selection. Try again");
        continue;
    }

    Console.WriteLine();

    switch (selection)
    {
        case 1:
            WriteBlogPost(database);
            break;
        case 2:
            ReadBlogPost(database);
            break;
        case 3:
            GetAll(database);
            break;
        case 4:
            GetAllByTag(database);
            break;
        case 5:
            break;
        default:
            Console.WriteLine("Could not understand your selection. Try again");
            break;
    }
} while (selection != 5);

void WriteBlogPost(ILiteDatabase db)
{
    Console.Write("Title:");
    var title = Console.ReadLine();
    Console.Write("Published? (true/false): ");
    bool.TryParse(Console.ReadLine(), out var isPublished);
    Console.Write("Tags (comma-seperated):");
    var tags = Console.ReadLine();
    var blogPost = new BlogPost { Title = title, IsPublished = isPublished, Tags = tags.Split(",").ToList() };
    db.GetCollection<BlogPost>().Insert(blogPost);
    Console.WriteLine($"Created blog post with id: {blogPost.Id}");
}

void ReadBlogPost(ILiteDatabase db)
{
    Console.Write("Which id?: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Id has to be an integer");
    }

    var blogPost = db.GetCollection<BlogPost>().FindById(id);

    Console.WriteLine(blogPost is null
        ? "Could not find blog post"
        : $"Found blog post with title: {blogPost.Title}");
}

void GetAll(ILiteDatabase db)
{
    var published = db.GetCollection<BlogPost>().Count(bp => bp.IsPublished);
    Console.WriteLine($"All published {published}");
}

void GetAllByTag(ILiteDatabase db)
{
    Console.Write("Which tag: ");
    var tag = Console.ReadLine();

    var blogPosts = db.GetCollection<BlogPost>().Query();

    var blogPostWithTag =
        (from bp in blogPosts
        where bp.Tags.Contains(tag)
        select bp).ToList();

    Console.WriteLine($"Found {blogPostWithTag.Count} entries");
    foreach (var post in blogPostWithTag)
    {
        Console.WriteLine($"Title: {post.Title}");
    }
}

public class BlogPost
{
    public int Id { get; set; }

    public string Title { get; set; }

    public bool IsPublished { get; set; }

    public List<string> Tags { get; set; }
}