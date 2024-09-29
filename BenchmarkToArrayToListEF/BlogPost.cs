public class BlogPost
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public DateTime PublishDate { get; set; }
    public int Likes { get; set; }
}