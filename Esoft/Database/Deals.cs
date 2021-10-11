using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.SQLite;
using Esoft.Helpers;

namespace Esoft.Database
{
    class Deals
    {
        public int ID { get; set; }
        public int OfferID { get; set; }

        public int DemandID { get; set; }

        public Main.ObjectType ObjectType { get; set; }

        public static List<Deals> GetBy(string columns = null, string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<Deals> deal = new List<Deals>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM deals {setString}", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            deal.Add(new Deals()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                DemandID = reader.GetInt32(reader.GetOrdinal("DemandID")),
                                OfferID = reader.GetInt32(reader.GetOrdinal("OfferID")),
                                ObjectType = (Main.ObjectType)reader.GetInt32(reader.GetOrdinal("ObjectType"))
                            });
                            if (one) break;
                        }
                    }
                }
                DB.Connection.Close();
                return deal;
            }
            catch (Exception)
            {
                DB.Reset();
                return deal;
            }
        }

        public static List<Deals> GetAll() => GetBy();

        public static Deals Get(int id) => GetBy("ID", id.ToString(), true).FirstOrDefault();

        public static bool Add(int DemandID,Main.ObjectType objectType,int OfferID) =>
            DB.Add("deals", "DemandID,ObjectType,OfferID", $"{DemandID},{(int)objectType},{OfferID}");

        public static bool Delete(int id) =>
            DB.Delete("deals", DB.MakeConditionString("ID", id.ToString()));

        public static bool Update(int id, int DemandID, Main.ObjectType objectType, int OfferID) =>
            DB.Update("deals", "DemandID,OfferID", $"{DemandID},{(int)objectType},{OfferID}", DB.MakeConditionString("ID", id.ToString()));

        public static bool IsOfferInDeal(int offerID) => GetBy("OfferID", offerID.ToString(), true).Count > 0;

        public static bool IsDemandInDeal(EDemand demand) => GetBy("DemandID,ObjectType", $"{demand.ID},{(int)demand.ObjectType}", false).Count > 0;
    }
}
