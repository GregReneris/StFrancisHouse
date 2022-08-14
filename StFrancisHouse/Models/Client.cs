using System.ComponentModel.DataAnnotations;

namespace StFrancisHouse.Models
{
    public class Client
    {
        public int ClientID { get; set; } = default!;

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string Birthday { get; set; }

        public int ZipCode { get; set; }

        public string Race { get; set; }

        public string Gender { get; set; }

        public string ClientNote { get; set; }

        public bool Banned { get; set; }
        
        public int LastVisitID { get; set; }

        public Nullable<DateTime> LastVisitDate { get; set; }

        public Nullable<DateTime> MostRecentBackpack { get; set; }
        public Nullable<DateTime> MostRecentSleepingBag { get; set; }

        public List<Visit> Visits { get; set; }



    }
}