using System;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Helper;

using Wis.WsClientAPI;

namespace Onix.Client.Pricing
{
    public class CPromotionProcessorPricingDefault : CPromotionProcessor
    {
        private CProcessingResult res = null;
        private Hashtable priceDefs = new Hashtable();

        public CPromotionProcessorPricingDefault(MBaseModel pkg, String grpName, MBaseModel bill) : 
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

            if (avBskt == null)
            {
                return (inBasket);
            }

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

        private CBasketItem calculatePrice(CBasketItem bi)
        {
            CBasketItem nbi = new CBasketItem(bi.Key, bi.Item, bi.Quantity);
            MSelectedItem si = (MSelectedItem) bi.Item;

            String pricingDef = si.PricingDefination;
            if (si.SelectionType.Equals("1"))
            {
                pricingDef = si.ServicePricingDefinition;
            }
            
            MIntervalConfig ivc = new MIntervalConfig(new CTable(""));
            ivc.DeserializeConfig(pricingDef);

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
                nbi.SetAppliedPackage(getPackage());
                nbi.SetUnitPrice(o.UnitPrice);

                return (nbi);
            }

            return (null);
        }

        #endregion
    }
}
