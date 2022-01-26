using System;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using System.Windows;
using Onix.Client.Report;

namespace Onix.ClientCenter.Forms.AcDesign.Inventory
{
    public partial class UFormInventoryBorrowDoc : UFormBase
    {
        public UFormInventoryBorrowDoc(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
        {
            if (model == null)
            {
                model = new MInventoryDoc(new Wis.WsClientAPI.CTable(""));
            }

            dataSource = model;
            pageNo = page;
            pageCount = totalPage;
            pageParam = param;
            rptConfig = cfg;

            init();

            MInventoryDoc ad = (dataSource as MInventoryDoc);
            numberTextAmount = "";
            amountFmt = "";

            primaryColumns.Clear();

            primaryColumns.Add(new GridLength(10, GridUnitType.Star));
            primaryColumns.Add(new GridLength(20, GridUnitType.Star));
            primaryColumns.Add(new GridLength(40, GridUnitType.Star));
            primaryColumns.Add(new GridLength(15, GridUnitType.Star));
            primaryColumns.Add(new GridLength(15, GridUnitType.Star));

            DataContext = model;
            InitializeComponent();

            //These 2 lines are important to place here after InitializeComponent();
            headerPanel = grdBody;
            tablePanel = dckBody;

            descriptionColumnIndex = 2;
        }        

        private ObservableCollection<CInventoryDocItem> filterItems()
        {
            ObservableCollection<CInventoryDocItem> temp = new ObservableCollection<CInventoryDocItem>();

            int i = 0;
            foreach (MInventoryTransaction m in pageParam.Items)
            {
                CInventoryDocItem d = new CInventoryDocItem(m, Lang, rptConfig);
                d.ItemNo = (pageParam.StartIndex + i).ToString();
                temp.Add(d);

                i++;
            }

            for (i = 1; i <= pageParam.PatchRow; i++)
            {
                temp.Add(new CInventoryDocItem(null, Lang, rptConfig));
            }

            return (temp);
        }

        public ObservableCollection<CInventoryDocItem> ItemChunks
        {
            get
            {
                return (filterItems());
            }
        }
    }
}
