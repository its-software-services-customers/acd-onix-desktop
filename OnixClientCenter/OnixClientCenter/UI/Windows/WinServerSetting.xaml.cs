using System;
using System.Windows.Controls;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinServerSetting : Window
    {
        private Boolean isModified = false;

        #region Data Biding
        private String url = "";
        private String key = "";

        public String UrlLink
        {
            get
            {
                return (url);
            }
            set
            {
                url = value;
            }
        }

        public String KeyPass
        {
            get
            {
                return (key);
            }
            set
            {
                key = value;
            }
        }
        #endregion

        public WinServerSetting()
        {            
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = CLanguage.getValue("server_setting");
            DataContext = this;

            url = CConfig.GetUrl();
            txtDesc.Password = CConfig.GetKey();
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblUrl, txtCode, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidatePasswordBox(lblKey, txtDesc, false);
            if (!result)
            {
                return (result);
            }


            return (result);
        }

        private Boolean SaveToView( )
        {
            if (!ValidateData())
            {
                return(false);
            }

            return (true);
        }

        private Boolean SaveData()
        {
            CConfig.SetKey(txtDesc.Password);
            CConfig.SetUrl(txtCode.Text);
            CConfig.ConfigWrite();

            OnixWebServiceAPI.Init(CConfig.GetKey(), CConfig.GetUrl());

            return (true);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveData();
            if (r)
            {
                isModified = false;
                CUtil.EnableForm(true, this);
                
                this.Close();
            }
   
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            isModified = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isModified)
            {
                Boolean result = CHelper.AskConfirmSave();

                if (result)
                {
                    //Yes, save it
                    Boolean r = SaveData();
                    e.Cancel = !r;
                }
            }
        }

        private void LoadData()
        {
            txtCode.Focus();

            isModified = false;

            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isModified = true;
        }

        private void txtDesc_PasswordChanged(object sender, RoutedEventArgs e)
        {
            isModified = true;
        }

        private void cmdTest_Click(object sender, RoutedEventArgs e)
        {
            OnixConnectionXML dbos = new OnixConnectionXML(txtDesc.Password, url, "pemgail9uzpgzl88");

            String sendString = "HELLOWORLD";
             
            CTable t = new CTable("");
            t.SetFieldValue("ECHO_MESSAGE", sendString);
            CUtil.EnableForm(false, this);
            CTable echo = OnixWebServiceAPI.TestServer(dbos, t);
            CUtil.EnableForm(true, this);

            if (echo == null)
            {
                CMessageBox.Show(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                String returnStr = echo.GetFieldValue("RETURN_MESSAGE");
                if (sendString.Equals(returnStr))
                {
                    CMessageBox.Show(CLanguage.getValue("connect_success"), "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    CMessageBox.Show(CLanguage.getValue("connect_fail"), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        private void MainSvSeting_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
