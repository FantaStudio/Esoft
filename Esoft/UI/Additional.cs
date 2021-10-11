using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Esoft.Helpers;
using Esoft.Database;
using Esoft.Database.Common;

namespace Esoft.UI.Additional
{
    public class ComboBoxEObjectTypeItem : ComboBoxItem
    {
        public Helpers.Main.ObjectType ObjectType { get; set; }

        public ComboBoxEObjectTypeItem(Helpers.Main.ObjectType objectType)
        {
            ObjectType = objectType;
            if (Helpers.Main.ObjectTypeTranslate.ContainsKey(objectType))
                Content = Helpers.Main.ObjectTypeTranslate[objectType];
        }
    }

    public class ComboBoxEobjectItem : ComboBoxItem
    {
        public Helpers.Main.ObjectType ObjectType { get; set; }
        public int ObjectID { get; set; }

        public ComboBoxEobjectItem(int objectID, Helpers.Main.ObjectType objectType, string address)
        {
            ObjectID = objectID;
            ObjectType = objectType;
            if (Helpers.Main.ObjectTypeTranslate.ContainsKey(objectType))
                Content = Helpers.Main.ObjectTypeTranslate[objectType] + " " + address;
        }
    }

    public class ComboBoxEuserItem : ComboBoxItem
    {
        public Helpers.Main.UserType UserType { get; set; }
        public int UserID { get; set; }
        public ComboBoxEuserItem(int userID, Helpers.Main.UserType userType, string FIO)
        {
            UserID = userID;
            UserType = userType;
            Content = FIO;
        }
    }

    public class ComboBoxDemandItem : ComboBoxItem
    {
        public EDemand Demand { get; set; }
        public ComboBoxDemandItem(EDemand demand, string text)
        {
            Demand = demand;
            Content = text;
        }
    }

    public class ComboBoxOfferItem : ComboBoxItem
    {
        public int OfferID { get; set; }
        public ComboBoxOfferItem(int offerID, string text)
        {
            OfferID = offerID;
            Content = text;
        }
    }

    // Класс, помогающий заполнять комбобоксы
    public static class ComboBoxPull
    {
        // Заполняет комбобокс типами объектов
        public static void EObjectTypePull(ComboBox box, Helpers.Main.ObjectType selectedType = default(Helpers.Main.ObjectType))
        {
            box.Items.Clear();
            int index = 0;
            int curIndex = 0;
            foreach (var suit in (Helpers.Main.ObjectType[])Enum.GetValues(typeof(Helpers.Main.ObjectType)))
            {
                box.Items.Add(new ComboBoxEObjectTypeItem(suit));
                    if (selectedType == suit)
                        curIndex = index;
                index++;
            }
            box.SelectedIndex = curIndex;
        }

        // Заполняет комбобокс недвижимостью
        public static void EObjectPull(ComboBox box,int? clientID = null, int? objectID = null, Helpers.Main.ObjectType objectType = default(Helpers.Main.ObjectType))
        {
            box.Items.Clear();
            List<Apartments> apartments;
            List<Houses> houses;
            List<Lands> lands;
            if (clientID == null)
            {
                apartments = Apartments.GetAll();
                houses = Houses.GetAll();
                lands = Lands.GetAll();
            }
            else
            {
                apartments = Apartments.GetAll((int)clientID);
                houses = Houses.GetAll((int)clientID);
                lands = Lands.GetAll((int)clientID);
            }

            int index = 0;
            int curIndex = -1;
            foreach (var flat in apartments)
            {
                box.Items.Add(new ComboBoxEobjectItem(flat.ID,Helpers.Main.ObjectType.Flat,Helpers.Main.GetAdressString(flat.Address_City,flat.Address_Street,flat.Address_House,flat.Address_Number)));
                if (flat.ID == objectID && objectType == Helpers.Main.ObjectType.Flat)
                    curIndex = index;
                index++;
            }
            foreach (var house in houses)
            {
                box.Items.Add(new ComboBoxEobjectItem(house.ID, Helpers.Main.ObjectType.House, Helpers.Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number)));
                if (house.ID == objectID && objectType == Helpers.Main.ObjectType.House)
                    curIndex = index;
                index++;
            }
            foreach (var land in lands)
            {
                box.Items.Add(new ComboBoxEobjectItem(land.ID, Helpers.Main.ObjectType.Land, Helpers.Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number)));
                if (land.ID == objectID && objectType == Helpers.Main.ObjectType.Land)
                    curIndex = index;
                index++;
            }
            box.SelectedIndex = curIndex;
        }

        // Заполняет комбобокс риэлторами
        public static void RieltorsPull(ComboBox box,int? rieltorID = null)
        {
            box.Items.Clear();
            int index = 0;
            int curIndex = -1;
            var rieltors = Agents.GetAll();
            foreach (var rieltor in rieltors)
            {
                box.Items.Add(new ComboBoxEuserItem(rieltor.ID,Helpers.Main.UserType.Agent,Helpers.Main.ToFioString(rieltor.Surname,rieltor.Name,rieltor.Middlename)));
                if (rieltor.ID == rieltorID)
                    curIndex = index;
                index++;
            }
            box.SelectedIndex = curIndex;
        }

        // Заполняет комбобокс клиентами
        public static void ClientsPull(ComboBox box, int? clientID = null)
        {
            box.Items.Clear();
            int index = 0;
            int curIndex = -1;
            var clients = Clients.GetAll();
            foreach (var client in clients)
            {
                box.Items.Add(new ComboBoxEuserItem(client.ID, Helpers.Main.UserType.Client, Helpers.Main.ToFioString(client.Surname, client.Name, client.Middlename)));
                if (client.ID == clientID)
                    curIndex = index;
                index++;
            }
            box.SelectedIndex = curIndex;
        }

        // Заполняет комбобокс предложениями
        public static void OffersPull(ComboBox box, EUser user = null, int? offerID = null)
        {
            box.Items.Clear();
            List<Offers> offers = new List<Offers>();
            if (user != null)
            {
                if (user.UserType == Helpers.Main.UserType.Agent)
                    offers = Offers.GetAgentAll(user.UserID);
                else
                    offers = Offers.GetClientAll(user.UserID);
            }
            else
            {
                offers = Offers.GetAll();
            }

            int index = 0;
            int currentIndex = -1;
            foreach (Offers offer in offers)
            {
                if (!Deals.IsOfferInDeal(offer.ID) || offer.ID == offerID)
                {
                    string address = "";
                    switch (offer.ObjectType)
                    {
                        case Helpers.Main.ObjectType.Flat:
                            Apartments apartment = Apartments.Get(offer.ObjectID);
                            if (apartment == null) return;
                            address = Helpers.Main.GetAdressString(apartment.Address_City, apartment.Address_Street, apartment.Address_House, apartment.Address_Number);
                            break;
                        case Helpers.Main.ObjectType.House:
                            Houses house = Houses.Get(offer.ObjectID);
                            if (house == null) return;
                            address = Helpers.Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number);
                            break;
                        case Helpers.Main.ObjectType.Land:
                            Lands land = Lands.Get(offer.ObjectID);
                            if (land == null) return;
                            address = Helpers.Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number);
                            break;
                    }
                    box.Items.Add(new ComboBoxOfferItem(offer.ID, Helpers.Main.ObjectTypeTranslate[offer.ObjectType] + " " + address + " " + offer.Price + Helpers.Main.Currency));
                    if (offer.ID == offerID) currentIndex = index;
                    index++;
                } 
            }
            box.SelectedIndex = currentIndex;
        }

        //Заполняет комобобокс потребностями
        public static void DemandsPull(ComboBox box, EUser user = null, EDemand demand = null)
        {
            box.Items.Clear();
            List<ApartmentDemand> apartments;
            List<HouseDemand> houses;
            List<LandDemand> lands;
            if (user != null)
            {
                if (user.UserType == Helpers.Main.UserType.Agent)
                {
                    apartments = ApartmentDemand.GetAgentAll(user.UserID);
                    houses = HouseDemand.GetAgentAll(user.UserID);
                    lands = LandDemand.GetAgentAll(user.UserID);
                }
                else
                {
                    apartments = ApartmentDemand.GetClientAll(user.UserID);
                    houses = HouseDemand.GetClientAll(user.UserID);
                    lands = LandDemand.GetClientAll(user.UserID);
                }
            }
            else
            {
                apartments = ApartmentDemand.GetAll();
                houses = HouseDemand.GetAll();
                lands = LandDemand.GetAll();
            }

            int index = 0;
            int currentIdex = -1;
            foreach(ApartmentDemand apartmentDemand in apartments)
            {
                if (!Deals.IsDemandInDeal(new EDemand() { ID = apartmentDemand.ID, ObjectType = Helpers.Main.ObjectType.Flat }) || demand != null)
                {
                    string text = $"{Helpers.Main.ObjectTypeTranslate[Helpers.Main.ObjectType.Flat]} ";
                    if (apartmentDemand.MinPrice != null)
                        text += $"Мин.Цена: {apartmentDemand.MinPrice + Helpers.Main.Currency} ";
                    if (apartmentDemand.MaxPrice != null)
                        text += $"Макс.Цена: {apartmentDemand.MinPrice + Helpers.Main.Currency} ";
                    if (apartmentDemand.MinArea != null)
                        text += $"Мин.Площадь: {apartmentDemand.MinArea + Helpers.Main.SquareUnit} ";
                    if (apartmentDemand.MaxArea != null)
                        text += $"Макс.Площадь: {apartmentDemand.MaxArea + Helpers.Main.SquareUnit} ";
                    if (apartmentDemand.MinRooms != null)
                        text += $"Мин.Комнат: {apartmentDemand.MinRooms} ";
                    if (apartmentDemand.MaxRooms != null)
                        text += $"Макс.Комнат: {apartmentDemand.MaxRooms} ";
                    if (apartmentDemand.MinFloor != null)
                        text += $"Мин.Этаж: {apartmentDemand.MinFloor} ";
                    if (apartmentDemand.MaxFloor != null)
                        text += $"Макс.Этаж: {apartmentDemand.MaxFloor} ";
                    box.Items.Add(new ComboBoxDemandItem(new EDemand() { ID = apartmentDemand.ID, ObjectType = Helpers.Main.ObjectType.Flat }, text));
                    if (demand != null && demand.ObjectType == Helpers.Main.ObjectType.Flat && demand.ID == apartmentDemand.ID)
                        currentIdex = index;
                    index++;
                }
            }
            foreach (HouseDemand houseDemand in houses)
            {
                if (!Deals.IsDemandInDeal(new EDemand() { ID = houseDemand.ID, ObjectType = Helpers.Main.ObjectType.House }) || demand != null)
                {
                    string text = $"{Helpers.Main.ObjectTypeTranslate[Helpers.Main.ObjectType.House]} ";
                    if (houseDemand.MinPrice != null)
                        text += $"Мин.Цена: {houseDemand.MinPrice + Helpers.Main.Currency} ";
                    if (houseDemand.MaxPrice != null)
                        text += $"Макс.Цена: {houseDemand.MinPrice + Helpers.Main.Currency} ";
                    if (houseDemand.MinArea != null)
                        text += $"Мин.Площадь: {houseDemand.MinArea + Helpers.Main.SquareUnit} ";
                    if (houseDemand.MaxArea != null)
                        text += $"Макс.Площадь: {houseDemand.MaxArea + Helpers.Main.SquareUnit} ";
                    if (houseDemand.MinRooms != null)
                        text += $"Мин.Комнат: {houseDemand.MinRooms} ";
                    if (houseDemand.MaxRooms != null)
                        text += $"Макс.Комнат: {houseDemand.MaxRooms} ";
                    if (houseDemand.MinFloors != null)
                        text += $"Мин.Этажность: {houseDemand.MinFloors} ";
                    if (houseDemand.MaxFloors != null)
                        text += $"Макс.Этажность: {houseDemand.MaxFloors} ";
                    box.Items.Add(new ComboBoxDemandItem(new EDemand() { ID = houseDemand.ID, ObjectType = Helpers.Main.ObjectType.House }, text));
                    if (demand != null && demand.ObjectType == Helpers.Main.ObjectType.House && demand.ID == houseDemand.ID)
                        currentIdex = index;
                    index++;
                }
            }
            foreach (LandDemand landDemand in lands)
            {
                if (!Deals.IsDemandInDeal(new EDemand() { ID = landDemand.ID, ObjectType = Helpers.Main.ObjectType.Land }) || demand != null)
                {
                    string text = $"{Helpers.Main.ObjectTypeTranslate[Helpers.Main.ObjectType.Land]} ";
                    if (landDemand.MinPrice != null)
                        text += $"Мин.Цена: {landDemand.MinPrice + Helpers.Main.Currency} ";
                    if (landDemand.MaxPrice != null)
                        text += $"Макс.Цена: {landDemand.MinPrice + Helpers.Main.Currency} ";
                    if (landDemand.MinArea != null)
                        text += $"Мин.Площадь: {landDemand.MinArea + Helpers.Main.SquareUnit} ";
                    if (landDemand.MaxArea != null)
                        text += $"Макс.Площадь: {landDemand.MaxArea + Helpers.Main.SquareUnit} ";
                    box.Items.Add(new ComboBoxDemandItem(new EDemand() { ID = landDemand.ID, ObjectType = Helpers.Main.ObjectType.Land }, text));
                    if (demand != null && demand.ObjectType == Helpers.Main.ObjectType.Land && demand.ID == landDemand.ID)
                        currentIdex = index;
                    index++;
                }
            }
            box.SelectedIndex = currentIdex;
        }
    }
}
