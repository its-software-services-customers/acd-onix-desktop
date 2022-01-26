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
using Onix.Client.Pricing;
using System.Collections;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public class CItemPriceAccum
    {
        public double Quantity = 0.0;
        public double Amount = 0.0;
        public double Discount = 0.0;
        public double AmountPerUnit = 0.0;
        public double DiscountPerUnit = 0.0;
    }

    public partial class WinAddEditAccountSaleDoc : Window
    {
        private MAccountDoc vw = null;
        private MAccountDoc actualView = null;
        private ObservableCollection<MBaseModel> parentItemsSource = null;
        private MCompanyPackage companyPackage = null;
        private MAccountDocItem currentViewObj = null;
        private MLogImportIssue logImportIssue = null;

        private ObservableCollection<CBasketItemDisplay> resultItems = new ObservableCollection<CBasketItemDisplay>();
        private Hashtable priceHash = new Hashtable();
        private AccountDocumentType dt;
        private Boolean isActivated = false;

        public Boolean DialogOK = false;
        public String Caption = "";
        public String Mode = "";

        private Boolean isInLoadData = true;
        private Boolean isBillCorrection = false;
        private RoutedEventHandler itemAddedHandler = null;
        private Boolean needPreview = false;
        private String createdID = "0";
        private Boolean isPromotionMode = false;

        public WinAddEditAccountSaleDoc(String md, AccountDocumentType docType, ObservableCollection<MBaseModel> pItems, MAccountDoc actView, Boolean isPromotion)
        {
            dt = docType;
            isPromotionMode = isPromotion;

            actualView = actView;
            parentItemsSource = pItems;
            Mode = md;

            if (md.Equals("A") && (actualView != null))
            {
                vw = actualView;
            }
            else
            {
                vw = new MAccountDoc(new CTable(""));
            }
            vw.IsVatClaimable = true;
            DataContext = vw;

            InitializeComponent();
        }

        public WinAddEditAccountSaleDoc(String md, AccountDocumentType docType, ObservableCollection<MBaseModel> pItems, MLogImportIssue actView)
        {
            dt = docType;

            isBillCorrection = true;
            logImportIssue = actView;
            Mode = md;

            CTable o = new CTable("");
            o.SetFieldValue("LOG_IMPORT_ISSUE_ID", logImportIssue.LogImportIssueID);

            vw = new MAccountDoc(o);
            vw.DocumentType = ((int)dt).ToString();
            DataContext = vw;

            InitializeComponent();
        }

        #region Properties
        public Boolean IsPromotionMode
        {
            get
            {
                return (isPromotionMode);
            }
        }

        public Boolean IsSaleOrder
        {
            get
            {
                return (dt == AccountDocumentType.AcctDocSaleOrder);
            }
        }

        public Visibility AllowARAPNegativeVisibility
        {
            get
            {
                if (dt == AccountDocumentType.AcctDocCashSale)
                {
                    return (Visibility.Collapsed);
                }

                return (Visibility.Visible);
            }

            set
            {
            }
        }

        public Visibility PayTypeVisibility
        {
            get
            {
                if (dt == AccountDocumentType.AcctDocDebtSale)
                {
                    return (Visibility.Collapsed);
                }

                if (dt == AccountDocumentType.AcctDocSaleOrder)
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

        public ObservableCollection<CBasketItemDisplay> VoucherItems
        {
            get
            {
                return (vw.BillSimulate.VoucherItems);
            }
        }

        public ObservableCollection<CBasketItemDisplay> FreeItems
        {
            get
            {
                return (vw.BillSimulate.FreeItems);
            }
        }

        public ObservableCollection<CBasketItemDisplay> PostGiftItems
        {
            get
            {
                return (vw.BillSimulate.PostGiftItems);
            }
        }

        public ObservableCollection<CBasketItemDisplay> ResultItems
        {
            get
            {
                return (vw.BillSimulate.ResultItems);
            }
        }

        public String TotalAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(vw.BillSimulate.TotalAmount));
            }

            set
            {
            }
        }

        public String DiscountAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(vw.BillSimulate.DiscountAmount));
            }

            set
            {
            }
        }

        public String NetAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(vw.BillSimulate.NetAmount));
            }

            set
            {
            }
        }

        public ObservableCollection<CProcessingResultGroup> ProcessingTree
        {
            get
            {
                return (vw.BillSimulate.ProcessingTree);
            }
        }

        public Boolean IsPreviewNeed
        {
            get
            {
                return (needPreview);
            }
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }


        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateLookup(lblSaleMan, uSalesman, false);
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

            Boolean isService = !vw.IsInInventory();
            result = CHelper.ValidateComboBox(lblFrom, cboFrom, isService);
            if (!result)
            {
                return (result);
            }

            if (vw.IsChangeByDrCr)
            {
                Boolean flag = CHelper.AskConfirmMessage("", "ERROR_CHANGE_BY_DRCR");
                return (flag);
            }

            if (dt == AccountDocumentType.AcctDocSaleOrder)
            {
                result = CHelper.ValidateLookup(lblProject, uProject, false);
                if (!result)
                {
                    return (result);
                }
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

            if (isBillCorrection)
            {
                //logImportIssue
                m = OnixWebServiceAPI.SaveUploadedAccountDoc(vw.GetDbObject());
            }
            else
            {
                m = OnixWebServiceAPI.UpdateAccountDoc(vw.GetDbObject());
            }

            return (m);
        }

        private CTable approveAccountDocWrapper()
        {
            CTable m = null;

            vw.IsPromotionMode = isPromotionMode;
            m = OnixWebServiceAPI.ApproveAccountDoc(vw.GetDbObject().CloneAll());

            //ตอนนี้ยังไม่มีการขายสดแล้วรับเช็คเลยยังไม่ต้อง approve cheque
            //if (vw.ChequeID.Equals(""))
            //{
            //    return (m);
            //}

            ////Approve cheque as well
            //CHelper.ApproveChequeFromAccountDoc(vw);

            return (m);
        }

        private String getAccessRightEdit()
        {
            String acr = "SALE_UNKNOW_EDIT";

            if (dt == AccountDocumentType.AcctDocCashSale)
            {
                acr = "SALE_BYCASH_EDIT";
            }
            else if (dt == AccountDocumentType.AcctDocDebtSale)
            {
                acr = "SALE_BYCREDIT_EDIT";
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

                populateBillSimulate(vw);
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
                    vw.IsPromotionMode = isPromotionMode;
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
                        CTable t = updateDocumentWrapper();
                        CUtil.EnableForm(true, this);
                        if (t != null)
                        {
                            if (!isBillCorrection)
                            {
                                actualView.SetDbObject(t);
                                (actualView as MAccountDoc).NotifyAllPropertiesChanged();
                            }
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

            if (isBillCorrection)
            {
                m = OnixWebServiceAPI.GetUploadedAccountDocInfo(logImportIssue.GetDbObject());
            }
            else
            {
                actualView.EntityAddressFlag = "Y";
                m = OnixWebServiceAPI.GetAccountDocInfo(actualView.GetDbObject());
            }

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
                    vw.InitAccountDocItem();
                    vw.InitAccountDocPayment();
                    String tmp = vw.EntityAddressID;
                    vw.InitEntityAddresses();
                    vw.EntityAddressID = tmp;
                    vw.AddressObj = CUtil.MasterIDToObject(vw.EntityAddresses, vw.EntityAddressID);

                    vw.BillSimulate.InitPromotionItems();
                    vw.BillSimulate.InitSelectedItems();

                    vw.NotifyAllPropertiesChanged();                  
                }

                vw.IsModified = false;
            }
            else if (Mode.Equals("A"))
            {
                vw.DocumentDate = DateTime.Now;                
                vw.VAT_PCT = CGlobalVariable.GetGlobalVariableValue("VAT_PERCENTAGE");
                vw.VATType = CGlobalVariable.GetGlobalVariableValue("DEFAULT_VAT_TYPE_SALE");
                vw.AllowCashNegative = CGlobalVariable.IsCashNegativeAllow();
                vw.AllowARAPNegative = CGlobalVariable.IsArApNegativeAllow();
                vw.AllowInventoryNegative = CGlobalVariable.IsInventoryNegativeAllow();

                vw.IsModified = (actualView != null);
            }

            
            isInLoadData = false;

            CUtil.EnableForm(true, this);
        }

        private void loadCompanyPackage()
        {
            CUtil.EnableForm(false, this);
            companyPackage = CMasterReference.GetCompanyPackage(false);
            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            loadCompanyPackage();
            LoadData();            
        }

        private void cbxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            calculatePromotion();
            vw.CalculateExtraFields();
            vw.IsModified = true;
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {           
            vw.IsModified = true;            
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

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());

            if (isActivated)
            {
                return;
            }

            isActivated = true;
        }

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            if (dt == AccountDocumentType.AcctDocCashSale)
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

            if (isBillCorrection)
            {
                m = OnixWebServiceAPI.VerifyUploadedAccountDoc(vw.GetDbObject().CloneAll());
            }
            else
            {
                m = OnixWebServiceAPI.VerifyAccountDoc(vw.GetDbObject().CloneAll());
            }

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
            WinAddEditAccountSaleDocItem w = new WinAddEditAccountSaleDocItem(isPromotionMode, IsSaleOrder);
            w.Caption = (String)(sender as Button).Content + " " + CLanguage.getValue("sale_item");
            w.Mode = "A";
            w.ParentView = (vw as MAccountDoc);
            w.ShowDialog();
            if (w.HasModified)
            {
                calculatePromotion();
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void updateDisplayItem(CBasketItemDisplay bi)
        {
            String key = String.Format("{0}-{1}-{2}-{3}", bi.SelectionType, bi.ItemID, bi.ServiceID, bi.IsTray);
            CItemPriceAccum hbi = (CItemPriceAccum) priceHash[key];

            if (hbi == null)
            {
                hbi = new CItemPriceAccum();

                hbi.Amount = bi.Amount;
                hbi.Quantity = bi.Quantity;
                hbi.Discount = bi.Discount;

                priceHash[key] = hbi;
            }
            else
            {
                hbi.Amount = hbi.Amount + bi.Amount;
                hbi.Quantity = hbi.Quantity + bi.Quantity;
                hbi.Discount = hbi.Discount + bi.Discount;
            }

            hbi.DiscountPerUnit = hbi.Discount / hbi.Quantity;
            hbi.AmountPerUnit = hbi.Amount / hbi.Quantity;
        }

        private String getKey(MAccountDocItem di)
        {
            String key = String.Format("{0}-{1}-{2}-{3}", di.SelectType, di.ItemId, di.ServiceID, di.IsTrayFlag);
            return (key);
        }

        private void updateDocumentItemPrice(MAccountDoc doc)
        {
            ObservableCollection<MAccountDocItem> items = vw.AccountItem;
            Hashtable quantityHash = new Hashtable();

            foreach (MAccountDocItem di in items)
            {
                String key = getKey(di);

                String flag = di.ExtFlag;
                if (flag.Equals("D"))
                {
                    continue;
                }

                CItemPriceAccum qbi = (CItemPriceAccum)quantityHash[key];
                if (qbi == null)
                {
                    qbi = new CItemPriceAccum();
                    qbi.Quantity = CUtil.StringToDouble(di.Quantity);
                    quantityHash[key] = qbi;
                }
                else
                {
                    qbi.Quantity = qbi.Quantity + CUtil.StringToDouble(di.Quantity);
                }
            }

            foreach (MAccountDocItem di in items)
            {
                String flag = di.ExtFlag;
                if (flag.Equals("D"))
                {
                    continue;
                }

                String key = getKey(di);
                CItemPriceAccum hbi = (CItemPriceAccum)priceHash[key];

                CItemPriceAccum qdi = (CItemPriceAccum) quantityHash[key];
                double qty = qdi.Quantity;
                double amt = 0.00;
                double unitPrice = 0.00;
                double discount = 0.00;
                double discountPerUnit = discount / qty;

                if (hbi == null)
                {
                    di.Quantity = qty.ToString();
                    di.Amount = "0.00";
                    di.Discount = "0.00";
                    di.TotalAfterDiscount = "0.00";
                    di.TOTAL_AMT = "0.00";
                }
                else
                {
                    amt = hbi.Amount;
                    unitPrice = amt / qty;
                    discount = hbi.Discount;
                    discountPerUnit = discount / qty;


                    double itemQty = CUtil.StringToDouble(di.Quantity);
                    double itemAmt = unitPrice * itemQty;
                    double itemDisc = discountPerUnit * itemQty;

                    double afterDiscount = itemAmt - itemDisc;

                    di.UnitPrice = unitPrice.ToString();
                    di.Amount = itemAmt.ToString();                    
                    di.Discount = itemDisc.ToString();
                    di.TotalAfterDiscount = afterDiscount.ToString();
                    di.TOTAL_AMT = di.TotalAfterDiscount;
                }

                if (!flag.Equals("A"))
                {
                    di.ExtFlag = "E";
                }
            }
        }

        private void processPromotion(MAccountDoc doc)
        {
            MBillSimulate billSim = doc.BillSimulate;
            priceHash.Clear();

            CPriceProcessor.SetGetCompanyPackageAllCallback(OnixWebServiceAPI.GetCompanyPackageAll);

            CUtil.EnableForm(false, this);
            CPriceProcessor.LoadStandardPackages(companyPackage);
            CUtil.EnableForm(true, this);

            CBasketSet bks = CPriceProcessor.CreateInitialBasketSet(billSim.SelectedItems);

            CUtil.EnableForm(false, this);
            Boolean tmp = billSim.IsModified;

            CBasketSet output = CPriceProcessor.PromotionProcessing(companyPackage, bks, billSim);
            CPriceProcessor.CreateDisplayProcessingTreeView(billSim);
            CPriceProcessor.CreateDisplayView(output, billSim, updateDisplayItem);

            doc.PromotionAmount = billSim.TotalAmount;
            doc.PromotionFinalDiscount = billSim.DiscountAmount;
            doc.PromotionTotalAmt = billSim.NetAmount;
            doc.FreeItemCount = billSim.FreeItemCount.ToString();
            doc.VoucherItemCount = billSim.VoucherItemCount.ToString();
            doc.PostFreeItemCount = billSim.PostFreeItemCount.ToString();
            updateDocumentItemPrice(doc);
            doc.NotifyPromotionCalulation();

            doc.FinalDiscount = billSim.DiscountAmount;

            CUtil.EnableForm(true, this);
        }

        private void populateBillSimulate(MAccountDoc doc)
        {
            MBillSimulate bs = doc.BillSimulate;            
            bs.ClearSelectedItem();

            bs.DocumentDate = doc.DocumentDate;
            bs.SimulateTime = doc.DocumentDate;
            bs.CustomerObj = doc.CustomerObj;
            bs.CustomerID = doc.EntityId;
            bs.BranchId = doc.BranchId;
            bs.DocumentType = doc.DocumentType;
            bs.DocumentStatus = doc.DocumentStatus;

            foreach (MAccountDocItem di in doc.AccountItem)
            {
                if (di.ExtFlag.Equals("D"))
                {
                    continue;
                }

                MSelectedItem si = new MSelectedItem(new CTable(""));

                si.TrayFlag = "N";
                if (di.IsTrayFlag == true)
                {
                    si.TrayFlag = "Y";
                }

                si.SelectionType = di.SelectType;
                si.ServiceID = di.ServiceID;
                si.ItemID = di.ItemId;
                si.ServiceCode = di.ServiceCode;
                si.ServiceName = di.ServiceName;
                si.ItemCode = di.ItemCode;
                si.ItemNameThai = di.ItemNameThai;
                si.ItemQuantity = di.Quantity;
                si.ItemCategory = di.ItemCategory;
                si.ServicePricingDefinition = di.ServicePricingDefinition;
                si.PricingDefination = di.PricingDefinition;
                si.EnabledFlag = "Y";

                bs.AddSelectedItem(si);
            }
        }

        private void uCustomer_SelectedObjectChanged(object sender, EventArgs e)
        {
            if (!isInLoadData)
            {
                if (!vw.CreditTerm.Equals(""))
                {
                    vw.DueDate = vw.DocumentDate.AddDays(CUtil.StringToInt(vw.CreditTerm));
                }

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
            if (lsvAccoutItem.SelectedItems.Count == 1)
            {
                currentViewObj = (MAccountDocItem)lsvAccoutItem.SelectedItems[0];
                ShowEditWindow();
            }
        }

        private void ShowEditWindow()
        {
            WinAddEditAccountSaleDocItem w = new WinAddEditAccountSaleDocItem(isPromotionMode, IsSaleOrder);
            w.ViewData = currentViewObj;
            w.Caption = CLanguage.getValue("ADMIN_EDIT") + " " + CLanguage.getValue("sale_item");
            w.ParentView = (vw as MAccountDoc);
            w.Mode = "E";
            w.ShowDialog();

            if (w.HasModified)
            {
                calculatePromotion();
                vw.CalculateExtraFields();
                vw.IsModified = true;
            }
        }

        private void lsvFree_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvFree.View as GridView;

            double w = (e.NewSize.Width * 1) - 35;
            double[] ratios = new double[8] { 0.05, 0.15, 0.35, 0.10, 0.10, 0.10, 0.10, 0.05 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void lsvFree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void lsvVoucher_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvVoucher.View as GridView;

            double w = (e.NewSize.Width * 1) - 35;
            double[] ratios = new double[8] { 0.05, 0.15, 0.35, 0.10, 0.10, 0.10, 0.10, 0.05 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void lsvPostGift_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvPostGift.View as GridView;

            double w = (e.NewSize.Width * 1) - 35;
            double[] ratios = new double[7] { 0.05, 0.15, 0.40, 0.10, 0.10, 0.10, 0.10 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void lsvMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvMain.View as GridView;

            double w = (e.NewSize.Width * 1) - 35;
            double[] ratios = new double[8] { 0.05, 0.15, 0.35, 0.10, 0.10, 0.10, 0.10, 0.05 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void populateItems(ObservableCollection<CBasketItemDisplay> results, ObservableCollection<CBasketItemDisplay> processed)
        {
            resultItems.Clear();

            foreach (CBasketItemDisplay bd in processed)
            {
                resultItems.Add(bd);
            }
        }

        private void calculatePromotion()
        {
            if (!isPromotionMode)
            {
                return;
            }

            populateBillSimulate(vw);
            processPromotion(vw);
            vw.IsModified = true;
        }

        private void cmdCalculate_Click(object sender, RoutedEventArgs e)
        {
            calculatePromotion();
            vw.CalculateExtraFields();
        }

        private void trvMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void lsvAccoutItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvAccoutItem.ActualWidth * 1) - 35;
            double[] ratios = new double[10] { 0.04, 0.04, 0.06, 0.14, 0.28, 0.08, 0.08, 0.10, 0.08, 0.10 };
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

        private void cmdPayment_Click(object sender, RoutedEventArgs e)
        {            
            WinAddEditPayment w = new WinAddEditPayment(vw.CashActualReceiptAmt, vw, true);
            w.ShowDialog();

            if (w.IsOK)
            {
                vw.CalculateReceiveAndChange();
                vw.IsModified = true;
            }
        }

        private void cmdApprovePrint_Click(object sender, RoutedEventArgs e)
        {
            needPreview = true;
            cmdApprove_Click(sender, e);
        }

        private void cmdPreview_Click(object sender, RoutedEventArgs e)
        {
            String group = "grpSaleByCashInvoice";
            if (dt == AccountDocumentType.AcctDocDebtSale)
            {
                group = "grpSaleByDebtInvoice";
            }
            else if (dt == AccountDocumentType.AcctDocSaleOrder)
            {
                group = "grpSaleOrder";
            }

            vw.ConstructWhDefinitionFromDocItem();

            WinFormPrinting w = new WinFormPrinting(group, vw);
            w.ShowDialog();
        }

        private void lsvAccoutItem_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lsvAccoutItem_MouseDoubleClick(sender, null);
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

        private void uProject_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void dtVatDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdSaleOrderUnlink_Click(object sender, RoutedEventArgs e)
        {
            CTable m = new CTable("");
            m.SetFieldValue("ACCOUNT_DOC_ID", vw.RefSaleOrderID);

            CUtil.EnableForm(false, this);
            CTable t = OnixWebServiceAPI.GetAccountDocInfo(m);
            CUtil.EnableForm(true, this);

            if (t == null)
            {
                return;
            }

            MAccountDoc vm = new MAccountDoc(t);            
            Boolean ok = CHelper.AskConfirmMessage(vm.DocumentNo, "ERROR_SALE_ORDER_UNLINK");            

            if (!ok)
            {
                return;
            }

            OnixWebServiceAPI.UnlinkSaleOrderFromInvoice(vw.GetDbObject());
            vw.RefSaleOrderID = "";

        }
    }
}
