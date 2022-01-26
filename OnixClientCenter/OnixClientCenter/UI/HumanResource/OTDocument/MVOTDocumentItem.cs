using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;
using System.Collections.ObjectModel;

namespace Onix.ClientCenter.UI.HumanResource.OTDocument
{
    public class MVOTDocumentItem : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        public MVOTDocumentItem(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }        

        //public override String ID
        //{
        //    get
        //    {
        //        return (EmployeeID);
        //    }
        //}

        public bool IsDeleted
        {
            get
            {
                return (forDelete);
            }

            set
            {
                forDelete = value;
                if (forDelete)
                {
                    oldFlag = ExtFlag;
                    ExtFlag = "D";
                }
                else
                {
                    ExtFlag = oldFlag;
                }
            }
        }

        #region

        public MMasterRef MultiplierObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    MultiplierType = m.MasterID;
                    CalculateOTAmount();
                    NotifyPropertyChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.OtMultipliers;
                if (items == null)
                {
                    return (null);
                }

                return ((MMasterRef)CUtil.IDToObject(items, "MasterID", MultiplierType));
            }
        }

        public String MultiplierType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("MULTIPLIER"));
            }

            set
            {
                GetDbObject().SetFieldValue("MULTIPLIER", value);
                NotifyPropertyChanged();
            }
        }
        
        #endregion

        public String OTDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OT_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("OT_DOC_ID", value);
            }
        }
        
        public String ReceiveAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ReceiveAmountFmt");

                updateFlag();
            }
        }

        public String ReceiveAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveAmount);
                return (fmt);
            }

            set
            {
            }
        }        

        public String Note
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE", value);
                updateFlag();
            }
        }

        public DateTime ToOtDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("TO_OT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("TO_OT_DATE", str);
                int dow = (int)value.DayOfWeek;
                ToDayOfWeek = dow.ToString();

                calculateOTHour();
                CalculateOTAmount();
                NotifyPropertyChanged();
            }
        }

        public String ToOtDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("TO_OT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public DateTime FromOtDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("FROM_OT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("FROM_OT_DATE", str);
                int dow = (int)value.DayOfWeek;
                FromDayOfWeek = dow.ToString();

                calculateOTHour();
                CalculateOTAmount();
                NotifyPropertyChanged();
            }
        }

        public String FromOtDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("FROM_OT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public String FromTime
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String fmt = String.Format("{0}:{1}", FromTimeHH, FromTimeMM);
                return (fmt);
            }

            set
            {
            }
        }

        public String FromTimeHH
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_OT_HH"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_OT_HH", value);
                calculateOTHour();
                CalculateOTAmount();
                NotifyPropertyChanged();
            }
        }

        public String FromTimeMM
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_OT_MM"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_OT_MM", value);
                calculateOTHour();
                CalculateOTAmount();
                NotifyPropertyChanged();
            }
        }

        public String ToTime
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String fmt = String.Format("{0}:{1}", ToTimeHH, ToTimeMM);
                return (fmt);
            }

            set
            {
            }
        }

        public String ToTimeHH
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_OT_HH"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_OT_HH", value);
                calculateOTHour();
                CalculateOTAmount();
                NotifyPropertyChanged();
            }
        }

        public String ToTimeMM
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_OT_MM"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_OT_MM", value);
                calculateOTHour();
                CalculateOTAmount();
                NotifyPropertyChanged();
            }
        }

        public String OtHour
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OT_HOUR"));
            }

            set
            {
                GetDbObject().SetFieldValue("OT_HOUR", value);
                NotifyPropertyChanged("OtHourFmt");
                updateFlag();
            }
        }

        public String OtHourFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(OtHour);
                return (fmt);
            }

            set
            {
            }
        }

        public String OtRate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OT_RATE"));
            }

            set
            {
                GetDbObject().SetFieldValue("OT_RATE", value);
                updateFlag();
            }
        }

        public String OtRateFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(OtRate);
                return (fmt);
            }
        }

        public String OtAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OT_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("OT_AMOUNT", value);
                NotifyPropertyChanged("OtAmountFmt");
                updateFlag();
            }
        }

        public String OtAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(OtAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String OtAdjustHour
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OT_ADJUST_HOUR"));
            }

            set
            {
                GetDbObject().SetFieldValue("OT_ADJUST_HOUR", value);
                calculateOTHour();
                CalculateOTAmount();
                updateFlag();
            }
        }

        public String OtAdjustedTotalHour
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OT_ADJUSTED_TOTAL_HOUR"));
            }

            set
            {
                GetDbObject().SetFieldValue("OT_ADJUSTED_TOTAL_HOUR", value);
                updateFlag();
            }
        }

        public String OtAdjustedTotalHourFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(OtAdjustedTotalHour);
                return (fmt);
            }

            set
            {
            }
        }

        #region Day Of Week

        public String FromDayOfWeek
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_DAY_OF_WEEK"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_DAY_OF_WEEK", value);
                NotifyPropertyChanged("FromDayOfWeekDesc");
                updateFlag();
            }
        }

        public String FromDayOfWeekDesc
        {
            get
            {
                String dow = CUtil.DayOfWeekToText(CUtil.StringToInt(FromDayOfWeek));
                return (dow);
            }

            set
            {
            }
        }

        public String ToDayOfWeek
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_DAY_OF_WEEK"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_DAY_OF_WEEK", value);
                NotifyPropertyChanged("ToDayOfWeekDesc");
                updateFlag();
            }
        }

        public String ToDayOfWeekDesc
        {
            get
            {
                String dow = CUtil.DayOfWeekToText(CUtil.StringToInt(ToDayOfWeek));
                return (dow);
            }

            set
            {
            }
        }

        #endregion

        private void calculateOTHour()
        {
            // YYYY/MM/DD HH:MM:SS
            String startDate = CUtil.DateTimeToDateStringInternal(FromOtDate).Substring(0, 10);
            String startHHMMSS = String.Format("{0:D2}:{1:D2}:00", CUtil.StringToInt(FromTimeHH), CUtil.StringToInt(FromTimeMM));
            String fromIntDate = String.Format("{0} {1}", startDate, startHHMMSS);
            DateTime startDtm = CUtil.InternalDateToDate(fromIntDate);

            String endDate = CUtil.DateTimeToDateStringInternal(FromOtDate).Substring(0, 10); //เปลี่ยนมาใช้แบบภายในวันเดียวกัน
            String endHHMMSS = String.Format("{0:D2}:{1:D2}:00", CUtil.StringToInt(ToTimeHH), CUtil.StringToInt(ToTimeMM));

            if (endHHMMSS.CompareTo(startHHMMSS) < 0)
            {
                //End time less than start time
                //ให้ข้ามไปอีกหนึ่งวัน
                endDate = CUtil.DateTimeToDateStringInternal(FromOtDate.AddDays(1)).Substring(0, 10);
            }

            String endIntDate = String.Format("{0} {1}", endDate, endHHMMSS);
            DateTime endDtm = CUtil.InternalDateToDate(endIntDate);

            TimeSpan span = endDtm.Subtract(startDtm);
            double hour = span.TotalHours;
            double adjust = CUtil.StringToDouble(OtAdjustHour);
            if (hour >= 0)
            {
                OtHour = hour.ToString();
                OtAdjustedTotalHour = (hour - adjust).ToString();
            }
            else
            {
                OtHour = "0.00";
                OtAdjustedTotalHour = "0.00";
            }
        }

        public void CalculateOTAmount()
        {
            double rate = CUtil.StringToDouble(OtRate);
            double hour = CUtil.StringToDouble(OtHour);
            double multiplier = CUtil.StringToDouble(MultiplierType);
            double adjust = CUtil.StringToDouble(OtAdjustHour);
            double amt = rate * (hour - adjust) * multiplier;

            OtAmount = amt.ToString();
        }

        #region Regular Work

        public Boolean OtFlag
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("OT_FLAG");
                return (flag.Equals("Y") || flag.Equals(""));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("OT_FLAG", flag);
                NotifyPropertyChanged();
            }
        }


        public String WorkAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WORK_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("WORK_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("WorkAmountFmt");

                updateFlag();
            }
        }

        public String WorkAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(WorkAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public DateTime FromWorkDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("FROM_WORK_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("FROM_WORK_DATE", str);
                int dow = (int)value.DayOfWeek;
                FromWorkDayOfWeek = dow.ToString();

                calculateWorkHour();
                CalculateWorkAmount();
                NotifyPropertyChanged();
            }
        }

        public String FromWorkDayOfWeek
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_WORK_DAY_OF_WEEK"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_WORK_DAY_OF_WEEK", value);
                NotifyPropertyChanged("FromWorkDayOfWeekDesc");
                updateFlag();
            }
        }

        public String FromWorkDayOfWeekDesc
        {
            get
            {
                String dow = CUtil.DayOfWeekToText(CUtil.StringToInt(FromWorkDayOfWeek));
                return (dow);
            }

            set
            {
            }
        }

        public String FromWorkDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str2 = CUtil.DateTimeToDateString(FromWorkDate);
                return (str2);
            }

            set
            {
            }
        }

        public String FromWorkTime
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String fmt = String.Format("{0}:{1}", FromWorkTimeHH, FromWorkTimeMM);
                return (fmt);
            }

            set
            {
            }
        }

        public String FromWorkTimeHH
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_WORK_HH"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_WORK_HH", value);
                calculateWorkHour();
                CalculateWorkAmount();
                NotifyPropertyChanged();
            }
        }

        public String FromWorkTimeMM
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_WORK_MM"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_WORK_MM", value);
                calculateWorkHour();
                CalculateWorkAmount();
                NotifyPropertyChanged();
            }
        }

        public String ToWorkTime
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String fmt = String.Format("{0}:{1}", ToWorkTimeHH, ToWorkTimeMM);
                return (fmt);
            }

            set
            {
            }
        }

        public String ToWorkTimeHH
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_WORK_HH"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_WORK_HH", value);
                calculateWorkHour();
                CalculateWorkAmount();
                NotifyPropertyChanged();
            }
        }

        public String ToWorkTimeMM
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_WORK_MM"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_WORK_MM", value);
                calculateWorkHour();
                CalculateWorkAmount();
                NotifyPropertyChanged();
            }
        }

        public String WorkAdjustHour
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WORK_ADJUST_HOUR"));
            }

            set
            {
                GetDbObject().SetFieldValue("WORK_ADJUST_HOUR", value);
                calculateWorkHour();
                CalculateWorkAmount();
                updateFlag();
            }
        }

        public String WorkHour
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WORK_HOUR"));
            }

            set
            {
                GetDbObject().SetFieldValue("WORK_HOUR", value);
                NotifyPropertyChanged("WorkHourFmt");
                updateFlag();
            }
        }

        public String WorkHourFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(WorkHour);
                return (fmt);
            }

            set
            {
            }
        }

        public String WorkAdjustedTotalHour
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WORK_ADJUSTED_TOTAL_HOUR"));
            }

            set
            {
                GetDbObject().SetFieldValue("WORK_ADJUSTED_TOTAL_HOUR", value);
                updateFlag();
            }
        }

        public String WorkAdjustedTotalHourFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(WorkAdjustedTotalHour);
                return (fmt);
            }

            set
            {
            }
        }

        private void calculateWorkHour()
        {
            // YYYY/MM/DD HH:MM:SS
            String startDate = CUtil.DateTimeToDateStringInternal(FromWorkDate).Substring(0, 10);
            String startHHMMSS = String.Format("{0:D2}:{1:D2}:00", CUtil.StringToInt(FromWorkTimeHH), CUtil.StringToInt(FromWorkTimeMM));
            String fromIntDate = String.Format("{0} {1}", startDate, startHHMMSS);
            DateTime startDtm = CUtil.InternalDateToDate(fromIntDate);

            String endDate = CUtil.DateTimeToDateStringInternal(FromWorkDate).Substring(0, 10);
            String endHHMMSS = String.Format("{0:D2}:{1:D2}:00", CUtil.StringToInt(ToWorkTimeHH), CUtil.StringToInt(ToWorkTimeMM));
            String endIntDate = String.Format("{0} {1}", endDate, endHHMMSS);
            DateTime endDtm = CUtil.InternalDateToDate(endIntDate);

            TimeSpan span = endDtm.Subtract(startDtm);
            double hour = span.TotalHours;
            double adjust = CUtil.StringToDouble(WorkAdjustHour);
            if (hour >= 0)
            {
                WorkHour = hour.ToString();
                WorkAdjustedTotalHour = (hour - adjust).ToString();
            }
            else
            {
                WorkHour = "0.00";
                WorkAdjustedTotalHour = "0.00";
            }
        }

        public void CalculateWorkAmount()
        {
            double rate = CUtil.StringToDouble(OtRate); //Same as work rate
            double hour = CUtil.StringToDouble(WorkHour);
            double adjust = CUtil.StringToDouble(WorkAdjustHour);
            double amt = rate * (hour - adjust);

            WorkAmount = amt.ToString();
        }

        #endregion Regular Work

        public String ReceiveType
        {
            get
            {
                String defaultValue = "0";
                String type = GetDbObject().GetFieldValue("RECEIVE_TYPE");

                if (GetDbObject() == null)
                {
                    return (defaultValue);
                }

                if (type.Equals(""))
                {
                    return (defaultValue);
                }

                return (type);
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_TYPE", value);

                NotifyPropertyChanged("IsForOT");
                NotifyPropertyChanged("IsForIncome");
                NotifyPropertyChanged("IsForBoth");
            }
        }

        public Boolean IsForBoth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (ReceiveType.Equals("0"));
            }

            set
            {
                if (value)
                {
                    ReceiveType = "0";
                    NotifyPropertyChanged("IsAllowOT");
                    NotifyPropertyChanged("IsAllowIncome");
                }
            }
        }

        public Boolean IsForIncome
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (ReceiveType.Equals("2"));
            }

            set
            {
                if (value)
                {
                    ReceiveType = "2";
                    NotifyPropertyChanged("IsAllowOT");
                    NotifyPropertyChanged("IsAllowIncome");
                }
            }
        }

        public Boolean IsForOT
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (ReceiveType.Equals("1"));
            }

            set
            {
                if (value)
                {
                    ReceiveType = "1";
                    NotifyPropertyChanged("IsAllowOT");
                    NotifyPropertyChanged("IsAllowIncome");
                }
            }
        }

        public Boolean IsAllowIncome
        {
            get
            {
                return (IsForIncome || IsForBoth);
            }

            set
            {
            }
        }

        public Boolean IsAllowOT
        {
            get
            {
                return (IsForOT || IsForBoth);
            }

            set
            {
            }
        }
    }
}
