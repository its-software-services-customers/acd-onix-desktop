using System;
using System.Collections.ObjectModel;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Windows.Media;

namespace Onix.Client.Model
{
    public class MAuxilaryDoc : MBaseModel
    {
        private ObservableCollection<MAuxilaryDocItem> docItem = new ObservableCollection<MAuxilaryDocItem>();
        private ObservableCollection<MEntityAddress> entityAddresses = new ObservableCollection<MEntityAddress>();
        private ObservableCollection<MEntityBankAccount> entityAccounts = new ObservableCollection<MEntityBankAccount>();
        private ObservableCollection<MPaymentCriteria> paymentCriteriaes = new ObservableCollection<MPaymentCriteria>();
        private ObservableCollection<MAuxilaryDocRemark> remarks = new ObservableCollection<MAuxilaryDocRemark>();
        private ObservableCollection<MAuxilaryDocSubItem> attachItems = new ObservableCollection<MAuxilaryDocSubItem>();

        private int internalSeq = 0;

        public MAuxilaryDoc(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("inventory_doc_no", DocumentNo);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_date", DocumentDateFmt);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_desc", DocumentDesc);
            ttItems.Add(ct);

            ct = new CToolTipItem("supplier_name", EntityName);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_status", DocumentStatusDesc);
            ttItems.Add(ct);
        }

        public ObservableCollection<MAuxilaryDocSubItem> AttachItems
        {
            get
            {
                return (attachItems);
            }
        }

        public void InitAttachItems()
        {
            attachItems.Clear();

            int i = 0;
            foreach (MAuxilaryDocItem di in docItem)
            {
                if (di.ExtFlag.Equals("D"))
                {
                    continue;
                }

                i++;

                di.InitItemDetails();
                ObservableCollection<MAuxilaryDocSubItem> itemDetails = di.ItemDetails;

                MAuxilaryDocSubItem subHeader = new MAuxilaryDocSubItem(new CTable(""));
                subHeader.RowType = 0;
                subHeader.Index = i.ToString();
                subHeader.Description = di.SelectItemName;
                attachItems.Add(subHeader);

                foreach (MAuxilaryDocSubItem si in itemDetails)
                {
                    if (si.ExtFlag.Equals("D"))
                    {
                        continue;
                    }

                    si.RowType = 1;
                    attachItems.Add(si);
                }

                MAuxilaryDocSubItem subTotal = new MAuxilaryDocSubItem(new CTable(""));
                subTotal.RowType = 2;
                subTotal.Amount = di.TotalAfterDiscount;
                attachItems.Add(subTotal);
            }

            MAuxilaryDocSubItem grandTotal = new MAuxilaryDocSubItem(new CTable(""));
            grandTotal.RowType = 3;
            grandTotal.Amount = RevenueExpenseAmt;
            attachItems.Add(grandTotal);
        }

        public void CreateDefaultValue()
        {
        }

        private double getSumPricingAmt()
        {
            double totalPricingAmt = 0.00;

            foreach (MAuxilaryDocItem di in docItem)
            {
                if (di.ExtFlag.Equals("D"))
                {
                    continue;
                }

                double pricingAmt = CUtil.StringToDouble(di.TotalAmt);
                totalPricingAmt = totalPricingAmt + pricingAmt;
            }

            return (totalPricingAmt);
        }

        public void CalculateExtraFields()
        {
            String vt = VatType;
            double vatPct = CUtil.StringToDouble(VatPct);

            double totalVatAmt = 0.00;
            double totalWhAmt = 0.00;
            double totalRevenueExpenseAmt = 0.00;
            double totalArApAmt = 0.00;
            double totalItemDiscount = 0.00;

            double primaryTotalRevenueExpenseAmt = 0.00;
            double primaryTotalItemDiscountAmt = 0.00;
            double primaryTotalItemFinalDiscountAmt = 0.00;

            double finalDiscount = CUtil.StringToDouble(FinalDiscount);
            double totalPricingAmt = getSumPricingAmt();
            double finalDiscRatio = 0.00;
            double totalQuantity = 0.00;

            if (finalDiscount > totalPricingAmt)
            {
                finalDiscount = totalPricingAmt;
            }

            if (totalPricingAmt != 0)
            {
                finalDiscRatio = finalDiscount / totalPricingAmt;
            }

            foreach (MAuxilaryDocItem di in docItem)
            {
                if (di.ExtFlag.Equals("D"))
                {
                    continue;
                }

                //This field should be equal di.Amount - di.DiscountAmt
                double pricingAmt = CUtil.StringToDouble(di.TotalAmt);
                double rawAmount = CUtil.StringToDouble(di.Amount);

                double fcDiscount = finalDiscRatio * pricingAmt;
                pricingAmt = pricingAmt - fcDiscount;

                double vatAmt = 0.00;
                double revenueExpenseAmt = 0.00;
                double primaryRevenueExpenseAmt = 0.00;
                double primaryItemDiscountAmt = 0.00;
                double primaryFinalDiscountAmt = 0.00;
                double arApAmt = 0.00;
                double discount = CUtil.StringToDouble(di.Discount);
                double qty = CUtil.StringToDouble(di.Quantity);

                revenueExpenseAmt = pricingAmt;
                primaryRevenueExpenseAmt = rawAmount;
                primaryItemDiscountAmt = discount;
                primaryFinalDiscountAmt = fcDiscount;

                if (vt.Equals("1"))
                {
                    //Novat
                    vatAmt = 0.00;
                    arApAmt = pricingAmt;

                    //di.VatTaxFlag = "N";
                }
                else if (vt.Equals("2"))
                {
                    if (di.IsVatTax)
                    {
                        revenueExpenseAmt = (100 * pricingAmt) / (vatPct + 100);
                        primaryRevenueExpenseAmt = (100 * rawAmount) / (vatPct + 100);
                        primaryItemDiscountAmt = (100 * discount) / (vatPct + 100);
                        primaryFinalDiscountAmt = (100 * fcDiscount) / (vatPct + 100);

                        //Vat Included
                        vatAmt = (vatPct / 100) * revenueExpenseAmt;
                        arApAmt = revenueExpenseAmt + vatAmt;
                    }
                    else
                    {
                        vatAmt = 0.00;
                        arApAmt = pricingAmt;
                    }

                    //di.VatTaxFlag = "Y";
                }
                else
                {
                    if (di.IsVatTax)
                    {
                        //Vat Excluded (3)                 
                        vatAmt = (vatPct / 100) * revenueExpenseAmt;
                        arApAmt = revenueExpenseAmt + vatAmt;
                    }
                    else
                    {
                        vatAmt = 0.00;
                        arApAmt = pricingAmt;
                    }

                    //di.VatTaxFlag = "Y";
                }

                double whTaxAmount = 0.00;
                if (di.WHTaxFlag.Equals("Y"))
                {
                    double whPct = CUtil.StringToDouble(di.WHTaxPct);
                    whTaxAmount = (whPct / 100) * revenueExpenseAmt;
                }

                di.FinalDiscountAvg = fcDiscount.ToString();
                di.VatTaxAmt = vatAmt.ToString();
                di.VatTaxPct = VatPct;
                di.WHTaxAmt = whTaxAmount.ToString();
                di.ArApAmt = arApAmt.ToString();
                di.RevenueExpenseAmt = revenueExpenseAmt.ToString();

                di.PrimaryRevenueExpenseAmt = primaryRevenueExpenseAmt.ToString();
                di.PrimaryFinalDiscountAvgAmt = primaryFinalDiscountAmt.ToString();
                di.PrimaryItemDiscountAmt = primaryItemDiscountAmt.ToString();

                totalVatAmt = totalVatAmt + vatAmt;
                totalRevenueExpenseAmt = totalRevenueExpenseAmt + revenueExpenseAmt;
                totalWhAmt = totalWhAmt + whTaxAmount;
                totalArApAmt = totalArApAmt + arApAmt;
                totalItemDiscount = totalItemDiscount + discount;
                totalQuantity = totalQuantity + qty;

                primaryTotalRevenueExpenseAmt = primaryTotalRevenueExpenseAmt + primaryRevenueExpenseAmt;
                primaryTotalItemDiscountAmt = primaryTotalItemDiscountAmt + primaryItemDiscountAmt;
                primaryTotalItemFinalDiscountAmt = primaryTotalItemFinalDiscountAmt + primaryFinalDiscountAmt;

            } //For Loop

            PricingAmt = totalPricingAmt.ToString();
            ArApAmt = totalArApAmt.ToString();
            WHTaxAmt = totalWhAmt.ToString();
            VatAmt = totalVatAmt.ToString();
            ItemDiscountAmt = totalItemDiscount.ToString();
            RevenueExpenseAmt = totalRevenueExpenseAmt.ToString();
            Quantity = totalQuantity.ToString();

            PrimaryRevenueExpenseAmt = primaryTotalRevenueExpenseAmt.ToString();
            PrimaryItemDiscountAmt = primaryTotalItemDiscountAmt.ToString();
            PrimaryFinalDiscountAvgAmt = primaryTotalItemFinalDiscountAmt.ToString();

            CashReceiptAmt = (totalArApAmt - totalWhAmt).ToString();
            CashActualReceiptAmt = CUtil.RoundUp25(totalArApAmt - totalWhAmt).ToString();
        }

        #region Extra Field

        public String VatAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VAT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("VatAmtFmt");
            }
        }

        public String VatAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(VatAmt));
            }

            set
            {
            }
        }

        public String WHTaxAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_TAX_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("WHTaxAmtFmt");
            }
        }

        public String ItemDiscountAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_DISCOUNT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_DISCOUNT_AMT", value);
                NotifyPropertyChanged("ItemDiscountAMTFmt");
                NotifyPropertyChanged();
            }
        }

        public String ItemDiscountAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemDiscountAmt.ToString()));
            }
        }


        public String Quantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0.00");
                }
                return (GetDbObject().GetFieldValue("QUANTITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("QUANTITY", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("QuantityFmt");
                updateFlag();
            }
        }

        public String QuantityFmt
        {
            get
            {
                return (CUtil.FormatNumber(Quantity));
            }

            set
            {
            }
        }

        public String PrimaryRevenueExpenseAmt
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("PRIMARY_REVENUE_EXPENSE_AMT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("PRIMARY_REVENUE_EXPENSE_AMT", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String PrimaryItemDiscountAmt
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("PRIMARY_ITEM_DISCOUNT_AMT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("PRIMARY_ITEM_DISCOUNT_AMT", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String PrimaryFinalDiscountAvgAmt
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("PRIMARY_FINAL_DISCOUNT_AVG_AMT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("PRIMARY_FINAL_DISCOUNT_AVG_AMT", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ArApAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AR_AP_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AR_AP_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ArApAmtFmt");
            }
        }


        public String ArApAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(ArApAmt));
            }

            set
            {
            }
        }

        public String CashReceiptAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_RECEIPT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_RECEIPT_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("CashReceiptAmtFmt");
            }
        }

        public String CashReceiptAmtReport
        {
            get
            {
                if (!DocumentNo.Equals(""))
                    return (CashReceiptAmtFmt);
                else
                    return ("");
            }

            set
            {
            }
        }

        public String RevenueExpenseAmtReport
        {
            get
            {
                if (!DocumentNo.Equals(""))
                    return (RevenueExpenseAmtFmt);
                else
                    return ("");
            }

            set
            {
            }
        }

        public String CashReceiptAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(CashReceiptAmt));
            }

            set
            {
            }
        }

        public String CashActualReceiptAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_ACTUAL_RECEIPT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_ACTUAL_RECEIPT_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("CashActualReceiptAmtFmt");
            }
        }

        public String CashActualReceiptAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(CashActualReceiptAmt));
            }

            set
            {
            }
        }

        public String RevenueExpenseAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REVENUE_EXPENSE_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("REVENUE_EXPENSE_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("RevenueExpenseAmtFmt");
            }
        }

        public String RevenueExpenseAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(RevenueExpenseAmt));
            }

            set
            {
            }
        }

        public String PricingAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PRICING_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRICING_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("PricingAmtFmt");
            }
        }

        public String PricingAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(PricingAmt));
            }

            set
            {
            }
        }

        #endregion

        #region Doc Item
        public void InitAuxilaryDocItem()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("AUXILARY_DOC_ITEM");

            if (arr == null)
            {
                docItem.Clear();
                return;
            }

            docItem.Clear();
            foreach (CTable t in arr)
            {
                MAuxilaryDocItem v = new MAuxilaryDocItem(t);

                docItem.Add(v);
                v.ExtFlag = "I";
            }
        }

        public void AddAuxilaryDocItem(MAuxilaryDocItem pi)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("AUXILARY_DOC_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("AUXILARY_DOC_ITEM", arr);
            }

            pi.ExtFlag = "A";
            arr.Add(pi.GetDbObject());
            docItem.Add(pi);

            pi.Seq = internalSeq;
            internalSeq++;
        }

        public ObservableCollection<MAuxilaryDocItem> AuxilaryDocItems
        {
            get
            {
                return (docItem);
            }
        }
        #endregion

        #region Payment Criteria

        public void InitPaymentCriteria()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("PAYMENT_CRITERIA_ITEM");

            if (arr == null)
            {
                paymentCriteriaes.Clear();
                return;
            }

            paymentCriteriaes.Clear();
            foreach (CTable t in arr)
            {
                MPaymentCriteria v = new MPaymentCriteria(t);

                paymentCriteriaes.Add(v);
                v.ExtFlag = "I";
            }
        }

        public void AddPaymentCriteriaItem(MPaymentCriteria pi)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PAYMENT_CRITERIA_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PAYMENT_CRITERIA_ITEM", arr);
            }

            pi.ExtFlag = "A";
            arr.Add(pi.GetDbObject());
            paymentCriteriaes.Add(pi);

            pi.Seq = internalSeq;
            internalSeq++;
        }

        public ObservableCollection<MPaymentCriteria> PaymentCriteriaes
        {
            get
            {
                return (paymentCriteriaes);
            }
        }

        public void RemovePaymentCriteria(MPaymentCriteria vp)
        {
            removeAssociateItems(vp, "PAYMENT_CRITERIA_ITEM", "INTERNAL_SEQ", "PAYMENT_CRITERIA_ID");
            paymentCriteriaes.Remove(vp);
        }

        public void CalculatePaymentTotal()
        {
            double expTotal = 0.00;
            double vatTotal = 0.00;
            double whTotal = 0.00;
            double vatIncTotal = 0.00;
            double remainTotal = 0.00;

            foreach (MPaymentCriteria ps in paymentCriteriaes)
            {
                if (ps.ExtFlag.Equals("D"))
                {
                    continue;
                }

                double expamt = CUtil.StringToDouble(ps.PaymentAmount);
                double vatamt = CUtil.StringToDouble(ps.VatAmount);
                double whamt = CUtil.StringToDouble(ps.WhTaxAmount);
                double vatinc = expamt + vatamt;
                double remain = vatinc - whamt;

                expTotal = expTotal + expamt;
                vatTotal = vatTotal + vatamt;
                whTotal = whTotal + whamt;
                vatIncTotal = vatIncTotal + vatinc;
                remainTotal = remainTotal + remain;
            }

            PmtRemainAmt = remainTotal.ToString();
            PmtRevenueExpenseAmt = expTotal.ToString();
            PmtVatAmt = vatTotal.ToString();
            PmtVatIncludeAmt = vatIncTotal.ToString();
            PmtWhTaxAmt = whTotal.ToString();
        }

        public String PmtVatAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PMT_VAT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PMT_VAT_AMT", value);
                NotifyPropertyChanged("PmtVatAmtFmt");
            }
        }

        public String PmtVatAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(PmtVatAmt));
            }

            set
            {
            }
        }

        public String PmtVatIncludeAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PMT_VAT_INCLUDE_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PMT_VAT_INCLUDE_AMT", value);
                NotifyPropertyChanged("PmtVatIncludeAmtFmt");
            }
        }

        public String PmtVatIncludeAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(PmtVatIncludeAmt));
            }

            set
            {
            }
        }

        public String PmtRevenueExpenseAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PMT_REVENUE_EXPENSE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PMT_REVENUE_EXPENSE", value);
                NotifyPropertyChanged("PmtRevenueExpenseAmtFmt");
            }
        }

        public String PmtRevenueExpenseAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(PmtRevenueExpenseAmt));
            }

            set
            {
            }
        }

        public String PmtRemainAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PMT_REMAIN_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PMT_REMAIN_AMT", value);
                NotifyPropertyChanged("PmtRemainAmtFmt");
            }
        }

        public String PmtRemainAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(PmtRemainAmt));
            }

            set
            {
            }
        }

        public String PmtWhTaxAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PMT_WH_TAX_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PMT_WH_TAX_AMT", value);
                NotifyPropertyChanged("PmtWhTaxAmtFmt");
            }
        }

        public String PmtWhTaxAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(PmtWhTaxAmt));
            }

            set
            {
            }
        }

        #endregion

        public String FinalDiscount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FINAL_DISCOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("FINAL_DISCOUNT", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String FinalDiscountFmt
        {
            get
            {
                return (CUtil.FormatNumber(FinalDiscount));
            }

            set
            {
            }
        }

        public String AuxilaryDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AUXILARY_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("AUXILARY_DOC_ID", value);
            }
        }

        public String DocumentType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_TYPE", value);
            }
        }

        public String QuotationType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("QUOTATION_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("QUOTATION_TYPE", value);
            }
        }

        public String DocumentDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE", value);
            }
        }

        public String RefDocNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_DOC_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_DOC_NO", value);
            }
        }

        public String ShiftTo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SHIFT_TO"));
            }

            set
            {
                GetDbObject().SetFieldValue("SHIFT_TO", value);
            }
        }

        #region Document Status

        public MMasterRef DocumentStatusObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                DocumentStatus = m.MasterID;
            }
        }

        public MMasterRef QuotationTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                QuotationType = m.MasterID;
            }
        }

        public String DocumentNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_NO", value);
            }
        }

        public String DocumentStatus
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_STATUS", value);
            }
        }

        public String DocumentStatusDesc
        {
            get
            {
                if (DocumentStatus.Equals(""))
                {
                    return ("");
                }

                PoDocumentStatus dt = (PoDocumentStatus)Int32.Parse(DocumentStatus);
                String str = CUtil.PoStatusToString(dt);

                return (str);
            }

            set
            {

            }
        }
        #endregion

        #region Project

        public MBaseModel ProjectObj
        {
            set
            {
                if (value == null)
                {
                    ProjectID = "";
                    ProjectName = "";
                    ProjectCode = "";

                    return;
                }

                MProject m = value as MProject;
                ProjectID = m.ProjectID;
                ProjectName = m.ProjectName;
                ProjectCode = m.ProjectCode;
                ProjectGroupName = m.ProjectGroupName;

                NotifyPropertyChanged("ProjectGroupName");
            }

            get
            {
                MProject m = new MProject(new CTable(""));
                m.ProjectID = ProjectID;
                m.ProjectName = ProjectName;
                m.ProjectCode = ProjectCode;

                return (m);
            }
        }

        public String ProjectCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_CODE", value);
            }
        }

        public String ProjectID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_ID", value);
            }
        }

        public String ProjectName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_NAME", value);
            }
        }

        public String ProjectGroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_GROUP_NAME", value);
            }
        }
        #endregion


        #region Delivery Date

        public String DeliveryDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DELIVERY_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public DateTime DeliveryDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("DELIVERY_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("DELIVERY_DATE", str);
                NotifyPropertyChanged();
            }
        }

        #endregion;

        #region Document Date

        public DateTime? FromDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_DOCUMENT_DATE", str);
            }
        }

        public DateTime? ToDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_DOCUMENT_DATE", str);
            }
        }

        public String DocumentDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public DateTime DocumentDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("DOCUMENT_DATE", str);
                //For Quotation
                calculateDateUntil();
                NotifyPropertyChanged();
            }
        }
        
        #endregion;

        public String VatPct
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VAT_PERCENTAGE"));
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_PERCENTAGE", value);
                CalculateExtraFields();
                NotifyPropertyChanged();
            }
        }

        private void calculateDateUntil()
        {
            DeliveryDate = DocumentDate.AddDays(CUtil.StringToInt(DayValidity));
        }

        public String DayValidity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DAY_VALIDITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("DAY_VALIDITY", value);

                calculateDateUntil();
                NotifyPropertyChanged();
            }
        }

        #region Vat Type

        public String VatType
        {
            get
            {
                String defaultValue = "1";
                String type = GetDbObject().GetFieldValue("VAT_TYPE");

                if (GetDbObject() == null)
                {
                    return (defaultValue);
                }

                if (type.Equals(""))
                {
                    return (defaultValue);
                }

                return (type);
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_TYPE", value);

                NotifyPropertyChanged("IsVatIncluded");
                NotifyPropertyChanged("IsVatExcluded");
                NotifyPropertyChanged("IsNoVat");
            }
        }

        public Boolean IsVat
        {
            get
            {
                return (!IsNoVat);
            }

            set
            {
            }
        }

        public Boolean IsNoVat
        {
            get
            {
                return (VatType.Equals("1"));
            }

            set
            {
                if (value)
                {
                    VatType = "1";
                    NotifyPropertyChanged("IsVat");
                }
            }
        }

        public Boolean IsVatIncluded
        {
            get
            {
                return (VatType.Equals("2"));
            }

            set
            {
                if (value)
                {
                    VatType = "2";
                    NotifyPropertyChanged("IsVat");
                }
            }
        }

        public Boolean IsVatExcluded
        {
            get
            {
                return (VatType.Equals("3"));
            }

            set
            {
                if (value)
                {
                    VatType = "3";
                    NotifyPropertyChanged("IsVat");
                }
            }
        }

        public String VatTypeDesc
        {
            get
            {
                String defaultValue = "no_vat";
                String type = GetDbObject().GetFieldValue("VAT_TYPE");

                if (GetDbObject() == null)
                {
                    return (CLanguage.getValue("price") + CLanguage.getValue(defaultValue));
                }

                if (type.Equals("1"))
                {
                    defaultValue = "no_vat";
                }
                else if (type.Equals("2"))
                {
                    defaultValue = "include_vat";
                }
                else if (type.Equals("3"))
                {
                    defaultValue = "exclude_vat";
                }
                return (CLanguage.getValue("price") + CLanguage.getValue(defaultValue));
            }
        }

        #endregion

        #region Entity

        public MBaseModel EntityObj
        {
            get
            {
                MEntity cst = new MEntity(new CTable(""));
                cst.EntityID = EntityId;
                cst.EntityName = EntityName;
                cst.EntityCode = EntityCode;
                cst.EntityGroup = EntityGroup;
                cst.EntityType = EntityType;
                cst.Phone = EntityPhone;
                cst.Fax = EntityFax;
                cst.IDCardNumber = EntityIDNumber;
                cst.NamePrefixDesc = EntityNamePrefixDesc;
                cst.PromptPayID = PromptPayID;

                return (cst);
            }

            set
            {
                if (value == null)
                {
                    EntityId = "";
                    EntityName = "";
                    EntityCode = "";
                    EntityGroup = "";
                    EntityType = "";
                    EntityPhone = "";
                    EntityFax = "";
                    EntityIDNumber = "";
                    EntityNamePrefixDesc = "";
                    PromptPayID = "";

                    return;
                }

                MEntity ii = (value as MEntity);

                EntityId = ii.EntityID;
                EntityName = ii.EntityName;
                EntityCode = ii.EntityCode;
                EntityGroup = ii.EntityGroup;
                EntityType = ii.EntityType;
                EntityPhone = ii.Phone;
                EntityFax = ii.Fax;
                EntityIDNumber = ii.IDCardNumber;
                EntityNamePrefixDesc = ii.NamePrefixDesc;
                PromptPayID = ii.PromptPayID;

                updateFlag();
            }
        }

        public String EntityId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ID", value);
            }
        }

        public String EntityGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_GROUP", value);
            }
        }

        public String EntityType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_TYPE", value);
            }
        }

        public String EntityNamePrefixDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NAME_PREFIX_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("NAME_PREFIX_DESC", value);
            }
        }

        public String EntityName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_NAME", value);
            }
        }

        public String EntityPhone
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_PHONE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_PHONE", value);
            }
        }

        public String EntityFax
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FAX"));
            }

            set
            {
                GetDbObject().SetFieldValue("FAX", value);
            }
        }

        public String EntityIDNumber
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ID_NUMBER"));
            }

            set
            {
                GetDbObject().SetFieldValue("ID_NUMBER", value);
            }
        }

        public String PromptPayID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROMPT_PAY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROMPT_PAY_ID", value);
            }
        }

        public String EntityCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_CODE", value);
            }
        }
        #endregion

        #region BranchObj
        public MMasterRef BranchObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                BranchId = m.MasterID;
                BranchCode = m.Code;
                BranchName = m.Description;
                BranchNameEng = m.DescriptionEng;

                updateFlag();
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                //ObservableCollection<MMasterRef> branches = CMasterReference.Instance.Branches;
                //if (branches == null)
                //{
                //    return (null);
                //}

                //MMasterRef mr = CUtil.MasterIDToObject(branches, BranchId);
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = BranchId;
                mr.Code = BranchCode;
                mr.Description = BranchName;
                mr.DescriptionEng = BranchNameEng;

                return (mr);
            }
        }

        public String BranchId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String BranchCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_CODE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }
        public String BranchName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_NAME", value);
                NotifyPropertyChanged();
            }
        }
        public String BranchNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region CreditTermObj
        public MMasterRef CreditTermObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                CreditTermID = m.MasterID;
                CreditTermCode = m.Code;
                CreditTermName = m.Description;

                updateFlag();
                NotifyPropertyChanged();
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = CreditTermID;
                mr.Code = CreditTermCode;
                mr.Description = CreditTermName;

                return (mr);
            }
        }

        public String CreditTermID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CREDIT_TERM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CREDIT_TERM_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String CreditTermCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CREDIT_TERM_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CREDIT_TERM_CODE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }
        public String CreditTermName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CREDIT_TERM_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CREDIT_TERM_NAME", value);
                NotifyPropertyChanged();
            }
        }
        
        #endregion

        #region Entity Address

        public String EntityAddressID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ADDRESS_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ADDRESS_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String EntityAddress
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ADDRESS"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ADDRESS", value);
                NotifyPropertyChanged();
            }
        }

        public String EntityAddressFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ADDRESS_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ADDRESS_FLAG", value);
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<MEntityAddress> EntityAddresses
        {
            get
            {
                return (entityAddresses);
            }

            set
            {
                entityAddresses = value;
                NotifyPropertyChanged();
            }
        }

        public void ReloadEntityAddresses(ObservableCollection<MEntityAddress> addresses)
        {
            entityAddresses.Clear();
            foreach (MEntityAddress me in addresses)
            {
                entityAddresses.Add(me);
            }

            NotifyPropertyChanged("EntityAddresses");
        }

        public void InitEntityAddresses()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("ENTITY_ADDRESS_ITEMS");

            if (arr == null)
            {
                entityAddresses.Clear();
                return;
            }

            entityAddresses.Clear();
            foreach (CTable t in arr)
            {
                MEntityAddress v = new MEntityAddress(t);

                if (!v.ExtFlag.Equals("D"))
                {
                    entityAddresses.Add(v);
                }
            }

            //NotifyPropertyChanged("AddressObj");
        }

        public MBaseModel AddressObj
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MEntityAddress> items = entityAddresses;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(entityAddresses, EntityAddressID));
            }

            set
            {
                if (value == null)
                {
                    EntityAddressID = "";
                    EntityAddress = "";
                    return;
                }

                MEntityAddress ii = (value as MEntityAddress);

                EntityAddressID = ii.EntityAddressID;
                EntityAddress = ii.Address;

                updateFlag();
            }
        }

        #endregion Entity Address

        #region Payment Type

        public MBaseModel PoPaymentTypeObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                PaymentType = m.MasterID;
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.PoPayments;
                if (items == null)
                {
                    return (null);
                }

                MMasterRef mr = CUtil.MasterIDToObject(items, PaymentType);
                return (mr);
            }
        }

        public MBaseModel PaymentTypeObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                PaymentType = m.MasterID;
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.AccountSalePayTypes;
                if (items == null)
                {
                    return (null);
                }

                MMasterRef mr = CUtil.MasterIDToObject(items, PaymentType);
                return (mr);
            }
        }

        public String PaymentType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYMENT_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYMENT_TYPE", value);
            }
        }
        #endregion

        #region Entity Bank Account

        public String EntityBankAccountID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BANK_ACCOUNT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_ACCOUNT_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String EntityBankAccountDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String EntityBankName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BANK_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String EntityBankAccountInfo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (EntityBankAccountDesc + " " + EntityBankName);
            }

            set
            {
            }
        }

        public ObservableCollection<MEntityBankAccount> EntityBankAccounts
        {
            get
            {
                return (entityAccounts);
            }

            set
            {
                entityAccounts = value;
                NotifyPropertyChanged();
            }
        }

        public void ReloadEntityBankAccount(ObservableCollection<MEntityBankAccount> accounts)
        {
            entityAccounts.Clear();
            foreach (MEntityBankAccount me in accounts)
            {
                entityAccounts.Add(me);
            }

            //NotifyPropertyChanged("EntityBankAccountDesc");
        }

        public void InitEntityBankAccounts()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("ENTITY_BANK_ACCOUNT_ITEMS");

            if (arr == null)
            {
                entityAccounts.Clear();
                return;
            }

            entityAccounts.Clear();
            foreach (CTable t in arr)
            {
                MEntityBankAccount v = new MEntityBankAccount(t);

                if (!v.ExtFlag.Equals("D"))
                {
                    entityAccounts.Add(v);
                }
            }

            //NotifyPropertyChanged("AddressObj");
        }

        public MBaseModel EntityBankAccountObj
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MEntityBankAccount> items = entityAccounts;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.IDToObject(items, "EntityBankAccountID", EntityBankAccountID));
            }

            set
            {
                if (value == null)
                {
                    EntityBankAccountID = "";
                    EntityBankAccountDesc = "";
                    return;
                }

                MEntityBankAccount ii = (value as MEntityBankAccount);

                EntityBankAccountID = ii.EntityBankAccountID;
                EntityBankAccountDesc = ii.AccountNo;
                updateFlag();
            }
        }

        #endregion Entity Bank Account

        #region Employee

        public String EmployeeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String EmployeeCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String EmployeeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String EmployeeNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }
        public MBaseModel EmployeeObj
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MEmployee m = new MEmployee(new CTable(""));
                m.EmployeeID = EmployeeID;
                m.EmployeeCode = EmployeeCode;
                m.EmployeeName = EmployeeName;
                m.EmployeeNameEng = EmployeeNameEng;

                return (m);
            }

            set
            {
                if (value == null)
                {
                    EmployeeID = "";
                    EmployeeCode = "";
                    EmployeeName = "";
                    EmployeeNameEng = "";

                    return;
                }

                MEmployee ii = (value as MEmployee);

                EmployeeID = ii.EmployeeID;
                EmployeeCode = ii.EmployeeCode;
                EmployeeName = ii.EmployeeName;
                EmployeeNameEng = ii.EmployeeNameEng;

                updateFlag();
            }
        }

        #endregion Employee

        #region Currency

        public MBaseModel CurrencyObj
        {
            set
            {
                if (value == null)
                {
                    CurrencyID = "";
                    CurrencyName = "";
                    CurrencyCode = "";

                    return;
                }

                MMasterRef m = value as MMasterRef;
                CurrencyID = m.MasterID;
                CurrencyCode = m.Code;
                CurrencyName = m.Description;
                CurrencyNameEng = m.DescriptionEng;
                CurrencySubName = m.Optional;
                CurrencySubNameEng = m.OptionalEng;
            }

            get
            {
                MMasterRef m = new MMasterRef(new CTable(""));
                m.MasterID = CurrencyID;   
                m.Code = CurrencyCode;
                m.Description = CurrencyName;
                m.DescriptionEng = CurrencyNameEng;
                m.Optional = CurrencySubName;
                m.OptionalEng = CurrencySubNameEng;

                return (m);
            }
        }

        public String CurrencyCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CURRENCY_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CURRENCY_CODE", value);
            }
        }

        public String CurrencyID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CURRENCY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CURRENCY_ID", value);
            }
        }

        public String CurrencyName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CURRENCY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CURRENCY_NAME", value);
            }
        }

        public String CurrencyNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CURRENCY_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("CURRENCY_NAME_ENG", value);
            }
        }

        public String CurrencySubName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CURRENCY_SUB_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CURRENCY_SUB_NAME", value);
            }
        }

        public String CurrencySubNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CURRENCY_SUB_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("CURRENCY_SUB_NAME_ENG", value);
            }
        }

        #endregion

        #region Payment Term

        public MBaseModel PaymentTermObj
        {
            set
            {
                if (value == null)
                {
                    PaymentTerm = "";
                    PaymentTermName = "";
                    PaymentTermCode = "";

                    return;
                }

                MMasterRef m = value as MMasterRef;
                PaymentTerm = m.MasterID;  
                PaymentTermCode = m.Code;
                PaymentTermName = m.Description;
                PaymentTermNameEng = m.DescriptionEng;
            }

            get
            {
                MMasterRef m = new MMasterRef(new CTable(""));
                m.MasterID = PaymentTerm;  
                m.Code = PaymentTermCode;
                m.Description = PaymentTermName;
                m.DescriptionEng = PaymentTermNameEng;

                return (m);
            }
        }

        public String PaymentTermCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYMENT_TERM_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYMENT_TERM_CODE", value);
            }
        }

        public String PaymentTerm
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYMENT_TERM"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYMENT_TERM", value);
            }
        }

        public String PaymentTermName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYMENT_TERM_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYMENT_TERM_NAME", value);
            }
        }

        public String PaymentTermNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYMENT_TERM_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYMENT_TERM_NAME_ENG", value);
            }
        }

        #endregion


        #region Remarks

        public void InitRemarks()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("REMARK_ITEM");

            if (arr == null)
            {
                remarks.Clear();
                return;
            }

            remarks.Clear();

            foreach (CTable t in arr)
            {
                MAuxilaryDocRemark v = new MAuxilaryDocRemark(t);

                remarks.Add(v);
                v.ExtFlag = "I";
            }
        }

        public void AddRemarkItem(MAuxilaryDocRemark pi)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("REMARK_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("REMARK_ITEM", arr);
            }

            pi.ExtFlag = "A";
            arr.Add(pi.GetDbObject());
            remarks.Add(pi);

            pi.Seq = internalSeq;
            internalSeq++;
        }

        public ObservableCollection<MAuxilaryDocRemark> Remarks
        {
            get
            {
                return (remarks);
            }
        }

        public void RemoveRemarkItem(MAuxilaryDocRemark vp)
        {
            removeAssociateItems(vp, "REMARK_ITEM", "INTERNAL_SEQ", "AUXILARY_DOC_REMARK_ID");
            remarks.Remove(vp);
        }

        #endregion

        public Boolean IsEditable
        {
            get
            {
                String status = DocumentStatus;
                if (status.Equals(""))
                {
                    return (true);
                }

                if (Int32.Parse(status) == (int)PoDocumentStatus.PoApproved)
                {
                    return (false);
                }

                return (true);
            }
        }

        public override SolidColorBrush RowColor
        {
            get
            {
                return (CUtil.DocStatusToColor(DocumentStatus));
            }
        }

        public String PoInvoiceRefType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PO_INVOICE_REF_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PO_INVOICE_REF_TYPE", value);
            }
        }

        public Boolean IsInUsedBySaleOrder
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                String flag = GetDbObject().GetFieldValue("IN_USED_BY_SO");                
                return (flag.Equals("Y"));
            }

            set
            {
                if (value)
                {
                    GetDbObject().SetFieldValue("IN_USED_BY_SO", "Y");
                }
                else
                {
                    GetDbObject().SetFieldValue("IN_USED_BY_SO", "N");
                }

                NotifyPropertyChanged();
            }
        }

        public Boolean IsPoInvoiceRefByItem
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (PoInvoiceRefType.Equals("1"));
            }

            set
            {
                if (value)
                {
                    PoInvoiceRefType = "1";
                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean IsPoInvoiceRefByCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (PoInvoiceRefType.Equals("2"));
            }

            set
            {
                if (value)
                {
                    PoInvoiceRefType = "2";
                    NotifyPropertyChanged();
                }
            }
        }

        public String RevisionNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REVISE_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REVISE_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String ContactPerson
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CONTACT_PERSON"));
            }

            set
            {
                GetDbObject().SetFieldValue("CONTACT_PERSON", value);
                NotifyPropertyChanged();
            }
        }

        #region Note

        public String NoteWidthCm
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE_WIDTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE_WIDTH", value);
                NotifyPropertyChanged("NoteWidth");
                NotifyPropertyChanged();
            }
        }

        public String NoteHeightCm
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE_HEIGHT"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE_HEIGHT", value);
                NotifyPropertyChanged("NoteHeight");
                NotifyPropertyChanged();
            }
        }


        public String NoteTopCm
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE_TOP"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE_TOP", value);
                NotifyPropertyChanged("NoteTop");
                NotifyPropertyChanged();
            }
        }

        public String NoteLeftCm
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE_LEFT"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE_LEFT", value);
                NotifyPropertyChanged("NoteLeft");
                NotifyPropertyChanged();
            }
        }


        public double NoteHeight
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (0.00);
                }

                return (CUtil.CmToDot(NoteHeightCm));
            }

            set
            {
            }
        }

        public double NoteWidth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (0.00);
                }

                return (CUtil.CmToDot(NoteWidthCm));
            }

            set
            {
            }
        }


        public double NoteTop
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (0.00);
                }

                return (CUtil.CmToDot(NoteTopCm));
            }

            set
            {
            }
        }

        public double NoteLeft
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (0.00);
                }

                return (CUtil.CmToDot(NoteLeftCm));
            }

            set
            {
            }
        }

        public String NoteText
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE_TEXT"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE_TEXT", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsNoteStick
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("NOTE_STICK_FLAG");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("NOTE_STICK_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsLastPage
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_LAST_PAGE");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("IS_LAST_PAGE", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsDisplayNote
        {
            get
            {
                return IsLastPage && IsNoteStick;
            }

            set
            {
            }
        }

        #endregion

        public String CreditTerm
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CREDIT_TERM"));
            }

            set
            {
                GetDbObject().SetFieldValue("CREDIT_TERM", value);
                NotifyPropertyChanged();
            }
        }

        public String IndexItems
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INDEX_ITEMS"));
            }

            set
            {
                GetDbObject().SetFieldValue("INDEX_ITEMS", value);
                NotifyPropertyChanged();
            }
        }
    }
}