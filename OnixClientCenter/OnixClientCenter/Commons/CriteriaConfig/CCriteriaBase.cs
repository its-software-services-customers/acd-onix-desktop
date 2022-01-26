using System;
using System.Collections;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.Client.Controller;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.Commons.CriteriaConfig
{
    class SortableColumnComparator : IComparer
    {
        public int Compare(object x, object y)
        {
            MCriteriaColumn bi1 = x as MCriteriaColumn;
            MCriteriaColumn bi2 = y as MCriteriaColumn;

            int no1 = CUtil.StringToInt(bi1.ItemSeqNo);
            int no2 = CUtil.StringToInt(bi2.ItemSeqNo);

            return (no1.CompareTo(no2));
        }
    }

    public interface ICriteriaConfig
    {
        ArrayList GetTableColumns();
        void AddData();
        int DeleteData(int rowCount);
        void DoubleClickData(MBaseModel m);
        Button GetButton();
        void Init(String data);
        void Initialize(String keyword);
        void SetDefaultData(MBaseModel data);
        Boolean IsAddEnable();
        Boolean IsDeleteEnable();
        MBaseModel GetModel();
        Tuple<CTable, ObservableCollection<MBaseModel>> QueryData();
    }

    public class CCriteriaBase : ICriteriaConfig
    {
        private ArrayList criteriaControls = new ArrayList();
        private ArrayList criteriaTopControls = new ArrayList();
        private ArrayList infoControls = new ArrayList();
        private ArrayList gridColumns = new ArrayList();
        private MBaseModel defaultData = null;

        protected MBaseModel model = null;
        protected String refID = "";
        private UserControl parentCtrl = null;
        private RoutedEventHandler itemAddedEvent = null;
        private MScreenConfig scrConfig = new MScreenConfig(new CTable(""));
        private String colorBindingField = "RowColor";        

        public CCriteriaBase(MBaseModel mdl, String rf)
        {            
            model = mdl;
            refID = rf;
        }

        public virtual void SetCheckUncheckHandler(RoutedEventHandler chdler, RoutedEventHandler uhdler)
        {
        }

        public virtual void SetActionEnable(Boolean en)
        {
        }

        public virtual void Initialize(String keyword)
        {
            Init("");
        }

        public ArrayList GetTableColumns()
        {
            return (null);
        }

        public virtual void AddData()
        {
        }

        public virtual int DeleteData(int rowCount)
        {
            return (0);
        }

        public virtual void DoubleClickData(MBaseModel m)
        {
        }

        public virtual void Init(String data)
        {
        }

        public virtual Boolean IsAddEnable()
        {
            return (true);
        }

        public virtual Boolean IsDeleteEnable()
        {
            return (true);
        }

        public virtual Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            return (null);
        }

        public ArrayList CriteriaEntries
        {
            get
            {
                return (criteriaControls);
            }
        }

        public ArrayList TopCriteriaEntries
        {
            get
            {
                return (criteriaTopControls);
            }
        }

        public ArrayList InfoEntries
        {
            get
            {
                return (infoControls);
            }
        }

        public ArrayList GridColumns
        {
            get
            {
                return (gridColumns);
            }
        }

        public virtual Button GetButton()
        {
            return (null);
        }

        public UserControl ParentControl
        {
            set
            {
                parentCtrl = value;
            }

            get
            {
                return (parentCtrl);
            }
        }

        public RoutedEventHandler ItemAddedEvent
        {
            get
            {
                return (itemAddedEvent);
            }

            set
            {
                itemAddedEvent = value;
            }
        }

        public MBaseModel Model
        {
            get
            {
                return (model);
            }
        }

        protected void AddCriteriaControl(CCriteriaEntry entry)
        {
            criteriaControls.Add(entry);
        }

        protected void AddTopCriteriaControl(CCriteriaEntry entry)
        {
            criteriaTopControls.Add(entry);
        }

        protected void AddInfoControl(CCriteriaEntry entry)
        {
            entry.IsEnable = false;
            infoControls.Add(entry);
        }

        protected void AddGridColumn(CCriteriaColumnBase cc)
        {
            if ((cc is CCriteriaColumnText) || (cc is CCriteriaColumnImageText))
            {
                cc.Sortable = true;
                cc.BindingColorField = colorBindingField;
            }

            gridColumns.Add(cc);
        }

        protected void SetReferenceName(String id)
        {
            refID = id;
        }

        protected String ColorBindingField
        {
            get
            {
                return (colorBindingField);
            }

            set
            {
                colorBindingField = value;
            }
        }
        private String getSettingValue(String columnName, String fieldName, String valNotFound)
        {
            foreach (MCriteriaColumn o in scrConfig.SortingColumns)
            {
                if (o.ColumnName.Equals(columnName))
                {
                    String v = (String)o.GetType().GetProperty(fieldName).GetValue(o, null);
                    return (v);
                }
            }

            return (valNotFound);
        }

        public MBaseModel GetModel()
        {
            return (model);
        }

        //Get ScreenConfig by using the same API as GetReportConfig
        public void LoadScreenConfig()
        {
            CTable o = new CTable("");
            o.SetFieldValue("REPORT_NAME", refID);
            o.SetFieldValue("USER_ID", OnixWebServiceAPI.UserID());

            ArrayList arr = OnixWebServiceAPI.GetReportConfigList(o);
            if (arr.Count <= 0)
            {
                return;
            }

            CTable obj = (CTable)arr[0];

            CTable info = OnixWebServiceAPI.GetReportConfigInfo(obj);
            scrConfig.SetDbObject(info);

            ArrayList cols = info.GetChildArray("SORTABLE_COLUMN_ITEMS");
            if (cols == null)
            {
                return;
            }
            foreach (CTable t in cols)
            {
                MCriteriaColumn v = new MCriteriaColumn(t);
                scrConfig.SortingColumns.Add(v);
            }
        }

        public void SaveCriteriaConfig(MScreenConfig cfg)
        {
            CTable tb = scrConfig.GetDbObject();

            ObservableCollection<MBaseModel> items = cfg.SortingColumns;
            scrConfig.SortingColumns.Clear();
            ArrayList sortableClmns = new ArrayList();

            foreach (MBaseModel o in items)
            {
                scrConfig.SortingColumns.Add(o);
                sortableClmns.Add(o.GetDbObject());
            }

            tb.RemoveChildArray("SORTABLE_COLUMN_ITEMS");
            tb.AddChildArray("SORTABLE_COLUMN_ITEMS", sortableClmns);

            scrConfig.UserID = OnixWebServiceAPI.UserID();
            scrConfig.ScreenConfigName = refID;
            OnixWebServiceAPI.SaveReportConfig(tb);
        }

        public void PopulateQuerySortSetting(CTable table)
        {
            String arrName = "@ORDER_BY_COLUMNS";

            table.RemoveChildArray(arrName);
            table.AddChildArray(arrName, getOrderByColumns());
        }

        public void SetDefaultData(MBaseModel data)
        {
            defaultData = data;
        }

        public MBaseModel GetDefaultData()
        {
            return (defaultData);
        }

        private ArrayList getOrderByColumns()
        {
            ObservableCollection<MBaseModel> items = scrConfig.SortingColumns;
            ArrayList arr = new ArrayList();

            foreach (MCriteriaColumn m in items)
            {
                if (m.IsUsedForSorting)
                {
                    arr.Add(m.GetDbObject());
                }
            }

            return (arr);
        }

        public ObservableCollection<MBaseModel> GetSortableColumns()
        {
            ObservableCollection<MBaseModel> items = new ObservableCollection<MBaseModel>();
            ArrayList arr = new ArrayList();

            int idx = 0;
            foreach (CCriteriaColumnBase cc in gridColumns)
            {
                if (cc.Sortable)
                {
                    MCriteriaColumn mc = new MCriteriaColumn();
                    mc.ColumnName = cc.ColumnName;
                    mc.ColumnText = CLanguage.getValue(cc.ColumnCaptionKey);
                    mc.ColumnKey = cc.ColumnCaptionKey;
                    mc.SortingOrderBy = getSettingValue(mc.ColumnName, "SortingOrderBy", "ASC");
                    mc.IsUsedForSortingFlag = getSettingValue(mc.ColumnName, "IsUsedForSortingFlag", "N");
                    mc.ItemSeqNo = getSettingValue(mc.ColumnName, "ItemSeqNo", idx.ToString());

                    arr.Add(mc);                    
                }

                idx++;
            }

            //Sort by ItemSeqNo here
            arr.Sort(new SortableColumnComparator());

            foreach (MCriteriaColumn cc in arr)
            {
                items.Add(cc);
            }

            return (items);
        }

        protected static void showWindow(CWinLoadParam param, String className)
        {
            WinBase cr = (WinBase)Activator.CreateInstance(Type.GetType(className), new object[] { param });
            cr.ShowDialog();
        }
    }
}
