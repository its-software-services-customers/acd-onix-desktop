using System;
using Onix.Client.Model;
using Onix.Client.Helper;
using System.Windows;
using Onix.Client.Report;

namespace Onix.ClientCenter.Forms.AcDesign.HREmployeeTax
{
    public partial class UFormEmployeeWithholdingTax : UFormBase
    {
        private int issueCout = 1;

        public UFormEmployeeWithholdingTax(MBaseModel model, int page, int issue ,int totalPage, MReportConfig cfg, CReportPageParam param)
        {
            if (model == null)
            {
                model = new MEmployee(new Wis.WsClientAPI.CTable(""));
            }

            dataSource = model;
            pageNo = page;
            pageCount = totalPage;
            pageParam = param;
            rptConfig = cfg;
            issueCout = issue;

            init();

            MEmployee ad = (dataSource as MEmployee);
            numberTextAmount = ad.EmployeeTax.TaxAmount;

            primaryColumns.Clear();

            primaryColumns.Add(new GridLength(70, GridUnitType.Star));
            primaryColumns.Add(new GridLength(14, GridUnitType.Star));
            primaryColumns.Add(new GridLength(13, GridUnitType.Star));
            primaryColumns.Add(new GridLength(13, GridUnitType.Star));

            DataContext = model;
            InitializeComponent();
        }

        public String IssueTitle
        {
            get
            {
                if (!rptConfig.GetConfigValue("IsCopyReport").Equals("N"))
                {
                    return (rptConfig.GetConfigValue("Issue1") + " " + rptConfig.GetConfigValue("Issue2") + " " + rptConfig.GetConfigValue("Issue3") + " " + rptConfig.GetConfigValue("Issue4"));
                }
                else
                {
                    return (rptConfig.GetConfigValue("Issue" + issueCout.ToString()));
                }
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

                String txt = ((dataSource as MEmployee).EmployeeNameLastname);
                return (txt);
            }
        }

        public String YearTax
        {
            get
            {
                String txt = ""; // (dataSource as MAccountDoc).BEDocumentDateFmt; 
                return (txt);
            }
        }

        public String WitholdingTaxAsText
        {
            get
            {
                if (Lang.Equals("TH"))
                {
                    String txt = ""; // CUtil.CurrencyToThaiText((dataSource as MAccountDoc).WHTaxAmtFmt);
                    return (txt);
                }
                else
                {
                    String txt = ""; //CUtil.changeCurrencyToWords((dataSource as MAccountDoc).WHTaxAmtFmt);
                    return (txt);
                }
            }
        }
        
        public String RvTaxType
        {
            get
            {
                return "";
            }
        }

        public Boolean? RvTaxType3
        {
            get
            {
                return (false);
            }

            set
            {
            }
        }

        public Boolean? RvTaxType53
        {
            get
            {
                return (true);
            }

            set
            {
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
                if (!(dataSource as MEmployee).IDNumber.Equals(""))
                {
                    var chars = (dataSource as MEmployee).IDNumber.ToCharArray();
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
