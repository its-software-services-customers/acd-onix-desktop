using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MPeriod : MBaseModel
    {
        public MPeriod(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {
            IsEnabled = true;
            PeriodType = IntervalTypeEnum.DAY_ENTIRE;
        }

        public String PeriodID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PERIOD_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PERIOD_ID", value);
            }
        }

        public String DayOfWeek
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
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

        public IntervalTypeEnum PeriodType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (IntervalTypeEnum.DAY_ENTIRE);
                }

                String tmp = GetDbObject().GetFieldValue("PERIOD_TYPE");
                if (tmp.Equals(""))
                {
                    return (IntervalTypeEnum.DAY_ENTIRE);
                }


                return ((IntervalTypeEnum)int.Parse(tmp));
            }

            set
            {
                String tmp = ((int)value).ToString();
                GetDbObject().SetFieldValue("PERIOD_TYPE", tmp);
                updateFlag();

                NotifyPropertyChanged();
            }
        }

        public String IsEnabledIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ENABLE_FLAG");
                if (flag.Equals("Y"))
                {

                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }

        public String FromHour1
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_HOUR1"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_HOUR1", value);
                updateFlag();

                NotifyPropertyChanged();
            }
        }

        public String FromMinute1
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_MINUTE1"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_MINUTE1", value);
                updateFlag();

                NotifyPropertyChanged();
            }
        }

        public String ToHour1
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_HOUR1"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_HOUR1", value);
                updateFlag();

                NotifyPropertyChanged();
            }
        }

        public String ToMinute1
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_MINUTE1"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_MINUTE1", value);
                updateFlag();

                NotifyPropertyChanged();
            }
        }

        public override bool IsEnabled
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (GetDbObject().GetFieldValue("ENABLE_FLAG").Equals("Y"));
            }

            set
            {
                String tmp = "N";
                if (value)
                {
                    tmp = "Y";
                }

                GetDbObject().SetFieldValue("ENABLE_FLAG", tmp);
                updateFlag();

                NotifyPropertyChanged();
            }
        }
    }
}
