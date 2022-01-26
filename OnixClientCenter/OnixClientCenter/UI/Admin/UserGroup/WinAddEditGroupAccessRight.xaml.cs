using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using System.Collections;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.Admin.UserGroup
{
    public partial class WinAddEditGroupAccessRight : WinBase
    {
        private ObservableCollection<MAccessRight> permissions = new ObservableCollection<MAccessRight>();

        public WinAddEditGroupAccessRight(CWinLoadParam param) : base(param)
        {
            accessRightName = "ADMIN_GROUP_EDIT";

            createAPIName = "N/A";
            updateAPIName = "N/A";
            getInfoAPIName = "GetUserGroupInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblCode, txtCode, false);

            double[] ratios = new double[3] { 0.20, 0.70, 0.10 };
            registerListViewSize(lsvImportItem.Name, ratios);
        }

        protected override MBaseModel createObject()
        {
            MAccessRight mv = new MAccessRight(new CTable(""));
            return (mv);
        }

        public String ItemText
        {
            get
            {
                return (CLanguage.getValue("access_right"));
            }

            set
            {
            }
        }

        public ObservableCollection<MAccessRight> AccessRights
        {
            get
            {
                return (permissions);
            }

            set
            {
            }
        }        

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }  

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            permissions.Clear();

            CUtil.EnableForm(false, this);
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetGroupAccessRightList", "GROUP_ACCESS_RIGHTS", vw.GetDbObject());
            CUtil.EnableForm(true, this);

            foreach (CTable tb in arr)
            {
                MAccessRight ar = new MAccessRight(tb);
                permissions.Add(ar);
            }
        }

        private void updateIsEnable(object obj)
        {
            CheckBox cbx = (CheckBox) obj;
            MAccessRight ac = (MAccessRight) cbx.Tag;

            CUtil.EnableForm(false, this);
            CTable dat = OnixWebServiceAPI.SubmitObjectAPI("UpdateGroupAccessRight", ac.GetDbObject());
            CUtil.EnableForm(true, this);
            if (dat != null)
            {
                ac.SetDbObject(dat);
            }
        }

        private void CbkRemove_Unchecked(object sender, RoutedEventArgs e)
        {
            updateIsEnable(sender);
        }

        private void CbkRemove_Checked(object sender, RoutedEventArgs e)
        {
            updateIsEnable(sender);
        }
    }
}
