
using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MMasterRef : MBaseModel
    {
        public MMasterRef(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("code", Code);
            ttItems.Add(ct);

            ct = new CToolTipItem("description", Description);
            ttItems.Add(ct);

            ct = new CToolTipItem("reference_type", RefTypeName);
            ttItems.Add(ct);
        }

        public String Code
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String Description
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
                NotifyPropertyChanged();
            }
        }

        public String Optional
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OPTIONAL"));
            }

            set
            {
                GetDbObject().SetFieldValue("OPTIONAL", value);
                NotifyPropertyChanged();
            }
        }

        public String OptionalEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OPTIONAL_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("OPTIONAL_ENG", value);
                NotifyPropertyChanged();
            }
        }

        public String DescriptionEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DESCRIPTION_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("DESCRIPTION_ENG", value);
                NotifyPropertyChanged();
            }
        }

        public String EnabledFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENABLE_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENABLE_FLAG", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsSelected
        {
            get
            {
                String flag = EnabledFlag;
                if (flag.Equals(""))
                {
                    return (false);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                EnabledFlag = flag;
                NotifyPropertyChanged();
            }
        }

        public String RefType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("REF_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_TYPE", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("RefTypeName");
            }
        }

        public String RefTypeName
        {
            get
            {
                if (RefType.Equals(""))
                {
                    return ("");
                }

                MasterRefEnum dt = (MasterRefEnum)Int32.Parse(RefType);
                String str = CUtil.MasterRefTypeToString(dt);

                return (str);
            }

            set
            {

            }
        }

        public MMasterRef RefTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                RefType = m.MasterID;
                RefTypeName = m.Description;
            }
        }

        public String MasterID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("MASTER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("MASTER_ID", value);
                NotifyPropertyChanged();
            }
        }
    }
}
