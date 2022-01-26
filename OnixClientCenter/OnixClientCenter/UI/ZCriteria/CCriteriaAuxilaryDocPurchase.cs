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
    public class CCriteriaAuxilaryDocPurchase : CCriteriaBase
    {
        private AuxilaryDocumentType docType = AuxilaryDocumentType.AuxDocPO;
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        public CCriteriaAuxilaryDocPurchase() : base(new MAuxilaryDoc(new CTable("")), "CCriteriaAuxilaryDocPurchase")
        {
            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
        }

        public void SetDocumentType(AuxilaryDocumentType dt)
        {
            docType = dt;
        }

        public override void Initialize(string keyword)
        {
            SetDocumentType(AuxilaryDocumentType.AuxDocPO);
        }

        #region Criteria Configure

        private void loadRelatedReferences()
        {

        }

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuCopy", "copy", new RoutedEventHandler(mnuContextMenu_Click), 2);
            contexts.Add(ct2);

            CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuPrint", "print", new RoutedEventHandler(mnuContextMenu_Click), 3);
            contexts.Add(ct3);

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

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocNo", "inventory_doc_no", "DocumentNo", 15, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDocDate", "inventory_doc_date", "DocumentDateFmt", 15, HorizontalAlignment.Left);
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

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "item_info"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "IndexItems", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "supplier_name"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "project_code"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ProjectCode", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "account_doc_status"));

            CCriteriaEntry statusEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "DocumentStatusObj", "");
            statusEntry.SetComboItemSources("PoStatuses", "Description");
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

        #endregion;

        #region Private

        private MAuxilaryDoc getPurchaseOrderObject(String id)
        {
            CUtil.EnableForm(false, ParentControl);

            CTable t = new CTable("");
            MAuxilaryDoc md = new MAuxilaryDoc(t);
            md.AuxilaryDocID = id;

            CTable obj = OnixWebServiceAPI.GetAuxilaryDocInfo(md.GetDbObject());
            md.SetDbObject(obj);

            md.InitAuxilaryDocItem();
            md.InitPaymentCriteria();
            md.InitEntityAddresses();

            String tmp = md.EntityBankAccountID;
            md.InitEntityBankAccounts();
            md.EntityBankAccountID = tmp;

            CUtil.EnableForm(true, ParentControl);

            return (md);
        }

        private void showEditWindow()
        {
            String caption = CLanguage.getValue("purchase_po");
            if (!CHelper.VerifyAccessRight("PURCHASE_PO_VIEW"))
            {
                return;
            }

            MAuxilaryDoc v = (MAuxilaryDoc) currentObj;

            WinAddEditPurchaseOrder w = new WinAddEditPurchaseOrder("E", itemSources, v, docType);
            w.Caption = CLanguage.getValue("edit") + " " + caption;
            w.ShowDialog();

            if (w.IsPreviewNeed)
            {
                WinFormPrinting wp = new WinFormPrinting("grpPurchasePO", getPurchaseOrderObject(w.CreatedID));
                wp.ShowDialog();
            }
        }

        #endregion

        public void SetDocumentType(AccountDocumentType dt)
        {
        }

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, cmdAdd_Click);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            (model as MAuxilaryDoc).DocumentType = ((int) docType).ToString();            

            items = OnixWebServiceAPI.GetAuxilaryDocList(model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            //int idx = 0;
            foreach (CTable o in items)
            {
                MAuxilaryDoc v = new MAuxilaryDoc(o);

                //v.RowIndex = idx;
                itemSources.Add(v);
                //idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("PURCHASE_PO_DELETE"))
            {
                //Need to check for access right per document type
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteAuxilaryDoc, rc.ToString());

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            String caption = CLanguage.getValue("purchase_po");
            if (!CHelper.VerifyAccessRight("PURCHASE_PO_ADD"))
            {
                return;
            }

            WinAddEditPurchaseOrder w = new WinAddEditPurchaseOrder("A", itemSources, null, docType);
            w.Caption = (String)(sender as Button).Content + " " + caption;
            w.ShowDialog();
            if (w.IsPreviewNeed)
            {
                WinFormPrinting wp = new WinFormPrinting("grpPurchasePO", getPurchaseOrderObject(w.CreatedID));
                wp.ShowDialog();
            }
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            currentObj = (MBaseModel)(sender as UActionButton).Tag;
        }

        private void mnuContextMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuEdit"))
            {
                showEditWindow();
            }
            else if (name.Equals("mnuCopy"))
            {
                CUtil.EnableForm(false, ParentControl);
                CTable newobj = OnixWebServiceAPI.CopyAuxilaryDoc(currentObj.GetDbObject());

                if (newobj != null)
                {
                    MAuxilaryDoc ivd = new MAuxilaryDoc(newobj);
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

                WinFormPrinting wp = new WinFormPrinting("grpPurchasePO", getPurchaseOrderObject((currentObj as MAuxilaryDoc).AuxilaryDocID));
                wp.ShowDialog();

                CUtil.EnableForm(true, ParentControl);
            }
        }
#endregion
    }
}
