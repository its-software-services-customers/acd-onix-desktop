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
    public class CCriteriaSalePurchaseHistory : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String eType = "1";

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaSalePurchaseHistory() : base(new MSalePurchaseHistory(new CTable("")), "CCriteriaSalePurchaseHistory")
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

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left);
            c0.RegisterCheckboxBindingVariable(CheckBox.IsEnabledProperty, "IsEnabled");
            AddGridColumn(c0);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocNo", "inventory_doc_no", "DocumentNo", 15, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDocDate", "inventory_doc_date", "DocumentDateFmt", 10, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colItemCode", getSelectKeyMap("pcode"), getSelectKeyMap("pfcode"), 15, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colItemName", getSelectKeyMap("pname"), getSelectKeyMap("pfname"), 25, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colQuantity", "quantity", "QuantityFmt", 10, HorizontalAlignment.Right);            
            AddGridColumn(c5);
            c5.Sortable = false;

            CCriteriaColumnText c6 = new CCriteriaColumnText("colAmount", "amount", "AmountFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c6);
            c6.Sortable = false;

            CCriteriaColumnText c7 = new CCriteriaColumnText("colUnitPrice", "unit_price", "UnitPriceFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c7);
            c7.Sortable = false;
        }

        private String getCategoryKeyMap(String name)
        {
            MSalePurchaseHistory d = (MSalePurchaseHistory)GetDefaultData();
            String category = d.Category;

            Hashtable map = new Hashtable();
            map["ecode"] = 0;
            map["ename"] = 1;

            String[] arr1 = { "customer_code", "customer_name" };
            String[] arr2 = { "supplier_code", "supplier_name" };

            String[] arr = arr1;
            if (category.Equals("2"))
            {
                arr = arr2;
            }

            int idx = (int)map[name];
            String key = arr[idx];

            return (key);
        }

        private String getSelectKeyMap(String name)
        {
            MSalePurchaseHistory d = (MSalePurchaseHistory)GetDefaultData();
            String selectionType = d.SelectionType;

            Hashtable map = new Hashtable();
            map["pcode"] = 0;
            map["pname"] = 1;
            map["pfcode"] = 2;
            map["pfname"] = 3;

            String[] arr1 = { "service_code", "service_name", "ServiceCode", "ServiceName" };
            String[] arr2 = { "item_code", "item_name_thai", "ItemCode", "ItemNameThai" };            

            String[] arr = arr1;
            if (selectionType.Equals("2"))
            {
                arr = arr2;
            }

            int idx = (int)map[name];
            String key = arr[idx];

            return (key);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", getCategoryKeyMap("ecode")));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityCode", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", getCategoryKeyMap("ename")));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", getSelectKeyMap("pcode")));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, getSelectKeyMap("pfcode"), ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", getSelectKeyMap("pname")));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, getSelectKeyMap("pfname"), ""));

            MSalePurchaseHistory m = (MSalePurchaseHistory) model;
            MSalePurchaseHistory d = (MSalePurchaseHistory) GetDefaultData();
            if (d != null)
            {
                m.ServiceCode = d.ServiceCode;
                m.ItemCode = d.ItemCode;
                m.Category = d.Category;
                m.SelectionType = d.SelectionType;
            }
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
            //MSalePurchaseHistory mp = model as MSalePurchaseHistory;
            //mp.Category = eType;

            CTable tb = model.GetDbObject();            

            items = OnixWebServiceAPI.GetSalePurchaseHistoryList(tb);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MSalePurchaseHistory v = new MSalePurchaseHistory(o);

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
