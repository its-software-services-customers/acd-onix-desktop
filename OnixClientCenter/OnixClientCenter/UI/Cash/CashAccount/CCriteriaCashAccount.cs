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

namespace Onix.ClientCenter.UI.Cash.CashAccount
{
    public class CCriteriaCashAccount : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        public CCriteriaCashAccount() : base(new MCashAccount(new CTable("")), "CCriteriaCashAccount")
        {
            createCriteriaEntries();
            createGridColumns();
        }

        #region Criteria Configure
        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuCopy", "copy", new RoutedEventHandler(mnuContextMenu_Click), 2);
            contexts.Add(ct2);

            CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuCashMovement", "cash_movement", new RoutedEventHandler(mnuContextMenu_Click), 3);
            contexts.Add(ct3);

            return (contexts);
        }

        private void createGridColumns()
        {
            //0.05, 0.05, 0.20, 0.30, 0.15, 0.15, 0.10 
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left);
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colAccNo", "AccNo", "AccountNo", 20, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colAccName", "AccName", "AccountName", 30, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colBank", "Bank", "BankName", 15, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colBranch", "Branch", "BankBranchName", 15, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colAmount", "money_quantity", "TotalAmountFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "AccNo"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "AccountNo", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "AccName"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "AccountName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "Bank"));

            CCriteriaEntry lcTypeEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "BankObj", "");
            lcTypeEntry.SetComboItemSources("Banks", "Description");
            AddCriteriaControl(lcTypeEntry);
            
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "Branch"));            

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "BankBranchName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsForOwner", "cash_account_for_owner"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsForPayroll", "cash_account_for_payroll"));
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

        private void refreshCashAccount()
        {
            CMasterReference.LoadCashAccount();
        }

        private void showEditWindow()
        {
            if (!CHelper.VerifyAccessRight("CASH_ACCOUNT_VIEW"))
            {
                return;
            }


            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("CashAcc");

            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.Mode = "E";
            param.ActualView = currentObj;
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditCashAcc", param);          

            refreshCashAccount();
        }

        #endregion

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("CASH_ACCOUNT_ADD"))
            {
                return;
            }

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("CashAcc");

            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.Mode = "A";
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditCashAcc", param);

            refreshCashAccount();
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

                MBaseModel v = currentObj;
            
                CUtil.EnableForm(true, ParentControl);

                CUtil.EnableForm(false, ParentControl);
                CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("CopyCashAccount", (v as MCashAccount).GetDbObject());
                CUtil.EnableForm(true, ParentControl);
                if (newobj != null)
                {
                    MCashAccount ivd = new MCashAccount(newobj);
                    itemSources.Insert(0, ivd);
                }
                else
                {
                    //Error here
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                }

                refreshCashAccount();
            }
            else if (name.Equals("mnuCashMovement"))
            {
                showMovementHistoryWindow("cash_movement");
            }
        }

        private void showMovementHistoryWindow(String captionKey)
        {
            CCriteriaCashMovementHistory cr = new CCriteriaCashMovementHistory();
            cr.SetActionEnable(false);
            cr.SetDefaultData(currentObj);
            cr.Init("");

            WinMovementHistory w = new WinMovementHistory(cr, CLanguage.getValue(captionKey));
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
            items = OnixWebServiceAPI.GetListAPI("GetCashAccountList", "CASH_ACCOUNT_LIST", model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MCashAccount v = new MCashAccount(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("CASH_ACCOUNT_DELETE"))
            {
                return (rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteAPI, rc.ToString(), "DeleteCashAccount");
            if (rCount > 0)
            {
                refreshCashAccount();
            }

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }
    }
}
