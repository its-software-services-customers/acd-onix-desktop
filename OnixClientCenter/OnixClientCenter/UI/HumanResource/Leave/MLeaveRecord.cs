using Onix.Client.Helper;
using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MLeaveRecord : MBaseModel
    {
        private int seq = 0;

        public MLeaveRecord(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }

        public int Seq
        {
            get
            {
                return (seq);
            }

            set
            {
                GetDbObject().SetFieldValue("INTERNAL_SEQ", value.ToString());
                seq = value;
            }
        }

        public DateTime LeaveDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("LEAVE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);
                updateFlag();

                GetDbObject().SetFieldValue("LEAVE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String EmployeeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_ID", value);
            }
        }

        public String LeaveMonth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LEAVE_MONTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("LEAVE_MONTH", value);
            }
        }

        public String LeaveMonthName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                string name = CUtil.IDToMonth(CUtil.StringToInt(LeaveMonth));
                return (name);
            }

            set
            {
            }
        }

        public String LeaveRecordID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMP_LEAVE_REC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMP_LEAVE_REC_ID", value);
            }
        }

        public String Late
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LATE"));
            }

            set
            {
                GetDbObject().SetFieldValue("LATE", value);
                updateFlag();
            }
        }

        public String LateFmt
        {
            get
            {
                return (CUtil.FormatNumber(Late));
            }

            set
            {
            }
        }

        #region Sick Leave
        public String SickLeave
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SICK_LEAVE"));
            }

            set
            {
                GetDbObject().SetFieldValue("SICK_LEAVE", value);
                updateFlag();
            }
        }

        public MMasterRef SickLeaveObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                SickLeave = m.MasterID;

                updateFlag();
                NotifyPropertyChanged();
            }

            get
            {
                ObservableCollection<MMasterRef> items = CMasterReference.Instance.LeaveDurations;
                if (items == null)
                {
                    return (null);
                }

                String tm = SickLeave;
                return (CUtil.MasterIDToObject(items, tm));                
            }
        }

        public String SickLeaveFmt
        {
            get
            {
                return (CUtil.FormatNumberDash(SickLeave));
            }

            set
            {
            }
        }
        #endregion

        #region Personal Leave
        public String PersonalLeave
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PERSONAL_LEAVE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PERSONAL_LEAVE", value);
                updateFlag();
            }
        }

        public MMasterRef PersonalLeaveObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                PersonalLeave = m.MasterID;

                updateFlag();
                NotifyPropertyChanged();
            }

            get
            {
                ObservableCollection<MMasterRef> items = CMasterReference.Instance.LeaveDurations;
                if (items == null)
                {
                    return (null);
                }

                String tm = PersonalLeave;
                return (CUtil.MasterIDToObject(items, tm));
            }
        }

        public String PersonalLeaveFmt
        {
            get
            {
                return (CUtil.FormatNumberDash(PersonalLeave));
            }

            set
            {
            }
        }
        #endregion

        #region Extra Leave
        public String ExtraLeave
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXTRA_LEAVE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXTRA_LEAVE", value);
                updateFlag();
            }
        }

        public MMasterRef ExtraLeaveObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                ExtraLeave = m.MasterID;

                updateFlag();
                NotifyPropertyChanged();
            }

            get
            {
                ObservableCollection<MMasterRef> items = CMasterReference.Instance.LeaveDurations;
                if (items == null)
                {
                    return (null);
                }

                String tm = ExtraLeave;
                return (CUtil.MasterIDToObject(items, tm));
            }
        }

        public String ExtraLeaveFmt
        {
            get
            {
                return (CUtil.FormatNumberDash(ExtraLeave));
            }

            set
            {
            }
        }
        #endregion

        #region Annual Leave
        public String AnnualLeave
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ANNUAL_LEAVE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ANNUAL_LEAVE", value);
                updateFlag();
            }
        }

        public MMasterRef AnnualLeaveObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                AnnualLeave = m.MasterID;

                updateFlag();
                NotifyPropertyChanged();
            }

            get
            {
                ObservableCollection<MMasterRef> items = CMasterReference.Instance.LeaveDurations;
                if (items == null)
                {
                    return (null);
                }

                String tm = AnnualLeave;
                return (CUtil.MasterIDToObject(items, tm));
            }
        }

        public String AnnualLeaveFmt
        {
            get
            {
                return (CUtil.FormatNumberDash(AnnualLeave));
            }

            set
            {
            }
        }
        #endregion

        public String AbnormalLeave
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ABNORMAL_LEAVE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ABNORMAL_LEAVE", value);
                updateFlag();
            }
        }

        public String AbnormalLeaveFmt
        {
            get
            {
                return (CUtil.FormatNumberDash(AbnormalLeave));
            }

            set
            {
            }
        }

        public String DeductionLeave
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCTION_LEAVE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCTION_LEAVE", value);
                updateFlag();
            }
        }

        public String DeductionLeaveFmt
        {
            get
            {
                return (CUtil.FormatNumberDash(DeductionLeave));
            }

            set
            {
            }
        }
    }
}
