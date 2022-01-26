using System;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Helper;
using System.Windows;
using Onix.Client.Report;

namespace Onix.ClientCenter.Forms.AcDesign.PaymentVoucher
{
    public partial class UPaymentVoucher : UFormBase
    {
        public UPaymentVoucher(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
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

            MAccountDoc ad = (dataSource as MAccountDoc);
            numberTextAmount = ad.CashReceiptAmtFmt;

            primaryColumns.Clear();

            primaryColumns.Add(new GridLength(15, GridUnitType.Star));
            primaryColumns.Add(new GridLength(15, GridUnitType.Star));
            primaryColumns.Add(new GridLength(14, GridUnitType.Star));
            primaryColumns.Add(new GridLength(12, GridUnitType.Star));

            primaryColumns.Add(new GridLength(9, GridUnitType.Star));
            primaryColumns.Add(new GridLength(11, GridUnitType.Star));
            primaryColumns.Add(new GridLength(11, GridUnitType.Star));
            primaryColumns.Add(new GridLength(13, GridUnitType.Star));

            DataContext = model;
            InitializeComponent();

            //These 2 lines are important to place here after InitializeComponent();
            headerPanel = grdBody;
            tablePanel = stckBody;

            descriptionColumnIndex = 1;
        }

        #region PayTable
        public GridLength ColumnPay1Width
        {
            get
            {
                GridLength l = new GridLength(16, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength ColumnPay2Width
        {
            get
            {
                GridLength l = new GridLength(17, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength ColumnPay3Width
        {
            get
            {
                GridLength l = new GridLength(15, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength ColumnPay4Width
        {
            get
            {
                GridLength l = new GridLength(30, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength ColumnPay5Width
        {
            get
            {
                GridLength l = new GridLength(17, GridUnitType.Star);
                return (l);
            }
        }
        #endregion

        #region RefDoc
        public GridLength ColumnRef1Width
        {
            get
            {
                GridLength l = new GridLength(35, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength ColumnRef2Width
        {
            get
            {
                GridLength l = new GridLength(35, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength ColumnRef3Width
        {
            get
            {
                GridLength l = new GridLength(30, GridUnitType.Star);
                return (l);
            }
        }
        #endregion

        private ObservableCollection<CDocReceipt> filterItems()
        {
            ObservableCollection<CDocReceipt> temp = new ObservableCollection<CDocReceipt>();
            MAccountDoc md = (MAccountDoc)dataSource;

            int i = 0;
            foreach (MAccountDocReceipt m in pageParam.Items)
            {
                CDocReceipt d = new CDocReceipt(m, Lang);
                d.ItemNo = (pageParam.StartIndex + i).ToString();
                d.docType = md.DocumentType;
                temp.Add(d);

                i++;
            }

            //int left = itemPerPage - temp.Count;

            for (i = 1; i <= pageParam.PatchRow; i++)
            {
                temp.Add(new CDocReceipt(null, Lang));
            }

            return (temp);            
        }

        private ObservableCollection<CDocReceipt> filterSingleItems()
        {
            ObservableCollection<CDocReceipt> temp = new ObservableCollection<CDocReceipt>();
            MAccountDoc md = (MAccountDoc)dataSource;

            int i = 0;
            foreach (MAccountDocItem di in pageParam.Items) //มีได้มากกว่า 1 รายการ แยกตาม PO
            {
                MAccountDocReceipt m = new MAccountDocReceipt(new Wis.WsClientAPI.CTable(""));
                double amt = CUtil.StringToDouble(di.RevenueExpenseAmt) + CUtil.StringToDouble(di.VatTaxAmt) -
                    CUtil.StringToDouble(di.WHTaxAmt);

                m.DocumentNo = md.RefDocNo;
                m.RefPoNo = di.RefPoNo;
                m.ProjectCode = di.ProjectCode;
                m.PricingAmt = di.RevenueExpenseAmt; //di.TotalAfterDiscount;
                m.FinalDiscount = di.FinalDiscountAvg;
                m.VatAmt = di.VatTaxAmt;
                m.WHTaxAmt = di.WHTaxAmt;
                m.CashReceiptAmt = amt.ToString(); //di.TotalAmt;

                CDocReceipt d = new CDocReceipt(m, Lang);
                d.ItemNo = (pageParam.StartIndex + i).ToString();
                d.docType = md.DocumentType;

                temp.Add(d);

                i++;
            }

            for (i = 1; i <= pageParam.PatchRow; i++)
            {
                temp.Add(new CDocReceipt(null, Lang));
            }

            return (temp);
        }

        public ObservableCollection<CDocReceipt> ItemChunks
        {
            get
            {
                AccountDocumentType dt = (AccountDocumentType) CUtil.StringToInt((dataSource as MAccountDoc).DocumentType);

                if ((dt == AccountDocumentType.AcctDocApReceipt) || (dt == AccountDocumentType.AcctDocArReceipt))
                {
                    return (filterItems());
                }

                return (filterSingleItems());
            }
        }        

        private ObservableCollection<CDocPayment> filterPayments()
        {
            ObservableCollection<CDocPayment> temp = new ObservableCollection<CDocPayment>();

            int itemPerPage = ItemPerPage;
            int start = (pageNo - 1) * itemPerPage + 1;
            int end = start + itemPerPage - 1;

            ObservableCollection<MAccountDocPayment> arr = (dataSource as MAccountDoc).PaymentItemsNoChange;
            int i = 0;
            foreach (MAccountDocPayment m in arr)
            {
                if (m.ExtFlag.Equals("D"))
                {
                    continue;
                }

                i++;

                if ((i >= start) && (i <= end))
                {
                    CDocPayment d = new CDocPayment(m, Lang);
                    d.ItemNo = i.ToString();

                    temp.Add(d);
                }
            }

            int left = itemPerPage - temp.Count;

            for (i = 1; i <= left; i++)
            {
                temp.Add(new CDocPayment(null, Lang));
            }

            return (temp);
        }

        public ObservableCollection<CDocPayment> PaymentChunks
        {
            get
            {
                return (filterPayments());
            }
        }
    }
}
