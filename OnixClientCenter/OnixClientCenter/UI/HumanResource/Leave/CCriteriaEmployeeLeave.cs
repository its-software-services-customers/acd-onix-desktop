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

namespace Onix.ClientCenter.UI.HumanResource.Leave
{
    public class CCriteriaEmployeeLeave : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;
        private String employeeType = "";

        public CCriteriaEmployeeLeave() : base(new MEmployeeLeave(new CTable("")), "CCriteriaEmployeeLeave")
        {
            loadRelatedReferences();
        }

        public override void Init(String type)
        {
            employeeType = type;
            createCriteriaEntries();
            createGridColumns();
        }

        public override bool IsAddEnable()
        {
            return false;
        }

        public override bool IsDeleteEnable()
        {
            return false;
        }

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, null);

            return (btn.GetButton());
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
            CMasterReference.LoadEmployeeDepartments();
            CMasterReference.LoadEmployeePositions();
        }

        #region Criteria Configure

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);

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

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            c0_1.SetButtonEnable(isActionEnable);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colEmployeeCode", "employee_code", "EmployeeCode", 10, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colEmployeeName", "employee_name", "EmployeeName", 14, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c2_1 = new CCriteriaColumnText("colEmployeeLastName", "last_name", "EmployeeLastName", 15, HorizontalAlignment.Left);
            AddGridColumn(c2_1);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colEmployeeType", "employee_type", "EmployeeTypeName", 10, HorizontalAlignment.Left);
            AddGridColumn(c3);

            //CCriteriaColumnText l1 = new CCriteriaColumnText("colLeave1", "late", "LateFmt", 6, HorizontalAlignment.Left);
            //AddGridColumn(l1);

            CCriteriaColumnText l2 = new CCriteriaColumnText("colLeave2", "sick_leave", "SickLeaveFmt", 12, HorizontalAlignment.Left);
            AddGridColumn(l2);

            CCriteriaColumnText l3 = new CCriteriaColumnText("colLeave3", "personal_leave", "PersonalLeaveFmt", 12, HorizontalAlignment.Left);
            AddGridColumn(l3);

            CCriteriaColumnText l4 = new CCriteriaColumnText("colLeave4", "extra_personal_leave", "ExtraLeaveFmt", 12, HorizontalAlignment.Left);
            AddGridColumn(l4);

            CCriteriaColumnText l5 = new CCriteriaColumnText("colLeave5", "annual_leave", "AnnualLeaveFmt", 10, HorizontalAlignment.Left);
            AddGridColumn(l5);

            //CCriteriaColumnText l6 = new CCriteriaColumnText("colLeave6", "abnormal_leave", "AbnormalLeaveFmt", 6, HorizontalAlignment.Left);
            //AddGridColumn(l6);

            //CCriteriaColumnText l7 = new CCriteriaColumnText("colLeave7", "deduction_leave", "DeductionLeaveFmt", 10, HorizontalAlignment.Left);
            //AddGridColumn(l7);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "year"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "LeaveYear", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "month"));
            CCriteriaEntry monthEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "LeaveMonthObj", "");
            monthEntry.SetComboItemSources("Months", "Description");
            AddCriteriaControl(monthEntry);

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "employee_code"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EmployeeCode", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "employee_name"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EmployeeName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "last_name"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EmployeeLastName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "HasResignedFlag", "has_resigned"));

            MEmployeeLeave em = (MEmployeeLeave)model;
            em.LeaveYear = (DateTime.Now.Year).ToString();

            ObservableCollection<MMasterRef> months = CMasterReference.Instance.Months;
            em.LeaveMonthObj = months[DateTime.Now.Month];
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
            if (!CHelper.VerifyAccessRight("HR_LEAVE_VIEW"))
            {
                return;
            }

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("employee");

            CWinLoadParam param = new CWinLoadParam();

            param.Caption =  caption;
            param.Mode = "E";
            param.ActualView = currentObj;
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditEmployeeLeave", param);
        }

        #endregion

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            items = OnixWebServiceAPI.GetListAPI("GetEmployeeLeaveDocList", "EMPLOYEE_LEAVE_LIST", model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MEmployeeLeave v = new MEmployeeLeave(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }


        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("HR_LEAVE_ADD"))
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
