using System;
using System.ComponentModel;
using System.Windows;

namespace Onix.ClientCenter
{
    public partial class WinMessageBox : Window
    {
        private MessageBoxResult result = MessageBoxResult.None;
        private String messageText = "";
        private String titleText = "";
        private MessageBoxImage img = MessageBoxImage.None;

        public MessageBoxResult ReturnResult
        {
            get { return result; }
        }

        public String MessageText
        {
            get
            {
                return (messageText);
            }
        }

        public String TitleText
        {
            get
            {
                return (titleText);
            }
        }

        public String MsgboxIcon
        {
            get
            {
                //Icon should be 64x64 dimention

                String iconName = "";
                if (img == MessageBoxImage.Question)
                {
                    iconName = "pack://application:,,,/OnixClient;component/Images/004-question.png";
                }
                else if (img == MessageBoxImage.Information)
                {
                    iconName = "pack://application:,,,/OnixClient;component/Images/001-information.png";
                }
                else if (img == MessageBoxImage.Error)
                {
                    iconName = "pack://application:,,,/OnixClient;component/Images/exclamation.png";
                }
                
                return (iconName);
            }
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WinMessageBox(string text, string title, MessageBoxButton buttons, MessageBoxImage icon)
        {
            img = icon;
            messageText = text;
            titleText = title;

            DataContext = this;
            InitializeComponent();

            #region Show Button

            //ให้ปรับตรงนี้ไปใช้วิธีการ Binding แทน
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    cmdOK.Visibility = Visibility.Visible;
                    cmdYes.Visibility = Visibility.Collapsed;
                    cmdNo.Visibility = Visibility.Collapsed;
                    cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case MessageBoxButton.OKCancel:
                    cmdOK.Visibility = Visibility.Visible;
                    cmdYes.Visibility = Visibility.Collapsed;
                    cmdNo.Visibility = Visibility.Collapsed;
                    cmdCancel.Visibility = Visibility.Visible;
                    break;

                case MessageBoxButton.YesNo:
                    cmdOK.Visibility = Visibility.Collapsed;
                    cmdYes.Visibility = Visibility.Visible;
                    cmdNo.Visibility = Visibility.Visible;
                    cmdCancel.Visibility = Visibility.Collapsed;
                    break;

                case MessageBoxButton.YesNoCancel:
                    cmdOK.Visibility = Visibility.Collapsed;
                    cmdYes.Visibility = Visibility.Visible;
                    cmdNo.Visibility = Visibility.Visible;
                    cmdCancel.Visibility = Visibility.Visible;
                    break;
            }
            #endregion

            
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.OK;
            this.Close();
        }

        private void cmdYes_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.Yes;
            this.Close();
        }

        private void cmdNo_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.No;
            this.Close();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            result = MessageBoxResult.Cancel;
            this.Close();
        }

        private void MainMsgBox_ContentRendered(object sender, EventArgs e)
        {
           
        }

        private void MainMsgBox_Loaded(object sender, RoutedEventArgs e)
        {
            cmdOK.Focus();
        }
    }
}
