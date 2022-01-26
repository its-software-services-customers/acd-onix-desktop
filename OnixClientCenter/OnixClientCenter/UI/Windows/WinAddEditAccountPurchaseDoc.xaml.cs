using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using System.Windows.Input;
using Onix.ClientCenter.Criteria;
using System.Text.RegularExpressions;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.Client.Pricing;
using System.Collections;
using System.Windows.Media;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.UI.Cash.Cheque;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditAccountPurchaseDoc : Window
    {
        private MAccountDoc vw = null;
        private MAccountDoc actualView = null;
        private ObservableCollection<MBaseModel> parentItemsSource = null;
  
        private MAccountDocItem currentViewObj = null;

        private MGlobalVariable vv = new MGlobalVariable(new CTable(""));

        private ObservableCollection<CBasketItemDisplay> resultItems = new ObservableCollection<CBasketItemDisplay>();
        private Hashtable priceHash = new Hashtable();
        private AccountDocumentType dt;

        public Boolean DialogOK = false;
        public String Caption = "";
        public String Mode = "";

        private Boolean isInLoadData = true;

        private RoutedEventHandler itemAddedHandler = null;
        private Boolean needPreview = false;
        private String createdID = "0";

        public Visibility AllowARAPNegativeVisibility
        {
            get
            {
                if (dt == AccountDocumentType.AcctDocCashPurchase)
                {
                    return (Visibility.Collapsed);
                }

                return (Visibility.Visible);
            }

            set
            {
            }
        }

        public RoutedEventHandler ItemAddedHandler
        {
            get
            {
                return (itemAddedHandler);
            }

            set
            {
                itemAddedHandler = value;
            }
        }

        public String CreatedID
        {
            get
            {
                return (createdID);
            }
        }

        public Boolean IsPreviewNeed
        {
            get
            {
                return (needPreview);
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

        public Boolean IsCash
        {
            get
            {
                return (dt == AccountDocumentType.AcctDocCashPurchase);
            }

            set
            {
            }
        }

        public Boolean IsDebt
        {
            get
            {
                return (!IsCash);
            }

            set
            {
            }
        }

        public Visibility PayTypeVisibility
        {
            get
            {
                if (dt == AccountDocumentType.AcctDocDebtPurchase)
                {
                    return (Visibility.Collapsed);
                }

                return (Visibility.Visible);
            }

            set
            {
            }
        }

        private void EnumVisual(Visual myVisual)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(myVisual); i++)
            {
                // Retrieve child visual at specified index value.
                Visual childVisual = (Visual)VisualTreeHelper.GetChild(myVisual, i);

                if (childVisual is Control)
                {
                    Control ctrl = (childVisual as Control);
                    if (ctrl is TextBox)
                    {
                        String nm = (ctrl as TextBox).Name;
                        if (nm.Equals("txtReceiptNo"))
                        {
                            String n = nm;
                        }

                        (ctrl as TextBox).IsEnabled = false;
                    }
                }

                //Enumerate children of the child visual object.
                EnumVisual(childVisual);
            }
        }

        public WinAddEditAccountPurchaseDoc(String md, AccountDocumentType docType, ObservableCollection<MBaseModel> pItems, MAccountDoc actView)
        {
            dt = docType;

            actualView = actView;
            parentItemsSource = pItems;
            Mode = md;

            vw = new MAccountDoc(new CTable(""));
            vw.DocumentType = ((int) dt).ToString();
            vw.IsVatClaimable = true;

            DataContext = vw;

            InitializeComponent();
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
                CTable m = getDocInfoWrapper();
                if (m != null)
                {
                    vw.SetDbObject(m);
                    vw.InitAccountDocItem();
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
                vw.RefDocDate = DateTime.Now;
                vw.IsInvoiceAvailable = true;
                vw.VAT_PCT = CGlobalVariable.GetGlobalVariableValue("VAT_PERCENTAGE");
                vw.VATType = CGlobalVariable.GetGlobalVariableValue("DEFAULT_VAT_TYPE_PERCHASE");
                vw.IsWhPayType1 = true;
                vw.AllowCashNegative = CGlobalVariable.IsCashNegativeAllow();
                vw.AllowARAPNegative = CGlobalVariable.IsArApNegativeAllow();
                vw.AllowInventoryNegative = CGlobalVariable.IsInventoryNegativeAllow();
            }

            vw.IsModified = false;
            isInLoadData = false;

            CUtil.EnableForm(true, this);
        }

        private CTable getDocInfoWrapper()
        {
            CTable m = null;

            actualView.EntityAddressFlag = "Y";
            m = OnixWebServiceAPI.GetAccountDocInfo(actualView.GetDbObject());

            return (m);
        }

        private CTable verifyAccountDocWrapper()
        {
            CTable m = null;

            m = OnixWebServiceAPI.VerifyAccountDoc(vw.GetDbObject().CloneAll());

            return (m);
        }

        private void dtFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInLoadData)
            {
                if (!vw.CreditTerm.Equals(""))
                {
                    vw.DueDate = vw.DocumentDate.AddDays(CUtil.StringToInt(vw.CreditTerm));
                }
            }

            vw.IsModified = true;
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
            currentViewObj = (MAccountDocItem)btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
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
            WinAddEditAccountPurchaseDocItem w = new WinAddEditAccountPurchaseDocItem(false);
            w.Caption = (String)(sender as Button).Content + " " + CLanguage.getValue("purchase_item");
            w.Mode = "A";
            w.ParentView = (vw as MAccountDoc);
            w.ShowDialog();
            if (w.HasModified)
            {
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void cmdPayment_Click(object sender, RoutedEventArgs e)
        {
            WinAddEditPayment w = new WinAddEditPayment(vw.CashReceiptAmt, vw, false);
            w.ShowDialog();

            if (w.IsOK)
            {
                vw.CalculateReceiveAndChange();
                vw.IsModified = true;
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblRefDoc, txtRefDoc, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateLookup(lblSupplier, uSupplier, false);
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

            Boolean isService = !vw.IsInInventory();
            result = CHelper.ValidateComboBox(lblTo, cboTo, isService);
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

        private Boolean SaveToView()
        {
            if (!ValidateData())
            {
                return (false);
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

            if (dt == AccountDocumentType.AcctDocCashPurchase)
            {
                acr = "PURCHASE_BYCASH_EDIT";
            }
            else if (dt == AccountDocumentType.AcctDocDebtPurchase)
            {
                acr = "PURCHASE_BYCREDIT_EDIT";
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
            vw.ConstructWhDefinitionFromDocItem();

            if (approveFlag.Equals("Y"))
            {
                Boolean result = SaveToView();
                if (!result)
                {
                    return (false);
                }

                CUtil.EnableForm(false, this);

                vw.CustomerObj = uSupplier.SelectedObject;
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
                            if (itemAddedHandler != null)
                            {
                                itemAddedHandler(vw, null);
                            }
                            else
                            {
                                //Will be obsoleted soon
                                parentItemsSource.Insert(0, vw);
                            }
                        }
                        else if (Mode.Equals("E"))
                        {

                            actualView.SetDbObject(t);
                            actualView.DocumentStatus = ((int)CashDocumentStatus.CashDocApproved).ToString();
                            createdID = actualView.AccountDocId;
                            (actualView as MAccountDoc).NotifyAllPropertiesChanged();
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
                    vw.CustomerObj = uSupplier.SelectedObject;

                    CTable newobj = OnixWebServiceAPI.CreateAccountDoc(vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (newobj != null)
                    {
                        vw.SetDbObject(newobj);
                        if (itemAddedHandler != null)
                        {
                            itemAddedHandler(vw, null);
                        }
                        else
                        {
                            //Will be obsoleted soon
                            parentItemsSource.Insert(0, vw);
                        }

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
          
                        vw.CustomerObj = uSupplier.SelectedObject;

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

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            if (dt == AccountDocumentType.AcctDocCashPurchase)
            {
                Boolean result = validatePayment();
                if (!result)
                {
                    return;
                }
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

        private void cmdPreview_Click(object sender, RoutedEventArgs e)
        {
            String group = "grpPurchaseByCashInvoice";
            if (dt == AccountDocumentType.AcctDocDebtPurchase)
            {
                group = "grpPurchaseByDebtInvoice";
            }
            if (dt == AccountDocumentType.AcctDocCashPurchase)
            {
                group = "grpPurchaseByCashInvoice";
            }            

            vw.ConstructWhDefinitionFromDocItem();
            WinFormPrinting w = new WinFormPrinting(group, vw);
            w.ShowDialog();
        }

        private void cmdApprovePrint_Click(object sender, RoutedEventArgs e)
        {
            needPreview = true;
            cmdApprove_Click(sender, e);
        }

        private void uSupplier_SelectedObjectChanged(object sender, EventArgs e)
        {
            if (!isInLoadData)
            {
                if (!vw.CreditTerm.Equals(""))
                {
                    vw.DueDate = vw.DocumentDate.AddDays(CUtil.StringToInt(vw.CreditTerm));
                }

                CTable supp = OnixWebServiceAPI.GetEntityInfo(vw.SupplierObj.GetDbObject());
                MEntity en = new MEntity(supp);
                en.InitEntityAddress();

                vw.ReloadEntityAddresses(en.AddressItems);

                if (en.AddressItems.Count > 0)
                {
                    cboAddress.SelectedIndex = 0;
                }
            }
            vw.IsModified = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void NumberPercentageValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            string expression = "^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$|[0-9]*[.]{0,1}[0-9]*%$";
            Regex regex = new Regex(expression);
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void lsvAccoutItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lsvAccoutItem.SelectedItems.Count == 1)
            {
                currentViewObj = (MAccountDocItem)lsvAccoutItem.SelectedItems[0];
                ShowEditWindow();
            }
        }

        private void ShowEditWindow()
        {
            WinAddEditAccountPurchaseDocItem w = new WinAddEditAccountPurchaseDocItem(false);
            w.ViewData = currentViewObj;
            w.Caption = CLanguage.getValue("ADMIN_EDIT") + " " + CLanguage.getValue("purchase_item");
            w.Mode = "E";
            w.ShowDialog();

            if (w.HasModified)
            {
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void lsvAccoutItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvAccoutItem.ActualWidth * 1) - 35;
            double[] ratios = new double[9] { 0.04, 0.04, 0.06, 0.14, 0.32, 0.1, 0.1, 0.1, 0.1 };
            CUtil.ResizeGridViewColumns(lsvAccoutItem.View as GridView, ratios, w);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cbxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            vw.CalculateExtraFields();
            vw.IsModified = true;                
        }

        private void cbxAllowNegative_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxAllowNegative_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void radNoVat_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInLoadData)
            {
                vw.CalculateExtraFields();
            }
            vw.IsModified = true;
        }

        private void radIncludeVat_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInLoadData)
            {
                vw.CalculateExtraFields();
            }
            vw.IsModified = true;
        }

        private void radExcludeVat_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInLoadData)
            {
                vw.CalculateExtraFields();
            }
            vw.IsModified = true;
        }

        private void txtVAT_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isInLoadData)
            {
                vw.CalculateExtraFields();
            }
            vw.IsModified = true;
        }

        private void dtDueDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void lsvAccoutItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lsvAccoutItem_MouseDoubleClick(sender, null);
            }
        }

        private void addAccountDocItems(ArrayList items)
        {
            int ind = 0;

            MProject firstProj = null;

            foreach (MAuxilaryDocItem ai in items)
            {
                MAccountDocItem di = new MAccountDocItem(new CTable(""));
                di.ExtFlag = "A";

                di.SelectType = ai.SelectType;
                di.ItemCode = ai.ItemCode;
                di.ItemNameThai = ai.ItemNameThai;
                di.ItemId = ai.ItemId;
                di.FreeText = ai.FreeText;
                di.ServiceCode = ai.ServiceCode;
                di.ServiceName = ai.ServiceName;
                di.ServiceID = ai.ServiceID;
                di.Quantity = ai.Quantity;
                di.Discount = ai.Discount;
                di.UnitPrice = ai.UnitPrice;
                di.TotalAfterDiscount = ai.TotalAfterDiscount;
                di.VatTaxFlag = ai.VatTaxFlag;
                di.WHTaxFlag = ai.WHTaxFlag;
                di.WHTaxPct = ai.WHTaxPct;
                di.VatTaxPct = ai.VatTaxPct;
                di.ItemNote = ai.DocumentNo;
                di.ProjectID = ai.ProjectID;
                di.ProjectCode = ai.ProjectCode;
                di.ProjectName = ai.ProjectName;
                di.ProjectGroupName = ai.ProjectGroupName;
                di.RefPoNo = ai.DocumentNo;
                di.WhGroupCriteria = ai.WhGroup;
                di.PoItemID = ai.AuxilaryDocItemID;
                di.PoID = ai.AuxilaryDocID;

                if (ind == 0)
                {
                    firstProj = (MProject) di.ProjectObj;
                }

                ind++;

                vw.AddAccountDocItem(di);
            }

            if (vw.ProjectID.Equals("") && (firstProj != null))
            {
                //If not yet been set
                vw.ProjectObj = firstProj;
            }

            String poString = vw.GetRefPoString();
            vw.RefPoNo = poString;
        }

        private void addAccountDocCriteria(ArrayList items)
        {
            int ind = 0;
            MProject firstProj = null;

            foreach (MPaymentCriteria ai in items)
            {
                MAccountDocItem di = new MAccountDocItem(new CTable(""));
                di.ExtFlag = "A";

                di.SelectType = "3";
                di.FreeText = ai.Description;
                di.Quantity = "1";
                di.Amount = ai.PaymentAmount;
                di.UnitPrice = ai.PaymentAmount;
                di.TotalAfterDiscount = ai.PaymentAmount;

                di.WHTaxFlag = "N";
                if (CUtil.StringToDouble(ai.WhPercent) > 0)
                {
                    di.WHTaxFlag = "Y";
                }

                di.VatTaxFlag = "N";
                if (CUtil.StringToDouble(ai.VatPercent) > 0)
                {
                    di.VatTaxFlag = "Y";
                }
                
                di.WHTaxPct = ai.WhPercent;
                di.VatTaxPct = ai.VatPercent;
                di.ItemNote = ai.DocumentNo;

                di.ProjectID = ai.ProjectID;
                di.ProjectCode = ai.ProjectCode;
                di.ProjectName = ai.ProjectName;
                di.ProjectGroupName = ai.ProjectGroupName;
                di.RefPoNo = ai.DocumentNo;
                di.WhGroup = ai.WHGroup;
                di.WhGroupCriteria = ai.WHGroup;
                di.PoCriteriaID = ai.PaymentCriteriaID;
                di.PoID = ai.AuxilaryDocID;

                if (ind == 0)
                {
                    firstProj = (MProject)di.ProjectObj;
                }

                ind++;

                vw.AddAccountDocItem(di);
            }

            if (vw.ProjectID.Equals("") && (firstProj != null))
            {
                //If not yet been set
                vw.ProjectObj = firstProj;
            }

            String poString = vw.GetRefPoString();
            vw.RefPoNo = poString;
        }

        private void cmdAddByPO_Click(object sender, RoutedEventArgs e)
        {
            Boolean result = CHelper.ValidateLookup(lblSupplier, uSupplier, false);
            if (!result)
            {
                return;
            }

            //cmdAddByPO.Open
            cmdAddByPO.ContextMenu.IsOpen = true;
            //mnuByItem_Click(sender, e);
        }

        private void mnuByItem_Click(object sender, RoutedEventArgs e)
        {
            CCriteriaPurchaseOrderItem cr = new Criteria.CCriteriaPurchaseOrderItem();
            cr.SetActionEnable(false);
            cr.SetDefaultData(vw);
            cr.Init("");

            WinMultiSelection w = new WinMultiSelection(cr, CLanguage.getValue("purchase_item"));
            w.ShowDialog();

            if (w.IsOK)
            {
                addAccountDocItems(w.SelectedItems);
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void mnuByCriteria_Click(object sender, RoutedEventArgs e)
        {
            CCriteriaPurchaseOrderCriteria cr = new Criteria.CCriteriaPurchaseOrderCriteria();
            cr.SetActionEnable(false);
            cr.SetDefaultData(vw);
            cr.Init("");

            WinMultiSelection w = new WinMultiSelection(cr, CLanguage.getValue("purchase_item"));
            w.ShowDialog();

            if (w.IsOK)
            {
                addAccountDocCriteria(w.SelectedItems);
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void uProject_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdCheque_Click(object sender, RoutedEventArgs e)
        {
            Boolean result = CHelper.ValidateLookup(lblSupplier, uSupplier, false);
            if (!result)
            {
                return;
            }

            if (vw.ChequeID.Equals(""))
            {
                ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();
                CCriteriaCheque.ShowAddChequeWindow("2", arr, vw);

                if (arr.Count > 0)
                {
                    MCheque cq = (MCheque)arr[0];
                    vw.ChequeID = cq.ChequeID;
                    vw.ChequeNo = cq.ChequeNo;
                    vw.IsModified = true;
                }
            }
            else
            {
                CCriteriaCheque.ShowEditWindow("2", null, vw);
            }
        }

        private void cmdDiscount_Click(object sender, RoutedEventArgs e)
        {
            Boolean isOK = WinAddEditDiscount.ShowDiscountWindow(vw);
            if (isOK)
            {
                vw.CalculateExtraFields();
                vw.IsModified = true;
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
