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
    public partial class DealsAddEditWindow : EUserWindow
    {
        public int? DealID = null;
        public EDemand Demand = null;
        public int? OfferID = null;
        public DealsAddEditWindow(EUser user) : base(user)
        {
            InitializeComponent();
            LoadDealInfo();

        }

        public DealsAddEditWindow(EUser user, int dealID) : base(user)
        {
            DealID = dealID;
            InitializeComponent();
            LoadDealInfo();
        }

        public DealsAddEditWindow(EUser user, EDemand demand, int offerID) : base(user)
        {
            Demand = demand;
            OfferID = offerID;
            InitializeComponent();
            LoadDealInfo();
        }

        private void LoadDealInfo()
        {
            if (DealID == null)
            {
                ComboBoxPull.OffersPull(OffersBox, CurrentUser);
                ComboBoxPull.DemandsPull(DemandsBox, CurrentUser);
                SaveButton.Content = "Добавить";
                Title = "Добавление сделки";
            }else if (Demand != null && OfferID != null)
            {
                ComboBoxPull.OffersPull(OffersBox, CurrentUser,OfferID);
                ComboBoxPull.DemandsPull(DemandsBox, CurrentUser,Demand);
                SaveButton.Content = "Добавить";
                Title = "Добавление сделки";
            }
            else
            {
                SaveButton.Content = "Сохранить";
                Title = "Редактирование сделки";
                Deals deal = Deals.Get((int)DealID);
                if (deal == null) return;
                ComboBoxPull.OffersPull(OffersBox, CurrentUser, deal.OfferID);
                ComboBoxPull.DemandsPull(DemandsBox, CurrentUser, new EDemand() { ID = deal.DemandID, ObjectType = deal.ObjectType });
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxOfferItem offerItem = OffersBox.SelectedItem as ComboBoxOfferItem;
            ComboBoxDemandItem demandItem = DemandsBox.SelectedItem as ComboBoxDemandItem;
            if (demandItem == null)
            {
                MessageHelper.Show("Выберите потребность");
                return;
            }
            if (offerItem == null)
            {
                MessageHelper.Show("Выберите предложение");
                return;
            }
            if (DealID != null)
            {
                Deals currentDeal = Deals.Get((int)DealID);
                if (Deals.IsDemandInDeal(demandItem.Demand) && demandItem.Demand.ID != currentDeal.DemandID)
                {
                    MessageHelper.Show("Потребность уже удовлетворена и не может учавствовать в сделке");
                    return;
                }
                if (Deals.IsOfferInDeal(offerItem.OfferID) && offerItem.OfferID != currentDeal.OfferID)
                {
                    MessageHelper.Show("Предложение уже удовлетворено и не может учавствовать в сделке");
                    return;
                }
            }else if(Demand != null || OfferID != null)
            {
                if (Deals.IsDemandInDeal(demandItem.Demand) && demandItem.Demand.ID != Demand.ID)
                {
                    MessageHelper.Show("Потребность уже удовлетворена и не может учавствовать в сделке");
                    return;
                }
                if (Deals.IsOfferInDeal(offerItem.OfferID) && offerItem.OfferID != OfferID)
                {
                    MessageHelper.Show("Предложение уже удовлетворено и не может учавствовать в сделке");
                    return;
                }
            }

            if (DealID == null)
            {
                if(Deals.Add(demandItem.Demand.ID, demandItem.Demand.ObjectType, offerItem.OfferID))
                {
                    MessageHelper.Show("Сделка успешно добавлена");
                    DialogResult = true;
                }else
                    MessageHelper.Show("Не удалось добавить сделку");
            }
            else
            {
                if (Deals.Update((int)DealID, demandItem.Demand.ID, demandItem.Demand.ObjectType, offerItem.OfferID))
                {
                    MessageHelper.Show("Сделка успешно отредактирована");
                    DialogResult = true;
                }
                else
                    MessageHelper.Show("Не удалось отредактировать сделку");
            }
        }

        private void DemandsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxDemandItem demandItem = DemandsBox.SelectedItem as ComboBoxDemandItem;
            if (demandItem == null) return;
            ComboBoxOfferItem offerItem = OffersBox.SelectedItem as ComboBoxOfferItem;
            if (offerItem != null) return;
            switch (demandItem.Demand.ObjectType)
            {
                case Main.ObjectType.Flat:
                    ApartmentDemand apdemand = ApartmentDemand.Get(demandItem.Demand.ID);
                    if (apdemand != null && apdemand.AgentID == CurrentUser.UserID)
                    {
                        ComboBoxPull.OffersPull(OffersBox);
                    }
                    else
                        ComboBoxPull.OffersPull(OffersBox, CurrentUser);
                    break;
                case Main.ObjectType.House:
                    HouseDemand housedemand = HouseDemand.Get(demandItem.Demand.ID);
                    if (housedemand != null && housedemand.AgentID == CurrentUser.UserID)
                        ComboBoxPull.OffersPull(OffersBox);
                    else
                        ComboBoxPull.OffersPull(OffersBox, CurrentUser);
                    break;
                case Main.ObjectType.Land:
                    LandDemand landdemand = LandDemand.Get(demandItem.Demand.ID);
                    if (landdemand != null && landdemand.AgentID == CurrentUser.UserID)
                        ComboBoxPull.OffersPull(OffersBox);
                    else
                        ComboBoxPull.OffersPull(OffersBox, CurrentUser);
                    break;
            }
        }

        private void OffersBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxOfferItem offerItem = OffersBox.SelectedItem as ComboBoxOfferItem;
            if (offerItem == null) return;
            ComboBoxDemandItem demandItem = DemandsBox.SelectedItem as ComboBoxDemandItem;
            if (demandItem != null) return;
            Offers offer = Offers.Get(offerItem.OfferID);
            if (offer == null) return;
            if (offer.AgentID == CurrentUser.UserID)
                ComboBoxPull.DemandsPull(DemandsBox);
            else
                ComboBoxPull.DemandsPull(DemandsBox, CurrentUser);
        }
    }
}
