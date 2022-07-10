using System.ComponentModel.DataAnnotations;

namespace StFrancisHouse.Models
{
    public class Family
    {
        public int ID { get; set; }

        public int NumKids { get; set; }

        public int BoyAge { get; set; }

        public int GirlAge { get; set; }

    }
}

