using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.Admin.User
{
    public partial class WinAddEditUser : WinBase
    {
        public WinAddEditUser(CWinLoadParam param) : base(param)
        {
            accessRightName = "ADMIN_USER_EDIT";

            createAPIName = "CreateUser";
            updateAPIName = "UpdateUser";
            getInfoAPIName = "GetUserInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblUser, txtUserName, false);
            registerValidateControls(lblGroup, cboGroup, false);
            registerValidateControls(lblDescription, txtDescription, true);
        }

        protected override MBaseModel createObject()
        {
            MUserView mv = new MUserView(new CTable(""));
            return (mv);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }        
    }
}
