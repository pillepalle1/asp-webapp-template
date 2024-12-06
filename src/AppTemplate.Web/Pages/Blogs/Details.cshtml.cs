using AppTemplate.Application.Cqrs.Blogs;
using AppTemplate.Application.Cqrs.Posts;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppTemplate.Web.Pages.Blogs;

public class Details(
    IMediator mediator) : PageModel
{
    [BindProperty(SupportsGet = true)] public Guid BlogId { get; set; }

    [BindProperty] public string PostContent { get; set; } = String.Empty;
    
    public Blog? Blog { get; set; }
    
    public async Task OnGetAsync(string content)
    {
        var fetch = await mediator.Send(new BlogQuery
        {
            BlogId = BlogId
        });

        if (fetch.Succeeded())
        {
            Blog = fetch.Unwrap();
        }
    }

    public async Task OnPostAddPostAsync()
    {
        var add = await mediator.Send(new CreatePostCmd
        {
            BlogId = BlogId,
            Timestamp = DateTimeOffset.UtcNow,
            Content = PostContent
        });

        if (add.Succeeded())
        {
            Blog = add.Unwrap();
        }
    }
}