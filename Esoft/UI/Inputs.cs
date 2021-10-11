using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Esoft.UI.Main;

namespace Esoft.UI.Inputs
{
    public class EInput : TextBox
    {
        public static readonly DependencyProperty PlaceHolderProperty = DependencyProperty.Register("PlaceHolder", typeof(string), typeof(EInput), new PropertyMetadata(""));
        public static readonly DependencyProperty PlaceHolderColorProperty = DependencyProperty.Register("PlaceHolderColor", typeof(SolidColorBrush), typeof(EInput), 
            new PropertyMetadata(new SolidColorBrush() { Color = Colors.Gray }));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(EInput), new PropertyMetadata(""));
        public static readonly DependencyProperty DescriptionColorProperty = DependencyProperty.Register("DescriptionColor", typeof(SolidColorBrush), typeof(EInput),
             new PropertyMetadata(new SolidColorBrush() { Color = Colors.Gray }));
        public static readonly DependencyProperty EStatusProperty = DependencyProperty.Register("EStatus", typeof(Statuses.Main), typeof(EInput), new PropertyMetadata(Statuses.Main.Default));

        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        public SolidColorBrush PlaceHolderColor
        {
            get { return (SolidColorBrush)GetValue(PlaceHolderColorProperty); }
            set { SetValue(PlaceHolderColorProperty, value); }
        }
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public SolidColorBrush DescriptionColor
        {
            get { return (SolidColorBrush)GetValue(DescriptionColorProperty); }
            set { SetValue(DescriptionColorProperty, value); }
        }

        public Statuses.Main EStatus
        {
            get { return (Statuses.Main)GetValue(EStatusProperty); }
            set { SetValue(EStatusProperty, value); }
        }

        public EInput() { }

        // Очищает обводку у EInput
        public void ClearStatus()
        {
            if(EStatus == Statuses.Main.Danger || EStatus == Statuses.Main.Success)
            {
                Description = "";
                EStatus = Statuses.Main.Default;
            }
        }

        // Добавляет событие, которое очищает обводку, объекту, который содержит EInput
        public static void AddClearStatusHandler(DependencyObject obj)
        {
            foreach (EInput input in Helpers.Main.FindVisualChildren<EInput>(obj))
                input.TextChanged += (s, e) => (s as EInput).ClearStatus();
        }
    }
}
