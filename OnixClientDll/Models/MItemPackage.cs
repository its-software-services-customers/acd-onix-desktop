using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MItemPackage : MBaseModel
    {
        public MItemPackage(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

        }

        public String PackageID
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
            }
        }

        public String PackageCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String PackageName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public bool IsDeleted
        {
            get
            {
                return (ExtFlag.Equals("D"));
            }

            set
            {
                bool forDelete = value;
                if (forDelete)
                {
                    OldExtFlag = ExtFlag;
                    ExtFlag = "D";
                }
                else
                {
                    ExtFlag = OldExtFlag;
                }
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

        public string OldExtFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OLD_EXT_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("OLD_EXT_FLAG", value);
            }
        }

        public void UpdateFlag()
        {
            if (!ExtFlag.Equals("A") && !ExtFlag.Equals("D"))
            {
                ExtFlag = "E";
            }
            else
            {
                OldExtFlag = "E";
            }
        }

        public String PackageGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_GROUP", value);
                NotifyPropertyChanged();
            }
        }

        #region Package Type

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

        public String DiscountPct
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DISCOUNT_PCT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_PCT", value);
                NotifyPropertyChanged();
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

#endregion

        #region EffectiveDate
        public DateTime EffectiveDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("EFFECTIVE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("EFFECTIVE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String EffectiveDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("EFFECTIVE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);

            }

            set
            {
            }
        }

        public String ExcludePackageIDs
        {
            set
            {
                //Set value like "(1, 3, 4, 5)"
                GetDbObject().SetFieldValue("EXCLUDE_ID_SET", value);
            }
        }

        public DateTime FromEffectiveDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_EFFECTIVE_DATE", str);
            }
        }


        public DateTime ToEffectiveDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_EFFECTIVE_DATE", str);
            }
        }
#endregion

        #region ExpireDate
        public DateTime ExpireDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("EXPIRE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("EXPIRE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String ExpireDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("EXPIRE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);

            }

            set
            {
            }
        }

        public DateTime FromExpireDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_EXPIRE_DATE", str);
            }
        }

        public DateTime ToExpireDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_EXPIRE_DATE", str);
            }
        }
        #endregion

        public string SequenceNo
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
            }
        }
    }
}
