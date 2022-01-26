using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.UI.Cash.Cheque;
using Onix.ClientCenter.Criteria;
using System.Collections;
using Onix.ClientCenter.UI.HumanResource.PayrollDocument;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditAccountMiscDoc : Window
    {
        private MAccountDoc vw = null;
        private MAccountDoc actualView = null;
        private ObservableCollection<MBaseModel> parentItemsSource = null;
  
        private MAccountDocItem currentViewObj = null;

        private MGlobalVariable vv = new MGlobalVariable(new CTable(""));

        private AccountDocumentType dt;

        public Boolean DialogOK = false;
        public String Caption = "";
        public String Mode = "";

        private Boolean isInLoadData = true;

        private RoutedEventHandler itemAddedHandler = null;
        private Boolean needPreview = false;
        private String createdID = "0";

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

        public Boolean IsExpense
        {
            get
            {
                return (DocType == AccountDocumentType.AcctDocMiscExpense);
            }

            set
            {                
            }
        }

        public Boolean IsRevenue
        {
            get
            {
                return (DocType == AccountDocumentType.AcctDocMiscRevenue);
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
                if (dt == AccountDocumentType.AcctDocMiscRevenue)
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
                if (dt == AccountDocumentType.AcctDocMiscRevenue)
                {
                    lk = LookupSearchType2.CustomerLookup;
                }

                return (lk);
            }

            set
            {
            }
        }

        public WinAddEditAccountMiscDoc(String md, AccountDocumentType docType, ObservableCollection<MBaseModel> pItems, MAccountDoc actView)
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
                vw.RefDocDate = vw.DocumentDate;
                vw.VAT_PCT = CGlobalVariable.GetGlobalVariableValue("VAT_PERCENTAGE");
                vw.VATType = CGlobalVariable.GetGlobalVariableValue("DEFAULT_VAT_TYPE_PERCHASE");
                vw.AllowCashNegative = CGlobalVariable.IsCashNegativeAllow();
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

        private String getCaptionKey()
        {
            String temp = CLanguage.getValue("purchase_item");
            if (dt == AccountDocumentType.AcctDocMiscRevenue)
            {
                temp = CLanguage.getValue("sale_item");
            }

            return (temp);
        }

        public String ItemCaption
        {
            get
            {
                return (getCaptionKey());
            }
        }

        private void cmdAddProduct_Click(object sender, RoutedEventArgs e)
        {
            WinAddEditMiscItemComplex w = new WinAddEditMiscItemComplex();
            w.Caption = (String)(sender as Button).Content + " " + getCaptionKey();
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
            WinAddEditPayment w = new WinAddEditPayment(vw.CashReceiptAmt, vw, dt == AccountDocumentType.AcctDocMiscRevenue);
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

            result = CHelper.ValidateTextBox(lblRefDoc, txtRefDoc, dt == AccountDocumentType.AcctDocMiscRevenue);
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

            return (m);
        }

        private String getAccessRightEdit()
        {
            String acr = "PURCHASE_UNKNOW_EDIT";

            if (dt == AccountDocumentType.AcctDocMiscExpense)
            {
                acr = "PURCHASE_MISC_EDIT";
            }
            else if (dt == AccountDocumentType.AcctDocMiscRevenue)
            {
                acr = "SALE_MISC_EDIT";
            }

            return (acr);
        }

        private Boolean SaveData(String approveFlag)
        {
            if (!CHelper.VerifyAccessRight(getAccessRightEdit()))
            {
                return (false);
            }

            vw.RefDocDate = vw.DocumentDate;
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

            double amt = CUtil.StringToDouble(vw.CashReceiptAmt);
            if (total < amt)
            {
                CMessageBox.Show(CLanguage.getValue("ERROR_PAYMENT_NOT_ENOUGH"), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return (false);
            }

            return (true);
        }

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            Boolean result = validatePayment();
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

        private void cmdPreview_Click(object sender, RoutedEventArgs e)
        {
            String group = "grpPurchaseMiscInvoice";
            if (dt == AccountDocumentType.AcctDocMiscRevenue)
            {
                group = "grpSaleMiscInvoice";
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
            WinAddEditMiscItemComplex w = new WinAddEditMiscItemComplex();
            w.ViewData = currentViewObj;
            w.Caption = CLanguage.getValue("ADMIN_EDIT") + " " + getCaptionKey();
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

        private void uProject_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
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

        private void dtFromDate_OnChanged(object sender, EventArgs e)
        {

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

        private CTable getObject(String code, String fieldName, String apiName, String arrName)
        {
            CTable t = new CTable("");
            t.SetFieldValue(fieldName, code);
            ArrayList arr = OnixWebServiceAPI.GetListAPI(apiName, arrName, t);

            if (arr.Count <= 0)
            {
                return (t);
            }

            t = (CTable) arr[0];
            return (t);
        }

        private void addItemFromPayroll(ArrayList arr, int type)
        {
            String projectVariable = "PAYROLL_PROJECT_CODE";
            String serviceVariable = "PAYROLL_SERVICE_CODE";
            if (type == 2)
            {
                serviceVariable = "SOCIAL_SECURITY_SERVICE_CODE";
            }

            String projectCode = CGlobalVariable.GetGlobalVariableValue(projectVariable);
            CTable p = getObject(projectCode, "PROJECT_CODE", "GetProjectList", "PROJECT_LIST");
            MProject pj = new MProject(p);

            String serviceCode = CGlobalVariable.GetGlobalVariableValue(serviceVariable);
            CTable s = getObject(serviceCode, "SERVICE_CODE", "GetServiceList", "SERVICE_LIST");
            MService sv = new MService(s);

            foreach (MVPayrollDocument pd in arr)
            {
                CTable t = new CTable("");
                MAccountDocItem mdi = new MAccountDocItem(t);
                mdi.CreateDefaultValue();
                mdi.SelectType = "1";
                mdi.ServiceObj = sv;
                mdi.ProjectObj = pj;

                vw.AddAccountDocItem(mdi);
                
                MAuxilaryDocSubItem mi = new MAuxilaryDocSubItem(new CTable(""));
                mi.Description = String.Format("{0}-{1} {2}", pd.FromSalaryDateFmt, pd.ToSalaryDateFmt, pd.EmployeeTypeDesc);
                mi.SubItemDate = pd.ToSalaryDate;
                mi.UnitPrice = pd.ReceiveAmount;

                if (type == 2)
                {
                    mi.UnitPrice = pd.SocialSecurityCompanyAmount;
                }

                mi.Quantity = "1.00";
                mdi.AddItemDetail(mi);

                mdi.CalculateSubItemTotal();
                mdi.SerializeItemDetails();
            }
        }

        private void CmdAddPayroll_Click(object sender, RoutedEventArgs e)
        {
            CCriteriaExpenseFromPayroll cr = new CCriteriaExpenseFromPayroll();
            cr.SetActionEnable(false);
            cr.SetDefaultData(vw);
            cr.Init("");

            WinMultiSelection w = new WinMultiSelection(cr, CLanguage.getValue("payroll_item"));
            w.ShowDialog();

            if (w.IsOK)
            {
                addItemFromPayroll(w.SelectedItems, 1);
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void CmdSocialAssure_Click(object sender, RoutedEventArgs e)
        {
            CCriteriaExpenseFromPayroll cr = new CCriteriaExpenseFromPayroll();
            cr.SetActionEnable(false);
            cr.SetDefaultData(vw);
            cr.Init("");

            WinMultiSelection w = new WinMultiSelection(cr, CLanguage.getValue("social_insurance_item"));
            w.ShowDialog();

            if (w.IsOK)
            {
                addItemFromPayroll(w.SelectedItems, 2);
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }
    }
}
