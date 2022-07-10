using System.ComponentModel.DataAnnotations;

namespace StFrancisHouse.Models
{
    public class Client
    {
        public string ClientID { get; set; } = default!;

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string Birthday { get; set; }

        public int ZipCode { get; set; }

        public string Race { get; set; }

        public string Gender { get; set; }

    }
}