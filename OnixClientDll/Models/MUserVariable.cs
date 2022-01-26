using System;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MUserVariable : MBaseModel
    {
        public MUserVariable(CTable obj) : base(obj)
        {

        }

        public String VariableID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VARIABLE_ID"));
            }
            set
            {
                GetDbObject().SetFieldValue("VARIABLE_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String VariableName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VARIABLE_NAME"));
            }
            set
            {
                GetDbObject().SetFieldValue("VARIABLE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String VariableValue
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VARIABLE_VALUE"));
            }
            set
            {
                GetDbObject().SetFieldValue("VARIABLE_VALUE", value);
                NotifyPropertyChanged();
            }
        }

        public String UserID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("USER_ID"));
            }
            set
            {
                GetDbObject().SetFieldValue("USER_ID", value);
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
                NotifyPropertyChanged();
            }
        }
    }
}
