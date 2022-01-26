using System;
using System.Collections;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV3_53
{
    class CTaxFormPageRv3_53
    {
        private ArrayList groups = new ArrayList();

        public CTaxFormPageRv3_53()
        {
        }

        public void AddGroup(CTaxFormGroupBySupplierRv3_53 grp)
        {
            groups.Add(grp);
        }

        public int GroupCount
        {
            get
            {
                return (groups.Count);
            }
        }

        public ArrayList Groups
        {
            get
            {
                return (groups);
            }
        }
    }
}
