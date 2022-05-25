namespace RavenDBUnitTest;

public class BlogPost : Aggregate
{
    public string Title { get; set; }

    public string Content { get; set; }
}