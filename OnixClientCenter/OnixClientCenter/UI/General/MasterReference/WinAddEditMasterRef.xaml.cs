using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.General.MasterReference
{
    public partial class WinAddEditMasterRef : WinBase
    {
        public WinAddEditMasterRef(CWinLoadParam param) : base(param)
        {
            accessRightName = "GENERAL_MASTER_EDIT";

            createAPIName = "CreateMasterRef";
            updateAPIName = "UpdateMasterRef";
            getInfoAPIName = "GetMasterRefInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblCode, txtCode, false);
            registerValidateControls(lblDesc, txtDesc, false);
        }

        protected override MBaseModel createObject()
        {
            MMasterRef mv = new MMasterRef(new CTable(""));

            mv.RefType = loadParam.GenericType;

            return (mv);
        }
        
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                IsOKClick = true;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }
    }
}
