using Esoft.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoft.Database
{
    class Offers
    {
        public int ID { get; set; }
        public int AgentID { get; set; }
        public int ClientID { get; set; }
        public int ObjectID { get; set; }
        public Main.ObjectType ObjectType { get; set; }

        public int Price { get; set; }

        public static List<Offers> GetBy(string columns = null, string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<Offers> offer = new List<Offers>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM offers {setString}", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            offer.Add(new Offers()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                AgentID = reader.GetInt32(reader.GetOrdinal("AgentID")),
                                ClientID = reader.GetInt32(reader.GetOrdinal("ClientID")),
                                ObjectID = reader.GetInt32(reader.GetOrdinal("ObjectID")),
                                ObjectType = (Main.ObjectType)reader.GetInt32(reader.GetOrdinal("ObjectType")),
                                Price = reader.GetInt32(reader.GetOrdinal("Price"))
                            });
                            if (one) break;
                        }
                    }
                }
                DB.Connection.Close();
                return offer;
            }
            catch (Exception)
            {
                DB.Reset();
                return offer;
            }
        }

        public static List<Offers> GetAll() => GetBy();

        public static List<Offers> GetClientAll(int clientID) => GetBy("ClientID", clientID.ToString());

        public static List<Offers> GetAgentAll(int agentID) => GetBy("AgentID", agentID.ToString());

        public static Offers Get(int id) => GetBy("ID", id.ToString(), true).FirstOrDefault();

        public static bool Add(int agentID,int clientID,int objectID, Main.ObjectType objectType,int price) =>
            DB.Add("offers", "AgentID,ClientID,ObjectID,ObjectType,Price", $"{agentID},{clientID},{objectID},{(int)objectType},{price}");

        public static bool Delete(int id) =>
            DB.Delete("offers", DB.MakeConditionString("ID", id.ToString()));

        public static bool Update(int id,int agentID, int clientID, int objectID, Main.ObjectType objectType, int price) =>
            DB.Update("offers", "AgentID,ClientID,ObjectID,ObjectType,Price", $"{agentID},{clientID},{objectID},{(int)objectType},{price}", DB.MakeConditionString("ID", id.ToString()));

        public static bool IsObjectExist(int objectID, Main.ObjectType objectType) => GetBy("ObjectID,ObjectType", $"{objectID},{(int)objectType}", true).Count > 0;
    }
}
