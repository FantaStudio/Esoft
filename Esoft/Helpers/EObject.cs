using Esoft.Database;
using Esoft.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoft.Helpers
{
    // Класс недвижимости
    public class EObject
    {
        public int ID { get; set; }

        public Main.ObjectType ObjectType { get; set; }

        public EObject()
        {
            ID = -1;
        }

        // Перечень инпутов недвижимости
        public enum EObjectInputTypes
        {
            City,
            Street,
            House,
            Number,
            Latitude,
            Longtitude,
            Floor,
            Rooms,
            Square,
        }

        // Метод, проверяющий переданные в него поля в соответствии с типом объекта, который возвращает список ошибок и инпутов, в которых они возникли
        public static (List<string> errors, List<EInput> inputs) CheckerEnteredData(Dictionary<EObjectInputTypes, EInput> allInputs, Main.ObjectType objectType)
        {
            var errors = new List<string>();
            var inputs = new List<EInput>();
            string city, street, house, number, latitude, longtitude, floor, rooms, square;
            city = street = house = number = latitude = longtitude = floor = rooms = square = null;

            if (allInputs.ContainsKey(EObjectInputTypes.City))
                city = allInputs[EObjectInputTypes.City].Text;
            if (allInputs.ContainsKey(EObjectInputTypes.Street))
                street = allInputs[EObjectInputTypes.Street].Text;
            if (allInputs.ContainsKey(EObjectInputTypes.House))
                house = allInputs[EObjectInputTypes.House].Text;
            if (allInputs.ContainsKey(EObjectInputTypes.Number))
                number = allInputs[EObjectInputTypes.Number].Text;
            if (allInputs.ContainsKey(EObjectInputTypes.Latitude))
                latitude = allInputs[EObjectInputTypes.Latitude].Text;
            if (allInputs.ContainsKey(EObjectInputTypes.Longtitude))
                longtitude = allInputs[EObjectInputTypes.Longtitude].Text;
            if (allInputs.ContainsKey(EObjectInputTypes.Floor) && (objectType == Main.ObjectType.Flat || objectType == Main.ObjectType.House))
                floor = allInputs[EObjectInputTypes.Floor].Text;
            if (allInputs.ContainsKey(EObjectInputTypes.Rooms) && (objectType == Main.ObjectType.Flat || objectType == Main.ObjectType.House))
                rooms = allInputs[EObjectInputTypes.Rooms].Text;
            if (allInputs.ContainsKey(EObjectInputTypes.Square))
                square = allInputs[EObjectInputTypes.Square].Text;

            if (city != null && (string.IsNullOrEmpty(city) || string.IsNullOrWhiteSpace(city)))
            {
                errors.Add("Введите город");
                inputs.Add(allInputs[EObjectInputTypes.City]);
            }
            if (street != null)
            {
                if (!string.IsNullOrEmpty(street) && !Main.IsOnlyLettersInString(street))
                {
                    errors.Add("Неверное название улицы");
                    inputs.Add(allInputs[EObjectInputTypes.Street]);
                }
            }
            if (house != null)
            {
                if (!string.IsNullOrEmpty(house) && !int.TryParse(house, out int _))
                {
                    errors.Add("Неверный номер дома");
                    inputs.Add(allInputs[EObjectInputTypes.House]);
                }
            }
            if (number != null)
            {
                if (!string.IsNullOrEmpty(number) && !int.TryParse(number, out int _))
                {
                    errors.Add("Неверный номер квартиры");
                    inputs.Add(allInputs[EObjectInputTypes.Number]);
                }
            }
            if (latitude != null)
            {
                double latitudeValue;
                if (string.IsNullOrEmpty(latitude) || string.IsNullOrWhiteSpace(latitude))
                {
                    errors.Add("Введите широту");
                    inputs.Add(allInputs[EObjectInputTypes.Latitude]);
                }
                else if (!double.TryParse(latitude, out latitudeValue))
                {
                    errors.Add("Неверный формат широты");
                    inputs.Add(allInputs[EObjectInputTypes.Latitude]);
                }else if(latitudeValue < -90 || latitudeValue > 90)
                {
                    errors.Add("Неверный формат широты");
                    inputs.Add(allInputs[EObjectInputTypes.Latitude]);
                }
            }
            if (longtitude != null)
            {
                double longtitudeValue;
                if (string.IsNullOrEmpty(longtitude) || string.IsNullOrWhiteSpace(longtitude))
                {
                    errors.Add("Введите долготу");
                    inputs.Add(allInputs[EObjectInputTypes.Longtitude]);
                }
                else if (!double.TryParse(longtitude, out longtitudeValue))
                {
                    errors.Add("Неверный формат долготы");
                    inputs.Add(allInputs[EObjectInputTypes.Longtitude]);
                }
                else if (longtitudeValue < -180 || longtitudeValue > 180)
                {
                    errors.Add("Неверный формат широты");
                    inputs.Add(allInputs[EObjectInputTypes.Latitude]);
                }
            }
            if (floor != null)
            {
                if (string.IsNullOrEmpty(floor) || string.IsNullOrWhiteSpace(floor))
                {
                    if (objectType == Main.ObjectType.Flat)
                        errors.Add("Введите этаж");
                    else
                        errors.Add("Введите этажность");
                    inputs.Add(allInputs[EObjectInputTypes.Floor]);
                }
                else if (!int.TryParse(floor, out int _))
                {
                    errors.Add("Неверный формат этажа/этажности");
                    inputs.Add(allInputs[EObjectInputTypes.Floor]);
                }
            }
            if (rooms != null)
            {
                if (string.IsNullOrEmpty(rooms) || string.IsNullOrWhiteSpace(rooms))
                {
                    errors.Add("Введите количество комнат");
                    inputs.Add(allInputs[EObjectInputTypes.Rooms]);
                }
                else if (!int.TryParse(rooms, out int _))
                {
                    errors.Add("Неверный формат количества комнат");
                    inputs.Add(allInputs[EObjectInputTypes.Rooms]);
                }
            }
            if (square != null)
            {
                if (string.IsNullOrEmpty(square) || string.IsNullOrWhiteSpace(square))
                {
                    errors.Add("Введите площадь");
                    inputs.Add(allInputs[EObjectInputTypes.Square]);
                }
                else if (!double.TryParse(square, out double _))
                {
                    errors.Add("Неверный формат площади");
                    inputs.Add(allInputs[EObjectInputTypes.Square]);
                }
            }
            return (errors, inputs);
        }

    }
}
