using System.ComponentModel.DataAnnotations;

namespace StFrancisHouse.Models
{

    /// <summary>
    /// Visit is the model that holds the information displayed in the visit
    /// It contains relevant dates as well as requests.
    /// </summary>
    public class Visit
    {
        public int VisitID { get; set; }
        
        public int ClientID { get; set; }

        public DateTime VisitDate { get; set; }

        public DateTime LastBackpack { get; set; }

        public DateTime LastSleepingBag { get; set; }

        public String Request { get; set; }


    }
}
