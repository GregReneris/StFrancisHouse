using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StFrancisHouse.Pages
{
    public class NavbarModel : PageModel
    {
        private readonly ILogger<NavbarModel> _logger;

        public NavbarModel(ILogger<NavbarModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}