using System;
using System.Collections;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;

namespace Onix.Client.Pricing
{
    public class CPromotionProcessorGift : CPromotionProcessor
    {
        private Hashtable combinedHash = new Hashtable();
        private CProcessingResult res = null;
        private int minumConfigQty = 999999999;
        private int lastSuccessMultiplier = 0;
        private int triedCount = 0;

        public CPromotionProcessorGift(MBaseModel pkg, String grpName, MBaseModel bill) : base(pkg, grpName, bill)
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
            CBasket avBskt = inBasket.GetBasket(BasketTypeEnum.Available, 0);

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
                trackingBasket = new CBasket(BasketTypeEnum.Available);

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
            preserveOriginalGrouping(output, input);

            output.AddBasket(finalizeBasket(trackingBasket, BasketTypeEnum.Available));
            output.AddBasket(finalizeBasket(used, BasketTypeEnum.Used));

            if (triedCount > 0)
            {
                MPackage pkg = getPackage();
                ObservableCollection<MPackageVoucher> gifts = pkg.PackageVoucherFree;

                CBasket gbsk = new CBasket(BasketTypeEnum.FreeVoucher);

                foreach (MPackageVoucher g in gifts)
                {
                    if (g.EnabledFlag.Equals("N"))
                    {
                        continue;
                    }

                    MSelectedItem si = createSelectedItem(g);
                    CBasketItem bi = new CBasketItem(si.Key, si, CUtil.StringToInt(g.Quantity) * triedCount);

                    gbsk.AddBasketItem(bi);
                }

                if (gbsk.GetBasketItemCount() > 0)
                {
                    output.AddBasket(gbsk);
                }
            }

            return (output);
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

        private void populateHash(ObservableCollection<MPackageVoucher> items, int type)
        {
            foreach (MPackageVoucher pb in items)
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
            pkg.InitPackageVoucherFilters();

            ObservableCollection<MPackageVoucher> useds = pkg.PackageVoucherBuy;

            //1 for Used, and 2 for Free
            populateHash(useds, 1);
        }
    }
#endregion
}
