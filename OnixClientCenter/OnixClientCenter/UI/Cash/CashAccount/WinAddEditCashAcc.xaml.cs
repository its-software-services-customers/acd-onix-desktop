using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.Cash.CashAccount
{
    public partial class WinAddEditCashAcc : WinBase
    {
        public WinAddEditCashAcc(CWinLoadParam param) : base(param)
        {
            accessRightName = "CASH_ACCOUNT_EDIT";

            createAPIName = "CreateCashAccount";
            updateAPIName = "UpdateCashAccount";
            getInfoAPIName = "GetCashAccountInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblAccNo, txtAccNo, false);
            registerValidateControls(lblAccName, txtAccName, false);
            registerValidateControls(lblBank, cboBank, false);
        }         

        protected override MBaseModel createObject()
        {
            MCashAccount mv = new MCashAccount(new CTable(""));

            mv.CreateDefaultValue();
            mv.IsForOwner = false;

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
