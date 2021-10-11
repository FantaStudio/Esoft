using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Esoft.Database
{
    class Districts
    {
        public string Name { get; set; }

        public string Area { get; set; }

        public static List<Districts> GetBy(string columns = null, string values = null, bool one = false)
        {
            string setString = (columns != null && values != null) ? DB.MakeConditionString(columns, values) : null;
            List<Districts> district = new List<Districts>();
            try
            {
                DB.Connection.Open();
                using (SQLiteCommand command = new SQLiteCommand($"SELECT * FROM districts {setString} LIMIT 30", DB.Connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            district.Add(new Districts()
                            {
                                Name = DB.GetStringFromColumn(reader, "Name"),
                                Area = DB.GetStringFromColumn(reader, "Area"),
                            });
                            if (one) break;
                        }
                    }
                }
                DB.Connection.Close();
                return district;
            }
            catch (Exception)
            {
                DB.Reset();
                return district;
            }
        }

        public static Districts Get(string name) => GetBy("Name", name, true).FirstOrDefault();

        public static List<Districts> GetAll() => GetBy();

        public List<Point> GetPoints() => AreaToPoints(Area);

        public static List<Point> AreaToPoints(string area)
        {
            if (!area.StartsWith("(") || !area.EndsWith(")"))
                throw new ArgumentException("Неверный формат строки точек");
            var points = new List<Point>();
            area = area.Trim(new char[] { '(', ')' });
            var areaLocations = area.Split('(');
            foreach (string locationString in areaLocations)
            {
                string locationStr = locationString.Trim(new char[]{',',')'});
                string[] locationPoints = locationStr.Split(',');
                if (locationPoints.Length == 2)
                {
                    double x, y;
                    if(double.TryParse(locationPoints[0].Replace('.',','),out x) && double.TryParse(locationPoints[1].Replace('.',','),out y))
                        points.Add(new Point(x,y));
                }
            }
            return points;
        }

        public static bool IsInPloygon(List<Point> poly, Point point)
        {
            var coef = poly.Skip(1).Select((p, i) => (point.Y - poly[i].Y) * (p.X - poly[i].X) - (point.X - poly[i].X) * (p.Y - poly[i].Y)).ToList();
            if (coef.Any(p => p == 0))
                return true;
            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }
    }
}
