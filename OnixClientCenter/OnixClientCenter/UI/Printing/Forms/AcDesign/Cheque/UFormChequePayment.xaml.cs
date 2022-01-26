using System;
using Onix.Client.Model;
using Onix.Client.Helper;
using System.Threading;
using Onix.Client.Report;

namespace Onix.ClientCenter.Forms.AcDesign.Cheque
{
    public partial class UFormChequePayment : UFormBase
    {
        public UFormChequePayment(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
        {
            if (model == null)
            {
                model = new MCheque(new Wis.WsClientAPI.CTable(""));
            }

            dataSource = model;
            pageNo = page;
            pageCount = totalPage;
            pageParam = param;
            rptConfig = cfg;

            init();

            MCheque ad = (dataSource as MCheque);
            numberTextAmount = ad.ChequeAmountFmt;

            DataContext = model;
            InitializeComponent();
        }
        
        public String ChequeAmountFmt
        {
            get
            {
                String amt = (dataSource as MCheque).ChequeAmount;
                String txt = CUtil.CurrencyChequeAmtFmt(amt);

                return (txt);
            }
        }       

        public String IsAcPayeeOnly
        {
            get
            {
                String txt = "";
                Boolean? IsAcPayeeOnly = false;
                IsAcPayeeOnly = (dataSource as MCheque).IsAcPayeeOnly;
                if (IsAcPayeeOnly.Equals(true))
                {
                    txt = rptConfig.GetConfigValue("IsAcPayeeOnlyText");
                }

                return (txt);
            }

        }

        public String ChequeDateThai
        {
            get
            {
                System.Globalization.CultureInfo th = new System.Globalization.CultureInfo("th-TH");
                Thread.CurrentThread.CurrentCulture = th;
                String format = "dd MMMM yyyy";

                String str = (dataSource as MCheque).ChequeDate.ToString(format);

                return str;
            }
        }       
        
        public String MaskingText
        {
            get
            {
                string p = rptConfig.GetConfigValue("MaskingText");
                return (p);
            }
        }

        public String PositionChequeDateFmt
        {
            get
            {
                string p = rptConfig.GetConfigValue("PositionChequeDateFmt") + ",0,0";
                return (p);
            }
        }
        
        public String PositionHolderMask
        {
            get
            {
                string p = rptConfig.GetConfigValue("PositionHolderMask") + ",0,0";
                return (p);
            }
        }

        public String PositionCompany
        {
            get
            {
                string p = rptConfig.GetConfigValue("PositionCompany") + ",0,0";
                return (p);
            }
        }

        public String PositionPayeeName
        {
            get
            {
                string p = rptConfig.GetConfigValue("PositionPayeeName") + ",0,0";
                return (p);
            }
        }

        public String PositionNumberAsText
        {
            get
            {
                string p = rptConfig.GetConfigValue("PositionNumberAsText") + ",0,0";
                return (p);
            }
        }

        public String PositionChequeAmountFmt
        {
            get
            {
                string p = rptConfig.GetConfigValue("PositionChequeAmountFmt") + ",0,0";
                return (p);
            }
        }

        public String PositionAcPayeeOnly
        {
            get
            {
                string p = rptConfig.GetConfigValue("PositionAcPayeeOnly") + ",0,0";
                return (p);
            }
        }
    }
}
