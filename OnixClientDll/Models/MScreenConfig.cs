using System;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MScreenConfig : MBaseModel
    {
        private ObservableCollection<MBaseModel> sortingColumns = new ObservableCollection<MBaseModel>();

        public MScreenConfig(CTable obj) : base(obj)
        {

        }

        public void InitScreenConfig()
        {
            sortingColumns.Clear();

            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("SORTABLE_COLUMN_ITEMS");
            if (arr == null)
            {
                return;
            }

            foreach (CTable obj in arr)
            {
                MBaseModel v = new MBaseModel(obj);
                sortingColumns.Add(v);
            }
        }

        public String UserID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("USER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("USER_ID", value);
            }
        }

        //Intend to use REPORT_CONFIG_ID column
        public String ScreenConfigID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REPORT_CONFIG_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("REPORT_CONFIG_ID", value);
            }
        }

        //Intend to use REPORT_NAME column
        public String ScreenConfigName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REPORT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("REPORT_NAME", value);
            }
        }

        public void InitSortingColumns(ObservableCollection<MBaseModel> items)
        {
            foreach (MBaseModel mb in items)
            {
                sortingColumns.Add(mb);
            }
        }

        public ObservableCollection<MBaseModel> SortingColumns
        {
            get
            {
                return (sortingColumns);
            }
        }
    }
}
