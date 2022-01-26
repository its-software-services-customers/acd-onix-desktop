using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.Client.Report;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Onix.ClientCenter.Forms.AcDesign.Quotation
{
    public partial class UFormQuotationAttachSheet : UFormBase
    {
        public UFormQuotationAttachSheet(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
        {
            dataSource = model;
            pageNo = page;
            pageCount = totalPage;
            pageParam = param;
            rptConfig = cfg;

            init();

            MAuxilaryDoc ad = (dataSource as MAuxilaryDoc);
            if (ad != null)
            {
                numberTextAmount = ad.ArApAmtFmt;
                amountFmt = ad.ArApAmtFmt;
            }

            primaryColumns.Clear();

            primaryColumns.Add(new GridLength(10, GridUnitType.Star));
            primaryColumns.Add(new GridLength(100, GridUnitType.Star));
            primaryColumns.Add(new GridLength(20, GridUnitType.Star));
            primaryColumns.Add(new GridLength(20, GridUnitType.Star));
            primaryColumns.Add(new GridLength(25, GridUnitType.Star));
            primaryColumns.Add(new GridLength(15, GridUnitType.Star));

            DataContext = model;
            InitializeComponent();

            //These 2 lines are important to place here after InitializeComponent();
            headerPanel = grdBody;
            tablePanel = dckBody;

            descriptionColumnIndex = 1;
        }

        private ObservableCollection<MAuxilaryDocSubItem> filterItems()
        {
            ObservableCollection<MAuxilaryDocSubItem> temp = new ObservableCollection<MAuxilaryDocSubItem>();

            foreach (MAuxilaryDocSubItem m in pageParam.Items)
            {
                temp.Add(m);
            }

            return (temp);
        }

        public ObservableCollection<MAuxilaryDocSubItem> ItemChunks
        {
            get
            {
                return (filterItems());
            }
        }        
    }
}
