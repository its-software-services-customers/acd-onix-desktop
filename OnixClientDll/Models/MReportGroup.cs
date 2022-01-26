using System;
using Wis.WsClientAPI;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Helper;
using System.Windows;

namespace Onix.Client.Model
{
    public class MReportGroup : MBaseModel
    {
        private ObservableCollection<MReportFilter> items = new ObservableCollection<MReportFilter>();

        public MReportGroup(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();
        }

        public void CreateDefaultValue()
        {
        }

        public ObservableCollection<MReportFilter> Items
        {
            get
            {
                return (items);
            }
        }

        public void AddItem(MReportFilter mi)
        {
            items.Add(mi);
        }

        public String GroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("GROUP_NAME", value);
                NotifyPropertyChanged();
            }
        }
    }
}
