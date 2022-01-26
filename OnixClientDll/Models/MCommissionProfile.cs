using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Models;

namespace Onix.Client.Model
{
    public class MCommissionProfile : MBaseModel
    {
        private ObservableCollection<MCommissionProfileDetail> commisionByItemDetails = new ObservableCollection<MCommissionProfileDetail>();
        private ObservableCollection<MCommissionProfileDetail> commisionByGroupDetails = new ObservableCollection<MCommissionProfileDetail>();

        private int internalSeq = 0;

        public MCommissionProfile(CTable obj) : base(obj)
        {
            commisionByItemDetails = new ObservableCollection<MCommissionProfileDetail>();
            commisionByGroupDetails = new ObservableCollection<MCommissionProfileDetail>();
        }

        public void CreateDefaultValue()
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("comm_profile_code", ProfileCode);
            ttItems.Add(ct);

            ct = new CToolTipItem("comm_profile_name", ProfileDesc);
            ttItems.Add(ct);

            ct = new CToolTipItem("comm_profile_type", ProfileTypeName);
            ttItems.Add(ct);
        }

        public void CopyCommissionInfo(MCompanyCommissionProfile ip)
        {
            CommProfileID = ip.CommProfileID;
            ProfileCode = ip.ProfileCode;
            ProfileDesc = ip.ProfileDesc;
            ProfileType = ip.ProfileType;
            ProfileTypeName = ip.ProfileTypeName;
        }

        public String CommProfileID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMMISSION_PROF_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISSION_PROF_ID", value);
            }
        }

        public String ProfileCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROFILE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROFILE_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String ProfileDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROFILE_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROFILE_DESC", value);
                NotifyPropertyChanged();
            }
        }

        public String CommissionDefinition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMMISSION_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISSION_DEFINITION", value);
                updateFlag();
                NotifyPropertyChanged("PriceConfigIcon");
            }
        }

        public String PriceConfigIcon
        {
            get
            {
                if (CommissionDefinition.Equals(""))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/error_24.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/ok-icon.png");
            }

            set
            {
            }
        }
        public new Boolean? IsEnabled
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ENABLE_FLAG");
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
                GetDbObject().SetFieldValue("ENABLE_FLAG", flag);
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

        public String ProfileTypeIcon
        {
            get
            {
                if (ProfileType.Equals("1"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/standard-package-icon.png");
                }
                else if (ProfileType.Equals("2"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/gift-icon.png");
                }
                return ("pack://application:,,,/OnixClient;component/Images/coupon-icon.png");
            }
        }

        public Boolean? IsEmployeeSpecific
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("EMPLOYEE_SPECIFIC_FLAG");
                if (flag.Equals(""))
                {
                    return (false);
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
                GetDbObject().SetFieldValue("EMPLOYEE_SPECIFIC_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean? IsProductSpecific
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("PRODUCT_SPECIFIC_FLAG");
                if (flag.Equals(""))
                {
                    return (false);
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
                GetDbObject().SetFieldValue("PRODUCT_SPECIFIC_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        #region Commission Pfofile Type

        public MMasterRef ProfileTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                ProfileType = m.MasterID;
                ProfileTypeName = m.Description;
            }
        }

        public String ProfileType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("PROFILE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROFILE_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String ProfileTypeName
        {
            get
            {
                if (ProfileType.Equals(""))
                {
                    return ("");
                }

                String str = CUtil.CommissionProfileToString(ProfileType);

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

        public String DayEffective
        {
            set
            {
                GetDbObject().SetFieldValue("DAY_EFFECTIVE", value);
            }
        }

        public String DayExpire
        {
            set
            {
                GetDbObject().SetFieldValue("DAY_EXPIRE", value);
            }
        }

        #region commisioByItemnDetails
        public void InitCommisionByItemDetails()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COMMISSION_DETAIL_ITEM");

            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("COMMISSION_DETAIL_ITEM", arr);
            }

            foreach (CTable t in arr)
            {
                MCommissionProfileDetail v = new MCommissionProfileDetail(t);
                commisionByItemDetails.Add(v);

                v.Seq = internalSeq;
                internalSeq++;
                v.ExtFlag = "I";
            }
        }

        public void AddCommissionByItemDetail(MCommissionProfileDetail vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COMMISSION_DETAIL_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("COMMISSION_DETAIL_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            commisionByItemDetails.Add(vp);

            vp.Seq = internalSeq;
            internalSeq++;

            vp.ExtFlag = "A";
        }

        public void RemoveCommissionByItemDetail(MCommissionProfileDetail vp)
        {
            removeAssociateItems(vp, "COMMISSION_DETAIL_ITEM", "INTERNAL_SEQ", "COMMISSION_PDETL_ID");
            commisionByItemDetails.Remove(vp);
        }

        public ObservableCollection<MCommissionProfileDetail> CommissionProfByItemDetails
        {
            get
            {
                return (commisionByItemDetails);
            }
        }
        #endregion

        #region commisioByGroupDetails
        public void InitCommisionByGroupDetails()
        {
            //commisionByGroupDetails.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COMMISSION_DETAIL_ITEM");

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                MCommissionProfileDetail v = new MCommissionProfileDetail(t);
                commisionByGroupDetails.Add(v);

                v.Seq = internalSeq;
                internalSeq++;
                v.ExtFlag = "I";
            }
        }

        public void AddCommissionByGroupDetail(MCommissionProfileDetail vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COMMISSION_DETAIL_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("COMMISSION_DETAIL_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            commisionByGroupDetails.Add(vp);

            vp.Seq = internalSeq;
            internalSeq++;

            vp.ExtFlag = "A";
        }

        public void RemoveCommissionByGroupDetail(MCommissionProfileDetail vp)
        {
            removeAssociateItems(vp, "COMMISSION_DETAIL_ITEM", "INTERNAL_SEQ", "COMMISSION_PDETL_ID");
            commisionByGroupDetails.Remove(vp);
        }

        public ObservableCollection<MCommissionProfileDetail> CommissionProfByGroupDetails
        {
            get
            {
                return (commisionByGroupDetails);
            }
        }
        #endregion
    }
}
