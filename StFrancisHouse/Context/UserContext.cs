﻿using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace StFrancisHouse.Models
{
    public class UserContext : DbContext
    {


        //TODO: Add SQL encoder function or param parsing to ensure security and SQL consistency.

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

        public T CheckNull<T>(object obj)
        {
            return (obj == DBNull.Value ? default(T) : (T)obj);
        }


        //example method to get all Clients from Client Table on DB.
        //Note: May be out of date to modified/newest DB Schema based on sqlver1.0, no updates. 
        //Note: possible null assignments for fields. 
        //Returns ALL clients.
        public List<Client> getAllClients()
        {
            List<Client> clients = new List<Client>();
            //int numEntry = 10000; //change this to user chosen value in production later.  

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                //MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE ClientID < " + numEntry , conn);
                
                MySqlCommand cmd = new MySqlCommand("SELECT * from Client", conn);

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
            //string insertLastName = "'" + lastName + "'";
            //string insertBirthdate = "'" + birthdate + "'";

            //example of required formatting.
            //string lastname = "Fort";
            //string firstname = "";
            //string birthdate = "1935-12-23";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();


                string sqlcmd = "SELECT * from Client";
                string seperator = " WHERE ";

                if (firstName.Length > 0)
                {
                    sqlcmd += seperator + "FirstName = '" + firstName + "' ";
                    seperator = " AND ";
                }

                if (lastName.Length > 0)
                {
                    sqlcmd += seperator + "LastName = '" + lastName + "' ";
                    seperator = " AND ";
                }

                if (birthdate.Length > 0)
                {
                    sqlcmd += seperator + "BIRTHDAY = '" + birthdate + "' ";
                    seperator = " AND ";
                }


                MySqlCommand cmd = new MySqlCommand(sqlcmd, conn);

                
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

        public List<Client> getClientByInfo(string firstName)
        {
            List<Client> clients = new List<Client>();
            int numEntry = 50; //change this to user chosen value in production later.  

            //adjusted formatting for easier cmd string.
            string insertFirstName = "'" + firstName + "'";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();



                MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE FirstName LIKE "+ insertFirstName +"%", conn);


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


        public List<Client> getClientVisits(int clientID)
        {
            List<Visit> clientsVisits = new List<Visit>();
         
            List<Client> clients = new List<Client>();

            //adjusted formatting for easier cmd string.
            //string insertLastName = "'" + lastName + "'";
            //string insertBirthdate = "'" + birthdate + "'";

            //example of required formatting.
            //string lastname = "Fort";
            //string firstname = "";
            //string birthdate = "1935-12-23";



            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE clientID = " + clientID, conn);

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


                //MySqlCommand cmd2 = new MySqlCommand("SELECT client.ClientID, client.FirstName, client.LastName, visit.Date, visit.LastBackpack, visit.LastSleepingBag from client, visit WHERE client.ClientID =" + clientID , conn);
                MySqlCommand cmd2 = new MySqlCommand("SELECT * from visit WHERE ClientID =" + clientID , conn);
                
                //string timeHolderBackpack;
                //DateTime timeHolderSleepingBag;

                using (var reader = cmd2.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    
                    
                    while (reader.Read())
                    {
                    

                     //   while (reader.Read())
                     //   {
                     //       tblBPN_InTrRecon Bpn = new tblBPN_InTrRecon();
                     //       Bpn.BPN_Date = CheckNull<DateTime?>(dr["BPN_Date"]);
                     //       Bpn.Cust_Backorder_Qty = CheckNull<int?>(dr["Cust_Backorder_Qty"]);
                     //       Bpn.Cust_Min = CheckNull<int?>(dr["Cust_Min"]);
                     //   }


                        clientsVisits.Add(new Visit()
                        {
                            VisitID = Convert.ToInt32(reader["VisitID"]),
                            ClientID = Convert.ToInt32(reader["ClientID"]),
                            Mens =
                            Womens = 
                            Kids =  
                            VisitDate = (DateTime)reader["Date"],
                            LastBackpack = CheckNull<DateTime>(reader["LastBackpack"]),
                            LastSleepingBag = CheckNull<DateTime>(reader["LastSleepingBag"]),
                            Request = CheckNull<String>(reader["Request"].ToString())


                            //if ((DateTime)reader["LastBackpack"] == null)
                            //{
                            //  clientsVisits.LastBackpack = backSleepDefault;
                            //}

                            //LastBackpack = (DateTime)reader["LastBackpack"] != null ? value : backSleepDefault,
                            //LastBackpack = (DateTime)reader["LastBackpack"] ?? "default_value";
                            //LastBackpack = (DateTime)timeHolderBackpack
                            //LastSleepingBag = (DateTime)reader["LastSleepingBag"]
                            //Lasts can be null, so need to find a way around that exception.
                        });
                    }
                }

                clients[0].Visits = clientsVisits;

            }

            return clients; //returns the client list.

        }


        /**
         * 
         * Requires client ID to add a visit to.
         */
        public void createClientVisitByID(int clientID)
        {

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO VISIT (ClientID, Date) values (" + clientID +", NOW()) " , conn);

                int result = cmd.ExecuteNonQuery(); //this returns how many rows were effected

                //consider returning the visit or at least the visitID


                Console.WriteLine(); //WriteLine for break point usage
            }
        }

        public Visit getVisitByID(int visitID)
        {
            Visit clientVisit = new Visit();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM VISIT WHERE visitID =" + visitID , conn);

                using (var reader = cmd.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    while (reader.Read())
                    {
                        
                            clientVisit.VisitID = Convert.ToInt32(reader["VisitID"]);
                            clientVisit.ClientID = Convert.ToInt32(reader["ClientID"]);
                            clientVisit.VisitDate = (DateTime)reader["Date"];
                            //LastBackpack = (DateTime)reader["LastBackpack"],
                            //LastBackpack = (DateTime)timeHolderBackpack
                            //LastSleepingBag = (DateTime)reader["LastSleepingBag"]
                            //Lasts can be null, so need to find a way around that exception.
                        
                    }
                }
            }


            return clientVisit;
        }


     public Visit checkout(int visitID, int mens, int womens, int kids, bool backpack, bool sleepingbag, string request)
        {
            Visit clientVisit = new Visit();

            int insertMens = mens;
            int insertWomens = womens;
            int insertKids = kids;
            string backpackInsert = "Current_Date()";
            string sleepInsert = "Current_Date()";
            string requestInsert = "'" + request + "'";


            if (backpack == false)
            {
                backpackInsert = "1111-11-11";
            }
            if(sleepingbag == false)
            {
                sleepInsert = "1111-11-11";
            }



            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                
                MySqlCommand cmd = new MySqlCommand("UPDATE VISIT SET Mens = '"+ insertMens +"' , Womens = '"+ insertWomens +"', Kids = '"+ insertKids +"', LastBackpack = " +backpackInsert+ ", LastSleepingBag = "+ sleepInsert +", Request = "+ requestInsert +" WHERE visitID =" + visitID, conn);

                cmd.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM VISIT WHERE visitID =" + visitID, conn);


                using (var reader = cmd2.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    while (reader.Read())
                    {

                        clientVisit.VisitID = Convert.ToInt32(reader["VisitID"]);
                        clientVisit.ClientID = Convert.ToInt32(reader["ClientID"]);
                        clientVisit.VisitDate = (DateTime)reader["Date"];
                        //LastBackpack = (DateTime)reader["LastBackpack"],
                        //LastBackpack = (DateTime)timeHolderBackpack
                        //LastSleepingBag = (DateTime)reader["LastSleepingBag"]
                        //Lasts can be null, so need to find a way around that exception.

                    }
                }
            }

            return clientVisit;

        }



        /*
         * 
         * Helper function section!
         * 
         */
        public object checkForNull(object input)
        {
            DateTime fixedDateTime = DateTime.MinValue;
            //object example = new DBNull();

            if(input == null)
            {
                input = fixedDateTime.ToString();
            }

            return input;
        }

        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                //if(obj.GetType == DBNull.Value)
                //{
                //      return DateTime.minValue or something.
                //}
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
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
