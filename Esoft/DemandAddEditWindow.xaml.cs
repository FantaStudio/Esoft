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
using Esoft.UI.Additional;
using Esoft.Database.Common;

namespace Esoft
{
    public partial class DemandAddEditWindow : EUserWindow
    {
        public EDemand CurrentDemand { get; set; }
        public DemandAddEditWindow(EUser user) : base(user)
        {
            InitializeComponent();
            LoadDemandInfo();
            EInput.AddClearStatusHandler(this);
        }

        public DemandAddEditWindow(EUser user, EDemand demand) : base(user)
        {
            CurrentDemand = demand;
            InitializeComponent();
            LoadDemandInfo();
            EInput.AddClearStatusHandler(this);
        }

        private void LoadDemandInfo()
        {
            if(CurrentDemand != null)
            {
                switch (CurrentDemand.ObjectType)
                {
                    case Main.ObjectType.Flat:
                        var flatDemand = ApartmentDemand.Get(CurrentDemand.ID);
                        if (flatDemand == null) return;
                        ComboBoxPull.ClientsPull(ClientsBox, flatDemand.ClientID);
                        ComboBoxPull.RieltorsPull(RieltorsBox, flatDemand.AgentID);
                        ComboBoxPull.EObjectTypePull(ObjectTypeBox, Main.ObjectType.Flat);
                        CityBox.Text = flatDemand.Address_City;
                        StreetBox.Text = flatDemand.Address_Street;
                        HouseBox.Text = flatDemand.Address_House.ToString();
                        FlatBox.Text = flatDemand.Address_Number.ToString();
                        MinPriceBox.Text = flatDemand.MinPrice.ToString();
                        MaxPriceBox.Text = flatDemand.MaxPrice.ToString();
                        MinSquareBox.Text = flatDemand.MinArea.ToString();
                        MaxSquareBox.Text = flatDemand.MaxArea.ToString();
                        MinFloorsBox.Text = flatDemand.MinFloor.ToString();
                        MaxFloorsBox.Text = flatDemand.MaxFloor.ToString();
                        MinRoomsBox.Text = flatDemand.MinRooms.ToString();
                        MaxRoomsBox.Text = flatDemand.MaxRooms.ToString();
                        break;
                    case Main.ObjectType.House:
                        var houseDemand = HouseDemand.Get(CurrentDemand.ID);
                        if (houseDemand == null) return;
                        ComboBoxPull.ClientsPull(ClientsBox, houseDemand.ClientID);
                        ComboBoxPull.RieltorsPull(RieltorsBox, houseDemand.AgentID);
                        ComboBoxPull.EObjectTypePull(ObjectTypeBox, Main.ObjectType.House);
                        CityBox.Text = houseDemand.Address_City;
                        StreetBox.Text = houseDemand.Address_Street;
                        HouseBox.Text = houseDemand.Address_House.ToString();
                        FlatBox.Text = houseDemand.Address_Number.ToString();
                        MinPriceBox.Text = houseDemand.MinPrice.ToString();
                        MaxPriceBox.Text = houseDemand.MaxPrice.ToString();
                        MinSquareBox.Text = houseDemand.MinArea.ToString();
                        MaxSquareBox.Text = houseDemand.MaxArea.ToString();
                        MinFloorsBox.Text = houseDemand.MinFloors.ToString();
                        MaxFloorsBox.Text = houseDemand.MaxFloors.ToString();
                        MinRoomsBox.Text = houseDemand.MinRooms.ToString();
                        MaxRoomsBox.Text = houseDemand.MaxRooms.ToString();
                        break;

                    case Main.ObjectType.Land:
                        var landDemand = LandDemand.Get(CurrentDemand.ID);
                        if (landDemand == null) return;
                        ComboBoxPull.ClientsPull(ClientsBox, landDemand.ClientID);
                        ComboBoxPull.RieltorsPull(RieltorsBox, landDemand.AgentID);
                        ComboBoxPull.EObjectTypePull(ObjectTypeBox, Main.ObjectType.Land);
                        CityBox.Text = landDemand.Address_City;
                        StreetBox.Text = landDemand.Address_Street;
                        HouseBox.Text = landDemand.Address_House.ToString();
                        FlatBox.Text = landDemand.Address_Number.ToString();
                        MinPriceBox.Text = landDemand.MinPrice.ToString();
                        MaxPriceBox.Text = landDemand.MaxPrice.ToString();
                        MinSquareBox.Text = landDemand.MinArea.ToString();
                        MaxSquareBox.Text = landDemand.MaxArea.ToString();
                        break;
                }
                ObjectTypeBox.IsEnabled = false;
                SaveButton.Content = "Сохранить";
                Title = "Редактирование потребности";
            }
            else
            {
                ComboBoxPull.ClientsPull(ClientsBox,CurrentUser.UserID);
                ComboBoxPull.RieltorsPull(RieltorsBox);
                ComboBoxPull.EObjectTypePull(ObjectTypeBox);
                SaveButton.Content = "Добавить";
                Title = "Добавление потребности";
            }
        }

        private void UpdateDemandAddEditWindow()
        {
            ComboBoxEObjectTypeItem item = ObjectTypeBox.SelectedItem as ComboBoxEObjectTypeItem;
            switch (item.ObjectType)
            {
                case Main.ObjectType.Flat:
                    {
                        FloorsLabel.Content = "Этаж";
                        MinFloorsBox.Description = "Минимальный";
                        MaxFloorsBox.Description = "Максимальный";
                        FloorsGrid.Visibility = Visibility.Visible;
                        RoomsGrid.Visibility = Visibility.Visible;
                        RoomsLabel.Visibility = Visibility.Visible;
                        FloorsLabel.Visibility = Visibility.Visible;
                        break;
                    }
                case Main.ObjectType.House:
                    {
                        FloorsLabel.Content = "Этажность";
                        MinFloorsBox.Description = "Минимальная";
                        MaxFloorsBox.Description = "Максимальная";
                        FloorsGrid.Visibility = Visibility.Visible;
                        RoomsGrid.Visibility = Visibility.Visible;
                        RoomsLabel.Visibility = Visibility.Visible;
                        FloorsLabel.Visibility = Visibility.Visible;
                        break;
                    }
                case Main.ObjectType.Land:
                    {
                        RoomsLabel.Visibility = Visibility.Collapsed;
                        FloorsLabel.Visibility = Visibility.Collapsed;
                        FloorsGrid.Visibility = Visibility.Collapsed;
                        RoomsGrid.Visibility = Visibility.Collapsed;
                        break;
                    }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(ClientsBox.SelectedItem == null)
            {
                MessageHelper.Show("Выберите клиента");
                return;
            }
            if(RieltorsBox.SelectedItem == null)
            {
                MessageHelper.Show("Выберите риэлтора");
                return;
            }
            if (ObjectTypeBox.SelectedItem == null)
            {
                MessageHelper.Show("Выберите тип недвижимости");
                return;
            }

            ComboBoxEuserItem client = ClientsBox.SelectedItem as ComboBoxEuserItem;
            ComboBoxEuserItem rieltor = RieltorsBox.SelectedItem as ComboBoxEuserItem;
            ComboBoxEObjectTypeItem objectType = ObjectTypeBox.SelectedItem as ComboBoxEObjectTypeItem;

            List<EInput> errorInputs = new List<EInput>();
            List<string> errorMessages = new List<string>();
            int? minPrice, maxPrice,minFloors,maxFloors,minRooms,maxRooms;
            double? minSquare, maxSquare;
            minPrice = Main.GetInt(MinPriceBox.Text);
            maxPrice = Main.GetInt(MaxPriceBox.Text);
            minFloors = Main.GetInt(MinFloorsBox.Text);
            maxFloors = Main.GetInt(MaxFloorsBox.Text);
            minRooms = Main.GetInt(MinRoomsBox.Text);
            maxRooms = Main.GetInt(MaxRoomsBox.Text);
            minSquare = Main.GetDouble(MinSquareBox.Text);
            maxSquare = Main.GetDouble(MaxSquareBox.Text);
            if(!string.IsNullOrEmpty(MinPriceBox.Text) && minPrice == null)
            {
                errorInputs.Add(MinPriceBox);
                errorMessages.Add("Минимальная цена должна быть числом!");
            }
            if (minPrice != null && minPrice < 0)
            {
                errorInputs.Add(MinPriceBox);
                errorMessages.Add("Минимальная цена не может быть отрицательной");
            }
            if (!string.IsNullOrEmpty(MaxPriceBox.Text) && maxPrice == null)
            {
                errorInputs.Add(MaxPriceBox);
                errorMessages.Add("Максимальная цена должна быть числом!");
            }
            if (maxPrice != null && maxPrice < 0)
            {
                errorInputs.Add(MaxPriceBox);
                errorMessages.Add("Максимальная цена не может быть отрицательной");
            }
            if(minPrice != null && maxPrice != null && !Main.IsNumbersLikeRange((int)minPrice, (int)maxPrice))
            {
                errorInputs.Add(MinPriceBox);
                errorInputs.Add(MaxPriceBox);
                errorMessages.Add("Максимальная цена не может быть меньше минимальной.");
            }
            if (!string.IsNullOrEmpty(MinSquareBox.Text) && minSquare == null)
            {
                errorInputs.Add(MinSquareBox);
                errorMessages.Add("Минимальная площадь должна быть числом!");
            }
            if (minSquare != null && minSquare < 0)
            {
                errorInputs.Add(MinSquareBox);
                errorMessages.Add("Минимальная площадь не может быть отрицательная");
            }
            if (!string.IsNullOrEmpty(MaxSquareBox.Text) && maxSquare == null)
            {
                errorInputs.Add(MaxSquareBox);
                errorMessages.Add("Максимальная площадь должна быть числом!");
            }
            if (maxSquare != null && maxSquare < 0)
            {
                errorInputs.Add(MaxSquareBox);
                errorMessages.Add("Минимальная площадь не может быть отрицательная");
            }
            if (minSquare != null && maxSquare != null && !Main.IsNumbersLikeRange((int)minSquare, (int)maxSquare))
            {
                errorInputs.Add(MinSquareBox);
                errorInputs.Add(MaxSquareBox);
                errorMessages.Add("Максимальная площадь не может быть меньше минимальной.");
            }
            if (!string.IsNullOrEmpty(MinFloorsBox.Text) && minFloors == null)
            {
                errorInputs.Add(MinFloorsBox);
                errorMessages.Add("Мин.Этаж должен быть числом!");
            }
            if (minFloors != null && minFloors < 0)
            {
                errorInputs.Add(MinFloorsBox);
                if(objectType.ObjectType == Main.ObjectType.House)
                    errorMessages.Add("Минимальная этажность не может быть отрицательная");
                else
                    errorMessages.Add("Минимальный этаж не может быть отрицательным");
            }
            if (!string.IsNullOrEmpty(MaxFloorsBox.Text) && maxFloors == null)
            {
                errorInputs.Add(MaxFloorsBox);
                errorMessages.Add("Макс.Этаж должен быть числом!");
            }
            if (maxFloors != null && maxFloors < 0)
            {
                errorInputs.Add(MinFloorsBox);
                if (objectType.ObjectType == Main.ObjectType.House)
                    errorMessages.Add("Минимальная этажность не может быть отрицательная");
                else
                    errorMessages.Add("Максимальный этаж не может быть отрицательным");
            }
            if (minFloors != null && maxFloors != null && !Main.IsNumbersLikeRange((int)minFloors, (int)maxFloors))
            {
                errorInputs.Add(MinFloorsBox);
                errorInputs.Add(MaxFloorsBox);
                if (objectType.ObjectType == Main.ObjectType.House)
                    errorMessages.Add("Максимальная этажность не может быть меньше минимальной.");
                else
                    errorMessages.Add("Максимальный этаж не может быть меньше минимального.");
            }
            if (!string.IsNullOrEmpty(MinRoomsBox.Text) && minRooms == null)
            {
                errorInputs.Add(MinRoomsBox);
                errorMessages.Add("Мин.Кол-во комнат должно быть числом!");
            }
            if (minRooms != null && minRooms < 0)
            {
                errorInputs.Add(MinRoomsBox);
                errorMessages.Add("Минимальное кол-во комнат не может быть отрицательным.");
            }
            if (maxRooms != null && maxRooms < 0)
            {
                errorInputs.Add(MaxRoomsBox);
                errorMessages.Add("Максимальное кол-во комнат не может быть отрицательным.");
            }
            if (minRooms != null && maxRooms != null && !Main.IsNumbersLikeRange((int)minRooms, (int)maxRooms))
            {
                errorInputs.Add(MinRoomsBox);
                errorInputs.Add(MaxRoomsBox);
                errorMessages.Add("Максимальное кол-во комнат не может быть меньше минимального.");
            }
            if(errorMessages.Count > 0)
            {
                foreach (EInput input in errorInputs)
                    input.EStatus = Statuses.Main.Danger;
                MessageHelper.Show(errorMessages[0]);
                return;
            }

            foreach (EInput input in Main.FindVisualChildren<EInput>(this))
                input.EStatus = Statuses.Main.Success;

            bool result = false;
            if(CurrentDemand == null)
            {
                if (objectType.ObjectType == Main.ObjectType.Flat)
                    result = ApartmentDemand.Add(rieltor.UserID, client.UserID,
                        CityBox.Text, StreetBox.Text, Main.GetInt(HouseBox.Text), Main.GetInt(FlatBox.Text),
                        minPrice, maxPrice, minSquare, maxSquare, minRooms, maxRooms, minFloors, maxFloors);
                else if(objectType.ObjectType == Main.ObjectType.House)
                    result = HouseDemand.Add(rieltor.UserID, client.UserID,
                        CityBox.Text, StreetBox.Text, Main.GetInt(HouseBox.Text), Main.GetInt(FlatBox.Text),
                        minPrice, maxPrice, minSquare, maxSquare, minRooms, maxRooms, minFloors, maxFloors);
                else if (objectType.ObjectType == Main.ObjectType.Land)
                    result = LandDemand.Add(rieltor.UserID, client.UserID, objectType.ObjectType,
                       CityBox.Text, StreetBox.Text, Main.GetInt(HouseBox.Text), Main.GetInt(FlatBox.Text),
                       minPrice, maxPrice, minSquare, maxSquare);
            }
            else
            {
                if (objectType.ObjectType == Main.ObjectType.Flat)
                    result = ApartmentDemand.Update(CurrentDemand.ID,rieltor.UserID, client.UserID,
                        CityBox.Text, StreetBox.Text, Main.GetInt(HouseBox.Text), Main.GetInt(FlatBox.Text),
                        minPrice, maxPrice, minSquare, maxSquare, minRooms, maxRooms, minFloors, maxFloors);
                else if (objectType.ObjectType == Main.ObjectType.House)
                    result = HouseDemand.Update(CurrentDemand.ID,rieltor.UserID, client.UserID,
                        CityBox.Text, StreetBox.Text, Main.GetInt(HouseBox.Text), Main.GetInt(FlatBox.Text),
                        minPrice, maxPrice, minSquare, maxSquare, minRooms, maxRooms, minFloors, maxFloors);
                else if (objectType.ObjectType == Main.ObjectType.Land)
                    result = LandDemand.Update(CurrentDemand.ID,rieltor.UserID, client.UserID,
                       CityBox.Text, StreetBox.Text, Main.GetInt(HouseBox.Text), Main.GetInt(FlatBox.Text),
                       minPrice, maxPrice, minSquare, maxSquare);
            }
            if (result)
            {
                MessageHelper.Show(CurrentDemand == null ? "Потребность успешно добавлена" : "Потребность успешно отредактирована",MessageHelper.MessageType.success);
                DialogResult = true;
            }
            else
            {
                MessageHelper.Show(CurrentDemand == null ? "Не удалось добавить потребность" : "Не удалось отредактировать потребность", MessageHelper.MessageType.error);
                DialogResult = false;
            }
        }

        private void ObjectTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateDemandAddEditWindow();
    }
}
