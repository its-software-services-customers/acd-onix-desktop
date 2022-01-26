using System;
using Onix.Client.Model;

namespace Onix.Client.Pricing
{
    public class CPackageItemAdapter
    {
        private MBaseModel item = null;
        private MPackageBonus bonus = null;
        private MPackageBundle bundle = null;
        private MPackageVoucher voucher = null;

        public CPackageItemAdapter(MBaseModel vw)
        {
            item = vw;

            if (item is MPackageBonus)
            {
                bonus = (MPackageBonus)item;
            }
            else if (item is MPackageBundle)
            {
                bundle = (MPackageBundle) item;
            }
            else if (item is MPackageVoucher)
            {
                voucher = (MPackageVoucher)item;
            }
        }

        public String SelectionType
        {
            get
            {
                if (item is MPackageBonus)
                {
                    return (bonus.SelectionType);
                }
                else if (item is MPackageBundle)
                {
                    return (bundle.SelectionType);
                }
                else if (item is MPackageVoucher)
                {
                    return (voucher.SelectionType);
                }

                return ("");
            }
        }

        public String ItemCategoryID
        {
            get
            {
                if (item is MPackageBonus)
                {
                    return (bonus.ItemCategoryID);
                }
                else if (item is MPackageBundle)
                {
                    return (bundle.ItemCategoryID);
                }
                else if (item is MPackageVoucher)
                {
                    return (voucher.ItemCategoryID);
                }

                return ("");
            }
        }

        public String ServiceID
        {
            get
            {
                if (item is MPackageBonus)
                {
                    return (bonus.ServiceID);
                }
                else if (item is MPackageBundle)
                {
                    return (bundle.ServiceID);
                }
                else if (item is MPackageVoucher)
                {
                    return (voucher.ServiceID);
                }

                return ("");
            }
        }

        public String ItemID
        {
            get
            {
                if (item is MPackageBonus)
                {
                    return (bonus.ItemID);
                }
                else if (item is MPackageBundle)
                {
                    return (bundle.ItemID);
                }
                else if (item is MPackageVoucher)
                {
                    return (voucher.ItemID);
                }

                return ("");
            }
        }

        public String QuantityType
        {
            get
            {
                if (item is MPackageBonus)
                {
                    return (bonus.QuantityType);
                }
                else if (item is MPackageBundle)
                {
                    //QuantityType of Bundle is always "1"
                    return ("1");
                }
                else if (item is MPackageVoucher)
                {
                    return (voucher.QuantityType);
                }

                return ("");
            }
        }

        public String Quantity
        {
            get
            {
                if (item is MPackageBonus)
                {
                    return (bonus.Quantity);
                }
                else if (item is MPackageBundle)
                {
                    return (bundle.Quantity);
                }
                else if (item is MPackageVoucher)
                {
                    return (voucher.Quantity);
                }

                return ("");
            }
        }
    }
}
