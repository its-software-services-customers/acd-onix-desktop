using System;
using System.Windows;
using System.Collections;
using Onix.Client.Helper;
using Onix.Client.Model;
using Wis.WsClientAPI;

namespace Onix.Client.Pricing
{
    public class CBasketItemDisplay
    {
        private CBasketItem bki = null;
        private MSelectedItem si = null;
        private MPackage pkg = null;
        private BasketTypeEnum basketType;
        private int grpNum = 0;
        private int sequence = 0;
        private Hashtable weights = new Hashtable();
        private CBasket basket = null;
        private double amount = 0.00;
        private double totalAmount = 0.00;
        private Boolean isTray = false;

        private String extFlag = "";
        private int seq = 0;
        private String id = "";
        private int displayCategory = 0;

        private void initWeight()
        {
            weights[BasketTypeEnum.Available] = 0;
            weights[BasketTypeEnum.Bundled] = 10;
            weights[BasketTypeEnum.Priced] = 20;
            weights[BasketTypeEnum.Discounted] = 20;
            weights[BasketTypeEnum.FreeAnnonymous] = 30;
            weights[BasketTypeEnum.FreeVoucher] = 30;
            weights[BasketTypeEnum.Used] = 40;
            weights[BasketTypeEnum.AvailableTray] = 0;
            weights[BasketTypeEnum.PricedTray] = 20;
            weights[BasketTypeEnum.DiscountedTray] = 20;
            weights[BasketTypeEnum.FreeAnnonymousTray] = 30;
            weights[BasketTypeEnum.BundledTray] = 11;
            weights[BasketTypeEnum.UsedTray] = 40;
        }

        public CBasketItemDisplay(CBasketItem bi, BasketTypeEnum bt, int groupNum, int seq, CBasket bk)
        {
            initWeight();

            basket = bk;
            basketType = bt;
            grpNum = groupNum;
            sequence = seq;
            bki = bi;
            si = (MSelectedItem) bi.Item;

            amount = bki.GetAmount();
            totalAmount = bki.GetTotal();
        }

        public CBasketItemDisplay(CTable tb)
        {
            initWeight();

            basketType = (BasketTypeEnum)CUtil.StringToInt(tb.GetFieldValue("BASKET_TYPE"));
            basket = new CBasket(basketType);

            
            grpNum = CUtil.StringToInt(tb.GetFieldValue("GROUP_NO"));
            sequence = CUtil.StringToInt(tb.GetFieldValue("GROUP_NO"));

            double qty = CUtil.StringToDouble(tb.GetFieldValue("QUANTITY"));
            double amt = CUtil.StringToDouble(tb.GetFieldValue("AMOUNT"));
            double tot = CUtil.StringToDouble(tb.GetFieldValue("TOTAL_AMOUNT"));
            double discount = CUtil.StringToDouble(tb.GetFieldValue("DISCOUNT"));
            Boolean isPriced = tb.GetFieldValue("IS_PRICED").Equals("Y");

            String code = tb.GetFieldValue("CODE");

            sequence = 1;
            id = tb.GetFieldValue("BILL_SIM_DISPLAY_ID");
            //CTable o = new CTable("");

            si = new MSelectedItem(tb);
            si.TrayFlag = tb.GetFieldValue("IS_TRAY");
            isTray = si.TrayFlag.Equals("Y");
            si.EnabledFlag = "Y";
            si.VoucherID = tb.GetFieldValue("VOUCHER_ID");
            si.VoucherNo = tb.GetFieldValue("VOUCHER_CODE");
            si.VoucherName = tb.GetFieldValue("VOUCHER_NAME");
            si.FreeText = tb.GetFieldValue("FREE_TEXT");

            bki = new CBasketItem(code, si, qty, amt);
            bki.SetDiscount(discount);
            if (qty != 0)
            {
                if (isPriced)
                {
                    bki.SetUnitPrice(amt / qty);
                }
            }

            amount = amt;
            totalAmount = tot;
        }


        public String ID
        {
            get
            {
                return (id);
            }

            set
            {
                id = value;
            }
        }

        public String ExtFlag
        {
            get
            {
                return (extFlag);
            }

            set
            {
                extFlag = value;
            }
        }

        public int Seq
        {
            get
            {
                return (seq);
            }

            set
            {
                seq = value;
            }
        }

        public int BasketTypeWeight
        {
            get
            {
                return ((int) weights[basketType]);
            }
        }

        public BasketTypeEnum BasketType
        {
            get
            {
                return (basketType);
            }
        }

        public FontWeight DisplayFontWeight
        {
            get
            {
                if (BasketType == BasketTypeEnum.Bundled)
                {
                    return (FontWeights.Black);
                }
                else if (BasketType == BasketTypeEnum.BundledTray)
                {
                    return (FontWeights.Black);
                }

                return (FontWeights.Normal);
            }
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

        #region for Calculate Bill
        public String ItemID
        {
            get
            {
                return (si.ItemID);
            }
        }
        public String ItemCode
        {
            get
            {
                return (si.ItemCode);
            }
        }
        public String ItemName
        {
            get
            {
                return (si.ItemNameThai);
            }
        }
        public String ServiceID
        {
            get
            {
                return (si.ServiceID);
            }
        }
        public String ServiceCode
        {
            get
            {
                return (si.ServiceCode);
            }
        }
        public String ServiceName
        {
            get
            {
                return (si.ServiceName);
            }
        }
        public String ItemCategoryID
        {
            get
            {
                return (si.ItemCategory);
            }
        }
        public String SelectionType
        {
            get
            {
                return (si.SelectionType);
            }
        }

        public int DisplayCategory
        {
            get
            {
                return (displayCategory);
            }

            set
            {
                displayCategory = value;
            }
        }

        #endregion

        #region Method
        public void SetPromotion(MPackage promotion)
        {
            pkg = promotion;
        }

        public CTable CreateCTableObject()
        {
            CTable o = new CTable("");

            o.SetFieldValue("EXT_FLAG", "A");
            o.SetFieldValue("SELECTION_TYPE", SelectionType);
            o.SetFieldValue("ITEM_ID", ItemID);
            o.SetFieldValue("ITEM_NAME_THAI", ItemName);
            o.SetFieldValue("ITEM_CODE", ItemCode);
            o.SetFieldValue("ITEM_CATEGORY_ID", ItemCategoryID);
            o.SetFieldValue("SERVICE_ID", ServiceID);
            o.SetFieldValue("SERVICE_NAME", ServiceName);
            o.SetFieldValue("SERVICE_CODE", ServiceCode);
            o.SetFieldValue("CODE", Code);
            o.SetFieldValue("NAME", Name);

            o.SetFieldValue("VOUCHER_ID", si.VoucherID);
            o.SetFieldValue("VOUCHER_NAME", si.VoucherName);
            o.SetFieldValue("VOUCHER_CODE", si.VoucherNo);
            o.SetFieldValue("FREE_TEXT", si.FreeText);

            String flag = "N";
            if (IsTray)
            {
                flag = "Y";
            }
            o.SetFieldValue("IS_TRAY", flag);

            String pricedFlag = "N";
            if (IsPriced)
            {
                pricedFlag = "Y";
            }
            o.SetFieldValue("IS_PRICED", pricedFlag);

            o.SetFieldValue("TOTAL_AMOUNT", TotalAmount.ToString());
            o.SetFieldValue("DISCOUNT", Discount.ToString());
            o.SetFieldValue("AMOUNT", Amount.ToString());
            o.SetFieldValue("QUANTITY", Quantity.ToString());
            o.SetFieldValue("BASKET_TYPE", ((int) BasketType).ToString());
            o.SetFieldValue("BUNDLE_AMOUNT", BundledAmount.ToString());
            o.SetFieldValue("GROUP_NO", GroupNo.ToString());
            o.SetFieldValue("SEQUENCE", Sequence.ToString());
            o.SetFieldValue("PROMOTION_CODE", Sequence.ToString());
            o.SetFieldValue("PROMOTION_NAME", Sequence.ToString());
            o.SetFieldValue("DISPLAY_CATEGORY", DisplayCategory.ToString());

            return (o);
        }
#endregion
    }
}
