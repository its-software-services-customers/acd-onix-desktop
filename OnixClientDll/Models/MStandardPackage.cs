using System;
using System.Collections.ObjectModel;
using System.Collections;

using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class VStandardPackage : MBaseModel
    {
        private ObservableCollection<MBaseModel> pricingByItemAndType = new ObservableCollection<MBaseModel>();
        private ObservableCollection<MBaseModel> servicePackages = new ObservableCollection<MBaseModel>();
        private ObservableCollection<MBaseModel> bonusPackages = new ObservableCollection<MBaseModel>();
        private ObservableCollection<MBaseModel> discountPackages = new ObservableCollection<MBaseModel>();

        private MPackage vpk = new MPackage(new CTable(""));

        private Boolean forDelete = false;
        private String oldFlag = "";

        public VStandardPackage(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

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

        public void InitChildItems()
        {
            InitChildItems("STANDARD_ITEM_PACKAGE_ITEM", pricingByItemAndType);
            InitChildItems("STANDARD_SERVICE_PACKAGE_ITEM", servicePackages);
            InitChildItems("STANDARD_BONUS_PACKAGE_ITEM", bonusPackages);
            InitChildItems("STANDARD_DISCOUNT_PACKAGE_ITEM", discountPackages);
        }

        public ObservableCollection<MBaseModel> GetItemByName(String name)
        {
            if (name.Equals("STANDARD_ITEM_PACKAGE_ITEM"))
            {
                return (PricingByItemAndType);
            }
            else if (name.Equals("STANDARD_SERVICE_PACKAGE_ITEM"))
            {
                return (ServicePackages);
            }
            else if (name.Equals("STANDARD_BONUS_PACKAGE_ITEM"))
            {
                return (BonusPackages);
            }
            else if (name.Equals("STANDARD_DISCOUNT_PACKAGE_ITEM"))
            {
                return (DiscountPackages);
            }

            return (null);
        }

        #region PackagePrice
        public ObservableCollection<MBaseModel> PricingByItemAndType
        {
            get
            {
                return (pricingByItemAndType);
            }
        }

        public MBaseModel PackagePriceObj
        {
            get
            {
                vpk.PackageID = PackageID;
                vpk.PackageCode = PackageCode;
                vpk.PackageCode = PackageName;

                return (vpk);
            }

            set
            {
                if (value == null)
                {
                    PackageID = "";
                    PackageCode = "";
                    PackageName = "";

                    return;
                }

                MPackage ii = (value as MPackage);
                vpk.SetDbObject(ii.GetDbObject());

                PackageID = ii.PackageID;
                PackageCode = ii.PackageCode;
                PackageName = ii.PackageName;

                updateFlag();
            }
        }
        #endregion

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
    }
}
