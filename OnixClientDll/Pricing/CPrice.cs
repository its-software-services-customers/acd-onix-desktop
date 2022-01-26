using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace Onix.Client.Pricing
{
    public class CPrice
    {
        private double unitPrice = 0.00;
        private double totAmt = 0.00;
        private double quantity = 0.00;

        public CPrice()
        {
        }
        
        public double UnitPrice
        {
            get
            {
                return(unitPrice);
            }

            set
            {
                unitPrice = value;
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

        public double Quantity
        {
            get
            {
                return (quantity);
            }

            set
            {
                quantity = value;
            }
        }

        public double DiscountAmount
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
    }
}
