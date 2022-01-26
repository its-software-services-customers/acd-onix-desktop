using System;
using System.Collections;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV1
{
    class CTaxFormPageRv1
    {
        private ArrayList groups = new ArrayList();

        public CTaxFormPageRv1()
        {
        }

        public void AddGroup(CTaxFormGroupByEmployeeRv1 grp)
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
