﻿using Microsoft.AspNetCore.Mvc;
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



        public List<Volunteer> getAllUsers()
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            List<Volunteer> Volunteers = context.getAllVolunteers();

            return Volunteers;
        }

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
         * First Name Last Name and birthday are now required fields. 
         * @returns a list of the client added to the DB.
         */
        public List<Client> postClientbyInfo(string firstName, string lastName, string middleInitial, string suffix, string birthdate, string race, string gender, int numFamily, int ZipCode)
        {

            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            //pass in object of some sort.
            //List<Client> clients = context.createNewClient(firstName, lastName, middleInitial, suffix, birthdate, race, gender, ZipCode);
            //context.createNewClient(firstName, lastName, middleInitial, suffix, birthdate, race, gender, ZipCode);

            List<Client> clients = context.createNewClient(firstName, lastName, middleInitial, suffix, birthdate, race, gender, numFamily, ZipCode);

            return clients;

        }

        public void updateClientByID(int clientID, string firstName, string lastName, string middleInitial, string birthdate, string race, string gender, int numKids, int ZipCode, bool banned)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            context.updateClientByID(clientID,firstName, lastName, middleInitial, birthdate, race, gender, numKids, ZipCode, banned);
        }

        public List<Client> getClientVisits(int clientID)
        {

            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;



            List<Client> clients = context.getClientVisits(clientID);

            return clients;
        }

        public List<Client> getClientByID(int clientID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

           List<Client> clients = context.getClientByID(clientID);

            return clients;
        }



        public Visit getVisitByID(int visitID)
        {            
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            Visit Visit = context.getVisitByID(visitID);

            return Visit;
        }

        public string getLatestVisitByClientID(int clientID)
        {

            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            string visitID = context.getLatestVisitByClientID(clientID);

            return visitID;

        }


        public string createClientVisitByID(int clientID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            string result = context.createClientVisitByID(clientID);
            return result;
        }

        public void deleteClientByID(int clientID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            context.deleteClientByID(clientID);
        }

        public void deleteVisitByID(int visitID)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            context.deleteVisitByID(visitID);
        }



        public Visit checkout(int visitID, int mens, int womens, int kids, bool backpack, bool sleepingbag, string request, int financialAid, int diapers, int giftCard, int busTicket, string houseHoldItems)
        {
            UserContext context = HttpContext.RequestServices.GetService(typeof(StFrancisHouse.Models.UserContext)) as UserContext;

            Visit Visit = context.checkout(visitID, mens, womens, kids, backpack, sleepingbag, request, financialAid , diapers, giftCard, busTicket, houseHoldItems);

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
