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

        public List<Client> getClientByInfo(string firstName, string lastName, string birthdate)
        {
            List<Client> clients = new List<Client>();
            int numEntry = 50; //change this to user chosen value in production later.  

            //adjusted formatting for easier cmd string.
            string insertLastName = "'" + lastName + "'";
            string insertBirthdate = "'" + birthdate + "'";

            //example of required formatting.
            //string lastname = "Fort";
            //string firstname = "";
            //string birthdate = "1935-12-23";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();


                
                MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE LastName = " + insertLastName + " AND BIRTHDAY = " + insertBirthdate, conn);

                
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


        //public List<Client> createNewClient(string firstName, string lastName, string middleInitial, string suffix, string birthdate, string race, string gender, int ZipCode)
        public void createNewClient(string firstName, string lastName, string middleInitial, string suffix, string birthdate, string race, string gender, int ZipCode)
        {

            List<Client> clients = new List<Client>();
            int numEntry = 2; //change this to user chosen value in production later.  
            
            string insertFirstName = "'" + firstName + "'";
            string insertLastName = "'" + lastName + "'";
            string insertMiddleInitial = "'" + middleInitial + "'"; 
            string insertSuffix = "'" + suffix + "'";
            string insertBirthdate = "'" + birthdate + "'";
            string insertRace = "'" + race + "'";
            string insertGender = "'" + gender + "'";
            string insertZip = "" + ZipCode + "";
            

            string sqlFormattedValueString = insertFirstName +", " + insertLastName + ", " + insertMiddleInitial + ", " + insertSuffix +", " + insertBirthdate + ", " +insertZip + ", " +
                insertRace + ", " + insertGender;

            Console.WriteLine(sqlFormattedValueString);
            //define client variables.

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                //Create new client
                MySqlCommand cmd = new MySqlCommand("INSERT INTO client(FirstName, LastName, MI, SUFFIX, Birthday, `Zip Code`, Race, Gender) VALUES ( " + sqlFormattedValueString + ")" , conn);
                Console.WriteLine(cmd.ToString());

                int result = cmd.ExecuteNonQuery();
                
                //INSERT INTO client(FirstName, LastName, MI, SUFFIX, Birthday, `Zip Code`, Race, Gender) VALUES("Ken", "Hamilton", "B", "MRS", '1935-12-23', 98344, "N/A", "M");

                //Retrieve this client
                //MySqlCommand cmd2 = new MySqlCommand("SELECT * from Client WHERE LastName = " + insertLastName + " AND BIRTHDAY = " + insertBirthdate, conn);


            }

            Console.WriteLine("End of method.");
            //  return clients;

        }


        public List<Client> getClientVisits(string firstName, string lastName, string birthdate)
        {
         
            List<Client> clients = new List<Client>();
            List<Visit> clientsVisits = new List<Visit>();

            //adjusted formatting for easier cmd string.
            string insertLastName = "'" + lastName + "'";
            string insertBirthdate = "'" + birthdate + "'";

            //example of required formatting.
            //string lastname = "Fort";
            //string firstname = "";
            //string birthdate = "1935-12-23";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();



                MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE LastName = " + insertLastName + " AND BIRTHDAY = " + insertBirthdate, conn);

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


                int clientID = clients[0].ClientID;

                //MySqlCommand cmd2 = new MySqlCommand("SELECT client.ClientID, client.FirstName, client.LastName, visit.Date, visit.LastBackpack, visit.LastSleepingBag from client, visit WHERE client.ClientID =" + clientID , conn);
                MySqlCommand cmd2 = new MySqlCommand("SELECT * from visit WHERE ClientID =" + clientID , conn);

                using (var reader = cmd2.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    while (reader.Read())
                    {
                        clientsVisits.Add(new Visit()
                        {
                            VisitID = Convert.ToInt32(reader["VisitID"]),
                            ClientID = Convert.ToInt32(reader["ClientID"]),
                            VisitDate = (DateTime)reader["Date"],
                            //LastBackpack = (DateTime)reader["LastBackpack"],
                            //LastSleepingBag = (DateTime)reader["LastSleepingBag"]
                            //Lasts can be null, so need to find a way around that exception.
                        });
                    }
                }

                clients[0].Visits = clientsVisits;

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
