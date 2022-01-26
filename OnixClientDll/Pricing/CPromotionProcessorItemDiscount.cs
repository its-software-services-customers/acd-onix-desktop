using System;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Helper;

using Wis.WsClientAPI;

namespace Onix.Client.Pricing
{
    public class CPromotionProcessorItemDiscount : CPromotionProcessor
    {
        private CProcessingResult res = null;

        public CPromotionProcessorItemDiscount(MBaseModel pkg, String grpName, MBaseModel bill) : 
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
            ArrayList priceds = inBasket.GetAllBasketByType(BasketTypeEnum.Priced);
            CBasketSet output = new CBasketSet();
            int matchCnt = 0;

            if (!isPackageEligible(res, null, false))
            {
                res.SetInputBasketSet(inBasket.Clone());
                res.SetOutputBasketSet(inBasket);
                addProcessingResult(res);
                return (inBasket);
            }

            loadPackageDiscountDefinitions();

            if (priceds == null)
            {
                priceds = new ArrayList();
            }

            foreach (CBasket b in priceds)
            {
                int c = b.GetBasketItemCount();

                CBasket nb = new CBasket(BasketTypeEnum.Discounted);
                CBasket absk = new CBasket(BasketTypeEnum.Priced);

                for (int i = 0; i < c; i++)
                {
                    CBasketItem bi = b.GetBasketItem(i);
                    CBasketItem bo = calculateDiscount(bi);

                    if (bo != null)
                    {
                        matchCnt++;
                        nb.AddBasketItem(bo);
                    }
                    else
                    {
                        //Move to Priced but in the new basket
                        absk.AddBasketItem(bi);
                    }
                }

                if (nb.GetBasketItemCount() > 0)
                {
                    output.AddBasket(nb);
                }

                if (absk.GetBasketItemCount() > 0)
                {
                    output.AddBasket(absk);
                }
            }

            Hashtable filterSet = new Hashtable();
            filterSet[BasketTypeEnum.Priced] = null;
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

        private void loadPackageDiscountDefinitions()
        {
            MPackage pkg = getPackage();
            pkg.InitPackageDiscountFilters();
        }

        private Boolean isItemApplicable(MPackageDiscount pp, CBasketItem bi)
        {
            String ppSelectType = pp.SelectionType;
            MSelectedItem vs = (MSelectedItem)bi.Item;


            if ((ppSelectType.Equals("1")) && (vs.SelectionType.Equals("1")))
            {
                //Service
                if (vs.ServiceID.Equals(pp.ServiceID))
                {
                    return (true);
                }
            }
            else if ((ppSelectType.Equals("2")) && (vs.SelectionType.Equals("2")))
            {
                //Item
                if (vs.ItemID.Equals(pp.ItemID))
                {
                    return (true);
                }
            }
            else if ((ppSelectType.Equals("3")) && (vs.SelectionType.Equals("2")))
            {
                //Item Category VS Item

                MInventoryItem vi = (MInventoryItem)vs.ItemObj;
                if (vi.ItemCategory.Equals(pp.ItemCategoryID))
                {
                    return (true);
                }
            }

            return (false);
        }

        private CBasketItem calculateDiscount(CBasketItem bi)
        {
            MPackage pkg = getPackage();
            MSelectedItem vi = (MSelectedItem) bi.Item;

            CBasketItem nbi = new CBasketItem(bi.Key, bi.Item, bi.Quantity);
            nbi.SetUnitPrice(bi.GetUnitPrice());

            foreach (MPackageDiscount pp in pkg.PackageDiscount)
            {
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

        #endregion
    }
}
