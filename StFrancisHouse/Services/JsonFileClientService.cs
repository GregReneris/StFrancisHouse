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

        /// Get the file path and filename of Client data for loading
        private string JsonFileClientName => Path.Combine(
            WebHostEnvironment.WebRootPath, "data", "Client.json");

        /// Get the json text and convert it to list
        public IEnumerable<Client> GetClient()
        {
            using var jsonFileReader = File.OpenText(JsonFileClientName);
            var clientlist = JsonSerializer.Deserialize<Client[]>
                (jsonFileReader.ReadToEnd(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            // Handle nulliable
            if (clientlist is null) {
                clientlist = Array.Empty<Client>();
            }

            return clientlist;
        }


    }
}
