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
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.UI.Inventory.InventoryItem;
using Onix.ClientCenter.UI.General.Service;
using System.Collections;

namespace Onix.ClientCenter
{
    public partial class WinAddEditPurchaseOrder : Window
    {
        private MAuxilaryDoc vw = null;
        private MAuxilaryDoc actualView = null;
        private ObservableCollection<MBaseModel> parentItemsSource = null;
        private MBaseModel currentViewObj = null;
        private AuxilaryDocumentType docType = AuxilaryDocumentType.AuxDocPO;

        public String createdID = "0";

        public Boolean DialogOK = false;
        public String Caption = "";
        public String Mode = "";
        public Boolean IsPreviewNeed = false;
        

        private Boolean isInLoadData = true;

        public WinAddEditPurchaseOrder(String md, ObservableCollection<MBaseModel> pItems, MAuxilaryDoc actView, AuxilaryDocumentType dt)
        {   
            actualView = actView;
            parentItemsSource = pItems;
            Mode = md;
            docType = dt;

            vw = new MAuxilaryDoc(new CTable(""));
            DataContext = vw;

            InitializeComponent();
        }        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private Boolean validatePaymentCriteria<T>(ObservableCollection<T> collection, TabItem titem, Boolean chkCnt) where T : MBaseModel
        {
            int idx = 0;
            int cnt = 0;
            double total = 0.00;

            foreach (MBaseModel t in collection)
            {
                MPaymentCriteria c = (MPaymentCriteria) t;

                idx++;

                if (c.IsEmpty)
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    titem.IsSelected = true;
                    return (false);
                }

                double pct = CUtil.StringToDouble(c.Percent);
                total = total + pct;

                cnt++;
            }

            if ((cnt <= 0) && chkCnt)
            {
                CHelper.ShowErorMessage(idx.ToString(), "ERROR_ITEM_COUNT", null);
                titem.IsSelected = true;
                return (false);
            }

            if (cnt > 0)
            {
                if (!vw.PmtVatAmtFmt.Equals(vw.VatAmtFmt))
                {
                    String temp = String.Format("{0} != {1}", vw.PmtVatAmtFmt, vw.VatAmtFmt);

                    Boolean result = CHelper.AskConfirmMessage(temp, "ERROR_VAT_DIFFERENT");
                    return (result);
                }
            }

            return (true);
        }


        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateLookup(lblCustomer, uSupplier, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateComboBox(lblBranch, cboBranch, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateComboBox(lblCreditTerm, cboCredit, false);
            if (!result)
            {
                return (result);
            }

            result = validatePaymentCriteria(vw.PaymentCriteriaes, tbiPaymentCriteria, false);
            if (!result)
            {
                return (result);
            }

            return (result);
        }

        private Boolean SaveToView( )
        {
            if (!ValidateData())
            {
                return(false);
            }

            return (true);
        }

        public String CreatedID
        {
            get
            {
                return (createdID);
            }
        }

        private Boolean SaveData(String approveFlag)
        {
            if (!CHelper.VerifyAccessRight("PURCHASE_PO_EDIT"))
            {
                return (false);
            }

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
                    actualView.DocumentStatus = ((int)PoDocumentStatus.PoApproved).ToString();
                    if (Mode.Equals("A"))
                    {
                        vw.SetDbObject(t);
                        vw.NotifyAllPropertiesChanged();
                        createdID = vw.AuxilaryDocID;

                        parentItemsSource.Insert(0, vw);
                    }
                    else if (Mode.Equals("E"))
                    {
                        actualView.SetDbObject(t);
                        actualView.NotifyAllPropertiesChanged();
                    }

                    vw.IsModified = false;
                    this.Close();
                }
            }
            else
            {
                if (!SaveToView())
                {
                    return (false);
                }

                CUtil.EnableForm(false, this);
                vw.DocumentType = ((int) docType).ToString();
                CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("SaveAuxilaryDoc", vw.GetDbObject());
                CUtil.EnableForm(true, this);

                if (newobj != null)
                {
                    if (Mode.Equals("A"))
                    {
                        vw.SetDbObject(newobj);
                        parentItemsSource.Insert(0, vw);
                    }
                    else
                    {
                        actualView.SetDbObject(newobj);
                        actualView.NotifyAllPropertiesChanged();
                    }
                        
                    return (true);
                }

                //Error here
                CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                return (false);
            }

            return (false);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (!vw.IsModified)
            {
                this.Close();
                return;
            }

            Boolean r = SaveData("N");
            if (r)
            {
                vw.IsModified = false;
                DialogOK = true;
                CUtil.EnableForm(true, this);

                this.Close();
            }
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

        private CTable approveAccountDocWrapper()
        {
            CTable m = OnixWebServiceAPI.SubmitObjectAPI("ApproveAuxilaryDoc", vw.GetDbObject().CloneAll());
            return (m);
        }

        private CTable getDocInfoWrapper()
        {
            if (actualView != null)
            {
                //Edit mode
                vw.AuxilaryDocID = actualView.AuxilaryDocID;
            }

            vw.EntityAddressFlag = "Y";
            CTable m = OnixWebServiceAPI.SubmitObjectAPI("GetAuxilaryDocInfo", vw.GetDbObject().CloneAll());
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
                CTable m = getDocInfoWrapper();
                if (m != null)
                {
                    vw.SetDbObject(m);
                    vw.InitAuxilaryDocItem();
                    vw.InitPaymentCriteria();

                    String tmp = vw.EntityAddressID;
                    vw.InitEntityAddresses();
                    vw.EntityAddressID = tmp;

                    tmp = vw.EntityBankAccountID;
                    vw.InitEntityBankAccounts();
                    vw.EntityBankAccountID = tmp;

                    vw.AddressObj = CUtil.MasterIDToObject(vw.EntityAddresses, vw.EntityAddressID);

                    vw.NotifyAllPropertiesChanged();                    
                }
            }
            else if (Mode.Equals("A"))
            {
                vw.DocumentDate = DateTime.Now;                
                vw.VatPct = CGlobalVariable.GetGlobalVariableValue("VAT_PERCENTAGE");
                vw.VatType = CGlobalVariable.GetGlobalVariableValue("DEFAULT_VAT_TYPE_PERCHASE");
                vw.NoteWidthCm = "10.00";
                vw.NoteHeightCm = "7.00";
                vw.NoteTopCm = "12.00";
                vw.NoteLeftCm = "1.00";
                vw.IsNoteStick = false;

                ObservableCollection<MMasterRef> branches = CMasterReference.Instance.Branches;
                if (branches.Count > 1)
                {
                    MMasterRef b = branches[1];
                    cboBranch.SelectedObject = b;
                }
                                
                vw.IsPoInvoiceRefByItem = true;
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

        private void mnuDocumentEdit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuDocumentEdit"))
            {
                ShowEditWindow();
            }
            else if (name.Equals("mnuPaymentEdit"))
            {
                ShowPaymentEditWindow();
            }
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MBaseModel) btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
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

        private void cmdVerify_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cmdAddProduct_Click(object sender, RoutedEventArgs e)
        {
            WinAddEditPurchaseOrderItem w = new WinAddEditPurchaseOrderItem();
            w.Caption = (String)(sender as Button).Content + " " + CLanguage.getValue("po_item");
            w.Mode = "A";
            w.ParentView = vw;
            w.ShowDialog();
            if (w.HasModified)
            {
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void uCustomer_SelectedObjectChanged(object sender, EventArgs e)
        {

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

        private void uSalesman_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void lsvAccoutItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lsvPoItem.SelectedItems.Count == 1)
            {
                currentViewObj = (MAuxilaryDocItem)lsvPoItem.SelectedItems[0];
                ShowEditWindow();
            }
        }

        private void ShowPaymentEditWindow()
        {
            WinAddEditPaymentCriteria w = new WinAddEditPaymentCriteria(vw);
            w.ViewData = (MPaymentCriteria)currentViewObj;
            w.Caption = CLanguage.getValue("ADMIN_EDIT") + " " + CLanguage.getValue("payment_criteria");
            w.Mode = "E";
            w.ShowDialog();

            if (w.HasModified)
            {
                vw.CalculatePaymentTotal();
                vw.IsModified = true;
            }
        }

        private void ShowEditWindow()
        {
            WinAddEditPurchaseOrderItem w = new WinAddEditPurchaseOrderItem();
            w.ViewData = (MAuxilaryDocItem) currentViewObj;
            w.ParentView = vw;
            w.Caption = CLanguage.getValue("ADMIN_EDIT") + " " + CLanguage.getValue("po_item");
            w.Mode = "E";
            w.ShowDialog();

            if (w.HasModified)
            {
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void radNoVat_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInLoadData)
            {
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }            
        }

        private void radIncludeVat_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInLoadData)
            {
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void radExcludeVat_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInLoadData)
            {
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
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

        private void cmdApprovePrint_Click(object sender, RoutedEventArgs e)
        {
            IsPreviewNeed = true;
            cmdApprove_Click(sender, e);
        }

        private void cmdPreview_Click(object sender, RoutedEventArgs e)
        {
            String group = "grpPurchasePO";
            WinFormPrinting w = new WinFormPrinting(group, vw);
            w.ShowDialog();
        }

        private void lsvAccoutItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void lsvPoItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvPoItem.View as GridView;

            double w = (lsvPoItem.ActualWidth * 1) - 35;
            double[] ratios = new double[11] { 0.04, 0.04, 0.00, 0.12, 0.28, 0.1, 0.08, 0.08, 0.1, 0.08, 0.08 };
            CUtil.ResizeGridViewColumns(lsvPoItem.View as GridView, ratios, w);
        }

        private void cbxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboBranch_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void uSupplier_SelectedObjectChanged(object sender, EventArgs e)
        {
            if (!isInLoadData)
            {
                CTable cust = OnixWebServiceAPI.SubmitObjectAPI("GetEntityInfo", vw.EntityObj.GetDbObject());
                MEntity en = new MEntity(cust);
                en.InitEntityAddress();
                en.InitBankAccounts();

                vw.ReloadEntityAddresses(en.AddressItems);
                vw.ReloadEntityBankAccount(en.BankAccounts);
                vw.CreditTermObj = en.CreditTermObj;
                vw.IsModified = true;

                if (en.AddressItems.Count > 0)
                {
                    cboAddress.SelectedIndex = 0;
                }

                if (en.BankAccounts.Count > 0)
                {
                    cboSupplierAccount.SelectedIndex = 0;
                }
            } 
        }

        private void uProject_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void uEmployee_SelectedObjectChanged(object sender, EventArgs e)
        {

        }

        private void cbbPaymentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdPaymentDelete_Click(object sender, RoutedEventArgs e)
        {
            MPaymentCriteria pp = (MPaymentCriteria)(sender as Button).Tag;
            vw.RemovePaymentCriteria(pp);

            vw.IsModified = true;
        }

        private void cmdPaymentAdd_Click(object sender, RoutedEventArgs e)
        {
            WinAddEditPaymentCriteria w = new WinAddEditPaymentCriteria(vw);
            w.Caption = (String)(sender as Button).Content + " " + CLanguage.getValue("payment_criteria");
            w.Mode = "A";
            w.ShowDialog();
            if (w.HasModified)
            {
                vw.CalculatePaymentTotal();
                vw.IsModified = true;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void lsvPayment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lsvPayment.SelectedItems.Count == 1)
            {
                currentViewObj = (MPaymentCriteria)lsvPayment.SelectedItems[0];
                ShowPaymentEditWindow();
            }
        }

        private void lsvPayment_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvPayment.View as GridView;

            double w = (lsvPayment.ActualWidth * 1) - 35;
            double[] ratios = new double[9] { 0.05, 0.05, 0.00, 0.40, 0.1, 0.1, 0.1, 0.1, 0.1 };
            CUtil.ResizeGridViewColumns(lsvPayment.View as GridView, ratios, w);
        }

        private void cbkRemove_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
            vw.CalculatePaymentTotal();
        }

        private void lsvPoItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lsvAccoutItem_MouseDoubleClick(sender, null);
            }
        }

        private void lsvPayment_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lsvPayment_MouseDoubleClick(sender, null);
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (!isInLoadData)
            {
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void CmdAddMultiple_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button);
            btn.ContextMenu.IsOpen = true;
        }

        private void MnuItem_Click(object sender, RoutedEventArgs e)
        {
            CCriteriaInventoryItem cr = new CCriteriaInventoryItem();
            cr.SetActionEnable(false);
            cr.SetDefaultData(vw);
            cr.IsForMultiSelect = true;
            cr.Init("");

            WinMultiSelection w = new WinMultiSelection(cr, CLanguage.getValue("inventory_item"));
            w.ShowDialog();

            if (w.IsOK)
            {
                addPoDocStockItems(w.SelectedItems);
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void MnuOther_Click(object sender, RoutedEventArgs e)
        {
            CCriteriaService cr = new CCriteriaService();
            cr.SetActionEnable(false);
            cr.SetDefaultData(vw);
            cr.IsForMultiSelect = true;
            cr.Init("");

            WinMultiSelection w = new WinMultiSelection(cr, CLanguage.getValue("service"));
            w.ShowDialog();

            if (w.IsOK)
            {
                addPoDocServiceItems(w.SelectedItems);
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void addPoDocStockItems(ArrayList items)
        {
            foreach (MInventoryItem ai in items)
            {
                MAuxilaryDocItem di = new MAuxilaryDocItem(new CTable(""));
                di.ExtFlag = "A";

                di.SelectType = "2";
                di.ItemCode = ai.ItemCode;
                di.ItemNameThai = ai.ItemNameThai;
                di.ItemId = ai.ItemID;
                di.Quantity = "1";
                di.DiscountPct = "0.00";
                di.Discount = "0.00";
                vw.AddAuxilaryDocItem(di);
            }
        }

        private void addPoDocServiceItems(ArrayList items)
        {
            foreach (MService ai in items)
            {
                MAuxilaryDocItem di = new MAuxilaryDocItem(new CTable(""));
                di.ExtFlag = "A";

                di.SelectType = "1";
                di.ServiceCode = ai.ServiceCode;
                di.ServiceName = ai.ServiceName;
                di.ServiceID = ai.ServiceID;
                di.Quantity = "1";
                di.DiscountPct = "0.00";
                di.Discount = "0.00";
                vw.AddAuxilaryDocItem(di);
            }            
        }
    }
}
