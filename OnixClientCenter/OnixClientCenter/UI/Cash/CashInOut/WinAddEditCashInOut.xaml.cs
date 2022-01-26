using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.Cash.CashInOut
{
    public partial class WinAddEditCashInOut : WinBase
    {
        private CashDocumentType docType = CashDocumentType.CashDocImport;

        public WinAddEditCashInOut(CWinLoadParam param) : base(param)
        {
            docType = ((CashDocumentType) CUtil.StringToInt(param.GenericType));

            if (docType == CashDocumentType.CashDocImport)
            {
                accessRightName = "CASH_IN_EDIT";
            }
            else if (docType == CashDocumentType.CashDocExport)
            {
                accessRightName = "CASH_OUT_EDIT";
            }

            createAPIName = "CreateCashDoc";
            updateAPIName = "UpdateCashDoc";
            getInfoAPIName = "GetCashDocInfo";
            approveAPIName = "ApproveCashDoc";
            verifyAPIName = "VerifyCashDoc";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblNumber, txtNumber, false);
            registerValidateControls(lblBank, cboAccount, false);
            registerValidateControls(lblAmount, txtAmount, false);
        }

        protected override MBaseModel createObject()
        {
            MCashDoc mv = null;
            if (docType == CashDocumentType.CashDocImport)
            {
                mv = new MCashDocIn(new CTable(""));
            }
            else if (docType == CashDocumentType.CashDocExport)
            {
                mv = new MCashDocOut(new CTable(""));
            }

            mv.CreateDefaultValue();
            mv.DocumentDate = DateTime.Now;
            mv.DocumentType = ((Int32)docType).ToString();
            mv.IsInternalDoc = false;
            mv.DocumentStatus = ((int)CashDocumentStatus.CashDocPending).ToString();
            mv.AllowNegative = CGlobalVariable.IsCashNegativeAllow();

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
