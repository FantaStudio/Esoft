using Esoft.UI.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Esoft.Helpers;
using Esoft.Database;
using Esoft.UI.Inputs;

namespace Esoft
{
    public partial class ProfileEditWindow : EUserWindow
    {
        public ProfileEditWindow(EUser user) : base(user)
        {
            InitializeComponent();
            UpdateProfileEditWindow();
            LoadUserInfo();
            EInput.AddClearStatusHandler(this);
        }

        private void LoadUserInfo()
        {
            if(CurrentUser == null)
            {
                NameBox.Text = "";
                SurnameBox.Text = "";
                MiddlenameBox.Text = "";
                PhoneOrCommisionBox.Text = "";
                EmailBox.Text = "";
                return;
            }
            if (CurrentUser.UserType == Main.UserType.Agent)
            {
                var agent = Agents.Get(CurrentUser.UserID);
                NameBox.Text = agent.Name;
                SurnameBox.Text = agent.Surname;
                MiddlenameBox.Text = agent.Middlename;
                PhoneOrCommisionBox.Text = agent.DealShare.ToString();
            }
            else
            {
                var client = Clients.Get(CurrentUser.UserID);
                NameBox.Text = client.Name;
                SurnameBox.Text = client.Surname;
                MiddlenameBox.Text = client.Middlename;
                PhoneOrCommisionBox.Text = client.Phone;
                EmailBox.Text = client.Email;
            }
        }

        private void UpdateProfileEditWindow()
        {
            if (CurrentUser == null) return;
            if (CurrentUser.UserType == Main.UserType.Agent)
            {
                PhoneOrCommisionLabel.Content = "Доля от сделок";
                PhoneOrCommisionBox.PlaceHolder = "Доля от сделок";
                EmailLabel.Visibility = Visibility.Collapsed;
                EmailBox.Visibility = Visibility.Collapsed;
                Height = 500;
            }
            else
            {
                PhoneOrCommisionLabel.Content = "Телефон";
                PhoneOrCommisionBox.PlaceHolder = "Телефон";
                EmailLabel.Visibility = Visibility.Visible;
                EmailBox.Visibility = Visibility.Visible;
                Height = 600;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text;
            string surname = SurnameBox.Text;
            string middlename = MiddlenameBox.Text;
            string phoneOrCommission = PhoneOrCommisionBox.Text;
            string email = EmailBox.Text;
            var list = new Dictionary<EUser.EUserInputTypes, EInput>() {
                { EUser.EUserInputTypes.Name, NameBox },
                { EUser.EUserInputTypes.Surname, SurnameBox },
                { EUser.EUserInputTypes.Middlename, MiddlenameBox },
                { EUser.EUserInputTypes.Email, EmailBox },
                { EUser.EUserInputTypes.DealShare, PhoneOrCommisionBox },
                { EUser.EUserInputTypes.Phone, PhoneOrCommisionBox },
            };
            (List<string>, List<EInput>) errors = EUser.CheckerEnteredData(list, CurrentUser.UserType);
            if (errors.Item1.Count > 0)
            {
                foreach (EInput input in errors.Item2)
                    input.EStatus = Statuses.Main.Danger;
                MessageHelper.Show(errors.Item1[0],MessageHelper.MessageType.error);
                return;
            }

            foreach (EInput input in Main.FindVisualChildren<EInput>(this))
                input.EStatus = Statuses.Main.Success;

            bool updated = false;
            if(CurrentUser.UserType == Main.UserType.Agent)
            {
                if (phoneOrCommission == "")
                    updated = Agents.Update(CurrentUser.UserID, surname, name, middlename);
                else
                    updated = Agents.Update(CurrentUser.UserID, surname, name, middlename, int.Parse(phoneOrCommission));
            }
            else
                updated = Clients.Update(CurrentUser.UserID, surname, name, middlename, email, phoneOrCommission);
            if (updated)
            {
                MessageHelper.Show("Вы успешно изменили данные",MessageHelper.MessageType.success);
                DialogResult = true;
            }
            else
                MessageHelper.Show("Не удалось изменить данные",MessageHelper.MessageType.error);
        }
    }
}
