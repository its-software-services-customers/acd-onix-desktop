using System;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Helper;
using System.Windows;
using Onix.Client.Report;

namespace Onix.ClientCenter.Forms.AcDesign.PurchaseOrder
{
    public partial class UFormPurchaseOrderService : UFormBase
    {
        public UFormPurchaseOrderService(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
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

            primaryColumns.Add(new GridLength(31, GridUnitType.Star));
            primaryColumns.Add(new GridLength(9, GridUnitType.Star));
            primaryColumns.Add(new GridLength(9, GridUnitType.Star));
            primaryColumns.Add(new GridLength(9, GridUnitType.Star));
            primaryColumns.Add(new GridLength(9, GridUnitType.Star));
            primaryColumns.Add(new GridLength(9, GridUnitType.Star));
            primaryColumns.Add(new GridLength(8, GridUnitType.Star));
            primaryColumns.Add(new GridLength(8, GridUnitType.Star));
            primaryColumns.Add(new GridLength(8, GridUnitType.Star));

            DataContext = model;
            InitializeComponent();
        }

        private ObservableCollection<CPaymentCriteria> filterItems()
        {
            ObservableCollection<CPaymentCriteria> temp = new ObservableCollection<CPaymentCriteria>();

            int i = 0;
            foreach (MPaymentCriteria m in pageParam.Items)
            {
                CPaymentCriteria d = new CPaymentCriteria(m);
                d.ItemNo = (pageParam.StartIndex + i).ToString();
                temp.Add(d);

                i++;
            }

            //int left = itemPerPage - temp.Count;

            for (i = 1; i <= pageParam.PatchRow; i++)
            {
                temp.Add(new CPaymentCriteria(null));
            }

            return (temp);
        }

        public ObservableCollection<CPaymentCriteria> ItemChunks
        {
            get
            {
                return (filterItems());
            }
        }      

        #region Label
        
        public new String LbDeliveryDate
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "delivery_date");
                return (txt);
            }
        }
        
        public String LbShipTo
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "ship_to");
                return (txt);
            }
        }        

        public String LbCompany_person
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "job_detail");
                return (txt);
            }
        }       

        public String LbTHB
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "Bath");
                return (txt);
            }
        }

        public String LbJobDetail
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "job_detail");
                return (txt);
            }
        }

        public String LbMoneyBeforeVAT
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "money_before_vat");
                return (txt);
            }
        }

        public String LbVAT7
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "vat7");
                return (txt);
            }
        }

        public String LbMoneyAllVAT7
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "money_vat7");
                return (txt);
            }
        }

        public String LbTaxWH3
        { 
            get
            {
                String txt = CLanguage.getValue(Lang, "tax_WH3");
                return (txt);
            }
        }

        public String LbBalanceMoney
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "balance_money");
                return (txt);
            }
        }

        public String LbSaleDepartment
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "sale_department");
                return (txt);
            }
        }

        public String LbCommitteeApproved
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "committee_approved");
                return (txt);
            }
        }

        public String LbAccountDepartment
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "account_department");
                return (txt);
            }
        }

        public String LbTotalBalance
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "total_balance");
                return (txt);
            }
        }

        public String LbNetAmount
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "net_amount");
                return (txt);
            }
        }

        #endregion

        public void DisplayQR()
        {
            MAuxilaryDoc ad = (dataSource as MAuxilaryDoc);

            string ppid = ad.PromptPayID;
            if (string.IsNullOrEmpty(ppid))
            {
                return;
            }

            var items = ad.PaymentCriteriaes;
            if ((items == null) || (items.Count <= 0))
            {
                uPromptPayQR.Amount = "0.00";
            }
            else
            {
                MPaymentCriteria mp = items[items.Count - 1];
                uPromptPayQR.Amount = mp.RemainAmount;
            }

            uPromptPayQR.PropmtPayID = ppid;            
            uPromptPayQR.GenerateQR();
        }

        private void RootElement_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
