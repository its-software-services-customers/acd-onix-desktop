using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Windows;

namespace Onix.ClientCenter.UI.Cash.Cheque
{
    public partial class WinAddEditCheque : WinBase
    {
        public WinAddEditCheque(CWinLoadParam param) : base(param)
        {
            accessRightName = "CASH_CHEQUE_EDIT";

            createAPIName = "CreateCheque";
            updateAPIName = "UpdateCheque";
            getInfoAPIName = "GetChequeInfo";
            approveAPIName = "ApproveCheque";
            verifyAPIName = "VerifyCheque";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblNumber, txtNumber, false);
            registerValidateControls(lblChequeBank, cboChequeBank, false);
            registerValidateControls(lblBank, cboAccount, false);
            registerValidateControls(lblAmount, txtAmount, false);
        }

        public Boolean IsPreviewNeed
        {
            get
            {
                return (loadParam.GenericType.Equals("2"));
            }
        }

        public Boolean IsAr
        {
            get
            {
                return (loadParam.GenericType.Equals("1"));
            }
        }

        public Boolean IsAp
        {
            get
            {
                return (!IsAr);
            }
        }

        public LookupSearchType2 LookupType
        {
            get
            {
                if (IsAr)
                {
                    return (LookupSearchType2.CustomerLookup);
                }

                return (LookupSearchType2.SupplierLookup);
            }
        }

        public String EntityName
        {
            get
            {
                if (IsAr)
                {
                    return (CLanguage.getValue("customer_name"));
                }

                return (CLanguage.getValue("supplier_name"));
            }
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

        protected override MBaseModel createObject()
        {
            MCheque av = (MCheque) loadParam.ActualView;

            MCheque mv = new MCheque(new CTable(""));

            mv.CreateDefaultValue();
            mv.ChequeDate = DateTime.Now;
            mv.Direction = loadParam.GenericType;

            if (av != null)
            {
                //Default value
                mv.EntityObj = av.EntityObj;
                mv.PayeeName = av.PayeeName;
                mv.ChequeAmount = av.ChequeAmount;
            }

            mv.IssueDate = DateTime.Now;
            mv.ChequeDate = DateTime.Now;
            mv.ChequeStatus = ((int)InventoryDocumentStatus.InvDocPending).ToString();
            mv.IsAcPayeeOnly = true;
            mv.AllowNegative = CGlobalVariable.IsCashNegativeAllow();

            return (mv);
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
        
        private void cmdPreview_Click(object sender, RoutedEventArgs e)
        {
            String group = "grpChequePayment";
            if (loadParam.GenericType.Equals("2"))
            {
                WinFormPrinting w = new WinFormPrinting(group, vw);
                w.ShowDialog();
            }
        }

        private void cboBank_GotFocus(object sender, RoutedEventArgs e)
        {
            MCheque mv = (MCheque) vw;
            if (loadParam.GenericType.Equals("1"))
            {
                return;
            }

            MMasterRef cb = mv.ChequeBankObj;
            MMasterRef accBank = mv.BankObj;
 
            MMasterRef bank = new MMasterRef(new CTable(""));
            bank.MasterID = cb.MasterID;
            bank.Code = cb.Code;
            bank.Description = cb.Description;

            mv.BankObj = bank;
        }
    }
}
