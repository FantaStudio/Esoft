using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Esoft.UI.Main;

namespace Esoft.UI.Buttons
{
    // Основная кнопка
    public class EButton : Button
    {

        public static readonly DependencyProperty ESizeProperty = DependencyProperty.Register("ESize", typeof(Sizes.Main), typeof(EButton), new PropertyMetadata(Sizes.Main.Default));

        public Sizes.Main ESize
        {
            get { return (Sizes.Main)GetValue(ESizeProperty); }
            set
            {
                SetValue(ESizeProperty, value);
            }
        }

        public EButton() { }
    }

    // Кнопка статуса ( просто меняет цвет, в зависимости от него )
    public class EStatusButton : EButton
    {
        public static readonly DependencyProperty EStatusProperty = DependencyProperty.Register("EStatus", typeof(Statuses.Main), typeof(EStatusButton), new PropertyMetadata(Statuses.Main.Success));
        public Statuses.Main EStatus
        {
            get { return (Statuses.Main)GetValue(EStatusProperty); }
            set
            {
                SetValue(EStatusProperty, value);
            }
        }
        public EStatusButton() { }
    }
}
