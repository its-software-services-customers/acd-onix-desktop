using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Model;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.ClientCenter.Windows;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Criteria
{
    public class CCriteriaAccountDocSale : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;

        private AccountDocumentType docType = AccountDocumentType.AcctDocCashSale;

        public CCriteriaAccountDocSale() : base(new MAccountDoc(new CTable("")), "CCriteriaAccountDocSale")
        {

        }

        public override void Init(String type)
        {
            (model as MAccountDoc).IsInternalDrCr = false;

            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
        }

        #region Criteria Configure

        private void loadRelatedReferences()
        {
            if (!CMasterReference.IsLocationLoad())
            {
                CMasterReference.LoadLocation(true, null);
            }

            if (!CMasterReference.IsCashAccountLoad())
            {
                CMasterReference.LoadCashAccount(OnixWebServiceAPI.GetCashAccountList);
            }
        }

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2_1 = new CCriteriaContextMenu("mnuCopyHead", "copy_head", new RoutedEventHandler(mnuContextMenu_Click), 2);
            contexts.Add(ct2_1);

            //ปิด feature นี้เพราะว่าจะทำให้ ตัว item ลูกมีการลิงค์กันซ้ำไปยังเอกสารอื่น ๆ ได้ จะทำให้ยุ่งเหยิงมากขึ้น
            //CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuCopy", "copy", new RoutedEventHandler(mnuContextMenu_Click), 2);
            //contexts.Add(ct2);

            CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuPrint", "print", new RoutedEventHandler(mnuContextMenu_Click), 3);
            contexts.Add(ct3);

            if ((docType == AccountDocumentType.AcctDocDebtSale) || 
                (docType == AccountDocumentType.AcctDocCashSale) ||
                (docType == AccountDocumentType.AcctDocArReceipt) ||
                (docType == AccountDocumentType.AcctDocBillSummary) ||
                (docType == AccountDocumentType.AcctDocMiscRevenue) ||
                (docType == AccountDocumentType.AcctDocSaleOrder))
            {
                CCriteriaContextMenu ct4 = new CCriteriaContextMenu("mnuCancel", "void_document", new RoutedEventHandler(mnuVoidMenu_Click), 4);
                contexts.Add(ct4);
            }

            return (contexts);
        }

        private void createGridColumns()
        {
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left, cbxSelect_Checked, cbxSelect_UnChecked);
            c0.RegisterCheckboxBindingVariable(CheckBox.IsEnabledProperty, "IsEditable");
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocNo", "inventory_doc_no", "DocumentNo", 19, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDocDate", "inventory_doc_date", "DocumentDateFmt", 11, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colDocDesc", "inventory_doc_desc", "DocumentDesc", 30, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colEntityName", "customer_name", "EntityName", 20, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colStatus", "inventory_doc_status", "DocumentStatusDesc", 10, HorizontalAlignment.Left);
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_no"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "DocumentNo", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "description"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "DocumentDesc", ""));

            if (docType == AccountDocumentType.AcctDocArReceipt)
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "include_invoice_no"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "IndexDocInclude", ""));

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "cheque_no"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ChequeNo", ""));
            }
            else if ((docType == AccountDocumentType.AcctDocCrNote) || (docType == AccountDocumentType.AcctDocDrNote))
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsInternalDrCr", "internal_doc"));                
            }
            else if (docType == AccountDocumentType.AcctDocBillSummary)
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "include_invoice_no"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "IndexDocInclude", ""));
            }
            else if ((docType == AccountDocumentType.AcctDocCashSale) || (docType == AccountDocumentType.AcctDocDebtSale))
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "sale_order"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "RefSaleOrderNo", ""));
            }

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "customer_name"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_status"));

            CCriteriaEntry statusEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "DocumentStatusObj", "");
            statusEntry.SetComboItemSources("DocumentStatuses", "Description");
            AddCriteriaControl(statusEntry);

            //===
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));
        }

        private ArrayList createAddContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "ADMIN_EDIT", new RoutedEventHandler(cmdAdd_Click), 1);
            contexts.Add(ct1);

            return (contexts);
        }

        private ArrayList createInvoiceContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAddWithPromotion", "pricing_by_promotion", new RoutedEventHandler(cmdAdd_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuAddWithOutPromotion", "pricing_by_manual", new RoutedEventHandler(cmdAdd_Click), 2);
            contexts.Add(ct2);

            CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuAddWithSaleOrder", "invoice_from_saleorder", new RoutedEventHandler(cmdAdd_Click), 3);
            contexts.Add(ct3);

            return (contexts);
        }

        private ArrayList createSaleOrderContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAddRegular", "so_by_manual", new RoutedEventHandler(cmdAdd_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuAddFromQuotation", "so_by_quotation", new RoutedEventHandler(cmdAdd_Click), 2);
            contexts.Add(ct2);

            return (contexts);
        }

        private static void printPreview(MAccountDoc doc, String id, AccountDocumentType dt)
        {
            WinFormPrinting wp = new WinFormPrinting(docTypeToReportGroup(dt), getAccountDocObject(id));
            wp.ShowDialog();
        }

        #endregion;

        #region Private

        private static MAccountDoc getAccountDocObject(String id)
        {
            //CUtil.EnableForm(false, ParentControl);

            CTable t = new CTable("");
            MAccountDoc md = new MAccountDoc(t);
            md.AccountDocId = id;

            CTable obj = OnixWebServiceAPI.GetAccountDocInfo(md.GetDbObject());
            md.SetDbObject(obj);

            md.InitAccountDocItem();
            md.InitAccountDocPayment();
            md.InitEntityAddresses();
            md.InitEntityAddresses();
            md.InitAccountDocReceipt();

            //CUtil.EnableForm(true, ParentControl);

            return (md);
        }

        public static void ShowEditWindowEx(AccountDocumentType docType, MAccountDoc v, GenericAccountDocCallback printCallBack)
        {
            int type = 1;

            String caption = "";
            if (docType == AccountDocumentType.AcctDocCashSale)
            {
                caption = CLanguage.getValue("sale_cash_saling");
                if (!CHelper.VerifyAccessRight("SALE_BYCASH_VIEW"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocDebtSale)
            {
                caption = CLanguage.getValue("sale_debt_saling");
                if (!CHelper.VerifyAccessRight("SALE_BYCREDIT_VIEW"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocDrNote)
            {
                type = 2;

                caption = CLanguage.getValue("sale_debit_note");
                if (!CHelper.VerifyAccessRight("SALE_DRNOTE_VIEW"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocCrNote)
            {
                type = 2;

                caption = CLanguage.getValue("sale_credit_note");
                if (!CHelper.VerifyAccessRight("SALE_CRNOTE_VIEW"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocArReceipt)
            {
                type = 3;
                caption = CLanguage.getValue("sale_ar_receipt");
                if (!CHelper.VerifyAccessRight("SALE_RECEIPT_VIEW"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocBillSummary)
            {
                type = 6;
                caption = CLanguage.getValue("bill_summary");
                if (!CHelper.VerifyAccessRight("SALE_BILLSUM_VIEW"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocMiscRevenue)
            {
                type = 4;
                caption = CLanguage.getValue("sale_misc");
                if (!CHelper.VerifyAccessRight("SALE_MISC_VIEW"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocCashDepositAr)
            {
                type = 5;
                caption = CLanguage.getValue("cash_deposit_ar");
                if (!CHelper.VerifyAccessRight("SALE_DEPOSIT_VIEW"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocSaleOrder)
            {
                type = 1;
                caption = CLanguage.getValue("sale_order");
                if (!CHelper.VerifyAccessRight("SALE_ORDER_VIEW"))
                {
                    return;
                }
            }

            if (type == 1)
            {
                WinAddEditAccountSaleDoc w = new WinAddEditAccountSaleDoc("E", docType, null, v, v.IsPromotionMode);
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();

                if (w.IsPreviewNeed)
                {
                    printCallBack?.Invoke(v, w.CreatedID, docType);
                }
            }
            else if (type == 2)
            {
                WinAddEditDrCrNote w = new WinAddEditDrCrNote("E", docType, null, v);
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();
            }
            else if (type == 3)
            {
                WinAddEditReceiptDoc w = new WinAddEditReceiptDoc("E", docType, null, v);
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();

                if (w.IsPreviewNeed)
                {
                    printCallBack?.Invoke(v, w.CreatedID, docType);
                }
            }
            else if (type == 4)
            {
                WinAddEditAccountMiscDoc w = new WinAddEditAccountMiscDoc("E", docType, null, v);
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();

                if (w.IsPreviewNeed)
                {
                    printCallBack?.Invoke(v, w.CreatedID, docType);
                }
            }
            else if (type == 5)
            {
                WinAddEditCashDeposit w = new WinAddEditCashDeposit("E", docType, null, v);
                //w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();

                if (w.IsPreviewNeed)
                {
                    printCallBack?.Invoke(v, w.CreatedID, docType);
                }
            }
            else if (type == 6)
            {
                WinAddEditBillSummary w = new WinAddEditBillSummary("E", docType, null, v);
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();

                //if (w.IsPreviewNeed)
                //{
                //    printCallBack?.Invoke(v, w.CreatedID, docType);
                //}
            }
        }

        #endregion

        public void SetDocumentType(AccountDocumentType dt)
        {
            docType = dt;
        }

        public override void Initialize(string keyword)
        {
            Hashtable maps = new Hashtable();
            maps["mnuCashSelling"] = AccountDocumentType.AcctDocCashSale;
            maps["mnuSaleOrder"] = AccountDocumentType.AcctDocSaleOrder;
            maps["mnuDebtSelling"] = AccountDocumentType.AcctDocDebtSale;
            maps["mnuDebitNOte"] = AccountDocumentType.AcctDocDrNote;
            maps["mnuCreditNote"] = AccountDocumentType.AcctDocCrNote;
            maps["mnuARReceipt"] = AccountDocumentType.AcctDocArReceipt;
            maps["mnuBillSummary"] = AccountDocumentType.AcctDocBillSummary;
            maps["mnuCashDepositAr"] = AccountDocumentType.AcctDocCashDepositAr;
            maps["mnuSaleMisc"] = AccountDocumentType.AcctDocMiscRevenue;

            if (maps.ContainsKey(keyword))
            {
                AccountDocumentType dt = (AccountDocumentType)maps[keyword];
                SetDocumentType(dt);
                Init("");
            }            
        }

        public override Button GetButton()
        {
            ArrayList menues = null;
            CCriteriaButton btn = null;

            if ((docType == AccountDocumentType.AcctDocCashSale) || (docType == AccountDocumentType.AcctDocDebtSale))
            {
                menues = createInvoiceContextMenu();
                btn = new CCriteriaButton("add", true, menues, cmdInvoiceAdd_Click);
            }
            else if (docType == AccountDocumentType.AcctDocSaleOrder)
            {
                menues = createSaleOrderContextMenu();
                btn = new CCriteriaButton("add", true, menues, cmdSaleOrderAdd_Click);
            }
            else
            {
                menues = createAddContextMenu();
                btn = new CCriteriaButton("add", true, menues, cmdAdd_Click);
            }
            
            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            (model as MAccountDoc).DocumentType = ((int) docType).ToString();

            items = OnixWebServiceAPI.GetAccountDocList(model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MAccountDoc v = new MAccountDoc(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }


        private String getAccessRightDelete()
        {
            String acr = "SALE_UNKNOW_DELETE";

            if (docType == AccountDocumentType.AcctDocCashSale)
            {
                acr = "SALE_BYCASH_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocDebtSale)
            {
                acr = "SALE_BYCREDIT_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocDrNote)
            {
                acr = "SALE_DRNOTE_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocCrNote)
            {
                acr = "SALE_CRNOTE_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocArReceipt)
            {
                acr = "SALE_RECEIPT_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocBillSummary)
            {
                acr = "SALE_BILLSUM_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocArReceipt)
            {
                acr = "SALE_RECEIPT_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocSaleOrder)
            {
                acr = "SALE_ORDER_DELETE";
            }

            return (acr);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight(getAccessRightDelete()))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteAccountDoc, rc.ToString());

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            ShowEditWindowEx(docType, (MAccountDoc)currentObj, printPreview);
        }

        #region Event Handler

        private static String docTypeToReportGroup(AccountDocumentType docType)
        {
            String rptGroup = "";

            if (docType == AccountDocumentType.AcctDocCashSale)
            {
                rptGroup = "grpSaleByCashInvoice";
            }
            else if (docType == AccountDocumentType.AcctDocDebtSale)
            {
                rptGroup = "grpSaleByDebtInvoice";
            }
            else if (docType == AccountDocumentType.AcctDocArReceipt)
            {
                rptGroup = "grpSaleArReceipt";
            }
            else if (docType == AccountDocumentType.AcctDocMiscRevenue)
            {
                rptGroup = "grpSaleByCashInvoice";
            }

            return (rptGroup);
        }

        private void cmdInvoiceAdd_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void cmdSaleOrderAdd_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private MAccountDoc constructSaleOrderFromQuotation(MAuxilaryDoc quotation)
        {
            CTable qt = OnixWebServiceAPI.GetAuxilaryDocInfo(quotation.GetDbObject());

            MAuxilaryDoc qd = new MAuxilaryDoc(qt);
            qd.InitAuxilaryDocItem();
            qd.InitEntityAddresses();

            MAccountDoc md = new MAccountDoc(new CTable(""));

            md.DocumentType = ((int) AccountDocumentType.AcctDocSaleOrder).ToString();
            md.DocumentDate = qd.DocumentDate;
            md.DocumentDesc = qd.DocumentDesc;
            md.DueDate = qd.DocumentDate;

            md.ProjectID = qd.ProjectID;
            md.ProjectCode = qd.ProjectCode;
            md.ProjectName = qd.ProjectName;
            md.ProjectGroupName = qd.ProjectGroupName;

            md.EntityId = qd.EntityId;
            md.EntityCode = qd.EntityCode;
            md.EntityName = qd.EntityName;
            md.EntityAddressID = qd.EntityAddressID;

            md.EmployeeID = qd.EmployeeID;
            md.EmployeeCode = qd.EmployeeCode;
            md.EmployeeName = qd.EmployeeName;
            
            CTable cust = OnixWebServiceAPI.GetEntityInfo(qd.EntityObj.GetDbObject());
            MEntity en = new MEntity(cust);
            en.InitEntityAddress();
            md.ReloadEntityAddresses(en.AddressItems);

            md.BranchId = qd.BranchId;
            md.VATType = qd.VatType;
            md.RefQuotationNo = qd.DocumentNo;
            md.RefQuotationID = qd.AuxilaryDocID;

            foreach (MAuxilaryDocItem ad in qd.AuxilaryDocItems)
            {
                MAccountDocItem adi = new MAccountDocItem(ad.GetDbObject());
                adi.ExtFlag = "A";
                md.AddAccountDocItem(adi);
            }

            md.CalculateExtraFields();
            md.IsModified = true;

            return (md);
        }

        private MAccountDoc constructInvoiceFromSaleOrder(MAccountDoc saleOrder)
        {
            CTable so = OnixWebServiceAPI.GetAccountDocInfo(saleOrder.GetDbObject());

            MAccountDoc sod = new MAccountDoc(so);
            sod.InitAccountDocItem();
            sod.InitEntityAddresses();

            MAccountDoc md = new MAccountDoc(new CTable(""));

            md.DocumentType = ((int) docType).ToString();
            md.DocumentDate = sod.DocumentDate;
            md.DocumentDesc = sod.DocumentDesc;
            md.DueDate = sod.DocumentDate;

            md.ProjectID = sod.ProjectID;
            md.ProjectCode = sod.ProjectCode;
            md.ProjectName = sod.ProjectName;
            md.ProjectGroupName = sod.ProjectGroupName;

            md.EntityId = sod.EntityId;
            md.EntityCode = sod.EntityCode;
            md.EntityName = sod.EntityName;
            md.EntityAddressID = sod.EntityAddressID;

            md.EmployeeID = sod.EmployeeID;
            md.EmployeeCode = sod.EmployeeCode;
            md.EmployeeName = sod.EmployeeName;

            CTable cust = OnixWebServiceAPI.GetEntityInfo(sod.EntityObj.GetDbObject());
            MEntity en = new MEntity(cust);
            en.InitEntityAddress();
            md.ReloadEntityAddresses(en.AddressItems);

            md.BranchId = sod.BranchId;
            md.VATType = sod.VATType;
            md.VAT_PCT = sod.VAT_PCT;
            md.RefSaleOrderNo = sod.DocumentNo;
            md.RefSaleOrderID = sod.AccountDocId;
            md.RefPoNo = sod.RefPoNo;
            md.RefQuotationNo = sod.RefQuotationNo;

            foreach (MAccountDocItem ad in sod.AccountItem)
            {
                MAccountDocItem adi = new MAccountDocItem(ad.GetDbObject());
                adi.ProjectID = sod.ProjectID;
                adi.ProjectCode = sod.ProjectCode;
                adi.ProjectName = sod.ProjectName;

                adi.ExtFlag = "A";
                md.AddAccountDocItem(adi);
            }

            md.CalculateExtraFields();
            md.IsModified = true;

            return (md);
        }

        public void PopulateExtraFields()
        {
            MAccountDoc m = (MAccountDoc) model;
            m.IsSaleOrderInUsedByInvoice = false;
            m.DocStatusSet = "(1, 2)";
        }

        private MAccountDoc createInvoiceFromSaleOrder()
        {
            MAccountDoc md = null;

            CCriteriaAccountDocSale cr = new CCriteriaAccountDocSale();
            cr.SetActionEnable(false);
            cr.SetDefaultData(new MAccountDoc(new CTable("")));
            cr.SetDocumentType(AccountDocumentType.AcctDocSaleOrder);
            cr.Init("");
            cr.PopulateExtraFields();

            WinLookupSearch2 w = new WinLookupSearch2(cr, CLanguage.getValue("sale_order"));
            w.ShowDialog();

            if (w.IsOK)
            {
                CUtil.EnableForm(false, ParentControl);
                md = constructInvoiceFromSaleOrder((MAccountDoc)w.ReturnedObj);
                CUtil.EnableForm(true, ParentControl);
            }

            return (md);
        }

        public override void SetCheckUncheckHandler(RoutedEventHandler chdler, RoutedEventHandler uhdler)
        {
            checkHandler = chdler;
            unCheckHandler = uhdler;
        }

        private void cbxSelect_Checked(object sender, RoutedEventArgs e)
        {
            if (checkHandler != null)
            {
                checkHandler(sender, e);
            }
        }

        private void cbxSelect_UnChecked(object sender, RoutedEventArgs e)
        {
            if (unCheckHandler != null)
            {
                unCheckHandler(sender, e);
            }
        }

        private MAccountDoc createSaleOrderFromQuotation()
        {
            MAccountDoc md = null;

            CCriteriaAuxilaryDocSale cr = new CCriteriaAuxilaryDocSale();
            cr.SetActionEnable(false);
            cr.SetDefaultData(new MAuxilaryDoc(new CTable("")));
            cr.Init(((int) AuxilaryDocumentType.AuxDocQuotation).ToString());
            cr.PopulateExtraFields();

            WinLookupSearch2 w = new WinLookupSearch2(cr, CLanguage.getValue("quotation"));
            w.ShowDialog();

            if (w.IsOK)
            {
                CUtil.EnableForm(false, ParentControl);
                md = constructSaleOrderFromQuotation((MAuxilaryDoc) w.ReturnedObj);
                CUtil.EnableForm(true, ParentControl);
            }

            return (md);
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            int type = 1;
            String caption = "";
            if (docType == AccountDocumentType.AcctDocCashSale)
            {
                caption = CLanguage.getValue("sale_cash_saling");
                if (!CHelper.VerifyAccessRight("SALE_BYCASH_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocDebtSale)
            {
                caption = CLanguage.getValue("sale_debt_saling");
                if (!CHelper.VerifyAccessRight("SALE_BYCREDIT_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocDrNote)
            {
                type = 2;
                caption = CLanguage.getValue("sale_debit_note");
                if (!CHelper.VerifyAccessRight("SALE_DRNOTE_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocCrNote)
            {
                type = 2;
                caption = CLanguage.getValue("sale_credit_note");
                if (!CHelper.VerifyAccessRight("SALE_CRNOTE_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocBillSummary)
            {
                type = 6;
                caption = CLanguage.getValue("bill_summary");
                if (!CHelper.VerifyAccessRight("SALE_BILLSUM_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocArReceipt)
            {
                type = 3;
                caption = CLanguage.getValue("sale_ar_receipt");
                if (!CHelper.VerifyAccessRight("SALE_RECEIPT_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocMiscRevenue)
            {
                type = 4;
                caption = CLanguage.getValue("sale_misc");
                if (!CHelper.VerifyAccessRight("SALE_MISC_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocCashDepositAr)
            {
                type = 5;
                caption = CLanguage.getValue("cash_deposit_ar");
                if (!CHelper.VerifyAccessRight("SALE_DEPOSIT_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocSaleOrder)
            {
                type = 1;
                caption = CLanguage.getValue("sale_order");
                if (!CHelper.VerifyAccessRight("SALE_ORDER_ADD"))
                {
                    return;
                }
            }

            if (type == 1)
            {
                MAccountDoc ad = null;

                Boolean isPromotion = false;
                String cpt = caption;

                if (sender is MenuItem)
                {
                    MenuItem mnu = (MenuItem)sender;

                    if (mnu.Name.Equals("mnuAddWithPromotion"))
                    {
                        isPromotion = true;
                        cpt = (String)mnu.Header + caption;
                    }
                    else if (mnu.Name.Equals("mnuAddFromQuotation"))
                    {
                        //Create from Quotation
                        ad = createSaleOrderFromQuotation();
                        if (ad == null)
                        {
                            return;
                        }

                        cpt = caption;
                    }
                    else if (mnu.Name.Equals("mnuAddWithSaleOrder"))
                    {
                        //Create from SaleOrder
                        ad = createInvoiceFromSaleOrder();
                        if (ad == null)
                        {
                            return;
                        }

                        cpt = caption;
                    }
                }

                WinAddEditAccountSaleDoc w = new WinAddEditAccountSaleDoc("A", docType, itemSources, ad, isPromotion);
                w.Caption = cpt;
                w.ShowDialog();
                w.ItemAddedHandler = ItemAddedEvent;
                if (w.IsPreviewNeed)
                {
                    WinFormPrinting wp = new WinFormPrinting(docTypeToReportGroup(docType), getAccountDocObject(w.CreatedID));
                    wp.ShowDialog();
                }
            }
            else if (type == 2)
            {
                WinAddEditDrCrNote w = new WinAddEditDrCrNote("A", docType, itemSources, null);
                w.Caption = (String)(sender as Button).Content + " " + caption;
                w.ShowDialog();
            }
            else if (type == 3)
            {
                WinAddEditReceiptDoc w = new WinAddEditReceiptDoc("A", docType, itemSources, (MAccountDoc)null);
                w.Caption = (String)(sender as Button).Content + " " + caption;
                w.ShowDialog();
                //w.ItemAddedHandler = ItemAddedEvent;
                if (w.IsPreviewNeed)
                {
                    WinFormPrinting wp = new WinFormPrinting(docTypeToReportGroup(docType), getAccountDocObject(w.CreatedID));
                    wp.ShowDialog();
                }
            }
            else if (type == 4)
            {
                WinAddEditAccountMiscDoc w = new WinAddEditAccountMiscDoc("A", docType, itemSources, (MAccountDoc)null);
                w.Caption = (String)(sender as Button).Content + " " + caption;
                w.ShowDialog();

                if (w.IsPreviewNeed)
                {
                    WinFormPrinting wp = new WinFormPrinting(docTypeToReportGroup(docType), getAccountDocObject(w.CreatedID));
                    wp.ShowDialog();
                }
            }
            else if (type == 5)
            {
                WinAddEditCashDeposit w = new WinAddEditCashDeposit("A", docType, itemSources, (MAccountDoc)null);
                //w.Caption = (String)(sender as Button).Content + " " + caption;
                w.ShowDialog();
            }
            else if (type == 6)
            {
                WinAddEditBillSummary w = new WinAddEditBillSummary("A", docType, itemSources, (MAccountDoc)null);
                w.Caption = (String)(sender as Button).Content + " " + caption;
                w.ShowDialog();
                //if (w.IsPreviewNeed)
                //{
                //    WinFormPrinting wp = new WinFormPrinting(docTypeToReportGroup(docType), getAccountDocObject(w.CreatedID));
                //    wp.ShowDialog();
                //}
            }
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            currentObj = (MBaseModel)(sender as UActionButton).Tag;
        }

        private void mnuVoidMenu_Click(object sender, RoutedEventArgs e)
        {
            String status = (currentObj as MAccountDoc).DocumentStatus;

            if (!status.Equals(((int) InventoryDocumentStatus.InvDocApproved).ToString()))
            {
                CHelper.ShowErorMessage("", "ERROR_DOC_STATUS", null);
                return;
            }

            Boolean result = CHelper.AskConfirmMessage("CONFIRM_VOID_DOCUMENT");
            if (!result)
            {
                return;
            }

            MAccountDoc md = (MAccountDoc) currentObj;
            WinAddEditVoidedDoc w = new WinAddEditVoidedDoc(md);
            w.Caption = md.DocumentNo;
            w.ShowDialog();
        }

        private void mnuContextMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuEdit"))
            {
                ShowEditWindowEx(docType, (MAccountDoc)currentObj, printPreview);
            }
            else if (name.Equals("mnuCopy") || name.Equals("mnuCopyHead"))
            {
                CTable t = currentObj.GetDbObject();
                CUtil.EnableForm(false, ParentControl);

                t.SetFieldValue("IS_ONLY_HEAD", "N");
                if (name.Equals("mnuCopyHead"))
                {
                    t.SetFieldValue("IS_ONLY_HEAD", "Y");
                }

                CTable newobj = OnixWebServiceAPI.CopyAccountDoc(t);

                if (newobj != null)
                {
                    MAccountDoc ivd = new MAccountDoc(newobj);
                    ItemAddedEvent(ivd, e);
                }
                else
                {
                    //Error here
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                }

                CUtil.EnableForm(true, ParentControl);
            }
            else if (name.Equals("mnuPrint"))
            {
                CUtil.EnableForm(false, ParentControl);

                WinFormPrinting wp = new WinFormPrinting(docTypeToReportGroup(docType), getAccountDocObject((currentObj as MAccountDoc).AccountDocId));
                wp.ShowDialog();

                CUtil.EnableForm(true, ParentControl);
            }
        }
#endregion
    }
}
