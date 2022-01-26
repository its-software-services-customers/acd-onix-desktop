using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Helper;
using Onix.Client.Controller;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MPackage : MBaseModel
    {
        private ObservableCollection<MPeriod> periods = new ObservableCollection<MPeriod>();
        private ObservableCollection<MPackagePrice> package_itemPrice = new ObservableCollection<MPackagePrice>();
        private ObservableCollection<MPackageCustomer> package_customerFilter = new ObservableCollection<MPackageCustomer>();
        private ObservableCollection<MPackageDiscount> package_discountFilter = new ObservableCollection<MPackageDiscount>();
        private ObservableCollection<MPackageBonus> package_bonusFilterBuy = new ObservableCollection<MPackageBonus>();
        private ObservableCollection<MPackageBonus> package_bonusFilterFree = new ObservableCollection<MPackageBonus>();
        private ObservableCollection<MPackageVoucher> package_voucherBuy = new ObservableCollection<MPackageVoucher>();
        private ObservableCollection<MPackageVoucher> package_voucherFree = new ObservableCollection<MPackageVoucher>();
        private ObservableCollection<MPackageBundle> package_bundles = new ObservableCollection<MPackageBundle>();
        private ObservableCollection<MPackageFinalDiscount> package_final_discounts = new ObservableCollection<MPackageFinalDiscount>();
        private ObservableCollection<MPackageBranch> package_branches = new ObservableCollection<MPackageBranch>();
        private ObservableCollection<MPackageTrayPriceDiscount> tray_prices = new ObservableCollection<MPackageTrayPriceDiscount>();

        //Use MPackageVoucher as an item
        private ObservableCollection<MPackageVoucher> package_post_buys = new ObservableCollection<MPackageVoucher>();
        private ObservableCollection<MPackageVoucher> package_post_gifts = new ObservableCollection<MPackageVoucher>();

        private ObservableCollection<MPackageDiscount> packageTest = new ObservableCollection<MPackageDiscount>();

        private int priceItemSeq = 0;
        private int customerItemSeq = 0;
        private int bonusItemSeq = 0;
        private int internalSeq = 0;
        private int voucherItemSeq = 0;

        public MPackage(CTable obj) : base(obj)
        {
            periods = new ObservableCollection<MPeriod>();
        }

        public void CreateDefaultValue()
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("package_code", PackageCode);
            ttItems.Add(ct);

            ct = new CToolTipItem("package_name", PackageName);
            ttItems.Add(ct);

            ct = new CToolTipItem("package_type", PackageTypeName);
            ttItems.Add(ct);
        }

        public ObservableCollection<MPackagePrice> PackageItemPrices
        {
            get
            {
                return (package_itemPrice);
            }
            set { }
        }

        public void CopyPackageInfo(MCompanyPackage ip)
        {
            PackageID = ip.PackageID;
            PackageCode = ip.PackageCode;
            PackageName = ip.PackageName;
            PackageType = ip.PackageType;
            PackageTypeName = ip.PackageTypeName;
            PackageGroup = ip.PackageType; //PackageType same PackageGroup
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

        public Boolean? IsTimeSpecific
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("TIME_SPECIFIC_FLAG");
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
                GetDbObject().SetFieldValue("TIME_SPECIFIC_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean? IsCustomerSpecific
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("CUSTOMER_SPECIFIC_FLAG");
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
                GetDbObject().SetFieldValue("CUSTOMER_SPECIFIC_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean? IsBranchSpecific
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("BRANCH_SPECIFIC_FLAG");
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
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("BRANCH_SPECIFIC_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public String IsTimeSpecificIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("TIME_SPECIFIC_FLAG");
                if (flag.Equals("Y"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }

        public String BundleAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BUNDLE_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("BUNDLE_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String TrayName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TRAY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("TRAY_NAME", value);
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

        public String FromTime
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_TIME"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_TIME", value);
                NotifyPropertyChanged();
            }
        }

        public String ToTime
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_TIME"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_TIME", value);
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

        public Boolean IsPackageTypeNeed
        {
            get
            {
                if (PackageGroup.Equals("3"))
                {
                    return (false);
                }

                return (true);
            }

            set
            {
            }
        }


        public Visibility IsPackageTypeVisibility
        {
            get
            {
                if (PackageGroup.Equals("3"))
                {
                    return (Visibility.Collapsed);
                }

                return (Visibility.Visible);
            }

            set
            {
            }
        }

        #region Periods

        private CTable getObjectByDay(ArrayList arr, String day)
        {
            MPeriod p = new MPeriod(null);

            foreach (CTable t in arr)
            {
                p.SetDbObject(t);
                if (p.DayOfWeek.Equals(day))
                {
                    return (t);
                }
            }

            return (null);
        }

        private MPeriod getPeriodByDay(ObservableCollection<MPeriod> arr, String day)
        {
            foreach (MPeriod p in arr)
            {
                if (p.DayOfWeek.Equals(day))
                {
                    return (p);
                }
            }

            return (null);
        }

        public void InitPeriods()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_PERIOD_ITEM");

            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_PERIOD_ITEM", arr);
            }

            for (int d = 1; d <= 7; d++)
            {
                CTable dbo = getObjectByDay(arr, d.ToString());
                if (dbo == null)
                {
                    dbo = new CTable("PERIOD");
                    MPeriod p = new MPeriod(dbo);
                    p.ExtFlag = "A";
                    p.DayOfWeek = d.ToString();
                    p.CreateDefaultValue();

                    arr.Add(dbo);
                    periods.Add(p);
                }
                else
                {
                    MPeriod p = new MPeriod(dbo);
                    p.ExtFlag = "I";
                    periods.Add(p);
                }
            }
        }

        public MPeriod MondayPeriod
        {
            get
            {
                MPeriod p = getPeriodByDay(periods, "1");
                return (p);
            }

            set
            {
            }
        }

        public MPeriod TuesdayPeriod
        {
            get
            {
                MPeriod p = getPeriodByDay(periods, "2");
                return (p);
            }

            set
            {
            }
        }

        public MPeriod WednesdayPeriod
        {
            get
            {
                MPeriod p = getPeriodByDay(periods, "3");
                return (p);
            }

            set
            {
            }
        }

        public MPeriod ThursdayPeriod
        {
            get
            {
                MPeriod p = getPeriodByDay(periods, "4");
                return (p);
            }

            set
            {
            }
        }

        public MPeriod FridayPeriod
        {
            get
            {
                MPeriod p = getPeriodByDay(periods, "5");
                return (p);
            }

            set
            {
            }
        }

        public MPeriod SathurdayPeriod
        {
            get
            {
                MPeriod p = getPeriodByDay(periods, "6");
                return (p);
            }

            set
            {
            }
        }

        public MPeriod SundayPeriod
        {
            get
            {
                MPeriod p = getPeriodByDay(periods, "7");
                return (p);
            }

            set
            {
                //sunday = value;
            }
        }

        #endregion

        #region Customer
        public void InitPackageCustomers()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_CUSTOMER_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_CUSTOMER_ITEM", arr);
            }

            int cnt = 0;

            foreach (CTable t in arr)
            {
                MPackageCustomer v = new MPackageCustomer(t);
                package_customerFilter.Add(v);
                v.ExtFlag = "I";

                v.Seq = customerItemSeq;
                customerItemSeq++;

                cnt++;
            }
        }

        public void AddCustomerItem(MPackageCustomer vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_CUSTOMER_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_CUSTOMER_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_customerFilter.Add(vp);

            vp.Seq = customerItemSeq;
            customerItemSeq++;

            //Not use
            vp.ExtFlag = "A";
        }

        public void RemoveCustomerItem(MPackageCustomer vp)
        {
            removeAssociateItems(vp, "PACKAGE_CUSTOMER_ITEM", "INTERNAL_SEQ", "PACKAGE_CUSTOMER_ID");
            package_customerFilter.Remove(vp);
        }

        public ObservableCollection<MPackageCustomer> PackageCustomers
        {
            get
            {
                return (package_customerFilter);
            }
        }
        #endregion

        #region Discount

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

        public String DiscountBasketTypeConfig
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISCOUNT_BASKET_TYPE_CONFIG"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_BASKET_TYPE_CONFIG", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("DiscountBasketTypeConfigIcon");
            }
        }

        public String BasketConfigType
        {
            get
            {
                if (IsProductSpecific == false)
                {
                    return ("1");
                }

                if (DiscountMapType.Equals("0"))
                {
                    //Quantity
                    return ("1");
                }

                //Bundle will not know the amount individually
                return ("2");

            }

            set
            {
            }
        }

        public String PostGiftBasketConfigType
        {
            get
            {
                if (IsProductSpecific == false)
                {
                    return ("3");
                }

                //Bundle will not know the amount individually
                return ("4");

            }

            set
            {
            }
        }

        public String DiscountBasketTypeConfigIcon
        {
            get
            {
                if (DiscountBasketTypeConfig.Equals(""))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/error_24.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/ok-icon.png");
            }

            set
            {
            }
        }

        public String DiscountDefinition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DISCOUNT_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_DEFINITION", value);
                NotifyPropertyChanged("DiscountDefinitionConfigIcon");
                NotifyPropertyChanged();
            }
        }

        public String DiscountMapType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DISCOUNT_MAP_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_MAP_TYPE", value);
                NotifyPropertyChanged();
            }
        }


        public String DiscountDefinitionConfigIcon
        {
            get
            {
                if (DiscountDefinition.Equals(""))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/error_24.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/ok-icon.png");
            }

            set
            {
            }
        }

        public void InitPackageDiscountFilters()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_DISCOUNT_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_DISCOUNT_ITEM", arr);
            }

            int cnt = 0;

            foreach (CTable t in arr)
            {
                MPackageDiscount v = new MPackageDiscount(t);
                package_discountFilter.Add(v);
                v.ExtFlag = "I";

                v.Seq = priceItemSeq;
                priceItemSeq++;

                cnt++;
            }
        }

        public void AddDiscountItem(MPackageDiscount vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_DISCOUNT_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_DISCOUNT_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_discountFilter.Add(vp);

            vp.Seq = priceItemSeq;
            priceItemSeq++;

            vp.DiscountType = "1";
            vp.ExtFlag = "A";
        }

        public void RemoveDiscountItem(MPackageDiscount vp)
        {
            removeAssociateItems(vp, "PACKAGE_DISCOUNT_ITEM", "INTERNAL_SEQ", "PACKAGE_DISCOUNT_ID");
            package_discountFilter.Remove(vp);
        }

        public ObservableCollection<MPackageDiscount> PackageDiscount
        {
            get
            {
                return (package_discountFilter);
            }
        }
        #endregion

        #region Price

        public void InitItemsPrice()
        {
            package_itemPrice.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_ITEM_PRICE_ITEM");

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                MPackagePrice v = new MPackagePrice(t);
                package_itemPrice.Add(v);

                v.Seq = priceItemSeq;
                priceItemSeq++;
                v.ExtFlag = "I";
            }
        }

        public void AddPriceItem(MPackagePrice vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_ITEM_PRICE_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_ITEM_PRICE_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_itemPrice.Add(vp);

            vp.Seq = priceItemSeq;
            priceItemSeq++;

            //Not use
            vp.SeqNo = "1";
            vp.PricingType = "1";

            vp.ExtFlag = "A";
        }

        public void RemovePriceItem(MPackagePrice vp)
        {
            removeAssociateItems(vp, "PACKAGE_ITEM_PRICE_ITEM", "INTERNAL_SEQ", "PACKAGE_ITM_PRICE_ID");
            package_itemPrice.Remove(vp);
        }

        #endregion

        #region Bonus
        public void InitPackageBonusFilters()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_BONUS_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_BONUS_ITEM", arr);
            }

            int cntBuy = 0, cntFree = 0;

            foreach (CTable t in arr)
            {
                MPackageBonus v = new MPackageBonus(t);
                if (v.QuantityType.Equals("1"))
                {
                    package_bonusFilterBuy.Add(v);
                    cntBuy++;
                }
                else if (v.QuantityType.Equals("2"))
                {
                    package_bonusFilterFree.Add(v);
                    cntFree++;
                }
                v.ExtFlag = "I";

                v.Seq = bonusItemSeq;
                bonusItemSeq++;
            }

        }

        public void AddBonusItemBuy(MPackageBonus vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_BONUS_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_BONUS_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_bonusFilterBuy.Add(vp);

            vp.Seq = bonusItemSeq;
            vp.QuantityType = "1";
            bonusItemSeq++;

            vp.ExtFlag = "A";
        }


        public void AddBonusItemFree(MPackageBonus vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_BONUS_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_BONUS_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_bonusFilterFree.Add(vp);

            vp.Seq = bonusItemSeq;
            vp.QuantityType = "2";
            bonusItemSeq++;

            vp.ExtFlag = "A";
        }

        public void RemoveBonusBuyItem(MPackageBonus vp)
        {
            removeAssociateItems(vp, "PACKAGE_BONUS_ITEM", "INTERNAL_SEQ", "PACKAGE_BONUS_ID");
            package_bonusFilterBuy.Remove(vp);
        }

        public void RemoveBonusFreeItem(MPackageBonus vp)
        {
            removeAssociateItems(vp, "PACKAGE_BONUS_ITEM", "INTERNAL_SEQ", "PACKAGE_BONUS_ID");
            package_bonusFilterFree.Remove(vp);
        }

        public ObservableCollection<MPackageBonus> PackageBonusBuy
        {
            get
            {
                return (package_bonusFilterBuy);
            }
        }

        public ObservableCollection<MPackageBonus> PackageBonusFree
        {
            get
            {
                return (package_bonusFilterFree);
            }
        }
        #endregion


        #region Bundle
        public void InitPackageBundles()
        {
            package_bundles.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_BUNDLE_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_BUNDLE_ITEM", arr);
            }

            foreach (CTable t in arr)
            {
                MPackageBundle v = new MPackageBundle(t);
                package_bundles.Add(v);

                v.ExtFlag = "I";

                v.Seq = internalSeq;
                internalSeq++;
            }
        }

        public void AddBundleItem(MPackageBundle vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_BUNDLE_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_BUNDLE_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_bundles.Add(vp);

            vp.Seq = internalSeq;
            vp.QuantityType = "1";
            internalSeq++;

            vp.ExtFlag = "A";
        }

        public void RemoveBundleItem(MPackageBundle vp)
        {
            removeAssociateItems(vp, "PACKAGE_BUNDLE_ITEM", "INTERNAL_SEQ", "PACKAGE_BUNDLE_ID");
            package_bundles.Remove(vp);
        }

        public ObservableCollection<MPackageBundle> PackageBundles
        {
            get
            {
                return (package_bundles);
            }
        }

        #endregion

        #region FinalDiscount
        public void InitPackageFinalDiscounts()
        {
            package_final_discounts.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_FINAL_DISCOUNT_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_FINAL_DISCOUNT_ITEM", arr);
            }

            foreach (CTable t in arr)
            {
                MPackageFinalDiscount v = new MPackageFinalDiscount(t);
                package_final_discounts.Add(v);

                v.ExtFlag = "I";

                v.Seq = internalSeq;
                internalSeq++;
            }
        }

        public void AddFinalDiscountItem(MPackageFinalDiscount vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_FINAL_DISCOUNT_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_FINAL_DISCOUNT_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_final_discounts.Add(vp);

            vp.Seq = internalSeq;
            vp.QuantityType = "1";
            internalSeq++;

            vp.ExtFlag = "A";
        }

        public void RemoveFinalDiscountItem(MPackageFinalDiscount vp)
        {
            removeAssociateItems(vp, "PACKAGE_FINAL_DISCOUNT_ITEM", "INTERNAL_SEQ", "PACKAGE_FNLDISC_ID");
            package_final_discounts.Remove(vp);
        }

        public ObservableCollection<MPackageFinalDiscount> PackageFinalDiscounts
        {
            get
            {
                return (package_final_discounts);
            }
        }

        #endregion

        #region Voucher

        public void InitPackageVoucherFilters()
        {
            package_voucherBuy.Clear();
            package_voucherFree.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_VOUCHER_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_VOUCHER_ITEM", arr);
            }

            int cntBuy = 0, cntFree = 0;

            foreach (CTable t in arr)
            {
                MPackageVoucher v = new MPackageVoucher(t);
                if (v.QuantityType.Equals("1"))
                {
                    package_voucherBuy.Add(v);
                    cntBuy++;
                }
                else if (v.QuantityType.Equals("2"))
                {
                    package_voucherFree.Add(v);
                    cntFree++;
                }
                v.ExtFlag = "I";

                v.Seq = voucherItemSeq;
                voucherItemSeq++;
            }

        }
        
        public void AddVoucherBuyItem(MPackageVoucher vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_VOUCHER_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_VOUCHER_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_voucherBuy.Add(vp);

            vp.Seq = voucherItemSeq;
            vp.QuantityType = "1";
            voucherItemSeq++;
        }

        public void AddVoucherFreeItem(MPackageVoucher vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_VOUCHER_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_VOUCHER_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_voucherFree.Add(vp);

            vp.Seq = voucherItemSeq;
            vp.QuantityType = "2";
            voucherItemSeq++;

            vp.ExtFlag = "A";
        }

        public void RemoveVoucherBuyItem(MPackageVoucher vp)
        {
            removeAssociateItems(vp, "PACKAGE_VOUCHER_ITEM", "INTERNAL_SEQ", "PACKAGE_VOUCHER_ID");
            package_voucherBuy.Remove(vp);
        }

        public void RemoveVoucherFreeItem(MPackageVoucher vp)
        {
            removeAssociateItems(vp, "PACKAGE_VOUCHER_ITEM", "INTERNAL_SEQ", "PACKAGE_VOUCHER_ID");
            package_voucherFree.Remove(vp);
        }

        public ObservableCollection<MPackageVoucher> PackageVoucherBuy
        {
            get
            {
                return (package_voucherBuy);
            }
        }

        public ObservableCollection<MPackageVoucher> PackageVoucherFree
        {
            get
            {
                return (package_voucherFree);
            }
        }
        #endregion

        #region Branch

        public void InitPackageBranches()
        {
            package_branches.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_BRANCH_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_BRANCH_ITEM", arr);
            }

            Hashtable temp = new Hashtable();
            foreach (CTable t in arr)
            {
                MPackageBranch v = new MPackageBranch(t);

                package_branches.Add(v);
                temp[v.BranchId] = "dummy";

                v.ExtFlag = "I";

                v.Seq = internalSeq;
                internalSeq++;
            }

            if (!CMasterReference.IsMasterRefLoad(MasterRefEnum.MASTER_BRANCH))
            {
                CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetMasterRefList);
            }

            foreach (MMasterRef mr in CMasterReference.Instance.Branches)
            {
                if (mr.MasterID.Equals(""))
                {
                    continue;
                }

                if (temp.ContainsKey(mr.MasterID))
                {
                    continue;
                }

                MPackageBranch v = new MPackageBranch(new CTable(""));

                v.Code = mr.Code;
                v.Name = mr.Description;
                v.BranchId = mr.MasterID;
                v.ExtFlag = "A";

                package_branches.Add(v);
                arr.Add(v.GetDbObject());

                v.Seq = internalSeq;
                internalSeq++;
            }

        }

        public void AddBranchItem(MPackageBranch vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_BRANCH_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_BRANCH_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_branches.Add(vp);

            vp.Seq = internalSeq;
            internalSeq++;
        }

        public void RemoveBranchItem(MPackageBranch vp)
        {
            removeAssociateItems(vp, "PACKAGE_BRANCH_ITEM", "INTERNAL_SEQ", "PACKAGE_BRANCH_ID");
            package_branches.Remove(vp);
        }

        public ObservableCollection<MPackageBranch> PackageBranches
        {
            get
            {
                return (package_branches);
            }
        }
        #endregion

        #region Post Gift

        public void InitPackagePostFrees()
        {
            package_post_buys.Clear();
            package_post_gifts.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_VOUCHER_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_VOUCHER_ITEM", arr);
            }

            int cntBuy = 0;
            int cntFree = 0;

            foreach (CTable t in arr)
            {
                MPackageVoucher v = new MPackageVoucher(t);
                if (v.QuantityType.Equals("1"))
                {
                    package_post_buys.Add(v);
                    cntBuy++;
                }
                else if (v.QuantityType.Equals("2"))
                {
                    package_post_gifts.Add(v);
                    cntFree++;
                }
                v.ExtFlag = "I";

                v.Seq = internalSeq;
                internalSeq++;
            }
        }

        public void AddPostGiftBuyItem(MPackageVoucher vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_VOUCHER_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_VOUCHER_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_post_buys.Add(vp);

            vp.Seq = internalSeq;
            vp.QuantityType = "1";
            internalSeq++;
        }

        public void AddPostGiftFreeItem(MPackageVoucher vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_VOUCHER_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_VOUCHER_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            package_post_gifts.Add(vp);

            vp.Seq = internalSeq;
            vp.QuantityType = "2";
            internalSeq++;

            vp.ExtFlag = "A";
        }

        public void RemovePostGiftBuyItem(MPackageVoucher vp)
        {
            removeAssociateItems(vp, "PACKAGE_VOUCHER_ITEM", "INTERNAL_SEQ", "PACKAGE_VOUCHER_ID");
            package_post_buys.Remove(vp);
        }

        public void RemovePostGiftFreeItem(MPackageVoucher vp)
        {
            removeAssociateItems(vp, "PACKAGE_VOUCHER_ITEM", "INTERNAL_SEQ", "PACKAGE_VOUCHER_ID");
            package_post_gifts.Remove(vp);
        }

        public ObservableCollection<MPackageVoucher> PackagePostGiftBuys
        {
            get
            {
                return (package_post_buys);
            }
        }

        public ObservableCollection<MPackageVoucher> PackagePostGiftFrees
        {
            get
            {
                return (package_post_gifts);
            }
        }
        #endregion

        //PackageTrayByItems

        #region PackageTrayByItems
        public void InitTrayPriceItem()
        {
            tray_prices.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_TRAY_PRICE_ITEM");

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                MPackageTrayPriceDiscount v = new MPackageTrayPriceDiscount(t);
                tray_prices.Add(v);

                v.Seq = internalSeq;
                internalSeq++;
                v.ExtFlag = "I";
            }
        }

        public void AddTrayPriceItem(MPackageTrayPriceDiscount vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PACKAGE_TRAY_PRICE_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PACKAGE_TRAY_PRICE_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            tray_prices.Add(vp);

            vp.Seq = internalSeq;
            internalSeq++;

            //Not use
            vp.SeqNo = "1";
            vp.PricingType = "1";

            vp.ExtFlag = "A";
        }

        public void RemoveTrayPriceItem(MPackageTrayPriceDiscount vp)
        {
            removeAssociateItems(vp, "PACKAGE_TRAY_PRICE_ITEM", "INTERNAL_SEQ", "PACKAGE_TRAY_PRICE_ID");
            tray_prices.Remove(vp);
        }

        public ObservableCollection<MPackageTrayPriceDiscount> PackageTrayByItems
        {
            get
            {
                return (tray_prices);
            }
        }

#endregion
    }
}
