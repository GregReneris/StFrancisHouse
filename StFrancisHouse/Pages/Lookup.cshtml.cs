using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StFrancisHouse.Models;

namespace StFrancisHouse.Pages
{
    public class LookupModel : PageModel
    {
        public void OnGet()
        {
            //Console.WriteLine("GO TO LOOKUP PAGE");
        }

        public void getMessage()
        {
            Console.WriteLine("WE GOT HERE TO THIS BUTTON");


        }

    }
}





/// <summary>
/// REST Get request
/// </summary>
/// <param name="id"></param>
//public void OnGet(string id)
//{
//    Hike = HikeService.GetHikes().FirstOrDefault(m => m.Id.Equals(id));
//    if (Hike == null)
//    {
//        throw new ArgumentNullException("ID NOT FOUND");
//    }
//
//}