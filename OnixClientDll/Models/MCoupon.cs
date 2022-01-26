using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MCoupon : MBaseModel
    {
        private Boolean forDelete = false;

        public MCoupon(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

        }

        public String CouponID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COUPON_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COUPON_ID", value);
            }
        }

        public String CouponCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COUPON_NUMBER"));
            }

            set
            {
                GetDbObject().SetFieldValue("COUPON_NUMBER", value);
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public String DocumentNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String CouponDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COUPON_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("COUPON_DESC", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean? IsUsed
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_USED_FLAG");
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
                GetDbObject().SetFieldValue("IS_USED_FLAG", flag);
                NotifyPropertyChanged();
            }
        }



        public String IsUsedIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_USED_FLAG");
                if (flag.Equals("Y"))
                {

                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }

        public DateTime ToDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_DOCUMENT_DATE", str);
            }
        }

        public DateTime FromDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_DOCUMENT_DATE", str);
            }
        }


        public Boolean? IsAssigned
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_ASSIGNED_FLAG");
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
                GetDbObject().SetFieldValue("IS_ASSIGNED_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public String IsAssignedIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_ASSIGNED_FLAG");
                if (flag.Equals("Y"))
                {

                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }

        public String CouponAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COUPON_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("COUPON_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String CouponAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(CouponAmount));
            }
        }

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

        public bool IsDeleted
        {
            get
            {
                return (forDelete);
            }

            set
            {
                forDelete = value;
            }
        }
    }
}
