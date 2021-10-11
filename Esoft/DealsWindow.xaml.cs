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
    public partial class DealsWindow : EUserWindow
    {
        public int? DealID = null;
        public int? OfferID = null;
        public EDemand demand = null;
        public DealsWindow(EUser user, int dealID) : base(user)
        {
            DealID = dealID;
            InitializeComponent();
            UpdateDealWindow();
        }

        public void ToolBarSetVisible(bool visible)
        {
            if (!visible)
            {
                ToolBar.Visibility = Visibility.Collapsed;
                Height = 582;
            }
            else
            {
                ToolBar.Visibility = Visibility.Visible;
                Height = 648;
            }
        }

        private void UpdateDealWindow()
        {
            if (DealID == null) return;
            Deals deal = Deals.Get((int)DealID);
            if (deal == null) return;
            OfferID = deal.OfferID;
            demand = new EDemand() { ID = deal.DemandID, ObjectType = deal.ObjectType };

            Agents offerAgent = null;
            Agents demandAgent = null;
            double clientBuyerPrice = 0; // Покупатель
            double clientSellerPrice = 0; // Продавец

            double rieltorBuyerPrice = 0; // Риэлтор покупателя
            double rieltorSellerPrice = 0; // Риэлтор продавца

            double companyPrice = 0;
            double objectPrice = 0;

            if (OfferID != null) 
            {
                Offers offer = Offers.Get((int)OfferID);
                if (offer != null)
                {
                    offerAgent = Agents.Get(offer.AgentID);
                    clientBuyerPrice = offer.Price * Main.ClientBuyerCommission / 100;
                    objectPrice = offer.Price;
                }
            }
            if(demand != null)
            {
                if(demand.ObjectType == Main.ObjectType.Flat)
                {
                    ApartmentDemand apdemand = ApartmentDemand.Get(demand.ID);
                    if (apdemand != null)
                        demandAgent = Agents.Get(apdemand.AgentID);
                }
                else if(demand.ObjectType == Main.ObjectType.House)
                {
                    HouseDemand housedemand = HouseDemand.Get(demand.ID);
                    if (housedemand != null)
                        demandAgent = Agents.Get(housedemand.AgentID);
                }
                else if (demand.ObjectType == Main.ObjectType.Land)
                {
                    LandDemand landdemand = LandDemand.Get(demand.ID);
                    if (landdemand != null)
                        demandAgent = Agents.Get(landdemand.AgentID);
                }
                (double, int) clientSellerCommision = Main.ClientSellerCommission[demand.ObjectType];
                clientSellerPrice = clientSellerCommision.Item1 + clientSellerCommision.Item2 * objectPrice / 100;
            }

            if (offerAgent != null) 
            {
                rieltorSellerPrice = clientSellerPrice * offerAgent.DealShare / 100;
                companyPrice += clientSellerPrice * (100 - offerAgent.DealShare)/100;
            }

            if (demandAgent != null)
            {
                rieltorBuyerPrice = clientBuyerPrice * demandAgent.DealShare/100;
                companyPrice += clientBuyerPrice * (100 - demandAgent.DealShare)/100;
            }

            ClientSellerServicePrice.Content = "Риэлтора-продавца: "+ clientSellerPrice.ToString() + Main.Currency;
            ClientBuyerSercivePrice.Content = "Риэлтора-покупателя: " + clientBuyerPrice.ToString() + Main.Currency;
            RieltorSellerDeduction.Content = "Клиента-продавца: "+ rieltorSellerPrice.ToString() + Main.Currency;
            RieltorBuyerDeduction.Content = "Клиента-покупателя: "+ rieltorBuyerPrice.ToString() + Main.Currency;
            CompanyDeduction.Content = "Компании: "+ companyPrice.ToString();

            bool isOwner = false;
            if (offerAgent != null && offerAgent.ID == CurrentUser.UserID && CurrentUser.UserType == Main.UserType.Agent)
                isOwner = true;
            else if (demandAgent != null && demandAgent.ID == CurrentUser.UserID && CurrentUser.UserType == Main.UserType.Agent)
                isOwner = true;
            ToolBarSetVisible(isOwner);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DealID == null) return;
            if (MessageHelper.Show("Вы точно хотите удалить сделку?",MessageHelper.MessageType.warning, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                bool deleted = Deals.Delete((int)DealID);
                MessageHelper.Show(deleted ? "Вы успешно удалили сделку" : "Не удалось удалить сделку");
                if (deleted) Close();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DealID == null) return;
            if (new DealsAddEditWindow(CurrentUser, (int)DealID).ShowDialog() == true)
                UpdateDealWindow();
        }

        private void OfferLink_Click(object sender, RoutedEventArgs e)
        {
            if (OfferID == null) return;
            new OfferWindow(CurrentUser, (int)OfferID).ShowDialog();
        }

        private void DemandLink_Click(object sender, RoutedEventArgs e)
        {
            if (demand == null) return;
            new DemandWindow(CurrentUser, demand).ShowDialog();
        }
    }
}
