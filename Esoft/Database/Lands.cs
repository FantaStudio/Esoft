using Esoft.Database.Common;
using Esoft.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoft.Database
{
    class Lands : Realty
    {
        public static List<Lands> GetBy(string columns = null, string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<Lands> Land = new List<Lands>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM lands {setString}", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Land.Add(new Lands()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Address_City = DB.GetStringFromColumn(reader,"Address_City"),
                                Address_House = DB.GetIntFromColumn(reader,"Address_House"),
                                Address_Number = DB.GetIntFromColumn(reader,"Address_Number"),
                                Address_Street = DB.GetStringFromColumn(reader,"Address_Street"),
                                Coordinate_latitude = reader.GetDouble(reader.GetOrdinal("Coordinate_latitude")),
                                Coordinate_longitude = reader.GetDouble(reader.GetOrdinal("Coordinate_longitude")),
                                TotalArea = reader.GetDouble(reader.GetOrdinal("TotalArea")),
                                ClientID = DB.GetIntFromColumn(reader, "ClientID"),
                            });
                            if(one) break;
                        }
                    }
                }
                DB.Connection.Close();
                return Land;
            }
            catch (Exception)
            {
                DB.Reset();
                return Land;
            }
        }

        public static Lands Get(int id) => GetBy("ID", id.ToString(),true).FirstOrDefault();

        public static List<Lands> GetAll() => GetBy();

        public static List<Lands> GetAll(int clientID) => GetBy("ClientID", clientID.ToString());

        public static bool Add(int clientID,string city, double latitude, double longtitude, double square, string street = "", int? house = null, int? number = null) =>
            DB.Add("lands", "Address_City,Address_Street,Address_House,Address_Number,Coordinate_latitude,Coordinate_longitude,TotalArea,ClientID",
                $"{city},{street},{house},{number},{latitude},{longtitude},{square},{clientID}");

        public static bool Delete(int id) =>
            DB.Delete("lands", DB.MakeConditionString("ID", id.ToString()));

        public static bool Update(int id, string city, double latitude, double longtitude, double square, string street = "", int? house = null, int? number = null) =>
            DB.Update("lands", "Address_City,Address_Street,Address_House,Address_Number,Coordinate_latitude,Coordinate_longitude,TotalArea", 
                $"{city},{street},{house},{number},{latitude},{longtitude},{square}", DB.MakeConditionString("ID", id.ToString()));
    }
}
