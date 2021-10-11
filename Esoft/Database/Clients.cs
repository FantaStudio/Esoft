using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using Esoft.Database.Common;

namespace Esoft.Database
{
    class Clients
    {
        public int ID { get; set; }

        public string Login { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Middlename { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public static List<Clients> GetBy(string columns = null, string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<Clients> clients = new List<Clients>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM clients {setString}", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new Clients()
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Surname = DB.GetStringFromColumn(reader,"Surname"),
                                Name = DB.GetStringFromColumn(reader,"Name"),
                                Middlename = DB.GetStringFromColumn(reader,"Middlename"),
                                Phone = DB.GetStringFromColumn(reader,"Phone"),
                                Email = DB.GetStringFromColumn(reader,"Email"),
                                Login = DB.GetStringFromColumn(reader, "Login")
                            });
                            if(one)break;
                        }
                    }
                }
                DB.Connection.Close();
                return clients;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                DB.Reset();
                return clients;
            }
        }

        public static List<Clients> GetAll() => GetBy();

        public static Clients Get(int id) => GetBy("ID", id.ToString(),true).FirstOrDefault();

        public static Clients Get(string login) => GetBy("Login", login,true).FirstOrDefault();

        public static Clients Get(string login, string password) => GetBy("Login,Password", $"{login},{password}",true).FirstOrDefault();

        public static bool Add(string login, string password, string surname, string name, string middlename = "",string email = "",string phone = "") =>
            DB.Add("clients", "Login,Password,Surname,Name,Middlename,Phone,Email", $"{login},{password},{surname},{name},{middlename},{phone},{email}");

        public static bool Delete(int id) =>
            DB.Delete("clients", DB.MakeConditionString("ID", id.ToString()));

        public static bool Update(int id, string surname, string name, string middlename, string email = "", string phone = "") =>
            DB.Update("clients","Surname,Name,Middlename,Phone,Email",$"{surname},{name},{middlename},{phone},{email}", DB.MakeConditionString("ID", id.ToString()));
        public static bool IsExistInOffer(int id) => Offers.GetBy("ClientID", id.ToString(), true).Count > 0;
        public static bool IsExistInDemand(int id)
        {
            int apartment = ApartmentDemand.GetBy("ClientID", id.ToString(), true).Count;
            int house = HouseDemand.GetBy("ClientID", id.ToString(), true).Count;
            int land = LandDemand.GetBy("ClientID", id.ToString(), true).Count;
            return apartment > 0 || house > 0 || land > 0;
        }
    }
}
