using System;
using System.Collections.ObjectModel;

namespace Onix.Client.Pricing
{
    public class CProcessingResultGroup
    {
        private ObservableCollection<CProcessingResultJob> childs = new ObservableCollection<CProcessingResultJob>();
        private CProcessingResult result = null;

        public CProcessingResultGroup(CProcessingResult rs)
        {
            result = rs;
        }

        public ObservableCollection<CProcessingResultJob> Items
        {
            get
            {
                return (childs);
            }

            set
            {
            }
        }

        public String GroupName
        {
            get
            {
                return (result.GetGroupName());
            }

            set
            {
                result.SetGroupName(value);
            }
        }

        public Boolean IsOperation
        {
            get
            {
                return (result.IsOperation);
            }

            set
            {

            }
        }

        public String Icon
        {
            get
            {
                if (IsOperation)
                {

                    return ("pack://application:,,,/OnixClient;component/Images/add-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/Apps-Run-icon.png");
            }
        }

        public Boolean ExpandSubTree
        {
            get
            {
                return (true);
            }

            set
            {
            }
        }

        public String FinalDiscountFmt
        {
            get
            {
                return (result.FinalDiscountFmt);
            }
        }

        #region Method

        public void AddItem(CProcessingResultJob job)
        {
            childs.Add(job);
        }

        #endregion

    }
}
