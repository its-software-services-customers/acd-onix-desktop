using System;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Helper;
using Wis.WsClientAPI;


namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PP30
{
    public class MVTaxFormPP30 : MBaseModel
    {
        private ObservableCollection<MMasterRef> forms = new ObservableCollection<MMasterRef>();

        public MVTaxFormPP30() : base(new CTable(""))
        {

        }

        public void NotifyPopulatedFields()
        {
            NotifyPropertyChanged("SaleAmt");
            NotifyPropertyChanged("SaleVatAmt");
            NotifyPropertyChanged("PurchaseEligibleAmt");
            NotifyPropertyChanged("PurchaseVatAmt");
        }

        private void calculateCorrespondFields()
        {
            double amt1 = CUtil.StringToDouble(SaleAmt);
            double amt2 = CUtil.StringToDouble(SaleZeroPctAmt);
            double amt3 = CUtil.StringToDouble(SaleExemptAmt);

            double amt5 = CUtil.StringToDouble(SaleVatAmt);
            double amt6 = CUtil.StringToDouble(PurchaseEligibleAmt);
            double amt7 = CUtil.StringToDouble(PurchaseVatAmt);

            double amt8 = 0.00;
            double amt9 = 0.00;

            
            if (amt5 > amt7)
            {
                amt8 = amt5 - amt7;
            }
            else
            {
                amt9 = amt7 - amt5;
            }

            double saleEligibleAmount = amt1 - amt2 - amt3;

            SaleEligibleAmt = saleEligibleAmount.ToString();
            VatClaimAmt = amt8.ToString();
            VatExtraAmt = amt9.ToString();

            Boolean refundFlag = true;

            double amt11 = 0.00;
            double amt10 = CUtil.StringToDouble(VatPreviousFwdAmt);
            if (amt8 > amt10)
            {
                //vat ขาย > vat ซื้อ
                refundFlag = false;
                amt11 = amt8 - amt10;
            }

            double amt12 = 0.00;
            if (refundFlag)
            {
                amt12 = amt10 + amt9 - amt8;
            }

            double amt13 = CUtil.StringToDouble(AdditionalAmt);
            double amt14 = CUtil.StringToDouble(PenaltyAmt);

            double finalAmt = amt11 + (amt13 + amt14) - amt12;
            double amt15 = 0.00;
            double amt16 = 0.00;

            if (finalAmt > 0)
            {
                amt15 = finalAmt;
            }
            else
            {
                amt16 = Math.Abs(finalAmt);
            }

            VatClaimTotalAmt = amt11.ToString();
            VatExtraTotalAmt = amt12.ToString();
            VatClaimGrandAmt = amt15.ToString();
            VatExtraGrandAmt = amt16.ToString();

            NotifyPropertyChanged("SaleEligibleAmt");
            NotifyPropertyChanged("VatClaimAmt");
            NotifyPropertyChanged("VatExtraAmt");
            NotifyPropertyChanged("VatClaimTotalAmt");
            NotifyPropertyChanged("VatExtraTotalAmt");
            NotifyPropertyChanged("VatClaimGrandAmt");
            NotifyPropertyChanged("VatExtraGrandAmt");
        }

        public String PenaltyAmt
        {
            get
            {
                return (GetFieldValue("PENALTY_AMOUNT"));
            }

            set
            {
                SetFieldValue("PENALTY_AMOUNT", value);
                calculateCorrespondFields();
            }
        }

        public String AdditionalAmt
        {
            get
            {
                return (GetFieldValue("ADDITIONAL_AMOUNT"));
            }

            set
            {
                SetFieldValue("ADDITIONAL_AMOUNT", value);
                calculateCorrespondFields();
            }
        }

        #region Vat

        public String VatClaimGrandAmt
        {
            get
            {
                return (GetFieldValue("VAT_CLAIM_GRAND_AMOUNT"));
            }

            set
            {
                SetFieldValue("VAT_CLAIM_GRAND_AMOUNT", value);
            }
        }

        public String VatExtraGrandAmt
        {
            get
            {
                return (GetFieldValue("VAT_EXTRA_GRAND_AMOUNT"));
            }

            set
            {
                SetFieldValue("VAT_EXTRA_GRAND_AMOUNT", value);
            }
        }

        public String VatClaimTotalAmt
        {
            get
            {
                return (GetFieldValue("VAT_CLAIM_TOTAL_AMOUNT"));
            }

            set
            {
                SetFieldValue("VAT_CLAIM_TOTAL_AMOUNT", value);
            }
        }

        public String VatExtraTotalAmt
        {
            get
            {
                return (GetFieldValue("VAT_EXTRA_TOTAL_AMOUNT"));
            }

            set
            {
                SetFieldValue("VAT_EXTRA_TOTAL_AMOUNT", value);
            }
        }

        public String VatPreviousFwdAmt
        {
            get
            {
                return (GetFieldValue("VAT_PREV_FWD_AMOUNT"));
            }

            set
            {
                SetFieldValue("VAT_PREV_FWD_AMOUNT", value);
                calculateCorrespondFields();
            }
        }

        public String VatClaimAmt
        {
            get
            {
                return (GetFieldValue("VAT_CLAIM_AMOUNT"));
            }

            set
            {
                SetFieldValue("VAT_CLAIM_AMOUNT", value);
            }
        }

        public String VatExtraAmt
        {
            get
            {
                return (GetFieldValue("VAT_EXTRA_AMOUNT"));
            }

            set
            {
                SetFieldValue("VAT_EXTRA_AMOUNT", value);
            }
        }

        #endregion

        #region Purchase

        public String PurchaseEligibleAmt
        {
            get
            {
                return (GetFieldValue("PURCHASE_ELIGIBLE_AMOUNT"));
            }

            set
            {
                SetFieldValue("PURCHASE_ELIGIBLE_AMOUNT", value);
                calculateCorrespondFields();
            }
        }

        public String PurchaseVatAmt
        {
            get
            {
                return (GetFieldValue("PURCHASE_VAT_AMOUNT"));
            }

            set
            {
                SetFieldValue("PURCHASE_VAT_AMOUNT", value);
                calculateCorrespondFields();
            }
        }

        #endregion

        #region Sale

        public String SaleAmt
        {
            get
            {
                return (GetFieldValue("SALE_AMOUNT"));
            }

            set
            {
                SetFieldValue("SALE_AMOUNT", value);
                calculateCorrespondFields();
            }
        }

        public String SaleZeroPctAmt
        {
            get
            {
                return (GetFieldValue("SALE_ZERO_PCT_AMOUNT"));
            }

            set
            {
                SetFieldValue("SALE_ZERO_PCT_AMOUNT", value);
                calculateCorrespondFields();
            }
        }

        public String SaleExemptAmt
        {
            get
            {
                return (GetFieldValue("SALE_EXEMPT_AMOUNT"));
            }

            set
            {
                SetFieldValue("SALE_EXEMPT_AMOUNT", value);
                calculateCorrespondFields();
            }
        }

        public String SaleEligibleAmt
        {
            get
            {
                return (GetFieldValue("SALE_ELIGIBLE_AMOUNT"));
            }

            set
            {
                SetFieldValue("SALE_ELIGIBLE_AMOUNT", value);
            }
        }

        public String SaleVatAmt
        {
            get
            {
                return (GetFieldValue("SALE_VAT_AMOUNT"));
            }

            set
            {
                SetFieldValue("SALE_VAT_AMOUNT", value);
                calculateCorrespondFields();
            }
        }

        #endregion

        #region Year and Month

        //public MMasterRef MonthObj
        //{
        //    get
        //    {
        //        return (currentMonth);
        //    }

        //    set
        //    {
        //        currentMonth = value;
        //    }
        //}

        //public String Month
        //{
        //    get
        //    {
        //        return (GetFieldValue("MONTH"));
        //    }

        //    set
        //    {
        //        SetFieldValue("MONTH", value);
        //    }
        //}

        //public String Year
        //{
        //    get
        //    {
        //        return (GetFieldValue("YEAR"));
        //    }

        //    set
        //    {
        //        SetFieldValue("YEAR", value);
        //    }
        //}

        #endregion
    }
}
