using Onix.Client.Helper;
using Onix.Client.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wis.WsClientAPI;

namespace Onix.Client.Models
{
    public class MCompanyCommissionProfile : MBaseModel
    {
        private ObservableCollection<MCompanyCommissionProfile> commByItem = new ObservableCollection<MCompanyCommissionProfile>();
        private ObservableCollection<MCompanyCommissionProfile> commByGroup = new ObservableCollection<MCompanyCommissionProfile>();

        private Boolean forDelete = false;
        private String oldFlag = "";

        private Boolean forDelete_CompanySelected = false;
        private String oldFlag_CompanySelected = "";

        public MCompanyCommissionProfile(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {
        }

        public ObservableCollection<MCompanyCommissionProfile> CommissionByItem
        {
            get
            {
                return (commByItem);
            }
        }

        public ObservableCollection<MCompanyCommissionProfile> CommissionByGroup
        {
            get
            {
                return (commByGroup);
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
                if (forDelete)
                {
                    oldFlag = ExtFlag;
                    ExtFlag = "D";
                }
                else
                {
                    ExtFlag = oldFlag;
                }
            }
        }

        public bool IsCompanySelected
        {
            get
            {
                return (forDelete_CompanySelected);
            }

            set
            {
                forDelete_CompanySelected = value;
                if (forDelete_CompanySelected)
                {
                    oldFlag_CompanySelected = ExtFlag;
                }
                else
                {
                    ExtFlag = "E";
                }
            }
        }

        public String CompanyCommProfID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMPANY_COMMPROF_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMPANY_COMMPROF_ID", value);
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
            }
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
        

        public String SeqNo
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
            }
        }

        #region CommissionProfile
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
        #endregion

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
        #endregion

        public String TimeSpecificFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TIME_SPECIFIC_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("TIME_SPECIFIC_FLAG", value);
                NotifyPropertyChanged();
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
                updateFlag();
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
                updateFlag();
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

        public int GetMaxSeqNo(ObservableCollection<MCompanyCommissionProfile> CollecSeqNo)
        {
            int maxSeq = 0;
            foreach (MCompanyCommissionProfile v in CollecSeqNo)
            {
                int SeqN = CUtil.StringToInt(v.SeqNo.ToString());
                if (SeqN > maxSeq)
                {
                    maxSeq = SeqN;
                }
            }
            return maxSeq;
        }

        public void InitChildItems()
        {
            InitChildItems("1", commByItem);
            InitChildItems("2", commByGroup);

        }

        public void InitChildItems(String typeNo, ObservableCollection<MCompanyCommissionProfile> items)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COMPANY_COMM_PROFILE_ITEM");

            if (arr == null)
            {
                return;
            }

            items.Clear();
            foreach (CTable t in arr)
            {
                MCompanyCommissionProfile v = new MCompanyCommissionProfile(t);
                if (v.ProfileType.Equals(typeNo))
                {
                    items.Add(v);
                }
                v.ExtFlag = "I";
            }

        }

        public void Addtems(String nameTag, MCompanyCommissionProfile vw, MCommissionProfile cv)
        {
            if (ProfileType.Equals("1"))
            {
                AddChildItems(commByItem, vw, cv);
            }
            else if (ProfileType.Equals("2"))
            {
                AddChildItems(commByGroup, vw, cv);
            }
        }

        public void AddChildItems(ObservableCollection<MCompanyCommissionProfile> items, MCompanyCommissionProfile vw, MCommissionProfile cv)
        {
            CTable t = new CTable("COMPANY_COMM_PROFILE");
            t.CopyFrom(cv.GetDbObject());

            MCompanyCommissionProfile v = new MCompanyCommissionProfile(t);
            v.CompanyCommProfID = "";
            v.CompanyID = "1";
            v.SeqNo = (v.GetMaxSeqNo((items)) + 1).ToString();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COMPANY_COMM_PROFILE_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("COMPANY_COMM_PROFILE_ITEM", arr);
            }

            arr.Add(t);
            items.Add(v);

            v.ExtFlag = "A";
        }

        public ObservableCollection<MCompanyCommissionProfile> GetItemByName(String name)
        {
            if (name.Equals("COMPANY_COMM_PROFILE_BY_ITEM"))
            {
                return (CommissionByItem);
            }
            else if (name.Equals("COMPANY_COMM_PROFILE_BY_GROUP"))
            {
                return (CommissionByGroup);
            }

            return (null);
        }
    }
}
