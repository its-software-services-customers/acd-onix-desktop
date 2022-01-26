using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using System.Windows.Input;
using Onix.ClientCenter.Commons.Utils;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.Client.Pricing;
using System.Collections;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditAccountPurchaseDocApproved : Window
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

        public WinAddEditAccountPurchaseDocApproved(String md, AccountDocumentType docType, ObservableCollection<MBaseModel> pItems, MAccountDoc actView)
        {
            dt = docType;

            actualView = actView;
            parentItemsSource = pItems;
            Mode = md;

            vw = new MAccountDoc(new CTable(""));
            vw.DocumentType = ((int) dt).ToString();
            DataContext = vw;

            InitializeComponent();
        }

        public Boolean IsInvoice
        {
            get
            {
                if ((dt == AccountDocumentType.AcctDocMiscExpense) ||
                    (dt == AccountDocumentType.AcctDocCashPurchase) ||
                    (dt == AccountDocumentType.AcctDocDebtPurchase))
                {
                    return (true);
                }

                return (false);
            }
        }

        public Boolean IsReceipt
        {
            get
            {
                if ((dt == AccountDocumentType.AcctDocMiscExpense) ||
                    (dt == AccountDocumentType.AcctDocCashPurchase) ||
                    (dt == AccountDocumentType.AcctDocApReceipt))
                {
                    return (true);
                }

                return (false);
            }
        }

        public Boolean IsExpense
        {
            get
            {
                return (dt == AccountDocumentType.AcctDocMiscExpense);
            }

            set
            {
            }
        }

        public Boolean IsApReceipt
        {
            get
            {
                if (dt == AccountDocumentType.AcctDocApReceipt)
                {
                    return (true);
                }

                return (false);
            }
        }

        public Boolean IsProjectShow
        {
            get
            {
                if ((dt == AccountDocumentType.AcctDocMiscExpense) ||
                    (dt == AccountDocumentType.AcctDocCashPurchase) ||
                    (dt == AccountDocumentType.AcctDocDebtPurchase))
                {
                    return (true);
                }

                return (false);
            }
        }

        private void LoadData()
        {
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

            vw.IsModified = false;

            CUtil.EnableForm(true, this);
        }

        private CTable getDocInfoWrapper()
        {
            CTable m = null;

            actualView.EntityAddressFlag = "Y";
            m = OnixWebServiceAPI.SubmitObjectAPI("GetAccountDocInfo", actualView.GetDbObject());

            return (m);
        }

        private void dtFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
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

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblRefDoc, txtRefDoc, false);
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

            m = OnixWebServiceAPI.SubmitObjectAPI("AdjustApproveAccountDoc", vw.GetDbObject());

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
            else if (dt == AccountDocumentType.AcctDocMiscExpense)
            {
                acr = "PURCHASE_MISC_EDIT";
            }
            else if (dt == AccountDocumentType.AcctDocApReceipt)
            {
                acr = "PURCHASE_RECEIPT_EDIT";
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

            if (Mode.Equals("E"))
            {
                if (vw.IsModified)
                {
                    Boolean result = SaveToView();
                    if (result)
                    {
                        CUtil.EnableForm(false, this);
          
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
            WinAddEditAccountPurchaseDocItemApproved w = new WinAddEditAccountPurchaseDocItemApproved(false);
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
            double[] ratios = new double[9] { 0.00, 0.04, 0.06, 0.18, 0.32, 0.1, 0.1, 0.1, 0.1 };
            CUtil.ResizeGridViewColumns(lsvAccoutItem.View as GridView, ratios, w);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
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

        private void cbxInvoiceAvailable_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void radType1_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
