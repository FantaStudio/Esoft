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
    class Apartments : Realty
    {
        public int Rooms { get; set; }
        public int Floor { get; set; }

        public static List<Apartments> GetBy(string columns = null, string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<Apartments> apartment = new List<Apartments>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM apartments {setString}", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            apartment.Add(new Apartments()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Address_City = DB.GetStringFromColumn(reader,"Address_City"),
                                Address_House = DB.GetIntFromColumn(reader,"Address_House"),
                                Address_Number = DB.GetIntFromColumn(reader,"Address_Number"),
                                Address_Street = DB.GetStringFromColumn(reader,"Address_Street"),
                                Coordinate_latitude = reader.GetDouble(reader.GetOrdinal("Coordinate_latitude")),
                                Coordinate_longitude = reader.GetDouble(reader.GetOrdinal("Coordinate_longitude")),
                                TotalArea = reader.GetDouble(reader.GetOrdinal("TotalArea")),
                                Rooms = reader.GetInt32(reader.GetOrdinal("Rooms")),
                                Floor = reader.GetInt32(reader.GetOrdinal("Floor")),
                                ClientID = DB.GetIntFromColumn(reader,"ClientID"),
                            });
                            if(one) break;
                        }
                    }
                }
                DB.Connection.Close();
                return apartment;
            }
            catch (Exception)
            {
                DB.Reset();
                return apartment;
            }
        }

        public static Apartments Get(int id) => GetBy("ID", id.ToString(),true).FirstOrDefault();

        public static List<Apartments> GetAll() => GetBy();

        public static List<Apartments> GetAll(int clientID) => GetBy("ClientID", clientID.ToString());

        public static bool Add(int clientID,string city, double latitude, double longtitude, double square, int floors, int rooms, string street = "", int? house = null, int? number = null) => 
            DB.Add("apartments", "Address_City,Address_Street,Address_House,Address_Number,Coordinate_latitude,Coordinate_longitude,TotalArea,Rooms,Floor,ClientID",
                $"{city},{street},{house},{number},{latitude},{longtitude},{square},{rooms},{floors},{clientID}");

        public static bool Delete(int id) =>
            DB.Delete("apartments", DB.MakeConditionString("ID", id.ToString()));

        public static bool Update(int id, string city, double latitude, double longtitude, double square, int floors, int rooms, string street = "", int? house = null, int? number = null) =>
            DB.Update("apartments", "Address_City,Address_Street,Address_House,Address_Number,Coordinate_latitude,Coordinate_longitude,TotalArea,Rooms,Floor",
                 $"{city},{street},{house},{number},{latitude},{longtitude},{square},{rooms},{floors}", DB.MakeConditionString("ID", id.ToString()));
    }
}
