using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.ClientCenter.UControls;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.HumanResource.EmployeeInfo;
using Onix.ClientCenter.Commons.ModelViews;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.HumanResource.EmployeeInfo
{
    public class CCriteriaEmployeeSearch : CCriteriaGenericBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MVBase> itemSources = new ObservableCollection<MVBase>();
        private MVBase currentObj = null;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaEmployeeSearch() : base(new MVEmployee(new CTable("")), "CCriteriaEmployeeSearch")
        {
            loadRelatedReferences();
        }

        public override void Init(String type)
        {
            createCriteriaEntries();
            createGridColumns();
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

        private void loadRelatedReferences()
        {
            if (!CMasterReference.IsCycleLoad())
            {
                CMasterReference.LoadCycle(true, null);
            }
        }

        #region Criteria Configure

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuCopy", "copy", new RoutedEventHandler(mnuContextMenu_Click), 2);
            contexts.Add(ct2);

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

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            c0_1.SetButtonEnable(isActionEnable);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colEmployeeCode", "employee_code", "EmployeeCode", 10, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colEmployeeName", "employee_name", "EmployeeName", 20, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c2_1 = new CCriteriaColumnText("colEmployeeLastName", "last_name", "EmployeeLastName", 20, HorizontalAlignment.Left);
            AddGridColumn(c2_1);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colEmployeeGender", "gender", "GenderName", 10, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colEmployeeType", "employee_type", "EmployeeTypeName", 10, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colPosition", "position", "PositionName", 20, HorizontalAlignment.Left);
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "employee_code"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EmployeeCode", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "employee_name"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EmployeeName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "last_name"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EmployeeLastName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "department"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "DepartmentName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "position"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "PositionName", ""));
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
            if (!CHelper.VerifyAccessRight("HR_EMPLOYEE_VIEW"))
            {
                return;
            }

            WinAddEditEmployeeInfo c = new WinAddEditEmployeeInfo();
            c.ViewData = currentObj;
            String caption = CLanguage.getValue("employee");
            c.Caption = CLanguage.getValue("edit") + " " + caption;
            c.Mode = "E";
            c.ParentItemSource = itemSources;
            c.ShowDialog();
        }

        #endregion

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, null);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MVBase>> QueryData()
        {           
            items = OnixWebServiceAPI.GetEmployeeList(model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MVEmployee v = new MVEmployee(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MVBase>> tuple = new Tuple<CTable, ObservableCollection<MVBase>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("HR_EMPLOYEE_DELETE"))
            {
                return(rc);
            }

            int rCount = CHelpers.DeleteSelectedItems(itemSources,  OnixWebServiceAPI.DeleteAPI, rc.ToString(), "DeleteEmployee");

            return (rCount);
        }

        public override void DoubleClickData(MVBase m)
        {
            currentObj = m;
            showEditWindow();
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("HR_EMPLOYEE_ADD"))
            {
                return;
            }

            WinAddEditEmployeeInfo w = new WinAddEditEmployeeInfo();
            String caption = CLanguage.getValue("employee");
            w.Caption = (String)(sender as Button).Content + " " + caption;
            w.Mode = "A";
            w.ParentItemSource = itemSources;
            w.ShowDialog();
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            currentObj = (MVBase)(sender as UActionButton).Tag;
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
                CTable newobj = OnixWebServiceAPI.CopyEmployee(currentObj.GetDbObject());

                if (newobj != null)
                {
                    MVEmployee ivd = new MVEmployee(newobj);
                    itemSources.Insert(0, ivd);
                }
                else
                {
                    //Error here
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                }

                CUtil.EnableForm(true, ParentControl);
            }
        }
#endregion
    }
}
