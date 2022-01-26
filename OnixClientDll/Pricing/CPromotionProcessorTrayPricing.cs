using System;
using System.Collections;
using Onix.Client.Model;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Pricing
{
    public class CPromotionProcessorTrayPricing : CPromotionProcessor
    {
        private CProcessingResult res = null;
        private Hashtable priceDefs = new Hashtable();

        public CPromotionProcessorTrayPricing(MBaseModel pkg, String grpName, MBaseModel bill) : 
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
            //Support only first one Available type basket
            CBasket avBskt = inBasket.GetBasket(BasketTypeEnum.AvailableTray, 0);

            if (!isPackageEligible(res, avBskt, true))
            {
                res.SetInputBasketSet(inBasket.Clone());
                res.SetOutputBasketSet(inBasket);
                addProcessingResult(res);
                return (inBasket);
            }

            loadPackagePriceDefinitions();

            int cnt = avBskt.GetBasketItemCount();
            int matchCnt = 0;

            CBasket nb = new CBasket(BasketTypeEnum.PricedTray);
            CBasket db = new CBasket(BasketTypeEnum.DiscountedTray);
            CBasket absk = new CBasket(BasketTypeEnum.AvailableTray);

            //For each item in the "AvailableTray" basket
            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bi = avBskt.GetBasketItem(i);
                CBasketItem bo = calculatePrice(bi);
                if (bo == null)
                {
                    //Move to AvailableTray but in the new basket
                    absk.AddBasketItem(bi);
                }
                else
                {
                    matchCnt++;

                    CBasketItem di = calculateDiscount(bo);
                    if (di == null)
                    {
                        nb.AddBasketItem(bo);
                    }
                    else
                    {
                        db.AddBasketItem(di);
                    }
                }
            }

            CBasketSet output = new CBasketSet();
            if (nb.GetBasketItemCount() > 0)
            {
                output.AddBasket(nb);
            }
            if (db.GetBasketItemCount() > 0)
            {
                output.AddBasket(db);
            }
            if (absk.GetBasketItemCount() > 0)
            {
                output.AddBasket(absk);
            }

            Hashtable filterSet = new Hashtable();
            filterSet[BasketTypeEnum.AvailableTray] = null;
            copyBasketsExc(output, inBasket, filterSet);

            if (matchCnt <= 0)
            {
                res.SetErrorCode("ERROR_NO_PROMOTION_MATCH");
                res.SetStatus(ProcessingResultStatus.ProcessingFail);
            }
            else
            {
                res.SetStatus(ProcessingResultStatus.ProcessingSuccess);
            }

            res.SetInputBasketSet(inBasket.Clone());
            res.SetOutputBasketSet(output);
            addProcessingResult(res);

            return (output);
        }

        #region Private

        private void loadPackagePriceDefinitions()
        {
            MPackage pkg = getPackage();
            pkg.InitTrayPriceItem();
        }

        private Boolean isItemApplicable(MPackageTrayPriceDiscount pp, CBasketItem bi)
        {
            String ppSelectType = pp.SelectionType;
            MSelectedItem vs = (MSelectedItem) bi.Item;

            
            if ((ppSelectType.Equals("1")) && (vs.SelectionType.Equals("1")))
            {
                //Service
                if (vs.ServiceID.Equals(pp.ServiceId))
                {
                    return (true);
                }
            }
            else if ((ppSelectType.Equals("2")) && (vs.SelectionType.Equals("2")))
            {
                //Item
                if (vs.ItemID.Equals(pp.ItemId))
                {
                    return (true);
                }
            }
            else if ((ppSelectType.Equals("3")) && (vs.SelectionType.Equals("2")))
            {
                //Item Category VS Item

                MInventoryItem vi = (MInventoryItem) vs.ItemObj;
                if (vi.ItemCategory.Equals(pp.CategoryId))
                {
                    return (true);
                }
            }

            return (false);
        }

        private CBasketItem calculateDiscount(CBasketItem bi)
        {
            MPackage pkg = getPackage();
            MSelectedItem vi = (MSelectedItem)bi.Item;

            CBasketItem nbi = new CBasketItem(bi.Key, bi.Item, bi.Quantity);
            nbi.SetUnitPrice(bi.GetUnitPrice());

            foreach (MPackageTrayPriceDiscount pp in pkg.PackageTrayByItems)
            {
                if (!isItemApplicable(pp, bi))
                {
                    continue;
                }

                MIntervalConfig ivc = new MIntervalConfig(new CTable(""));
                ivc.DeserializeConfig(pp.DiscountDefination);

                CPrice o = null;
                if (ivc.SelectionType == 1)
                {
                    //step
                    o = getStepDiscount(ivc, bi);
                }
                else
                {
                    //Tier
                    o = getTierDiscount(ivc, bi);
                }

                if (o != null)
                {
                    nbi.SetAppliedPackage(pkg);
                    nbi.SetDiscount(o.DiscountAmount);

                    return (nbi);
                }
            }

            return (null);
        }

        private CBasketItem calculatePrice(CBasketItem bi)
        {
            MPackage pkg = getPackage();
            MIntervalConfig ivc = null;
            CPrice o = null;
            Boolean applicable = false;
            CBasketItem nbi = new CBasketItem(bi.Key, bi.Item, bi.Quantity);

            foreach (MPackageTrayPriceDiscount pp in pkg.PackageTrayByItems)
            {
                if (pp.EnabledFlag.Equals("N"))
                {
                    continue;
                }

                if (!isItemApplicable(pp, bi))
                {
                    continue;
                }

                //Applicable here

                applicable = true;

                ivc = new MIntervalConfig(new CTable(""));
                ivc.DeserializeConfig(pp.PricingDefination);

                o = null;
                if (ivc.SelectionType == 1)
                {
                    //step
                    o = getStepPrice(ivc, bi);
                }
                else
                {
                    //Tier
                    o = getTierPrice(ivc, bi);
                }

                if (o != null)
                {
                    //Got the price

                    nbi.SetAppliedPackage(pkg);
                    nbi.SetUnitPrice(o.UnitPrice);

                    return (nbi);
                }
            }

            //Applicable but price not match here, or not applicable for all

            if (!applicable)
            {
                //Not applicable for all
                return (null);
            }

            //Get Default Price here
            MSelectedItem si = (MSelectedItem)bi.Item;
            ivc = new MIntervalConfig(new CTable(""));

            String pricingDef = si.PricingDefination;
            if (si.SelectionType.Equals("1"))
            {
                pricingDef = si.ServicePricingDefinition;
            }

            ivc.DeserializeConfig(pricingDef);

            o = null;
            if (ivc.SelectionType == 1)
            {
                //step
                o = getStepPrice(ivc, bi);
            }
            else
            {
                //Tier
                o = getTierPrice(ivc, bi);
            }

            if (o != null)
            {
                nbi.SetAppliedPackage(pkg);
                nbi.SetUnitPrice(o.UnitPrice);

                return (nbi);
            }

            return (null);
        }

        #endregion
    }
}
