using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Model;
using Onix.Client.Controller;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.Criteria
{
    public class CCriteriaBillSummaryItem : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String eType = "1";

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaBillSummaryItem() : base(new MAccountDoc(new CTable("")), "CCriteriaBillSummaryItem")
        {

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

#region Criteria Configure

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();
            return (contexts);
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

        private void createGridColumns()
        {
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left, cbxSelect_Checked, cbxSelect_UnChecked);
            AddGridColumn(c0);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocumentDate", "inventory_doc_date", "DocumentDateFmt", 10, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDueDate", "due_date", "DueDateFmt", 13, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c2_1 = new CCriteriaColumnText("colDayOverDue", "day_overdue", "DayOverDue", 8, HorizontalAlignment.Left);
            AddGridColumn(c2_1);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colDocumentNo", "inventory_doc_no", "DocumentNo", 21, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colInvoiceAmount", "debt_amount", "ArApAmtFmt", 13, HorizontalAlignment.Right);
            AddGridColumn(c4);

            CCriteriaColumnText c6 = new CCriteriaColumnText("colNote", "note", "DocumentDesc", 30, HorizontalAlignment.Left);
            AddGridColumn(c6);
            c6.Sortable = false;
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsOverDue", "is_overdue"));
        }

        private String getKeyMap(String name)
        {
            Hashtable map = new Hashtable();
            map["code"] = 0;
            map["name"] = 1;

            String[] arr1 = { "customer_code", "customer_name" };
            String[] arr2 = { "supplier_code", "supplier_name" };

            String[] arr = arr1;
            if (eType.Equals("2"))
            {
                arr = arr2;
            }

            int idx = (int)map[name];
            String key = arr[idx];

            return (key);
        }

        private void createInfoEntries()
        {
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", getKeyMap("code")));
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityCode", ""));

            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", getKeyMap("name")));
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityName", ""));

            MAccountDoc m = (MAccountDoc) model;
            MAccountDoc d = (MAccountDoc) GetDefaultData();
            if (d != null)
            {
                m.EntityCode = d.EntityCode;
                m.EntityName = d.EntityName;
                m.EntityId = d.EntityId;
                m.ExcludeDocSet = d.ExcludeDocSet;
            }
        }

        private ArrayList createAddContextMenu()
        {
            ArrayList contexts = new ArrayList();
            return (contexts);
        }

        #endregion;

        #region Private

        private void showEditWindow()
        {
        }

        #endregion

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, null);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            MAccountDoc mp = model as MAccountDoc;
            mp.Category = eType;

            CTable tb = model.GetDbObject();

            String tmpSet = "(1, 2)"; //Only pending and approved, no cancled status
            tb.SetFieldValue("DOCUMENT_STATUS_SET", tmpSet);

            items = OnixWebServiceAPI.GetBillSummaryAbleDocList(tb);
            
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

        public override int DeleteData(int rc)
        {
            return (0);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }

        public override void Init(String type)
        {
            eType = type;
            createCriteriaEntries();
            createInfoEntries();
            createGridColumns();
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            currentObj = (MBaseModel)(sender as UActionButton).Tag;
        }

        private void mnuContextMenu_Click(object sender, RoutedEventArgs e)
        {
        }
#endregion
    }
}
