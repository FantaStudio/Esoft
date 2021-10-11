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
    public partial class OfferWindow : EUserWindow
    {
        private int? OfferID = null;
        private int? RieltorID = null;

        public OfferWindow(EUser user, int offerID) : base(user)
        {
            OfferID = offerID;
            InitializeComponent();
            UpdateOfferWindow();
        }

        public void ToolBarSetVisible(bool visible,bool client)
        {
            if (!visible)
            {
                ToolBar.Visibility = Visibility.Collapsed;
                FindBar.Visibility = Visibility.Collapsed;
                Height = 315;
            }
            else
            {
                if (client)
                {
                    ToolBar.Visibility = Visibility.Visible;
                    FindBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    ToolBar.Visibility = Visibility.Collapsed;
                    FindBar.Visibility = Visibility.Visible;
                }
                Height = 385;
            }
        }

        private void UpdateOfferWindow()
        {
            if (OfferID == null) return;
            Offers offer = Offers.Get((int)OfferID);
            if (offer == null) return;
            Agents agent = Agents.Get(offer.AgentID);
            if (agent != null)
            {
                RieltorLink.Text = $"{agent.Surname} {agent.Name} {agent.Middlename}";
                RieltorID = agent.ID;
            }
            if (offer.AgentID == CurrentUser.UserID && CurrentUser.UserType == Main.UserType.Agent)
                ToolBarSetVisible(true, false);
            else if(offer.ClientID == CurrentUser.UserID && CurrentUser.UserType == Main.UserType.Client)
                ToolBarSetVisible(true,true);

            switch (offer.ObjectType)
            {
                case Main.ObjectType.Flat:
                    Apartments flat = Apartments.Get(offer.ObjectID);
                    if (flat == null) return;
                    AddressLabel.Content = $"Адрес: " + Main.GetAdressString(flat.Address_City, flat.Address_Street, flat.Address_House, flat.Address_Number);
                    break;
                case Main.ObjectType.House:
                    Houses house = Houses.Get(offer.ObjectID);
                    if (house == null) return;
                    AddressLabel.Content = $"Адрес: "+ Main.GetAdressString(house.Address_City, house.Address_Street, house.Address_House, house.Address_Number);
                    break;
                case Main.ObjectType.Land:
                    Lands land = Lands.Get(offer.ObjectID);
                    if (land == null) return;
                    AddressLabel.Content = $"Адрес: "+ Main.GetAdressString(land.Address_City, land.Address_Street, land.Address_House, land.Address_Number);
                    break;
            }
            TypeLabel.Content = "Тип объекта: " + Main.ObjectTypeTranslate[offer.ObjectType];
            PriceLabel.Content = "Стоимость: " + offer.Price + Main.Currency;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageHelper.Show("Вы точно хотите удалить предложение?",MessageHelper.MessageType.warning, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (Deals.IsOfferInDeal((int)OfferID))
                {
                    MessageHelper.Show("Вы не можете удалить предложение, участвующее в сделке",MessageHelper.MessageType.error);
                    return;
                }
                if (OfferID != null && Offers.Delete((int)OfferID))
                    Close();
                else
                    MessageHelper.Show("Не удалось удалить предложение",MessageHelper.MessageType.error);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (OfferID == null) return;
            if (new OfferAddEditWindow(CurrentUser, (int)OfferID).ShowDialog() == true)
                UpdateOfferWindow();
        }

        private void RieltorLink_Click(object sender, RoutedEventArgs e)
        {
            if(RieltorID != null)
            new ProfileWindow(new EUser((int)RieltorID, Main.UserType.Agent), false).ShowDialog();
        }

        private void FindDemandButton_Click(object sender, RoutedEventArgs e)
        {
            if (OfferID == null) return;
            var find = new FindWindow(CurrentUser, (int)OfferID);
            if (!find.IsClosed) find.ShowDialog();
        }
    }
}
