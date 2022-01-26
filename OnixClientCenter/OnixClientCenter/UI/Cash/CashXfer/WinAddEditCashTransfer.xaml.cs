using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.UI.Cash.CashXfer
{  
    public partial class WinAddEditCashTransfer : WinBase
    {
        public WinAddEditCashTransfer(CWinLoadParam param) : base(param)
        {
            accessRightName = "CASH_XFER_EDIT";

            createAPIName = "CreateCashDoc";
            updateAPIName = "UpdateCashDoc";
            getInfoAPIName = "GetCashDocInfo";
            approveAPIName = "ApproveCashDoc";
            verifyAPIName = "VerifyCashDoc";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblNumber, txtNumber, false);
            registerValidateControls(lblFromAcc, cboFromAccount, false);
            registerValidateControls(lblToAccount, cboToAccount, false);
            registerValidateControls(lblToAccount, cboToAccount, false);
            registerValidateControls(lblAmount, txtAmount, false);
        }

        protected override MBaseModel createObject()
        {
            MCashDocXfer mv = new MCashDocXfer(new CTable(""));

            mv.CashXferType = "1";
            mv.DocumentType = loadParam.GenericType;
            mv.AllowNegative = CGlobalVariable.IsInventoryNegativeAllow();
            mv.DocumentDate = DateTime.Now;
            mv.CreateDefaultValue();

            return (mv);
        }

        protected override bool postValidate()
        {
            MCashDocXfer mv = (MCashDocXfer)vw;

            if (mv.FromAccountID.Equals(mv.ToAccountID))
            {
                CHelper.ShowErorMessage("", "ERROR_XFER_SAME_ACCOUNT", null);
                return (false);
            }

            return (true);
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
        
        private void cmdVerify_Click(object sender, RoutedEventArgs e)
        {
            verifyData();
        }

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = approveData();
            if (r)
            {
                CUtil.EnableForm(true, this);
                vw.IsModified = false;
                this.Close();
            }
        }        
    }
}
