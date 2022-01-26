using System;
using System.Collections;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;


namespace Onix.Client.Pricing
{
    public class CPromotionProcessorPostGift : CPromotionProcessor
    {
        private CProcessingResult res = null;
        private int giftCount = 0;

        public CPromotionProcessorPostGift(MBaseModel pkg, String grpName, MBaseModel bill) : 
            base(pkg, grpName, bill)
        {
            res = new CProcessingResult((pkg as MPackage));
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
            CPrice accum = getAccumulatePrice(filterHash);

            CBasketSet output = finalizeOutput(inBasket, filterHash, accum);

            res.SetInputBasketSet(inBasket.Clone());
            res.SetOutputBasketSet(output);

            addProcessingResult(res);

            if (giftCount <= 0)
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

        private void updateBasketItem(CBasket bi, Hashtable filterHash)
        {
            int cnt = bi.GetBasketItemCount();

            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bki = bi.GetBasketItem(i);
                
                if (filterHash.ContainsKey(bki.Key))
                {
                    bki.IsPostGift = true;
                }
            }
        }

        private MSelectedItem createSelectedItem(MPackageVoucher pv)
        {
            MSelectedItem si = new MSelectedItem(new CTable(""));

            if (pv.SelectionType.Equals("1"))
            {
                //Service
                si.ServiceObj = pv.ServiceObj;
            }
            else if (pv.SelectionType.Equals("2"))
            {
                //Item
                si.ItemObj = pv.ItemObj;
            }
            else if (pv.SelectionType.Equals("4"))
            {
                //Voucher
                si.VoucherObj = pv.VoucherObj;
            }
            else if (pv.SelectionType.Equals("5"))
            {
                //Free text
                si.FreeText = pv.FreeText;
                si.FreeTextID = pv.PackageVoucherID;
            }

            si.SelectionType = pv.SelectionType;
            return (si);
        }

        private double getSumAmount(CBasketSet input, BasketTypeEnum type)
        {
            ArrayList baskets = input.GetAllBasketByType(type);

            if (baskets == null)
            {
                return (0.00);
            }

            double total = 0.00;

            foreach (CBasket bk in baskets)
            {
                BasketTypeEnum bt = bk.BasketType;

                if (type != bt)
                {
                    continue;
                }

                if (!isInBasketType(bt))
                {
                    continue;
                }

                if (bt == BasketTypeEnum.Bundled)
                {
                    total = total + bk.BundledAmount;
                }
                else
                {
                    int cnt = bk.GetBasketItemCount();
                    for (int i=0; i<cnt; i++)
                    {
                        CBasketItem bi = bk.GetBasketItem(i);
                        total = total + bi.GetAmount();
                    }
                }
            }

            return (total);
        }

        private CBasketSet finalizeOutput(CBasketSet input, Hashtable filterHash, CPrice accum)
        {
            CBasketSet interim = new CBasketSet();
            MPackage pkg = getPackage();

            double totalBundleAmt = getSumAmount(input, BasketTypeEnum.Bundled);

            accum.TotalAmount = accum.TotalAmount + totalBundleAmt;
            double ratio = calculateRatio(accum);

            if (ratio > 0)
            {
                ObservableCollection<MPackageVoucher> gifts = pkg.PackagePostGiftFrees;
                CBasket nbk = new CBasket(BasketTypeEnum.PostFree);

                foreach (MPackageVoucher g in gifts)
                {
                    if (g.EnabledFlag.Equals("N"))
                    {
                        continue;
                    }

                    double qty = CUtil.StringToDouble(g.Quantity) * ratio;
                    if (qty > 0)
                    {
                        MSelectedItem si = createSelectedItem(g);
                        CBasketItem nbi = new CBasketItem(si.Key, si, qty);
                        nbk.AddBasketItem(nbi);
                    }
                }

                if (nbk.GetBasketItemCount() > 0)
                {
                    interim.AddBasket(nbk);
                    giftCount++;
                }
            }

            //Copy the originals to output
            ArrayList types = input.GetBasketTypes();
            foreach (BasketTypeEnum bt in types)
            {
                ArrayList baskets = input.GetAllBasketByType(bt);

                foreach (CBasket bk in baskets)
                {
                    CBasket obk = new CBasket(bt);
                    obk.CopyEntireFrom(bk);

                    if (isInBasketType(bk.BasketType) && (giftCount > 0))
                    {
                        updateBasketItem(obk, filterHash);
                    }

                    interim.AddBasket(obk);
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
            //Different from Final Discount
            ObservableCollection<MPackageVoucher> arr = pkg.PackagePostGiftBuys;
           
            foreach (MPackageVoucher pf in arr)
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
            //Different from Final Discount
            ObservableCollection<MPackageVoucher> arr = pkg.PackagePostGiftBuys;

            foreach (MPackageVoucher pf in arr)
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

            //Same field as final discount
            cfg.DeserializeConfig(pkg.DiscountBasketTypeConfig, pkg.PostGiftBasketConfigType);

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
                        //Different from final discount
                        CBasketItem bi = bs.GetBasketItem(i);
                        if (bi.IsPostGift)
                        {
                            //Don't need to discount it again
                            continue;
                        }

                        if (bi.IsFinalDiscounted)
                        {
                            if (!isInBasketType(BasketTypeEnum.FinalDiscounted))
                            {
                                continue;
                            }
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

        #region Private

        private double calculateRatio(CPrice p)
        {
            MPackage pkg = getPackage();
            String pricingDef = pkg.DiscountDefinition;

            MIntervalConfig ivc = new MIntervalConfig(new CTable(""));
            ivc.DeserializeConfig(pricingDef);

            CPrice o = null;
            CBasketItem dummy = new CBasketItem("", null, p.Quantity, p.TotalAmount);

            //Gui might not see correctly earlier
            pkg.DiscountMapType = "1";
            ivc.TierScopeType = 0;

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
                //Use DiscountAmount as a temp field
                return (o.DiscountAmount);
            }

            return (-9999);
        }

        #endregion
    }
}
