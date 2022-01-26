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
    public class CCriteriaAuxilaryDocSale : CCriteriaBase
    {
        private AuxilaryDocumentType docType = AuxilaryDocumentType.AuxDocQuotation;
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaAuxilaryDocSale() : base(new MAuxilaryDoc(new CTable("")), "CCriteriaAuxilaryDocSale")
        {

        }

        public override void Init(String type)
        {
            docType = (AuxilaryDocumentType) int.Parse(type);

            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
        }

        public override void Initialize(string keyword)
        {
            Init(((int)AuxilaryDocumentType.AuxDocQuotation).ToString());
        }

        public override void SetActionEnable(Boolean en)
        {
            isActionEnable = en;
        }

        public override void SetCheckUncheckHandler(RoutedEventHandler chdler, RoutedEventHandler uhdler)
        {
            checkHandler = chdler;
            unCheckHandler = uhdler;
        }

        public void SetDocumentType(AuxilaryDocumentType dt)
        {
            docType = dt;
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

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuCopy", "copy_head", new RoutedEventHandler(mnuContextMenu_Click), 2);
            contexts.Add(ct2);

            CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuPrint", "print", new RoutedEventHandler(mnuContextMenu_Click), 3);
            contexts.Add(ct3);

            return (contexts);
        }

        private void createGridColumns()
        {
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left, cbxSelect_Checked, cbxSelect_UnChecked);
            c0.RegisterCheckboxBindingVariable(CheckBox.IsEnabledProperty, "IsEditable");
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            c0_1.SetButtonEnable(isActionEnable);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocNo", "inventory_doc_no", "DocumentNo", 15, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDocDate", "inventory_doc_date", "DocumentDateFmt", 15, HorizontalAlignment.Left);
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

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "customer_name"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "quotation_type"));

            CCriteriaEntry qTypeEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "QuotationTypeObj", "");
            qTypeEntry.SetComboItemSources("QuotationTypes", "Description");
            AddCriteriaControl(qTypeEntry);

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

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuSummary", "quotation_by_summary", new RoutedEventHandler(cmdAddMenu_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuDetail", "quotation_by_detail", new RoutedEventHandler(cmdAddMenu_Click), 2);
            contexts.Add(ct2);

            return (contexts);
        }

        #endregion;

        #region Private

        private MAuxilaryDoc getDocumentObject(String id)
        {
            CUtil.EnableForm(false, ParentControl);

            CTable t = new CTable("");
            MAuxilaryDoc md = new MAuxilaryDoc(t);
            md.AuxilaryDocID = id;

            CTable obj = OnixWebServiceAPI.GetAuxilaryDocInfo(md.GetDbObject());
            md.SetDbObject(obj);

            md.InitAuxilaryDocItem();
            md.InitEntityAddresses();

            CUtil.EnableForm(true, ParentControl);

            return (md);
        }

        private void showEditWindow()
        {
            String caption = CLanguage.getValue("sale_quotation");
            if (!CHelper.VerifyAccessRight("SALE_QUOTATION_VIEW"))
            {
                return;
            }

            MAuxilaryDoc v = (MAuxilaryDoc) currentObj;

            WinAddEditQuotation w = new WinAddEditQuotation("E", itemSources, v, docType, "2");
            w.Caption = CLanguage.getValue("edit") + " " + caption;
            w.ShowDialog();

            if (w.IsPreviewNeed)
            {
                WinFormPrinting wp = new WinFormPrinting("grpSaleQuotation", getDocumentObject(w.CreatedID));
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

        public void PopulateExtraFields()
        {
            (model as MAuxilaryDoc).IsInUsedBySaleOrder = false;
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
            if (!CHelper.VerifyAccessRight("SALE_QUOTATION_DELETE"))
            {
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

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void cmdAddMenu_Click(object sender, RoutedEventArgs e)
        {
            String name = (sender as MenuItem).Name;

            String qtype = "1";
            if (name.Equals("mnuDetail"))
            {
                qtype = "2";
            }

            String caption = (sender as MenuItem).Header.ToString();
            if (!CHelper.VerifyAccessRight("SALE_QUOTATION_ADD"))
            {
                return;
            }

            WinAddEditQuotation w = new WinAddEditQuotation("A", itemSources, null, docType, qtype);
            w.Caption = CLanguage.getValue("add") + " " + caption;
            w.ShowDialog();
            if (w.IsPreviewNeed)
            {
                WinFormPrinting wp = new WinFormPrinting("grpPurchasePO", getDocumentObject(w.CreatedID));
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
                CTable t = currentObj.GetDbObject();
                //t.SetFieldValue("IS_ONLY_HEAD", "Y");
                CUtil.EnableForm(false, ParentControl);
                CTable newobj = OnixWebServiceAPI.CopyAuxilaryDoc(t);

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

                WinFormPrinting wp = new WinFormPrinting("grpSaleQuotation", getDocumentObject((currentObj as MAuxilaryDoc).AuxilaryDocID));
                wp.ShowDialog();

                CUtil.EnableForm(true, ParentControl);
            }
        }
#endregion
    }
}
