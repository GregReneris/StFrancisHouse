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

        public int Mens { get; set; }

        public int Womens { get; set; }

        public int Kids { get; set; }

        public Nullable<DateTime> VisitDate { get; set; }

        public Nullable<DateTime> LastBackpack { get; set; }

        public Nullable<DateTime> LastSleepingBag { get; set; }

        public double busTicket { get; set; }
        public double diapers { get; set; }
        public double financialAid { get; set; }
        public double giftCard { get; set; }


        public String Request { get; set; }


        //TODO adjust model to fit the front end.

    }
}
