using System;
using System.Collections;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;

namespace Onix.Client.Pricing
{
    public class CPromotionProcessorFinalDiscount : CPromotionProcessor
    {
        private CProcessingResult res = null;
        private double finalDiscount = 0.00;
        private Boolean isFinalDiscount = false;
        private double totalBundleAmt = 0.00;

        public CPromotionProcessorFinalDiscount(MBaseModel pkg, String grpName, MBaseModel bill) : 
            base(pkg, grpName, bill)
        {
            res = new CProcessingResult((pkg as MPackage));
        }

        public Boolean IsFinalDiscount
        {
            get
            {
                return (isFinalDiscount);
            }
        }

        public double FinalDiscount
        {
            get
            {
                return (finalDiscount);
            }
        }

        /* Return array of BasketSet, return null if package not applicable */
        /*
         * Basket(Available) = {A:10, C:2, D:7}
         * Basket(Used) = {B:3}
         * Basket(Free) = {E:1}
         * BasketSet = {A={A:10, C:2, D:7}, U={B:3}, F={E:1}}
        */
        public override CBasketSet ApplyPakageLogic(CBasketSet inBasket)
        {
            if (!isPackageEligible(res, null, false))
            {
                res.SetInputBasketSet(inBasket.Clone());
                res.SetOutputBasketSet(inBasket);
                addProcessingResult(res);
                return (inBasket);
            }

            Hashtable accumHash = getAccumulateHash(inBasket);

            Hashtable filterHash = getEligibleItemsHash(accumHash);
            //CPrice accum = getAccumulatePrice(filterHash);

            //totalBundleAmt created in finalizeOutput() and used in getAccumulatePrice()
            CBasketSet output = finalizeOutput(inBasket, filterHash);
            CPrice accum = getAccumulatePrice(filterHash);

            res.SetInputBasketSet(inBasket.Clone());
            res.SetOutputBasketSet(output);
            Boolean priceMatched = calculateFinalDiscount(accum);
            addProcessingResult(res);

            if ((filterHash.Count <= 0) || (!priceMatched))
            {
                res.SetErrorCode("ERROR_NO_PROMOTION_MATCH");
                res.SetStatus(ProcessingResultStatus.ProcessingFail);
            }
            else
            {
                res.SetStatus(ProcessingResultStatus.ProcessingSuccess);
            }

            return (output);
        }

        #region Private

        private void updateBasketItem(CBasket bi, Hashtable filterHash)
        {
            int cnt = bi.GetBasketItemCount();

            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bki = bi.GetBasketItem(i);
                
                if (filterHash.ContainsKey(bki.Key))
                {
                    bki.IsFinalDiscounted = true;
                }
            }
        }

        private CBasketSet finalizeOutput(CBasketSet input, Hashtable filterHash)
        {
            CBasketSet interim = new CBasketSet();
            MPackage pkg = getPackage();

            ArrayList types = input.GetBasketTypes();
            foreach (BasketTypeEnum bt in types)
            {
                ArrayList baskets = input.GetAllBasketByType(bt);

                foreach (CBasket bk in baskets)
                {
                    if (bt == BasketTypeEnum.Bundled)
                    {
                        if (isInBasketType(bt))
                        {
                            totalBundleAmt = totalBundleAmt + bk.BundledAmount;
                        }
                    }

                    CBasket nbk = new CBasket(bt);
                    nbk.CopyEntireFrom(bk);

                    if (isInBasketType(bk.BasketType))
                    {
                        updateBasketItem(nbk, filterHash);
                    }                    

                    interim.AddBasket(nbk);
                }
            }

            return (interim);
        }

        private CPrice getAccumulatePrice(Hashtable filters)
        {
            CPrice accum = new CPrice();

            foreach (String key in filters.Keys)
            {
                CPrice p = (CPrice)filters[key];
                accum.Quantity = accum.Quantity + p.Quantity;
                accum.TotalAmount = accum.TotalAmount + p.TotalAmount;
            }

            //Bundle amount has not been added earlier
            accum.TotalAmount = accum.TotalAmount + totalBundleAmt;

            return (accum);
        }

        private Hashtable getEligibleItemsHash(Hashtable accumHash)
        {
            Hashtable filterHash = new Hashtable();

            MPackage pkg = getPackage();

            if (pkg.IsProductSpecific == false)
            {
                //All item in the basket is eligible
                return (accumHash);
            }

            foreach (String key in accumHash.Keys)
            {
                CPrice p = (CPrice)accumHash[key];
                bool isEligible = isItemEligible(p, pkg, key);

                if (isEligible)
                {
                    filterHash.Add(key, p);
                }
            }

            //If all all item exist in pkg.PackageFinalDiscounts
            if (isAllExist(filterHash, pkg))
            {
                return (filterHash);
            }

            return (new Hashtable());
        }

        private bool isAllExist(Hashtable hs, MPackage pkg)
        {
            ObservableCollection<MPackageFinalDiscount> arr = pkg.PackageFinalDiscounts;
           
            foreach (MPackageFinalDiscount pf in arr)
            {
                if (pf.EnabledFlag.Equals("N"))
                {
                    continue;
                }

                if (!hs.ContainsKey(pf.Key))
                {
                    return (false);
                }
            }

            return (true);
        }

        private bool isItemEligible(CPrice item, MPackage pkg, String key)
        {
            ObservableCollection<MPackageFinalDiscount> arr = pkg.PackageFinalDiscounts;

            foreach (MPackageFinalDiscount pf in arr)
            {
                if (pf.EnabledFlag.Equals("N"))
                {
                    continue;
                }

                if (key.Equals(pf.Key))
                {
                    double minimumQty = CUtil.StringToDouble(pf.Quantity);
                    if (item.Quantity >= minimumQty)
                    {
                        return (true);
                    }
                }
            }

            return (false);
        }

        private Boolean isInBasketType(BasketTypeEnum bt)
        {
            MPackage pkg = getPackage();
            MBasketTypeConfig cfg = new MBasketTypeConfig(new CTable(""));

            cfg.DeserializeConfig(pkg.DiscountBasketTypeConfig, pkg.BasketConfigType);

            foreach (MMasterRef vm in cfg.SelectedBaskets)
            {
                if (vm.EnabledFlag.Equals("Y"))
                {
                    if (bt.ToString().Equals(vm.Code))
                    {
                        return (true);
                    }
                }
            }

            return (false);
        }

        private Hashtable getAccumulateHash(CBasketSet inBasket)
        {
            Hashtable hash = new Hashtable();

            ArrayList types = inBasket.GetBasketTypes();
            foreach (BasketTypeEnum bt in types)
            {
                if (!isInBasketType(bt))
                {
                    continue;
                }

                ArrayList arr = inBasket.GetAllBasketByType(bt);

                foreach (CBasket bs in arr)
                {
                    int cnt = bs.GetBasketItemCount();

                    for (int i = 0; i < cnt; i++)
                    {
                        CBasketItem bi = bs.GetBasketItem(i);
                        if (bi.IsFinalDiscounted)
                        {
                            //Don't need to discount it again
                            continue;
                        }

                        String key = bi.Key;
                        CPrice p = null;

                        //At this point, the Amount of Bundle items is 0.00 (will be added in later step)
                        if (!hash.ContainsKey(key))
                        {
                            p = new CPrice();
                            p.Quantity = bi.Quantity;
                            p.TotalAmount = bi.GetTotal();

                            hash.Add(key, p);
                        }
                        else
                        {
                            p = (CPrice)hash[key];
                            p.Quantity = p.Quantity + bi.Quantity;
                            p.TotalAmount = p.TotalAmount + bi.GetTotal();
                        }
                    }
                }
            }

            return (hash);
        }

        

        private Boolean calculateFinalDiscount(CPrice p)
        {
            Boolean matched = false;
            MPackage pkg = getPackage();
            String pricingDef = pkg.DiscountDefinition;

            MIntervalConfig ivc = new MIntervalConfig(new CTable(""));
            ivc.DeserializeConfig(pricingDef);

            CPrice o = null;
            CBasketItem dummy = new CBasketItem("", null, p.Quantity, p.TotalAmount);

            if (ivc.SelectionType == 1)
            {
                //step
                o = getStepDiscount(ivc, dummy);
            }
            else
            {
                //Tier
                o = getTierDiscount(ivc, dummy);
            }

            if (o != null)
            {
                res.FinalDiscount = o.DiscountAmount;
                finalDiscount = res.FinalDiscount;
                isFinalDiscount = true;

                matched = true;
            }

            return (matched);
        }

        #endregion
    }
}
