using Onix.Client.Model;

namespace Onix.Client.Pricing
{
    public class CBaseBasket
    {
        private MPackage appliedPkg = null;

        public void SetAppliedPackage(MPackage p)
        {
            appliedPkg = p;
        }

        public MPackage GetAppliedPackage()
        {
            return (appliedPkg);
        }
    }
}
