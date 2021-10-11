using Esoft.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoft.Database.Common
{
    class Realty
    {
        public int ID { get; set; }
        public int? ClientID { get; set; }
        public string Address_City { get; set; }
        public string Address_Street { get; set; }
        public int? Address_House { get; set; }
        public int? Address_Number { get; set; }
        public double Coordinate_latitude { get; set; }
        public double Coordinate_longitude { get; set; }
        public double TotalArea { get; set; }
    }

    class RealtyDemand
    {
        public int ID { get; set; }
        public int AgentID { get; set; }
        public int ClientID { get; set; }
        public string Address_City { get; set; }
        public string Address_Street { get; set; }
        public int? Address_House { get; set; }
        public int? Address_Number { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public double? MinArea { get; set; }
        public double? MaxArea { get; set; }

    }

    class ApartmentDemand : RealtyDemand
    {
        public int? MinRooms { get; set; }
        public int? MaxRooms { get; set; }
        public int? MinFloor { get; set; }
        public int? MaxFloor { get; set; }

        public static List<ApartmentDemand> GetBy(string columns = null, string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<ApartmentDemand> apdemand = new List<ApartmentDemand>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM apartmentDemands {setString}", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            apdemand.Add(new ApartmentDemand()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                AgentID = reader.GetInt32(reader.GetOrdinal("AgentID")),
                                ClientID = reader.GetInt32(reader.GetOrdinal("ClientID")),
                                Address_City = DB.GetStringFromColumn(reader, "Address_City"),
                                Address_House = DB.GetIntFromColumn(reader, "Address_House"),
                                Address_Number = DB.GetIntFromColumn(reader, "Address_Number"),
                                Address_Street = DB.GetStringFromColumn(reader, "Address_Street"),
                                MinPrice = DB.GetIntFromColumn(reader, "MinPrice"),
                                MaxPrice = DB.GetIntFromColumn(reader, "MaxPrice"),
                                MinArea = DB.GetDoubleFromColumn(reader, "MinArea"),
                                MaxArea = DB.GetDoubleFromColumn(reader, "MaxArea"),
                                MinRooms = DB.GetIntFromColumn(reader, "MinRooms"),
                                MaxRooms = DB.GetIntFromColumn(reader, "MaxRooms"),
                                MinFloor = DB.GetIntFromColumn(reader, "MinFloor"),
                                MaxFloor = DB.GetIntFromColumn(reader, "MaxFloor"),
                            });
                            if (one) break;
                        }
                    }
                }
                DB.Connection.Close();
                return apdemand;
            }
            catch (Exception)
            {
                DB.Reset();
                return apdemand;
            }
        }

        public static List<ApartmentDemand> GetAll() => GetBy();

        public static List<ApartmentDemand> GetClientAll(int clientID) => GetBy("ClientID", clientID.ToString());

        public static List<ApartmentDemand> GetAgentAll(int agentID) => GetBy("AgentID", agentID.ToString());

        public static ApartmentDemand Get(int id) => GetBy("ID", id.ToString(), true).FirstOrDefault();

        
        public static bool Add(int agentID, int clientID, 
            string city = "",
            string street = "",
            int? house = null,
            int? number = null,
            int? minPrice = null,
            int? maxPrice = null,
            double? minArea = null,
            double? maxArea = null,
            int? minRooms = null,
            int? maxRooms = null,
            int? minFloor = null,
            int? maxFloor = null) =>
            DB.Add("apartmentDemands", "AgentID,ClientID,Address_City,Address_Street,Address_House,Address_Number,MinPrice,MaxPrice,MinArea,MaxArea,MinRooms,MaxRooms,MinFloor,MaxFloor",
                $"{agentID},{clientID},{city},{street},{house},{number},{minPrice},{maxPrice},{minArea},{maxArea},{minRooms},{maxRooms},{minFloor},{maxFloor}");

        public static bool Delete(int id) =>
            DB.Delete("apartmentDemands", DB.MakeConditionString("ID", id.ToString()));

        public static bool Update(int id,int agentID, int clientID,
            string city = "",
            string street = "",
            int? house = null,
            int? number = null,
            int? minPrice = null,
            int? maxPrice = null,
            double? minArea = null,
            double? maxArea = null,
            int? minRooms = null,
            int? maxRooms = null,
            int? minFloor = null,
            int? maxFloor = null) =>
            DB.Update("apartmentDemands", "AgentID,ClientID,Address_City,Address_Street,Address_House,Address_Number,MinPrice,MaxPrice,MinArea,MaxArea,MinRooms,MaxRooms,MinFloor,MaxFloor",
                $"{agentID},{clientID},{city},{street},{house},{number},{minPrice},{maxPrice},{minArea},{maxArea},{minRooms},{maxRooms},{minFloor},{maxFloor}",
                DB.MakeConditionString("ID", id.ToString()));
    }

    class HouseDemand : RealtyDemand
    {
        public int? MinRooms { get; set; }
        public int? MaxRooms { get; set; }
        public int? MinFloors { get; set; }
        public int? MaxFloors { get; set; }

        public static List<HouseDemand> GetBy(string columns = null, string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<HouseDemand> housedemand = new List<HouseDemand>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM houseDemands {setString}", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            housedemand.Add(new HouseDemand()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                AgentID = reader.GetInt32(reader.GetOrdinal("AgentID")),
                                ClientID = reader.GetInt32(reader.GetOrdinal("ClientID")),
                                Address_City = DB.GetStringFromColumn(reader, "Address_City"),
                                Address_House = DB.GetIntFromColumn(reader, "Address_House"),
                                Address_Number = DB.GetIntFromColumn(reader, "Address_Number"),
                                Address_Street = DB.GetStringFromColumn(reader, "Address_Street"),
                                MinPrice = DB.GetIntFromColumn(reader, "MinPrice"),
                                MaxPrice = DB.GetIntFromColumn(reader, "MaxPrice"),
                                MinArea = DB.GetDoubleFromColumn(reader, "MinArea"),
                                MaxArea = DB.GetDoubleFromColumn(reader, "MaxArea"),
                                MinRooms = DB.GetIntFromColumn(reader, "MinRooms"),
                                MaxRooms = DB.GetIntFromColumn(reader, "MaxRooms"),
                                MinFloors = DB.GetIntFromColumn(reader, "MinFloors"),
                                MaxFloors = DB.GetIntFromColumn(reader, "MaxFloors"),
                            });
                            if (one) break;
                        }
                    }
                }
                DB.Connection.Close();
                return housedemand;
            }
            catch (Exception)
            {
                DB.Reset();
                return housedemand;
            }
        }
        public static List<HouseDemand> GetAll() => GetBy();
        public static List<HouseDemand> GetClientAll(int clientID) => GetBy("ClientID", clientID.ToString());

        public static List<HouseDemand> GetAgentAll(int agentID) => GetBy("AgentID", agentID.ToString());
        public static HouseDemand Get(int id) => GetBy("ID", id.ToString(), true).FirstOrDefault();
        public static bool Add(int agentID, int clientID,
            string city = "",
            string street = "",
            int? house = null,
            int? number = null,
            int? minPrice = null,
            int? maxPrice = null,
            double? minArea = null,
            double? maxArea = null,
            int? minRooms = null,
            int? maxRooms = null,
            int? minFloors = null,
            int? maxFloors = null) =>
            DB.Add("houseDemands", "AgentID,ClientID,Address_City,Address_Street,Address_House,Address_Number,MinPrice,MaxPrice,MinArea,MaxArea,MinRooms,MaxRooms,MinFloors,MaxFloors",
                $"{agentID},{clientID},{city},{street},{house},{number},{minPrice},{maxPrice},{minArea},{maxArea},{minRooms},{maxRooms},{minFloors},{maxFloors}");

        public static bool Delete(int id) =>
            DB.Delete("houseDemands", DB.MakeConditionString("ID", id.ToString()));

        public static bool Update(int id,int agentID, int clientID,
            string city = "",
            string street = "",
            int? house = null,
            int? number = null,
            int? minPrice = null,
            int? maxPrice = null,
            double? minArea = null,
            double? maxArea = null,
            int? minRooms = null,
            int? maxRooms = null,
            int? minFloors = null,
            int? maxFloors = null) =>
            DB.Update("houseDemands", "AgentID,ClientID,Address_City,Address_Street,Address_House,Address_Number,MinPrice,MaxPrice,MinArea,MaxArea,MinRooms,MaxRooms,MinFloors,MaxFloors",
                $"{agentID},{clientID},{city},{street},{house},{number},{minPrice},{maxPrice},{minArea},{maxArea},{minRooms},{maxRooms},{minFloors},{maxFloors}",
                DB.MakeConditionString("ID", id.ToString()));
    }

    class LandDemand : RealtyDemand
    {
        public static List<LandDemand> GetBy(string columns = null, string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<LandDemand> landdemand = new List<LandDemand>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM landDemands {setString}", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            landdemand.Add(new LandDemand()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                AgentID = reader.GetInt32(reader.GetOrdinal("AgentID")),
                                ClientID = reader.GetInt32(reader.GetOrdinal("ClientID")),
                                Address_City = DB.GetStringFromColumn(reader, "Address_City"),
                                Address_House = DB.GetIntFromColumn(reader, "Address_House"),
                                Address_Number = DB.GetIntFromColumn(reader, "Address_Number"),
                                Address_Street = DB.GetStringFromColumn(reader, "Address_Street"),
                                MinPrice = DB.GetIntFromColumn(reader, "MinPrice"),
                                MaxPrice = DB.GetIntFromColumn(reader, "MaxPrice"),
                                MinArea = DB.GetDoubleFromColumn(reader, "MinArea"),
                                MaxArea = DB.GetDoubleFromColumn(reader, "MaxArea"),
                            });
                            if (one) break;
                        }
                    }
                }
                DB.Connection.Close();
                return landdemand;
            }
            catch (Exception)
            {
                DB.Reset();
                return landdemand;
            }
        }
        public static List<LandDemand> GetAll() => GetBy();
        public static List<LandDemand> GetClientAll(int clientID) => GetBy("ClientID", clientID.ToString());
        public static List<LandDemand> GetAgentAll(int agentID) => GetBy("AgentID", agentID.ToString());
        public static LandDemand Get(int id) => GetBy("ID", id.ToString(), true).FirstOrDefault();

        public static bool Add(int agentID, int clientID, Main.ObjectType objectType,
            string city = "",
            string street = "",
            int? house = null,
            int? number = null,
            int? minPrice = null,
            int? maxPrice = null,
            double? minArea = null,
            double? maxArea = null) =>
            DB.Add("landDemands", "AgentID,ClientID,Address_City,Address_Street,Address_House,Address_Number,MinPrice,MaxPrice,MinArea,MaxArea",
                $"{agentID},{clientID},{(int)objectType},{city},{street},{house},{number},{minPrice},{maxPrice},{minArea},{maxArea}");
        public static bool Delete(int id) =>
            DB.Delete("landDemands", DB.MakeConditionString("ID", id.ToString()));

        public static bool Update(int id,int agentID, int clientID,
            string city = "",
            string street = "",
            int? house = null,
            int? number = null,
            int? minPrice = null,
            int? maxPrice = null,
            double? minArea = null,
            double? maxArea = null) =>
            DB.Update("landDemands", "AgentID,ClientID,Address_City,Address_Street,Address_House,Address_Number,MinPrice,MaxPrice,MinArea,MaxArea",
                $"{agentID},{clientID},{city},{street},{house},{number},{minPrice},{maxPrice},{minArea},{maxArea}", 
                DB.MakeConditionString("ID", id.ToString()));
    }
}
