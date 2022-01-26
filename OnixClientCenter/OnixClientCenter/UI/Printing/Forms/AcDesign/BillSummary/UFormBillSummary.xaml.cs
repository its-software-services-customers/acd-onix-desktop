using System;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using System.Windows;
using Onix.Client.Report;

namespace Onix.ClientCenter.Forms.AcDesign.BillSummary
{
    public partial class UFormBillSummary : UFormBase
    {
        private MAccountDoc ad = null;

        public UFormBillSummary(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
        {
            if (model == null)
            {
                model = new MAccountDoc(new Wis.WsClientAPI.CTable(""));
            }

            dataSource = model;
            pageNo = page;
            pageCount = totalPage;
            pageParam = param;
            rptConfig = cfg;

            init();

            ad = (dataSource as MAccountDoc);
            numberTextAmount = ad.ArApAmt;
            amountFmt = ad.ArApAmtFmt;

            if (pageNo != pageCount)
            {
                //Only last page will show the number
                numberTextAmount = "";
            }
            
            primaryColumns.Clear();

            primaryColumns.Add(new GridLength(13, GridUnitType.Star));
            primaryColumns.Add(new GridLength(26, GridUnitType.Star));
            primaryColumns.Add(new GridLength(18, GridUnitType.Star));
            primaryColumns.Add(new GridLength(18, GridUnitType.Star));
            primaryColumns.Add(new GridLength(33, GridUnitType.Star));
            primaryColumns.Add(new GridLength(20, GridUnitType.Star));

            DataContext = model;            
            InitializeComponent();

            //These 2 lines are important to place here after InitializeComponent();
            headerPanel = grdBody;
            tablePanel = stckBody;

            descriptionColumnIndex = 4;
        }

        private ObservableCollection<CReceiptItem> filterItems()
        {
            ObservableCollection<CReceiptItem> temp = new ObservableCollection<CReceiptItem>();

            int i = 0;
            foreach (MAccountDocReceipt m in pageParam.Items)
            {
                CReceiptItem d = new CReceiptItem(m, Lang, rptConfig);
                d.ItemNo = (pageParam.StartIndex + i).ToString();
                temp.Add(d);

                i++;
            }

            //int left = itemPerPage - temp.Count;

            for (i = 1; i <= pageParam.PatchRow; i++)
            {
                temp.Add(new CReceiptItem(null, Lang, rptConfig));
            }

            return (temp);            
        }

        public ObservableCollection<CReceiptItem> ItemChunks
        {
            get
            {
                return (filterItems());
            }
        }

        public int ItemCount
        {
            get
            {
                return (pageParam.TotalItemCount);
            }
        }                
    }
}
