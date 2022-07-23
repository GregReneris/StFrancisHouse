using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace StFrancisHouse.Models
{
    public class UserContext : DbContext
    {

        //Adding in mySQL connection string to the online database.
        public string ConnectionString { get; set; }

        public UserContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }


        //example method to get all Clients from Client Table on DB.
        //Note: May be out of date to modified/newest DB Schema based on sqlver1.0, no updates. 
        //Note: possible null assignments for fields. 
        public List<Client> getAllClients()
        {
            List<Client> clients = new List<Client>();
            int numEntry = 10; //change this to user chosen value in production later.  

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE ClientID < " + numEntry , conn);

                using (var reader = cmd.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    while (reader.Read())
                    {
                        clients.Add(new Client()
                        {
                            ClientID = Convert.ToInt32(reader["ClientID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            MiddleInitial = reader["MI"].ToString(),
                            //Birthday = reader["Birthday"].ToString(),
                            ZipCode = Convert.ToInt32(reader["Zip Code"]),
                            Race = reader["Race"].ToString(),
                            Gender = reader["Gender"].ToString()
                        });
                    }
                }

            }

            return clients; //returns the client list.
        }

        public List<Client> getClientByInfo()
        {
            List<Client> clients = new List<Client>();
            int numEntry = 50; //change this to user chosen value in production later.  

            string lastname = "Fort";
            string firstname = "";
            string birthdate = "1935-12-23";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                //adjusted formatting for easier cmd string.
                string cmd1 = "'" +lastname+ "'";
                string cmd2 = "'" +birthdate+ "'";
                
                MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE LastName = " + cmd1 + " AND BIRTHDAY = " + cmd2 , conn);

                
                //MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE LastName = " + lastname + " AND BIRTHDAY = " + birthdate , conn);


                using (var reader = cmd.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    while (reader.Read())
                    {
                        clients.Add(new Client()
                        {
                            ClientID = Convert.ToInt32(reader["ClientID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            MiddleInitial = reader["MI"].ToString(),
                            Birthday = reader["Birthday"].ToString(),
                            ZipCode = Convert.ToInt32(reader["Zip Code"]),
                            Race = reader["Race"].ToString(),
                            Gender = reader["Gender"].ToString()
                        });
                    }
                }

            }

            return clients; //returns the client list.
        }




        //Greg: Not sure if the below needs to be commented out since conectionstring above has been used. 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=stfrancis;user=root;password=root238!");
        }
        public UserContext(DbContextOptions<UserContext> options)
            :base(options)
        {
        }

        //commented the below lone of code since it was not letting the rest of the code to compile
        //public DbSet<User> UserItems { get; set; } = null!;
    }
}
