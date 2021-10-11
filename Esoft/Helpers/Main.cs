using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Esoft.Helpers
{
    public static class Main
    {
        public static readonly string Currency = "Р"; // Валюта
        public static readonly string SquareUnit = "м²"; // ед.измерения площади

        public static readonly int ClientBuyerCommission = 3; // Коммиссия для клиента-покупателя в процентах
        public static readonly Dictionary<ObjectType,(double,int)> ClientSellerCommission = new Dictionary<ObjectType, (double, int)>() // Коммиссия для клиента-продавца (тип объекта, (фикс.цена, процент))
        {
            {ObjectType.Flat, (36000,1)},
            {ObjectType.House, (30000,1)},
            {ObjectType.Land, (30000,2)}
        };

        public static EUser CurrentAppUser { get; set; }
        
        // Перечень возможных пользователей
        public enum UserType
        {
            Agent,
            Client,
        }

        // Перечень возможных объектов недвижимости
        public enum ObjectType
        {
            House,
            Flat,
            Land,
        }

        public static Dictionary<ObjectType, string> ObjectTypeTranslate = new Dictionary<ObjectType, string>
        {
            {ObjectType.House,"Дом"},
            {ObjectType.Flat,"Квартира"},
            {ObjectType.Land,"Земля"}
        };

        public static Dictionary<UserType, string> UsertTypeTranslate = new Dictionary<UserType, string>
        {
            {UserType.Agent,"Риэлтор"},
            {UserType.Client,"Клиент"},
        };

        // Проверяет, что в строке только буквы
        public static bool IsOnlyLettersInString(string input) => 
            Regex.IsMatch(input, @"^[a-zA-Z]+$") || Regex.IsMatch(input, @"^[а-яА-Я]+$");


        private static readonly char[] theCharacetrers = new[] {'&','\\','/','!','%','#','^'};
        // Проверят, есть ли в строке спец символы
        public static bool IsStringWithoutSpecialSymbols(string input) => 
            input.IndexOfAny(theCharacetrers) == -1;

        // Преобразует данные об адресе, в удобную строку для представления
        static public string GetAdressString(string city, string street, int? house, int? flat)
        {
            if (string.IsNullOrEmpty(city)) return "";
            string adress = $"г.{city}";
            if (!string.IsNullOrEmpty(street)) adress += ", ул." + street; 
            if (house != null) adress += ", д." + house;
            if (flat != null) adress += ", кв." + flat;
            return adress;
        }

        // Позволяет найти в объекте все элементы нужного типа
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (rawChild is DependencyObject)
                    {
                        DependencyObject child = (DependencyObject)rawChild;
                        if (child is T)
                        {
                            yield return (T)child;
                        }

                        foreach (T childOfChild in FindVisualChildren<T>(child))
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        // Преобразует данные о ФИО, в удобную строку для представления
        public static string ToFioString(string surname,string name,string middlename)
        {
            if (surname == null || surname.Length < 1) surname = " ";
            if (name == null || name.Length < 1) name = " ";
            if (middlename == null || middlename.Length < 1) middlename = " ";
            return $"{surname} {name.ToUpper()[0]}.{middlename.ToUpper()[0]}";
        }

        // Проверяет, являются ли числа диапазоном
        public static bool IsNumbersLikeRange(int one, int two) => two >= one;

        // Преобразует строку в double
        public static double? GetDouble(string str)
        {
            try
            {
                return double.Parse(str);
            }
            catch (Exception)
            {
                
                return null;
            }
        }

        // Преобразует строку в int
        public static int? GetInt(string str)
        {
            try
            {
                int dd = int.Parse(str);
                return dd;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Проверяет, является ли окно модальным
        public static bool IsModal(this Window window) => (bool)typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window);

        // Получает изображение по пути до него
        public static ImageSource GetImageFromPath(string path)
        {
            try
            {
                return new ImageSourceConverter().ConvertFromString("pack://application:,,,/Esoft;component/"+path) as ImageSource;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }

    public partial class MessageHelper : Window
    {
        public enum MessageType
        {
            info,
            error,
            warning,
            success
        }

        private static Dictionary<MessageType, string> typeToIcon = new Dictionary<MessageType, string>()
        {
            {MessageType.error,"Resources/images/danger.png"},
            {MessageType.success,"Resources/images/success.png"},
            {MessageType.warning, "Resources/images/warning.png"},
            {MessageType.info,"Resources/images/alert.png"}
        };

        public MessageHelper()
        {
            InitializeComponent();
        }

        static MessageHelper cMessageHelper;
        static MessageBoxResult result;

        public static MessageBoxResult Show(string message,MessageType type = MessageType.success, MessageBoxButton button = MessageBoxButton.OK)
        {
            cMessageHelper = new MessageHelper();
            cMessageHelper.ContentText.Text = message;
            cMessageHelper.ContentIcon.Source = Main.GetImageFromPath(typeToIcon[type]);
            switch (button)
            {
                case MessageBoxButton.YesNo:
                    cMessageHelper.OkButton.Content = "Да";
                    cMessageHelper.CancelButton.Content = "Нет";
                    break;
                case MessageBoxButton.OK:
                    cMessageHelper.CancelButton.Visibility = Visibility.Collapsed;
                    cMessageHelper.ButtonsGrid.Columns = 1;
                    cMessageHelper.OkButton.Content = "ОК";
                    break;
            }
            cMessageHelper.MouseDown += (s,e) => cMessageHelper.DragMove();
            cMessageHelper.CloseButton.MouseDown += (s, e) =>
            {
                result = MessageBoxResult.None;
                cMessageHelper.Close();
            };
            cMessageHelper.OkButton.Click += (s, e) =>
            {
                switch (button)
                {
                    case MessageBoxButton.OK:
                        result = MessageBoxResult.OK;
                        break;
                    case MessageBoxButton.YesNo:
                        result = MessageBoxResult.Yes;
                        break;
                }
                cMessageHelper.Close();
            };
            cMessageHelper.CancelButton.Click += (s, e) =>
            {
                switch (button)
                {
                    case MessageBoxButton.YesNo:
                        result = MessageBoxResult.No;
                        break;
                }
                cMessageHelper.Close();
            };
            
            cMessageHelper.ShowDialog();
            return result;
        }
    }
}
