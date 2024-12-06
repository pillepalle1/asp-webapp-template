using AppTemplate.Application.Cqrs.Blogs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppTemplate.Web.Pages.Blogs;

public class IndexModel(
    IMediator mediator) : PageModel
{
    public ImmutableList<BlogHeader> BlogHeaders { set; get; } = ImmutableList<BlogHeader>.Empty;
    
    [BindProperty] public string BlogTitle { get; set; } = String.Empty;
    
    public async Task OnGetAsync()
    {
        var fetch = await mediator.Send(new AllBlogHeadersQuery());

        if (fetch.Succeeded())
        {
            BlogHeaders = fetch.Unwrap();
        }
    }
    
    public async Task OnPostAddBlogAsync()
    {
        var add = await mediator.Send(new CreateBlogCmd
        {
            Title = BlogTitle
        });

        if (add.Succeeded())
        {
            // Redirect to the error page or do something useful
        }
        
        var fetch = await mediator.Send(new AllBlogHeadersQuery());

        if (fetch.Succeeded())
        {
            BlogHeaders = fetch.Unwrap();
        }
    }
}
