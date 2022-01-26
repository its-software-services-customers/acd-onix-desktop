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
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Commons.Factories;

namespace Onix.ClientCenter.UI.AccountPayable.StockCost
{
    public class CCriteriaStockCostDocument : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String menuType = "";

        public CCriteriaStockCostDocument() : base(new MVStockCostDocument(new CTable("")), "CCriteriaStockCostDocument")
        {
        }

        public override void Init(String type)
        {
            menuType = type;

            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
        }

        #region Criteria Configure

        private void loadRelatedReferences()
        {
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
            
            return (contexts);
        }

        private void createGridColumns()
        {
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left);
            c0.RegisterCheckboxBindingVariable(CheckBox.IsEnabledProperty, "IsEditable");
            AddGridColumn(c0);

            CCriteriaColumnButton c1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colYear", "year", "CostDocYear", 10, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colBegin", "begin_stock_balance_year", "BeginStockBalanceFmt", 16, HorizontalAlignment.Right);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colEnd", "end_stock_balance_year", "EndStockBalanceFmt", 16, HorizontalAlignment.Right);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colIn", "in_amount", "InAmountFmt", 16, HorizontalAlignment.Right);
            AddGridColumn(c5);

            CCriteriaColumnText c6 = new CCriteriaColumnText("colOut", "out_amount", "OutAmountFmt", 16, HorizontalAlignment.Right);
            AddGridColumn(c6);

            CCriteriaColumnText c7 = new CCriteriaColumnText("colStatus", "inventory_doc_status", "DocumentStatusDesc", 16, HorizontalAlignment.Left);
            AddGridColumn(c7);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "year"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "CostDocYear", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_status"));
            CCriteriaEntry statusEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "DocumentStatusObj", "");
            statusEntry.SetComboItemSources("DocumentStatuses", "Description");
            AddCriteriaControl(statusEntry);
        }

        private ArrayList createAddContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuPP30", "tax_pp30", new RoutedEventHandler(cmdAdd_Click), 1);
            contexts.Add(ct1);

            return (contexts);
        }

        #endregion;

        #region Private        

        public void ShowEditWindowEx(MVStockCostDocument actDoc)
        {
            if (!CHelper.VerifyAccessRight("PURCHASE_COST_EDIT"))
            {
                return;
            }

            CWinLoadParam param = new CWinLoadParam();
            param.Caption = CLanguage.getValue("ap_stock_cost");
            param.Mode = "E";
            param.ActualView = actDoc;
            FactoryWindow.ShowWindow("WinAddEditCostDocument", param);            
        }

        #endregion

        public void SetDocumentType(AccountDocumentType dt)
        {
        }

        private void cmdTaxDocAdd_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, cmdTaxDocAdd_Click);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            MVStockCostDocument ad = (model as MVStockCostDocument);
            CTable tb = ad.GetDbObject();

            items = OnixWebServiceAPI.GetListAPI("GetCostDocumentList", "COST_DOC_LIST", tb);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MVStockCostDocument v = new MVStockCostDocument(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        private String getAccessRightDelete()
        {
            String acr = "PURCHASE_COST_DELETE";
            return (acr);
        }

        public override int DeleteData(int rc)
        {

            if (!CHelper.VerifyAccessRight(getAccessRightDelete()))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteAPI, rc.ToString(), "DeleteCostDocument");

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            ShowEditWindowEx((MVStockCostDocument) currentObj);
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("PURCHASE_COST_ADD"))
            {
                return;
            }

            CWinLoadParam param = new CWinLoadParam();
            param.Caption = CLanguage.getValue("ap_stock_cost");
            param.Mode = "A";
            param.GenericType = "1";
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditCostDocument", param);            
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
                ShowEditWindowEx((MVStockCostDocument) currentObj);
            }
        }
#endregion
    }
}
