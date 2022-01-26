using System;
using System.Windows;
using Wis.WsClientAPI;
using System.Windows.Input;
using System.Windows.Controls;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinLogin : Window
    {
        private Boolean isLogin = false;
        public Boolean LoginOK = false;

        public WinLogin()
        {
            String theme = CConfig.GetTheme();
            if (theme == null)
            {
                theme = "2";
                CConfig.SetTheme(theme);
            }

            InitializeComponent();
            CHelper.LoadTheme(cboTheme, false, theme);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
            txtUserName.Focus();            
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            isLogin = false;

            Application.Current.Shutdown();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            CTable user = new CTable("USER");
            user.SetFieldValue("USER_NAME", txtUserName.Text);
            user.SetFieldValue("PASSWORD", txtPassword.Password);

            CUtil.EnableForm(false, this);
            Boolean result = OnixWebServiceAPI.Login(user);
            CUtil.EnableForm(true, this);

            if (result)
            {
                isLogin = true;
                LoginOK = true;
                this.Close();

                return;
            }

            CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_LOGIN", null);
        }

        private void txtBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {            
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                PasswordBox t = (PasswordBox) sender;
                t.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isLogin)
            {
                CConfig.ConfigWrite();
                Application.Current.Shutdown();
            }
        }

        private void radThai_Checked(object sender, RoutedEventArgs e)
        {
            CTextLabel.SetLanguage("TH");
        }

        private void radEng_Checked(object sender, RoutedEventArgs e)
        {
            CTextLabel.SetLanguage("EN");
        }

        private void cboTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbo = (ComboBox) sender;
            MMasterRef m = (MMasterRef) cbo.SelectedItem;
            if (m != null)
            {
                CThemes.SetTheme(m.Description);
                CConfig.SetTheme(m.MasterID);
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            CTextLabel.SetLanguage(CConfig.GetLanguage());
            String lng = CTextLabel.GetLanguage();
            //CConfig.SetLanguage(lng);

            if (lng.Equals("TH"))
            {
                radThai.IsChecked = true;
            }
            else
            {
                radEng.IsChecked = true;
            }

            OnixWebServiceAPI.Init(CConfig.GetKey(), CConfig.GetUrl());

            CUtil.EnableForm(false, this);
            if (OnixWebServiceAPI.Patch(new CTable("DUMMY")) == null)
            {
                CUtil.EnableForm(true, this);
                CMessageBox.Show(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR", MessageBoxButton.OK);

                WinServerSetting w = new WinServerSetting();
                w.ShowDialog();
            }

            CUtil.EnableForm(true, this);

            if (CHelper.VerifyVersion())
            {
                //Comment this line below in order to test auto update from IDE
                return;
            }

            String api = CConfig.APIVersion;

            if (!api.Equals(""))
            {
                CUtil.AutoUpdateProgram("OnixClientCenter.exe", "OnixCenter.zip");
            }

            isLogin = false;
            this.Close();
        }

        private void cmdSetting_Click(object sender, RoutedEventArgs e)
        {
            WinServerSetting w = new WinServerSetting();
            w.ShowDialog();
        }
    }
}
