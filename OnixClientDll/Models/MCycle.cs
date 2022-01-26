using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MCycle : MBaseModel
    {

        public MCycle(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("cycle_code", CycleCode);
            ttItems.Add(ct);

            ct = new CToolTipItem("cycle_description", CycleDescription);
            ttItems.Add(ct);

            ct = new CToolTipItem("cycle_type", CycleTypeName);
            ttItems.Add(ct);
            if (CycleType.Equals("1"))
            {
                ct = new CToolTipItem("", DayOfWeekDesciption);
                ttItems.Add(ct);
            }
            else if (CycleType.Equals("2"))
            {
                ct = new CToolTipItem("", DayOfMonthDesciption);
                ttItems.Add(ct);
            }
        }

        public void CreateDefaultValue()
        {
        }
        public String CycleID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CYCLE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CYCLE_ID", value);
            }
        }

        public String CycleCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CYCLE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CYCLE_CODE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String CycleDescription
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DESCRIPTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DESCRIPTION", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String CycleTypeIcon
        {
            get
            {
                if (CycleType.Equals("1"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/view-calendar-week.png");
                }
                else if (CycleType.Equals("2"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/view-calendar-month.png");
                }  

                return ("pack://application:,,,/OnixClient;component/Images/view-calendar-week.png");
            }
        }

        public String DayOfWeek
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DAY_OF_WEEK"));
            }

            set
            {
                GetDbObject().SetFieldValue("DAY_OF_WEEK", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String DayOfMonth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DAY_OF_MONTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("DAY_OF_MONTH", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        #region Cycle Type
        public MMasterRef CycleTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                CycleType = m.MasterID;
                CycleTypeName = m.Description;
            }
        }

        public String CycleType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("CYCLE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CYCLE_TYPE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String CycleTypeName
        {
            get
            {
                if (CycleType.Equals(""))
                {
                    return ("");
                }

                String str = CUtil.CycleTypeToString(CycleType);

                return (str);
            }

            set
            {

            }
        }

        #endregion

        #region Cycle Weekly
        public MMasterRef CycleDayWeeklyObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                DayOfWeek = m.MasterID;
                DayOfWeekDesciption = m.Description;
            }
        }


        public String DayOfWeekDesciption
        {
            get
            {
                if (DayOfWeek.Equals(""))
                {
                    return ("");
                }

                String str = CUtil.CycleDayWeeklyToString(DayOfWeek);

                return (str);
            }

            set
            {

            }
        }
        #endregion

        #region Cycle Monthly
        public MMasterRef CycleDayMonthlyObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                DayOfMonth = m.MasterID;
                DayOfMonthDesciption = m.Description;
            }
        }


        public String DayOfMonthDesciption
        {
            get
            {
                if (DayOfMonth.Equals(""))
                {
                    return ("");
                }

                String str = CLanguage.getValue("date") + " " + DayOfMonth.ToString();

                return (str);
            }

            set
            {

            }
        }
        #endregion
    }


}
