using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.Admin.UserGroup
{
    public partial class WinAddEditUserGroup : WinBase
    {
        public WinAddEditUserGroup(CWinLoadParam param) : base(param)
        {
            accessRightName = "ADMIN_GROUP_EDIT";

            createAPIName = "CreateUserGroup";
            updateAPIName = "UpdateUserGroup";
            getInfoAPIName = "GetUserGroupInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblGroup, txtGroup, false);
        }

        protected override MBaseModel createObject()
        {
            MUserGroup mv = new MUserGroup(new CTable(""));
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
