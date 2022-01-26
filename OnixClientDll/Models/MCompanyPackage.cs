using Wis.WsClientAPI;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MCompanyPackage : MBaseModel
    {
        private ObservableCollection<MCompanyPackage> groupingPackage = new ObservableCollection<MCompanyPackage>();
        private ObservableCollection<MCompanyPackage> pricingPackages = new ObservableCollection<MCompanyPackage>();
        private ObservableCollection<MCompanyPackage> discountPackages = new ObservableCollection<MCompanyPackage>();
        private ObservableCollection<MCompanyPackage> finalDiscountPackages = new ObservableCollection<MCompanyPackage>();
        private ObservableCollection<MCompanyPackage> postGiftPackages = new ObservableCollection<MCompanyPackage>();
        private ObservableCollection<MCompanyPackage> trayPricePackages = new ObservableCollection<MCompanyPackage>();
        private ObservableCollection<MCompanyPackage> trayGroupPackages = new ObservableCollection<MCompanyPackage>();

        private MPackage vpk = new MPackage(new CTable("PACKAGE"));

        private Boolean forDelete = false;
        private String oldFlag = "";

        private Boolean forDelete_CompanySelected = false;
        private String oldFlag_CompanySelected = "";

        public MCompanyPackage(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

        }

        public String PackageTypeIcon
        {
            get
            {
                if (PackageType.Equals("1"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/standard-package-icon.png");
                }
                else if (PackageType.Equals("2"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/gift-icon.png");
                }
                else if (PackageType.Equals("3"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/discount-icon.png");
                }
                else if (PackageType.Equals("5"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/bundle-icon.png");
                }
                else if (PackageType.Equals("6"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/discount-icon.png");
                }
                else if (PackageType.Equals("8"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/warehouse-icon.png");
                }
                else if (PackageType.Equals("9"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/gift-icon.png");
                }
                else if (PackageType.Equals("10"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/bundle-icon.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/coupon-icon.png");
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

        public String EnableFlage
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

        public String CompanyPackageID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMPANY_PACKAGE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMPANY_PACKAGE_ID", value);
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

        public void InitChildItems()
        {
            InitChildItems("1", groupingPackage);
            InitChildItems("2", pricingPackages);
            InitChildItems("3", discountPackages);
            InitChildItems("4", finalDiscountPackages);
            InitChildItems("5", postGiftPackages);
            InitChildItems("6", trayPricePackages);
            InitChildItems("7", trayGroupPackages);
        }

        public void InitChildItems(String groupNo, ObservableCollection<MCompanyPackage> items)
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("COMPANY_PACKAGE_ITEM");

            if (arr == null)
            {
                return;
            }

            items.Clear();
            foreach (CTable t in arr)
            {
                MCompanyPackage v = new MCompanyPackage(t);
                if (v.PackageGroup.Equals(groupNo))
                {
                    items.Add(v);
                }

                v.ExtFlag = "I";
            }
        }

        public void Addtems(String nameTag, MCompanyPackage vw, MPackage cv)
        {
            if (PackageGroup.Equals("1"))
            {
                AddChildItems(groupingPackage, vw, cv);
            }
            else if (PackageGroup.Equals("2"))
            {
                AddChildItems(pricingPackages, vw, cv);
            }
            else if (PackageGroup.Equals("3"))
            {
                AddChildItems(discountPackages, vw, cv);
            }
            else if (PackageGroup.Equals("4"))
            {
                AddChildItems(finalDiscountPackages, vw, cv);
            }
            else if (PackageGroup.Equals("5"))
            {
                AddChildItems(postGiftPackages, vw, cv);
            }
            else if (PackageGroup.Equals("6"))
            {
                AddChildItems(trayPricePackages, vw, cv);
            }
            else if (PackageGroup.Equals("7"))
            {
                AddChildItems(trayGroupPackages, vw, cv);
            }
        }

        public void AddChildItems(ObservableCollection<MCompanyPackage> items, MCompanyPackage vw, MPackage cv)
        {
            CTable t = new CTable("COMPANY_PACKAGE");
            t.CopyFrom(cv.GetDbObject());
            
            MCompanyPackage v = new MCompanyPackage(t);
            v.CompanyPackageID = "";
            v.CompanyID = "1";
            v.SeqNo = (v.GetMaxSeqNo((items)) + 1).ToString();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COMPANY_PACKAGE_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("COMPANY_PACKAGE_ITEM", arr);
            }
           
            arr.Add(t);
            items.Add(v);

            v.ExtFlag = "A";
        }

        public ObservableCollection<MCompanyPackage> GetItemByName(String name)
        {
            if (name.Equals("pkg_group_grouping"))
            {
                return (GroupingPackage);
            }
            else if (name.Equals("pkg_group_pricing"))
            {
                return (PricingPackage);
            }
            else if (name.Equals("pkg_group_discount"))
            {
                return (DiscountPackage);
            }
            else if (name.Equals("pkg_group_final_discount"))
            {
                return (FinalDiscountPackage);
            }
            else if (name.Equals("post_gift"))
            {
                return (PostGiftPackage);
            }
            else if (name.Equals("tray_package_price"))
            {
                return (TrayPricePackage);
            }
            else if (name.Equals("tray_package_group"))
            {
                return (TrayPackageGroup);
            }

            return (null);
        }

        #region Package
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

        public String PackageGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_GROUP", value);
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

        public int GetMaxSeqNo(ObservableCollection<MCompanyPackage> CollecSeqNo)
        {
            int maxSeq = 0;
            foreach (MCompanyPackage v in CollecSeqNo)
            {
                int SeqN = CUtil.StringToInt(v.SeqNo.ToString());
                if (SeqN > maxSeq)
                {
                    maxSeq = SeqN;
                }
            }
            return maxSeq;
        }
        #endregion

        #region PackageObject
        public ObservableCollection<MCompanyPackage> GroupingPackage
        {
            get
            {
                return (groupingPackage);
            }
        }

        public ObservableCollection<MCompanyPackage> PricingPackage
        {
            get
            {
                return (pricingPackages);
            }
        }

        public ObservableCollection<MCompanyPackage> DiscountPackage
        {
            get
            {
                return (discountPackages);
            }
        }

        public ObservableCollection<MCompanyPackage> FinalDiscountPackage
        {
            get
            {
                return (finalDiscountPackages);
            }
        }

        public ObservableCollection<MCompanyPackage> PostGiftPackage
        {
            get
            {
                return (postGiftPackages);
            }
        }

        public ObservableCollection<MCompanyPackage> TrayPricePackage
        {
            get
            {
                return (trayPricePackages);
            }
        }

        public ObservableCollection<MCompanyPackage> TrayPackageGroup
        {
            get
            {
                return (trayGroupPackages);
            }
        }

        #endregion
    }
}
