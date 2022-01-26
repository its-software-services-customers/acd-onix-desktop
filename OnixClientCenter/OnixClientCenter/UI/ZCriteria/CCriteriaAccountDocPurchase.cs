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
    public class CCriteriaAccountDocPurchase : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        private AccountDocumentType docType = AccountDocumentType.AcctDocCashSale;

        public CCriteriaAccountDocPurchase() : base(new MAccountDoc(new CTable("")), "CCriteriaAccountDocPurchase")
        {
        }

        public override void Init(String type)
        {
            (model as MAccountDoc).IsInternalDrCr = false;

            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
        }

        public override void Initialize(string keyword)
        {
            Hashtable maps = new Hashtable();
            maps["mnuDebitNOteP"] = AccountDocumentType.AcctDocDrNotePurchase;
            maps["mnuCreditNoteP"] = AccountDocumentType.AcctDocCrNotePurchase;
            maps["mnuAPReceipt"] = AccountDocumentType.AcctDocApReceipt;
            maps["mnuCashDepositAp"] = AccountDocumentType.AcctDocCashDepositAp;
            maps["mnuPurchaseByCash"] = AccountDocumentType.AcctDocCashPurchase;
            maps["mnuPurchaseByDebt"] = AccountDocumentType.AcctDocDebtPurchase;
            maps["mnuPurchaseMisc"] = AccountDocumentType.AcctDocMiscExpense;

            if (maps.ContainsKey(keyword))
            {
                AccountDocumentType dt = (AccountDocumentType)maps[keyword];
                SetDocumentType(dt);
                Init("");
            }
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

            if ((docType == AccountDocumentType.AcctDocCashPurchase) ||
                (docType == AccountDocumentType.AcctDocDebtPurchase) ||
                (docType == AccountDocumentType.AcctDocApReceipt) ||
                (docType == AccountDocumentType.AcctDocMiscExpense))
            {
                CCriteriaContextMenu ct4 = new CCriteriaContextMenu("mnuCancel", "void_document", new RoutedEventHandler(mnuVoidMenu_Click), 4);
                contexts.Add(ct4);

                CCriteriaContextMenu ct5 = new CCriteriaContextMenu("mnuAdjust", "document_adjust", new RoutedEventHandler(mnuAdjustMenu_Click), 5);
                contexts.Add(ct5);
            }
            else if ((docType == AccountDocumentType.AcctDocCrNotePurchase) || (docType == AccountDocumentType.AcctDocDrNotePurchase))
            {
                CCriteriaContextMenu ct5 = new CCriteriaContextMenu("mnuAdjust", "document_adjust", new RoutedEventHandler(mnuAdjustMenu_Click), 5);
                contexts.Add(ct5);
            }

            return (contexts);
        }

        private void createGridColumns()
        {
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left);
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

            CCriteriaColumnText c4 = new CCriteriaColumnText("colEntityName", "supplier_name", "EntityName", 20, HorizontalAlignment.Left);
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

            if (docType == AccountDocumentType.AcctDocApReceipt)
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "include_invoice_no"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "IndexDocInclude", ""));

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "cheque_no"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ChequeNo", ""));
            }
            else if (docType == AccountDocumentType.AcctDocCashDepositAp)
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "cheque_no"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ChequeNo", ""));
            }
            else if ((docType == AccountDocumentType.AcctDocDebtPurchase) || (docType == AccountDocumentType.AcctDocCashPurchase))
            {
                //AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsInvoiceAvailable", "is_invoice_available"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "invoice_no"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "RefDocNo", ""));

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "po_no"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "RefPoNo", ""));

                if (docType == AccountDocumentType.AcctDocCashPurchase)
                {
                    AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "cheque_no"));
                    AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ChequeNo", ""));
                }
            }
            else if (docType == AccountDocumentType.AcctDocMiscExpense)
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "invoice_no"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "RefDocNo", ""));
            }
            else if ((docType == AccountDocumentType.AcctDocCrNotePurchase) || (docType == AccountDocumentType.AcctDocDrNotePurchase))
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsInternalDrCr", "internal_doc"));
            }

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "supplier_name"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "account_doc_status"));
            CCriteriaEntry statusEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "DocumentStatusObj", "");
            statusEntry.SetComboItemSources("DocumentStatuses", "Description");
            AddCriteriaControl(statusEntry);

            //====
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

        #endregion;

        #region Private

        private static MAccountDoc getAccountDocObject(String id)
        {
            //CUtil.EnableForm(false, parent);

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

            //CUtil.EnableForm(true, parent);

            return (md);
        }

        private static void printPreview(MAccountDoc doc, String id, AccountDocumentType dt)
        {
            WinFormPrinting wp = new WinFormPrinting(docTypeToReportGroup(dt), getAccountDocObject(id));
            wp.ShowDialog();
        }

        public static void ShowEditWindowEx(AccountDocumentType dt, MAccountDoc actDoc, GenericAccountDocCallback printCallBack)
        {
            int type = 1;

            String caption = "";
            if (dt == AccountDocumentType.AcctDocCashPurchase)
            {
                caption = CLanguage.getValue("purchase_cash");
                if (!CHelper.VerifyAccessRight("PURCHASE_BYCASH_VIEW"))
                {
                    return;
                }
            }
            else if (dt == AccountDocumentType.AcctDocDebtPurchase)
            {
                caption = CLanguage.getValue("purchase_debt");
                if (!CHelper.VerifyAccessRight("PURCHASE_BYCREDIT_VIEW"))
                {
                    return;
                }
            }
            else if (dt == AccountDocumentType.AcctDocDrNotePurchase)
            {
                type = 2;

                caption = CLanguage.getValue("purchase_debit_note");
                if (!CHelper.VerifyAccessRight("PURCHASE_DRNOTE_VIEW"))
                {
                    return;
                }
            }
            else if (dt == AccountDocumentType.AcctDocCrNotePurchase)
            {
                type = 2;

                caption = CLanguage.getValue("purchase_credit_note");
                if (!CHelper.VerifyAccessRight("PURCHASE_CRNOTE_VIEW"))
                {
                    return;
                }
            }
            else if (dt == AccountDocumentType.AcctDocApReceipt)
            {
                type = 3;
                caption = CLanguage.getValue("purchase_ap_receipt");
                if (!CHelper.VerifyAccessRight("PURCHASE_RECEIPT_VIEW"))
                {
                    return;
                }
            }
            else if (dt == AccountDocumentType.AcctDocMiscExpense)
            {
                type = 4;
                caption = CLanguage.getValue("purchase_misc");
                if (!CHelper.VerifyAccessRight("PURCHASE_MISC_VIEW"))
                {
                    return;
                }
            }

            MAccountDoc v = (MAccountDoc)actDoc;

            if (type == 1)
            {
                WinAddEditAccountPurchaseDoc w = new WinAddEditAccountPurchaseDoc("E", dt, null, v);
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();

                if (w.IsPreviewNeed)
                {
                    printCallBack?.Invoke(v, w.CreatedID, dt);
                }
            }
            else if (type == 2)
            {
                WinAddEditDrCrNote w = new WinAddEditDrCrNote("E", dt, null, v);
                w.Caption = CLanguage.getValue("edit") + caption;
                w.ShowDialog();
            }
            else if (type == 3)
            {
                WinAddEditReceiptDoc w = new WinAddEditReceiptDoc("E", dt, null, v);
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();
                if (w.IsPreviewNeed)
                {
                    printCallBack?.Invoke(v, w.CreatedID, dt);
                }
            }
            else if (type == 4)
            {
                WinAddEditAccountMiscDoc w = new WinAddEditAccountMiscDoc("E", dt, null, v);
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();

                if (w.IsPreviewNeed)
                {
                    printCallBack?.Invoke(v, w.CreatedID, dt);
                }
            }
        }

        public static void ShowEditApprovedWindowEx(AccountDocumentType dt, MAccountDoc actDoc, GenericAccountDocCallback printCallBack)
        {
            int type = 1;

            String caption = "";
            if (dt == AccountDocumentType.AcctDocCashPurchase)
            {
                caption = CLanguage.getValue("purchase_cash");
                if (!CHelper.VerifyAccessRight("PURCHASE_BYCASH_VIEW"))
                {
                    return;
                }
            }
            else if (dt == AccountDocumentType.AcctDocDebtPurchase)
            {
                caption = CLanguage.getValue("purchase_debt");
                if (!CHelper.VerifyAccessRight("PURCHASE_BYCREDIT_VIEW"))
                {
                    return;
                }
            }
            else if (dt == AccountDocumentType.AcctDocDrNotePurchase)
            {
                type = 2;

                caption = CLanguage.getValue("purchase_debit_note");
                if (!CHelper.VerifyAccessRight("PURCHASE_DRNOTE_VIEW"))
                {
                    return;
                }
            }
            else if (dt == AccountDocumentType.AcctDocCrNotePurchase)
            {
                type = 2;

                caption = CLanguage.getValue("purchase_credit_note");
                if (!CHelper.VerifyAccessRight("PURCHASE_CRNOTE_VIEW"))
                {
                    return;
                }
            }
            else if (dt == AccountDocumentType.AcctDocApReceipt)
            {
                caption = CLanguage.getValue("purchase_ap_receipt");
                if (!CHelper.VerifyAccessRight("PURCHASE_RECEIPT_VIEW"))
                {
                    return;
                }
            }
            else if (dt == AccountDocumentType.AcctDocMiscExpense)
            {
                caption = CLanguage.getValue("purchase_misc");
                if (!CHelper.VerifyAccessRight("PURCHASE_MISC_VIEW"))
                {
                    return;
                }
            }

            MAccountDoc v = (MAccountDoc)actDoc;

            if (type == 1)
            {
                WinAddEditAccountPurchaseDocApproved w = new WinAddEditAccountPurchaseDocApproved("E", dt, null, v);
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();
            }
            else if (type == 2)
            {
                WinAddEditDrCrNoteApproved w = new WinAddEditDrCrNoteApproved("E", dt, null, v);
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.ShowDialog();
            }
        }

        #endregion

        public void SetDocumentType(AccountDocumentType dt)
        {
            docType = dt;
        }

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, cmdAdd_Click);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            MAccountDoc ad = (model as MAccountDoc);
            String tempRefPoNo = ad.RefPoNo;
            String tempIdxRef = ad.IndexDocInclude;

            ad.DocumentType = ((int) docType).ToString();
            if (!ad.RefPoNo.Equals(""))
            {
                ad.RefPoNo = "%" + ad.RefPoNo;
            }

            if (!ad.IndexDocInclude.Equals(""))
            {
                ad.IndexDocInclude = "%" + ad.IndexDocInclude;
            }

            CTable t = model.GetDbObject();
            if ((docType == AccountDocumentType.AcctDocCrNotePurchase) || (docType == AccountDocumentType.AcctDocDrNotePurchase))
            {
                t.SetFieldValue("BY_VOID_FLAG", "N");
            }

            items = OnixWebServiceAPI.GetAccountDocList(t);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            ad.RefPoNo = tempRefPoNo;
            ad.IndexDocInclude = tempIdxRef;

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
            String acr = "PURCHASE_UNKNOW_DELETE";

            if (docType == AccountDocumentType.AcctDocCashPurchase)
            {
                acr = "PURCHASE_BYCASH_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocDebtPurchase)
            {
                acr = "PURCHASE_BYCREDIT_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocApReceipt)
            {
                acr = "PURCHASE_RECEIPT_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocMiscExpense)
            {
                acr = "PURCHASE_MISC_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocDrNotePurchase)
            {
                acr = "PURCHASE_DRNOTE_DELETE";
            }
            else if (docType == AccountDocumentType.AcctDocCrNotePurchase)
            {
                acr = "PURCHASE_CRNOTE_DELETE";
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
            ShowEditWindowEx(docType, (MAccountDoc) currentObj, printPreview);
        }

        #region Event Handler

        private static String docTypeToReportGroup(AccountDocumentType dt)
        {
            String rptGroup = "";

            if (dt == AccountDocumentType.AcctDocCashPurchase)
            {
                rptGroup = "grpPurchaseByCashInvoice";
            }
            else if (dt == AccountDocumentType.AcctDocDebtPurchase)
            {
                rptGroup = "grpPurchaseByDebtInvoice";
            }
            else if (dt == AccountDocumentType.AcctDocApReceipt)
            {
                rptGroup = "grpPurchaseApReceipt";
            }
            else if (dt == AccountDocumentType.AcctDocMiscExpense)
            {
                rptGroup = "grpPurchaseByCashInvoice";
            }

            return (rptGroup);
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            int type = 1;
            String caption = "";
            if (docType == AccountDocumentType.AcctDocCashPurchase)
            {
                caption = CLanguage.getValue("purchase_cash");
                if (!CHelper.VerifyAccessRight("PURCHASE_BYCASH_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocDebtPurchase)
            {
                caption = CLanguage.getValue("purchase_debt");
                if (!CHelper.VerifyAccessRight("PURCHASE_BYCREDIT_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocDrNotePurchase)
            {
                type = 2;
                caption = CLanguage.getValue("purchase_debit_note");
                if (!CHelper.VerifyAccessRight("PURCHASE_DRNOTE_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocCrNotePurchase)
            {
                type = 2;
                caption = CLanguage.getValue("purchase_credit_note");
                if (!CHelper.VerifyAccessRight("PURCHASE_CRNOTE_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocApReceipt)
            {
                type = 3;
                caption = CLanguage.getValue("purchase_ap_receipt");
                if (!CHelper.VerifyAccessRight("PURCHASE_RECEIPT_ADD"))
                {
                    return;
                }
            }
            else if (docType == AccountDocumentType.AcctDocMiscExpense)
            {
                type = 4;
                caption = CLanguage.getValue("purchase_misc");
                if (!CHelper.VerifyAccessRight("PURCHASE_MISC_ADD"))
                {
                    return;
                }
            }


            if (type == 1)
            {
                WinAddEditAccountPurchaseDoc w = new WinAddEditAccountPurchaseDoc("A", docType, itemSources, (MAccountDoc)null);
                w.Caption = (String)(sender as Button).Content + " " + caption;
                w.ShowDialog();
                w.ItemAddedHandler = ItemAddedEvent;
                if (w.IsPreviewNeed)
                {
                    printPreview(null, w.CreatedID, docType);
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

                if (w.IsPreviewNeed)
                {
                    printPreview(null, w.CreatedID, docType);
                }
            }
            else if (type == 4)
            {
                WinAddEditAccountMiscDoc w = new WinAddEditAccountMiscDoc("A", docType, itemSources, (MAccountDoc)null);
                w.Caption = (String)(sender as Button).Content + " " + caption;
                w.ShowDialog();
                
                if (w.IsPreviewNeed)
                {
                    printPreview(null, w.CreatedID, docType);
                }
            }
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            currentObj = (MBaseModel)(sender as UActionButton).Tag;
        }

        private void mnuAdjustMenu_Click(object sender, RoutedEventArgs e)
        {
            String status = (currentObj as MAccountDoc).DocumentStatus;

            if (!status.Equals(((int)InventoryDocumentStatus.InvDocApproved).ToString()))
            {
                CHelper.ShowErorMessage("", "ERROR_DOC_ADJUST_STATUS", null);
                return;
            }

            ShowEditApprovedWindowEx(docType, (MAccountDoc)currentObj, printPreview);
        }

        private void mnuVoidMenu_Click(object sender, RoutedEventArgs e)
        {
            String status = (currentObj as MAccountDoc).DocumentStatus;

            if (!status.Equals(((int)InventoryDocumentStatus.InvDocApproved).ToString()))
            {
                CHelper.ShowErorMessage("", "ERROR_DOC_STATUS", null);
                return;
            }

            Boolean result = CHelper.AskConfirmMessage("CONFIRM_VOID_DOCUMENT");
            if (!result)
            {
                return;
            }

            MAccountDoc md = (MAccountDoc)currentObj;
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
                ShowEditWindowEx(docType, (MAccountDoc) currentObj, printPreview);
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
