using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Model;
using Onix.Client.Controller;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.General.Entity
{
    public class CCriteriaArApMovementHistory : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String eType = "1";

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaArApMovementHistory() : base(new MEntity(new CTable("")), "CCriteriaArApMovementHistory")
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

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDocDate", "inventory_doc_date", "DocumentDateMovementFmt", 10, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colDueDate", "due_date", "DueDateFmt", 11, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocNo", "inventory_doc_no", "DocumentMovementNo", 19, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colInAmount", "in_quantity", "InAmountFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colOutAmount", "out_quantity", "OutAmountFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c5);

            CCriteriaColumnText c6 = new CCriteriaColumnText("colLeftAmount", "balance_quantity", "EndAmountFmt", 15, HorizontalAlignment.Right);            
            AddGridColumn(c6);
            c6.Sortable = false;

            CCriteriaColumnText c7 = new CCriteriaColumnText("colNote", "note", "NoteTX", 20, HorizontalAlignment.Left);
            AddGridColumn(c7);
            c6.Sortable = false;
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));
        }

        private String getKeyMap(String name)
        {
            Hashtable map = new Hashtable();
            map["code"] = 0;
            map["name"] = 1;
            map["type"] = 2;
            map["group"] = 3;
            map["group"] = 3;
            map["type_array"] = 4;
            map["group_array"] = 5;

            String[] arr1 = { "customer_code", "customer_name", "customer_type", "customer_group", "CustomerTypes", "CustomerGroups" };
            String[] arr2 = { "supplier_code", "supplier_name", "supplier_type", "supplier_group", "SupplierTypes", "SupplierGroups" };

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

            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "credit_limit"));
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "CreditLimitFmt", ""));

            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "ar_ap_balance"));
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ArApBalanceFmt", ""));

            MEntity m = (MEntity) model;
            MEntity d = (MEntity) GetDefaultData();
            if (d != null)
            {
                m.EntityCode = d.EntityCode;
                m.EntityName = d.EntityName;
                m.EntityID = d.EntityID;
                m.CreditLimit = d.CreditLimit;
                m.ArApBalance = d.ArApBalance;
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
            CTable tb = model.GetDbObject();
            tb.SetFieldValue("CATEGORY", eType);

            items = OnixWebServiceAPI.GetArApTransactionList(tb);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MArApTransaction v = new MArApTransaction(o);
                v.IsEnabled = false;

                if (idx == 0)
                {
                    //Create one for balance forward
                    MArApTransaction bfw = new MArApTransaction(new CTable(""));
                    bfw.IsEnabled = false;

                    bfw.IsBalanceForward = true;
                    bfw.DocumentMovementNo = CLanguage.getValue("balance_forward");
                    bfw.DocumentDateMovement = v.DocumentDateMovement;
                    bfw.EndAmount = v.BeginAmount;
                    itemSources.Add(bfw);
                }

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
