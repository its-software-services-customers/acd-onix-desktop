using System.Collections.ObjectModel;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.HumanResource.OrgChart
{
    public class CWinOrgChartLoadParam : CWinLoadParam
    {
        private ObservableCollection<MVOrgChart> currentPaths = null;

        public ObservableCollection<MVOrgChart> CurrentPaths
        {
            get
            {
                return (currentPaths);
            }

            set
            {
                currentPaths = value;
            }
        }
    }
}
