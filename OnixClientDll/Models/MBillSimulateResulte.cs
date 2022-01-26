using System;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Pricing;

namespace Onix.Client.Model
{
    public class MBillSimulateResulte : MBaseModel
    {
        private CBasketItem bki = null;
        private MSelectedItem si = null;
        private MPackage pkg = null;
        private int grpNum = 0;
        private int sequence = 0;
        private Hashtable weights = new Hashtable();
        private CBasket basket = null;
        private double amount = 0.00;
        private double totalAmount = 0.00;
        private Boolean isTray = false;

        public MBillSimulateResulte(CTable obj) : base(obj)
        {

            amount = bki.GetAmount();
            totalAmount = bki.GetTotal();
        }        

        public int GroupNo
        {
            get
            {
                return (grpNum);
            }
        }

        public int Sequence
        {
            get
            {
                return (sequence);
            }
        }

        public double BundledAmount
        {
            get
            {
                return (basket.BundledAmount);
            }
        }

        public double Quantity
        {
            get
            {
                return (bki.Quantity);
            }
        }

        public String QuantityFmt
        {
            get
            {
                return (CUtil.FormatNumber(Quantity.ToString()));
            }
        }

        public double Amount
        {
            get
            {
                return (amount);
            }

            set
            {
                amount = value;
            }
        }

        public String AmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(Amount.ToString()));
            }
        }

        public double Discount
        {
            get
            {
                return (bki.GetDiscount());
            }
        }

        public String DiscountFmt
        {
            get
            {
                return (CUtil.FormatNumber(Discount.ToString()));
            }
        }

        public double TotalAmount
        {
            get
            {
                return (totalAmount);
            }

            set
            {
                totalAmount = value;
            }
        }

        public String TotalAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(TotalAmount.ToString()));
            }
        }

        public String Code
        {
            get
            {
                return (si.DisplayCode);
            }
        }

        public String Name
        {
            get
            {
                return (si.DisplayName);
            }
        }

        public Boolean IsPriced
        {
            get
            {
                return (bki.IsPriced());
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


        public String PromotionCode
        {
            get
            {
                if (pkg == null)
                {
                    return ("");
                }

                return (pkg.PackageCode);
            }
        }

        public String PromotionName
        {
            get
            {
                if (pkg == null)
                {
                    return ("");
                }

                return (pkg.PackageName);
            }
        }

        #region Method
        public void SetPromotion(MPackage promotion)
        {
            pkg = promotion;
        }
        #endregion
    }
}
