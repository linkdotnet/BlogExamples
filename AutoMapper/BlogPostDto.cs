namespace AutoMapper;

public class BlogPostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly PublishedDate { get; set; }
}