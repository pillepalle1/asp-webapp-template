using AppTemplate.Application.Cqrs.Blogs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppTemplate.Web.Pages;

public class IndexModel(
    IMediator mediator) : PageModel
{
    public void OnGet()
    {
        
    }
}
