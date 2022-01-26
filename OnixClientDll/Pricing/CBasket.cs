using System;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Helper;

namespace Onix.Client.Pricing
{
    public class CBasket : CBaseBasket
    {
        private Hashtable items = new Hashtable();
        private ArrayList arrs = new ArrayList();
        private BasketTypeEnum bt = BasketTypeEnum.Available;
        private double totAmt = 0.00;
        private double bundleAmt = 0.00;
        private ObservableCollection<CBasketItem> childs = new ObservableCollection<CBasketItem>();

        public CBasket() : base()
        {
        }

        public CBasket(BasketTypeEnum type) 
        {
            bt = type;
        }

        public ObservableCollection<CBasketItem> Items
        {
            get
            {
                return (childs);
            }

            set
            {
            }
        }

        public BasketTypeEnum BasketType
        {
            get
            {
                return (bt);
            }
        }

        public String BasketTypeName
        {
            get
            {
                if (BasketType == BasketTypeEnum.Bundled)
                {
                    return (String.Format("{0} (Amount = {1})", bt.ToString(), CUtil.FormatNumber(BundledAmount.ToString())));
                }

                return (bt.ToString());
            }
        }

        public double TotalAmount
        {
            get
            {
                return (totAmt);
            }

            set
            {
                totAmt = value;
            }
        }

        public double BundledAmount
        {
            get
            {
                return (bundleAmt);
            }

            set
            {
                bundleAmt = value;
            }
        }

        public void AddBasketItem(CBasketItem bi)
        {
            String key = bi.Key;
            items.Add(key, bi);

            arrs.Add(bi);
        }

        public void RemoveBasketItem(String key)
        {
            items.Remove(key);
        }

        public void UpdateBasketItemQuantity(String key, double qty)
        {
            CBasketItem bi = (CBasketItem) items[key];
            bi.Quantity = qty;
        }

        public double GetItemQuantity(String key)
        {
            CBasketItem bi = (CBasketItem)items[key];
            return (bi.Quantity);
        }

        public Boolean IsItemExist(string key)
        {
            CBasketItem bi = (CBasketItem)items[key];
            return (bi != null);
        }

        public Boolean IsEmpty()
        {
            return (items.Count == 0);
        }

        public CBasketItem GetBasketItem(int idx)
        {
            CBasketItem bi = (CBasketItem)arrs[idx];
            return (bi);
        }

        public int GetBasketItemCount()
        {
            return (items.Count);
        }

        //public void CopyFrom(CBasket src)
        //{
        //    TotalAmount = src.TotalAmount;

        //    int cnt = src.GetBasketItemCount();
        //    for (int i = 0; i < cnt; i++)
        //    {
        //        CBasketItem bi = src.GetBasketItem(i);
        //        AddBasketItem(bi);
        //    }            
        //}

        public void CopyEntireFrom(CBasket src)
        {
            TotalAmount = src.TotalAmount;
            BundledAmount = src.BundledAmount;

            int cnt = src.GetBasketItemCount();
            for (int i = 0; i < cnt; i++)
            {
                CBasketItem bi = src.GetBasketItem(i);
                CBasketItem nbi = new CBasketItem(bi.Key, bi.Item, bi.Quantity);

                nbi.SetFinalDiscount(bi.GetFinalDiscount());
                nbi.IsFinalDiscounted = bi.IsFinalDiscounted;
                nbi.IsPostGift = bi.IsPostGift;
                nbi.IsTray = bi.IsTray;

                if (bi.IsPriced())
                {
                    nbi.SetUnitPrice(bi.GetUnitPrice());
                }
                nbi.SetDiscount(bi.GetDiscount());

                AddBasketItem(nbi);
            }
        }

        #region Method

        public void AddItem(CBasketItem bs)
        {
            childs.Add(bs);
        }

        public void ClearItem()
        {
            childs.Clear();
        }


        #endregion
    }
}
