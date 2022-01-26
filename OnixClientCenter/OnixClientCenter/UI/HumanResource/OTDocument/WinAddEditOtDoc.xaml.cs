using System;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Factories;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.Windows;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.HumanResource.OTDocument
{
    public partial class WinAddEditOtDoc : WinBase
    {
        private MVOTDocument mv = null;
        private MVOTDocumentItem currentViewObj = null;
        private MVPayrollExpenseItem currentExpenseViewObj = null;
        private MVPayrollDeductionItem currentDeductionViewObj = null;

        private String employeeType = "";

        public WinAddEditOtDoc(CWinLoadParam param) : base(param)
        {
            accessRightName = "HR_OT_EDIT";

            createAPIName = "CreateOtDoc";
            updateAPIName = "UpdateOtDoc";
            getInfoAPIName = "GetOtDocInfo";
            approveAPIName = "ApproveOtDoc";

            employeeType = param.GenericType;
            InitializeComponent();

            //Need to be after InitializeComponent
            //registerValidateControls(lblYear, dtFromDate, false);
            registerValidateControls(lblEmployee, uEmployee, false);

            double[] ratios = new double[10] {0.04, 0.06, 0.10, 0.10, 0.10, 0.14, 0.10, 0.12, 0.12, 0.12 };
            registerListViewSize(lsvItem.Name, ratios);

            ratios = new double[10] { 0.05, 0.05, 0.15, 0.15, 0.10, 0.10, 0.10, 0.10, 0.10, 0.10 };
            registerListViewSize(lsvDaily.Name, ratios);

            ratios = new double[8] { 0.05, 0.05, 0.15, 0.15, 0.30, 0.10, 0.10, 0.10 };
            registerListViewSize(lsvExpense.Name, ratios);

            ratios = new double[6] { 0.05, 0.05, 0.15, 0.15, 0.50, 0.10};
            registerListViewSize(lsvDeduct.Name, ratios);
        }

        protected override bool isEditable()
        {
            mv = (MVOTDocument)loadParam.ActualView;
            if (mv != null)
            {
                return (mv.IsEditable);
            }

            return (true);
        }

        protected override MBaseModel createObject()
        {
            mv = new MVOTDocument(new CTable(""));
            mv.EmployeeType = loadParam.GenericType;
            mv.DocumentStatus = "1";
            mv.DocumentDate = DateTime.Now;

            return (mv);
        }

        public LookupSearchType2 EmployeeLookupType
        {
            get
            {
                if (employeeType.Equals("1"))
                {
                    return (LookupSearchType2.EmployeeLookupDaily);
                }
                else if (employeeType.Equals("2"))
                {
                    return (LookupSearchType2.EmployeeLookupMonthly);
                }

                return (LookupSearchType2.EmployeeLookup);
            }

            set
            {
            }
        }

        public Boolean IsMonthly
        {
            get
            {
                return (employeeType.Equals("2"));
            }
        }

        public Boolean IsDaily
        {
            get
            {
                return (employeeType.Equals("1"));
            }
        }

        private void CmdAddEmployee_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (!vw.IsModified)
            {
                return;
            }

            Boolean r = saveData();
            if (r)
            {
                loadParam.ActualView = vw;
                loadParam.Mode = "E";

                loadData();
                vw.IsModified = false;
            }
        }

        protected override Boolean postValidate()
        {
            return (true);
        }        

        private void CmdApprove_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = approveData();
            if (r)
            {
                vw.IsModified = false;
                this.Close();
            }
        }

        private void LsvAccoutItem_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void LsvAccoutItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvItem.SelectedItems.Count == 1)
            {
                currentViewObj = (MVOTDocumentItem)lsvItem.SelectedItems[0];
                showEditWindow();
            }
        }

        private void CbkRemove_Checked(object sender, RoutedEventArgs e)
        {
            mv.CalculateTotalFields();
            vw.IsModified = true;
        }

        private void CmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MVOTDocumentItem)btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void MnuDocumentEdit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuDocumentEdit"))
            {
                showEditWindow();
            }
        }

        private void showEditWindow()
        {
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("hr_ot_form");
            param.Mode = "E";
            param.ActualParentView = mv;
            param.ActualView = currentViewObj;
            param.GenericType = loadParam.GenericType;
            string windowName = "WinAddEditOtDocItem";
            if (param.GenericType.Equals("2"))
            {
                windowName = "WinAddEditOtDocItem2";
            }
            Boolean okClick = FactoryWindow.ShowWindow(windowName, param);

            if (okClick)
            {
                (vw as MVOTDocument).CalculateTotalFields();
                vw.IsModified = true;
            }
        }

        private void showEditExpenseWindow()
        {
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("hr_allowance_form");
            param.Mode = "E";
            param.ActualParentView = mv;
            param.ActualView = currentExpenseViewObj;
            param.GenericType = loadParam.GenericType;
            Boolean okClick = FactoryWindow.ShowWindow("WinAddEditPayrollExpenseItem", param);

            if (okClick)
            {
                (vw as MVOTDocument).CalculateTotalFields();
                vw.IsModified = true;
            }
        }

        private void showEditDeductWindow()
        {
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("deduction_type");
            param.Mode = "E";
            param.ActualParentView = mv;
            param.ActualView = currentDeductionViewObj;
            param.GenericType = loadParam.GenericType;
            Boolean okClick = FactoryWindow.ShowWindow("WinAddEditPayrollDeductItem", param);

            if (okClick)
            {
                (vw as MVOTDocument).CalculateTotalFields();
                vw.IsModified = true;
            }
        }

        private void CmdAdd_Click(object sender, RoutedEventArgs e)
        {
            cmdAddAuto.ContextMenu.IsOpen = true;            
        }

        private void updateOtRate()
        {
            CUtil.EnableForm(false, this);
            CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("GetEmployeeInfo", vw.GetDbObject());
            if (newobj != null)
            {
                MEmployee emp = new MEmployee(newobj);
                mv.OtRate = emp.HourRate;
            }
            CUtil.EnableForm(true, this);
        }

        private void UEmployee_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
            if (!isInLoad)
            {
                updateOtRate();
            }
        }

        private void CmdCalculate_Click(object sender, RoutedEventArgs e)
        {
            updateOtRate();
            mv.CalculateOTDocItem();
            mv.CalculateTotalFields();
            vw.IsModified = true;
        }

        private void CmdAllowanceAdd_Click(object sender, RoutedEventArgs e)
        {
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = CLanguage.getValue("add") + " " + CLanguage.getValue("hr_allowance_form");
            param.Mode = "A";
            param.ActualParentView = mv;
            param.GenericType = loadParam.GenericType;
            Boolean okClick = FactoryWindow.ShowWindow("WinAddEditPayrollExpenseItem", param);

            if (okClick)
            {
                (vw as MVOTDocument).CalculateTotalFields();
                vw.IsModified = true;
            }
        }

        private void LsvExpense_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void LsvExpense_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvExpense.SelectedItems.Count == 1)
            {
                currentExpenseViewObj = (MVPayrollExpenseItem)lsvExpense.SelectedItems[0];
                showEditExpenseWindow();
            }
        }

        private void CmdDailyCalculate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdDailyAdd_Click(object sender, RoutedEventArgs e)
        {
            cmdDailyAdd.ContextMenu.IsOpen = true;
        }

        private void LvDaily_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvDaily.SelectedItems.Count == 1)
            {
                currentViewObj = (MVOTDocumentItem)lsvDaily.SelectedItems[0];
                showEditWindow();
            }
        }

        private void LvDaily_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void CmdDeductAdd_Click(object sender, RoutedEventArgs e)
        {
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = CLanguage.getValue("add") + " " + CLanguage.getValue("deduction_type");
            param.Mode = "A";
            param.ActualParentView = mv;
            param.GenericType = loadParam.GenericType;
            //param.ActualView = actDoc;
            Boolean okClick = FactoryWindow.ShowWindow("WinAddEditPayrollDeductItem", param);

            if (okClick)
            {
                (vw as MVOTDocument).CalculateTotalFields();
                vw.IsModified = true;
            }
        }

        private void LsvDeduct_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvDeduct.SelectedItems.Count == 1)
            {
                currentDeductionViewObj = (MVPayrollDeductionItem)lsvDeduct.SelectedItems[0];
                showEditDeductWindow();
            }
        }

        private void MnuDay_Click(object sender, RoutedEventArgs e)
        {
            CWinLoadParam param = new CWinLoadParam();
            MenuItem mnu = (MenuItem) sender;

            param.Caption = CLanguage.getValue("add") + " " + CLanguage.getValue("hr_ot_form");
            param.Mode = "A";
            param.ActualParentView = mv;
            param.GenericType = (string) mnu.Tag;
            Boolean okClick = FactoryWindow.ShowWindow("WinAddEditOtDocItem2", param);

            if (okClick)
            {
                (vw as MVOTDocument).CalculateTotalFields();
                vw.IsModified = true;
            }
        }

        private void MnuDaily_Click(object sender, RoutedEventArgs e)
        {
            CWinLoadParam param = new CWinLoadParam();
            MenuItem mnu = (MenuItem)sender;

            param.Caption = CLanguage.getValue("add") + " " + CLanguage.getValue("hr_ot_form");
            param.Mode = "A";
            param.ActualParentView = mv;
            param.GenericType = (string)mnu.Tag;
            Boolean okClick = FactoryWindow.ShowWindow("WinAddEditOtDocItem", param);

            if (okClick)
            {
                (vw as MVOTDocument).CalculateTotalFields();
                vw.IsModified = true;
            }        
        }
    }
}
