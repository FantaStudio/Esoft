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
using Esoft.UI.Additional;
using Esoft.UI.Inputs;

namespace Esoft
{
    public partial class OfferAddEditWindow : EUserWindow
    {
        private int? OfferID = null;

        public OfferAddEditWindow(EUser user) : base(user)
        {
            InitializeComponent();
            LoadOfferWindow();
            EInput.AddClearStatusHandler(this);
        }

        public OfferAddEditWindow(EUser user, int offerID) : base(user)
        {
            OfferID = offerID;
            InitializeComponent();
            LoadOfferWindow();
            EInput.AddClearStatusHandler(this);
        }

        private void LoadOfferWindow()
        {
            if (OfferID != null)
            {
                SaveButton.Content = "Сохранить";
                Title = "Редактирование предложения";
                Offers offer = Offers.Get((int)OfferID);
                if (offer == null) return;
                ComboBoxPull.EObjectPull(ObjectBox,offer.ClientID,offer.ObjectID,offer.ObjectType);
                ComboBoxPull.RieltorsPull(RieltorsBox, offer.AgentID);
                ComboBoxPull.ClientsPull(ClientsBox, offer.ClientID);
                PriceBox.Text = offer.Price.ToString();
            }
            else
            {
                ComboBoxPull.EObjectPull(ObjectBox,CurrentUser.UserID);
                ComboBoxPull.RieltorsPull(RieltorsBox);
                ComboBoxPull.ClientsPull(ClientsBox,CurrentUser.UserID);
                PriceBox.Text = "";
                SaveButton.Content = "Добавить";
                Title = "Добавление предложения";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(ObjectBox.SelectedItem == null)
            {
                MessageHelper.Show("Выберите объект",MessageHelper.MessageType.warning);
                return;
            }
            if (RieltorsBox.SelectedItem == null)
            {
                MessageHelper.Show("Выберите риэлтора", MessageHelper.MessageType.warning);
                return;
            }
            if (ClientsBox.SelectedItem == null)
            {
                MessageHelper.Show("Выберите клиента", MessageHelper.MessageType.warning);
                return;
            }
            ComboBoxEobjectItem choosedObject = ObjectBox.SelectedItem as ComboBoxEobjectItem;
            ComboBoxEuserItem choosedRieltor = RieltorsBox.SelectedItem as ComboBoxEuserItem;
            ComboBoxEuserItem choosedClient = ClientsBox.SelectedItem as ComboBoxEuserItem;
            int price;
            if (!int.TryParse(PriceBox.Text, out price))
            {
                PriceBox.EStatus = Statuses.Main.Danger;
                MessageHelper.Show("Введите стоимость", MessageHelper.MessageType.warning);
                return;
            }
            if(price < 1)
            {
                PriceBox.EStatus = Statuses.Main.Danger;
                MessageHelper.Show("Стоимость не может быть меньше 1", MessageHelper.MessageType.warning);
                return;
            }
            int agentID = choosedRieltor.UserID;
            int clientID = choosedClient.UserID;
            int objectID = choosedObject.ObjectID;
            Main.ObjectType objectType = choosedObject.ObjectType;
            if(OfferID == null)
            {
                if(Offers.Add(agentID, clientID, objectID, objectType, price))
                {
                    MessageHelper.Show("Предложение успешно добавлено", MessageHelper.MessageType.success);
                    DialogResult = true;
                }
                else
                    MessageHelper.Show("Не удалось добавить предложение", MessageHelper.MessageType.error);
            }
            else
            {
                if(Offers.Update((int)OfferID, agentID, clientID, objectID, objectType, price))
                {
                    MessageHelper.Show("Предложение успешно отредактировано", MessageHelper.MessageType.success);
                    DialogResult = true;
                }
                else
                    MessageHelper.Show("Не удалось отредактировать предложение", MessageHelper.MessageType.error);
            }
        }
    }
}
