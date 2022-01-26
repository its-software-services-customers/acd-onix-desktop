using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Windows;
using System.Windows.Controls;
using Onix.ClientCenter.Commons.Utils;
using System.Collections;
using Onix.Client.Controller;
using System.IO;
using Onix.ClientCenter.Windows;

namespace Onix.ClientCenter.UI.HumanResource.Leave
{
    public partial class WinAddEditEmployeeLeave : WinBase
    {
        public WinAddEditEmployeeLeave(CWinLoadParam param) : base(param)
        {               
            accessRightName = "HR_LEAVE_EDIT";

            createAPIName = "SaveEmployeeLeaveDoc";
            updateAPIName = "SaveEmployeeLeaveDoc";
            getInfoAPIName = "GetEmployeeLeaveDocInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblNameTh, txtNameTh, false);
            registerValidateControls(lblLastNameTh, txtLastNameTh, false);  
        }

        protected override MBaseModel createObject()
        {
            MEmployeeLeave mv = new MEmployeeLeave(new CTable("EMPLOYEE"));
            mv.DocumentDate = DateTime.Now;
            
            mv.CreateDefaultValue();
            //mv.AddLeaveRecord(new MLeaveRecord(new CTable("")));
            
            return (mv);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }

        protected override Boolean postValidate()
        {
            Hashtable hashDups = new Hashtable();

            MEmployeeLeave mv = (MEmployeeLeave)vw;            

            int leaveMonth = CUtil.StringToInt(mv.LeaveMonth);
            int leaveYear = CUtil.StringToInt(mv.LeaveYear);

            var items = mv.LeaveRecords;

            foreach (var item in items)
            {
                if (item.ExtFlag.Equals("D"))
                {
                    continue;
                }

                DateTime leaveDate = item.LeaveDate;

                string key = CUtil.DateTimeToDateString(leaveDate);
                if (hashDups.ContainsKey(key))
                {
                    CHelper.ShowErorMessage(leaveDate.ToString(), "ERROR_DUPLICATE_DATE", null);
                    return false;
                }
                else
                {
                    hashDups.Add(key, key);
                }
                
                if ((leaveDate.Month != leaveMonth) || (leaveDate.Year != leaveYear))
                {
                    CHelper.ShowErorMessage(leaveDate.ToString(), "ERROR_NOT_IN_SAME_MONTH", null);
                    return false;
                }

                double sum = CUtil.StringToDouble(item.AnnualLeave) + 
                    CUtil.StringToDouble(item.PersonalLeave) +
                    CUtil.StringToDouble(item.ExtraLeave) + CUtil.StringToDouble(item.SickLeave);
                if (sum > 1)
                {
                    CHelper.ShowErorMessage(leaveDate.ToString(), "ERROR_LEAVE_OVER_DAY", null);
                    return false;
                }
            }

            return (true);
        }

        private void CboPoc_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            MEmployeeLeave mv = (MEmployeeLeave)vw;

            mv.CalculateLeaveTotal();
            vw.IsModified = true;
        }

        private void CmdAdd_Click(object sender, RoutedEventArgs e)
        {
            MEmployeeLeave mv = (MEmployeeLeave)vw;            

            MLeaveRecord item = new MLeaveRecord(new CTable(""));
            int day = DateTime.Now.Day;

            item.LeaveDate = new DateTime(CUtil.StringToInt(mv.LeaveYear), CUtil.StringToInt(mv.LeaveMonth), day);

            mv.CalculateLeaveTotal();
            mv.AddLeaveRecord(item);

            mv.IsModified = true;
        }

        private void CmdDelete_Click(object sender, RoutedEventArgs e)
        {
            MEmployeeLeave mv = (MEmployeeLeave) vw;

            MLeaveRecord pp = (MLeaveRecord)(sender as Button).Tag;

            bool result = CHelper.AskConfirmMessage(pp.LeaveDate.ToString(), "CONFIRM_DELETE_ITEM");
            if (result)
            {
                mv.RemoveLeaveRecord(pp);
                mv.CalculateLeaveTotal();
                mv.IsModified = true;
            }
        }

        private void CmdAdd2_Click(object sender, RoutedEventArgs e)
        {
            postValidate();
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MEmployeeLeave mv = (MEmployeeLeave)vw;

            mv.CalculateLeaveTotal();
            vw.IsModified = true;
        }

        private MEmployeeLeave GetEmployeeLeaveInfo()
        {
            MEmployeeLeave mv = (MEmployeeLeave)vw;

            CTable t = new CTable("");
            t.SetFieldValue("EMPLOYEE_ID", mv.EmployeeID);
            CTable e = OnixWebServiceAPI.SubmitObjectAPI("GetEmployeeInfo", t);

            MEmployee emp = new MEmployee(e);

            CTable o = OnixWebServiceAPI.SubmitObjectAPI("GetEmployeeLeaveInfo", emp.GetDbObject());
            MEmployeeLeave el = new MEmployeeLeave(o);
            el.InitializeAfterLoaded();

            el.DepartmentName = Path.GetFileName(emp.DepartmentObj.Description);
            el.PositionName = Path.GetFileName(emp.PositionObj.Description);

            return el;
        }

        private void CmdLeaveReport_Click(object sender, RoutedEventArgs e)
        {
            MEmployeeLeave mv = GetEmployeeLeaveInfo();
            WinFormPrinting w = new WinFormPrinting("grpHRLeave", mv);
            w.ShowDialog();
        }

        private void CmdMonthChange_Click(object sender, RoutedEventArgs e)
        {
            cmdMonthChange.ContextMenu.IsOpen = true;
        }

        private void MnuMonth_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (MenuItem)sender;
            string month = (string) mnu.Tag;

            MEmployeeLeave mv = (MEmployeeLeave)vw;
            mv.EmpLeaveDocId = "";
            mv.ClearLeaveRecord();
            mv.LeaveMonth = month;            
            //Use the same LeaveYear

            loadParam.ActualView = mv;
            loadParam.Mode = "E";

            loadData();
            vw.IsModified = false;
        }
    }
}
