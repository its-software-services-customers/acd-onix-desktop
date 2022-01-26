using System;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Helper;

using Wis.WsClientAPI;

namespace Onix.Client.Pricing
{
    public class CPromotionProcessorPricing : CPromotionProcessor
    {
        private CProcessingResult res = null;
        private Hashtable priceDefs = new Hashtable();

        public CPromotionProcessorPricing(MBaseModel pkg, String grpName, MBaseModel bill) : 
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
            CBasket avBskt = inBasket.GetBasket(BasketTypeEnum.Available, 0);

            if (!isPackageEligible(res, null, false))
            {
                res.SetInputBasketSet(inBasket.Clone());
                res.SetOutputBasketSet(inBasket);
                addProcessingResult(res);
                return (inBasket);
            }

            if (avBskt == null)
            {
                return (inBasket);
            }

            loadPackagePriceDefinitions();

            int cnt = avBskt.GetBasketItemCount();
            int matchCnt = 0;

            CBasket nb = new CBasket(BasketTypeEnum.Priced);
            CBasket absk = new CBasket(BasketTypeEnum.Available);

            //For each item in the "Available" basket
            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bi = avBskt.GetBasketItem(i);
                CBasketItem bo = calculatePrice(bi);
                if (bo != null)
                {
                    matchCnt++;
                    nb.AddBasketItem(bo);
                }
                else
                {
                    //Move to Available but in the new basket
                    absk.AddBasketItem(bi);
                }
            }

            CBasketSet output = new CBasketSet();
            if (nb.GetBasketItemCount() > 0)
            {
                output.AddBasket(nb);
            }
            if (absk.GetBasketItemCount() > 0)
            {
                output.AddBasket(absk);
            }

            Hashtable filterSet = new Hashtable();
            filterSet[BasketTypeEnum.Available] = null;
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
            pkg.InitItemsPrice();
        }

        private Boolean isItemApplicable(MPackagePrice pp, CBasketItem bi)
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


        private CBasketItem calculatePrice(CBasketItem bi)
        {
            MPackage pkg = getPackage();

            CBasketItem nbi = new CBasketItem(bi.Key, bi.Item, bi.Quantity);

            foreach (MPackagePrice pp in pkg.PackageItemPrices)
            {
                if (pp.EnabledFlag.Equals("N"))
                {
                    continue;
                }

                if (!isItemApplicable(pp, bi))
                {
                    continue;
                }

                MIntervalConfig ivc = new MIntervalConfig(new CTable(""));
                ivc.DeserializeConfig(pp.PricingDefination);

                CPrice o = null;
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
            }

            return (null);
        }

        #endregion
    }
}
