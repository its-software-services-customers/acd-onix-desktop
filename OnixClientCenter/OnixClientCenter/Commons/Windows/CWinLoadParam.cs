using System;
using Onix.Client.Model;
using System.Collections.ObjectModel;
using Onix.ClientCenter.UI.HumanResource.OrgChart;

namespace Onix.ClientCenter.Commons.Windows
{
    public class CWinLoadParam
    {
        public String Mode { get; set; }
        public String Caption { get; set; }
        public MBaseModel ActualView { get; set; }
        public MBaseModel ActualParentView { get; set; }
        public ObservableCollection<MBaseModel> ParentItemSources { get; set; }
        public String GenericType { get; set; }
        public Boolean GenericFlag { get; set; }
    }
}
