using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV3_53
{
    class CTaxFormGroupBySupplierRv3_53
    {
        private ArrayList whItems = new ArrayList();
        private MVTaxFormPRV3_53 groupRep = null;

        public CTaxFormGroupBySupplierRv3_53(MVTaxFormPRV3_53 firstItem)
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

        public void AddWhItem(MVTaxFormPRV3_53 item)
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
