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
using System.Collections;
using Onix.ClientCenter.Criteria;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.UI.Cash.Cheque;

namespace Onix.ClientCenter
{
    public partial class WinAddEditReceiptDoc : Window
    {
        private MAccountDoc vw = null;
        private MAccountDoc actualView = null;
        private ObservableCollection<MBaseModel> parentItemsSource = null;
        private MAccountDocReceipt currentViewObj = null;

        private Hashtable priceHash = new Hashtable();
        private AccountDocumentType dt;

        public Boolean DialogOK = false;
        public String Caption = "";
        public String Mode = "";

        private Boolean isInLoadData = true;
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

        public WinAddEditReceiptDoc(String md, AccountDocumentType docType, ObservableCollection<MBaseModel> pItems, MAccountDoc actView)
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
                if (dt == AccountDocumentType.AcctDocApReceipt)
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
                if (dt == AccountDocumentType.AcctDocArReceipt)
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
                if (dt == AccountDocumentType.AcctDocArReceipt)
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

            if (vw.IsChangeByDrCr)
            {
                Boolean flag = CHelper.AskConfirmMessage("", "ERROR_CHANGE_BY_DRCR");
                return (flag);
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

            //ยอมให้ติดลบได้แล้วเพื่อสร้างใบเพิ่มหนี้

            //double amt = CUtil.StringToDouble(vw.CashReceiptAmt);
            //if (total < amt)
            //{
            //    CMessageBox.Show(CLanguage.getValue("ERROR_PAYMENT_NOT_ENOUGH"), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return (false);
            //}

            return (true);
        }

        private Boolean validateReceiptItem()
        {
            foreach (MAccountDocReceipt rcp in vw.ReceiptItems)
            {
                if (rcp.ExtFlag.Equals("D"))
                {
                    continue;
                }

                if (!vw.EntityId.Equals(rcp.EntityID))
                {
                    CMessageBox.Show(CLanguage.getValue("ERROR_INVALID_ENTITY") + " " + rcp.DocumentNo, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (vw.ChequeID.Equals(""))
            {
                return (m);
            }

            //Approve cheque as well
            CHelper.ApproveChequeFromAccountDoc(vw);

            return (m);
        }

        private String getAccessRightEdit()
        {
            String acr = "PURCHASE_UNKNOW_EDIT";

            if (dt == AccountDocumentType.AcctDocApReceipt)
            {
                acr = "PURCHASE_RECEIPT_EDIT";
            }
            else if (dt == AccountDocumentType.AcctDocArReceipt)
            {
                acr = "SALE_RECEIPT_EDIT";
            }

            return (acr);
        }

        private Boolean SaveData(String approveFlag)
        {
            if (!CHelper.VerifyAccessRight(getAccessRightEdit()))
            {
                return (false);
            }

            vw.DocumentType = ((int)dt).ToString();
            vw.ConstructWhDefinitionFromReceiptItem();

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
                DialogOK = true;
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

            this.Title = Caption;
            dtFromDate.Focus();

            vw.CreateDefaultValue();

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                //CTable m = OnixWebServiceAPI.GetAccountDocInfo(actualView.GetDbObject());
                CTable m = getDocInfoWrapper();
                if (m != null)
                {
                    vw.SetDbObject(m);
                    vw.InitAccountDocReceipt();
                    vw.InitAccountDocPayment();
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
                vw.IsWhPayType1 = true;
            }

            vw.IsModified = false;
            isInLoadData = false;

            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();            
        }

        private void cbxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            calculateReceiptTotal();
            vw.IsModified = true;
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {           
            vw.IsModified = true;            
        }

        private void dtFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void mnuDocumentEdit_Click(object sender, RoutedEventArgs e)
        {
            lsvAccoutItem_MouseDoubleClick(sender, null);
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

        private void calculateReceiptTotal()
        {
            ObservableCollection<MAccountDocReceipt> items = vw.ReceiptItems;
            double amtTotal = 0.00;
            double whTotal = 0.00;
            double vatTotal = 0.00;
            double revExpTotal = 0.00;
            double revExpForWhTotal = 0.00;
            double revExpForVatTotal = 0.00;
            double finalDiscountTotal = 0.00;
            double pricingAmtTotal = 0.00;
            double cashReceiptAmtTotal = 0.00;

            foreach (MAccountDocReceipt mr in items)
            {
                if (mr.ExtFlag.Equals("D"))
                {
                    continue;
                }

                int factor = 1;
                int docType = CUtil.StringToInt(mr.DocumentType);
                AccountDocumentType dt = (AccountDocumentType)docType;

                if ((dt == AccountDocumentType.AcctDocCrNote) || (dt == AccountDocumentType.AcctDocCrNotePurchase))
                {
                    factor = -1;
                }

                double amt = factor * Math.Abs(CUtil.StringToDouble(mr.ArApAmt));
                double wh = factor * Math.Abs(CUtil.StringToDouble(mr.WHTaxAmt));
                double vatAmt = factor * Math.Abs(CUtil.StringToDouble(mr.VatAmt));
                double revExp = factor * Math.Abs(CUtil.StringToDouble(mr.RevenueExpenseAmt));
                double revExpForWh = factor * Math.Abs(CUtil.StringToDouble(mr.RevenueExpenseForWhAmt));
                double revExpForVat = factor * Math.Abs(CUtil.StringToDouble(mr.RevenueExpenseForVatAmt));
                double finalDiscount = factor * Math.Abs(CUtil.StringToDouble(mr.FinalDiscount));
                double pricingAmt = factor * Math.Abs(CUtil.StringToDouble(mr.PricingAmt));
                double cashReceiptAmt = factor * Math.Abs(CUtil.StringToDouble(mr.CashReceiptAmt));

                revExpTotal = revExpTotal + revExp;
                revExpForWhTotal = revExpForWhTotal + revExpForWh;
                revExpForVatTotal = revExpForVatTotal + revExpForVat;
                amtTotal = amtTotal + amt;
                whTotal = whTotal + wh;
                vatTotal = vatTotal + vatAmt;
                finalDiscountTotal = finalDiscountTotal + finalDiscount;
                pricingAmtTotal = pricingAmtTotal + pricingAmt;
                cashReceiptAmtTotal = cashReceiptAmtTotal + cashReceiptAmt;

                mr.WHTaxAmt = wh.ToString();
                mr.ArApAmt = amt.ToString();
                mr.VatAmt = vatAmt.ToString();
                mr.RevenueExpenseAmt = revExp.ToString();
                mr.FinalDiscount = finalDiscount.ToString();
                mr.PricingAmt = pricingAmt.ToString();
                mr.CashReceiptAmt = cashReceiptAmt.ToString();
            }

            double total = amtTotal - whTotal;

            vw.CashActualReceiptAmt = total.ToString();
            vw.CashReceiptAmt = total.ToString();
            vw.ArApAmt = amtTotal.ToString();
            vw.WHTaxAmt = whTotal.ToString();
            vw.VatAmt = vatTotal.ToString();
            vw.RevenueExpenseAmt = revExpTotal.ToString();
            vw.RevenueExpenseForWhAmt = revExpForWhTotal.ToString();
            vw.RevenueExpenseForVatAmt = revExpForVatTotal.ToString();
            vw.FinalDiscount = finalDiscountTotal.ToString();
            vw.PricingAmt = pricingAmtTotal.ToString();
        }

        private ArrayList convertToReceiptItem(ArrayList items)
        {
            ArrayList temp = new ArrayList();

            foreach (MAccountDoc ma in items)
            {
                MAccountDocReceipt mr = new MAccountDocReceipt(new CTable(""));
                mr.DocumentDate = ma.DocumentDate;
                mr.DocumentNo = ma.DocumentNo;
                mr.DueDate = ma.DueDate;
                mr.WHTaxAmt = ma.WHTaxAmt;
                mr.ArApAmt = ma.ArApAmt;
                mr.DocumentID = ma.AccountDocId;
                mr.DocumentType = ma.DocumentType;
                mr.EntityID = ma.EntityId;
                mr.VatAmt = ma.VatAmt;
                mr.CashReceiptAmt = ma.CashReceiptAmt;
                mr.RevenueExpenseAmt = ma.RevenueExpenseAmt;
                mr.RevenueExpenseForWhAmt = ma.RevenueExpenseForWhAmt;
                mr.RevenueExpenseForVatAmt = ma.RevenueExpenseForVatAmt;
                mr.WhDefinition = ma.WhDefinition;
                mr.ProjectCode = ma.ProjectCode;
                mr.ProjectID = ma.ProjectID;
                mr.FinalDiscount = ma.FinalDiscount;
                mr.RefPoNo = ma.RefPoNo;
                mr.PricingAmt = ma.PricingAmt;

                temp.Add(mr);
            }

            return (temp);
        }

        private void populateExcludeDocSet()
        {
            ObservableCollection<MAccountDocReceipt> items = vw.ReceiptItems;
            int cnt = items.Count;
            int idx = 0;

            String temp = "";
            foreach (MAccountDocReceipt mr in items)
            {
                //Start with 1
                idx++;
                String id = mr.DocumentID.ToString();

                if (idx >= cnt)
                {
                    temp = temp + id;
                }
                else
                {
                    temp = temp + id + ",";
                }                
            }

            if (idx > 0)
            {
                temp = String.Format("({0})", temp);
            }

            vw.ExcludeDocSet = temp;
        }

        private void cmdAddProduct_Click(object sender, RoutedEventArgs e)
        {
            Boolean result = CHelper.ValidateLookup(lblCustomer, uCustomer, false);
            if (!result)
            {
                return;
            }

            populateExcludeDocSet();

            CCriteriaReceiptItem cr = new Criteria.CCriteriaReceiptItem();
            cr.SetActionEnable(false);
            cr.SetDefaultData(vw);

            if (dt == AccountDocumentType.AcctDocArReceipt)
            {
                cr.Init("1");
            }
            else
            {
                cr.Init("2");
            }

            WinMultiSelection w = new WinMultiSelection(cr, CLanguage.getValue("receipt_item"));
            w.ShowDialog();

            if (w.IsOK)
            {
                vw.AddAccountDocReceipts(convertToReceiptItem(w.SelectedItems));
                calculateReceiptTotal();
                vw.IsModified = true;
            }
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

        private void lsvAccoutItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lsvAccoutItem.SelectedItems.Count == 1)
            {
                currentViewObj = (MAccountDocReceipt)lsvAccoutItem.SelectedItems[0];
                ShowEditWindow();
            }
        }

        private void ShowEditWindow()
        {
            MAccountDoc ad = new MAccountDoc(new CTable(""));
            
            ad.DocumentType = currentViewObj.DocumentType;
            ad.AccountDocId = currentViewObj.DocumentID;

            AccountDocumentType dt = (AccountDocumentType) CUtil.StringToInt(ad.DocumentType);
            AccountDocumentType docType = (AccountDocumentType)CUtil.StringToInt(vw.DocumentType);

            if (docType == AccountDocumentType.AcctDocApReceipt)
            {
                CCriteriaAccountDocPurchase.ShowEditWindowEx(dt, ad, null);
            }
            else
            {
                CCriteriaAccountDocSale.ShowEditWindowEx(dt, ad, null);
            }
        }

        private void lsvAccoutItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvAccoutItem.ActualWidth * 1) - 35;
            double[] ratios = new double[10] { 0.05, 0.05, 0.10, 0.12, 0.17, 0.13, 0.08, 0.10, 0.10, 0.10 };
            CUtil.ResizeGridViewColumns(lsvAccoutItem.View as GridView, ratios, w);
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

            WinAddEditPayment w = new WinAddEditPayment(vw.CashActualReceiptAmt, vw, dt == AccountDocumentType.AcctDocArReceipt);
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
            String group = "grpSaleArReceipt";
            if (dt == AccountDocumentType.AcctDocApReceipt)
            {
                group = "grpPurchaseApReceipt";
            }

            WinFormPrinting w = new WinFormPrinting(group, vw);
            w.ShowDialog();
        }

        private void cboBranch_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdCR_Click(object sender, RoutedEventArgs e)
        {
            Boolean result = CHelper.ValidateLookup(lblCustomer, uCustomer, false);
            if (!result)
            {
                return;
            }

            AccountDocumentType crdrDt = AccountDocumentType.AcctDocCrNote;
            if (dt == AccountDocumentType.AcctDocApReceipt)
            {
                crdrDt = AccountDocumentType.AcctDocCrNotePurchase;
            }

            ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();

            WinAddEditDrCrNote w = new WinAddEditDrCrNote("A", crdrDt, itemSources, vw);
            w.Caption = (String)(sender as Button).Content;
            w.ShowDialog();
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

        private void lsvAccoutItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lsvAccoutItem_MouseDoubleClick(sender, null);
            }
        }

        private void radType1_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdWhNo_Click(object sender, RoutedEventArgs e)
        {
            CUtil.GenerateWhDocNumber(vw);
        }
    }
}
