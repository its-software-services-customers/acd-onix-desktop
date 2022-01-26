using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.Admin.User
{
    public partial class WinAdminEditPassword : WinBase
    {
        public WinAdminEditPassword(CWinLoadParam param) : base(param)
        {
            accessRightName = "ADMIN_USER_EDIT";

            createAPIName = "N/A";
            updateAPIName = "ChangeUserPassword";
            getInfoAPIName = "GetUserInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblNewPassword, txtPassword, false);
            registerValidateControls(lblConfirmPassword, txtConfirm, false);
        }

        protected override MBaseModel createObject()
        {
            MUserView mv = new MUserView(new CTable(""));
            return (mv);
        }

        protected override Boolean postValidate()
        {
            Boolean result = CHelper.ValidateConfirmPassword(lblNewPassword, txtPassword, lblConfirmPassword, txtConfirm);
            return (result);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            MUserView mv = (MUserView)vw; ;
            mv.NewPassword = txtPassword.Password;

            Boolean r = saveData();
            if (r)
            {
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }
    }
}
