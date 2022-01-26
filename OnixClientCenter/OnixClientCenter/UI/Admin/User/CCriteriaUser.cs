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

namespace Onix.ClientCenter.UI.Admin.User
{
    public class CCriteriaUser : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaUser() : base(new MUserView(new CTable("")), "CCriteriaUser")
        {
        }

        public override void Init(String type)
        {
            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
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

        public void SetDocumentType(CashDocumentType dt)
        {
        }

        #region Criteria Configure

        private void loadRelatedReferences()
        {
            CMasterReference.LoadUserGroup(OnixWebServiceAPI.GetUserGroupList);
        }

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuChangePassword", "passwd", new RoutedEventHandler(mnuContextMenu_Click), 2);
            contexts.Add(ct2);

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

            CCriteriaColumnText c1 = new CCriteriaColumnText("colUserName", "user", "UserName", 15, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDescription", "description", "Description", 35, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colGroup", "user_group", "GroupName", 20, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnImageText c4 = new CCriteriaColumnImageText("colIsActive", "is_active", "", "IsActiveIcon", 10, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnImageText c5 = new CCriteriaColumnImageText("colIsAdmin", "is_admin", "", "IsAdminIcon", 10, HorizontalAlignment.Left);
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "user"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "UserName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "user_group"));
        
            CCriteriaEntry groupEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "GroupObj", "");
            groupEntry.SetComboItemSources("UserGroups", "GroupName");
            AddCriteriaControl(groupEntry);


            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsAdminCriteria", "is_admin"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsActiveCriteria", "is_active"));
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
            if (!CHelper.VerifyAccessRight("ADMIN_USER_VIEW"))
            {
                return;
            }

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("user");

            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.Mode = "E";
            param.ActualView = currentObj;
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditUser", param);
        }

        #endregion

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, cmdAdd_Click);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            items = OnixWebServiceAPI.GetUserList(model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();

            foreach (CTable o in items)
            {
                MUserView v = null;
                v = new MUserView(o);

                itemSources.Add(v);
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteUser, rc.ToString());
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
            if (!CHelper.VerifyAccessRight("ADMIN_USER_ADD"))
            {
                return;
            }

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("user");

            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.Mode = "A";
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditUser", param);
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
            else if (name.Equals("mnuChangePassword"))
            {
                CWinLoadParam param = new CWinLoadParam();

                param.Mode = "E";
                param.ActualView = currentObj;
                FactoryWindow.ShowWindow("WinAdminEditPassword", param);
            }
        }
#endregion
    }
}
