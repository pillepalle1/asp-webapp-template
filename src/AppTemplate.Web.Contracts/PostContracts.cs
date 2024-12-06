namespace AppTemplate.Web.Contracts;

public class PostResponse
{
    [JsonPropertyName("post_id")]
    public required Guid Id { init; get; }
    
    [JsonPropertyName("timestamp")]
    public required DateTimeOffset Timestamp { init; get; }
    
    [JsonPropertyName("content")]
    public required string Content { init; get; }
}

public class CreatePostRequest
{
    [JsonPropertyName("content")]
    public required string Content { init; get; }
}