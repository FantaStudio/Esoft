using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;

namespace Esoft.Database
{
    class DB
    {
        static readonly string dbPath = "database/db.db";
        static SQLiteConnection connection;

        public static SQLiteConnection Connection
        {
            get {
                if(connection == null) Connect();
                return connection;
            }
        }

        public static void Connect() => connection = new SQLiteConnection("Data source=" + dbPath);

        public static void Reset()
        {
            if (Connection.State == System.Data.ConnectionState.Open) 
                DB.Connection.Close();
        }

        private static string ParameterNormalize(string parametrValue)
        {
            if (!int.TryParse(parametrValue, out int _) && !double.TryParse(parametrValue, out double _))
                parametrValue = $"'{parametrValue}'";
            return parametrValue;
        }

        public static string EqualRequestBuilder(string columns,string values, string separator = ",")
        {
            //Преобразуем всё в нормальную форму для запроса
            string setString = "";
            string[] columnsTable = columns.Split(',');
            string[] valuesTable = values.Split(',');
            for (int i = 0; i < columnsTable.Length; i++)
            {
                var value = ParameterNormalize(valuesTable[i]);
                if (i == 0) setString = columnsTable[i] + " = " + value;
                else setString += separator + columnsTable[i] + " = " + value;
            }
            return setString;
        }

        public static string MakeConditionString(string columns,string values) =>
            "WHERE " + EqualRequestBuilder(columns, values," AND ");

        public static bool Add(string table, string columns, string values)
        {
            try
            {
                string setString = "";
                string[] valuesTable = values.Split(',');
                for(int i = 0; i < valuesTable.Length; i++)
                {
                    var value = ParameterNormalize(valuesTable[i]);
                    if (i == 0) setString = value;
                    else setString += "," + value;
                }
                int changedCount = 0;
                Connection.Open();
                
                using (var CMD = new SQLiteCommand($"INSERT INTO {table}({columns}) VALUES({setString})",Connection))
                {
                    changedCount = CMD.ExecuteNonQuery();
                }
                Connection.Close();
                return changedCount > 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                DB.Reset();
                return false;
            }
        }

        public static bool Update(string table, string columns, string values, string condition = "")
        {
            try
            {
                string setString = EqualRequestBuilder(columns, values);
                if (setString != "")
                {
                    int changedCount = 0;
                    Connection.Open();
                    using (var CMD = new SQLiteCommand($"UPDATE {table} SET {setString} {condition}",Connection))
                    {
                        changedCount = CMD.ExecuteNonQuery();
                    }
                    Connection.Close();
                    return changedCount > 0;
                }
                else 
                    return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                DB.Reset();
                return false;
            }
        }

        public static bool Delete(string table, string condition = "")
        {
            try
            {
                Connection.Open();
                using (var CMD = new SQLiteCommand($"DELETE FROM {table} {condition}", Connection))
                {
                    CMD.ExecuteNonQuery();
                }
                Connection.Close();
                return true;
            }
            catch (Exception)
            {
                DB.Reset();
                return false;
            }
        }

        public static int GetLastInsertedID(string table)
        {
            int id = -1;
            try
            {
                Connection.Open();
                using(var command = new SQLiteCommand($"SELECT seq FROM sqlite_sequence WHERE name={ParameterNormalize(table)}",Connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt32(reader.GetOrdinal("seq"));
                        break;
                    }
                }
                Connection.Close();
                return id;
            }catch (Exception e)
            {
                MessageBox.Show(e.Message);
                DB.Reset();
                return id;
            }
        }

        public static int? GetIntFromColumn(SQLiteDataReader reader,string column)
        {
            int columnI = reader.GetOrdinal(column);
            if (columnI == -1) return null;
            try
            {
                return reader.GetInt32(columnI);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string GetStringFromColumn(SQLiteDataReader reader, string column)
        {
            int columnI = reader.GetOrdinal(column);
            if (columnI == -1) return null;
            try
            {
                return reader.GetString(columnI);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static double? GetDoubleFromColumn(SQLiteDataReader reader, string column)
        {
            int columnI = reader.GetOrdinal(column);
            if (columnI == -1) return null;
            try
            {
                return reader.GetDouble(columnI);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
