public record BlogPost
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Author { get; set; }
    public required DateTime Created { get; set; }
    public required DateTime Updated { get; set; }
    public required List<string> Tags { get; set; }
}