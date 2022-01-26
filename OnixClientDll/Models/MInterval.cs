using System;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MInterval : MBaseModel
    {
        private Boolean isFirst = false;
        private String oldToValue = "";
        private double gap = 0.00;
        private double used = 0.00;
        private int rptcount = 0;

        public MInterval(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

        }

        public String IntervalID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INTERVAL_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("INTERVAL_ID", value);
            }
        }

        public String FromValue
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_VALUE"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_VALUE", value);
                NotifyPropertyChanged();
            }
        }

        public String ToValue
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_VALUE"));
            }

            set
            {
                oldToValue = GetDbObject().GetFieldValue("TO_VALUE");

                GetDbObject().SetFieldValue("TO_VALUE", value);
                NotifyPropertyChanged();
            }
        }

        public String OldToValue
        {
            get
            {
                return (oldToValue);
            }

            set
            {
            }
        }

        public String ConfigValue
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CONFIG_VALUE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CONFIG_VALUE", value);
                NotifyPropertyChanged();
            }
        }

        public double Gap
        {
            get
            {
                return (gap);
            }

            set
            {
                gap = value;
            }
        }

        public double Used
        {
            get
            {
                return (used);
            }

            set
            {
                used = value;
            }
        }

        public int RepeatCount
        {
            get
            {
                return (rptcount);
            }

            set
            {
                rptcount = value;
            }
        }

        public Boolean IsNotFirst
        {
            get
            {
                return (!isFirst);
            }

            set
            {
                isFirst = !value;
            }
        }
    }
}
