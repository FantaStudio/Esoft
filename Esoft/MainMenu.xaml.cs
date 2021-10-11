using Esoft.Database;
using Esoft.Database.Common;
using Esoft.Helpers;
using Esoft.UI.Cards;
using Esoft.UI.Main;
using MaterialDesignThemes.Wpf;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Esoft
{
    public partial class MainMenu : EUserWindow
    {
        public MainMenu()
        {
            InitializeComponent();
            CurrentUser = new EUser(2, Main.UserType.Client);
            SetupMenu();
        }

        public MainMenu(EUser user) : base(user)
        {
            InitializeComponent();
            SetupMenu();
        }

        // Общее
        private void SetupMenu()
        {
            if (CurrentUser == null) return;
            TypeLabel.Content = "Вы вошли как: " + Main.UsertTypeTranslate[CurrentUser.UserType];
            if (CurrentUser.UserType == Main.UserType.Client)
            {
                Clients client = Clients.Get(CurrentUser.UserID);
                LoginLabel.Content = "Ваш логин: " + client.Login;
                ClientObjects.Visibility = Visibility.Visible;
                addDealButton.Visibility = Visibility.Collapsed;
                LoadDistricts();
            }
            else
            {
                Agents agent = Agents.Get(CurrentUser.UserID);
                LoginLabel.Content = "Ваш логин: " + agent.Login;
                MenuTab.SelectedIndex = 1;
                ClientObjects.Visibility = Visibility.Collapsed;
                addDealButton.Visibility = Visibility.Visible;
                addOfferButton.Visibility = Visibility.Collapsed;
                addDemandButton.Visibility = Visibility.Collapsed;
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            bool? result = new ProfileWindow(CurrentUser, true).ShowDialog();
            if (result == true)
                Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            new Oauth().Show();
            Close();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            TabItem item = (sender as TabControl).SelectedItem as TabItem;
            if (item == ClientObjects)
                LoadObjects();
            else if (item == ClientOffers)
                LoadOffers();
            else if (item == ClientDemands)
                LoadDemands();
            else if (item == DealsItem)
                LoadDeals();
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            if (sender is EObjectCard)
            {
                var card = (EObjectCard)sender;
                new ObjectWindow(new EUser(2, Main.UserType.Client), card.CardObject).ShowDialog();
                LoadObjects();
            }else if(sender is EOfferCard)
            {
                var card = (EOfferCard)sender;
                new OfferWindow(CurrentUser,card.OfferID).ShowDialog();
                LoadOffers();
            }else if(sender is EDemandCard)
            {
                var card = (EDemandCard)sender;
                new DemandWindow(CurrentUser,card.Demand).ShowDialog();
                LoadDemands();
            }else if(sender is EDealCard)
            {
                var card = (EDealCard)sender;
                new DealsWindow(CurrentUser, card.DealID).ShowDialog();
                LoadDeals();
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var viewer = sender as ScrollViewer;
            if (viewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
            {
                stackSearchFilter.Margin = new Thickness(10, 0, 25, 0);
                filterObjectButton.Margin = new Thickness(0, 0, 25, 60);
                addObjectButton.Margin = new Thickness(0, 0, 25, 0);
                addOfferButton.Margin = new Thickness(0, 0, 25, 0);
                addDemandButton.Margin = new Thickness(0, 0, 25, 0);
            }
            else
            {
                stackSearchFilter.Margin = new Thickness(10, 0, 0, 0);
                filterObjectButton.Margin = new Thickness(0, 0, 0, 60);
                addObjectButton.Margin = new Thickness(0, 0, 0, 0);
                addOfferButton.Margin = new Thickness(0, 0, 0, 0);
                addDemandButton.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        // Недвижимость
        private List<Districts> allDistricts = new List<Districts>();
        private void LoadDistricts()
        {
            allDistricts = Districts.GetAll();
            DistrictsBox.Items.Add("Все районы");
            DistrictsBox.SelectedIndex = 0;
            foreach(Districts district in allDistricts)
            {
                DistrictsBox.Items.Add(district.Name);
            }
        }

        private void LoadObjects()
        {
            if (CurrentUser == null) return;
            objectsList.Children.Clear();

            List<Apartments> aps = Apartments.GetAll(CurrentUser.UserID);
            List<Houses> houses = Houses.GetAll(CurrentUser.UserID);
            List<Lands> lands = Lands.GetAll(CurrentUser.UserID);
            List<EObjectCard> cards = new List<EObjectCard>();

            List<Point> districtPoints = new List<Point>();
            if(DistrictsBox.SelectedIndex > 0 && allDistricts.Count > DistrictsBox.SelectedIndex)
            {
                districtPoints = Districts.AreaToPoints(allDistricts[DistrictsBox.SelectedIndex].Area);
            }
            foreach(Apartments apart in aps)
            {
                if(districtPoints.Count > 0)
                {
                    if (!Districts.IsInPloygon(districtPoints, new Point(apart.Coordinate_latitude, apart.Coordinate_longitude)))
                        return;
                }
                cards.Add(new EObjectCard() 
                { 
                    EObjectAdress = Main.GetAdressString(apart.Address_City,apart.Address_Street,apart.Address_House,apart.Address_Number),
                    EObjectType = Main.ObjectTypeTranslate[Main.ObjectType.Flat],
                    CardObject = new EObject() { ID = apart.ID, ObjectType = Main.ObjectType.Flat},
                    Style = FindResource("EObjectCard") as Style,
                    Margin = new Thickness(10, 0, 0, 15),
                });
            }
            foreach(Houses house in houses)
            {
                if (districtPoints.Count > 0)
                {
                    if (!Districts.IsInPloygon(districtPoints, new Point(house.Coordinate_latitude, house.Coordinate_longitude)))
                        return;
                }
                cards.Add(new EObjectCard()
                {
                    EObjectAdress = Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number),
                    EObjectType = Main.ObjectTypeTranslate[Main.ObjectType.House],
                    CardObject = new EObject() { ID = house.ID, ObjectType = Main.ObjectType.House },
                    Style = FindResource("EObjectCard") as Style,
                    Margin = new Thickness(10, 0, 0, 15),
                });
            }
            foreach (Lands land in lands)
            {
                if (districtPoints.Count > 0)
                {
                    if (!Districts.IsInPloygon(districtPoints, new Point(land.Coordinate_latitude, land.Coordinate_longitude)))
                        return;
                }
                cards.Add(new EObjectCard()
                {
                    EObjectAdress = Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number),
                    EObjectType = Main.ObjectTypeTranslate[Main.ObjectType.Land],
                    CardObject = new EObject() { ID = land.ID, ObjectType = Main.ObjectType.Land },
                    Style = FindResource("EObjectCard") as Style,
                    Margin = new Thickness(10, 0, 0, 15),
                });
            }
            foreach (EObjectCard card in cards) 
            {
                objectsList.Children.Add(card);
                card.MouseDown += Card_MouseDown;
            }
        }

        private void FilterObjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (stackSearchFilter.Visibility == Visibility.Collapsed)
                stackSearchFilter.Visibility = Visibility.Visible;
            else stackSearchFilter.Visibility = Visibility.Collapsed;
        }

        private void AddObjectButton_Click(object sender, RoutedEventArgs e)
        {
            new ObjectAddEditWindow(CurrentUser).ShowDialog();
            LoadObjects();
        }

        private void DistrictsBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadObjects();

        // Предложения
        private void LoadOffers()
        {
            if (CurrentUser == null) return;
            offersList.Children.Clear();
            List<Offers> offers = new List<Offers>();
            if (CurrentUser.UserType == Main.UserType.Client)
                offers = Offers.GetClientAll(CurrentUser.UserID);
            else
                offers = Offers.GetAgentAll(CurrentUser.UserID);
            foreach(var offer in offers)
            {
                string address = "";
                switch (offer.ObjectType)
                {
                    case Main.ObjectType.Flat:
                        Apartments flat = Apartments.Get(offer.ObjectID);
                        if (flat == null) return;
                        address = Main.GetAdressString(flat.Address_City, flat.Address_Street, flat.Address_House, flat.Address_Number);
                        break;
                    case Main.ObjectType.House:
                        Houses house = Houses.Get(offer.ObjectID);
                        if (house == null) return;
                        address = Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number);
                        break;
                    case Main.ObjectType.Land:
                        Lands land = Lands.Get(offer.ObjectID);
                        if (land == null) return;
                        address = Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number);
                        break;
                }
                var card = new EOfferCard(offer.Price, offer.ID, Main.ObjectTypeTranslate[offer.ObjectType], address)
                {
                    Style = FindResource("EOfferCard") as Style,
                    Margin = new Thickness(10, 0, 0, 15),
                };
                offersList.Children.Add(card);
                card.MouseDown += Card_MouseDown;
            }
        }
        private void AddOfferButton_Click(object sender, RoutedEventArgs e)
        {
            new OfferAddEditWindow(CurrentUser).ShowDialog();
            LoadOffers();
        }

        // Потребности
        private void LoadDemands()
        {
            if (CurrentUser == null) return;
            demandsList.Children.Clear();

            List<ApartmentDemand> aps = new List<ApartmentDemand>();
            List<HouseDemand> houses = new List<HouseDemand>();
            List<LandDemand> lands = new List<LandDemand>();

            if (CurrentUser.UserType == Main.UserType.Client)
            {
                aps = ApartmentDemand.GetClientAll(CurrentUser.UserID);
                houses = HouseDemand.GetClientAll(CurrentUser.UserID);
                lands = LandDemand.GetClientAll(CurrentUser.UserID);
            }
            else
            {
                aps = ApartmentDemand.GetAgentAll(CurrentUser.UserID);
                houses = HouseDemand.GetAgentAll(CurrentUser.UserID);
                lands = LandDemand.GetAgentAll(CurrentUser.UserID);
            }
            List<EDemandCard> cards = new List<EDemandCard>();

            foreach (ApartmentDemand apart in aps)
            {
                cards.Add(new EDemandCard(){
                    Demand = new EDemand() { ID = apart.ID, ObjectType = Main.ObjectType.Flat},
                    EObjectAddress = Main.GetAdressString(apart.Address_City,apart.Address_Street,apart.Address_House,apart.Address_Number),
                    EObjectType = Main.ObjectTypeTranslate[Main.ObjectType.Flat],
                    MinPrice = apart.MinPrice,
                    MaxPrice = apart.MaxPrice,
                    MinSquare = apart.MinArea,
                    MaxSquare = apart.MinArea,
                    Style = FindResource("EDemandCard") as Style,
                    Margin = new Thickness(10, 0, 0, 15),
                });
            }
            foreach (HouseDemand house in houses)
            {
                cards.Add(new EDemandCard()
                {
                    Demand = new EDemand() { ID = house.ID, ObjectType = Main.ObjectType.House },
                    EObjectAddress = Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number),
                    EObjectType = Main.ObjectTypeTranslate[Main.ObjectType.House],
                    MinPrice = house.MinPrice,
                    MaxPrice = house.MaxPrice,
                    MinSquare = house.MinArea,
                    MaxSquare = house.MaxArea,
                    Style = FindResource("EDemandCard") as Style,
                    Margin = new Thickness(10, 0, 0, 15),
                });
            }
            foreach (LandDemand land in lands)
            {
                cards.Add(new EDemandCard()
                {
                    Demand = new EDemand() { ID = land.ID, ObjectType = Main.ObjectType.Land },
                    EObjectAddress = Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number),
                    EObjectType = Main.ObjectTypeTranslate[Main.ObjectType.Land],
                    MinPrice = land.MinPrice,
                    MaxPrice = land.MaxPrice,
                    MinSquare = land.MinArea,
                    MaxSquare = land.MinArea,
                    Style = FindResource("EDemandCard") as Style,
                    Margin = new Thickness(10, 0, 0, 15),
                });
            }
            foreach (EDemandCard card in cards)
            {
                demandsList.Children.Add(card);
                card.MouseDown += Card_MouseDown;
            }
        }

        private void AddDemandButton_Click(object sender, RoutedEventArgs e)
        {
            new DemandAddEditWindow(CurrentUser).ShowDialog();
            LoadDemands();
        }

        // Сделки
        private void LoadDeals()
        {
            dealsList.Children.Clear();
            List<Deals> deals = Deals.GetAll();
            foreach (Deals deal in deals)
            {
                bool my = false;
                string offerType, offerAddress, offerPrice, offerArea;
                string demandType, demandAddress, demandMinPrice, demandMaxPrice, demandMinArea, demandMaxArea;
                offerType = offerAddress = offerPrice = offerArea = "";
                demandType = demandAddress = demandMinPrice = demandMaxPrice = demandMinArea = demandMaxArea = "";

                Offers offer = Offers.Get(deal.OfferID);
                if (offer != null)
                {
                    offerType = Main.ObjectTypeTranslate[offer.ObjectType];
                    offerPrice = offer.Price.ToString();

                    switch (offer.ObjectType)
                    {
                        case Main.ObjectType.Flat:
                            Apartments apartment = Apartments.Get(offer.ObjectID);
                            if (apartment != null)
                            {
                                offerAddress = Main.GetAdressString(apartment.Address_City, apartment.Address_Street, apartment.Address_House, apartment.Address_Number);
                                offerArea = apartment.TotalArea.ToString();
                            }
                            break;
                        case Main.ObjectType.House:
                            Houses house = Houses.Get(offer.ObjectID);
                            if (house != null)
                            {
                                offerAddress = Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number);
                                offerArea = house.TotalArea.ToString();
                            }
                            break;
                        case Main.ObjectType.Land:
                            Lands land = Lands.Get(offer.ObjectID);
                            if (land != null)
                            {
                                offerAddress = Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number);
                                offerArea = land.TotalArea.ToString();
                            }
                            break;
                    }

                    if (CurrentUser.UserType == Main.UserType.Agent && offer.AgentID == CurrentUser.UserID)
                        my = true;
                    if (CurrentUser.UserType == Main.UserType.Client && offer.ClientID == CurrentUser.UserID)
                        my = true;
                }

                switch (deal.ObjectType)
                {
                    case Main.ObjectType.Flat:
                        ApartmentDemand apartment = ApartmentDemand.Get(deal.DemandID);
                        if(apartment != null)
                        {  
                            demandType = Main.ObjectTypeTranslate[Main.ObjectType.Flat];
                            demandAddress = Main.GetAdressString(apartment.Address_City, apartment.Address_Street, apartment.Address_House, apartment.Address_Number);
                            demandMinPrice = apartment.MinPrice.ToString();
                            demandMaxPrice = apartment.MaxPrice.ToString();
                            demandMinArea = apartment.MinArea.ToString();
                            if (CurrentUser.UserType == Main.UserType.Agent && apartment.AgentID == CurrentUser.UserID)
                                my = true;
                            if (CurrentUser.UserType == Main.UserType.Client && apartment.ClientID == CurrentUser.UserID)
                                my = true;
                        }
                        break;
                    case Main.ObjectType.House:
                        HouseDemand house = HouseDemand.Get(deal.DemandID);
                        if (house != null)
                        {
                            demandType = Main.ObjectTypeTranslate[Main.ObjectType.House];
                            demandAddress = Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number);
                            demandMinPrice = house.MinPrice.ToString();
                            demandMaxPrice = house.MaxPrice.ToString();
                            demandMinArea = house.MinArea.ToString();
                            demandMaxArea = house.MaxArea.ToString();
                            if (CurrentUser.UserType == Main.UserType.Agent && house.AgentID == CurrentUser.UserID)
                                my = true;
                            if (CurrentUser.UserType == Main.UserType.Client && house.ClientID == CurrentUser.UserID)
                                my = true;
                        }
                        break;
                    case Main.ObjectType.Land:
                        LandDemand land = LandDemand.Get(deal.DemandID);
                        if (land != null)
                        {
                            demandType = Main.ObjectTypeTranslate[Main.ObjectType.Land];
                            demandAddress = Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number);
                            demandMinPrice = land.MinPrice.ToString();
                            demandMaxPrice = land.MaxPrice.ToString();
                            demandMinArea = land.MinArea.ToString();
                            demandMaxArea = land.MaxArea.ToString();
                            if (CurrentUser.UserType == Main.UserType.Agent && land.AgentID == CurrentUser.UserID)
                                my = true;
                            if (CurrentUser.UserType == Main.UserType.Client && land.ClientID == CurrentUser.UserID)
                                my = true; 
                        }
                        break;
                }

                if (my)
                {
                    var card = new EDealCard()
                    {
                        DealID = deal.ID,
                        OfferType = offerType,
                        OfferAddress = offerAddress,
                        OfferPrice = offerPrice,
                        DemandType = demandType,
                        DemandAddress = demandAddress,
                        DemandMinPrice = demandMinPrice,
                        DemandMaxPrice = demandMaxPrice,
                        DemandMinArea = demandMinArea,
                        DemandMaxArea = demandMaxArea,
                        Style = FindResource("EDealCard") as Style,
                        Margin = new Thickness(10, 0, 0, 15),
                    };
                    card.MouseDown += Card_MouseDown;
                    dealsList.Children.Add(card);
                }
            }
        }

        private void AddDealButton_Click(object sender, RoutedEventArgs e)
        {
            new DealsAddEditWindow(CurrentUser).ShowDialog();
            LoadDeals();
        }

    }
}
