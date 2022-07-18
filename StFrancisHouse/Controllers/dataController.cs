using Microsoft.AspNetCore.Mvc;
using StFrancisHouse.Models;

namespace StFrancisHouse
{
    public class dataController : Controller
    {
        
        //currently calls getAllClients
        public IActionResult GetAllClients()
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            return View(context.getAllClients());
        }
    }
}
