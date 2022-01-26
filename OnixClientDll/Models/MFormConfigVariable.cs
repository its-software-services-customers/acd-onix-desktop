using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MFormConfigVariable : MBaseModel
    {
        public MFormConfigVariable(CTable obj) : base(obj)
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

        public String VariableType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VariableType"));
            }
            set
            {
                GetDbObject().SetFieldValue("VariableType", value);
                NotifyPropertyChanged();
            }
        }

        public String VariableTypeName
        {
            get
            {
                return (CLanguage.getValue(VariableType));
            }

            set
            {
            }
        }

        public String SystemVariableFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SYSTEM_VARIABLE_FLAG"));
            }
            set
            {
                GetDbObject().SetFieldValue("SYSTEM_VARIABLE_FLAG", value);
                NotifyPropertyChanged();
            }
        }

        public String OVerridedFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OVERRIDED_FLAG"));
            }
            set
            {
                GetDbObject().SetFieldValue("OVERRIDED_FLAG", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsOverrided
        {
            get
            {
                return (OVerridedFlag.Equals("Y"));
            }
        }

        //public String Language
        //{
        //    get
        //    {
        //        if (GetDbObject() == null)
        //        {
        //            return ("");
        //        }

        //        return (GetDbObject().GetFieldValue("LANGUAGE"));
        //    }
        //    set
        //    {
        //        GetDbObject().SetFieldValue("LANGUAGE", value);
        //        NotifyPropertyChanged();
        //    }
        //}

        public String Scope
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SCOPE"));
            }
            set
            {
                GetDbObject().SetFieldValue("SCOPE", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsEditable
        {
            get;
            set;
        }

        public MMasterRef VariableTypeObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                VariableType = m.Code;
                updateFlag();
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = (MMasterRef) CUtil.IDToObject(CMasterReference.Instance.VariableTypes, "Code", VariableType);
                return (mr);
            }
        }
    }
}
