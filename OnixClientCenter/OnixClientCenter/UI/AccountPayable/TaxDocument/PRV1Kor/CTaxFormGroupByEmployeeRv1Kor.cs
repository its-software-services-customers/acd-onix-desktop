using Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV1;
using System;
using System.Collections;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV1Kor
{
    class CTaxFormGroupByEmployeeRv1Kor
    {
        private ArrayList whItems = new ArrayList();
        private MVTaxFormPRV1 groupRep = null;

        public CTaxFormGroupByEmployeeRv1Kor(MVTaxFormPRV1 firstItem)
        {
            groupRep = firstItem;
        }

        public ArrayList WhItems
        {
            get
            {
                return (whItems);
            }
        }

        public void AddWhItem(MVTaxFormPRV1 item)
        {
            whItems.Add(item);
        }

        public int ItemCount
        {
            get
            {
                return (whItems.Count);
            }
        }

        public String Name
        {
            get
            {
                return (groupRep.SupplierName);
            }
        }

        public String Address
        {
            get
            {
                return (groupRep.SupplierAddress);
            }
        }

        public String TaxID
        {
            get
            {
                return (groupRep.SupplierTaxID);
            }
        }
    }
}
