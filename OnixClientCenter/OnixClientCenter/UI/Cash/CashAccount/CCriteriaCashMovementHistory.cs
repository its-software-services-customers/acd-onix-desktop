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

namespace Onix.ClientCenter.UI.Cash.CashAccount
{
    public class CCriteriaCashMovementHistory : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String eType = "1";

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaCashMovementHistory() : base(new MCashDoc(new CTable("")), "CCriteriaCashMovementHistory")
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

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocNo", "inventory_doc_no", "DocumentNo", 19, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDocDate", "inventory_doc_date", "DocumentDateFmt", 11, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colInAmount", "in_quantity", "InAmountFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colOutAmount", "out_quantity", "OutAmountFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colLeftAmount", "left_amount_avg", "EndAmountFmt", 15, HorizontalAlignment.Right);            
            AddGridColumn(c5);
            c5.Sortable = false;

            CCriteriaColumnText c6 = new CCriteriaColumnText("colNote", "note", "NoteTX", 30, HorizontalAlignment.Left);
            AddGridColumn(c6);
            c6.Sortable = false;
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));
        }

        private void createInfoEntries()
        {
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "AccNo"));
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "AccountNo", ""));

            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "AccName"));
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "AccountName", ""));

            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "money_quantity"));
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "TotalAmountFmt", ""));

            MCashDoc m = (MCashDoc) model;
            MCashAccount d = (MCashAccount) GetDefaultData();
            if (d != null)
            {
                m.AccountNo = d.AccountNo;
                m.AccountName = d.AccountName;
                m.CashAccountID = d.CashAccountID;
                m.TotalAmount = d.TotalAmount;
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
            //MSalePurchaseHistory mp = model as MSalePurchaseHistory;
            //mp.Category = eType;

            CTable tb = model.GetDbObject();            

            items = OnixWebServiceAPI.GetCashTransactionList(tb);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MCashDoc v = new MCashDoc(o);
                v.IsEnabled = false;

                if (idx == 0)
                {
                    //Create one for balance forward
                    MCashDoc bfw = new MCashDoc(new CTable(""));
                    bfw.IsEnabled = false;

                    bfw.DocumentNo = CLanguage.getValue("balance_forward");
                    bfw.DocumentDate = v.DocumentDate;
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
