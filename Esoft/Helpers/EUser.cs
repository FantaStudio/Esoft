using Esoft.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esoft.Helpers
{
    // Класс пользователя
    public class EUser
    {
        public int UserID { get; set; }

        public Main.UserType UserType { get; set; }

        public EUser() => UserID = -1;
        public EUser(int userID, Main.UserType userType)
        {
            UserID = userID;
            UserType = userType;
        }

        // Перечень инпутов пользователя
        public enum EUserInputTypes
        {
            Login,
            Password,
            Name,
            Surname,
            Middlename,
            Phone,
            Email,
            DealShare,
        }


        // Метод, проверяющий переданные в него поля в соответствии с типом пользователя, который возвращает список ошибок и инпутов, в которых они возникли
        public static (List<string> errors, List<EInput> inputs) CheckerEnteredData(Dictionary<EUserInputTypes, EInput> allInputs, Main.UserType userType)
        {
            var errors = new List<string>();
            var inputs = new List<EInput>();
            string login, password, name, surname, middlename, phone, email, dealshare;
            login = password = name = surname = middlename = phone = email = dealshare = null;

            if (allInputs.ContainsKey(EUserInputTypes.Login))
                login = allInputs[EUserInputTypes.Login].Text;
            if (allInputs.ContainsKey(EUserInputTypes.Password))
                password = allInputs[EUserInputTypes.Password].Text;
            if (allInputs.ContainsKey(EUserInputTypes.Name))
                name = allInputs[EUserInputTypes.Name].Text;
            if (allInputs.ContainsKey(EUserInputTypes.Surname))
                surname = allInputs[EUserInputTypes.Surname].Text;
            if (allInputs.ContainsKey(EUserInputTypes.Middlename) && userType == Main.UserType.Agent)
                middlename = allInputs[EUserInputTypes.Middlename].Text;
            if (allInputs.ContainsKey(EUserInputTypes.Email) && userType == Main.UserType.Client)
                email = allInputs[EUserInputTypes.Email].Text;
            if (allInputs.ContainsKey(EUserInputTypes.DealShare) && userType == Main.UserType.Agent)
                dealshare = allInputs[EUserInputTypes.DealShare].Text;

            if (login != null && (string.IsNullOrEmpty(login) || string.IsNullOrWhiteSpace(login)))
            {
                errors.Add("Введите логин");
                inputs.Add(allInputs[EUserInputTypes.Login]);
            }

            if (password != null && (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password)))
            {
                errors.Add("Введите пароль");
                inputs.Add(allInputs[EUserInputTypes.Password]);
            }
            if (name != null)
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                {
                    errors.Add("Введите имя");
                    inputs.Add(allInputs[EUserInputTypes.Name]);
                }
                else if (!Main.IsOnlyLettersInString(name))
                {
                    errors.Add("Неверный формат имени");
                    inputs.Add(allInputs[EUserInputTypes.Name]);
                }
            }
            if (surname != null)
            {
                if (string.IsNullOrEmpty(surname) || string.IsNullOrWhiteSpace(surname))
                {
                    errors.Add("Введите фамилию");
                    inputs.Add(allInputs[EUserInputTypes.Surname]);
                }
                else if (!Main.IsOnlyLettersInString(surname))
                {
                    errors.Add("Неверный формат фамилии");
                    inputs.Add(allInputs[EUserInputTypes.Surname]);
                }
            }
            if (middlename != null)
            {
                if (string.IsNullOrEmpty(middlename) || string.IsNullOrWhiteSpace(middlename))
                {
                    errors.Add("Введите отчество");
                    inputs.Add(allInputs[EUserInputTypes.Middlename]);
                }
                else if (!Main.IsOnlyLettersInString(middlename))
                {
                    errors.Add("Неверный формат отчества");
                    inputs.Add(allInputs[EUserInputTypes.Middlename]);
                }
            }
            if (email != null)
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                {
                    errors.Add("Введите почту");
                    inputs.Add(allInputs[EUserInputTypes.Email]);
                }
            }
            if (dealshare != null && (!string.IsNullOrEmpty(dealshare) && !int.TryParse(dealshare, out int _)))
            {
                errors.Add("Неправильный формат доли от сделки");
                inputs.Add(allInputs[EUserInputTypes.DealShare]);
            }
            return (errors, inputs);
        }
    }
}
