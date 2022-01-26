using System;
using System.Windows;


namespace Onix.ClientCenter.Commons.Utils
{
    public static class CMessageBox
    {
        public static MessageBoxResult Show(string text)
        {
            return Show(text, "Message Box", MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string text, string title)
        {
            return Show(text, title, MessageBoxButton.OK, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string text, string title, MessageBoxButton buttons)
        {
            return Show(text, title, buttons, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(string text, MessageBoxButton buttons, MessageBoxImage imgs)
        {
            return Show(text, "Message Box", buttons, MessageBoxImage.None);
        }

        public static MessageBoxResult Show(String txt, String title, MessageBoxButton buttons, MessageBoxImage imgs)
        {
            WinMessageBox msg = new WinMessageBox(txt, title, buttons, imgs);
            msg.ShowDialog();
            return msg.ReturnResult;
        }
    }
}
