using System;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using System.Windows;
using Onix.Client.Report;

namespace Onix.ClientCenter.Forms.AcDesign.PurchaseOrder
{
    public partial class UFormPurchaseOrder : UFormBase
    {
        public UFormPurchaseOrder(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
        {
            if (model == null)
            {
                model = new MAuxilaryDoc(new Wis.WsClientAPI.CTable(""));
            }

            dataSource = model;
            pageNo = page;
            pageCount = totalPage;
            pageParam = param;
            rptConfig = cfg;


            init();

            MAuxilaryDoc ad = (dataSource as MAuxilaryDoc);
            numberTextAmount = ad.ArApAmtFmt;

            primaryColumns.Clear();

            primaryColumns.Add(new GridLength(6, GridUnitType.Star));
            primaryColumns.Add(new GridLength(42, GridUnitType.Star));
            primaryColumns.Add(new GridLength(8, GridUnitType.Star));
            primaryColumns.Add(new GridLength(8, GridUnitType.Star));
            primaryColumns.Add(new GridLength(10, GridUnitType.Star));
            primaryColumns.Add(new GridLength(7, GridUnitType.Star));
            primaryColumns.Add(new GridLength(9, GridUnitType.Star));

            (model as MAuxilaryDoc).IsLastPage = (page == totalPage);

            DataContext = model;
            InitializeComponent();

            //These 2 lines are important to place here after InitializeComponent();
            headerPanel = grdBody;
            tablePanel = dckBody;

            descriptionColumnIndex = 1;
        }

        private ObservableCollection<CAuxDocItem> filterItems()
        {
            ObservableCollection<CAuxDocItem> temp = new ObservableCollection<CAuxDocItem>();

            int i = 0;
            foreach (MAuxilaryDocItem m in pageParam.Items)
            {
                CAuxDocItem d = new CAuxDocItem(m, Lang, rptConfig);
                d.ItemNo = (pageParam.StartIndex + i).ToString();
                temp.Add(d);

                i++;
            }

            //int left = itemPerPage - temp.Count;

            for (i = 1; i <= pageParam.PatchRow; i++)
            {
                temp.Add(new CAuxDocItem(null, Lang, rptConfig));
            }

            return (temp);
        }

        public ObservableCollection<CAuxDocItem> ItemChunks
        {
            get
            {                
                return (filterItems());
            }
        }
    }
}
