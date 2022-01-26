using System;
using System.Windows.Controls;
using System.Collections;
using System.Windows;
using Wis.WsClientAPI;
using System.Windows.Input;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Criteria;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.UI.Cash.CashXfer
{
    public partial class WinAddEditCashExternalXfer : WinBase
    {
        public WinAddEditCashExternalXfer(CWinLoadParam param) : base(param)
        {
            accessRightName = "CASH_XFER_EDIT";

            createAPIName = "CreateCashDoc";
            updateAPIName = "UpdateCashDoc";
            getInfoAPIName = "GetCashDocInfo";
            approveAPIName = "ApproveCashDoc";
            verifyAPIName = "VerifyCashDoc";
            
            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblCode, txtCode, false);
            registerValidateControls(lblFromAcc, cboFromBank, false);
            registerValidateControls(lblFromAcc, cboFromAccount, false);
            registerValidateControls(lblToAccount, cboToBank, false);
            registerValidateControls(lblToAccount, cboToAccount, false);

            double[] ratios = new double[8] { 0.05, 0.05, 0.15, 0.15, 0.15, 0.20, 0.15, 0.10 };
            registerListViewSize(lsvImportItem.Name, ratios);
        }

        protected override MBaseModel createObject()
        {
            MCashDocXfer mv = new MCashDocXfer(new CTable(""));

            mv.CashXferType = "2";
            mv.DocumentType = loadParam.GenericType;
            mv.AllowNegative = false; // CGlobalVariable.IsInventoryNegativeAllow();
            mv.DocumentDate = DateTime.Now;
            mv.CreateDefaultValue();

            return (mv);
        }

        protected override bool postValidate()
        {
            MCashDocXfer mv = (MCashDocXfer) vw;

            if (mv.FromAccountID.Equals(mv.ToAccountID))
            {
                CHelper.ShowErorMessage("", "ERROR_XFER_SAME_ACCOUNT", null);
                return (false);
            }

            return (true);
        }

        public String ItemText
        {
            get
            {
                return (CLanguage.getValue("xfer_item"));
            }

            set
            {
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

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (!vw.IsModified)
            {
                return;
            }

            Boolean r = saveData();
            if (r)
            {
                loadParam.ActualView = vw;
                loadParam.Mode = "E";

                loadData();
                vw.IsModified = false;
            }
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

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            String caption = ItemText;

            CCriteriaExternalPayment cr = new CCriteriaExternalPayment();
            cr.SetActionEnable(false);
            cr.SetDefaultData(vw);
            cr.Init("");

            WinMultiSelection w = new WinMultiSelection(cr, CLanguage.getValue("xfer_item"));
            w.ShowDialog();

            if (w.IsOK)
            {
                addReturnDocItems(w.SelectedItems);
                vw.IsModified = true;
            }
        }

        private void addReturnDocItems(ArrayList items)
        {
            MCashDocXfer mv = (MCashDocXfer)vw;

            //To prevent the duplication selection
            Hashtable hash = CUtil.ObserableCollectionToHash(mv.XferItems, "AccountDocPaymentID");
            double total = 0.00;
            foreach (MAccountDocPayment ai in items)
            {
                if (hash.ContainsKey(ai.AccountDocPaymentID))
                {
                    continue;
                }

                MCashDocXferDetail di = new MCashDocXferDetail(new CTable(""));
                di.SetDbObject(ai.GetDbObject().CloneAll());
                di.ExtFlag = "A";

                total = total + CUtil.StringToDouble(di.PaidAmount);
                mv.AddXferItem(di);
            }

            mv.TotalAmount = total.ToString();
        }
        
        private void cbxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MCashDocXfer mv = (MCashDocXfer)vw;

            if (!isInLoad)
            {
                mv.CalculateTransferTotal();
            }

            mv.IsModified = true;
        }        

        private void mnuDocumentEdit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuDocumentEdit"))
            {
                ShowEditWindow();
            }
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            //currentViewObj = (MInventoryTransaction)btn.Tag;
            //btn.ContextMenu.IsOpen = true;
        }
        
        private void cmdVerify_Click(object sender, RoutedEventArgs e)
        {
            verifyData();
        }

        private void lsvImportItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void ShowEditWindow()
        {
        }
        
        private void cmdPreview_Click(object sender, RoutedEventArgs e)
        {
        }       
    }
}
