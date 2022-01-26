using System;
using System.Windows;
using System.Collections;
using System.Printing;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.Client.Helper;
using Onix.Client.Controller;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinFormPrinting : Window
    {
        private String reportGroup = "";
        private CBaseReport rpt = null;
        private int zoomCnt = 100;
        private MBaseModel dataSource = null;

        private Hashtable reports = new Hashtable();
        private Hashtable reportGroups = new Hashtable();
        private ArrayList reportGroupKeys = new ArrayList();
        private Hashtable reportObjs = new Hashtable();
        private ObservableCollection<MReportGroup> groups = null;
        private Hashtable dbReportFilter = new Hashtable();
        private MMasterRef dummyContext = new MMasterRef(new CTable(""));
        private Boolean isInLoad = true;
        private PrintDialog dialog = new PrintDialog();

        public WinFormPrinting(String rptGroup, MBaseModel model)
        {
            reportGroup = rptGroup;
            dataSource = model;

            initReportList();
            initReportGroup();
            loadFilterFromDB();

            dummyContext.IsModified = false;

            DataContext = dummyContext;

            InitializeComponent();            
        }

        public ObservableCollection<MReportGroup> ReportTrees
        {
            get
            {
                return (constructReportTree());
            }
        }

        public Boolean IsConfigMode
        {
            get
            {
                return (reportGroup.Equals(""));
            }
        }

        private void initReportList()
        {
            String prefix = "Onix.ClientCenter.Forms.";

            reports.Add("SaleInvoiceFullCashContACD", prefix + "AcDesign.SaleInvoiceCash.CFormSaleInvoiceFullCash");
            reports.Add("SaleInvoiceDeptReceiptFullContACD", prefix + "AcDesign.SaleInvoice.CFormSaleInvoiceFull");
            reports.Add("SaleOrderACD", prefix + "AcDesign.SaleOrder.CFormSaleOrder");
            reports.Add("SaleBillSummaryACD", prefix + "AcDesign.BillSummary.CFormBillSummary");
            reports.Add("official_receipt", prefix + "AcDesign.OfficialReceipt.CFormOfficialReceipt");
            reports.Add("PurchaseOrder_Form", prefix + "AcDesign.PurchaseOrder.CFormPurchaseOrder");
            reports.Add("PurchaseOrderService_Form", prefix + "AcDesign.PurchaseOrder.CFormPurchaseOrderService");
            reports.Add("payment_voucher_new", prefix + "AcDesign.PaymentVoucher.CPaymentVoucher");
            reports.Add("payment_voucher_cont", prefix + "AcDesign.PaymentVoucher.CPaymentVoucher");
            reports.Add("receive_voucher_a4", prefix + "AcDesign.ReceiveVoucher.CReceiveVoucher");
            reports.Add("payable_cheque", prefix + "AcDesign.Cheque.CFormChequePay");
            reports.Add("AcctDocDrNote", prefix + "AcDesign.DrCrNote.CFormDrCrNote");
            reports.Add("AcctDocCrNote", prefix + "AcDesign.DrCrNote.CFormDrCrNote");
            reports.Add("no_resident_witholding_tax", prefix + "AcDesign.WitholdingTax.CFormNonResidentWithholdingTax");
            reports.Add("no_resident_witholding_tax_con", prefix + "AcDesign.WitholdingTax.CFormNonResidentWithholdingTax");
            reports.Add("no_resident_witholding_tax_half_a4", prefix + "AcDesign.WitholdingTax.CFormNonResidentWithholdingTax2Pages");            
            reports.Add("quotation_complext", prefix + "AcDesign.Quotation.CFormQuotationComplex");
            reports.Add("quotation_attached_sheet", prefix + "AcDesign.Quotation.CFormQuotationAttachSheet");

            reports.Add("inventory_borrow_document", prefix + "AcDesign.Inventory.CFormInventoryBorrowDoc");
            reports.Add("inventory_export_document", prefix + "AcDesign.Inventory.CFormInventoryExportDoc");

            reports.Add("hr_payroll_slip", prefix + "AcDesign.HRPayrollSlip.CFormPayrollSlip");
            reports.Add("hr_payroll_withdraw", prefix + "AcDesign.HRPayrollWithdraw.CFormPayrollWithdraw");
            reports.Add("hr_payroll_deposit", prefix + "AcDesign.HRPayrollDeposit.CFormPayrollDeposit");

            reports.Add("hr_employee_leave", prefix + "AcDesign.HREmployeeLeave.CFormEmployeeLeave");

            reports.Add("hr_employee_witholding_tax", prefix + "AcDesign.HREmployeeTax.CFormEmployeeWithholdingTax");
        }

        private void initReportGroup()
        {            
            addReportGroup("grpSaleQuotation", "quotation_complext");
            addReportGroup("grpSaleQuotation", "quotation_attached_sheet");

            addReportGroup("grpSaleByCashInvoice", "SaleInvoiceFullCashContACD");
            addReportGroup("grpSaleByCashInvoice", "receive_voucher_a4");
            addReportGroup("grpSaleByDebtInvoice", "SaleInvoiceDeptReceiptFullContACD");
            addReportGroup("grpSaleMiscInvoice", "receive_voucher_a4");

            addReportGroup("grpSaleOrder", "SaleOrderACD");

            addReportGroup("grpBillSummary", "SaleBillSummaryACD");

            addReportGroup("grpSaleArReceipt", "official_receipt");
            addReportGroup("grpSaleArReceipt", "receive_voucher_a4");

            addReportGroup("grpPurchasePO", "PurchaseOrder_Form");
            addReportGroup("grpPurchasePO", "PurchaseOrderService_Form");

            addReportGroup("grpPurchaseByCashInvoice", "payment_voucher_new");
            addReportGroup("grpPurchaseMiscInvoice", "payment_voucher_new");
            addReportGroup("grpPurchaseApReceipt", "payment_voucher_new");

            addReportGroup("grpPurchaseByCashInvoice", "payment_voucher_cont");
            addReportGroup("grpPurchaseByCashInvoice", "no_resident_witholding_tax");
            addReportGroup("grpPurchaseByCashInvoice", "no_resident_witholding_tax_con");
            addReportGroup("grpPurchaseByCashInvoice", "no_resident_witholding_tax_half_a4");

            addReportGroup("grpPurchaseByDebtInvoice", "no_resident_witholding_tax");
            addReportGroup("grpPurchaseByDebtInvoice", "no_resident_witholding_tax_con");

            addReportGroup("grpPurchaseMiscInvoice", "payment_voucher_cont");

            addReportGroup("grpPurchaseApReceipt", "payment_voucher_cont");
            addReportGroup("grpPurchaseApReceipt", "no_resident_witholding_tax");
            addReportGroup("grpPurchaseApReceipt", "no_resident_witholding_tax_con");
            addReportGroup("grpPurchaseApReceipt", "no_resident_witholding_tax_half_a4");

            addReportGroup("grpChequePayment", "payable_cheque");
            addReportGroup("grpDrNote", "AcctDocDrNote");
            addReportGroup("grpCrNote", "AcctDocCrNote");

            addReportGroup("grpInventoryBorrow", "inventory_borrow_document");
            addReportGroup("grpInventoryExport", "inventory_export_document");

            addReportGroup("grpHRSlip", "hr_payroll_slip");
            addReportGroup("grpHRSlip", "hr_payroll_withdraw");
            addReportGroup("grpHRSlip", "hr_payroll_deposit");

            addReportGroup("grpHRLeave", "hr_employee_leave");

            addReportGroup("grpHRTax", "hr_employee_witholding_tax");
        }

        private void addReportGroup(String grpName, String rptName)
        {
            ArrayList arr = null;
            if (!reportGroups.ContainsKey(grpName))
            {
                arr = new ArrayList();
                reportGroups.Add(grpName, arr);
                reportGroupKeys.Add(grpName);
            }
            else
            {
                arr = (ArrayList) reportGroups[grpName];
            }

            arr.Add(rptName);
        }

        private ObservableCollection<MReportGroup> constructReportTree()
        {
            groups = new ObservableCollection<MReportGroup>();
            MReportFilter r = null;

            foreach (String key in reportGroupKeys)
            {
                if (!reportGroup.Equals("") && !reportGroup.Equals(key))
                {
                    continue;
                }

                MReportGroup rg = new MReportGroup(new CTable(""));
                rg.GroupName = CLanguage.getValue(key);

                groups.Add(rg);

                ArrayList rpts = (ArrayList)reportGroups[key];
                if (rpts == null)
                {
                    rpts = new ArrayList();
                }

                int index = 0;
                foreach (String name in rpts)
                {
                    r = new MReportFilter(new CTable(""));
                    r.ReportName = name;
                    r.ReportGroup = key;
                    r.Description = CLanguage.getValue(name);

                    if (dbReportFilter.ContainsKey(r.Key))
                    {
                        MReportFilter rf = (MReportFilter)dbReportFilter[r.Key];
                        r.ReportFilterID = rf.ReportFilterID;                        
                        r.IsSelected = rf.IsSelected;
                        r.ExtFlag = "I";
                    }
                    else
                    {                        
                        r.IsSelected = false;
                        r.ExtFlag = "A";
                    }

                    if (index == 0)
                    {
                        r.IsNodeSelected = true;
                    }

                    if (!reportGroup.Equals("") && !r.IsSelected)
                    {
                        //Normal mode but not yet selected
                        continue;
                    }

                    rg.AddItem(r);
                    index++;
                }
            }

            return (groups);
        }

        private void cmdZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (zoomCnt > 200)
            {
                return;
            }

            zoomCnt = zoomCnt + 5;
            docViewer.Zoom = zoomCnt;
        }

        private void cmdZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (zoomCnt == 5)
            {
                return;
            }

            zoomCnt = zoomCnt - 5;
            docViewer.Zoom = zoomCnt;
        }

        private void cmdFitWidth_Click(object sender, RoutedEventArgs e)
        {
            docViewer.FitToWidth();
            zoomCnt = (int)docViewer.Zoom;
        }

        private void cmdFitHeigh_Click(object sender, RoutedEventArgs e)
        {
            docViewer.FitToHeight();
            zoomCnt = (int)docViewer.Zoom;
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (trvMain.SelectedItem is MReportGroup)
            {
                return;
            }

            MReportFilter mr = (MReportFilter)trvMain.SelectedItem;
            if (mr == null)
            {
                return;
            }

            CBaseReport paginator = (CBaseReport)reportObjs[mr.ReportName];
            if (paginator == null)
            {
                return;
            }

            Boolean isPageRange = (Boolean)cbxPageRange.IsChecked;

            if (isPageRange)
            {
                populatePageRange(paginator);

                if (paginator.toPage < paginator.fromPage)
                {
                    CHelper.ShowErorMessage("", "ERROR_PAGE_RANGE", null);
                    return;
                }
            }

            dialog.PrintQueue = LocalPrintServer.GetDefaultPrintQueue();
            dialog.PrintTicket = dialog.PrintQueue.DefaultPrintTicket;
            dialog.PrintTicket.PageOrientation = paginator.GetPageOrientation();
            dialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.Unknown, paginator.PageSize.Width, paginator.PageSize.Height);
            bool? result = dialog.ShowDialog();            

            if (result == true)
            {
                paginator.isPageRange = isPageRange;

                FixedDocument fd = null;
                if (!isPageRange)
                {
                    fd = paginator.GetFixedDocument();                    
                }
                else
                {
                    fd = paginator.CreateFixedDocument();
                }

                docViewer.Document = paginator.GetFixedDocument();
                dialog.PrintDocument(fd.DocumentPaginator, "");

                CUtil.LoadPageNavigateCombo(cboPageNo, paginator.PageCount);
            }
        }

        private void cboPageNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;

            MChunkNavigate item = (MChunkNavigate)cbo.SelectedItem;
            if (item == null)
            {
                return;
            }

            int pageno = int.Parse(item.PageNo);
            docViewer.GoToPage(pageno);
        }

        private CBaseReport createObject(String id)
        {
            String className = (String) reports[id];

            CBaseReport rp = (CBaseReport)Activator.CreateInstance(Type.GetType(className));
            return (rp);
        }

        private void trvMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!(e.NewValue is MReportFilter))
            {
                return;
            }

            if (isInLoad && IsConfigMode)
            {
                return;
            }

            MReportFilter mr = (MReportFilter) e.NewValue;
            String rptId = mr.ReportName;

            if (reportObjs.ContainsKey(rptId))
            {
                rpt = (CBaseReport)reportObjs[rptId];
            }
            else
            {
                CBaseReport r = createObject(rptId);
                CUtil.EnableForm(false, this);
                MReportConfig rptCfg = CReportConfigs.GetReportConfig(null, rptId);
                CUtil.EnableForm(true, this);
                MReportConfig defaultCfg = r.CreateDefaultConfigValues();
                defaultCfg.ReportName = rptId;

                if (rptCfg == null)
                {
                    rptCfg = defaultCfg;
                    CReportConfigs.SaveReportConfig(null, rptCfg);
                }
                else
                {
                    rptCfg.PopulateMissingValue(defaultCfg);
                }

                MBaseModel d = null;
                if (dataSource == null)
                {
                    d = r.CreateDefaultData();
                }
                else
                {
                    d = dataSource;
                }

                r.SetDataSourceAndParam(d, rptCfg);
                r.isPageRange = false;
                r.CreateFixedDocument();
                
                reportObjs.Add(rptId, r);
                rpt = r;
            }

            cmdSave.IsEnabled = rpt.GetReportConfig().IsModified;
            cbxPageRange.IsEnabled = rpt.IsPageRangeSupport;

            this.Title = mr.Description;
            docViewer.Document = rpt.GetFixedDocument();

            CUtil.LoadPageNavigateCombo(cboPageNo, rpt.PageCount);
            CUtil.LoadPageCombo(cboFromPage, rpt.PageCount);
            CUtil.LoadPageCombo(cboToPage, rpt.PageCount);
        }

        private void loadFilterFromDB()
        {
            CUtil.EnableForm(false, this);

            CTable t = OnixWebServiceAPI.GetReportFilterInfo(new CTable(""));
            ArrayList arr = t.GetChildArray("REPORT_FILTER_ITEM");
            foreach (CTable o in arr)
            {
                MReportFilter rf = new MReportFilter(o);
                dbReportFilter.Add(rf.Key, rf);
            }

            CUtil.EnableForm(true, this);
        }

        private void rootElement_ContentRendered(object sender, EventArgs e)
        {
            isInLoad = false;
        }

        private void cmdConfig_Click(object sender, RoutedEventArgs e)
        {
            if (rpt == null)
            {
                return;
            }

            MReportConfig cfg = rpt.GetReportConfig();
            WinFormConfigParam wc = new WinFormConfigParam("", cfg);
            wc.ShowDialog();

            if (wc.IsOK)
            {
                cfg = wc.Config;
                cfg.IsModified = true;

                rpt.UpdateReportConfig(wc.Config);
                cmdSave.IsEnabled = cfg.IsModified;

                rpt.UpdateReportConfig(cfg);
                rpt.isPageRange = false;
                rpt.SetDataSourceAndParam(dataSource, cfg);
                rpt.CreateFixedDocument();
                docViewer.Document = rpt.GetFixedDocument();

                //CUtil.LoadPageNavigateCombo(cboPageNo, rpt.PageCount);
                //CUtil.LoadPageCombo(cboFromPage, rpt.PageCount);
                //CUtil.LoadPageCombo(cboToPage, rpt.PageCount);
            }
        }

        private void populatePageRange(CBaseReport rpt)
        {
            if (!(Boolean) cbxPageRange.IsChecked)
            {
                return;
            }

            MChunkNavigate item1 = (MChunkNavigate)cboFromPage.SelectedItem;
            rpt.fromPage = 0;
            if (item1 != null)
            {
                rpt.fromPage = int.Parse(item1.PageNo);
            }

            MChunkNavigate item2 = (MChunkNavigate)cboToPage.SelectedItem;
            rpt.toPage = 0;
            if (item2 != null)
            {
                rpt.toPage = int.Parse(item2.PageNo);
            }
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (rpt == null)
            {
                return;
            }

            MReportConfig cfg = rpt.GetReportConfig();            
            CReportConfigs.SaveReportConfig(null, cfg);

            cfg.IsModified = false;
            cmdSave.IsEnabled = cfg.IsModified;
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            ArrayList arr = new ArrayList();
            foreach (MReportGroup rg in groups)
            {
                foreach (MReportFilter rf in rg.Items)
                {
                    rf.ReportSeq = "1";
                    rf.ReportNs = "1";
                    arr.Add(rf.GetDbObject());
                }
            }

            CTable o = new CTable("");
            o.AddChildArray("REPORT_FILTER_ITEM", arr);

            CUtil.EnableForm(false, this);
            OnixWebServiceAPI.CreateReportFilter(o);
            CUtil.EnableForm(true, this);

            this.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            dummyContext.IsModified = true;
        }

        private void rootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }        
    }
}
