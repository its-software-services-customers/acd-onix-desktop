using System;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Onix.Client.Model;

namespace Onix.Client.Pricing
{
    public class CProcessingResultJob
    {
        private CProcessingResult result = null;
        private MPackage package = null;
        private ObservableCollection<CBasketSet> childs = new ObservableCollection<CBasketSet>();
        private ObservableCollection<CToolTipItem> ttItems = new ObservableCollection<CToolTipItem>();
        //private double finalDiscount = 0.00;

        public CProcessingResultJob(CProcessingResult rs)
        {
            result = rs;
            package = rs.GetPackage();
        }

        private void createToolTipItems()
        {
            ttItems.Clear();

            if (result.GetErrorCode().Equals(""))
            {
                CToolTipItem ct1 = new CToolTipItem("error_code", "SUCCESS");
                ttItems.Add(ct1);

                return;
            }

            CToolTipItem ct = new CToolTipItem("error_code", result.GetErrorCode());
            ttItems.Add(ct);

            ct = new CToolTipItem("error_desc", CLanguage.getValue(result.GetErrorCode()));
            ttItems.Add(ct);
        }

        public ObservableCollection<CToolTipItem> ToolTipItems
        {
            get
            {
                createToolTipItems();
                return (ttItems);
            }
        }

        public ObservableCollection<CBasketSet> Items
        {
            get
            {
                return (childs);
            }

            set
            {
            }
        }

        public String ExecuteName
        {
            get
            {
                if (package == null)
                {
                    return (result.GetExecutioName());
                }

                return (package.PackageName);
            }

            set
            {
            }
        }

        public String ExecuteLabel
        {
            get
            {
                if (package == null)
                {
                    return (CLanguage.getValue("operator_name"));
                }

                return (CLanguage.getValue("package_name"));
            }

            set
            {
            }
        }

        public String PackageCode
        {
            get
            {
                if (package == null)
                {
                    return ("");
                }

                return (package.PackageCode);
            }

            set
            {
            }
        }

        public String PackageName
        {
            get
            {
                if (package == null)
                {
                    return ("");
                }

                return (package.PackageName);
            }

            set
            {
            }
        }

        public Boolean IsFinalDiscount
        {
            get
            {
                return (result.IsFinalDiscount);
            }
        }

        public String FinalDiscountFmt
        {
            get
            {
                return (result.FinalDiscountFmt);
            }
        }

        public String PackageType
        {
            get
            {
                if (package == null)
                {
                    return ("");
                }

                return (package.PackageType);
            }

            set
            {
            }
        }

        public String PackageTypeName
        {
            get
            {
                if (package == null)
                {
                    return ("");
                }

                return (package.PackageTypeName);
            }

            set
            {
            }
        }

        public String Icon
        {
            get
            {
                if (result.GetStatus() == ProcessingResultStatus.ProcessingFail)
                {

                    return ("pack://application:,,,/OnixClient;component/Images/bullet-red-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/bullet-green-icon-16.png");
            }
        }

        public Boolean ExpandSubTree
        {
            get
            {
                return (false);
            }

            set
            {
            }
        }

        #region Method

        public void AddItem(CBasketSet bs)
        {
            childs.Add(bs);
        }

        #endregion

    }
}
