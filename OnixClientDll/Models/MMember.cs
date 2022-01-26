using System;
using System.Collections;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MMember : MBaseModel
    {
        private ObservableCollection<MBaseModel> pricingByItemAndType = new ObservableCollection<MBaseModel>();
        private ObservableCollection<MBaseModel> servicePackages = new ObservableCollection<MBaseModel>();
        private ObservableCollection<MBaseModel> bonusPackages = new ObservableCollection<MBaseModel>();
        private ObservableCollection<MBaseModel> discountPackages = new ObservableCollection<MBaseModel>();

        public MMember(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

        }

        public String MemberID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MEMBER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("MEMBER_ID", value);
            }
        }

        public String MemberNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MEMBER_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("MEMBER_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String MemberDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MEMBER_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("MEMBER_DESC", value);
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

        public Boolean? IsUsed
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("USED_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("USED_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public String IsUsedIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("USED_FLAG");
                if (flag.Equals("Y"))
                {

                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }

#region Member Type

        public MMasterRef MemberTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                MemberType = m.MasterID;
                MemberTypeName = m.Description;
            }
        }

        public String MemberType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("MEMBER_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("MEMBER_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String MemberTypeName
        {
            get
            {
                if (MemberType.Equals(""))
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MEMBER_TYPE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("MEMBER_TYPE_NAME", value);
                NotifyPropertyChanged();
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

        public String EffectiveWithinDate
        {
            set
            {
                GetDbObject().SetFieldValue("EFFECTIVE_WITHIN_DATE", value);
            }
        }

        public String ExpireWithinDate
        {
            set
            {
                GetDbObject().SetFieldValue("EXPIRE_WITHIN_DATE", value);
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

        public String CustomerID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_ID", value);
            }
        }

        public String CustomerName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_NAME", value);
            }
        }

        #region Package Items

        public void InitChildItems(String itemName, ObservableCollection<MBaseModel> items)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray(itemName);

            if (arr == null)
            {
                return;
            }

            items.Clear();
            foreach (CTable t in arr)
            {
                MItemPackage v = new MItemPackage(t);
                items.Add(v);

                v.ExtFlag = "I";
            }
        }

        public void InitChildItems()
        {
            InitChildItems("MEMBER_ITEM_PACKAGE_ITEM", pricingByItemAndType);
            InitChildItems("MEMBER_SERVICE_PACKAGE_ITEM", servicePackages);
            InitChildItems("MEMBER_BONUS_PACKAGE_ITEM", bonusPackages);
            InitChildItems("MEMBER_DISCOUNT_PACKAGE_ITEM", discountPackages);
        }

        public ObservableCollection<MBaseModel> GetItemByName(String name)
        {
            if (name.Equals("MEMBER_ITEM_PACKAGE_ITEM"))
            {
                return (PricingByItemAndType);
            }
            else if (name.Equals("MEMBER_SERVICE_PACKAGE_ITEM"))
            {
                return (ServicePackages);
            }
            else if (name.Equals("MEMBER_BONUS_PACKAGE_ITEM"))
            {
                return (BonusPackages);
            }
            else if (name.Equals("MEMBER_DISCOUNT_PACKAGE_ITEM"))
            {
                return (DiscountPackages);
            }

            return (null);
        }

        public ObservableCollection<MBaseModel> PricingByItemAndType
        {
            get
            {
                return (pricingByItemAndType);
            }
        }

        public ObservableCollection<MBaseModel> ServicePackages
        {
            get
            {
                return (servicePackages);
            }
        }

        public ObservableCollection<MBaseModel> BonusPackages
        {
            get
            {
                return (bonusPackages);
            }
        }

        public ObservableCollection<MBaseModel> DiscountPackages
        {
            get
            {
                return (discountPackages);
            }
        }
#endregion

    }
}
