using Microsoft.AspNetCore.Mvc;
using StFrancisHouse.Models;

namespace StFrancisHouse.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController/Create
        public ActionResult Create()
        {
            return View();
        }


        public IActionResult AjaxMethod(string name)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            return View(context.getAllClients());
        }

        //perhaps a different thing than view than the Ajax method above?
        //
        //url/home/getAllClients
        //Returns ALL the clients. This is a huge list. 
        public List<Client> getAllClients()
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            List<Client> clients = context.getAllClients();

            return clients;
        }


    //    /*
    //    * When you have all firstname parameter for getClientByInfo.
    //    * 
    //    */
    //    public List<Client> getClientByInfo(string firstName)
    //    {
    //        UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;
    //
    //        //object of some sort.
    //
    //        //pass in object of some sort.
    //        List<Client> clients = context.getClientByInfo(firstName);
    //
    //        return clients;
    //    }

        /*
         * When you have all 3 parameters for getClientByInfo.
         * Best search results
         */
        public List<Client> getClientByInfo(string firstName, string lastName, string birthdate)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            //object of some sort.

            //pass in object of some sort.
            List<Client> clients = context.getClientByInfo(firstName, lastName, birthdate);

            return clients;
        }


        /**
         * List<Client>
         * @returns a list of the client added to the DB.
         */
        public void postClientbyInfo(string firstName, string lastName, string middleInitial, string suffix, string birthdate, string race, string gender, int ZipCode)
        {

            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            //pass in object of some sort.
            //List<Client> clients = context.createNewClient(firstName, lastName, middleInitial, suffix, birthdate, race, gender, ZipCode);
            context.createNewClient(firstName, lastName, middleInitial, suffix, birthdate, race, gender, ZipCode);

            //return clients;

        }

        public void updateClientByID(int clientID, string firstName, string lastName, string middleInitial, string suffix, string birthdate, string race, string gender, int ZipCode)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            context.updateClientByID(clientID,firstName, lastName, middleInitial, suffix, birthdate, race, gender, ZipCode);
        }

        public List<Client> getClientVisits(int clientID)
        {

            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;



            List<Client> clients = context.getClientVisits(clientID);

            return clients;
        }

        public Visit getVisitByID(int visitID)
        {            
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            Visit Visit = context.getVisitByID(visitID);

            return Visit;
        }



        public void createClientVisitByID(int clientID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            context.createClientVisitByID(clientID);
        }

        public Visit checkout(int visitID, int mens, int womens, int kids, bool backpack, bool sleepingbag, string request)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            Visit Visit = context.checkout(visitID, mens, womens, kids, backpack, sleepingbag, request);

            return Visit;
        }



        //public JsonResult get5ClientsJSON()
        //{
        //    UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;
        //
        //    List<Client> clients = context.getAllClients();
        //    JsonResult clientJSON = 
        //
        //    return clients;
        //}


        // POST: HomeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
