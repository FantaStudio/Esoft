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
using Esoft.UI.Main;
using Esoft.Helpers;
using Esoft.Database;

namespace Esoft
{
    public partial class ProfileWindow : EUserWindow
    {
        private bool isShowedForMe = true;
        public ProfileWindow(EUser user,bool ShowedForMe = true) : base(user)
        {
            isShowedForMe = ShowedForMe;
            InitializeComponent();
            UpdateProfileWindow();
        }

        public void ToolBarSetVisible(bool visible)
        {
            if (!visible)
            {
                ToolBar.Visibility = Visibility.Collapsed;
                Height = 200;
            }
            else
            {
                ToolBar.Visibility = Visibility.Visible;
                Height = 250;
            }
        }

        private void LoadUserInfo()
        {
            if(CurrentUser == null)
            {
                NameLabel.Content = "";
                SurnameLabel.Content = "";
                MiddlenameLabel.Content = "";
                EmailLabel.Content = "";
                PhoneOrCommisionLabel.Content = "";
                return;
            }
            if (CurrentUser.UserType == Main.UserType.Agent)
            {
                var agent = Agents.Get(CurrentUser.UserID);
                NameLabel.Content = "Имя: "+agent.Name;
                SurnameLabel.Content = "Фамилия: "+agent.Surname;
                MiddlenameLabel.Content = "Отчество: "+agent.Middlename;
                PhoneOrCommisionLabel.Content = "Доля от сделки: "+agent.DealShare.ToString();
            }
            else
            {
                var client = Clients.Get(CurrentUser.UserID);
                NameLabel.Content = "Имя: " + client.Name;
                SurnameLabel.Content = "Фамилия: " + client.Surname;
                MiddlenameLabel.Content = "Отчество: " + client.Middlename;
                PhoneOrCommisionLabel.Content = "Телефон: "+client.Phone;
                EmailLabel.Content = "Почта: "+client.Email;
            }
        }

        private void UpdateProfileWindow()
        {
            ToolBarSetVisible(isShowedForMe);
            if (CurrentUser == null) return;

            LoadUserInfo();
            if (CurrentUser.UserType == Main.UserType.Agent)
            {
                EmailLabel.Visibility = Visibility.Collapsed;
                ProfileInfo.Rows = 2;
            }
            else
            {
                ProfileInfo.Rows = 3;
                EmailLabel.Visibility = Visibility.Visible;
                Height += 60;
            } 
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isShowedForMe) return;
            if (MessageHelper.Show("Вы точно хотите удалить аккаунт?",MessageHelper.MessageType.warning, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                bool deleted;
                if (CurrentUser.UserType == Main.UserType.Agent)
                {
                    if (Agents.IsExistInOffer(CurrentUser.UserID))
                    {
                        MessageHelper.Show("Вы не можете удалить аккаунт, который связан с предложением",MessageHelper.MessageType.error);
                        return;
                    }
                    else if (Agents.IsExistInDemand(CurrentUser.UserID))
                    {
                        MessageHelper.Show("Вы не можете удалить аккаунт, который связан с потребностью", MessageHelper.MessageType.error);
                        return;
                    }
                    deleted = Agents.Delete(CurrentUser.UserID);
                }
                else
                {
                    if (Clients.IsExistInOffer(CurrentUser.UserID))
                    {
                        MessageHelper.Show("Вы не можете удалить аккаунт, который связан с предложением", MessageHelper.MessageType.error);
                        return;
                    }
                    else if (Clients.IsExistInDemand(CurrentUser.UserID))
                    {
                        MessageHelper.Show("Вы не можете удалить аккаунт, который связан с потребностью", MessageHelper.MessageType.error);
                        return;
                    }
                    deleted = Clients.Delete(CurrentUser.UserID);
                }
                if (deleted)
                {
                    MessageHelper.Show("Вы успешно удалили аккаунт", MessageHelper.MessageType.success);
                    if (Main.IsModal(this))
                        DialogResult = true;
                    Close();
                }
                else
                    MessageHelper.Show("Не удалось удалить аккаунт", MessageHelper.MessageType.error);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isShowedForMe) return;
            if(new ProfileEditWindow(CurrentUser).ShowDialog() == true)
                UpdateProfileWindow();
        }
    }
}
