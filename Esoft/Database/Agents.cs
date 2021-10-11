using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Esoft.Database.Common;

namespace Esoft.Database
{
    class Agents
    {
        public int ID { get; set; }

        public string Login { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string Middlename { get; set; }

        public int DealShare { get; set; }

        public static List<Agents> GetBy(string columns = null,string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<Agents> agent = new List<Agents>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM agents {setString}", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            agent.Add(new Agents()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Surname = DB.GetStringFromColumn(reader,"Surname"),
                                Name = DB.GetStringFromColumn(reader,"Name"),
                                Middlename = DB.GetStringFromColumn(reader,"Middlename"),
                                DealShare = reader.GetInt32(reader.GetOrdinal("DealShare")),
                                Login = DB.GetStringFromColumn(reader, "Login")
                            });
                            if(one) break;
                        }
                    }
                }
                DB.Connection.Close();
                return agent;
            }
            catch (Exception)
            {
                DB.Reset();
                return agent;
            }
        }

        public static List<Agents> GetAll() => GetBy();

        public static Agents Get(int id) => GetBy("ID", id.ToString(), true).FirstOrDefault();

        public static Agents Get(string login) => GetBy("Login", login, true).FirstOrDefault();

        public static Agents Get(string login, string password) => GetBy("Login,Password", $"{login},{password}",true).FirstOrDefault();

        public static bool Add(string login,string password, string surname, string name, string middlename, int dealshare = 45) => 
            DB.Add("agents", "Login,Password,Surname,Name,Middlename,DealShare", $"{login},{password},{surname},{name},{middlename},{dealshare}");

        public static bool Delete(int id) =>
            DB.Delete("agents", DB.MakeConditionString("ID",id.ToString()));

        public static bool Update(int id, string surname, string name, string middlename, int dealshare = 45) =>
            DB.Update("agents","Surname,Name,Middlename,DealShare",$"{surname},{name},{middlename},{dealshare}",DB.MakeConditionString("ID",id.ToString()));

        public static bool IsExistInOffer(int id) => Offers.GetBy("AgentID", id.ToString(), true).Count > 0;
        public static bool IsExistInDemand(int id)
        {
            int apartment = ApartmentDemand.GetBy("AgentID",id.ToString(),true).Count;
            int house = HouseDemand.GetBy("AgentID",id.ToString(),true).Count;
            int land = LandDemand.GetBy("AgentID", id.ToString(), true).Count;
            return apartment > 0 || house > 0 || land > 0;
        }

    }
}
