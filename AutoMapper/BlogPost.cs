namespace AutoMapper;

public class BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly PublishedDate { get; set; }
}