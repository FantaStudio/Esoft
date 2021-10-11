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

namespace Esoft
{
    /// <summary>
    /// Логика взаимодействия для ObjectWindow.xaml
    /// </summary>
    public partial class ObjectWindow : EUserWindow
    {
        public ObjectWindow()
        {
            InitializeComponent();
        }

        public ObjectWindow(EUser user, EObject obj) : base(user, obj)
        {
            InitializeComponent();
            UpdateObjectWindow();
        }

        public void ToolBarSetVisible(bool visible)
        {
            if (!visible)
            {
                ToolBar.Visibility = Visibility.Collapsed;
                Height = 315;
            }
            else
            {
                ToolBar.Visibility = Visibility.Visible;
                Height = 380;
            }
        }

        private int? clientID = null;
        private void UpdateObjectWindow()
        {
            if (CurrentObject == null || CurrentObject.ID == -1) return;
            bool isOwner = false;
            switch (CurrentObject.ObjectType)
            {
                case Main.ObjectType.Flat:
                    Apartments flat = Apartments.Get(CurrentObject.ID);
                    clientID = flat.ClientID;
                    AddressLabel.Content = $"Адрес: " + Main.GetAdressString(flat.Address_City, flat.Address_Street, flat.Address_House, flat.Address_Number);
                    CoordinatesLabel.Content = $"Координаты: {flat.Coordinate_latitude},{flat.Coordinate_longitude}";
                    SquareLabel.Content = "Площадь: "+flat.TotalArea;
                    FloorLabel.Content = "Этаж: "+flat.Floor;
                    RoomsLabel.Content = "Кол-во комнат: "+flat.Rooms;
                    RoomsLabel.Visibility = Visibility.Visible;
                    FloorLabel.Visibility = Visibility.Visible;
                    break;
                case Main.ObjectType.House:
                    Houses house = Houses.Get(CurrentObject.ID);
                    clientID = house.ClientID;
                    AddressLabel.Content = $"Адрес: "+ Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number);
                    CoordinatesLabel.Content = $"Координаты: {house.Coordinate_latitude},{house.Coordinate_longitude}";
                    SquareLabel.Content = "Площадь: " + house.TotalArea;
                    FloorLabel.Content = "Этажность: " + house.TotalFloors;
                    RoomsLabel.Content = "Кол-во комнат: " + house.Rooms;
                    RoomsLabel.Visibility = Visibility.Visible;
                    FloorLabel.Visibility = Visibility.Visible;
                    break;

                case Main.ObjectType.Land:
                    Lands land = Lands.Get(CurrentObject.ID);
                    clientID = land.ClientID;
                    AddressLabel.Content = $"Адрес: "+ Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number);
                    CoordinatesLabel.Content = $"Координаты: {land.Coordinate_latitude},{land.Coordinate_longitude}";
                    SquareLabel.Content = "Площадь: " + land.TotalArea;
                    RoomsLabel.Visibility = Visibility.Collapsed;
                    FloorLabel.Visibility = Visibility.Collapsed;
                    break;
            }

            if (clientID == CurrentUser.UserID && CurrentUser.UserType == Main.UserType.Client) isOwner = true;
            ToolBarSetVisible(isOwner);
            RieltorLink.Text = "";
            if (isOwner)
            {
                OwnerLink.Text = "Вы";
            }
            else
            {
                if (clientID == null) OwnerLink.Text = "Неизвестно";
                else
                {
                    Clients client = Clients.Get((int)clientID);
                    OwnerLink.Text = $"{client.Surname} {client.Name} {client.Middlename}";
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageHelper.Show("Вы точно хотите удалить недвижимость?", MessageHelper.MessageType.warning, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (Offers.IsObjectExist(CurrentObject.ID, CurrentObject.ObjectType))
                {
                    MessageHelper.Show("Вы не можете удалить объект, который связан с предложением", MessageHelper.MessageType.error);
                    return;
                }
                bool deleted = false;
                if (CurrentObject.ObjectType == Main.ObjectType.Flat)
                    deleted = Apartments.Delete(CurrentObject.ID);
                else if (CurrentObject.ObjectType == Main.ObjectType.House)
                    deleted = Houses.Delete(CurrentObject.ID);
                else if (CurrentObject.ObjectType == Main.ObjectType.Land)
                    deleted = Lands.Delete(CurrentObject.ID);
                if (deleted)
                {
                    MessageHelper.Show("Вы успешно удалили недвижимость",MessageHelper.MessageType.success);
                    Close();
                }
                else
                {
                    MessageHelper.Show("Не удалось удалить недвижимость",MessageHelper.MessageType.error);
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (new ObjectAddEditWindow(CurrentUser, CurrentObject).ShowDialog() == true)
                UpdateObjectWindow();
        }

        private void OwnerLink_Click(object sender, RoutedEventArgs e)
        {
            if (clientID == null || clientID == CurrentUser.UserID) return;
            new ProfileWindow(new EUser((int)clientID, Main.UserType.Client), false).ShowDialog();
        }
    }
}
