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

        public string foo()
        {
            return ("got here");
        }

        public void OnPostFuncFoo(object sender, EventArgs e)
        {
            Console.WriteLine("WE GOT HERE TO THIS funcfoo");

        }

        protected void populateTable(object sender, EventArgs e)
        {
            Console.WriteLine("m_click is here");   
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Console.WriteLine("m_click is here");
            //foo.ServerClick += new EventHandler(populateTable);

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