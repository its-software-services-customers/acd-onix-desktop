using System;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Helper;

namespace Onix.Client.Pricing
{
    public class CPromotionProcessorTrayBundle : CPromotionProcessor
    {
        private Hashtable combinedHash = new Hashtable();
        private CProcessingResult res = null;
        private int minumConfigQty = 999999999;
        private int lastSuccessMultiplier = 0;
        private int triedCount = 0;

        public CPromotionProcessorTrayBundle(MBaseModel pkg, String grpName, MBaseModel bill) : base(pkg, grpName, bill)
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
            CBasket avBskt = inBasket.GetBasket(BasketTypeEnum.AvailableTray, 0);
            if (!isPackageEligible(res, avBskt, true))
            {
                res.SetInputBasketSet(inBasket.Clone());
                res.SetOutputBasketSet(inBasket);
                addProcessingResult(res);
                return (inBasket);
            }

            loadAllPossibleUsedAndFree();

            int multiplier = minumConfigQty;
            Boolean done = false;
            int goodBasketCnt = 0;
            CBasket trackingBasket = null;
            CBasket lastTrackingBkt = null;

            while (!done)
            {
                trackingBasket = new CBasket(BasketTypeEnum.AvailableTray);

                int cnt = avBskt.GetBasketItemCount();
                for (int i = 0; i < cnt; i++)
                {
                    CBasketItem bi = avBskt.GetBasketItem(i);
                    //Keep current amount of each item in the basket
                    CBasketItem nbi = new CBasketItem(bi.Key, bi.Item, bi.Quantity);
                    trackingBasket.AddBasketItem(nbi);
                }
                
                int status = ratioStatus(combinedHash, minumConfigQty, multiplier, trackingBasket);
                if (status == 0)
                {
                    //Good basket are already kept
                    lastTrackingBkt = trackingBasket;
                    lastSuccessMultiplier = multiplier;
                    triedCount++;

                    goodBasketCnt++;
                }
                else if (status == 1)
                {
                    done = true;
                }

                multiplier++;              
            }

            if (goodBasketCnt <= 0)
            {
                //Unable to find the best match
                res.SetErrorCode("ERROR_NO_PROMOTION_MATCH");
                res.SetStatus(ProcessingResultStatus.ProcessingFail);
            }
            else
            {
                res.SetStatus(ProcessingResultStatus.ProcessingSuccess);
            }

            CBasketSet output = finalizeOutput(inBasket, lastTrackingBkt, getLastGoodUsedBasket(), getLastGoodFreeBasket());

            res.SetInputBasketSet(inBasket.Clone());
            res.SetOutputBasketSet(output);
            addProcessingResult(res);

            return (output);
        }

        #region private

        private CBasketSet finalizeOutput(CBasketSet input, CBasket trackingBasket, CBasket used, CBasket free)
        {
            CBasketSet output = new CBasketSet();

            if ((used == null) && (free == null))
            {
                return (input);
            }

            //The original ones
            preserveOriginalGroupingTray(output, input);

            output.AddBasket(finalizeBasket(trackingBasket, BasketTypeEnum.AvailableTray));
            CBasket bundled = finalizeBasket(used, BasketTypeEnum.BundledTray);
            output.AddBasket(bundled);

            int cnt = bundled.GetBasketItemCount();
            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bi = bundled.GetBasketItem(i);
                bi.SetUnitPrice(0.00);
            }

            MPackage pkg = getPackage();

            bundled.SetAppliedPackage(getPackage());
            bundled.BundledAmount = triedCount * CUtil.StringToDouble(pkg.BundleAmount);

            return (output);
        }

        private void populateHash(ObservableCollection<MPackageBundle> items, int type)
        {
            foreach (MPackageBundle pb in items)
            {
                if (pb.EnabledFlag.Equals("Y"))
                {            
                    //MSelectedItem and MPackageBonus has some field in common
                    MSelectedItem si = new MSelectedItem(pb.GetDbObject());
                    String cmbKey = createCombinedKey(si, type);
                    combinedHash[cmbKey] = pb;

                    int qty = CUtil.StringToInt(pb.Quantity);
                    if (qty < minumConfigQty)
                    {
                        minumConfigQty = qty;
                    }                    
                }
            }
        }

        private void loadAllPossibleUsedAndFree()
        {
            MPackage pkg = getPackage();
            pkg.InitPackageBundles();

            ObservableCollection<MPackageBundle> useds = pkg.PackageBundles;

            //1 for Used, and 2 for Free
            populateHash(useds, 1);
        }
    }
#endregion
}
