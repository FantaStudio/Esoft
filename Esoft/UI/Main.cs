using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shapes;

namespace Esoft.UI.Main
{
    // основные используемые размеры
    static public class Sizes
    {
        public enum Main
        {
            Small,
            Default,
            Large
        }
    }

    // основные статусы
    static public class Statuses
    {
        public enum Main
        {
            Success,
            Warning,
            Danger,
            Default,
        }
    }

    // Кастомные окна
    public class EWindow : Window
    {
        //private HwndSource _hwndSource;
        public bool IsClosed { get; private set; }
        static EWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EWindow),
                new FrameworkPropertyMetadata(typeof(EWindow)));
        }
        public EWindow() : base()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            PreviewMouseMove += OnPreviewMouseMove;
            IsClosed = false;
        }

        protected void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                Cursor = Cursors.Arrow;
        }
        
        /*
        protected override void OnInitialized(EventArgs e)
        {
            SourceInitialized += OnSourceInitialized;
            base.OnInitialized(e);
        }

        private void OnSourceInitialized(object sender, EventArgs e)
        {
            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);
        }
        */

        // Свернуть
        protected void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Открыть на весь экран
        protected void RestoreClick(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        // Закрыть
        protected void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public override void OnApplyTemplate()
        {
            Button minimizeButton = GetTemplateChild("minimizeButton") as Button;
            if (minimizeButton != null)
                minimizeButton.Click += MinimizeClick;

            Button restoreButton = GetTemplateChild("restoreButton") as Button;
            if (restoreButton != null)
                restoreButton.Click += RestoreClick;

            Button closeButton = GetTemplateChild("closeButton") as Button;
            if (closeButton != null)
                closeButton.Click += CloseClick;

            StackPanel rect = GetTemplateChild("moveRectangle") as StackPanel;
            if (rect != null)
            {
                rect.PreviewMouseDown += MoveRectangle_PreviewMouseDown;
            }

            base.OnApplyTemplate();
        }

        // Перемещение окна
        private void MoveRectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            IsClosed = true;
        }
    }

    public class EUserWindow : EWindow
    {
        public Helpers.EUser CurrentUser { get; set; }

        public Helpers.EObject CurrentObject { get; set; }

        public EUserWindow() : base() => CurrentUser = new Helpers.EUser();
        public EUserWindow(int userID, Helpers.Main.UserType userType) => CurrentUser = new Helpers.EUser(userID, userType);

        public EUserWindow(Helpers.EUser user) => CurrentUser = user;
        public EUserWindow(Helpers.EUser user, Helpers.EObject obj)
        {
            CurrentObject = obj;
            CurrentUser = user;
        }
    }
}
