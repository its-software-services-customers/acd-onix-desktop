using System;
using System.Windows;
using System.Collections;
using System.Windows.Controls;

using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.UI.HumanResource.Utils;

namespace Onix.ClientCenter.UI.HumanResource.OTDocument
{  
    public partial class WinAddEditOtDocItem : WinBase
    {
        private MVOTDocumentItem mv = null;
        private MVOTDocument mvParent = null;
        private ArrayList otRates = null;

        public WinAddEditOtDocItem(CWinLoadParam param) : base(param)
        {
            accessRightName = "HR_OT_EDIT";
            mvParent = (MVOTDocument)loadParam.ActualParentView;

            InitializeComponent();

            otRates = HRUtils.ConstructOtRates();
            //Need to be after InitializeComponent

            registerValidateControls(lblWorkDateFrom, txtFromWorkHour, false);
            registerValidateControls(lblWorkDateFrom, txtFromWorkMin, false);
            registerValidateControls(lblWorkDateFrom, txtToWorkHour, false);
            registerValidateControls(lblWorkDateFrom, txtToWorkMin, false);
            registerValidateControls(lblFromDate, txtFromHour, false);
            registerValidateControls(lblFromDate, txtFromMin, false);
        }

        public Boolean IsMonthly
        {
            get
            {
                return (mvParent.EmployeeType.Equals("2"));
            }
        }

        public Boolean IsDaily
        {
            get
            {
                return (mvParent.EmployeeType.Equals("1"));
            }
        }

        protected override MBaseModel createObject()
        {
            mv = new MVOTDocumentItem(new CTable(""));
            mv.FromWorkDate = DateTime.Now;
            mv.FromOtDate = DateTime.Now;
            mv.ToOtDate = DateTime.Now;
            mv.CreateDefaultValue();
            mv.OtRate = mvParent.OtRate;

            if (IsMonthly)
            {
                mv.OtFlag = true;
            }

            if (loadParam.Mode.Equals("A"))
            {
                int idx = CUtil.StringToInt(loadParam.GenericType);
                MVOTRate rate = (MVOTRate)otRates[idx];

                mv.FromTimeHH = rate.FromTimeHH;
                mv.FromTimeMM = rate.FromTimeMM;
                mv.ToTimeHH = rate.ToTimeHH;
                mv.ToTimeMM = rate.ToTimeMM;
                mv.MultiplierType = rate.Multiplier;
            }

            return (mv);
        }

        private Boolean validateTime(String type, TextBox tbx)
        {
            String txt = tbx.Text;
            int value = CUtil.StringToInt(txt);

            if (txt.Length != 2)
            {
                CHelper.ShowErorMessage("", "ERROR_TIME_2_DIGIT", null);
                tbx.Focus();

                return (false);
            }

            Boolean invalidHH = (value > 23) || (value < 0);
            if (type.Equals("HH") && invalidHH)
            {
                CHelper.ShowErorMessage("", "ERROR_TIME_RANGE_INVALID", null);
                tbx.Focus();

                return (false);
            }

            Boolean invalidMM = (value > 59) || (value < 0);
            if (type.Equals("MM") && invalidMM)
            {
                CHelper.ShowErorMessage("", "ERROR_TIME_RANGE_INVALID", null);
                tbx.Focus();

                return (false);
            }

            return (true);
        }

        protected override bool postValidate()
        {
            if (mv.IsForIncome)
            {
                if (!validateTime("HH", txtFromWorkHour))
                {
                    return (false);
                }

                if (!validateTime("MM", txtFromWorkMin))
                {
                    return (false);
                }

                if (!validateTime("HH", txtToWorkHour))
                {
                    return (false);
                }

                if (!validateTime("MM", txtToWorkMin))
                {
                    return (false);
                }
            }

            if (mv.IsForOT)
            {
                if (!validateTime("HH", txtFromHour))
                {
                    return (false);
                }

                if (!validateTime("MM", txtFromMin))
                {
                    return (false);
                }

                if (!validateTime("HH", txtToHour))
                {
                    return (false);
                }

                if (!validateTime("MM", txtToMin))
                {
                    return (false);
                }
            }

            return (true);
        }

        protected override void addSubItem()
        {
            MVOTDocument parent = (MVOTDocument)loadParam.ActualParentView;
            parent.AddOTDocItem(mv);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveDataItem();
            if (r)
            {
                vw.IsModified = false;
                IsOKClick = true;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }

        private void UFromDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (loadParam.Mode.Equals("A"))
            {
                mv.ToOtDate = mv.FromOtDate;
            }
        }

        private void CbxOt_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
