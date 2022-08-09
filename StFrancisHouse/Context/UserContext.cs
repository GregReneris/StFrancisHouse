using MySql.Data.MySqlClient;
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

        public static int ToInt32(object obj, int value = 0)
        {
            return (obj == DBNull.Value ? value : Convert.ToInt32(obj));
        }
        
        public static Nullable<DateTime> ToDateTime(object obj, Nullable<DateTime> value = null)
        {
            return (obj == DBNull.Value ? value : Convert.ToDateTime(obj));
        }

        public static string ToString(object obj, string value = "none")
        {
            return (obj == DBNull.Value ? value : value.ToString());
        }

        public static bool BoolCheck(object obj, bool value = false)
        {
            return (obj == DBNull.Value ? value : value);
        }

        public static bool varChar1ToBool(string input)
        {
            if (input.Equals("0"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static List<Client> addClientsToList(MySqlCommand cmd, List<Client> clients)
        {
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
                        Gender = reader["Gender"].ToString(),
                        ClientNote = reader["ClientNote"].ToString(),
                        Banned = varChar1ToBool(reader["Banned"].ToString())
                        //note sure: add latest visitID.

                    });
                }
            }

            return clients;
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
                            Gender = reader["Gender"].ToString(),
                            ClientNote = reader["ClientNote"].ToString()
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

                if (firstName != null)
                {
                    sqlcmd += seperator + "FirstName = '" + firstName + "' ";
                    seperator = " AND ";
                }

                //if (CheckNull<string>(lastName).Length > 0)
                
                if (lastName != null)
                {
                    sqlcmd += seperator + "LastName = '" + lastName + "' ";
                    seperator = " AND ";
                }

                if (birthdate != null)
                {
                    sqlcmd += seperator + "BIRTHDAY = '" + birthdate + "' ";
                    seperator = " AND ";
                }


                MySqlCommand cmd = new MySqlCommand(sqlcmd, conn);


                //MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE LastName = " + lastname + " AND BIRTHDAY = " + birthdate , conn);

                clients = addClientsToList(cmd, clients);

                //using (var reader = cmd.ExecuteReader())
                //{
                //    //adding information MUST reflect the exact table id inside the [" "]
                //    //whereas the assignments must match the model data. 
                //    while (reader.Read())
                //    {
                //        clients.Add(new Client()
                //        {
                //            ClientID = Convert.ToInt32(reader["ClientID"]),
                //            FirstName = reader["FirstName"].ToString(),
                //            LastName = reader["LastName"].ToString(),
                //            MiddleInitial = reader["MI"].ToString(),
                //            Birthday = reader["Birthday"].ToString(),
                //            ZipCode = Convert.ToInt32(reader["Zip Code"]),
                //            Race = reader["Race"].ToString(),
                //            Gender = reader["Gender"].ToString(),
                //            ClientNote = reader["ClientNote"].ToString(),
                //            Banned = varChar1ToBool(reader["Banned"].ToString())
                //            //note sure: add latest visitID.
                //
                //        });
                //    }
                //}

            }
            return clients; //returns the client list.
        }



     //   public List<Client> getClientByInfo(string firstName)
     //   {
     //       List<Client> clients = new List<Client>();
     //       int numEntry = 50; //change this to user chosen value in production later.  
     //
     //       //adjusted formatting for easier cmd string.
     //       string insertFirstName = "'" + firstName + "'";
     //
     //       using (MySqlConnection conn = GetConnection())
     //       {
     //           conn.Open();
     //
     //
     //
     //           MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE FirstName LIKE "+ insertFirstName +"%", conn);
     //
     //
     //           //MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE LastName = " + lastname + " AND BIRTHDAY = " + birthdate , conn);
     //
     //
     //           using (var reader = cmd.ExecuteReader())
     //           {
     //               //adding information MUST reflect the exact table id inside the [" "]
     //               //whereas the assignments must match the model data. 
     //               while (reader.Read())
     //               {
     //                   clients.Add(new Client()
     //                   {
     //                       ClientID = Convert.ToInt32(reader["ClientID"]),
     //                       FirstName = reader["FirstName"].ToString(),
     //                       LastName = reader["LastName"].ToString(),
     //                       MiddleInitial = reader["MI"].ToString(),
     //                       Birthday = reader["Birthday"].ToString(),
     //                       ZipCode = Convert.ToInt32(reader["Zip Code"]),
     //                       Race = reader["Race"].ToString(),
     //                       Gender = reader["Gender"].ToString(),
     //                       ClientNote = reader["ClientNote"].ToString()
     //                   });
     //               }
     //           }
     //
     //       }
     //       return clients; //returns the client list.
     //   }


        //public List<Client> createNewClient(string firstName, string lastName, string middleInitial, string suffix, string birthdate, string race, string gender, int ZipCode)
        public void createNewClient(string firstName, string lastName, string middleInitial, string suffix, string birthdate, string race, string gender, int ZipCode)
        {

            //TODO: return the clientID and the latest / today's new visitID

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
                            Gender = reader["Gender"].ToString(),
                            ClientNote = reader["ClientNote"].ToString()
                        });
                    }
                }


                //MySqlCommand cmd2 = new MySqlCommand("SELECT client.ClientID, client.FirstName, client.LastName, visit.Date, visit.LastBackpack, visit.LastSleepingBag from client, visit WHERE client.ClientID =" + clientID , conn);
                MySqlCommand cmd2 = new MySqlCommand("SELECT * from visit WHERE ClientID =" + clientID , conn);
                
                //string timeHolderBackpack;
                //Nullable<DateTime> timeHolderSleepingBag;

                using (var reader = cmd2.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    
                    
                    while (reader.Read())
                    {


                        //   while (reader.Read())
                        //   {
                        //       tblBPN_InTrRecon Bpn = new tblBPN_InTrRecon();
                        //       Bpn.BPN_Date = CheckNull<Nullable<DateTime>?>(dr["BPN_Date"]);
                        //       Bpn.Cust_Backorder_Qty = CheckNull<int?>(dr["Cust_Backorder_Qty"]);
                        //       Bpn.Cust_Min = CheckNull<int?>(dr["Cust_Min"]);
                        //   }


                        clientsVisits.Add(new Visit()
                        {
                            VisitID = ToInt32(reader["VisitID"]),
                            ClientID = ToInt32(reader["ClientID"]),
                            Mens = ToInt32(reader["Mens"]),
                            Womens = ToInt32(reader["Womens"]),
                            Kids = ToInt32(reader["Kids"]),
                            VisitDate = ToDateTime(reader["Date"]),
                            LastBackpack = ToDateTime(reader["LastBackpack"]),
                            LastSleepingBag = ToDateTime(reader["LastSleepingBag"]),
                            Request = ToString(reader["Request"])
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
                            clientVisit.VisitDate = ToDateTime(reader["Date"]);
                            clientVisit.LastBackpack = ToDateTime(reader["LastBackpack"]);
                            clientVisit.LastSleepingBag = ToDateTime(reader["LastSleepingBag"]);
                            clientVisit.Request = reader["Request"].ToString();
                    }
                }
            }


            return clientVisit;
        }


     public Visit checkout(int visitID, int mens, int womens, int kids, bool backpack, bool sleepingbag, string request)
        {
            //TODO Add the front end's version parameters in so we line up.

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
                        clientVisit.VisitDate = ToDateTime(reader["Date"]);
                        clientVisit.LastBackpack = ToDateTime(reader["LastBackpack"]);
                        clientVisit.LastSleepingBag = ToDateTime(reader["LastSleepingBag"]);
                        clientVisit.Request = reader["Request"].ToString();
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
       //   public object checkForNull(object input)
       // {
       //     Nullable<DateTime> fixedNullable<DateTime> = Nullable<DateTime>.MinValue;
       //     //object example = new DBNull();
       //
       //     if(input == null)
       //     {
       //         input = fixedNullable<DateTime>.ToString();
       //     }
       //
       //     return input;
       // }
       //
       // public static T ConvertFromDBVal<T>(object obj)
       // {
       //     if (obj == null || obj == DBNull.Value)
       //     {
       //         //if(obj.GetType == DBNull.Value)
       //         //{
       //         //      return Nullable<DateTime>.minValue or something.
       //         //}
       //         return default(T); // returns the default value for the type
       //     }
       //     else
       //     {
       //         return (T)obj;
       //     }
       // }



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
