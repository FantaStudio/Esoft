using Esoft.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Esoft.UI.Cards
{
    // Карточка объекта
    class EObjectCard : MaterialDesignThemes.Wpf.Card
    {
        public static readonly DependencyProperty EObjectTypeProperty = DependencyProperty.Register("EObjectType", typeof(string), typeof(EObjectCard), 
            new FrameworkPropertyMetadata("",new PropertyChangedCallback(EObjectCard_PropertyChanged)));

        public static readonly DependencyProperty EObjectAdressProperty = DependencyProperty.Register("EObjectAdress", typeof(string), typeof(EObjectCard),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(EObjectCard_PropertyChanged)));
       
        private Label adressLabel;
        private Label typeLabel;

        public string EObjectType
        {
            get { return GetValue(EObjectTypeProperty).ToString(); }
            set { SetValue(EObjectTypeProperty, value); }
        }

        public string EObjectAdress
        {
            get { return GetValue(EObjectAdressProperty).ToString(); }
            set { SetValue(EObjectAdressProperty, value); }
        }

        public EObject CardObject { get; set; }

        public EObjectCard() => PullObjectInfo();

        private static void EObjectCard_PropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var card = sender as EObjectCard;
            if (e.Property == EObjectTypeProperty)
            {
                card.typeLabel.Content = card.EObjectType;
            }
            else if (e.Property == EObjectAdressProperty)
            {
                card.adressLabel.Content = card.EObjectAdress;
            }
        }

        private void PullObjectInfo()
        {
            var stack = new StackPanel();
            this.AddChild(stack);
            typeLabel = new Label() { Content = EObjectType, FontSize = 14 };
            adressLabel = new Label() { Content = EObjectAdress, FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            stack.Children.Add(typeLabel);
            stack.Children.Add(adressLabel);
        }
    }

    // Карточка предложения
    class EOfferCard : MaterialDesignThemes.Wpf.Card
    {
        public static readonly DependencyProperty EObjectTypeProperty = DependencyProperty.Register("EObjectType", typeof(string), typeof(EOfferCard),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(EOfferCard_PropertyChanged)));

        public static readonly DependencyProperty EObjectAddressProperty = DependencyProperty.Register("EObjectAddress", typeof(string), typeof(EOfferCard),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(EOfferCard_PropertyChanged)));

        public static readonly DependencyProperty PriceProperty = DependencyProperty.Register("Price", typeof(int), typeof(EOfferCard),
            new FrameworkPropertyMetadata(0, new PropertyChangedCallback(EOfferCard_PropertyChanged)));

        private Label adressLabel;
        private Label typeLabel;
        private Label priceLabel;

        public string EObjectType
        {
            get { return GetValue(EObjectTypeProperty).ToString(); }
            set { SetValue(EObjectTypeProperty, value); }
        }

        public string EObjectAddress
        {
            get { return GetValue(EObjectAddressProperty).ToString(); }
            set { SetValue(EObjectAddressProperty, value); }
        }

        public int Price
        {
            get { return (int)GetValue(PriceProperty); }
            set { SetValue(PriceProperty, value); }
        }

        public int OfferID { get; set; }

        public EOfferCard() => PullOfferInfo();

        public EOfferCard(int price,int offerID, string objectType, string address)
        {
            PullOfferInfo();
            Price = price;
            OfferID = offerID;
            EObjectType = objectType;
            EObjectAddress = address;
        }

        private static void EOfferCard_PropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var card = sender as EOfferCard;
            if (card == null) return;
            if (e.Property == EObjectTypeProperty)
                card.typeLabel.Content = card.EObjectType;
            else if (e.Property == EObjectAddressProperty)
                card.adressLabel.Content = card.EObjectAddress;
            else if (e.Property == PriceProperty)
                card.priceLabel.Content = "Стоимость: "+card.Price + Helpers.Main.Currency;
        }

        private void PullOfferInfo()
        {
            var stack = new StackPanel();
            this.AddChild(stack);
            typeLabel = new Label() { Content = EObjectType, FontSize = 14 };
            adressLabel = new Label() { Content = EObjectAddress, FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            priceLabel = new Label() { Content = "Стоимость: " + Price + Helpers.Main.Currency, FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            stack.Children.Add(typeLabel);
            stack.Children.Add(adressLabel);
            stack.Children.Add(priceLabel);
        }
    }

    // Карточка потребности
    class EDemandCard : MaterialDesignThemes.Wpf.Card
    {
        public static readonly DependencyProperty EObjectTypeProperty = DependencyProperty.Register("EObjectType", typeof(string), typeof(EDemandCard),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(EDemandCard_PropertyChanged)));

        public static readonly DependencyProperty EObjectAddressProperty = DependencyProperty.Register("EObjectAddress", typeof(string), typeof(EDemandCard),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(EDemandCard_PropertyChanged)));

        public static readonly DependencyProperty MinPriceProperty = DependencyProperty.Register("MinPrice", typeof(int?), typeof(EDemandCard),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(EDemandCard_PropertyChanged)));

        public static readonly DependencyProperty MaxPriceProperty = DependencyProperty.Register("MaxPrice", typeof(int?), typeof(EDemandCard),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(EDemandCard_PropertyChanged)));

        public static readonly DependencyProperty MinSquareProperty = DependencyProperty.Register("MinSquare", typeof(double?), typeof(EDemandCard),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(EDemandCard_PropertyChanged)));

        public static readonly DependencyProperty MaxSquareProperty = DependencyProperty.Register("MaxSquare", typeof(double?), typeof(EDemandCard),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(EDemandCard_PropertyChanged)));

        private Label adressLabel;
        private Label typeLabel;
        private Label minPriceLabel;
        private Label maxPriceLabel;
        private Label minSquareLabel;
        private Label maxSquareLabel;

        public string EObjectType
        {
            get { return GetValue(EObjectTypeProperty).ToString(); }
            set { SetValue(EObjectTypeProperty, value); }
        }

        public string EObjectAddress
        {
            get { return GetValue(EObjectAddressProperty).ToString(); }
            set { SetValue(EObjectAddressProperty, value); }
        }

        public int? MinPrice
        {
            get { return (int?)GetValue(MinPriceProperty); }
            set { SetValue(MinPriceProperty, value); }
        }

        public int? MaxPrice
        {
            get { return (int?)GetValue(MaxPriceProperty); }
            set { SetValue(MaxPriceProperty, value); }
        }

        public double? MinSquare
        {
            get { return (double?)GetValue(MinSquareProperty); }
            set { SetValue(MinSquareProperty, value); }
        }

        public double? MaxSquare
        {
            get { return (double?)GetValue(MaxSquareProperty); }
            set { SetValue(MaxSquareProperty, value); }
        }

        public EDemand Demand { get; set; }

        public EDemandCard() => PullDemandInfo();

        private static void EDemandCard_PropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var card = sender as EDemandCard;
            if (card == null) return;
            if (e.Property == EObjectTypeProperty)
                card.typeLabel.Content = card.EObjectType;
            else if (e.Property == EObjectAddressProperty)
            {
                if (string.IsNullOrEmpty(card.EObjectAddress))
                {
                    card.adressLabel.Visibility = Visibility.Collapsed;
                    return;
                }
                card.adressLabel.Visibility = Visibility.Visible;
                card.adressLabel.Content = card.EObjectAddress;
            }
            else if (e.Property == MinPriceProperty)
            {
                if (card.MinPrice == null)
                {
                    card.minPriceLabel.Visibility = Visibility.Collapsed;
                    return;
                }
                card.minPriceLabel.Visibility = Visibility.Visible;
                card.minPriceLabel.Content = "Мин.Стоимость: " + card.MinPrice + Helpers.Main.Currency;
            }
            else if (e.Property == MaxPriceProperty)
            {
                if (card.MaxPrice == null)
                {
                    card.maxPriceLabel.Visibility = Visibility.Collapsed;
                    return;
                }
                card.maxPriceLabel.Visibility = Visibility.Visible;
                card.maxPriceLabel.Content = "Макс.Стоимость: " + card.MaxPrice + Helpers.Main.Currency;
            }
            else if (e.Property == MinSquareProperty)
            {
                if (card.MinSquare == null)
                {
                    card.minSquareLabel.Visibility = Visibility.Collapsed;
                    return;
                }
                card.minSquareLabel.Visibility = Visibility.Visible;
                card.minSquareLabel.Content = "Мин.Площадь: " + card.MinSquare + Helpers.Main.SquareUnit;
            }
            else if (e.Property == MaxSquareProperty)
            {
                if (card.MaxSquare == null)
                {
                    card.maxSquareLabel.Visibility = Visibility.Collapsed;
                    return;
                }
                card.maxSquareLabel.Visibility = Visibility.Visible;
                card.maxSquareLabel.Content = "Макс.Площадь: " + card.MaxSquare + Helpers.Main.SquareUnit;
            }
        }

        private void PullDemandInfo()
        {
            var stack = new StackPanel();
            this.AddChild(stack);
            typeLabel = new Label() { Content = EObjectType, FontSize = 14 };
            adressLabel = new Label() { Content = EObjectAddress, FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush, Visibility = Visibility.Collapsed };
            minPriceLabel = new Label() { Content = "Мин.Стоимость: " + MinPrice, FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush,Visibility = Visibility.Collapsed };
            maxPriceLabel = new Label() { Content = "Макс.Стоимость: " + MaxPrice, FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush, Visibility = Visibility.Collapsed };
            minSquareLabel = new Label() { Content = "Мин.Площадь: " + MinSquare, FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush, Visibility = Visibility.Collapsed };
            maxSquareLabel = new Label() { Content = "Макс.Площадь: " + MaxSquare, FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush, Visibility = Visibility.Collapsed };
            stack.Children.Add(typeLabel);
            stack.Children.Add(adressLabel);
            stack.Children.Add(minPriceLabel);
            stack.Children.Add(maxPriceLabel);
            stack.Children.Add(minSquareLabel);
            stack.Children.Add(maxSquareLabel);
        }
    }

    // Карточка сделки
    class EDealCard : MaterialDesignThemes.Wpf.Card
    {
        private Label offerTypeLabel;
        private Label offerAddressLabel;
        private Label offerPriceLabel;
        private Label offerAreaLabel;

        private Label demandTypeLabel;
        private Label demandAddressLabel;
        private Label demandMinPriceLabel;
        private Label demandMaxPriceLabel;
        private Label demandMinAreaLabel;
        private Label demandMaxAreaLabel;

        public int DealID { get; set; }
        public string OfferType { set => offerTypeLabel.Content = value;}

        public string OfferAddress { set => offerAddressLabel.Content = value; }

        public string OfferPrice { set => offerPriceLabel.Content = value + Helpers.Main.Currency; }
        public string OfferArea { set => offerAreaLabel.Content = value + Helpers.Main.SquareUnit; }

        public string DemandType { set => demandTypeLabel.Content = value; }
        public string DemandAddress
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    demandAddressLabel.Content = "Адрес: Не указан";
                else
                    demandAddressLabel.Content = "Адрес: "+value;
            }
        }

        public string DemandMinPrice
        {
            set {
                if (string.IsNullOrEmpty(value))
                    demandMinPriceLabel.Content = "Мин: Не указано";
                else
                    demandMinPriceLabel.Content = "Мин: " + value + Helpers.Main.Currency; 
            }
        }
        public string DemandMaxPrice
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    demandMaxPriceLabel.Content = "Макс: Не указано";
                else
                    demandMaxPriceLabel.Content = "Макс: " + value + Helpers.Main.Currency;
            }
        }
        public string DemandMinArea
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    demandMinAreaLabel.Content = "Мин: Не указано";
                else
                    demandMinAreaLabel.Content = "Мин: " + value + Helpers.Main.SquareUnit;
            }
        }
        public string DemandMaxArea
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    demandMaxAreaLabel.Content = "Макс: Не указано";
                else
                    demandMaxAreaLabel.Content = "Макс: " + value + Helpers.Main.SquareUnit;
            }
        }

        public EDealCard() => PullDealInfo();

        private void PullDealInfo()
        {
            StackPanel stack = new StackPanel();
            UniformGrid grid = new UniformGrid() { Rows = 1, Columns = 2 };

            StackPanel offerStack = new StackPanel();
            Label offerLabel = new Label() { Content = "Предложение", FontSize = 14, HorizontalContentAlignment = HorizontalAlignment.Center };
            offerTypeLabel = new Label() { FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            offerAddressLabel = new Label() { FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            offerPriceLabel = new Label() { FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            offerAreaLabel = new Label() { FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            offerStack.Children.Add(offerLabel);
            offerStack.Children.Add(offerTypeLabel);
            offerStack.Children.Add(offerAddressLabel);
            offerStack.Children.Add(offerPriceLabel);
            offerStack.Children.Add(offerAreaLabel);
            grid.Children.Add(offerStack);

            StackPanel demandStack = new StackPanel();
            Label demandLabel = new Label() { Content = "Потребность", FontSize = 14, HorizontalContentAlignment = HorizontalAlignment.Center };
            demandTypeLabel = new Label() { FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            demandAddressLabel = new Label() { Content = "Адрес: ", FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };

            TextBlock priceBlock = new TextBlock();
            Label priceLabel = new Label() { Content = "Стоимость", FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            UniformGrid priceGrid = new UniformGrid() { Rows = 1, Columns = 2 };
            demandMinPriceLabel = new Label() { FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            demandMaxPriceLabel = new Label() { FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            priceGrid.Children.Add(demandMinPriceLabel);
            priceGrid.Children.Add(demandMaxPriceLabel);
            priceBlock.Inlines.Add(priceLabel);
            priceBlock.Inlines.Add(priceGrid);

            TextBlock areaBlock = new TextBlock();
            Label areaLabel = new Label() { Content = "Площадь", FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            UniformGrid areaGrid = new UniformGrid() { Rows = 1, Columns = 2 };
            demandMinAreaLabel = new Label() { FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            demandMaxAreaLabel = new Label() { FontSize = 12, Foreground = FindResource("Grey_4") as SolidColorBrush };
            areaGrid.Children.Add(demandMinAreaLabel);
            areaGrid.Children.Add(demandMaxAreaLabel);
            areaBlock.Inlines.Add(areaLabel);
            areaBlock.Inlines.Add(areaGrid);

            demandStack.Children.Add(demandLabel);
            demandStack.Children.Add(demandTypeLabel);
            demandStack.Children.Add(demandAddressLabel);
            demandStack.Children.Add(priceBlock);
            demandStack.Children.Add(areaBlock);
            grid.Children.Add(demandStack);
            stack.Children.Add(grid);
            this.AddChild(stack);
        }
    }
}
