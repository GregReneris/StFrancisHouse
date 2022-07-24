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
        
        public string VisitDate { get; set; }

        public string LastBackpage { get; set; }

        public string LastSleepingBag { get; set; }

        public string Requests { get; set; }


    }
}
