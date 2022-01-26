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

namespace Onix.ClientCenter.Criteria
{
    public class CCriteriaBranchConfig : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        public CCriteriaBranchConfig() : base(new MBranchConfigCenter(new CTable("")), "CCriteriaBranchConfig")
        {
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

            if (!CMasterReference.IsLocationLoad())
            {
                CMasterReference.LoadLocation(true, null);
            }

            if (!CMasterReference.IsMasterRefLoad(MasterRefEnum.MASTER_BRANCH))
            {
                CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetMasterRefList);
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
            //0.05 0.05, 0.10, 0.40, 0.10, 0.15, 0.15
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left);
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colBranchNo", "branch_no", "BranchCode", 10, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colBranchName", "branch_name", "BranchName", 20, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colKey", "key", "Key", 60, HorizontalAlignment.Left);
            AddGridColumn(c3);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "branch_no"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "BranchCode", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "branch_name"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "BranchName", ""));
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
            if (!CHelper.VerifyAccessRight("BRANCH_CONFIG_VIEW"))
            {
                return;
            }

            MBaseModel v = currentObj;

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("Branch");
            WinAddEditBranchConfig w = new WinAddEditBranchConfig("E", null, (MBranchConfigCenter) v, caption);
            w.ShowDialog();
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
            items = OnixWebServiceAPI.GetBranchConfigList(model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MBranchConfigCenter v = new MBranchConfigCenter(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("BRANCH_CONFIG_DELETE"))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteBranchConfig, rc.ToString());

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
            if (!CHelper.VerifyAccessRight("BRANCH_CONFIG_ADD"))
            {
                return;
            }

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("Branch");            
            WinAddEditBranchConfig w = new WinAddEditBranchConfig("A", itemSources, null, caption);
            w.ItemAddedHandler = ItemAddedEvent;
            w.ShowDialog();
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
        }
#endregion
    }
}
