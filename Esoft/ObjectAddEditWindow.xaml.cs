using Esoft.Database;
using Esoft.Helpers;
using Esoft.UI.Inputs;
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
using Esoft.UI.Additional;

namespace Esoft
{
    
    public partial class ObjectAddEditWindow : EUserWindow
    {
        public bool IsAddWindow { get; set; }

        public ObjectAddEditWindow(EUser user) : base(user)
        {
            IsAddWindow = true;
            InitializeComponent();
            LoadComboboxItems();
            UpdateObjectAddEditWindow();
            LoadObjectInfo();
            EInput.AddClearStatusHandler(this);
        }

        public ObjectAddEditWindow(EUser user, EObject obj) : base(user, obj)
        { 
            IsAddWindow = false;
            InitializeComponent();
            LoadComboboxItems();
            UpdateObjectAddEditWindow();
            LoadObjectInfo();
            EInput.AddClearStatusHandler(this);
        }

        private void LoadComboboxItems()
        {
            if(CurrentObject != null)
                ComboBoxPull.EObjectTypePull(ObjectTypeBox, CurrentObject.ObjectType);
            else
                ComboBoxPull.EObjectTypePull(ObjectTypeBox);
        }

        private void LoadObjectInfo()
        {
            if (IsAddWindow)
            {
                SaveButton.Content = "Добавить";
                Title = "Добавление объекта";
            }
            else
            {
                ObjectTypeBox.IsEnabled = false;
                SaveButton.Content = "Сохранить";
                Title = "Редактирование объекта";
            }

            if (CurrentObject == null)
            {
                LongtitudeBox.Text = "";
                LatitudeBox.Text = "";
                FloorBox.Text = "";
                RoomsBox.Text = "";
                SquareBox.Text = "";
                return;
            }
            switch (CurrentObject.ObjectType)
            {
                case Main.ObjectType.Flat:
                    Apartments curFlat = Apartments.Get(CurrentObject.ID);
                    if (curFlat == null) return;
                    CityBox.Text = curFlat.Address_City;
                    StreetBox.Text = curFlat.Address_Street;
                    HouseBox.Text = curFlat.Address_House.ToString();
                    FlatBox.Text = curFlat.Address_Number.ToString();
                    LatitudeBox.Text = curFlat.Coordinate_latitude.ToString();
                    LongtitudeBox.Text = curFlat.Coordinate_longitude.ToString();
                    FloorBox.Text = curFlat.Floor.ToString();
                    SquareBox.Text = curFlat.TotalArea.ToString();
                    RoomsBox.Text = curFlat.Rooms.ToString();
                    break;
                case Main.ObjectType.House:
                    Houses curHouse = Houses.Get(CurrentObject.ID);
                    if (curHouse == null) return;
                    CityBox.Text = curHouse.Address_City;
                    StreetBox.Text = curHouse.Address_Street;
                    HouseBox.Text = curHouse.Address_House.ToString();
                    FlatBox.Text = curHouse.Address_Number.ToString();
                    LatitudeBox.Text = curHouse.Coordinate_latitude.ToString();
                    LongtitudeBox.Text = curHouse.Coordinate_longitude.ToString();
                    FloorBox.Text = curHouse.TotalFloors.ToString();
                    SquareBox.Text = curHouse.TotalArea.ToString();
                    break;
                case Main.ObjectType.Land:
                    Lands curLand = Lands.Get(CurrentObject.ID);
                    if (curLand == null) return;
                    CityBox.Text = curLand.Address_City;
                    StreetBox.Text = curLand.Address_Street;
                    HouseBox.Text = curLand.Address_House.ToString();
                    FlatBox.Text = curLand.Address_Number.ToString();
                    LatitudeBox.Text = curLand.Coordinate_latitude.ToString();
                    LongtitudeBox.Text = curLand.Coordinate_longitude.ToString();
                    SquareBox.Text = curLand.TotalArea.ToString();
                    break;
            }
        }
        private bool AddOrEditObject(bool add = true)
        {
            Main.ObjectType objectType = (ObjectTypeBox.SelectedItem as ComboBoxEObjectTypeItem).ObjectType;
            string city = CityBox.Text;
            string street = StreetBox.Text;
            int? house = Main.GetInt(HouseBox.Text);
            int? flat = Main.GetInt(FlatBox.Text);
            double latitude = double.Parse(LatitudeBox.Text);
            double longtitude = double.Parse(LongtitudeBox.Text);
            int floor = int.Parse(FloorBox.Text);
            int rooms = int.Parse(RoomsBox.Text);
            double square = double.Parse(SquareBox.Text);
            bool result = false;
            switch (objectType)
            {
                case Main.ObjectType.Flat:
                    if (add)
                        result = Apartments.Add(CurrentUser.UserID, city, latitude, longtitude, square, floor, rooms, street, house, flat);
                    else
                        result = Apartments.Update(CurrentObject.ID, city, latitude, longtitude, square, floor, rooms, street, house, flat);
                    break;
                case Main.ObjectType.House:
                    if (add)
                        result = Houses.Add(CurrentUser.UserID, city, latitude, longtitude, floor, square, rooms, street, house, flat);
                    else
                        result = Houses.Update(CurrentObject.ID, city, latitude, longtitude, floor, square, rooms, street, house, flat);
                    break;
                case Main.ObjectType.Land:
                    if (add)
                        result = Lands.Add(CurrentUser.UserID, city, latitude, longtitude, square, street, house, flat);
                    else
                        result = Lands.Update(CurrentObject.ID, city, latitude, longtitude, square, street, house, flat);
                    break;
            }
            return result;
        }
        private void UpdateObjectAddEditWindow()
        {
            var item = ObjectTypeBox.SelectedItem as ComboBoxEObjectTypeItem;
            switch (item.ObjectType)
            {
                case Main.ObjectType.Flat:
                {
                    FloorLabel.Content = "Этаж*";
                    FloorBox.PlaceHolder = "Этаж";
                    FloorLabel.Visibility = Visibility.Visible;
                    FloorBox.Visibility = Visibility.Visible;
                    RoomsLabel.Visibility = Visibility.Visible;
                    RoomsBox.Visibility = Visibility.Visible;
                    break;
                }
                case Main.ObjectType.House:
                {
                    FloorLabel.Content = "Этажность*";
                    FloorBox.PlaceHolder = "Этажность";
                    FloorLabel.Visibility = Visibility.Visible;
                    FloorBox.Visibility = Visibility.Visible;
                    RoomsLabel.Visibility = Visibility.Visible;
                    RoomsBox.Visibility = Visibility.Visible;
                    break;
                }
                case Main.ObjectType.Land:
                {
                    FloorLabel.Visibility = Visibility.Collapsed;
                    FloorBox.Visibility = Visibility.Collapsed;
                    RoomsLabel.Visibility = Visibility.Collapsed;
                    RoomsBox.Visibility = Visibility.Collapsed;
                    break;
                }
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(ObjectTypeBox.SelectedItem == null)
            {
                MessageHelper.Show("Выберите тип недвижимости", MessageHelper.MessageType.error);
                return;
            }
            Main.ObjectType objectType = (ObjectTypeBox.SelectedItem as ComboBoxEObjectTypeItem).ObjectType;
            var list = new Dictionary<EObject.EObjectInputTypes, EInput>() {
                { EObject.EObjectInputTypes.City, CityBox },
                { EObject.EObjectInputTypes.Street, StreetBox },
                { EObject.EObjectInputTypes.House, HouseBox },
                { EObject.EObjectInputTypes.Number, FlatBox },
                { EObject.EObjectInputTypes.Latitude, LatitudeBox },
                { EObject.EObjectInputTypes.Longtitude, LongtitudeBox },
                { EObject.EObjectInputTypes.Floor, FloorBox },
                { EObject.EObjectInputTypes.Rooms, RoomsBox },
                { EObject.EObjectInputTypes.Square, SquareBox },
            };

            (List<string>, List<EInput>) errors = EObject.CheckerEnteredData(list, objectType);
            if (errors.Item1.Count > 0)
            {
                foreach (EInput input in errors.Item2)
                    input.EStatus = Statuses.Main.Danger;
                MessageHelper.Show(errors.Item1[0],MessageHelper.MessageType.error);
                return;
            }

            foreach (EInput input in Main.FindVisualChildren<EInput>(this))
                input.EStatus = Statuses.Main.Success;

            if (AddOrEditObject(IsAddWindow))
            {
                MessageHelper.Show("Недвижимость успешно " + (IsAddWindow ? "добавлена" : "отредактирована"),MessageHelper.MessageType.success);
                DialogResult = true;
            }
            else
            {
                MessageHelper.Show("Не удалось " + (IsAddWindow ? "добавить" : "отредактировать") + " недвижимость",MessageHelper.MessageType.error);
                DialogResult = false;
            }
        }
        private void ObjectTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateObjectAddEditWindow();
    }
}
