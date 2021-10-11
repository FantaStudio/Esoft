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
using Esoft.Database.Common;

namespace Esoft
{
    public partial class DemandWindow : EUserWindow
    {
        public EDemand CurrentDemand { get; set; }
        public int? RieltorID;
        public int? ClientID;

        public DemandWindow(EUser user, EDemand demand) : base(user)
        {
            CurrentDemand = demand;
            InitializeComponent();
            UpdateDemandWindow();
        }

        public void ToolBarSetVisible(bool visible,bool client)
        {
            if (!visible)
            {
                ToolBar.Visibility = Visibility.Collapsed;
                FindBar.Visibility = Visibility.Collapsed;
                Height = 596;
            }
            else
            {
                if (client)
                {
                    FindBar.Visibility = Visibility.Collapsed;
                    ToolBar.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolBar.Visibility = Visibility.Collapsed;
                    FindBar.Visibility = Visibility.Visible;
                }
                Height = 665;
            }
        }

        private void UpdateDemandWindow()
        {
            TypeLabel.Content = "Тип:";
            RieltorLink.Text = "";
            ClientLink.Text = "";
            MinSquareLabel.Content = "";
            MaxSquareLabel.Content = "";
            MinPriceLabel.Content = "";
            MaxPriceLabel.Content = "";
            MinRoomsLabel.Content = "";
            MaxRoomsLabel.Content = "";
            MinFloorsLabel.Content = "";
            MaxFloorsLabel.Content = "";
            
            if (CurrentDemand == null)
            {
                return;
            }
            string minArea, maxArea, minPrice, maxPrice, minRooms, maxRooms, minFloors, maxFloors,address;
            minArea = maxArea = minPrice = maxPrice = minRooms = maxRooms = minFloors = maxFloors = address = "";
            switch (CurrentDemand.ObjectType)
            {
                case Main.ObjectType.Flat:
                    var flatDemand = ApartmentDemand.Get(CurrentDemand.ID);
                    if (flatDemand == null) return;
                    RieltorID = flatDemand.AgentID;
                    ClientID = flatDemand.ClientID;
                    address = Main.GetAdressString(flatDemand.Address_City, flatDemand.Address_Street, flatDemand.Address_House, flatDemand.Address_Number);
                    minArea = string.IsNullOrEmpty(flatDemand.MinArea.ToString()) ? "не указано" : flatDemand.MinArea.ToString();
                    maxArea = string.IsNullOrEmpty(flatDemand.MaxArea.ToString()) ? "не указано" : flatDemand.MaxArea.ToString();
                    minPrice = string.IsNullOrEmpty(flatDemand.MinPrice.ToString()) ? "не указано" : flatDemand.MinPrice.ToString();
                    maxPrice = string.IsNullOrEmpty(flatDemand.MaxPrice.ToString()) ? "не указано" : flatDemand.MaxPrice.ToString();
                    minRooms = string.IsNullOrEmpty(flatDemand.MinRooms.ToString()) ? "не указано" : flatDemand.MinRooms.ToString();
                    maxRooms = string.IsNullOrEmpty(flatDemand.MaxRooms.ToString()) ? "не указано" : flatDemand.MaxRooms.ToString();
                    minFloors = string.IsNullOrEmpty(flatDemand.MinFloor.ToString()) ? "не указано" : flatDemand.MaxFloor.ToString();
                    maxFloors = string.IsNullOrEmpty(flatDemand.MaxFloor.ToString()) ? "не указано" : flatDemand.MaxFloor.ToString();
                    break;
                case Main.ObjectType.House:
                    var houseDemand = HouseDemand.Get(CurrentDemand.ID);
                    if (houseDemand == null) return;
                    RieltorID = houseDemand.AgentID;
                    ClientID = houseDemand.ClientID;
                    address = Main.GetAdressString(houseDemand.Address_City, houseDemand.Address_Street, houseDemand.Address_House, houseDemand.Address_Number);
                    minArea = string.IsNullOrEmpty(houseDemand.MinArea.ToString()) ? "не указано" : houseDemand.MinArea.ToString();
                    maxArea = string.IsNullOrEmpty(houseDemand.MaxArea.ToString()) ? "не указано" : houseDemand.MaxArea.ToString();
                    minPrice = string.IsNullOrEmpty(houseDemand.MinPrice.ToString()) ? "не указано" : houseDemand.MinPrice.ToString();
                    maxPrice = string.IsNullOrEmpty(houseDemand.MaxPrice.ToString()) ? "не указано" : houseDemand.MaxPrice.ToString();
                    minRooms = string.IsNullOrEmpty(houseDemand.MinRooms.ToString()) ? "не указано" : houseDemand.MinRooms.ToString();
                    maxRooms = string.IsNullOrEmpty(houseDemand.MaxRooms.ToString()) ? "не указано" : houseDemand.MaxRooms.ToString();
                    minFloors = string.IsNullOrEmpty(houseDemand.MinFloors.ToString()) ? "не указано" : houseDemand.MaxFloors.ToString();
                    maxFloors = string.IsNullOrEmpty(houseDemand.MaxFloors.ToString()) ? "не указано" : houseDemand.MaxFloors.ToString();
                    break;
                case Main.ObjectType.Land:
                    var landDemand = LandDemand.Get(CurrentDemand.ID);
                    if (landDemand == null) return;
                    RieltorID = landDemand.AgentID;
                    ClientID = landDemand.ClientID;
                    address = Main.GetAdressString(landDemand.Address_City, landDemand.Address_Street, landDemand.Address_House, landDemand.Address_Number);
                    minArea = string.IsNullOrEmpty(landDemand.MinArea.ToString()) ? "не указано" : landDemand.MinArea.ToString();
                    maxArea = string.IsNullOrEmpty(landDemand.MaxArea.ToString()) ? "не указано" : landDemand.MaxArea.ToString();
                    minPrice = string.IsNullOrEmpty(landDemand.MinPrice.ToString()) ? "не указано" : landDemand.MinPrice.ToString();
                    maxPrice = string.IsNullOrEmpty(landDemand.MaxPrice.ToString()) ? "не указано" : landDemand.MaxPrice.ToString();
                    break;
            }
            AddressLabel.Content = "Адрес: " + address;
            TypeLabel.Content = "Тип: " + Main.ObjectTypeTranslate[CurrentDemand.ObjectType];
            MinPriceLabel.Content = "Мин: " + minPrice;
            MaxPriceLabel.Content = "Макс: " + maxPrice;
            MinSquareLabel.Content = "Мин: " + minArea;
            MaxSquareLabel.Content = "Макс: " + maxArea;
            MinRoomsLabel.Content = "Мин: " + maxRooms;
            MaxRoomsLabel.Content = "Макс: " + minRooms;
            if (CurrentDemand.ObjectType == Main.ObjectType.Flat)
            {
                FloorLabel.Content = "Этаж";
            }
            else
            {
                FloorLabel.Content = "Этажность";
            }
            MinFloorsLabel.Content = "Мин: " + minFloors;
            MaxFloorsLabel.Content = "Макс: " + maxFloors;

            Agents agent = null;
            Clients client = null;
            if(RieltorID != null)
                agent = Agents.Get((int)RieltorID);
            if (ClientID != null)
                client = Clients.Get((int)ClientID);
            RieltorLink.Text = agent == null ? "не указано" : Main.ToFioString(agent.Surname,agent.Name,agent.Middlename);
            ClientLink.Text = client == null ? "не указано" : Main.ToFioString(client.Surname, client.Name, client.Middlename);

            if (ClientID == CurrentUser.UserID && CurrentUser.UserType == Main.UserType.Client)
                ToolBarSetVisible(true, true);
            else if(RieltorID == CurrentUser.UserID && CurrentUser.UserType == Main.UserType.Agent)
                ToolBarSetVisible(true, false);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageHelper.Show("Вы точно хотите удалить потребность?", MessageHelper.MessageType.warning, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if(Deals.IsDemandInDeal(CurrentDemand))
                {
                    MessageHelper.Show("Вы не можете удалить потребность, которая связана со сделкой");
                    return;
                }
                bool deleted = false;
                if(CurrentDemand != null)
                {
                    if (CurrentDemand.ObjectType == Main.ObjectType.Flat)
                        deleted = ApartmentDemand.Delete(CurrentDemand.ID);
                    else if (CurrentDemand.ObjectType == Main.ObjectType.House)
                        deleted = HouseDemand.Delete(CurrentDemand.ID);
                    else if (CurrentDemand.ObjectType == Main.ObjectType.Land)
                        deleted = LandDemand.Delete(CurrentDemand.ID);
                }
                if (deleted)
                    Close();
                else
                    MessageHelper.Show("Не удалось удалить потребность");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDemand == null) return;
            if (new DemandAddEditWindow(CurrentUser, CurrentDemand).ShowDialog() == true)
                UpdateDemandWindow();
        }

        private void RieltorLink_Click(object sender, RoutedEventArgs e)
        {
            if (RieltorID != null)
                new ProfileWindow(new EUser((int)RieltorID, Main.UserType.Agent), false).ShowDialog();
        }

        private void ClientLink_Click(object sender, RoutedEventArgs e)
        {
            if (ClientID != null)
                new ProfileWindow(new EUser((int)ClientID, Main.UserType.Client), false).ShowDialog();
        }

        private void FindDemandButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDemand == null) return;
            var find = new FindWindow(CurrentUser, CurrentDemand);
            if (!find.IsClosed) find.ShowDialog();
        }
    }
}
