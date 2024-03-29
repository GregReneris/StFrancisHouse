﻿using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace StFrancisHouse.Models
{
    public class UserContext : DbContext
    {

        
        //These two values represent the production clients and their visits (assistances in DB).
        static string Ctable = "clients";
        static string Vtable = "assistances";
       
        
        //these two values represent the development tables for clients and their visits. 
        //static string Vtable = "visits";
        //static string Ctable = "client"; 

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


        #region Helper Functions
        /**
         * Helper function section!
         * 
         */
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
            //return (obj == DBNull.Value ? value : value.ToString());
            if (obj == DBNull.Value)
                return value;
            else
            {
                return obj.ToString();

            }

        }

        public static bool BoolCheck(object obj, bool value = false)
        {
            return (obj == DBNull.Value ? value : value);
        }        
        
        public static int BoolCheckAsVarChar1(object obj, int value = 0)
        {
            if (obj == DBNull.Value)
                return 0;
            else
            {
                return ToInt32(obj);
            }

            //return (obj == DBNull.Value ? value : value);
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

        public static double DoubleCheck(object obj, double value = 0)
        {
            if (obj == DBNull.Value)
            {
                return value;
            }
            else
            {
                return Convert.ToDouble(obj); 
            }
        }

        /**
         * 
         * Banned is problematic since it is stored as a varchar(1) in the DB.
         * This was to try and avoid null fields, which happened when the new DB was imported.
         */
        public static List<Client> addClientsToList(MySqlCommand cmd, List<Client> clients)
        {
            using (var reader = cmd.ExecuteReader())
            {
                //adding information MUST reflect the exact table id inside the [" "]
                //whereas the assignments must match the model data. 
                while (reader.Read())
                {

                    if(reader.HasRows == true)
                    {
                        Console.WriteLine("Here and exists");
                    }

                    clients.Add(new Client()
                    {
                        //Adding ToString(obj) around the strings to handle dbnull errors
                        ClientID = Convert.ToInt32(reader["ClientID"]),
                        FirstName = ToString(reader["FirstName"]).ToString(),
                        LastName = ToString(reader["LastName"]).ToString(),
                        MiddleInitial = ToString(reader["MI"]).ToString(),
                        Birthday = ToDateTime(reader["Birthday"]).ToString(),
                        
                        
                        ZipCode = Convert.ToInt32(reader["Zip Code"]),
                        
                        
                        Race = ToString(reader["Race"]).ToString(),
                        Gender = ToString(reader["Gender"]).ToString(),
                        
                        
                        numFamily = ToInt32(reader["NumKids"]), //maybe should be a doubleCheck() instead?

                        //ClientNote = reader["ClientNote"].ToString(),
                        ClientNote = ToString(reader["Note"]).ToString(),
                        //Banned = BoolCheck(varChar1ToBool(reader["Banned"].ToString())) //changed to check for null as well. 
                        Banned = varChar1ToBool(BoolCheckAsVarChar1((reader["Banned"])).ToString()) //probably a bonkers way to handle this.
                        
                        
                        //not sure: add latest visitID.

                    });
                }
            }

            return clients;
        }

        #endregion

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
                
                MySqlCommand cmd = new MySqlCommand("SELECT * from " + Ctable, conn);

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
                            ClientNote = reader["Note"].ToString()
                        });
                    }
                }

            }

            return clients; //returns the client list.
        }

        public List<Volunteer> getAllVolunteers()
        {
            List<Volunteer> Volunteers = new List<Volunteer>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * from volunteer", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    while (reader.Read())
                    {
                        Volunteers.Add(new Volunteer()
                        {
                            VolunteerID = Convert.ToInt32(reader["VolunteerID"]),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            VolunteerFirstName = reader["firstName"].ToString(),
                            VolunteerLastName = reader["lastName"].ToString(),
                            isAdmin = varChar1ToBool(reader["isAdmin"].ToString())
                        });
                    }
                }

                return Volunteers;
            }
        }

        public List<Client> getClientByInfo(string firstName, string lastName, string birthdate)
        {
            List<Client> clients = new List<Client>();
            //int numEntry = 50; //change this to user chosen value in production later.  
            List<Visit> clientsVisits = new List<Visit>();
            int numVisits = 1;
            int c0ID;


            //TODO: IF ALL NULL PARAMS
            if(firstName == null && lastName == null && birthdate == null)
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();

                    string sqlcmdNulls = "SELECT * from " + Ctable;

                    MySqlCommand cmd10 = new MySqlCommand(sqlcmdNulls, conn);

                    clients = addClientsToList(cmd10, clients);

                    return clients;

                }
            }





            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();


                string sqlcmd = "SELECT * from " + Ctable;
                string seperator = " WHERE ";

                //   %input% searches for any values that have "input" in any position, for example. %le% could return Alex, Leopold, or Charles. An exact match is of course best. 

                if (firstName != null)
                {
                    //sqlcmd += seperator + "FirstName = '" + firstName + "' ";
                    sqlcmd += seperator + "FirstName LIKE '%" + firstName + "%' ";
                    seperator = " AND ";
                }

                //if (CheckNull<string>(lastName).Length > 0)
                
                if (lastName != null)
                {
                    //sqlcmd += seperator + "LastName = '" + lastName + "' ";
                    sqlcmd += seperator + "LastName LIKE '%" + lastName + "%' ";
                    seperator = " AND ";
                }

                if (birthdate != null)
                {
                    sqlcmd += seperator + "BIRTHDAY = '" + birthdate + "' ";
                    seperator = " AND ";
                }

                // adding limit to the number of responses to avoid hitting the query count of 18,000 because this method is multiplicative. 
                sqlcmd += " LIMIT 100";

                MySqlCommand cmd = new MySqlCommand(sqlcmd, conn);


                //MySqlCommand cmd = new MySqlCommand("SELECT * from Client WHERE LastName = " + lastname + " AND BIRTHDAY = " + birthdate , conn);

                clients = addClientsToList(cmd, clients);


                //can only get visits if client exists, so check to make sure there is a client entry.
                int counter = 0;

                if (clients.Count > 0)
                {
                    while (counter < clients.Count)
                    {
                        List<Visit> clientsVisits2 = new List<Visit>();

                        int clientID = clients[counter].ClientID;

                        //MySqlCommand cmd2 = new MySqlCommand("SELECT * from visit WHERE ClientID =" + clientID + " ORDER BY Date DESC LIMIT " +numVisits, conn);
                        MySqlCommand cmd2 = new MySqlCommand("SELECT * from "+ Vtable +" WHERE ClientID =" + clientID + " ORDER BY Date DESC LIMIT 1", conn);

                        //SELECT * from visit
                        //ORDER BY stu_date DESC
                        //WHERE ClientID = ;

                        //string timeHolderBackpack;
                        //Nullable<DateTime> timeHolderSleepingBag;


                        using (var reader = cmd2.ExecuteReader())
                        {
                            //adding information MUST reflect the exact table id inside the [" "]
                            //whereas the assignments must match the model data. 

                            if (reader.HasRows == true)
                            {




                                while (reader.Read())
                                {



                                    clientsVisits2.Add(new Visit()
                                    {
                                        VisitID = ToInt32(reader["VisitID"]),
                                        ClientID = ToInt32(reader["ClientID"]),
                                        Mens = ToInt32(reader["Mens"]),
                                        Womens = ToInt32(reader["Womens"]),
                                        Kids = ToInt32(reader["Kids"]),
                                        VisitDate = ToDateTime(reader["Date"]),

                                        LastBackpack = ToDateTime(reader["LastBackPackDate"]),
                                        LastSleepingBag = ToDateTime(reader["LastSleepingBagDate"]),

                                        busTicket = DoubleCheck((reader["busTicket"])),
                                        diapers = DoubleCheck((reader["diapers"])),
                                        financialAid = DoubleCheck((reader["financialAid"])),
                                        giftCard = DoubleCheck((reader["giftCard"])),
                                        HouseHoldItems = ToString(reader["HouseHoldItems"]),
                                        Request = ToString(reader["Request"])
                                    });
                                }
                            }
                        }

                        clients[counter].Visits = clientsVisits2; 

                        /*
                         * NEW STUFF 8/22
                        */
                        c0ID = clients[counter].ClientID;

                        MySqlCommand cmd3 = new MySqlCommand("SELECT ClientID, MAX(Date) AS Date, MAX(VisitID) AS VisitID, MAX(LastBackPackDate) AS LastBackPackDate, MAX(LastSleepingBagDate) AS LastSleepingBagDate FROM assistances a WHERE ClientID = " + c0ID + " GROUP BY ClientID", conn);

                        using (var reader = cmd3.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clients[counter].MostRecentBackpack = ToDateTime(reader["LastBackPackDate"]);
                                clients[counter].LastVisitID = ToInt32(reader["VisitID"]);
                                clients[counter].MostRecentSleepingBag = ToDateTime(reader["LastSleepingBagDate"]);
                                clients[counter].LastVisitDate = ToDateTime(reader["Date"]);
                                
                            }
                        }
                        /*
                         * end new stuff 8/22
                         */

                        counter++;
                    }
                }

            }

            return clients; //returns the client list.
        }


        public void updateClientByID(int clientID, string firstName, string lastName, string middleInitial, string birthdate, string race, string gender, int numKids, int ZipCode, bool Banned)
        {
            string b_result;
            if(Banned == true)
            {
                b_result = "1";
            }
            else
            {
                b_result = "0";
            }

            //TODO: UPDATE WITH LATEST FIELDS


            string insertFirstName = "'" + firstName + "'";
            string insertLastName = "'" + lastName + "'";
            string insertMiddleInitial = "'" + middleInitial + "'";
            //string insertSuffix = "'" + suffix + "'";
            string insertBirthdate = "'" + birthdate + "'";
            string insertRace = "'" + race + "'";
            string insertGender = "'" + gender + "'";
            string insertBool = "'" + b_result + "'";
            string insertZip = "" + ZipCode + "";
            string insertNumKids = "" + numKids + "";




            string sqlcmd = "Update " + Ctable + " SET ";
            string sqlcmdEnd = " Banned = " + insertBool + " WHERE ClientID = " + clientID;
            string updateCmd;



            string seperator = ",";

            if (firstName != null)
            {
                sqlcmd += " FirstName = " + insertFirstName + " " + seperator;
            }
        
            if (lastName != null)
            {
                sqlcmd += " LastName = " + insertLastName + " " + seperator;
            }            
            
            if(middleInitial != null)
            {
                sqlcmd += " MI = " + insertMiddleInitial + " " + seperator;
            }

            if (birthdate != null)
            {
                sqlcmd += " Birthday = " + insertBirthdate + " " + seperator;
            }

            if(numKids != 0)
            {
                sqlcmd += " NumKids = " + insertNumKids + " " + seperator;
            }
        
            if (race != null)
            {
                sqlcmd += " MI = " + insertMiddleInitial + " " + seperator;
            }

            if (gender != null)
            {
                sqlcmd += " Gender = " + insertGender + " " + seperator;
            }

            //CHECK THIS 
            if(ZipCode != 0)
            {
                sqlcmd += "`Zip Code` = " + insertZip + " " + seperator; 
            }

            updateCmd = sqlcmd + sqlcmdEnd;
            
            //Console.WriteLine(updateCmd);


            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                //Update existing client
                //MySqlCommand cmd = new MySqlCommand("Update client(FirstName, LastName, MI, SUFFIX, Birthday, `Zip Code`, Race, Gender) VALUES ( " + sqlFormattedValueString + ") WHERE CLIENTID = " + clientID, conn);
                //MySqlCommand cmd = new MySqlCommand("Update "+ Ctable +" SET FirstName = "+ insertFirstName +", LastName = "+ insertLastName +", MI = "+ insertMiddleInitial +", Birthday ="+ insertBirthdate +", `Zip Code` = "+ insertZip+", Race = "+ insertRace +" , Gender = "+insertGender+", Banned =" + insertBool + " WHERE ClientID = " + clientID, conn);

                //new adjustable update command

                MySqlCommand cmd = new MySqlCommand(updateCmd, conn);
                
                Console.WriteLine(cmd.ToString());

                int result = cmd.ExecuteNonQuery();

                //INSERT INTO client(FirstName, LastName, MI, SUFFIX, Birthday, `Zip Code`, Race, Gender) VALUES("Ken", "Hamilton", "B", "MRS", '1935-12-23', 98344, "N/A", "M");



            }

        }


        public void deleteClientByID(int clientID)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM " + Ctable + " WHERE ClientID = " + clientID, conn);

                int result = cmd.ExecuteNonQuery(); //this returns how many rows were effected

            }
        }

        public void deleteVisitByID(int visitID)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM " + Vtable + " WHERE VisitID = " + visitID, conn);

                int result = cmd.ExecuteNonQuery(); //this returns how many rows were effected

            }
        }

        public List<Client> getClientByID(int clientID)
        {

            List<Client> clients = new List<Client>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + Ctable + " WHERE ClientID = " + clientID, conn);

                clients = addClientsToList(cmd, clients);
            }

            return clients;

        }





        //public List<Client> createNewClient(string firstName, string lastName, string middleInitial, string suffix, string birthdate, string race, string gender, int ZipCode)
        public List<Client> createNewClient(string firstName, string lastName, string middleInitial, string suffix, string birthdate, string race, string gender, int numKids, int ZipCode)
        {

            //TODO: return the clientID and the latest / today's new visitID

            List<Client> clients = new List<Client>();
            long lastReturned;
            int numEntry = 2; //change this to user chosen value in production later.  
            
            string insertFirstName = "'" + firstName + "'";
            string insertLastName = "'" + lastName + "'";
            string insertMiddleInitial = "'" + middleInitial + "'"; 
            string insertSuffix = "'" + suffix + "'";
            string insertBirthdate = "'" + birthdate + "'";
            string insertRace = "'" + race + "'";
            string insertGender = "'" + gender + "'";
            string insertZip = "" + ZipCode + "";
            string insertNumKids = "'" + numKids + "'";


            string sqlFormattedValueString = insertFirstName +", " + insertLastName + ", " + insertMiddleInitial + ", " + insertBirthdate + ", " +insertZip + ", " +
                insertRace + ", " + insertGender + ", " + insertNumKids;

            Console.WriteLine(sqlFormattedValueString);
            //define client variables.

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                //Create new client
                MySqlCommand cmd = new MySqlCommand("INSERT INTO "+ Ctable +"(FirstName, LastName, MI, Birthday, `Zip Code`, Race, Gender, NumKids) VALUES ( " + sqlFormattedValueString + ")" , conn);
                Console.WriteLine(cmd.ToString());

                int result = cmd.ExecuteNonQuery();


            }

            clients = getClientByInfo(firstName, lastName, birthdate);


            return clients;

        }


        public List<Client> getClientVisits(int clientID)
        {
            List<Visit> clientsVisits = new List<Visit>();
         
            List<Client> clients = new List<Client>();


            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * from " + Ctable + " WHERE clientID = " + clientID, conn);

                clients = addClientsToList(cmd, clients);


                //MySqlCommand cmd2 = new MySqlCommand("SELECT client.ClientID, client.FirstName, client.LastName, visit.Date, visit.LastBackpack, visit.LastSleepingBag from client, visit WHERE client.ClientID =" + clientID , conn);
                MySqlCommand cmd2 = new MySqlCommand("SELECT * from " + Vtable + " WHERE ClientID =" + clientID , conn);

                //SELECT * from visit
                //ORDER BY stu_date DESC
                //WHERE ClientID = ;

                //string timeHolderBackpack;
                //Nullable<DateTime> timeHolderSleepingBag;


                using (var reader = cmd2.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    
                    
                    while (reader.Read())
                    {



                        clientsVisits.Add(new Visit()
                        {
                            VisitID = ToInt32(reader["VisitID"]),
                            ClientID = ToInt32(reader["ClientID"]),
                            Mens = ToInt32(reader["Mens"]),
                            Womens = ToInt32(reader["Womens"]),
                            Kids = ToInt32(reader["Kids"]),
                            VisitDate = ToDateTime(reader["Date"]),
                            LastBackpack = ToDateTime(reader["LastBackPackDate"]),
                            LastSleepingBag = ToDateTime(reader["LastSleepingBagDate"]),

                            busTicket = DoubleCheck((reader["busTicket"])),
                            diapers = DoubleCheck((reader["diapers"])),
                            financialAid = DoubleCheck((reader["financialAid"])),
                            giftCard = DoubleCheck((reader["giftCard"])),
                            HouseHoldItems = ToString(reader["HouseHoldItems"]),
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
        public string createClientVisitByID(int clientID)
        {
            object result2;
            string result3;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO " + Vtable + "(ClientID, Date) values (" + clientID +", NOW()) " , conn);
                //MySqlCommand cmd = new MySqlCommand("INSERT INTO VISIT (ClientID, Date) values (" + clientID +", NOW()) " , conn);

               
                int result = cmd.ExecuteNonQuery(); //this returns how many rows were effected



                MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM " + Vtable + " WHERE ClientID = " + clientID + " GROUP BY VisitID DESC", conn);

                result2 = cmd2.ExecuteScalar();
                //consider returning the visit or at least the visitID
                result3 = result2.ToString();
                

                Console.WriteLine(result2); //WriteLine for break point usage
            }

            return result3;
        }

        public string getLatestVisitByClientID(int clientID)
        {
            object result2;
            string result3;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM " + Vtable + " WHERE ClientID = " + clientID + " GROUP BY VisitID DESC", conn);

                result2 = cmd2.ExecuteScalar();
                //consider returning the visit or at least the visitID
                result3 = result2.ToString();

            }

            return result3;
        }

        public Visit getVisitByID(int visitID)
        {
            Visit clientVisit = new Visit();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + Vtable + " WHERE visitID =" + visitID , conn);

                using (var reader = cmd.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    while (reader.Read())
                    {
                        
                            clientVisit.VisitID = Convert.ToInt32(reader["VisitID"]);
                            clientVisit.ClientID = Convert.ToInt32(reader["ClientID"]);
                            clientVisit.VisitDate = ToDateTime(reader["Date"]);

                            clientVisit.Mens = Convert.ToInt32(reader["Mens"]);
                            clientVisit.Womens = Convert.ToInt32(reader["Womens"]);
                            clientVisit.Kids = Convert.ToInt32(reader["Kids"]);

                            clientVisit.LastBackpack = ToDateTime(reader["LastBackPackDate"]);
                            clientVisit.LastSleepingBag = ToDateTime(reader["LastSleepingBagDate"]);

                            clientVisit.busTicket = DoubleCheck((reader["busTicket"]));
                            clientVisit.diapers = DoubleCheck((reader["diapers"]));
                            clientVisit.financialAid = DoubleCheck((reader["financialAid"]));
                            clientVisit.giftCard = DoubleCheck((reader["giftCard"]));
                            clientVisit.HouseHoldItems = ToString(reader["HouseHoldItems"]);

                            clientVisit.Request = reader["Request"].ToString();
                    }
                }
            }


            return clientVisit;
        }


     public Visit checkout(int visitID, int mens, int womens, int kids, bool backpack, bool sleepingbag, string request, int financialAid , int diapers, int giftCard, int busTicket, string houseHoldItems)
        {

            // STILL PENDING: Financial Aid was causing problems. So its the only one left out at the moment.

            Visit clientVisit = new Visit();

            int insertMens = mens;
            int insertWomens = womens;
            int insertKids = kids;
            string backpackInsert = "Current_Date()";
            string sleepInsert = "Current_Date()";
            string requestInsert = "'" + request + "'";
            int financialAidInsert = financialAid;
            int diapersInsert = diapers;
            int giftCardInsert = giftCard;
            int busTicketInsert = busTicket;
            string houseHoldItemsInsert = "'" + houseHoldItems + "'";


            if (backpack == false)
            {
                backpackInsert = "0000-00-00";
            }
            if(sleepingbag == false)
            {
                sleepInsert = "0000-00-00";
            }



            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                
                MySqlCommand cmd = new MySqlCommand("UPDATE " + Vtable + " SET Mens = '" + insertMens +"' , Womens = '"+ insertWomens +"', Kids = '"+ insertKids + "', LastBackPackDate = " + backpackInsert+ ", LastSleepingBagDate = "+ sleepInsert +", Request = "+ requestInsert +", HouseHoldItems = " + houseHoldItemsInsert + " , busTicket = '"+ busTicketInsert +"' , diapers = '"+ diapersInsert +"' , giftCard = '"+ giftCardInsert + "'  WHERE VisitID = " + visitID, conn);

                cmd.ExecuteNonQuery();

                MySqlCommand cmd2 = new MySqlCommand("SELECT * FROM " + Vtable + " WHERE visitID =" + visitID, conn);


                using (var reader = cmd2.ExecuteReader())
                {
                    //adding information MUST reflect the exact table id inside the [" "]
                    //whereas the assignments must match the model data. 
                    while (reader.Read())
                    {

                        clientVisit.VisitID = Convert.ToInt32(reader["VisitID"]);
                        clientVisit.ClientID = Convert.ToInt32(reader["ClientID"]);
                        clientVisit.VisitDate = ToDateTime(reader["Date"]);

                        clientVisit.Mens = Convert.ToInt32(reader["Mens"]);
                        clientVisit.Womens = Convert.ToInt32(reader["Womens"]);
                        clientVisit.Kids = Convert.ToInt32(reader["Kids"]);

                        clientVisit.LastBackpack = ToDateTime(reader["LastBackPackDate"]);
                        clientVisit.LastSleepingBag = ToDateTime(reader["LastSleepingBagDate"]);

                        clientVisit.busTicket = DoubleCheck((reader["busTicket"]));
                        clientVisit.diapers = DoubleCheck((reader["diapers"]));
                        clientVisit.financialAid = DoubleCheck((reader["financialAid"]));
                        clientVisit.giftCard = DoubleCheck((reader["giftCard"]));
                        clientVisit.HouseHoldItems = ToString(reader["HouseHoldItems"]);

                        clientVisit.Request = reader["Request"].ToString();
                    }
                }
            }

            return clientVisit;

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
