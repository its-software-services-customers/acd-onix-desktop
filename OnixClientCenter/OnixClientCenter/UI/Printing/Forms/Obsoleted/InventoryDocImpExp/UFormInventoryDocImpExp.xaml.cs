using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Helper;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Media;

namespace Onix.ClientCenter.Forms.InventoryDocImpExp
{
    public class CInvDocItem
    {
        private MInventoryTransaction mi = null;
        private String Lang = "";
        private String itemNo = "";
        private MReportConfig rptConfig = null;

        public CInvDocItem(MInventoryTransaction di, String lang, MReportConfig cfg)
        {
            rptConfig = cfg;
            mi = di;
            Lang = lang;
        }

        public String ItemNo
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (itemNo);
            }

            set
            {
                itemNo = value;
            }
        }

        public String ItemName
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }
                
                if (Lang.Equals("TH"))
                {
                    return (mi.ItemNameThai);
                }
                else
                {
                    return (mi.ItemNameEng);
                }
            }
        }

        public String ItemCode
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.ItemCode);
            }
        }

        public String QuantityFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.ItemQuantityFmt);
            }
        }

        public String UnitName
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                if (Lang.Equals("TH"))
                {
                    return (mi.ItemUnitName);
                }

                return (mi.ItemUnitNameEng);
            }
        }

        public String UnitPriceFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.UIItemUnitPrice);
            }
        }

        public String TotalFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.UIItemAmountFmt);
            }
        }
    }

    public partial class UFormInventoryDocImpExp : UserControl
    {
        private MBaseModel dataSource = null;
        private int pageNo = 1;
       
        private int pageCount = 1;
        private MReportConfig rptConfig = null;

        private int footerHeight = 275; //270 is ok
        private int RowRemark;

        public UFormInventoryDocImpExp(MBaseModel model, int page, int totalPage,int rowRemark, MReportConfig cfg)
        {
            dataSource = model;
            pageNo = page;
            pageCount = totalPage;

            RowRemark = rowRemark;

            rptConfig = cfg;

            DataContext = model;

            InitializeComponent();
        }

        public double TextSize
        {
            get
            {
                return (CUtil.StringToDouble(rptConfig.GetConfigValue("FontSize")));
            }
        }

        public String FontName
        {
            get
            {
                return (rptConfig.GetConfigValue("FontName"));
            }
        }

        public GridLength Column1Width
        {
            get
            {
                GridLength l = new GridLength(7, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column2Width
        {
            get
            {
                GridLength l = new GridLength(43, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column3Width
        {
            get
            {
                GridLength l = new GridLength(7, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column4Width
        {
            get
            {
                GridLength l = new GridLength(10, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column5Width
        {
            get
            {
                GridLength l = new GridLength(10, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column6Width
        {
            get
            {
                GridLength l = new GridLength(10, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column7Width
        {
            get
            {
                GridLength l = new GridLength(13, GridUnitType.Star);
                return (l);
            }
        }

        public int FooterHeight
        {
            get
            {
                return (footerHeight + (RowRemark * 35));
            }

            set
            {
                footerHeight = value;
            }
        }

        public int PageNo
        {
            get
            {
                return (pageNo);
            }

            set
            {
                pageNo = value;
            }
        }

        private ObservableCollection<CInvDocItem> filterItems()
        {
            ObservableCollection<CInvDocItem> temp = new ObservableCollection<CInvDocItem>();

            int itemPerPage = ItemPerPage;
            int start = (pageNo - 1) * itemPerPage + 1;
            int end = start + itemPerPage - 1;

            ObservableCollection<MInventoryTransaction> arr = (dataSource as MInventoryDoc).TxItems;

            int i = 0;
            foreach (MInventoryTransaction m in arr)
            {
                if (m.ExtFlag.Equals("D"))
                {
                    continue;
                }

                i++;

                if ((i >= start) && (i <= end))
                {
                    CInvDocItem d = new CInvDocItem(m, Lang, rptConfig);
                    d.ItemNo = i.ToString();

                    temp.Add(d);
                }
            }

            int left = itemPerPage - temp.Count;

            for (i = 1; i <= left; i++)
            {
                temp.Add(new CInvDocItem(null, Lang, rptConfig));
            }

            return (temp);
        }

        public ObservableCollection<CInvDocItem> ItemChunks
        {
            get
            {
                return (filterItems());
            }
        }

        public MCompanyProfile CompanyProfile
        {
            get
            {
                return (CMasterReference.Instance.CompanyProfile);
            }
        }

        public String LocationName
        {
            get
            {
                if (Lang.Equals("TH"))
                {
                    String txt = (dataSource as MAuxilaryDoc).BranchName;
                    return (txt);
                }
                else
                {
                    String txt = (dataSource as MAuxilaryDoc).BranchNameEng;
                    return (txt);
                }
            }
        }

        public String EmployeeName
        {
            get
            {
                String txt = "";
                if (Lang.Equals("TH"))
                {
                    txt = (dataSource as MAuxilaryDoc).EmployeeName;
                }
                else
                {
                    txt = (dataSource as MAuxilaryDoc).EmployeeNameEng;

                }
                return (txt);
            }
        }

        public String CompanyAddress
        {
            get
            {
                if (Lang.Equals("EN"))
                {
                    return (CMasterReference.Instance.CompanyProfile.AddressEng);
                }
                else
                {
                    return (CMasterReference.Instance.CompanyProfile.Address);
                }
            }
        }

        public String CompanyName
        {
            get
            {
                if (Lang.Equals("EN"))
                {
                    return (CMasterReference.Instance.CompanyProfile.CompanyNameEng);
                }
                else
                {
                    return (CMasterReference.Instance.CompanyProfile.CompanyNameThai);
                }
            }
        }

        public String Lang
        {
            get
            {
                return (rptConfig.GetConfigValue("Language"));
            }
        }

        public double LineWidth
        {
            get
            {
                return (CUtil.StringToDouble(rptConfig.GetConfigValue("LineWidth")));
            }
        }

        public double AddressBoxWidth
        {
            get
            {
                return (CUtil.StringToDouble(rptConfig.GetConfigValue("AddressBoxWidth")));
            }
        }

        public double HeaderHeightDot
        {
            get
            {
                return (40);
            }
        }

        public int ItemPerPage
        {
            get
            {
                return (CUtil.StringToInt(rptConfig.GetConfigValue("ItemPerPage")));
            }
        }

        public double AreaHeight
        {
            get
            {
                return (stkBody.ActualHeight);
            }
        }

        #region Label

        public String LbDocumentType
        {
            get
            {
                if (Lang.Equals("EN"))
                {
                    return (rptConfig.GetConfigValue("DocumentTypeEng"));
                }
                else
                {
                    return (rptConfig.GetConfigValue("DocumentTypeThai"));
                }
            }
        }

        public String LbHeader
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "quotation");
                return (txt);
            }
        }

        public String LbFromLocation
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "from_location");
                return (txt);
            }
        }

        public String LbCompanyName
        {
            get
            {
                MCompanyProfile mp = CMasterReference.Instance.CompanyProfile;

                String txt = "";

                if (Lang.Equals("TH"))
                {
                    txt = mp.NamePrefixDesc;
                }
                else
                {
                    txt = mp.NamePrefixDescEng;
                }

                return (txt);
            }
        }

        public String LbAddress
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "address");
                return (txt);
            }
        }

        public String LbTel
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "tel");
                return (txt);
            }
        }

        public String LbFax
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "fax");
                return (txt);
            }
        }

        public String LbEmail
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "email");
                return (txt);
            }
        }

        public String LbTaxID
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "tax_id");
                return (txt);
            }
        }

        public String LbCustomerName
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "customer");
                return (txt);
            }
        }

        public String LbCustomerAddress
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "address");
                return (txt);
            }
        }

        public String LbDocumentNo
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "docno");
                return (txt);
            }
        }

        public String LbDate
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "date");
                return (txt);
            }
        }

        public String LbDeliveryDate
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "validate_until");
                return (txt);
            }
        }

        public String LbRefDoc
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "RefDoc");
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

        public String LbNote
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "note");
                return (txt);
            }
        }

        public String LbApprovedBy
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "rpt_approved");
                return (txt);
            }
        }

        public String LbAuthoSign
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "authorizerSign");
                return (txt);
            }
        }


        public String LbPayMethod
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "payMethod");
                return (txt);
            }
        }

        public String LbDays
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "day");
                return (txt);
            }
        }

        public String LbSaleMan
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "salesman");
                return (txt);
            }
        }

        public String LbSalesManager
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "exporter");
                return (txt);
            }
        }

        public String LbDueDate
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "due_date");
                return (txt);
            }
        }

        public String LbValad
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "day_validity");
                return (txt);
            }
        }

        public String LbCondition
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "payment_term");
                return (txt);
            }
        }

        public String LbQuoteDesc
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "quote_desc");
                return (txt);
            }
        }

        public String LbCompanyFooter
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "company_footer");
                return (txt);
            }
        }

        public String LbItemNo
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "item_no");
                return (txt);
            }
        }

        public String LbItemDesc
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "description");
                return (txt);
            }
        }

        public String LbQuantity
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "quantity");
                return (txt);
            }
        }

        public String LbUnitPrice
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "unit_price");
                return (txt);
            }
        }
        
        public String LbUnitName
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "Unit");
                return (txt);
            }
        }

        public String LbDiscount
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "discount");
                return (txt);
            }
        }

        public String LbAmount
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "money_quantity");
                return (txt);
            }
        }

        public String LbTotal
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "total");
                return (txt);
            }
        }

        public String LbVat
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "vat_rpt");
                return (txt);
            }
        }

        public String LbNetTotal
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "net");
                return (txt);
            }
        }

        public String LbPage
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "page_no") + " " + pageNo + " / " + pageCount;
                return (txt);
            }
        }

        public String LbProject
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "project");
                return (txt);
            }
        }

        public String LbGProject
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "project_group");
                return (txt);
            }
        }

        public String LbCurrency
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "currency");
                return (txt);
            }
        }
        #endregion

        public Visibility BranchVisibility
        {
            get
            {
                Boolean flag = rptConfig.GetConfigValue("DisplayBranchFlag").Equals("Y");
                if (flag)
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility NamePrefixVisibility
        {
            get
            {
                Boolean flag = rptConfig.GetConfigValue("DisplayNamePrefixFlag").Equals("Y");
                if (flag)
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility LogoVisibility
        {
            get
            {
                Boolean flag = rptConfig.GetConfigValue("DisplayLogoFlag").Equals("Y");
                if (flag)
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public String ShadowBackground
        {
            get
            {
                Boolean flag = rptConfig.GetConfigValue("DisplayShadowFlag").Equals("Y");
                if (flag)
                {
                    return ("LightGray");
                }

                return ("White");
            }
        }

        public BitmapImage Logo
        {
            get
            {
                BitmapImage b = CUtil.GetBitmapFromUrl(CGlobalVariable.GetGlobalVariableValue("COMPANY_LOGO_URL"));
                return (b);
            }
        }
    }
}
