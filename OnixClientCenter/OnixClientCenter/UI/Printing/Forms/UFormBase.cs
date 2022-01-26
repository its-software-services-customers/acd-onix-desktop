using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.Client.Report;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Forms
{
    public class CPaymentCriteria
    {
        private MPaymentCriteria mi = null;
        private String itemNo = "";

        public CPaymentCriteria(MPaymentCriteria di)
        {
            mi = di;
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

        public String Description
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.Description);
            }
        }

        public String PaymentAmountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.PaymentAmountFmt);
            }
        }


        public String VatAmountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(mi.VatAmount));
            }
        }

        public String WhTaxAmountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(mi.WhTaxAmount));
            }
        }

        public String MonneyPlusVatAmountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber((CUtil.StringToDouble(mi.PaymentAmountFmt) + CUtil.StringToDouble(mi.VatAmount)).ToString()));
            }
        }

        public String BalanceAmountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber((CUtil.StringToDouble(mi.PaymentAmountFmt) + CUtil.StringToDouble(mi.WhTaxAmount)).ToString()));
            }
        }

        public String RemainAmountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.RemainAmountFmt);
            }
        }
    }

    public class CInventoryDocItem
    {
        private MInventoryTransaction mi = null;
        private String Lang = "";
        private String itemNo = "";
        private MReportConfig rptConfig = null;

        public CInventoryDocItem(MInventoryTransaction di, String lang, MReportConfig cfg)
        {
            rptConfig = cfg;
            mi = di;
            Lang = lang;
        }

        public String UnitName
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.ItemUnitName);
            }

            set
            {
            }
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

        public String SelectItemNameComposit
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (SelectItemCode + " " + SelectItemName);
            }
        }

        public String SelectItemName
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

        public String SelectItemCode
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

        public String UnitPriceFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.UnitPriceAvgFmt);
            }
        }
    }

    public class CAuxDocItem
    {
        private MAuxilaryDocItem mi = null;
        private String Lang = "";
        private String itemNo = "";
        private MReportConfig rptConfig = null;

        public CAuxDocItem(MAuxilaryDocItem di, String lang, MReportConfig cfg)
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

        public String SelectItemNameComposit
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                if (ItemCodeVisibility == Visibility.Collapsed)
                {
                    return (SelectItemName);
                }

                return (SelectItemCode + " " + SelectItemName);
            }
        }

        public String SelectItemName
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                if (Lang.Equals("TH"))
                {
                    return (mi.SelectItemName);
                }
                else
                {
                    return (mi.SelectItemNameEng);
                }
            }
        }

        public String SelectItemCode
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.SelectItemCode);
            }
        }

        public Visibility ItemCodeVisibility
        {
            get
            {
                if ((mi == null) || mi.SelectType.Equals("3"))
                {
                    return (Visibility.Collapsed);
                }

                Boolean flag = rptConfig.GetConfigValue("DisplayItemCodeFlag").Equals("Y");
                if (flag)
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
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

                return (mi.QuantityFmt);
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

                return (mi.UnitPriceFmt);
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

                return (mi.UnitName);
            }
        }

        public String DiscountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                if (CUtil.StringToDouble(mi.Discount) == 0.00)
                {
                    return ("");
                }

                return (mi.DiscountFmt);
            }
        }

        public String TotalAfterDiscountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.TotalAfterDiscountFmt);
            }
        }
    }

    public class CDocPayment
    {
        private MAccountDocPayment mi = null;
        private String Lang = "";
        private String itemNo = "";

        public CDocPayment(MAccountDocPayment di, String lang)
        {
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

        public String PaymentTypeDesc
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                String paymentTypeDesc = CUtil.PaymentTypeToString(mi.PaymentTypeObj.MasterID, Lang);

                return (paymentTypeDesc);
            }
        }

        public String BankDesc
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                if (Lang.Equals("TH"))
                {
                    return (mi.BankObj.Description + " " + mi.AccountNo);
                }
                else
                {
                    return (mi.BankObj.DescriptionEng + " " + mi.AccountNo);
                }
            }
        }

        public String ChequeNo
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.ChequeNo);
            }
        }

        public String ChequeDateFmt
        {
            get
            {
                if (mi == null || !mi.PaymentTypeObj.MasterID.Equals("4"))
                {
                    return ("");
                }

                return (mi.ChequeDateFmt);
            }
        }


        public String PaidAmountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                double amt = CUtil.StringToDouble(mi.PaidAmount) + CUtil.StringToDouble(mi.FeeAmount);
                return (CUtil.FormatNumber(amt.ToString()));
            }
        }
    }

    public class CAccountDocItem
    {
        private MAccountDocItem mi = null;
        private String Lang = "";
        private String itemNo = "";
        private MReportConfig rptConfig = null;

        public CAccountDocItem(MAccountDocItem di, String lang, MReportConfig cfg)
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

        public String SelectItemName
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                if (Lang.Equals("TH"))
                {
                    return (mi.SelectItemName);
                }
                else
                {
                    return (mi.SelectItemNameEng);
                }
            }
        }

        public Visibility ItemCodeVisibility
        {
            get
            {
                if ((mi == null) || mi.SelectType.Equals("3"))
                {
                    return (Visibility.Collapsed);
                }

                Boolean flag = rptConfig.GetConfigValue("DisplayItemCodeFlag").Equals("Y");
                if (flag)
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public String SelectItemCode
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.SelectItemCode);
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

                return (mi.QuantityFmt);
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

                return (mi.UnitName);
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

                return (mi.UnitPriceFmt);
            }
        }

        public String DiscountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                if (CUtil.StringToDouble(mi.Discount) == 0.00)
                {
                    return ("");
                }

                return (mi.DiscountFmt);
            }
        }

        public String TotalAfterDiscountFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.TotalAfterDiscountFmt);
            }
        }
    }

    public class CReceiptItem
    {
        private MAccountDocReceipt mi = null;
        private String Lang = "";
        private String itemNo = "";
        private MReportConfig rptConfig = null;

        public CReceiptItem(MAccountDocReceipt di, String lang, MReportConfig cfg)
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

        public String DocumentNo
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.DocumentNo);
            }
        }

        public String DocumentDate
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.DocumentDateFmt);
            }
        }

        public String DueDate
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.DueDateFmt);
            }
        }

        public String Description
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.DocumentDesc);
            }
        }

        public String ArApAmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.ArApAmtFmt);
            }
        }
    }

    public class CDocReceipt
    {
        private MAccountDocReceipt mi = null;
        private String Lang = "";
        private String itemNo = "";
        public String docType = "";

        public CDocReceipt(MAccountDocReceipt di, String lang)
        {
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

        public String SelectItemDocNo
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }


                return (mi.DocumentNo);
            }
        }

        public String RefPONo
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }


                return (mi.RefPoNo);
            }
        }

        public String ProjectCode
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                String dt1 = ((int)AccountDocumentType.AcctDocCashPurchase).ToString();
                String dt2 = ((int)AccountDocumentType.AcctDocMiscExpense).ToString();

                if (docType.Equals(dt1) || docType.Equals(dt2))
                {
                    return (mi.ProjectCode);
                }

                return (mi.IndexProject);

            }
        }

        public String RevenueExpenseAmtFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.RevenueExpenseAmtFmt);
            }
        }

        public String ArApAmtFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.ArApAmtFmt);
            }
        }

        public String PricingAmtFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.PricingAmtFmt);
            }
        }

        public String VatAmtFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.VatAmtFmt);
            }
        }

        public String WHTaxAmtFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.WHTaxAmtFmt);
            }
        }

        public String CashReceiptAmtFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.CashReceiptAmtFmt);
            }
        }

        public String FinalDiscountAmtFmt
        {
            get
            {
                if (mi == null)
                {
                    return ("");
                }

                return (mi.FinalDiscountFmt);
            }
        }
    }

    public class UFormBase : UserControl
    {
        protected MReportConfig rptConfig = null;
        protected MBaseModel dataSource = null;
        protected int pageNo = 1;
        protected int pageCount = 1;
        protected CReportPageParam pageParam = null;

        protected String branchNameEng = "";
        protected String branchNameThai = "";

        protected String numberTextAmount = "";
        protected String amountFmt = "";
        protected ArrayList primaryColumns = new ArrayList();

        protected FrameworkElement headerPanel = null;
        protected FrameworkElement tablePanel = null;
        protected int descriptionColumnIndex = 1;

        private double getDescColumnWidth(double areaWidth)
        {
            double sum = 0;
            foreach (GridLength grdl in primaryColumns)
            {
                sum = sum + grdl.Value;
            }

            GridLength gl = (GridLength)primaryColumns[descriptionColumnIndex];
            double colWidth = gl.Value * areaWidth / sum;

            return (colWidth);
        }

        public virtual void rootElement_LayoutUpdated(object sender, EventArgs e)
        {
            String oldPrevItemWidth = rptConfig.GetConfigValue("PrevItemWidth");
            String oldPrevAreaHeight = rptConfig.GetConfigValue("PrevAreaHeight");

            String hstr1 = CUtil.FormatNumber(oldPrevAreaHeight);
            String wstr1 = CUtil.FormatNumber(oldPrevItemWidth);


            double areaHeight = tablePanel.ActualHeight - headerPanel.ActualHeight;
            double areaWidth = tablePanel.ActualWidth;

            GridLength gl = (GridLength)primaryColumns[descriptionColumnIndex];
            double colWidth = getDescColumnWidth(areaWidth);

            String hstr2 = CUtil.FormatNumber(areaHeight.ToString());
            String wstr2 = CUtil.FormatNumber(colWidth.ToString());

            if (!hstr1.Equals(hstr2) || !wstr1.Equals(wstr2))
            {
                rptConfig.SetConfigValue("PrevItemWidth", colWidth.ToString(), "double", "");
                rptConfig.SetConfigValue("PrevAreaHeight", areaHeight.ToString(), "double", "");
                CReportConfigs.SaveReportConfig(null, rptConfig);

                CHelper.ShowErorMessage("", "ERROR_PREVIEW_AGAIN", null);
            }
        }

        protected void init()
        {
            if (dataSource is MAccountDoc)
            {
                MAccountDoc ad = (dataSource as MAccountDoc);

                branchNameThai = ad.BranchName;
                branchNameEng = ad.BranchNameEng;
                numberTextAmount = ad.CashReceiptAmtFmt;
            }
        }

        #region Label

        public String LbAuthoSign
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "authorizerSign");
                return (txt);
            }
        }


        public String LbSalesManager
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "rpt_salesManager");
                return (txt);
            }
        }


        public String LbDocumentNumber
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "document_no");
                return (txt);
            }
        }

        public String LbProdValue
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "value_product");
                return (txt);
            }
        }

        public String LbVatAmount
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "vat_amount");
                return (txt);
            }
        }

        public String LbWhTaxValue
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "wh_eligible");
                return (txt);
            }
        }

        public String LbBalanceAmount
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "balance_amount");
                return (txt);
            }
        }

        public String LbReceiptNo
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "receipt_no");
                return (txt);
            }
        }

        public String LbRefWHDocNo
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "ref_wh_doc_no");
                return (txt);
            }
        }

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

        public String LbPaidBy
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "paid_by");
                return (txt);
            }
        }

        public String LbRefPO
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "po_no");
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

        public String LbClaimDate
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "claim_date");
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

        public String LbPurchaseBillNo
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "purchase_bill_no");
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
                String txt = CLanguage.getValue(Lang, "approved_by");
                return (txt);
            }
        }

        public String LbPayee
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "payee");
                return (txt);
            }
        }

        public String LbEntityCode
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "code");
                return (txt);
            }
        }
        
        public String LbPayMethod
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "payment_type");
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

        public String LbDeliveryDate
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "delivery_date");
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

        public String LbUnit
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "Unit");
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

        public String LbTypePayment
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "type_cash");
                return (txt);
            }
        }

        public String LbChequeNo
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "cheque_no");
                return (txt);
            }
        }

        public String LbChequeDate
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "cheque_date");
                return (txt);
            }
        }

        public String LbSentBy
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "sent_by");
                return (txt);
            }
        }

        public String LbReceivedBy
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "received_by");
                return (txt);
            }
        }

        public String LbWHTaxDesc
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "wh_tax_desc");
                return (txt);
            }
        }

        public String LbBank
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "Bank");
                return (txt);
            }
        }

        public String LbBnkBranch
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "Branch");
                return (txt);
            }
        }

        public String LbDocumentTypeTh
        {
            get
            {
                return (rptConfig.GetConfigValue("DocumentTypeThai"));
            }
        }

        public String LbDocumentTypeEn
        {
            get
            {
                return (rptConfig.GetConfigValue("DocumentTypeEng"));
            }
        }

        public String LbCreditTerm
        {
            get
            {
                String txt = CLanguage.getValue(Lang, "credit_term");
                return (txt);
            }
        }

        #endregion

        #region ColumnWidth

        public GridLength Column1Width
        {
            get
            {
                GridLength l = (GridLength) primaryColumns[0];
                return (l);
            }
        }

        public GridLength Column2Width
        {
            get
            {
                GridLength l = (GridLength)primaryColumns[1];
                return (l);
            }
        }

        public GridLength Column3Width
        {
            get
            {
                GridLength l = (GridLength)primaryColumns[2];
                return (l);
            }
        }

        public GridLength Column4Width
        {
            get
            {
                GridLength l = (GridLength)primaryColumns[3];
                return (l);
            }
        }

        public GridLength Column5Width
        {
            get
            {
                GridLength l = (GridLength)primaryColumns[4];
                return (l);
            }
        }

        public GridLength Column6Width
        {
            get
            {
                GridLength l = (GridLength)primaryColumns[5];
                return (l);
            }
        }

        public GridLength Column7Width
        {
            get
            {
                GridLength l = (GridLength)primaryColumns[6];
                return (l);
            }
        }

        public GridLength Column8Width
        {
            get
            {
                GridLength l = (GridLength)primaryColumns[7];
                return (l);
            }
        }

        public GridLength Column9Width
        {
            get
            {
                GridLength l = (GridLength)primaryColumns[8];
                return (l);
            }
        }
        #endregion

        public Visibility VisibleLastPage
        {
            get
            {
                if (pageNo != pageCount)
                {
                    //Only last page will show the number
                    return (Visibility.Hidden);
                }

                return (Visibility.Visible);
            }
        }

        public String ArApAmtFmt
        {
            get
            {
                if (pageNo != pageCount)
                {
                    //Only last page will show the number
                    return ("");
                }

                return (amountFmt);
            }
        }

        public String NumberAsText
        {
            get
            {
                if (Lang.Equals("TH"))
                {
                    String txt = CUtil.CurrencyToThaiText(numberTextAmount);
                    return (txt);
                }
                else
                {
                    String txt = CUtil.changeCurrencyToWords(numberTextAmount);
                    return (txt);
                }
            }
        }

        public String BranchDesc
        {
            get
            {
                if (Lang.Equals("TH"))
                {
                    
                    return (branchNameThai);
                }
                else
                {
                    String txt = (dataSource as MAccountDoc).BranchNameEng;
                    return (branchNameEng);
                }
            }
        }

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
                MCompanyProfile cp = CMasterReference.Instance.CompanyProfile;
                
                BitmapImage b = CUtil.GetBitmapFromUrl(cp.LogoImageUrl);
                return (b);
            }
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

        public GridLength ItemRowHeight
        {
            get
            {
                Boolean wrapFlag = rptConfig.GetConfigValue("WrapFlag").Equals("Y");

                if (wrapFlag)
                {
                    return (GridLength.Auto);
                }

                double rowHeight = CUtil.StringToDouble(rptConfig.GetConfigValue("ItemRowHeight"));
                return (new GridLength(rowHeight));
            }
        }

        public TextWrapping TextWrapMode
        {
            get
            {
                Boolean wrapFlag = rptConfig.GetConfigValue("WrapFlag").Equals("Y");

                if (wrapFlag)
                {
                    return (TextWrapping.Wrap);
                }

                return (TextWrapping.NoWrap);
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

        public MCompanyProfile CompanyProfile
        {
            get
            {
                return (CMasterReference.Instance.CompanyProfile);
            }
        }

        public String Lang
        {
            get
            {
                return (rptConfig.GetConfigValue("Language"));
            }
        }

        public double CustomerBoxWidth
        {
            get
            {
                return (CUtil.StringToDouble(rptConfig.GetConfigValue("CustomerBoxWidth")));
            }
        }

        public double CustomerBoxHeight
        {
            get
            {
                return (CUtil.StringToDouble(rptConfig.GetConfigValue("CustomerBoxHeight")));
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

    }
}
