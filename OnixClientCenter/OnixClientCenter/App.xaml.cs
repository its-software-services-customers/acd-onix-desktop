using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using Onix.ClientCenter.Windows;

namespace Onix.ClientCenter
{
    public partial class App : Application
    {
        public const string ONIX_BUILD_LABEL = "$ONIX_BUILD_LABEL_VAR$-1.2";

        public App() : base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;

            //CultureInfo ci = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            //ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //ci.DateTimeFormat.LongDatePattern = "dd/MM/yyyy";
            //ci.DateTimeFormat.DateSeparator = "/";
            //Thread.CurrentThread.CurrentCulture = ci;
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.PreviewKeyDownEvent, new KeyEventHandler(Element_PreviewKeyDown));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler(TextBox_GotFocus));

            EventManager.RegisterClassHandler(typeof(ComboBox), ComboBox.PreviewKeyDownEvent, new KeyEventHandler(ComboBox_PreviewKeyDown));
            EventManager.RegisterClassHandler(typeof(CheckBox), CheckBox.PreviewKeyDownEvent, new KeyEventHandler(ComboBox_PreviewKeyDown));
            EventManager.RegisterClassHandler(typeof(RadioButton), RadioButton.PreviewKeyDownEvent, new KeyEventHandler(ComboBox_PreviewKeyDown));

            EventManager.RegisterClassHandler(typeof(PasswordBox), PasswordBox.PreviewKeyDownEvent, new KeyEventHandler(ElementPWD_PreviewKeyDown));
            EventManager.RegisterClassHandler(typeof(PasswordBox), PasswordBox.GotFocusEvent, new RoutedEventHandler(PasswordBox_GotFocus));

            EventManager.RegisterClassHandler(typeof(Window), Window.PreviewKeyDownEvent, new KeyEventHandler(Window_PreviewKeyDown));
            //EventManager.RegisterClassHandler(typeof(UserControl), UserControl.PreviewKeyDownEvent, new KeyEventHandler(UControl_PreviewKeyDown));

            base.OnStartup(e);
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as PasswordBox).SelectAll();
        }

        private void ElementPWD_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                if (sender is PasswordBox)
                {
                    e.Handled = true;
                    uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }

        private void UControl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //var uie = e.OriginalSource as UserControl;
            //Boolean mod = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

            //if ((e.Key == Key.LeftShift) && (mod))
            //{
            //}
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Boolean mod = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

            if ((e.Key == Key.LeftShift) && (mod))
            {
                WinDebugXml w = new WinDebugXml(sender as Window);
                w.ShowDialog();
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void ComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void Element_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                if (sender is TextBox)
                {
                    TextBox t = (TextBox) sender;
                    String s = t.Name;

                    if (!s.Equals("txtGeneric"))
                    {
                        e.Handled = true;
                        uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
            }
        }
    }
}
