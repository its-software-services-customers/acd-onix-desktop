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

namespace Onix.ClientCenter.UI.Admin.LoginHistory
{
    public class CCriteriaLoginHistory : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = false;

        public CCriteriaLoginHistory() : base(new MLoginHistory(new CTable("")), "CCriteriaLoginHistory")
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
            c0.RegisterCheckboxBindingVariable(CheckBox.IsEnabledProperty, "IsEditable");
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            c0_1.SetButtonEnable(isActionEnable);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colLoginDateTime", "login_date_time", "LoginDateTimeFmt", 15, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colUser", "user", "UserName", 15, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colIP", "ip_address", "IPAddress", 15, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colLogoutDateTime", "logout_date_time", "LogoutDateTimeFmt", 15, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colSession", "session", "Session", 30, HorizontalAlignment.Left);
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "user"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "UserName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "ip_address"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "IPAddress", ""));
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
            items = OnixWebServiceAPI.GetLoginHistoryList(model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MLoginHistory v = new MLoginHistory(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override bool IsAddEnable()
        {
            return(false);
        }

        public override bool IsDeleteEnable()
        {
            return (false);
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
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuEdit"))
            {
                showEditWindow();
            }
            else if (name.Equals("mnuCopy"))
            {
            }
        }
#endregion
    }
}
