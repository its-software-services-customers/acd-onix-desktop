using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;

namespace Onix.ClientCenter.UI.HumanResource.OTDocument
{
    public class MVOTRate : MBaseModel
    {
        public MVOTRate(CTable obj) : base(obj)
        {
        }

        public MVOTRate(String day, String fromHH, String fromMM, String toHH, String toMM, String multiplier) : base(new CTable(""))
        {
            FromDayOfWeek = day;
            Multiplier = multiplier;
            FromTimeHH = fromHH;
            FromTimeMM = fromMM;
            ToTimeHH = toHH;
            ToTimeMM = toMM;
        }

        #region

        public String Multiplier
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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

        #endregion       
    }
}
