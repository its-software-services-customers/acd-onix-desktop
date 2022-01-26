using System;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Helper;

namespace Onix.Client.Pricing
{
    public class CPromotionProcessorOperation : CPromotionProcessor
    {
        public CPromotionProcessorOperation(MBaseModel pkg, String grpName, MBaseModel bill) : 
            base(pkg, grpName, bill)
        {
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
            //Do nothing
            return (inBasket);
        }

        public CBasketSet MergeUsedToAvailable(CBasketSet inBasket, Boolean trayFlag)
        {
            BasketTypeEnum type1 = BasketTypeEnum.Available;
            BasketTypeEnum type2 = BasketTypeEnum.Used;
            String name = "MergeUsedToAvailable";

            if (trayFlag)
            {
                type1 = BasketTypeEnum.AvailableTray;
                type2 = BasketTypeEnum.UsedTray;
                name = name + "Tray";
            }

            CBasketSet output = new CBasketSet();

            CProcessingResult res = new CProcessingResult(name);
            res.SetInputBasketSet(inBasket.Clone());

            ArrayList useds = inBasket.GetAllBasketByType(type2);
            ArrayList avails = inBasket.GetAllBasketByType(type1);

            if (avails != null)
            {
                foreach (CBasket a in avails)
                {
                    //Move the original 'Available' basket
                    CBasket na = new CBasket(type1);
                    na.CopyEntireFrom(a);
                    output.AddBasket(na);
                }
            }

            if (useds != null)
            {
                foreach (CBasket u in useds)
                {
                    CBasket nb = new CBasket(type1);
                    nb.CopyEntireFrom(u);

                    output.AddBasket(nb);
                }
            }

            Hashtable filterSet = new Hashtable();
            filterSet[type1] = null;
            filterSet[type2] = null;
            copyBasketsExc(output, inBasket, filterSet);

            res.IsOperation = true;
            res.SetOutputBasketSet(output);
            res.SetStatus(ProcessingResultStatus.ProcessingSuccess);
            addProcessingResult(res);

            return (output);
        }

        public CBasketSet SumInGroup(CBasketSet inBasket, BasketTypeEnum bt)
        {
            CBasketSet output = new CBasketSet();

            CProcessingResult res = new CProcessingResult("SumInGroup-" + bt.ToString());
            res.SetInputBasketSet(inBasket.Clone());

            Hashtable quantities = new Hashtable();
            Hashtable objects = new Hashtable();
            ArrayList keys = new ArrayList();

            ArrayList items = inBasket.GetAllBasketByType(bt);
            if (items == null)
            {
                items = new ArrayList();
            }

            foreach (CBasket b in items)
            {
                int cnt = b.GetBasketItemCount();

                for (int i = 0; i < cnt; i++)
                {
                    CBasketItem bi = b.GetBasketItem(i);
                    object o = quantities[bi.Key];
                    if (o == null)
                    {
                        quantities[bi.Key] = bi.Quantity;
                        objects[bi.Key] = bi;

                        keys.Add(bi.Key);
                    }
                    else
                    {
                        double tmp = (double) quantities[bi.Key];
                        tmp = tmp + bi.Quantity;
                        quantities[bi.Key] = tmp;
                    }
                }                  
            }

            CBasket bkt = new CBasket(bt);
            foreach (String k in keys)
            {
                CBasketItem bi = (CBasketItem) objects[k];
                double qty = (double) quantities[k];
                CBasketItem bki = new CBasketItem(k, bi.Item, qty);

                bkt.AddBasketItem(bki);
            }

            if (bkt.GetBasketItemCount() > 0)
            {
                output.AddBasket(bkt);
            }

            Hashtable filterSet = new Hashtable();
            filterSet[bt] = null;
            copyBasketsExc(output, inBasket, filterSet);

            res.IsOperation = true;
            res.SetOutputBasketSet(output);
            res.SetStatus(ProcessingResultStatus.ProcessingSuccess);
            addProcessingResult(res);

            return (output);
        }

        #region Private

        #endregion
    }
}
