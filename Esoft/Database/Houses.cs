using Esoft.Database.Common;
using Esoft.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Esoft.Database
{
    class Houses : Realty
    {
        public int TotalFloors { get; set; }

        public int Rooms { get; set; }

        public static List<Houses> GetBy(string columns = null, string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<Houses> House = new List<Houses>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM houses {setString}", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            House.Add(new Houses()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Address_City = DB.GetStringFromColumn(reader,"Address_City"),
                                Address_House = DB.GetIntFromColumn(reader, "Address_House"),
                                Address_Number = DB.GetIntFromColumn(reader,"Address_Number"),
                                Address_Street = DB.GetStringFromColumn(reader,"Address_Street"),
                                Coordinate_latitude = reader.GetDouble(reader.GetOrdinal("Coordinate_latitude")),
                                Coordinate_longitude = reader.GetDouble(reader.GetOrdinal("Coordinate_longitude")),
                                TotalArea = reader.GetDouble(reader.GetOrdinal("TotalArea")),
                                TotalFloors = reader.GetInt32(reader.GetOrdinal("TotalFloors")),
                                ClientID = DB.GetIntFromColumn(reader, "ClientID"),
                            });
                            if (one) break;
                        }
                    }
                }
                DB.Connection.Close();
                return House;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Source);
                DB.Reset();
                return House;
            }
        }

        public static Houses Get(int id) => GetBy("ID", id.ToString(),true).FirstOrDefault();

        public static List<Houses> GetAll() => GetBy();

        public static List<Houses> GetAll(int clientID) => GetBy("ClientID", clientID.ToString());

        public static bool Add(int clientID,string city, double latitude, double longtitude, int floors, double square, int rooms, string street = "", int? house = null, int? number = null) =>
            DB.Add("houses", "Address_City,Address_Street,Address_House,Address_Number,Coordinate_latitude,Coordinate_longitude,TotalFloors,TotalArea,Rooms,ClientID",
                $"{city},{street},{house},{number},{latitude},{longtitude},{floors},{square},{rooms},{clientID}");

        public static bool Delete(int id) =>
            DB.Delete("houses", DB.MakeConditionString("ID", id.ToString()));

        public static bool Update(int id, string city, double latitude, double longtitude, int floors, double square,int rooms,string street = "", int? house = null, int? number = null) =>
            DB.Update("houses", "Address_City,Address_Street,Address_House,Address_Number,Coordinate_latitude,Coordinate_longitude,TotalFloors,TotalArea,Rooms",
                $"{city},{street},{house},{number},{latitude},{longtitude},{floors},{square},{rooms}", DB.MakeConditionString("ID", id.ToString()));
    }
}
