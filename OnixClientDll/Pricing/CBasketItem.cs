using System;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Helper;

namespace Onix.Client.Pricing
{
    public class CBasketItem : CBaseBasket
    {
        private String key = "";
        private MBaseModel v = null;
        private double qty = 0.00;
        private ArrayList intdms = null;
        private double price = 0.00;
        private double amount = 0.00;
        private double discount = 0.00;
        private Boolean isPriced = false;
        private double fdiscount = 0.00;
        private Boolean isFinalDiscounted = false;
        private Boolean isPostGift = false;
        private double leftQty = 0.00;
        private Boolean isTray = false;

        public CBasketItem(String k, MBaseModel item, double quantity)
        {
            key = k;
            v = item;
            qty = quantity;
            leftQty = qty;
        }

        public CBasketItem(String k, MBaseModel item, double quantity, double amt)
        {
            key = k;
            v = item;
            qty = quantity;
            amount = amt;
            if (qty > 0)
            {
                price = amount / qty;
            }
        }

        public double Quantity
        {
            get
            {
                return (qty);
            }

            set
            {
                qty = value;
            }
        }

        public double TempLeftQty
        {
            get
            {
                return (leftQty);
            }

            set
            {
                leftQty = value;
            }
        }

        public String QuantityFmt
        {
            get
            {
                return (CUtil.FormatNumber(qty.ToString()));
            }

            set
            {
            }
        }

        public Boolean IsFinalDiscounted
        {
            get
            {
                return (isFinalDiscounted);
            }

            set
            {
                isFinalDiscounted = value;
            }
        }

        public Boolean IsPostGift
        {
            get
            {
                return (isPostGift);
            }

            set
            {
                isPostGift = value;
            }
        }

        public Boolean IsTray
        {
            get
            {
                return (isTray);
            }

            set
            {
                isTray = value;
            }
        }

        public String Name
        {
            get
            {
                String caption = "";
                
                if (isFinalDiscounted)
                {
                    caption = "FinalDiscounted";
                }

                if (isPostGift)
                {
                    if (!caption.Equals(""))
                    {
                        caption = caption + ", " + "PostGift";
                    }
                    else
                    {
                        caption = "PostGift";
                    }
                }

                if (!caption.Equals(""))
                {
                    String tmp = String.Format("{0} ({1})", (Item as MSelectedItem).DisplayName, caption);
                    return (tmp);
                }

                return ((Item as MSelectedItem).DisplayName);
            }

            set
            {
            }
        }

        public MBaseModel Item
        {
            get
            {
                return (v);
            }
        }

        public String Key
        {
            get
            {
                return (key);
            }
        }

#region public method
        public void SetIntermediateItems(ArrayList items)
        {
            intdms = items;
        }

        public ArrayList GetIntermediateItems()
        {
            return(intdms);
        }

        public void SetUnitPrice(double prc)
        {
            isPriced = true;
            price = prc;
            amount = price * qty;
        }

        public double GetUnitPrice()
        {
            return (price);
        }

        public double GetAmount()
        {
            amount = price * qty;
            return (amount);
        }

        public void SetDiscount(double dsc)
        {
            discount = dsc;
        }

        public double GetDiscount()
        {
            return (discount);
        }

        public double GetTotal()
        {
            double tot = amount - discount;
            return (tot);
        }

        public Boolean IsPriced()
        {
            return (isPriced);
        }

        public void SetFinalDiscount(double fd)
        {
            fdiscount = fd;
        }

        public double GetFinalDiscount()
        {
            return (fdiscount);
        }

        #endregion
    }
}
