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

namespace Onix.ClientCenter
{
    public partial class WinAddEditQuotation : Window
    {
        private MAuxilaryDoc vw = null;
        private MAuxilaryDoc actualView = null;
        private ObservableCollection<MBaseModel> parentItemsSource = null;
        private MAuxilaryDocItem currentViewObj = null;
        private AuxilaryDocumentType docType = AuxilaryDocumentType.AuxDocPO;

        private String quotationType = "2";
        public String createdID = "0";

        public Boolean DialogOK = false;
        public String Caption = "";
        public String Mode = "";
        public Boolean IsPreviewNeed = false;
        
        private Boolean isInLoadData = true;

        public WinAddEditQuotation(String md, ObservableCollection<MBaseModel> pItems, MAuxilaryDoc actView, AuxilaryDocumentType dt, String qt)
        {   
            actualView = actView;
            parentItemsSource = pItems;
            Mode = md;
            docType = dt;
            quotationType = qt;

            vw = new MAuxilaryDoc(new CTable(""));
            vw.QuotationType = quotationType;

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

            return (true);
        }


        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblDayValidity, txtDayValidity, false, InputDataType.InputTypeZeroPossitiveInt);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateLookup(lblCustomer, uCustomer, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateComboBox(lblBranch, cboBranch, false);
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
            if (!CHelper.VerifyAccessRight("SALE_QUOTATION_EDIT"))
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
                CTable newobj = OnixWebServiceAPI.SaveAuxilaryDoc(vw.GetDbObject());                    
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
            CTable m = OnixWebServiceAPI.ApproveAuxilaryDoc(vw.GetDbObject().CloneAll());
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
            CTable m = OnixWebServiceAPI.GetAuxilaryDocInfo(vw.GetDbObject().CloneAll());
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
                    vw.InitRemarks();
                    quotationType = vw.QuotationType;

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
                vw.VatPct = CGlobalVariable.GetGlobalVariableValue("VAT_PERCENTAGE");
                vw.VatType = CGlobalVariable.GetGlobalVariableValue("DEFAULT_VAT_TYPE_PERCHASE");
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
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MAuxilaryDocItem) btn.Tag;
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
            if (quotationType.Equals("2"))
            {
                WinAddEditQuotationItemComplex w = new WinAddEditQuotationItemComplex();
                w.Caption = (String)(sender as Button).Content + " " + CLanguage.getValue("quotation_item");
                w.Mode = "A";
                w.ParentView = vw;
                w.ShowDialog();
                if (w.HasModified)
                {
                    vw.CalculateExtraFields();
                    vw.IsModified = true;
                }
            }
            else
            {
                WinAddEditQuotationItem w = new WinAddEditQuotationItem();
                w.Caption = (String)(sender as Button).Content + " " + CLanguage.getValue("quotation_item");
                w.Mode = "A";
                w.ParentView = vw;
                w.ShowDialog();
                if (w.HasModified)
                {
                    vw.CalculateExtraFields();
                    vw.IsModified = true;
                }
            }
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

        private void ShowEditWindow()
        {
            if (quotationType.Equals("1"))
            {
                WinAddEditQuotationItem w = new WinAddEditQuotationItem();
                w.ViewData = currentViewObj;
                w.Caption = CLanguage.getValue("ADMIN_EDIT") + " " + CLanguage.getValue("quotation_item");
                w.Mode = "E";
                w.ShowDialog();

                if (w.HasModified)
                {
                    vw.CalculateExtraFields();
                    vw.IsModified = true;
                }
            }
            else
            {
                WinAddEditQuotationItemComplex w = new WinAddEditQuotationItemComplex();
                w.ViewData = currentViewObj;
                w.Caption = CLanguage.getValue("ADMIN_EDIT") + " " + CLanguage.getValue("quotation_item");
                w.Mode = "E";
                w.ShowDialog();

                if (w.HasModified)
                {
                    vw.CalculateExtraFields();
                    vw.IsModified = true;
                }
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
            String group = "grpSaleQuotation";
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
            double[] ratios = new double[11] { 0.04, 0.04, 0.00, 0.10, 0.26, 0.08, 0.08, 0.12, 0.08, 0.12, 0.08 };
            CUtil.ResizeGridViewColumns(lsvPoItem.View as GridView, ratios, w);
        }

        private void cbxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            vw.CalculateExtraFields();
        }

        private void cboBranch_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void uSupplier_SelectedObjectChanged(object sender, EventArgs e)
        {
            if (!isInLoadData)
            {
                vw.IsModified = true;
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

        private void cmdRemarkDelete_Click(object sender, RoutedEventArgs e)
        {
            MAuxilaryDocRemark pp = (MAuxilaryDocRemark)(sender as Button).Tag;
            vw.RemoveRemarkItem(pp);

            vw.IsModified = true;
        }

        private void cmdRemarkAdd_Click(object sender, RoutedEventArgs e)
        {
            MAuxilaryDocRemark m = new MAuxilaryDocRemark(new CTable(""));

            vw.AddRemarkItem(m);
            vw.IsModified = true;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void uEmployee_SelectedObjectChanged_1(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void uCustomer_SelectedObjectChanged_1(object sender, EventArgs e)
        {
            if (!isInLoadData)
            {
                CTable cust = OnixWebServiceAPI.GetEntityInfo(vw.EntityObj.GetDbObject());
                MEntity en = new MEntity(cust);
                en.InitEntityAddress();

                vw.ContactPerson = en.ContactPerson;
                vw.ReloadEntityAddresses(en.AddressItems);
                vw.IsModified = true;

                if (en.AddressItems.Count > 0)
                {
                    cboAddress.SelectedIndex = 0;
                }
            }
        }

        private void lsvPoItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lsvAccoutItem_MouseDoubleClick(sender, null);
            }            
        }
    }
}
