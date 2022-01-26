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

namespace Onix.ClientCenter.UI.HumanResource.EmployeeInfo
{
    public class CCriteriaEmployee : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;
        private String employeeType = "";

        public CCriteriaEmployee() : base(new MEmployee(new CTable("")), "CCriteriaEmployee")
        {
            (model as MEmployee).HasResignedFlag = false;
            loadRelatedReferences();
        }

        public override void Init(String type)
        {
            employeeType = type;
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

            CMasterReference.LoadEmployeeDepartments();
            CMasterReference.LoadEmployeePositions();
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

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "HasResignedFlag", "has_resigned"));
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

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("employee");

            CWinLoadParam param = new CWinLoadParam();

            param.Caption =  caption;
            param.Mode = "E";
            param.ActualView = currentObj;
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditEmployeeInfo", param);
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
            MEmployee em = (MEmployee)model;
            em.EmployeeType = employeeType;

            items = OnixWebServiceAPI.GetEmployeeList(model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MEmployee v = new MEmployee(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("HR_EMPLOYEE_DELETE"))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteEmployee, rc.ToString());

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
            if (!CHelper.VerifyAccessRight("HR_EMPLOYEE_ADD"))
            {
                return;
            }

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("employee");

            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.Mode = "A";
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditEmployeeInfo", param);
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
                CTable newobj = OnixWebServiceAPI.CopyEmployee(currentObj.GetDbObject());

                if (newobj != null)
                {
                    MEmployee ivd = new MEmployee(newobj);
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
