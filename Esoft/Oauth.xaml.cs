using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Esoft.UI.Main;
using Esoft.Helpers;
using Esoft.UI.Inputs;
using Esoft.Database;

namespace Esoft
{
    public partial class Oauth : EWindow
    {
        readonly Dictionary<string, int> RieltorSizes = new Dictionary<string, int>()
        {
            {"Login", 540},
            {"Register", 600},
        };

        readonly Dictionary<string, int> ClientSizes = new Dictionary<string, int>()
        {
            {"Login", 540},
            {"Register", 600},
        };

        private Main.UserType choosedUserType;
        private string choosedOauthType;

        public Oauth()
        {
            InitializeComponent();
            OauthTab.SelectionChanged += OauthTab_SelectionChanged;
            UsersTypesMenu.SelectionChanged += Menu_SelectionChanged;
            LoginTabItem.IsSelected = true;
            ClientItem.IsSelected = true;
            EInput.AddClearStatusHandler(this);
        }

        private void RecalculateWindowHeight()
        {
            var sizes = ClientSizes;
            if (choosedUserType == Main.UserType.Agent)
                sizes = RieltorSizes;
            Height = sizes[choosedOauthType];
            FieldsReusingMethod();
        }

        private void FieldsReusingMethod()
        {
            if (choosedUserType == Main.UserType.Agent)
            {
                PhoneOrDealShareBox.PlaceHolder = "Доля от сделки";
                EmailBox.Visibility = Visibility.Collapsed;
                Middlename.Description = "";
            }
            else
            {
                PhoneOrDealShareBox.PlaceHolder = "Телефон";
                EmailBox.Visibility = Visibility.Visible;
                Middlename.Description = "Необязательное поле";
            }
            
        }

        private void LoginUser(string login,string password)
        {
            if (choosedUserType == Main.UserType.Agent)
            {
                Agents agent = Agents.Get(login, password);
                if (agent != null)
                {
                    new MainMenu(new EUser(agent.ID, Main.UserType.Agent)).Show();
                    Close();
                }
                else
                    MessageHelper.Show("Неверный логин или пароль",MessageHelper.MessageType.error);
            }
            else
            {
                Clients client = Clients.Get(login, password);
                if (client != null)
                {
                    new MainMenu(new EUser(client.ID, Main.UserType.Client)).Show();
                    Close();
                }
                else
                    MessageHelper.Show("Неверный логин или пароль",MessageHelper.MessageType.error);
            }
        }

        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersTypesMenu.SelectedItem == ClientItem)
            {
                choosedUserType = Main.UserType.Client;
                RecalculateWindowHeight();
            }
            else if (UsersTypesMenu.SelectedItem == RieltorItem)
            {
                choosedUserType = Main.UserType.Agent;
                RecalculateWindowHeight();
            }
        }

        private void OauthTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LoginTabItem.IsSelected)
                choosedOauthType = "Login";
            else if (RegisterTabItem.IsSelected)
                choosedOauthType = "Register";
            RecalculateWindowHeight();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginLogin.Text;
            string password = LoginPassword.Text;

            var list = new Dictionary<EUser.EUserInputTypes,EInput>() {
                { EUser.EUserInputTypes.Login,LoginLogin },
                { EUser.EUserInputTypes.Password, LoginPassword },
            };
            (List<string>, List<EInput>) errors = EUser.CheckerEnteredData(list, choosedUserType);

            if (errors.Item1.Count > 0)
            {
                foreach (EInput input in errors.Item2)
                    input.EStatus = Statuses.Main.Danger;
                MessageHelper.Show(errors.Item1[0],MessageHelper.MessageType.error);
                return;
            }
            LoginUser(login,password);
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            string login = RegisterLogin.Text;
            string password = RegisterPassword.Text;
            string name = Name.Text;
            string surname = Surname.Text;
            string middlename = Middlename.Text;
            string phoneOrCommission = PhoneOrDealShareBox.Text;
            string email = EmailBox.Text;

            var list = new Dictionary<EUser.EUserInputTypes, EInput>() {
                { EUser.EUserInputTypes.Login,RegisterLogin },
                { EUser.EUserInputTypes.Password, RegisterPassword },
                { EUser.EUserInputTypes.Name, Name },
                { EUser.EUserInputTypes.Surname, Surname },
                { EUser.EUserInputTypes.Middlename, Middlename },
                { EUser.EUserInputTypes.Email, EmailBox },
                { EUser.EUserInputTypes.DealShare, PhoneOrDealShareBox },
                { EUser.EUserInputTypes.Phone, PhoneOrDealShareBox },
            };
            (List<string>, List<EInput>) errors = EUser.CheckerEnteredData(list, choosedUserType);

            if (errors.Item1.Count > 0)
            {
                foreach (EInput input in errors.Item2)
                    input.EStatus = Statuses.Main.Danger;
                MessageHelper.Show(errors.Item1[0],MessageHelper.MessageType.error);
                return;
            }

            bool added = false;
            if(choosedUserType == Main.UserType.Agent)
            {
                if (Agents.Get(login) != null)
                {
                    MessageHelper.Show("Пользователь с таким логином уже зарегистрирован",MessageHelper.MessageType.error);
                    return;
                }
                else
                {
                    if (phoneOrCommission == "")
                        added = Agents.Add(login, password, surname, name, middlename);
                    else
                        added = Agents.Add(login, password, surname, name, middlename, int.Parse(phoneOrCommission));
                }
            }
            else
            {
                if (Clients.Get(login) != null)
                {
                    MessageHelper.Show("Пользователь с таким логином уже зарегистрирован",MessageHelper.MessageType.error);
                    return;
                }
                else
                    added = Clients.Add(login, password, surname, name, middlename, email, phoneOrCommission);
            }
            if (added)
            {
                MessageHelper.Show("Вы успешно зарегистрировались",MessageHelper.MessageType.success);
                LoginUser(login, password);
            }
            else
            {
                MessageHelper.Show("Не удалось зарегистрироваться",MessageHelper.MessageType.error);
            }
        }
    }
}
