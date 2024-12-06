namespace AppTemplate.Web.Contracts;

public class BlogHeaderResponse
{
    [JsonPropertyName("blog_id")]
    public required Guid Id { init; get; }
    
    [JsonPropertyName("title")]
    public required string Title { init; get; }
}

public class BlogResponse
{
    [JsonPropertyName("blog_id")]
    public required Guid Id { init; get; }
    
    [JsonPropertyName("title")]
    public required string Title { init; get; }
    
    [JsonPropertyName("posts")]
    public required ImmutableList<PostResponse> Posts { init; get; }
}

public class CreateBlogRequest
{
    [JsonPropertyName("title")]
    public required string Title { init; get; }
}