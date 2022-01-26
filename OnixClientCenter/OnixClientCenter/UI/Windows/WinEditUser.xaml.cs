using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinEditUser : Window
    {
        public String Caption = "";
        private MUserView vw = null;

        public WinEditUser()
        {
            CTable u = new CTable("USER");

            vw = new MUserView(u);
            DataContext = vw;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = Caption;
            txtOldPassword.Focus();
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidatePasswordBox(lblOldPassword, txtOldPassword, true);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidatePasswordBox(lblNewPassword, txtPassword, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidatePasswordBox(lblConfirmPassword, txtConfirm, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateConfirmPassword(lblNewPassword, txtPassword, lblConfirmPassword, txtConfirm);
            if (!result)
            {
                return (result);
            }

            return (result);
        }

        private Boolean SaveToView(MUserView v)
        {
            if (!ValidateData())
            {
                return (false);
            }

            vw.Password = txtOldPassword.Password;
            vw.NewPassword = txtPassword.Password;
             
            return (true);
        }

        private Boolean SaveData()
        {
            Boolean result = SaveToView((MUserView)vw);
            if (result)
            {
                CUtil.EnableForm(false, this);
                CTable t = OnixWebServiceAPI.ChangePassword(vw.GetDbObject());
                CUtil.EnableForm(true, this);
                if (t != null)
                {
                    return (true);
                }

                CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_EDIT", null);
            }

            return (false);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveData();
            if (r)
            {
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
