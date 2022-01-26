using System;
using System.Collections;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV1Kor
{
    class CTaxFormPageRv1Kor
    {
        private ArrayList groups = new ArrayList();

        public CTaxFormPageRv1Kor()
        {
        }

        public void AddGroup(CTaxFormGroupByEmployeeRv1Kor grp)
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
