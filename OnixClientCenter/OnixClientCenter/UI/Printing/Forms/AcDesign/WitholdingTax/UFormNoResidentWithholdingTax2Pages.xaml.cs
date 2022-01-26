using System;
using Onix.Client.Model;
using Onix.Client.Helper;
using System.Windows;
using Onix.Client.Report;

namespace Onix.ClientCenter.Forms.AcDesign.WitholdingTax
{
    public partial class UFormNoResidentWithholdingTax2Pages : UFormBase
    {
        private int issueCout = 1;
        private Boolean rvTaxType3 = false;
        private Boolean rvTaxType53 = false;

        public UFormNoResidentWithholdingTax2Pages(MBaseModel model, int page, int issue ,int totalPage, MReportConfig cfg, CReportPageParam param)
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
            issueCout = issue;

            init();

            MAccountDoc ad = (dataSource as MAccountDoc);
            numberTextAmount = ad.CashReceiptAmtFmt;

            primaryColumns.Clear();

            primaryColumns.Add(new GridLength(70, GridUnitType.Star));
            primaryColumns.Add(new GridLength(14, GridUnitType.Star));
            primaryColumns.Add(new GridLength(13, GridUnitType.Star));
            primaryColumns.Add(new GridLength(13, GridUnitType.Star));

            DataContext = model;
            InitializeComponent();
        }

        public String IssueTitleLeft
        {
            get
            {
                int tmp = 2*issueCout - 1;
                return (rptConfig.GetConfigValue("Issue" + tmp.ToString()));
            }
        }

        public String IssueTitleRight
        {
            get
            {
                int tmp = 2*issueCout;
                return (rptConfig.GetConfigValue("Issue" + tmp.ToString()));
            }
        }

        public GridLength _Column1Width
        {
            get
            {
                GridLength l = new GridLength(66, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength _Column2Width
        {
            get
            {
                GridLength l = new GridLength(14, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength _Column3Width
        {
            get
            {
                GridLength l = new GridLength(15, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength _Column4Width
        {
            get
            {
                GridLength l = new GridLength(15, GridUnitType.Star);
                return (l);
            }
        }        
        
        public String FullNameSupplier
        {
            get
            {

                String txt = (((dataSource as MAccountDoc).EntityNamePrefixDesc) + ((dataSource as MAccountDoc).EntityName)).Trim();
                return (txt);
            }
        }

        public String YearTax
        {
            get
            {
                String txt = (dataSource as MAccountDoc).BEDocumentDateFmt; 
                return (txt);
            }
        }

        public String WitholdingTaxAsText
        {
            get
            {
                if (Lang.Equals("TH"))
                {
                    String txt = CUtil.CurrencyToThaiText((dataSource as MAccountDoc).WHTaxAmtFmt);
                    return (txt);
                }
                else
                {
                    String txt = CUtil.changeCurrencyToWords((dataSource as MAccountDoc).WHTaxAmtFmt);
                    return (txt);
                }
            }
        }
        
        public String RvTaxType
        {
            get
            {
                String txt = (dataSource as MAccountDoc).RvTaxType;
                return txt;
            }
        }

        public Boolean? RvTaxType3
        {
            get
            {
                String flag = (dataSource as MAccountDoc).RvTaxType;
                if (flag.Equals("3"))
                {
                    return (true);
                }
                return (false);
            }

            set
            {
                rvTaxType3 = (Boolean)value;
            }
        }

        public Boolean? RvTaxType53
        {
            get
            {
                String flag = (dataSource as MAccountDoc).RvTaxType;
                if (flag.Equals("53"))
                {
                    return (true);
                }
                return (false);
            }

            set
            {
                rvTaxType53 = (Boolean)value;
            }
        }

        public double HeaderTextSize
        {
            get
            {
                //10
                return (CUtil.StringToDouble(rptConfig.GetConfigValue("HeaderTextSize")));
            }
        }

        public double TopHeaderTextSize
        {
            get
            {
                //13
                return (CUtil.StringToDouble(rptConfig.GetConfigValue("TopHeaderTextSize")));
            }
        }

        public double BottomLeftBoxWidth
        {
            get
            {
                //220
                return (CUtil.StringToDouble(rptConfig.GetConfigValue("BottomLeftBoxWidth")));
            }
        }

        public double BottomBoxHeight
        {
            get
            {
                //160
                return (CUtil.StringToDouble(rptConfig.GetConfigValue("BottomBoxHeight")));
            }
        }

        #region CompanyTaxID
        public String CompanyTaxIDChar0
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[0].ToString();
                    }
                    catch
                    { 
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar1
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[1].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar2
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[2].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar3
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[3].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar4
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[4].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar5
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[5].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar6
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[6].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar7
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[7].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar8
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[8].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar9
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[9].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar10
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[10].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar11
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[11].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String CompanyTaxIDChar12
        {
            get
            {
                String txt = "";
                if (!CMasterReference.Instance.CompanyProfile.TaxID.Equals(""))
                {
                    var chars = CMasterReference.Instance.CompanyProfile.TaxID.ToCharArray();
                    try
                    {
                        txt = chars[12].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        #endregion

        #region EntityTaxID
        public String EntityTaxIDChar0
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[0].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }

        public String EntityTaxIDChar1
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[1].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar2
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[2].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar3
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[3].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar4
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[4].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar5
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[5].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar6
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[6].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar7
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[7].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar8
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[8].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar9
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[9].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar10
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[10].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar11
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[11].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        public String EntityTaxIDChar12
        {
            get
            {
                String txt = "";
                if (!(dataSource as MAccountDoc).EntityIDNumber.Equals(""))
                {
                    var chars = (dataSource as MAccountDoc).EntityIDNumber.ToCharArray();
                    try
                    {
                        txt = chars[12].ToString();
                    }
                    catch
                    {
                        txt = "";
                    }
                }
                return (txt);
            }
        }
        #endregion
        
    }
}
