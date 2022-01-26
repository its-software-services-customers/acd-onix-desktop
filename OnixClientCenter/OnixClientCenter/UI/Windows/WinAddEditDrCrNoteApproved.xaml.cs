using System;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using System.Collections;
using Onix.ClientCenter.UControls;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditDrCrNoteApproved : Window
    {
        private MAccountDoc vw = null;
        private MAccountDoc actualView = null;

        private ObservableCollection<MBaseModel> parentItemsSource = null;

        public Boolean DialogOK = false;
        public String Caption = "";
        public String Mode = "";
        private AccountDocumentType dt;
        private Boolean isInLoadData = false;

        private Hashtable priceHash = new Hashtable();
        private RoutedEventHandler itemAddedHandler = null;

        public WinAddEditDrCrNoteApproved(String md, AccountDocumentType docType, ObservableCollection<MBaseModel> pItems, MAccountDoc actView)
        {
            dt = docType;

            actualView = actView;
            parentItemsSource = pItems;
            Mode = md;

            vw = new MAccountDoc(new CTable(""));
            vw.VATType = CGlobalVariable.GetGlobalVariableValue("DEFAULT_VAT_TYPE_SALE");

            if (md.Equals("A") && (actView != null))
            {
                //use actView as default value
                vw.BranchId = actView.BranchId;
                vw.BranchCode = actView.BranchCode;
                vw.BranchName = actView.BranchName;

                vw.EntityId = actView.EntityId;
                vw.EntityCode = actView.EntityCode;
                vw.EntityName = actView.EntityName;
            }

            DataContext = vw;
            vw.DocumentType = ((int)dt).ToString();

            InitializeComponent();
        }

        public Boolean IsPreviewNeed
        {
            get
            {
                return (false);
            }
        }

        public Boolean IsAr
        {
            get
            {
                Boolean flag = ((dt == AccountDocumentType.AcctDocDrNote) || (dt == AccountDocumentType.AcctDocCrNote));
                return (flag) ;
            }
        }

        public Boolean IsAp
        {
            get
            {
                return (!IsAr);
            }
        }

        public Boolean IsArAndEditable
        {
            get
            {
                return (IsAr && vw.IsEditable);
            }
        }

        public LookupSearchType2 EntityTypeLookup
        {
            get
            {
                if ((dt == AccountDocumentType.AcctDocCrNote) || (dt == AccountDocumentType.AcctDocDrNote))
                {
                    return (LookupSearchType2.CustomerLookup);
                }

                return (LookupSearchType2.SupplierLookup);
            }
        }

        public String EntityTypeCaption
        {
            get
            {
                if ((dt == AccountDocumentType.AcctDocCrNote) || (dt == AccountDocumentType.AcctDocDrNote))
                {
                    return (CLanguage.getValue("customer_name"));
                }

                return (CLanguage.getValue("supplier_name"));
            }
        }

        private void dtFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {

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

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblDocumentNo, txtDocNo, true);
            if (!result)
            {
                return (result);
            }

             result = CHelper.ValidateTextBox(lblDocumentStatus, txtStatus, false);
            if (!result)
            {
                return (result);
            }
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

            result = CHelper.ValidateTextBox(lblDebtAmount, txtDebtAmount, false, InputDataType.InputTypeZeroPossitiveDecimal);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblNet, txtNet, false, InputDataType.InputTypeZeroPossitiveDecimal);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblhwdvalue, txthwdvalue, false, InputDataType.InputTypeZeroPossitiveDecimal);
            if (!result)
            {
                return (result);
            }

            CTable ug = new CTable("ACCOUNT_DOC");
            MAccountDoc uv = new MAccountDoc(ug);
            uv.DocumentNo = txtDocNo.Text;
            if (vw != null)
            {
                uv.AccountDocId = (vw as MAccountDoc).AccountDocId;
            }
            if (OnixWebServiceAPI.IsAccountDocExist(uv.GetDbObject()))
            {
                CHelper.ShowKeyExist(lblDocumentNo, txtDocNo);
                return (false);
            }

            return (result);
        }

        private void dtDocuDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
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

        private Boolean SaveToView()
        {
            if (!ValidateData())
            {
                return (false);
            }

            return (true);
        }

        private void cbxAllowNegative_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxAllowNegative_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private String getAccessRightEdit()
        {
            String acr = "PURCHASE_UNKNOW_EDIT";

            if (dt == AccountDocumentType.AcctDocDrNote)
            {
                acr = "SALE_DRNOTE_EDIT";
            }
            else if (dt == AccountDocumentType.AcctDocCrNote)
            {
                acr = "SALE_CRNOTE_EDIT";
            }
            else if (dt == AccountDocumentType.AcctDocDrNotePurchase)
            {
                acr = "PURCHASE_DRNOTE_EDIT";
            }
            else if (dt == AccountDocumentType.AcctDocCrNotePurchase)
            {
                acr = "PURCHASE_CRNOTE_EDIT";
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
            vw.ConstructWhDefinitionFromCrDr();
            vw.CalculateARAmountForDrCr();

            if (approveFlag.Equals("Y"))
            {
                Boolean result = SaveToView();
                if (!result)
                {
                    return (false);
                }

                CUtil.EnableForm(false, this);
                CTable t = OnixWebServiceAPI.ApproveAccountDoc(vw.GetDbObject().Clone());
                CUtil.EnableForm(true, this);
                if (t != null)
                {
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
                //Not used
            }
            else if (Mode.Equals("E"))
            {
                if (vw.IsModified)
                {
                    Boolean result = SaveToView();
                    if (result)
                    {
                        CUtil.EnableForm(false, this);
                        CTable t = OnixWebServiceAPI.AdjustApproveAccountDoc(vw.GetDbObject());
                        CUtil.EnableForm(true, this);
                        if (t != null)
                        {
                            actualView.SetDbObject(t);
                            actualView.NotifyAllPropertiesChanged();

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
                actualView = vw;
                Mode = "E";

                LoadData();

                vw.IsModified = false;
                DialogOK = true;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
            Boolean r = SaveData("Y");
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);

                this.Close();
            }
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
            CTable t = OnixWebServiceAPI.VerifyAccountDoc(vw.GetDbObject().Clone());
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

        private void LoadData()
        {
            isInLoadData = true;

            this.Title = Caption;
            dtDocumentDate.Focus();

            vw.CreateDefaultValue();
            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable m = OnixWebServiceAPI.GetAccountDocInfo(actualView.GetDbObject());
                if (m != null)
                {
                    vw.SetDbObject(m);
                    vw.NotifyAllPropertiesChanged();
                }
            }

            else if (Mode.Equals("A"))
            {
                vw.DocumentDate = DateTime.Now;
                vw.DocumentType = ((int)dt).ToString();

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

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void uCustomer_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void txtDesc_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void txtWhtax_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void dtmDueDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboBranch_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdPreview_Click(object sender, RoutedEventArgs e)
        {
            String group = "grpDrNote";
            if (dt == AccountDocumentType.AcctDocCrNote)
            {
                group = "grpCrNote";
            }

            WinFormPrinting w = new WinFormPrinting(group, vw);
            w.ShowDialog();
        }

        private void cmdApprovePrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void uProject_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
