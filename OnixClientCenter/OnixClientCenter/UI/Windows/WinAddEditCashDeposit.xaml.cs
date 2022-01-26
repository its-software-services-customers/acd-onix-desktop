using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using System.Windows.Input;
using Onix.ClientCenter.Windows;
using System.Text.RegularExpressions;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.UControls;
using System.Collections;
using Onix.ClientCenter.Criteria;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.UI.Cash.Cheque;

namespace Onix.ClientCenter
{
    public partial class WinAddEditCashDeposit : Window
    {
        private MAccountDoc vw = null;
        private MAccountDoc actualView = null;
        private ObservableCollection<MBaseModel> parentItemsSource = null;
        private MAccountDocReceipt currentViewObj = null;

        private Hashtable priceHash = new Hashtable();
        private AccountDocumentType dt;

        public String Mode = "";

        private Boolean isInLoadData = false;
        private Boolean isBillCorrection = false;
        private Boolean needPreview = false;
        private String createdID = "0";

        public String CreatedID
        {
            get
            {
                return (createdID);
            }
        }

        public WinAddEditCashDeposit(String md, AccountDocumentType docType, ObservableCollection<MBaseModel> pItems, MAccountDoc actView)
        {   
            dt = docType;

            actualView = actView;
            parentItemsSource = pItems;
            Mode = md;

            vw = new MAccountDoc(new CTable(""));
            vw.DocumentType = ((int)dt).ToString();
            DataContext = vw;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public String ChequeCaption
        {
            get
            {
                String caption = CLanguage.getValue("receivable_cheque");
                if (dt == AccountDocumentType.AcctDocCashDepositAr)
                {
                    caption = CLanguage.getValue("payable_cheque");
                }

                return (caption);
            }

            set
            {
            }
        }

        public String EntityNameCaption
        {
            get
            {
                String caption = CLanguage.getValue("supplier_name");
                if (dt == AccountDocumentType.AcctDocCashDepositAr)
                {
                    caption = CLanguage.getValue("customer_name");
                }

                return (caption);
            }

            set
            {
            }
        }

        public LookupSearchType2 EntityLookupType
        {
            get
            {
                LookupSearchType2 lk = LookupSearchType2.SupplierLookup;
                if (dt == AccountDocumentType.AcctDocCashDepositAr)
                {
                    lk = LookupSearchType2.CustomerLookup;
                }

                return (lk);
            }

            set
            {
            }
        }

        public AccountDocumentType DocType
        {
            get
            {
                return (dt);
            }

            set
            {
                dt = value;
            }
        }

        public Boolean IsAr
        {
            get
            {
                return (DocType == AccountDocumentType.AcctDocArReceipt);
            }
        }

        public Boolean IsAp
        {
            get
            {
                return (!IsAr);
            }
        }

        public Boolean IsPreviewNeed
        {
            get
            {
                return (needPreview);
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateLookup(lblCustomer, uCustomer, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblDesc, txtDesc, true);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateComboBox(lblBranch, cboBranch, false);
            if (!result)
            {
                return (result);
            }

            result = validateReceiptItem();
            if (!result)
            {
                return (result);
            }

            return (result);
        }

        private Boolean validatePayment()
        {
            int cnt = 0;
            double total = 0;
            foreach (MAccountDocPayment pmt in vw.PaymentItems)
            {
                if (pmt.ExtFlag.Equals("D"))
                {
                    continue;
                }

                total = total + CUtil.StringToDouble(pmt.PaidAmount);
                cnt++;
            }

            if (cnt <= 0)
            {
                CMessageBox.Show(CLanguage.getValue("ERROR_NO_PAYMENT"), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return (false);
            }

            double amt = CUtil.StringToDouble(vw.CashReceiptAmt);
            if (total < amt)
            {
                CMessageBox.Show(CLanguage.getValue("ERROR_PAYMENT_NOT_ENOUGH"), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return (false);
            }

            return (true);
        }

        private Boolean validateReceiptItem()
        {
            int idx = 0;
            foreach (MAccountDocDeposit rcp in vw.DepositItems)
            {
                if (rcp.ExtFlag.Equals("D"))
                {
                    continue;
                }

                idx++;
                if (rcp.Note.Trim().Equals(""))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);                    
                    return (false);
                }

                if (CUtil.StringToDouble(rcp.DepositAmt) <= 0)
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    return (false);
                }
            }

            if (CUtil.StringToDouble(vw.CashReceiptAmt) < 0)
            {
                //Equal zero is fine
                CMessageBox.Show(CLanguage.getValue("ERROR_RECEIPT_AMOUNT"), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return (false);
            }

            return (true);
        }

        private Boolean SaveToView( )
        {
            if (!ValidateData())
            {
                return(false);
            }

            return (true);
        }

        private CTable updateDocumentWrapper()
        {
            CTable m = null;

            m = OnixWebServiceAPI.UpdateAccountDoc(vw.GetDbObject());

            return (m);
        }

        private CTable approveAccountDocWrapper()
        {
            CTable m = null;

            m = OnixWebServiceAPI.ApproveAccountDoc(vw.GetDbObject().CloneAll());

            return (m);
        }
        
        private Boolean SaveData(String approveFlag)
        {
            //if (!CHelper.VerifyAccessRight("INVENTORY_ITEM_EDIT"))
            //{
            //    return (false);
            //}

            vw.DocumentType = ((int)dt).ToString();

            if (approveFlag.Equals("Y"))
            {
                Boolean result = SaveToView();
                if (!result)
                {
                    return (false);
                }

                CUtil.EnableForm(false, this);
                CTable t = approveAccountDocWrapper();

                CUtil.EnableForm(true, this);
                if (t != null)
                {
                    //Intend to use MInventoryDoc
                    MInventoryDoc vcd = new MInventoryDoc(t);
                    vcd.InitErrorItem();
                    if (vcd.ErrorItems.Count > 0)
                    {
                        WinErrorDetails w = new WinErrorDetails(vcd.ErrorItems, "InventoryDoc");
                        w.Title = CLanguage.getValue("approve_error");
                        w.ShowDialog();
                    }
                    else
                    {
                        if (Mode.Equals("A"))
                        {
                            vw.SetDbObject(t);
                            vw.DocumentStatus = ((int)CashDocumentStatus.CashDocApproved).ToString();
                            (vw as MAccountDoc).NotifyAllPropertiesChanged();
                            createdID = vw.AccountDocId;
                            parentItemsSource.Insert(0, vw);
                        }
                        else if (Mode.Equals("E"))
                        {
                            if (!isBillCorrection)
                            {
                                actualView.SetDbObject(t);
                                actualView.DocumentStatus = ((int)CashDocumentStatus.CashDocApproved).ToString();
                                (actualView as MAccountDoc).NotifyAllPropertiesChanged();
                            }
                        }

                        vw.IsModified = false;
                        this.Close();
                    }
                }
            }
            else if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    CUtil.EnableForm(false, this);
                    vw.CustomerObj = uCustomer.SelectedObject;
                    CTable newobj = OnixWebServiceAPI.CreateAccountDoc(vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (newobj != null)
                    {
                        vw.SetDbObject(newobj);
                        parentItemsSource.Insert(0, vw);

                        return (true);
                    }

                    //Error here
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                    return (false);
                }
            }
            else if (Mode.Equals("E"))
            {
                if (vw.IsModified)
                {
                    Boolean result = SaveToView();
                    if (result)
                    {
                        CUtil.EnableForm(false, this);
                        vw.CustomerObj = uCustomer.SelectedObject;
                        CTable t = updateDocumentWrapper();
                        CUtil.EnableForm(true, this);
                        if (t != null)
                        {
                            actualView.SetDbObject(t);
                            (actualView as MAccountDoc).NotifyAllPropertiesChanged();

                            return (true);
                        }

                        CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_EDIT", null);
                    }

                    return (false);
                }

                return (true);
            }

            return (false);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveData("N");
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!vw.IsEditable)
            {
                return;
            }

            cmdOK.Focus();
            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    Boolean r = SaveData("N");
                    e.Cancel = !r;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private CTable getDocInfoWrapper()
        {
            CTable m = null;

            actualView.EntityAddressFlag = "Y";
            m = OnixWebServiceAPI.GetAccountDocInfo(actualView.GetDbObject());

            return (m);
        }

        private void LoadData()
        {
            isInLoadData = true;

            dtFromDate.Focus();

            vw.CreateDefaultValue();

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable m = getDocInfoWrapper();
                if (m != null)
                {
                    vw.SetDbObject(m);                    
                    vw.InitAccountDocPayment();
                    vw.InitAccountDocDeposit();
                    String tmp = vw.EntityAddressID;
                    vw.InitEntityAddresses();
                    vw.EntityAddressID = tmp;
                    vw.AddressObj = CUtil.MasterIDToObject(vw.EntityAddresses, vw.EntityAddressID);

                    vw.NotifyAllPropertiesChanged();                    
                }
            }
            else if (Mode.Equals("A"))
            {
                vw.DocumentDate = DateTime.Now;
                vw.VAT_PCT = CGlobalVariable.GetGlobalVariableValue("VAT_PERCENTAGE");;
                vw.AllowCashNegative = CGlobalVariable.IsCashNegativeAllow();
                vw.AllowARAPNegative = CGlobalVariable.IsArApNegativeAllow();
                vw.AllowInventoryNegative = CGlobalVariable.IsInventoryNegativeAllow();
            }

            vw.IsModified = false;
            isInLoadData = false;

            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();            
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {           
            vw.IsModified = true;            
        }

        private void dtFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MAccountDocReceipt)btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            Boolean result = validatePayment();
            if (!result)
            {
                return;
            }

            result = validateReceiptItem();
            if (!result)
            {
                return;
            }

            vw.IsModified = true;
            Boolean r = SaveData("Y");
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

            Boolean r = SaveData("N");
            if (r)
            {
                actualView = vw;
                Mode = "E";

                LoadData();

                vw.IsModified = false;
            }
        }

        private CTable verifyAccountDocWrapper()
        {
            CTable m = null;

            m = OnixWebServiceAPI.VerifyAccountDoc(vw.GetDbObject().CloneAll());

            return (m);
        }

        private void cmdVerify_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveToView();
            if (!r)
            {
                return;
            }

            CUtil.EnableForm(false, this);
            vw.DocumentType = ((int)dt).ToString();
            CTable t = verifyAccountDocWrapper();
            CUtil.EnableForm(true, this);

            if (t != null)
            {
                MInventoryDoc ivd = new MInventoryDoc(t);
                ivd.InitErrorItem();
                if (ivd.ErrorItems.Count > 0)
                {
                    WinErrorDetails w = new WinErrorDetails(ivd.ErrorItems, "InventoryDoc");
                    w.Title = CLanguage.getValue("approve_error");
                    w.ShowDialog();
                }
                else
                {
                    String msg = CLanguage.getValue("VERIFY_SUCCESS");
                    CMessageBox.Show(msg, "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void cmdAddProduct_Click(object sender, RoutedEventArgs e)
        {
            MAccountDocDeposit dep = new MAccountDocDeposit(new CTable(""));
            dep.RatioType = "2";
            vw.AddAccountDocDeposit(dep);

            vw.CalculateDopositTotal();
            vw.IsModified = true;
        }

        private void uCustomer_SelectedObjectChanged(object sender, EventArgs e)
        {
            if (!isInLoadData)
            {
                CTable cust = OnixWebServiceAPI.GetEntityInfo(vw.CustomerObj.GetDbObject());
                MEntity en = new MEntity(cust);
                en.InitEntityAddress();

                vw.ReloadEntityAddresses(en.AddressItems);

                if (en.AddressItems.Count > 0)
                {
                    cboAddress.SelectedIndex = 0;
                }

                vw.IsModified = true;
            }

        }

        private void cboAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void cbxAllowNegative_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxAllowNegative_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void dtDueDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdPayment_Click(object sender, RoutedEventArgs e)
        {
            vw.ConstructWhDefinitionFromReceiptItem();

            WinAddEditPayment w = new WinAddEditPayment(vw.CashActualReceiptAmt, vw, dt == AccountDocumentType.AcctDocCashDepositAr);
            w.ShowDialog();

            if (w.IsOK)
            {
                vw.IsModified = true;
                vw.CalculateReceiveAndChange();
            }
        }

        private void cmdApprovePrint_Click(object sender, RoutedEventArgs e)
        {
            needPreview = true;
            cmdApprove_Click(sender, e);
        }

        private void cmdPreview_Click(object sender, RoutedEventArgs e)
        {
            //String group = "grpSaleArReceipt";
            //if (dt == AccountDocumentType.AcctDocApReceipt)
            //{
            //    group = "grpPurchaseApReceipt";
            //}

            //WinFormPrinting w = new WinFormPrinting(group, vw);
            //w.ShowDialog();
        }

        private void cboBranch_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdCheque_Click(object sender, RoutedEventArgs e)
        {
            Boolean result = CHelper.ValidateLookup(lblCustomer, uCustomer, false);
            if (!result)
            {
                return;
            }

            String type = "2";
            if (dt == AccountDocumentType.AcctDocArReceipt)
            {
                type = "1";
            }

            if (vw.ChequeID.Equals(""))
            {
                ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();
                CCriteriaCheque.ShowAddChequeWindow(type, arr, vw);

                if (arr.Count > 0)
                {
                    MCheque cq = (MCheque)arr[0];
                    vw.ChequeID = cq.ChequeID;
                    vw.IsModified = true;
                }
            }
            else
            {
                CCriteriaCheque.ShowEditWindow(type, null, vw);
            }
        }

        private void radType1_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdDepositDelete_Click(object sender, RoutedEventArgs e)
        {
            MAccountDocDeposit v = (MAccountDocDeposit)(sender as Button).Tag;
            vw.RemoveAccountDocDeposit(v);

            vw.CalculateDopositTotal();
            vw.CalculateReceiveAndChange();

            vw.IsModified = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isInLoadData)
            {
                return;
            }

            vw.CalculateDopositTotal();
            vw.CalculateReceiveAndChange();

            vw.IsModified = true;
        }
    }
}
