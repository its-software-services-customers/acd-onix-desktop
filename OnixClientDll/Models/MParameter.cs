using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MParameter : MBaseModel
    {
        public MParameter(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {
        }

        public String ParamID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PARAM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PARAM_ID", value);
            }
        }

        public String ParamName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PARAM_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PARAM_NAME", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ParamValue
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PARAM_VALUE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PARAM_VALUE", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ParamNameSpace
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PARAM_NS"));
            }

            set
            {
                GetDbObject().SetFieldValue("PARAM_NS", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String UserName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("USER_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("USER_NAME", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }
    }
}
