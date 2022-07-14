using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Net;
using System;
using StFrancisHouse.Models;
using Microsoft.AspNetCore.Hosting;

namespace StFrancisHouse.Services


{
    public class JsonFileClientService
    {


        /// Initiate the web hosting environment for the application to use
        public JsonFileClientService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        /// Call the IWebHostEnvironment object
        public IWebHostEnvironment WebHostEnvironment { get; }


    }

   
}
