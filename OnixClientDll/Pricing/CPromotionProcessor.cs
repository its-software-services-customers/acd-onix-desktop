using System;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Onix.Client.Model;

namespace Onix.Client.Pricing
{
    public interface IPromotionProcessor
    {
        CBasketSet ApplyPakageLogic(CBasketSet inBasket);
        ArrayList GetProcessingResults();
    }

    public class BasketItemComparatorLowestPriceFirst : IComparer
    {
        //Comparer _comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);

        public int Compare(object x, object y)
        {
            //DefaultSellPrice
            CBasketItem bi1 = x as CBasketItem;
            CBasketItem bi2 = y as CBasketItem;

            MSelectedItem si1 = (MSelectedItem) bi1.Item;
            MSelectedItem si2 = (MSelectedItem) bi2.Item;

            double defSellPrice1 = 0.00;
            double defSellPrice2 = 0.00;

            if (si1.SelectionType.Equals("1"))
            {
                //Service
                //Not yet implemented default sell price for Service right now
                defSellPrice1 = 0.00;
            }
            else
            {
                //Item
                defSellPrice1 = CUtil.StringToDouble(si1.DefaultSellPriceItem);
            }

            if (si2.SelectionType.Equals("1"))
            {
                //Service
                //Not yet implemented default sell price for Service right now
                defSellPrice2 = 0.00;
            }
            else
            {
                //Item
                defSellPrice2 = CUtil.StringToDouble(si2.DefaultSellPriceItem);
            }

            return (defSellPrice2.CompareTo(defSellPrice1));
        }
    }

    public class BasketItemComparatorHightestPriceFirst : IComparer
    {
        //Comparer _comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);

        public int Compare(object x, object y)
        {
            //DefaultSellPrice
            CBasketItem bi1 = x as CBasketItem;
            CBasketItem bi2 = y as CBasketItem;

            MSelectedItem si1 = (MSelectedItem)bi1.Item;
            MSelectedItem si2 = (MSelectedItem)bi2.Item;

            double defSellPrice1 = 0.00;
            double defSellPrice2 = 0.00;

            if (si1.SelectionType.Equals("1"))
            {
                //Service
                //Not yet implemented default sell price for Service right now
                defSellPrice1 = 0.00;
            }
            else
            {
                //Item
                defSellPrice1 = CUtil.StringToDouble(si1.DefaultSellPriceItem);
            }

            if (si2.SelectionType.Equals("1"))
            {
                //Service
                //Not yet implemented default sell price for Service right now
                defSellPrice2 = 0.00;
            }
            else
            {
                //Item
                defSellPrice2 = CUtil.StringToDouble(si2.DefaultSellPriceItem);
            }

            return (defSellPrice1.CompareTo(defSellPrice2));
        }
    }

    public class CPromotionProcessor : IPromotionProcessor
    {
        private MPackage package = null;
        private ArrayList procResults = new ArrayList();
        private String groupName = "";
        private MBaseModel bill = null;

        private MEntity entity = null;
        private DateTime billDate = DateTime.Now;
        private DateTime billTime = DateTime.Now;

        private CBasket lastGoodUsed = null;
        private CBasket lastGoodFree = null;

        public CPromotionProcessor(MBaseModel pkg, String grpName, MBaseModel bl)
        {
            procResults = new ArrayList();
            package = (MPackage) pkg;
            bill = bl;

            if (bl is MBillSimulate)
            {
                entity = (MEntity) (bl as MBillSimulate).CustomerObj;
                billDate = (bl as MBillSimulate).DocumentDate;
                billTime = (bl as MBillSimulate).SimulateTime;
            }

            groupName = grpName;
        }

        public virtual CBasketSet ApplyPakageLogic(CBasketSet inBasket)
        {
            return (null);
        }

        public virtual ArrayList GetProcessingResults()
        {
            return (procResults);
        }

        #region Private

        private Boolean isBranchEligible()
        {
            MBillSimulate bl = bill as MBillSimulate;

            ObservableCollection<MPackageBranch> branches = package.PackageBranches;

            foreach (MPackageBranch b in branches)
            {
                if (b.EnabledFlag.Equals("Y"))
                {
                    if (b.BranchId.Equals(bl.BranchId))
                    {
                        return (true);
                    }
                }
            }

            return (false);

        }

        private Boolean isCustomerEligible()
        {
            ObservableCollection<MPackageCustomer> custs = package.PackageCustomers;

            foreach (MPackageCustomer c in custs)
            {
                if (c.EnabledFlag.Equals("Y"))
                {
                    if (c.SelectionType.Equals("1"))
                    {
                        //Type
                        if (entity.EntityType.Equals(c.CustomerTypeID))
                        {
                            return (true);
                        }
                    }
                    else if (c.SelectionType.Equals("2"))
                    {
                        //Group
                        if (entity.EntityGroup.Equals(c.CustomerGroupID))
                        {
                            return (true);
                        }
                    }
                    else if (c.SelectionType.Equals("3"))
                    {
                        //Customer
                        if (entity.EntityID.Equals(c.CustomerId))
                        {
                            return (true);
                        }
                    }
                }
            }
            
            return (false);
        }

        private Boolean isTimeEligible()
        {
            MPeriod mon = package.MondayPeriod;
            MPeriod tue = package.TuesdayPeriod;
            MPeriod wed = package.WednesdayPeriod;
            MPeriod thu = package.ThursdayPeriod;
            MPeriod fri = package.FridayPeriod;
            MPeriod sat = package.SathurdayPeriod;
            MPeriod sun = package.SundayPeriod;

            ArrayList periods = new ArrayList();
            periods.Add(mon);
            periods.Add(tue);
            periods.Add(wed);
            periods.Add(thu);
            periods.Add(fri);
            periods.Add(sat);
            periods.Add(sun);

            foreach (MPeriod p in periods)
            {
                if (isPeriodOK(p))
                {
                    return (true);
                }
            }

            return (false);
        }

        private Boolean isPeriodOK(MPeriod p)
        {
            Hashtable days = new Hashtable();

            days[1] = DayOfWeek.Monday;
            days[2] = DayOfWeek.Tuesday;
            days[3] = DayOfWeek.Wednesday;
            days[4] = DayOfWeek.Thursday;
            days[5] = DayOfWeek.Friday;
            days[6] = DayOfWeek.Saturday;
            days[7] = DayOfWeek.Sunday;

            DayOfWeek d = (DayOfWeek)days[CUtil.StringToInt(p.DayOfWeek)];
            if (d != billDate.DayOfWeek)
            {
                return (false);
            }

            if (!p.IsEnabled)
            {
                return (false);
            }


            if (p.IsEnabled && (p.PeriodType == IntervalTypeEnum.DAY_ENTIRE))
            {
                //All day
                return (true);
            }

            String fromHHMM = String.Format("{0}:{1}", p.FromHour1, p.FromMinute1);
            String toHHMM = String.Format("{0}:{1}", p.ToHour1, p.ToMinute1);
            String currTime = CUtil.DateTimeToDateStringTime(billTime).Substring(11, 5);

            if ((currTime.CompareTo(fromHHMM) >= 0) && (currTime.CompareTo(toHHMM) <= 0))
            {
                return (true);
            }

            return (false);
        }

        private Boolean isNotEffectiveOrExpire(CProcessingResult res)
        {
            DateTime effDate = package.EffectiveDate;
            DateTime expDate = package.ExpireDate;
            String effStr = "";
            String expStr = "";
            String currDateStr = CUtil.DateTimeToDateStringInternal(billDate);
            
            if (effDate != null)
            {
                effStr = CUtil.DateTimeToDateStringInternalMin(effDate);
                if (currDateStr.CompareTo(effStr) < 0)
                {
                    res.SetErrorCode("ERROR_PROMOTION_NOT_EFFECTIVE");
                    return (true);
                }
            }

            if (expDate != null)
            {
                expStr = CUtil.DateTimeToDateStringInternalMin(expDate);
                if (currDateStr.CompareTo(expStr) >= 0)
                {
                    res.SetErrorCode("ERROR_PROMOTION_IS_EXPIRE");
                    return (true);
                }
            }

            return (false);
        }

        #endregion

        #region Protected

        protected CBasket getLastGoodUsedBasket()
        {
            return (lastGoodUsed);
        }

        protected CBasket getLastGoodFreeBasket()
        {
            return (lastGoodFree);
        }

        protected CPrice getTierDiscount(MIntervalConfig ivc, CBasketItem bi)
        {
            MPackage pkg = getPackage();

            double qty = 0.00;
            if (ivc.MappingType == 0)
            {
                //Map by quantity
                qty = bi.Quantity;
            }
            else
            {
                //Map by amount
                qty = bi.GetAmount();
            }

            foreach (MInterval iv in ivc.IntervalItems)
            {
                double from = CUtil.StringToDouble(iv.FromValue);
                double to = CUtil.StringToDouble(iv.ToValue);
                double value = CUtil.StringToDouble(iv.ConfigValue);

                if ((qty > from) && (qty <= to))
                {
                    CPrice p = new CPrice();

                    if (ivc.TierScopeType == 0)
                    {
                        //Fixed
                        p.DiscountAmount = value;
                    }
                    else if (ivc.TierScopeType == 1)
                    {
                        //Per unit
                        p.DiscountAmount = bi.Quantity * value;
                    }
                    else
                    {
                        //2 - Percent of amount
                        p.DiscountAmount = (value * bi.GetAmount()) / 100;
                    }

                    return (p);
                }
            }

            return (null);
        }

        protected CPrice getStepDiscount(MIntervalConfig ivc, CBasketItem bi)
        {
            ArrayList intervals = new ArrayList();
            MPackage pkg = getPackage();

            int idx = 0;

            foreach (MInterval iv in ivc.IntervalItems)
            {
                iv.Used = 0.00;
                intervals.Add(iv);
                idx++;
            }

            double qty = 0.00;
            if (ivc.MappingType == 0)
            {
                qty = bi.Quantity;
            }
            else
            {
                //Map by amount
                //We can use both total bill amount or amount that we use for looking up
                qty = bi.GetAmount();
            }
            double left = qty;

            int cnt = ivc.IntervalItems.Count;
            idx = 0;

            while (left > 0)
            {
                MInterval iv = (MInterval)intervals[idx];
                double from = CUtil.StringToDouble(iv.FromValue);
                double to = CUtil.StringToDouble(iv.ToValue);
                double value = CUtil.StringToDouble(iv.ConfigValue);
                double gap = to - from;
                iv.RepeatCount++;

                double used = 0.00;
                if (left > gap)
                {
                    left = left - gap;
                    used = gap;
                }
                else
                {
                    used = left;
                    left = 0.00;
                }

                iv.Used = iv.Used + used;
                idx++;
                if (idx >= cnt)
                {
                    idx = 0;
                }
            }

            double total = 0.00;
            foreach (MInterval vi in intervals)
            {
                if (ivc.TierScopeType == 0)
                {
                    //Fixed
                    total = total + vi.RepeatCount * CUtil.StringToDouble(vi.ConfigValue);
                }
                else if (ivc.TierScopeType == 1)
                {
                    //Per unit
                    if (ivc.MappingType == 0)
                    {
                        //Map by quantity
                        total = total + vi.Used * CUtil.StringToDouble(vi.ConfigValue);
                    }
                }
                else
                {
                    //2 - Percent of amount
                    if (ivc.MappingType == 1)
                    {
                        //Map by amount
                        //For step, we can use the vi.Used from the amount that we split it up
                        total = total + vi.Used * CUtil.StringToDouble(vi.ConfigValue) / 100;
                    }
                }
            }

            CPrice p = new CPrice();
            p.DiscountAmount = total;

            return (p);
        }

        public CPrice getTierPrice(MIntervalConfig ivc, CBasketItem bi)
        {
            foreach (MInterval iv in ivc.IntervalItems)
            {
                double from = CUtil.StringToDouble(iv.FromValue);
                double to = CUtil.StringToDouble(iv.ToValue);
                double value = CUtil.StringToDouble(iv.ConfigValue);

                if ((bi.Quantity > from) && (bi.Quantity <= to))
                {
                    CPrice p = new CPrice();

                    if (ivc.TierScopeType == 1)
                    {
                        p.TotalAmount = value;
                        p.UnitPrice = value / bi.Quantity;
                    }
                    else
                    {
                        //0 - by unit price
                        p.TotalAmount = value * bi.Quantity;
                        p.UnitPrice = value;
                    }

                    return (p);
                }
            }

            return (null);
        }

        public CPrice getStepPrice(MIntervalConfig ivc, CBasketItem bi)
        {
            ArrayList intervals = new ArrayList();

            int idx = 0;

            foreach (MInterval iv in ivc.IntervalItems)
            {
                iv.Used = 0.00;
                intervals.Add(iv);
                idx++;
            }

            if (intervals.Count <= 0)
            {
                return (null);
            }

            double left = bi.Quantity;

            int cnt = ivc.IntervalItems.Count;
            idx = 0;

            while (left > 0)
            {
                MInterval iv = (MInterval)intervals[idx];
                double from = CUtil.StringToDouble(iv.FromValue);
                double to = CUtil.StringToDouble(iv.ToValue);
                double value = CUtil.StringToDouble(iv.ConfigValue);
                double gap = to - from;
                iv.RepeatCount++;

                double used = 0.00;
                if (left > gap)
                {
                    left = left - gap;
                    used = gap;
                }
                else
                {
                    used = left;
                    left = 0.00;
                }

                iv.Used = iv.Used + used;
                idx++;
                if (idx >= cnt)
                {
                    idx = 0;
                }
            }

            double total = 0.00;
            foreach (MInterval vi in intervals)
            {
                if (ivc.StepScopeType == 1)
                {
                    total = total + vi.RepeatCount * CUtil.StringToDouble(vi.ConfigValue);
                }
                else
                {
                    //0 - by unit price
                    total = total + vi.Used * CUtil.StringToDouble(vi.ConfigValue);
                }
            }

            CPrice p = new CPrice();
            p.TotalAmount = total;
            p.UnitPrice = total / bi.Quantity;

            return (p);
        }

        protected String createCombinedKey(MSelectedItem si, int type)
        {
            String selType = si.SelectionType;
            String key = "";

            if (selType.Equals("1"))
            {
                //Service
                key = si.ServiceID;
            }
            else if (selType.Equals("2"))
            {
                //Item
                key = si.ItemID;
            }
            else if (selType.Equals("3"))
            {
                //Item Category
                key = si.ItemCategory;
            }

            String cmbKey = String.Format("{0}-{1}-{2}", type, selType, key);

            return (cmbKey);
        }

        protected Boolean isPackageEligible(CProcessingResult res, CBasket inBsk, Boolean chkNullFlag)
        {
            if (chkNullFlag && (inBsk == null))
            {
                res.SetErrorCode("ERROR_NO_PROMOTION_MATCH");
                res.SetStatus(ProcessingResultStatus.ProcessingFail);
                return (false);
            }

            if (package.IsEnabled != true)
            {
                res.SetErrorCode("ERROR_PROMOTION_IS_DISABLE");
                res.SetStatus(ProcessingResultStatus.ProcessingFail);
                return (false);
            }

            if (isNotEffectiveOrExpire(res))
            {
                res.SetStatus(ProcessingResultStatus.ProcessingFail);
                return (false);
            }

            if (package.IsTimeSpecific == true)
            {
                Boolean isTimeOK = isTimeEligible();
                if (!isTimeOK)
                {
                    res.SetErrorCode("ERROR_TIME_NOT_MATCH");
                    res.SetStatus(ProcessingResultStatus.ProcessingFail);
                    return (false);
                }
            }

            if (package.IsCustomerSpecific == true)
            {
                Boolean isCustomerOK = isCustomerEligible();
                if (!isCustomerOK)
                {
                    res.SetErrorCode("ERROR_CUSTOMER_NOT_MATCH");
                    res.SetStatus(ProcessingResultStatus.ProcessingFail);
                    return (false);
                }
            }

            if (package.IsBranchSpecific == true)
            {
                Boolean isBranchOK = isBranchEligible();
                if (!isBranchOK)
                {
                    res.SetErrorCode("ERROR_BRANCH_NOT_MATCH");
                    res.SetStatus(ProcessingResultStatus.ProcessingFail);
                    return (false);
                }
            }

            return (true);
        }

        protected void addProcessingResult(CProcessingResult result)
        {
            result.SetGroupName(groupName);
            procResults.Add(result);
        }

        protected void copyBasketsInc(CBasketSet dst, CBasketSet src, Hashtable incSet)
        {
            ArrayList types = src.GetBasketTypes();
            foreach (BasketTypeEnum t in types)
            {
                if (incSet.ContainsKey(t))
                {
                    ArrayList arr = src.GetAllBasketByType(t);
                    if (arr.Count > 0)
                    {
                        dst.AddByArray(arr);
                    }
                }
            }
        }

        protected void copyBasketsExc(CBasketSet dst, CBasketSet src, Hashtable incSet)
        {
            ArrayList types = src.GetBasketTypes();
            foreach (BasketTypeEnum t in types)
            {
                if (!incSet.ContainsKey(t))
                {
                    ArrayList arr = src.GetAllBasketByType(t);
                    if (arr.Count > 0)
                    {
                        dst.AddByArray(arr);
                    }
                }
            }
        }

        protected MPackage getPackage()
        {
            return (package);
        }

        protected void accumulateBasket(CBasket used, CBasket freed, CBasket bk)
        {
            CBasket tmp = freed;
            if (bk.BasketType == BasketTypeEnum.Used)
            {
                tmp = used;
            }

            int cnt = bk.GetBasketItemCount();

            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bi = bk.GetBasketItem(i);

                int c = tmp.GetBasketItemCount();
                Boolean found = false;
                for (int j = 0; j < c; j++)
                {
                    CBasketItem nbi = tmp.GetBasketItem(j);

                    if (bi.Key.Equals(nbi.Key))
                    {
                        bi.Quantity = bi.Quantity + nbi.Quantity;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    tmp.AddBasketItem(bi);
                }
            }
        }

        protected CBasket finalizeBasket(CBasket bskt, BasketTypeEnum retType)
        {
            CBasket nbk = new CBasket(retType);
            if (bskt == null)
            {
                return (nbk);
            }

            int cnt = bskt.GetBasketItemCount();
            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bi = bskt.GetBasketItem(i);
                if (bi.TempLeftQty > 0)
                {
                    bi.Quantity = bi.TempLeftQty;
                    nbk.AddBasketItem(bi);
                }
            }

            return (nbk);
        }

        private CBasket getQualifyItem(CPackageItemAdapter v, CBasket trackingBasket, int required)
        {
            CBasket nbk = null;
            if (v.QuantityType.Equals("1"))
            {
                //Used
                nbk = new CBasket(BasketTypeEnum.Used);
            }
            else
            {
                //2 = Free/Bonus
                nbk = new CBasket(BasketTypeEnum.FreeAnnonymous);
            }

            int cnt = trackingBasket.GetBasketItemCount();
            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bi = trackingBasket.GetBasketItem(i);
                MSelectedItem si = (MSelectedItem)bi.Item;

                if (v.SelectionType.Equals("1"))
                {
                    //Service
                    if (v.ServiceID.Equals(si.ServiceID) && (bi.TempLeftQty >= required))
                    {
                        bi.TempLeftQty = bi.TempLeftQty - required;
                        CBasketItem nbi = new CBasketItem(bi.Key, si, required);
                        nbk.AddBasketItem(nbi);

                        return (nbk);
                    }
                }
                else if (v.SelectionType.Equals("2"))
                {
                    //Item
                    if (v.ItemID.Equals(si.ItemID) && (bi.TempLeftQty >= required))
                    {
                        bi.TempLeftQty = bi.TempLeftQty - required;
                        CBasketItem nbi = new CBasketItem(bi.Key, si, required);
                        nbk.AddBasketItem(nbi);

                        return (nbk);
                    }
                }
            }

            return (null);
        }

        private ArrayList sortBasketItem(CBasket trackingBasket, CPackageItemAdapter v)
        {
            int cnt = trackingBasket.GetBasketItemCount();
            ArrayList arr = new ArrayList();

            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bi = trackingBasket.GetBasketItem(i);
                arr.Add(bi);
            }

            MPackage pkg = getPackage();


            if (pkg.PackageType.Equals("5") || pkg.PackageType.Equals("10"))
            {
                //Bundle
                arr.Sort(new BasketItemComparatorHightestPriceFirst());
            }
            else if (v.QuantityType.Equals("1"))
            {
                //Used
                arr.Sort(new BasketItemComparatorLowestPriceFirst());
            }
            else
            {
                //2 - Free, cheapest come first
                arr.Sort(new BasketItemComparatorHightestPriceFirst());
            }

            return (arr);
        }

        private CBasket getQualifyItemByGroup(CPackageItemAdapter v, CBasket trackingBasket, int required)
        {
            CBasket nbk = null;
            if (v.QuantityType.Equals("1"))
            {
                //Used
                nbk = new CBasket(BasketTypeEnum.Used);
            }
            else
            {
                //2 = Free/Bonus
                nbk = new CBasket(BasketTypeEnum.FreeAnnonymous);
            }

            //This should be sorted by Price and sort order depend on Quantity Type
            //QuantityType = 1, most expensive come first
            //QuantityType = 2, cheapest come first

            ArrayList sortedArr = sortBasketItem(trackingBasket, v);

            int cnt = sortedArr.Count; //trackingBasket.GetBasketItemCount();
            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bi = (CBasketItem) sortedArr[i]; //trackingBasket.GetBasketItem(i);
                MSelectedItem si = (MSelectedItem)bi.Item;

                if (si.ItemCategory.Equals(v.ItemCategoryID) && (bi.TempLeftQty > 0))
                {
                    int req = 0;
                    if (required >= bi.TempLeftQty)
                    {
                        req = (int)bi.TempLeftQty;
                        required = required - (int)bi.TempLeftQty;
                        bi.TempLeftQty = 0.00;
                    }
                    else
                    {
                        req = required;
                        bi.TempLeftQty = bi.TempLeftQty - required;
                        required = 0;
                    }

                    CBasketItem nbi = new CBasketItem(bi.Key, si, req);
                    nbk.AddBasketItem(nbi);
                }

                if (required <= 0)
                {
                    break;
                }
            }

            if (required <= 0)
            {
                //Got all required
                return (nbk);
            }

            return (null);
        }

        protected CBasket isDeductAble(CPackageItemAdapter v, CBasket trackingBasket, int required)
        {
            CBasket qualifyBasket = null;
            if (v.SelectionType.Equals("1") || v.SelectionType.Equals("2"))
            {
                qualifyBasket = getQualifyItem(v, trackingBasket, required);
            }
            else if (v.SelectionType.Equals("3"))
            {
                qualifyBasket = getQualifyItemByGroup(v, trackingBasket, required);
            }

            return (qualifyBasket);
        }

        protected void preserveOriginalGroupingTray(CBasketSet output, CBasketSet input)
        {
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.UsedTray));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.FreeAnnonymousTray));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.BundledTray));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.Available));
        }

        protected void preserveOriginalGrouping(CBasketSet output, CBasketSet input)
        {
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.Used));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.FreeAnnonymous));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.Bundled));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.FreeVoucher));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.AvailableTray));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.PricedTray));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.DiscountedTray));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.UsedTray));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.FreeAnnonymousTray));
            output.AddByArray(input.GetAllBasketByType(BasketTypeEnum.BundledTray));
        }

        protected int ratioStatus(Hashtable combinedHash, int minumConfigQty, int ratio, CBasket trackingBasket)
        {
            CBasket usedAccum = new CBasket(BasketTypeEnum.Used);
            CBasket freeAccum = new CBasket(BasketTypeEnum.FreeAnnonymous);

            foreach (String key in combinedHash.Keys)
            {
                MBaseModel u = (MBaseModel)combinedHash[key];
                CPackageItemAdapter v = new CPackageItemAdapter(u);

                if ((ratio % minumConfigQty) != 0)
                {
                    return (-1);
                }

                if ((ratio * CUtil.StringToInt(v.Quantity) % minumConfigQty) != 0)
                {
                    return (-1);
                }

                int required = (ratio * CUtil.StringToInt(v.Quantity) / minumConfigQty);
                CBasket bk = isDeductAble(v, trackingBasket, required);

                if (bk == null)
                {
                    //Not enough
                    return (1);
                }
                else
                {
                    //Use returned basket to add item to hash for summing the quantity
                    accumulateBasket(usedAccum, freeAccum, bk);
                }
            }

            lastGoodUsed = usedAccum;
            lastGoodFree = freeAccum;

            return (0);
        }

    }
#endregion

}
