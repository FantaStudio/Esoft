using Esoft.Database;
using Esoft.Database.Common;
using Esoft.Helpers;
using Esoft.UI.Cards;
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

namespace Esoft
{
    /// <summary>
    /// Логика взаимодействия для FindWindow.xaml
    /// </summary>
    public partial class FindWindow : EUserWindow
    {
        private EDemand Demand = null;
        private int? OfferID = null;

        public FindWindow(EUser user,EDemand demand) : base(user)
        {
            OfferID = null;
            Demand = demand;
            InitializeComponent();
            PoolSimmilar();
        }
        public FindWindow(EUser user, int offerID) : base(user)
        {
            Demand = null;
            OfferID = offerID;
            InitializeComponent();
            PoolSimmilar();
        }

        private void AddCardIfIsDemandAndOfferAreSimmilar(ApartmentDemand apartmentDemand,Offers offer, bool offerOrDemandCard = false)
        {
            bool notSimmilar = false;
            Apartments apartment = Apartments.Get(offer.ObjectID);
            if (apartment != null && apartmentDemand != null)
            {
                if (apartmentDemand.MinPrice != null && offer.Price < apartmentDemand.MinPrice) notSimmilar = true;
                if (apartmentDemand.MaxPrice != null && offer.Price > apartmentDemand.MaxPrice) notSimmilar = true;
                if (apartmentDemand.MinArea != null && apartment.TotalArea < apartmentDemand.MinArea) notSimmilar = true;
                if (apartmentDemand.MaxArea != null && apartment.TotalArea > apartmentDemand.MaxArea) notSimmilar = true;
                if (apartmentDemand.MinRooms != null && apartment.Rooms < apartmentDemand.MinRooms) notSimmilar = true;
                if (apartmentDemand.MaxRooms != null && apartment.Rooms > apartmentDemand.MaxRooms) notSimmilar = true;
                if (apartmentDemand.MinFloor != null && apartment.Floor < apartmentDemand.MinFloor) notSimmilar = true;
                if (apartmentDemand.MaxFloor != null && apartment.Floor > apartmentDemand.MaxFloor) notSimmilar = true;
            }
            else return;

            if (!notSimmilar)
            {
                if (!offerOrDemandCard)
                {
                    var card = new EOfferCard(offer.Price, offer.ID, Main.ObjectTypeTranslate[offer.ObjectType],
                        Main.GetAdressString(apartment.Address_City, apartment.Address_Street, apartment.Address_House, apartment.Address_Number))
                    {
                        Style = FindResource("EOfferCard") as Style,
                        Margin = new Thickness(10, 0, 0, 15),
                        ToolTip = "Правая кнопка мыши - Подробный просмотр, Левая кнопка мыши - выбор для сделки",
                    };
                    card.MouseDown += Card_MouseDown;
                    SimmilarList.Children.Add(card);
                }
                else
                {
                    var card = new EDemandCard()
                    {
                        Demand = new EDemand() { ID = apartment.ID, ObjectType = Main.ObjectType.Flat },
                        EObjectAddress = Main.GetAdressString(apartment.Address_City, apartment.Address_Street, apartment.Address_House, apartment.Address_Number),
                        EObjectType = Main.ObjectTypeTranslate[Main.ObjectType.Flat],
                        MinPrice = apartmentDemand.MinPrice,
                        MaxPrice = apartmentDemand.MaxPrice,
                        MinSquare = apartmentDemand.MinArea,
                        MaxSquare = apartmentDemand.MaxArea,
                        Style = FindResource("EDemandCard") as Style,
                        Margin = new Thickness(10, 0, 0, 15),
                        ToolTip = "Правая кнопка мыши - Подробный просмотр, Левая кнопка мыши - выбор для сделки",
                    };
                    card.MouseDown += Card_MouseDown;
                    SimmilarList.Children.Add(card);
                }  
            }
        }

        private void AddCardIfIsDemandAndOfferAreSimmilar(HouseDemand houseDemand, Offers offer, bool offerOrDemandCard = false)
        {
            bool notSimmilar = false;
            Houses house = Houses.Get(offer.ObjectID);
            if (house != null && houseDemand != null)
            {
                if (houseDemand.MinPrice != null && offer.Price < houseDemand.MinPrice) notSimmilar = true;
                if (houseDemand.MaxPrice != null && offer.Price > houseDemand.MaxPrice) notSimmilar = true;
                if (houseDemand.MinArea != null && house.TotalArea < houseDemand.MinArea) notSimmilar = true;
                if (houseDemand.MaxArea != null && house.TotalArea > houseDemand.MaxArea) notSimmilar = true;
                if (houseDemand.MinRooms != null && house.Rooms < houseDemand.MinRooms) notSimmilar = true;
                if (houseDemand.MaxRooms != null && house.Rooms > houseDemand.MaxRooms) notSimmilar = true;
                if (houseDemand.MinFloors != null && house.TotalFloors < houseDemand.MinFloors) notSimmilar = true;
                if (houseDemand.MaxFloors != null && house.TotalFloors > houseDemand.MaxFloors) notSimmilar = true;
            }
            else return;

            if (!notSimmilar)
            {
                if (!offerOrDemandCard)
                {
                    var card = new EOfferCard(offer.Price, offer.ID, Main.ObjectTypeTranslate[offer.ObjectType],
                        Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number))
                    {
                        Style = FindResource("EOfferCard") as Style,
                        Margin = new Thickness(10, 0, 0, 15),
                        ToolTip = "Правая кнопка мыши - Подробный просмотр, Левая кнопка мыши - выбор для сделки",
                    };
                    card.MouseDown += Card_MouseDown;
                    SimmilarList.Children.Add(card);
                }
                else
                {
                    var card = new EDemandCard()
                    {
                        Demand = new EDemand() { ID = house.ID, ObjectType = Main.ObjectType.House },
                        EObjectAddress = Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number),
                        EObjectType = Main.ObjectTypeTranslate[Main.ObjectType.House],
                        MinPrice = houseDemand.MinPrice,
                        MaxPrice = houseDemand.MaxPrice,
                        MinSquare = houseDemand.MinArea,
                        MaxSquare = houseDemand.MaxArea,
                        Style = FindResource("EDemandCard") as Style,
                        Margin = new Thickness(10, 0, 0, 15),
                        ToolTip = "Правая кнопка мыши - Подробный просмотр, Левая кнопка мыши - выбор для сделки",
                    };
                    card.MouseDown += Card_MouseDown;
                    SimmilarList.Children.Add(card);
                }
            }
        }

        private void AddCardIfIsDemandAndOfferAreSimmilar(LandDemand landDemand, Offers offer, bool offerOrDemandCard = false)
        {
            bool notSimmilar = false;
            Lands land = Lands.Get(offer.ObjectID);
            if (land != null && landDemand != null)
            {
                if (landDemand.MinPrice != null && offer.Price < landDemand.MinPrice) notSimmilar = true;
                if (landDemand.MaxPrice != null && offer.Price > landDemand.MaxPrice) notSimmilar = true;
                if (landDemand.MinArea != null && land.TotalArea < landDemand.MinArea) notSimmilar = true;
                if (landDemand.MaxArea != null && land.TotalArea > landDemand.MaxArea) notSimmilar = true;
            }
            else return;

            if (!notSimmilar)
            {
                if (!offerOrDemandCard)
                {
                    var card = new EOfferCard(offer.Price, offer.ID, Main.ObjectTypeTranslate[offer.ObjectType],
                        Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number))
                    {
                        Style = FindResource("EOfferCard") as Style,
                        Margin = new Thickness(10, 0, 0, 15),
                        ToolTip = "Правая кнопка мыши - Подробный просмотр, Левая кнопка мыши - выбор для сделки",
                    };
                    card.MouseDown += Card_MouseDown;
                    SimmilarList.Children.Add(card);
                }
                else
                {
                    var card = new EDemandCard()
                    {
                        Demand = new EDemand() { ID = land.ID, ObjectType = Main.ObjectType.Land },
                        EObjectAddress = Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number),
                        EObjectType = Main.ObjectTypeTranslate[Main.ObjectType.Land],
                        MinPrice = landDemand.MinPrice,
                        MaxPrice = landDemand.MaxPrice,
                        MinSquare = landDemand.MinArea,
                        MaxSquare = landDemand.MaxArea,
                        Style = FindResource("EDemandCard") as Style,
                        Margin = new Thickness(10, 0, 0, 15),
                        ToolTip = "Правая кнопка мыши - Подробный просмотр, Левая кнопка мыши - выбор для сделки",
                    };
                    card.MouseDown += Card_MouseDown;
                    SimmilarList.Children.Add(card);
                }
            }
        }

        private void PoolSimmilar()
        {
            SimmilarList.Children.Clear();
            if(CurrentUser.UserType != Main.UserType.Agent)
            {
                MessageHelper.Show("У вас нет доступа к данному окну", MessageHelper.MessageType.error);
                Close();
                return;
            }
            if(Demand != null)
            {
                List<Offers> allOffers = Offers.GetBy("AgentID,ObjectType",$"{CurrentUser.UserID},{(int)Demand.ObjectType}",false);
                if (allOffers.Count < 1)
                {
                    MessageHelper.Show("Нет подходящих предложений", MessageHelper.MessageType.warning);
                    Close();
                    return;
                }
                if (Deals.IsDemandInDeal(Demand)) return;
                foreach(Offers offer in allOffers)
                {
                    if (!Deals.IsOfferInDeal(offer.ID))
                    {
                        switch (Demand.ObjectType)
                        {
                            case Main.ObjectType.Flat:
                                ApartmentDemand apartmentDemand = ApartmentDemand.Get(Demand.ID);
                                AddCardIfIsDemandAndOfferAreSimmilar(apartmentDemand, offer);
                                break;
                            case Main.ObjectType.House:
                                HouseDemand houseDemand = HouseDemand.Get(Demand.ID);
                                AddCardIfIsDemandAndOfferAreSimmilar(houseDemand, offer);
                                break;
                            case Main.ObjectType.Land:
                                LandDemand landDemand = LandDemand.Get(Demand.ID);
                                AddCardIfIsDemandAndOfferAreSimmilar(landDemand, offer);
                                break;
                        }
                    }
                }
            }
            else if(OfferID != null)
            {
                Offers offer = Offers.Get((int)OfferID);
                if(offer == null)
                {
                    MessageHelper.Show("Не удалось загрузить предложение, возможно его больше не существует",MessageHelper.MessageType.error);
                    return;
                }
                if (Deals.IsOfferInDeal(offer.ID)) return;
                switch (offer.ObjectType)
                {
                    case Main.ObjectType.Flat:
                        List<ApartmentDemand> apartmentDemands = ApartmentDemand.GetAgentAll(CurrentUser.UserID);
                        if(apartmentDemands.Count < 1)
                        {
                            MessageHelper.Show("Нет подходящих потребностей", MessageHelper.MessageType.warning);
                            Close();
                            return;
                        }
                        foreach (ApartmentDemand apartmentDemand in apartmentDemands)
                        {
                            if (!Deals.IsDemandInDeal(new EDemand() { ID = apartmentDemand.ID, ObjectType = Main.ObjectType.Flat }))
                                AddCardIfIsDemandAndOfferAreSimmilar(apartmentDemand, offer, true);
                        }
                        break;
                    case Main.ObjectType.House:
                        List<HouseDemand> houseDemands = HouseDemand.GetAgentAll(CurrentUser.UserID);
                        if (houseDemands.Count < 1)
                        {
                            MessageHelper.Show("Нет подходящих потребностей", MessageHelper.MessageType.warning);
                            Close();
                            return;
                        }
                        foreach (HouseDemand houseDemand in houseDemands)
                        {
                            if(!Deals.IsDemandInDeal(new EDemand() { ID = houseDemand.ID, ObjectType = Main.ObjectType.House }))
                                AddCardIfIsDemandAndOfferAreSimmilar(houseDemand, offer, true);
                        }
                        break;
                    case Main.ObjectType.Land:
                        List<LandDemand> landDemands = LandDemand.GetAgentAll(CurrentUser.UserID);
                        if (landDemands.Count < 1)
                        {
                            MessageHelper.Show("Нет подходящих потребностей", MessageHelper.MessageType.warning);
                            Close();
                            return;
                        }
                        foreach (LandDemand landDemand in landDemands)
                        {
                            if (!Deals.IsDemandInDeal(new EDemand() { ID = landDemand.ID, ObjectType = Main.ObjectType.Land }))
                                AddCardIfIsDemandAndOfferAreSimmilar(landDemand, offer, true);
                        }
                        break;
                }
            }
        }

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (sender is EOfferCard)
            {
                var card = (EOfferCard)sender;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    bool? result = new DealsAddEditWindow(CurrentUser, Demand, card.OfferID).ShowDialog();
                    if(result == true)
                        PoolSimmilar();
                }
                else if(e.RightButton == MouseButtonState.Pressed)
                    new OfferWindow(CurrentUser, card.OfferID).ShowDialog();
            }
            else if (sender is EDemandCard)
            {
                var card = (EDemandCard)sender;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    bool? result = new DealsAddEditWindow(CurrentUser, card.Demand, (int)OfferID).ShowDialog();
                    if (result == true)
                        PoolSimmilar();
                }
                else if(e.RightButton == MouseButtonState.Pressed)
                    new DemandWindow(CurrentUser, card.Demand).ShowDialog();
            }
        }

    }
}
