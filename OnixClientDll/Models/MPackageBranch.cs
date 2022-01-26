using System;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MPackageBranch : MBaseModel
    {
        private MMasterRef br = new MMasterRef(new CTable(""));
        private int seq = 0;

        public MPackageBranch(CTable obj) : base(obj)
        {
        }

        public String Code
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_CODE", value);
            }
        }

        public String Name
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_NAME", value);
            }
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

        public String PackageId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_ID", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String BranchId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_ID", value);

                updateFlag();
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

                updateFlag();
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
    }
}
