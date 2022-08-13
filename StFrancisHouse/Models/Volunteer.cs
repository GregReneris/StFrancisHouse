﻿namespace StFrancisHouse.Models
{
    public class Volunteer
    {
        //username for loging in
        public string Username { get; set; }

        //password for logging in as a volunteer
        public string Password { get; set; }

        public int VolunteerID { get; set; }

        public string VolunteerFirstName { get; set; }

        public string VolunteerLastName { get; set; }

        public bool isAdmin { get; set; }

    }
}
