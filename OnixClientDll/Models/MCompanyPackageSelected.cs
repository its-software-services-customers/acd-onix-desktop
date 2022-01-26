using Wis.WsClientAPI;
using System;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MCompanyPackageSelected : MBaseModel
    {
        public MCompanyPackageSelected(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

        }

        public String CompanyPackageSelectID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMPANY_PKGSEL_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMPANY_PKGSEL_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String CompanyID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMPANY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMPANY_ID", value);
                NotifyPropertyChanged();
            }
        }

        public MMasterRef PackageTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                PackageType = m.MasterID;
                PackageTypeName = m.Description;
            }
        }

        public String PackageType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String EnableFlag
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
            }
        }

        public String PackageTypeName
        {
            get
            {
                if (PackageType.Equals(""))
                {
                    return ("");
                }
                String str = CUtil.PackageTypeToString(PackageType);
                return (str);
            }

            set
            {

            }
        }

        public String SequenceNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SEQUENCE_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("SEQUENCE_NO", value);
                NotifyPropertyChanged();
            }
        }

        public bool IsCompanySelected
        {
            get
            {
                return (EnableFlag.Equals("Y"));
            }

            set
            {
                if (value)
                {
                    EnableFlag = "Y";
                }
                else
                {
                    EnableFlag = "N";
                }

                updateFlag();
            }
        }
    }
}
