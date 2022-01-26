using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Windows;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV3_53
{
    public partial class WinTaxDocRendererProgress : WinBase
    {
        private Thread t = null;
        private String pdfName = "";
        private int currIdx = 0;
        private int currRecord = 0;
        private int chunk = 1;
        private int chunkCount = 0;
        private int recordCount = 0;
        private ArrayList arr = new ArrayList();

        private int currSeq = 0;
        private Hashtable pdfFields = new Hashtable();
        private MVTaxDocument actualView = null;
        private MCompanyProfile company = null;
        private double expenseAmt = 0.00;
        private double whAmt = 0.00;
        private TaxDocumentType taxDocType = TaxDocumentType.TaxDocRev3;
        private ArrayList pages = null;
        private Hashtable suppliers = new Hashtable();

        private int[][] monthMap = null;

        public WinTaxDocRendererProgress(CWinLoadParam param) : base(param)
        {
            pdfName = String.Format("{0}{1}", Path.GetTempPath(), "rev3_53.pdf");
            InitializeComponent();

            actualView = (MVTaxDocument)loadParam.ActualView;
            company = CMasterReference.Instance.CompanyProfile;

            taxDocType = (TaxDocumentType) CUtil.StringToInt(actualView.DocumentType);

            initMonthMap();
        }

        private void initMonthMap()
        {
            monthMap = new int[][]
            {
                new int[] { -1, -1 }, //Dummy

                new int[] { 2, 0 },
                new int[] { 4, 4 },
                new int[] { 8, 8 },
                new int[] { 1, 1 },
                new int[] { 5, 5 },
                new int[] { 9, 9 },
                new int[] { 0, 2 },
                new int[] { 6, 6 },
                new int[] { 10, 11 },
                new int[] { 3, 3 },
                new int[] { 7, 7 },
                new int[] { 11, 10 },
            };
        }

        public String PDFName
        {
            get
            {
                return (pdfName);
            }
        }

        public void UpdateDone(Boolean flag, Boolean isFailed)
        {
            prgProgress.Dispatcher.Invoke(delegate
            {
                prgProgress.Value = prgProgress.Maximum;
            });
        }

        public void UpdateProgress(int c, int m)
        {
            prgProgress.Dispatcher.Invoke(delegate
            {
                prgProgress.Value = c;
                prgProgress.Maximum = m;
            });
        }

        private void registerPdfField(int group, int row, String pdfFieldName, String logicalName)
        {
            CTaxformRV3_53PdfField field = new CTaxformRV3_53PdfField(group, row, pdfFieldName, logicalName);
            pdfFields.Add(field.Key, field);
        }

        private void populateDetailFormValue(AcroFields pdfFormFields, int category, int pageNo, int pageCount, CTaxFormPageRv3_53 page)
        {
            String fldName = getFormFieldName(-1, -1, "CompanyTaxID");            
            pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatTaxIDField(company.TaxID));

            fldName = getFormFieldName(-1, -1, "SheetNo");
            pdfFormFields.SetField(fldName, pageNo.ToString());

            fldName = getFormFieldName(-1, -1, "TotalSheet");
            pdfFormFields.SetField(fldName, pageCount.ToString());

            fldName = getFormFieldName(-1, -1, "TotalExpenseAmount");
            pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatNumberField(actualView.ExpenseAmount));

            fldName = getFormFieldName(-1, -1, "TotalWhAmount");
            pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatNumberField(actualView.WhAmount));
            

            int grpNo = 0;
            foreach (CTaxFormGroupBySupplierRv3_53 grp in page.Groups)
            {
                currSeq++;

                fldName = getFormFieldName(grpNo, -1, "SupplierTaxID");
                if (taxDocType == TaxDocumentType.TaxDocRev3)
                {
                    pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatTaxIDField(grp.TaxID));
                }
                else
                {
                    //53
                    pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatTaxIDField2(grp.TaxID));
                }

                fldName = getFormFieldName(grpNo, -1, "SupplierAddress");
                pdfFormFields.SetField(fldName, grp.Address);

                if (taxDocType == TaxDocumentType.TaxDocRev3)
                {
                    String[] s = CTaxDocumentUtil.GetNameLastname(grp.Name);
                    fldName = getFormFieldName(grpNo, -1, "SupplierName");
                    pdfFormFields.SetField(fldName, s[0]);

                    fldName = getFormFieldName(grpNo, -1, "SupplierLastName");
                    pdfFormFields.SetField(fldName, s[1]);
                }
                else
                {
                    //53
                    fldName = getFormFieldName(grpNo, -1, "SupplierName");
                    pdfFormFields.SetField(fldName, grp.Name);
                }

                fldName = getFormFieldName(grpNo, -1, "SeqNo");
                pdfFormFields.SetField(fldName, currSeq.ToString());

                int j = 0;
                foreach (MVTaxFormPRV3_53 row in grp.WhItems)
                {
                    whAmt = whAmt + CUtil.StringToDouble(row.WhAmount);
                    expenseAmt = expenseAmt + CUtil.StringToDouble(row.ExpenseAmt);

                    fldName = getFormFieldName(grpNo, j, "DocumentDate");
                    pdfFormFields.SetField(fldName, "  " + row.DocumentDateFmt);

                    fldName = getFormFieldName(grpNo, j, "WhPayType");
                    pdfFormFields.SetField(fldName, " " + row.WhPayType);

                    fldName = getFormFieldName(grpNo, j, "WhGroupDesc");
                    pdfFormFields.SetField(fldName, row.WhGroupDesc);

                    fldName = getFormFieldName(grpNo, j, "WhPct");
                    pdfFormFields.SetField(fldName, row.WhPct);

                    fldName = getFormFieldName(grpNo, j, "WhAmount");
                    pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatNumberField(row.WhAmount));

                    fldName = getFormFieldName(grpNo, j, "ExpenseAmt");
                    pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatNumberField(row.ExpenseAmt));

                    j++;
                }

                grpNo++;
            }

            fldName = getFormFieldName(-1, -1, "TotalExpenseAmount");
            pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatNumberField(expenseAmt.ToString()));

            fldName = getFormFieldName(-1, -1, "TotalWhAmount");
            pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatNumberField(whAmt.ToString()));
        }

        private void registerPdfFields_3()
        {
            //========Page Level
            registerPdfField(-1, -1, "Text1.0", "CompanyTaxID");
            registerPdfField(-1, -1, "Text1.2", "SheetNo");
            registerPdfField(-1, -1, "Text1.3", "TotalSheet");
            registerPdfField(-1, -1, "Text6.24", "TotalExpenseAmount");
            registerPdfField(-1, -1, "Text6.25", "TotalWhAmount");
            registerPdfField(-1, -1, "Text1.1", "CompanyBranch");
            registerPdfField(-1, -1, "Text9.1", "OthorizeName");
            registerPdfField(-1, -1, "Text9.2", "OthorizeSignature");
            registerPdfField(-1, -1, "Text9.3", "SignDay");
            registerPdfField(-1, -1, "Text9.4", "SignMonth");
            registerPdfField(-1, -1, "Text9.5", "SignYear");

            //===========================================
            //Group Level "ลำดับที่"
            registerPdfField(0, -1, "Text1.27", "SeqNo");
            registerPdfField(1, -1, "Text2.27", "SeqNo");
            registerPdfField(2, -1, "Text3.27", "SeqNo");
            registerPdfField(3, -1, "Text4.27", "SeqNo");
            registerPdfField(4, -1, "Text5.27", "SeqNo");
            registerPdfField(5, -1, "Text6.27", "SeqNo");

            //Group Level "เลขที่ผู้เสียภาษี supplier"
            registerPdfField(0, -1, "Text1.4", "SupplierTaxID");
            registerPdfField(1, -1, "Text2.1", "SupplierTaxID");
            registerPdfField(2, -1, "Text3.1", "SupplierTaxID");
            registerPdfField(3, -1, "Text4.1", "SupplierTaxID");
            registerPdfField(4, -1, "Text5.1", "SupplierTaxID");
            registerPdfField(5, -1, "Text6.1", "SupplierTaxID");

            //Group Level "ชื่อ supplier"
            registerPdfField(0, -1, "Text1.6", "SupplierName");
            registerPdfField(1, -1, "Text2.3", "SupplierName");
            registerPdfField(2, -1, "Text3.3", "SupplierName");
            registerPdfField(3, -1, "Text4.3", "SupplierName");
            registerPdfField(4, -1, "Text5.3", "SupplierName");
            registerPdfField(5, -1, "Text6.3", "SupplierName");

            //Group Level "นามสกลุล supplier"
            registerPdfField(0, -1, "Text1.7", "SupplierLastName");
            registerPdfField(1, -1, "Text2.4", "SupplierLastName");
            registerPdfField(2, -1, "Text3.4", "SupplierLastName");
            registerPdfField(3, -1, "Text4.4", "SupplierLastName");
            registerPdfField(4, -1, "Text5.4", "SupplierLastName");
            registerPdfField(5, -1, "Text6.4", "SupplierLastName");

            //Group Level "ที่อยู่ supplier"
            registerPdfField(0, -1, "Text1.8", "SupplierAddress");
            registerPdfField(1, -1, "Text2.5", "SupplierAddress");
            registerPdfField(2, -1, "Text3.5", "SupplierAddress");
            registerPdfField(3, -1, "Text4.5", "SupplierAddress");
            registerPdfField(4, -1, "Text5.5", "SupplierAddress");
            registerPdfField(5, -1, "Text6.5", "SupplierAddress");



            //======================== Row Level "Document Date"
            registerPdfField(0, 0, "Text1.9", "DocumentDate");
            registerPdfField(0, 1, "Text1.15", "DocumentDate");
            registerPdfField(0, 2, "Text1.21", "DocumentDate");

            registerPdfField(1, 0, "Text2.6", "DocumentDate");
            registerPdfField(1, 1, "Text2.12", "DocumentDate");
            registerPdfField(1, 2, "Text2.18", "DocumentDate");

            registerPdfField(2, 0, "Text3.6", "DocumentDate");
            registerPdfField(2, 1, "Text3.12", "DocumentDate");
            registerPdfField(2, 2, "Text3.18", "DocumentDate");

            registerPdfField(3, 0, "Text4.6", "DocumentDate");
            registerPdfField(3, 1, "Text4.12", "DocumentDate");
            registerPdfField(3, 2, "Text4.18", "DocumentDate");

            registerPdfField(4, 0, "Text5.6", "DocumentDate");
            registerPdfField(4, 1, "Text5.12", "DocumentDate");
            registerPdfField(4, 2, "Text5.18", "DocumentDate");

            registerPdfField(5, 0, "Text6.6", "DocumentDate");
            registerPdfField(5, 1, "Text6.12", "DocumentDate");
            registerPdfField(5, 2, "Text6.18", "DocumentDate");


            //Row Level "WhGroupDesc"
            registerPdfField(0, 0, "Text1.10", "WhGroupDesc");
            registerPdfField(0, 1, "Text1.16", "WhGroupDesc");
            registerPdfField(0, 2, "Text1.22", "WhGroupDesc");

            registerPdfField(1, 0, "Text2.7", "WhGroupDesc");
            registerPdfField(1, 1, "Text2.13", "WhGroupDesc");
            registerPdfField(1, 2, "Text2.19", "WhGroupDesc");

            registerPdfField(2, 0, "Text3.7", "WhGroupDesc");
            registerPdfField(2, 1, "Text3.13", "WhGroupDesc");
            registerPdfField(2, 2, "Text3.19", "WhGroupDesc");

            registerPdfField(3, 0, "Text4.7", "WhGroupDesc");
            registerPdfField(3, 1, "Text4.13", "WhGroupDesc");
            registerPdfField(3, 2, "Text4.19", "WhGroupDesc");

            registerPdfField(4, 0, "Text5.7", "WhGroupDesc");
            registerPdfField(4, 1, "Text5.13", "WhGroupDesc");
            registerPdfField(4, 2, "Text5.19", "WhGroupDesc");

            registerPdfField(5, 0, "Text6.7", "WhGroupDesc");
            registerPdfField(5, 1, "Text6.13", "WhGroupDesc");
            registerPdfField(5, 2, "Text6.19", "WhGroupDesc");


            //Row Level "WhPct"
            registerPdfField(0, 0, "Text1.11", "WhPct");
            registerPdfField(0, 1, "Text1.17", "WhPct");
            registerPdfField(0, 2, "Text1.23", "WhPct");

            registerPdfField(1, 0, "Text2.8", "WhPct");
            registerPdfField(1, 1, "Text2.14", "WhPct");
            registerPdfField(1, 2, "Text2.20", "WhPct");

            registerPdfField(2, 0, "Text3.8", "WhPct");
            registerPdfField(2, 1, "Text3.14", "WhPct");
            registerPdfField(2, 2, "Text3.20", "WhPct");

            registerPdfField(3, 0, "Text4.8", "WhPct");
            registerPdfField(3, 1, "Text4.14", "WhPct");
            registerPdfField(3, 2, "Text4.20", "WhPct");

            registerPdfField(4, 0, "Text5.8", "WhPct");
            registerPdfField(4, 1, "Text5.14", "WhPct");
            registerPdfField(4, 2, "Text5.20", "WhPct");

            registerPdfField(5, 0, "Text6.8", "WhPct");
            registerPdfField(5, 1, "Text6.14", "WhPct");
            registerPdfField(5, 2, "Text6.20", "WhPct");



            //Row Level "ExpenseAmt"
            registerPdfField(0, 0, "Text1.12", "ExpenseAmt");
            registerPdfField(0, 1, "Text1.18", "ExpenseAmt");
            registerPdfField(0, 2, "Text1.24", "ExpenseAmt");

            registerPdfField(1, 0, "Text2.9", "ExpenseAmt");
            registerPdfField(1, 1, "Text2.15", "ExpenseAmt");
            registerPdfField(1, 2, "Text2.21", "ExpenseAmt");

            registerPdfField(2, 0, "Text3.9", "ExpenseAmt");
            registerPdfField(2, 1, "Text3.15", "ExpenseAmt");
            registerPdfField(2, 2, "Text3.21", "ExpenseAmt");

            registerPdfField(3, 0, "Text4.9", "ExpenseAmt");
            registerPdfField(3, 1, "Text4.15", "ExpenseAmt");
            registerPdfField(3, 2, "Text4.21", "ExpenseAmt");

            registerPdfField(4, 0, "Text5.9", "ExpenseAmt");
            registerPdfField(4, 1, "Text5.15", "ExpenseAmt");
            registerPdfField(4, 2, "Text5.21", "ExpenseAmt");

            registerPdfField(5, 0, "Text6.9", "ExpenseAmt");
            registerPdfField(5, 1, "Text6.15", "ExpenseAmt");
            registerPdfField(5, 2, "Text6.21", "ExpenseAmt");


            //Row Level "WhAmount"
            registerPdfField(0, 0, "Text1.13", "WhAmount");
            registerPdfField(0, 1, "Text1.19", "WhAmount");
            registerPdfField(0, 2, "Text1.25", "WhAmount");

            registerPdfField(1, 0, "Text2.10", "WhAmount");
            registerPdfField(1, 1, "Text2.16", "WhAmount");
            registerPdfField(1, 2, "Text2.22", "WhAmount");

            registerPdfField(2, 0, "Text3.10", "WhAmount");
            registerPdfField(2, 1, "Text3.16", "WhAmount");
            registerPdfField(2, 2, "Text3.22", "WhAmount");

            registerPdfField(3, 0, "Text4.10", "WhAmount");
            registerPdfField(3, 1, "Text4.16", "WhAmount");
            registerPdfField(3, 2, "Text4.22", "WhAmount");

            registerPdfField(4, 0, "Text5.10", "WhAmount");
            registerPdfField(4, 1, "Text5.16", "WhAmount");
            registerPdfField(4, 2, "Text5.22", "WhAmount");

            registerPdfField(5, 0, "Text6.10", "WhAmount");
            registerPdfField(5, 1, "Text6.16", "WhAmount");
            registerPdfField(5, 2, "Text6.22", "WhAmount");

            //Row Level "WhPayType"

            registerPdfField(0, 0, "Text1.14", "WhPayType");
            registerPdfField(0, 1, "Text1.20", "WhPayType");
            registerPdfField(0, 2, "Text1.26", "WhPayType");

            registerPdfField(1, 0, "Text2.11", "WhPayType");
            registerPdfField(1, 1, "Text2.17", "WhPayType");
            registerPdfField(1, 2, "Text2.23", "WhPayType");

            registerPdfField(2, 0, "Text3.11", "WhPayType");
            registerPdfField(2, 1, "Text3.17", "WhPayType");
            registerPdfField(2, 2, "Text3.23", "WhPayType");

            registerPdfField(3, 0, "Text4.11", "WhPayType");
            registerPdfField(3, 1, "Text4.17", "WhPayType");
            registerPdfField(3, 2, "Text4.23", "WhPayType");

            registerPdfField(4, 0, "Text5.11", "WhPayType");
            registerPdfField(4, 1, "Text5.17", "WhPayType");
            registerPdfField(4, 2, "Text5.23", "WhPayType");

            registerPdfField(5, 0, "Text6.11", "WhPayType");
            registerPdfField(5, 1, "Text6.17", "WhPayType");
            registerPdfField(5, 2, "Text6.23", "WhPayType");
        }


        private void registerPdfFields_53()
        {
            //========Page Level
            registerPdfField(-1, -1, "Text1.0", "CompanyTaxID");
            registerPdfField(-1, -1, "Text1.2", "SheetNo");
            registerPdfField(-1, -1, "Text1.3", "TotalSheet");
            registerPdfField(-1, -1, "Text6.28", "TotalExpenseAmount");
            registerPdfField(-1, -1, "Text6.29", "TotalWhAmount");
            registerPdfField(-1, -1, "Text1.1", "CompanyBranch");
            registerPdfField(-1, -1, "Text9.1", "OthorizeName");
            registerPdfField(-1, -1, "Text9.2", "OthorizeSignature");
            registerPdfField(-1, -1, "Text9.3", "SignDay");
            registerPdfField(-1, -1, "Text9.4", "SignMonth");
            registerPdfField(-1, -1, "Text9.5", "SignYear");

            //===========================================
            //Group Level "ลำดับที่"
            registerPdfField(0, -1, "Text1.4", "SeqNo");
            registerPdfField(1, -1, "Text2.4", "SeqNo");
            registerPdfField(2, -1, "Text3.4", "SeqNo");
            registerPdfField(3, -1, "Text4.4", "SeqNo");
            registerPdfField(4, -1, "Text5.4", "SeqNo");
            registerPdfField(5, -1, "Text6.4", "SeqNo");

            //Group Level "เลขที่ผู้เสียภาษี supplier"
            registerPdfField(0, -1, "Text1.5", "SupplierTaxID");
            registerPdfField(1, -1, "Text2.5", "SupplierTaxID");
            registerPdfField(2, -1, "Text3.5", "SupplierTaxID");
            registerPdfField(3, -1, "Text4.5", "SupplierTaxID");
            registerPdfField(4, -1, "Text5.5", "SupplierTaxID");
            registerPdfField(5, -1, "Text6.5", "SupplierTaxID");

            //Group Level "ชื่อ supplier"
            registerPdfField(0, -1, "Text1.6", "SupplierName");
            registerPdfField(1, -1, "Text2.6", "SupplierName");
            registerPdfField(2, -1, "Text3.6", "SupplierName");
            registerPdfField(3, -1, "Text4.6", "SupplierName");
            registerPdfField(4, -1, "Text5.6", "SupplierName");
            registerPdfField(5, -1, "Text6.6", "SupplierName");

            //Group Level "ที่อยู่ supplier line 1"
            registerPdfField(0, -1, "Text1.7", "SupplierAddress");
            registerPdfField(1, -1, "Text2.7", "SupplierAddress");
            registerPdfField(2, -1, "Text3.7", "SupplierAddress");
            registerPdfField(3, -1, "Text4.7", "SupplierAddress");
            registerPdfField(4, -1, "Text5.7", "SupplierAddress");
            registerPdfField(5, -1, "Text6.7", "SupplierAddress");

            //Group Level "ที่อยู่ supplier line 2"
            registerPdfField(0, -1, "Text1.8", "SupplierAddressLine2");
            registerPdfField(1, -1, "Text2.8", "SupplierAddressLine2");
            registerPdfField(2, -1, "Text3.8", "SupplierAddressLine2");
            registerPdfField(3, -1, "Text4.8", "SupplierAddressLine2");
            registerPdfField(4, -1, "Text5.8", "SupplierAddressLine2");
            registerPdfField(5, -1, "Text6.8", "SupplierAddressLine2");


            //======================== Row Level "Document Date"
            registerPdfField(0, 0, "Text1.10", "DocumentDate");
            registerPdfField(0, 1, "Text1.16", "DocumentDate");
            registerPdfField(0, 2, "Text1.22", "DocumentDate");

            registerPdfField(1, 0, "Text2.10", "DocumentDate");
            registerPdfField(1, 1, "Text2.16", "DocumentDate");
            registerPdfField(1, 2, "Text2.22", "DocumentDate");

            registerPdfField(2, 0, "Text3.10", "DocumentDate");
            registerPdfField(2, 1, "Text3.16", "DocumentDate");
            registerPdfField(2, 2, "Text3.22", "DocumentDate");

            registerPdfField(3, 0, "Text4.10", "DocumentDate");
            registerPdfField(3, 1, "Text4.16", "DocumentDate");
            registerPdfField(3, 2, "Text4.22", "DocumentDate");

            registerPdfField(4, 0, "Text5.10", "DocumentDate");
            registerPdfField(4, 1, "Text5.16", "DocumentDate");
            registerPdfField(4, 2, "Text5.22", "DocumentDate");

            registerPdfField(5, 0, "Text6.10", "DocumentDate");
            registerPdfField(5, 1, "Text6.16", "DocumentDate");
            registerPdfField(5, 2, "Text6.22", "DocumentDate");


            //Row Level "WhGroupDesc"
            registerPdfField(0, 0, "Text1.11", "WhGroupDesc");
            registerPdfField(0, 1, "Text1.17", "WhGroupDesc");
            registerPdfField(0, 2, "Text1.23", "WhGroupDesc");

            registerPdfField(1, 0, "Text2.11", "WhGroupDesc");
            registerPdfField(1, 1, "Text2.17", "WhGroupDesc");
            registerPdfField(1, 2, "Text2.23", "WhGroupDesc");

            registerPdfField(2, 0, "Text3.11", "WhGroupDesc");
            registerPdfField(2, 1, "Text3.17", "WhGroupDesc");
            registerPdfField(2, 2, "Text3.23", "WhGroupDesc");

            registerPdfField(3, 0, "Text4.11", "WhGroupDesc");
            registerPdfField(3, 1, "Text4.17", "WhGroupDesc");
            registerPdfField(3, 2, "Text4.23", "WhGroupDesc");

            registerPdfField(4, 0, "Text5.11", "WhGroupDesc");
            registerPdfField(4, 1, "Text5.17", "WhGroupDesc");
            registerPdfField(4, 2, "Text5.23", "WhGroupDesc");

            registerPdfField(5, 0, "Text6.11", "WhGroupDesc");
            registerPdfField(5, 1, "Text6.17", "WhGroupDesc");
            registerPdfField(5, 2, "Text6.23", "WhGroupDesc");


            //Row Level "WhPct"
            registerPdfField(0, 0, "Text1.12", "WhPct");
            registerPdfField(0, 1, "Text1.18", "WhPct");
            registerPdfField(0, 2, "Text1.24", "WhPct");

            registerPdfField(1, 0, "Text2.12", "WhPct");
            registerPdfField(1, 1, "Text2.18", "WhPct");
            registerPdfField(1, 2, "Text2.24", "WhPct");

            registerPdfField(2, 0, "Text3.12", "WhPct");
            registerPdfField(2, 1, "Text3.18", "WhPct");
            registerPdfField(2, 2, "Text3.24", "WhPct");

            registerPdfField(3, 0, "Text4.12", "WhPct");
            registerPdfField(3, 1, "Text4.18", "WhPct");
            registerPdfField(3, 2, "Text4.24", "WhPct");

            registerPdfField(4, 0, "Text5.12", "WhPct");
            registerPdfField(4, 1, "Text5.18", "WhPct");
            registerPdfField(4, 2, "Text5.24", "WhPct");

            registerPdfField(5, 0, "Text6.12", "WhPct");
            registerPdfField(5, 1, "Text6.18", "WhPct");
            registerPdfField(5, 2, "Text6.24", "WhPct");



            //Row Level "ExpenseAmt"
            registerPdfField(0, 0, "Text1.13", "ExpenseAmt");
            registerPdfField(0, 1, "Text1.19", "ExpenseAmt");
            registerPdfField(0, 2, "Text1.25", "ExpenseAmt");

            registerPdfField(1, 0, "Text2.13", "ExpenseAmt");
            registerPdfField(1, 1, "Text2.19", "ExpenseAmt");
            registerPdfField(1, 2, "Text2.25", "ExpenseAmt");

            registerPdfField(2, 0, "Text3.13", "ExpenseAmt");
            registerPdfField(2, 1, "Text3.19", "ExpenseAmt");
            registerPdfField(2, 2, "Text3.25", "ExpenseAmt");

            registerPdfField(3, 0, "Text4.13", "ExpenseAmt");
            registerPdfField(3, 1, "Text4.19", "ExpenseAmt");
            registerPdfField(3, 2, "Text4.25", "ExpenseAmt");

            registerPdfField(4, 0, "Text5.13", "ExpenseAmt");
            registerPdfField(4, 1, "Text5.19", "ExpenseAmt");
            registerPdfField(4, 2, "Text5.25", "ExpenseAmt");

            registerPdfField(5, 0, "Text6.13", "ExpenseAmt");
            registerPdfField(5, 1, "Text6.19", "ExpenseAmt");
            registerPdfField(5, 2, "Text6.25", "ExpenseAmt");


            //Row Level "WhAmount"
            registerPdfField(0, 0, "Text1.14", "WhAmount");
            registerPdfField(0, 1, "Text1.20", "WhAmount");
            registerPdfField(0, 2, "Text1.26", "WhAmount");

            registerPdfField(1, 0, "Text2.14", "WhAmount");
            registerPdfField(1, 1, "Text2.20", "WhAmount");
            registerPdfField(1, 2, "Text2.26", "WhAmount");

            registerPdfField(2, 0, "Text3.14", "WhAmount");
            registerPdfField(2, 1, "Text3.20", "WhAmount");
            registerPdfField(2, 2, "Text3.26", "WhAmount");

            registerPdfField(3, 0, "Text4.14", "WhAmount");
            registerPdfField(3, 1, "Text4.20", "WhAmount");
            registerPdfField(3, 2, "Text4.26", "WhAmount");

            registerPdfField(4, 0, "Text5.14", "WhAmount");
            registerPdfField(4, 1, "Text5.20", "WhAmount");
            registerPdfField(4, 2, "Text5.26", "WhAmount");

            registerPdfField(5, 0, "Text6.14", "WhAmount");
            registerPdfField(5, 1, "Text6.20", "WhAmount");
            registerPdfField(5, 2, "Text6.26", "WhAmount");

            //Row Level "WhPayType"

            registerPdfField(0, 0, "Text1.15", "WhPayType");
            registerPdfField(0, 1, "Text1.21", "WhPayType");
            registerPdfField(0, 2, "Text1.27", "WhPayType");

            registerPdfField(1, 0, "Text2.15", "WhPayType");
            registerPdfField(1, 1, "Text2.21", "WhPayType");
            registerPdfField(1, 2, "Text2.27", "WhPayType");

            registerPdfField(2, 0, "Text3.15", "WhPayType");
            registerPdfField(2, 1, "Text3.21", "WhPayType");
            registerPdfField(2, 2, "Text3.27", "WhPayType");

            registerPdfField(3, 0, "Text4.15", "WhPayType");
            registerPdfField(3, 1, "Text4.21", "WhPayType");
            registerPdfField(3, 2, "Text4.27", "WhPayType");

            registerPdfField(4, 0, "Text5.15", "WhPayType");
            registerPdfField(4, 1, "Text5.21", "WhPayType");
            registerPdfField(4, 2, "Text5.27", "WhPayType");

            registerPdfField(5, 0, "Text6.15", "WhPayType");
            registerPdfField(5, 1, "Text6.21", "WhPayType");
            registerPdfField(5, 2, "Text6.27", "WhPayType");
        }

        private String getFormFieldName(int group, int row, String logicalName)
        {
            String key = CTaxformRV3_53PdfField.ConstructKey(group, row, logicalName);
            CTaxformRV3_53PdfField field = (CTaxformRV3_53PdfField) pdfFields[key];

            if (field == null)
            {
                return ("");
            }

            return (field.PdfFieldName);
        }

        private MVTaxFormPRV3_53 fetchData()
        {
            if (currIdx >= arr.Count)
            {
                CTable t = new CTable("");

                t.SetFieldValue("TAX_DOC_ID", actualView.TaxDocID);
                t.SetFieldValue("EXT_CHUNK_NO", chunk.ToString());
                arr = OnixWebServiceAPI.GetListAPI("GetTaxDocRv3Rv53List", "TAX_DOC_REV_LIST", t);

                CTable lastObj = OnixWebServiceAPI.GetLastObjectReturned();
                chunkCount = CUtil.StringToInt(lastObj.GetFieldValue("EXT_CHUNK_COUNT"));
                recordCount = CUtil.StringToInt(lastObj.GetFieldValue("EXT_RECORD_COUNT"));

                currIdx = 0;
                chunk++;
            }

            if (arr.Count > 0)
            {
                CTable o = (CTable) arr[currIdx];
                currIdx++;
                currRecord++;

                MVTaxFormPRV3_53 m = new MVTaxFormPRV3_53(o);
                return (m);
            }

            return (null);
        }

        private ArrayList constructItems()
        {
            CTaxFormPageRv3_53 page = null;
            CTaxFormGroupBySupplierRv3_53 group = null;
            ArrayList pages = new ArrayList();

            while (true)
            {
                MVTaxFormPRV3_53 m = fetchData();
                if (m == null)
                {
                    break;
                }

                if ((page == null) || (page.GroupCount >= 6))
                {
                    page = new CTaxFormPageRv3_53();
                    pages.Add(page);
                }

                if ((group == null) || !group.Name.Equals(m.SupplierName) || (group.ItemCount >= 3))
                {
                    group = new CTaxFormGroupBySupplierRv3_53(m);
                    page.AddGroup(group);
                }

                group.AddWhItem(m);
                suppliers[m.SupplierName] = m.SupplierName;
            }

            if (pages.Count > 0)
            {
                int lastIdx = pages.Count - 1;

                CTaxFormPageRv3_53 lastPage = (CTaxFormPageRv3_53) pages[lastIdx];
                if (lastPage.GroupCount == 0)
                {
                    //Blank page
                    pages.RemoveAt(lastIdx);
                }
            }

            return (pages);
        }

        private void RenderPDF(ArrayList pages)
        {
            String coverName = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.Resources.WHT53_041060.pdf";
            String detailName = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.Resources.290360_attach53.pdf";

            if (taxDocType == TaxDocumentType.TaxDocRev3)
            {
                coverName = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.Resources.270360_WHT3.pdf";
                detailName = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.Resources.270360_attach3.pdf";
            }

            if (File.Exists(pdfName))
            {
                File.Delete(pdfName);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();            
            FileStream outputStream = new FileStream(pdfName, FileMode.Create);

            Document pdfDoc = new Document(PageSize.A4);
            PdfCopy pdf = new PdfCopy(pdfDoc, outputStream);
            pdfDoc.Open();

            Stream coverPageStream = assembly.GetManifestResourceStream(coverName);
            MemoryStream m = new MemoryStream();
            PdfStamper pdfStamper = new PdfStamper(new PdfReader(coverPageStream), m);
            AcroFields pdfFormFields = pdfStamper.AcroFields;
            populateCoverFormValue(pdfFormFields, 1);
            pdfStamper.FormFlattening = true;
            pdfStamper.Close();
            pdf.AddDocument(new PdfReader(new MemoryStream(m.ToArray())));

            int max = pages.Count;
            int i = 0;
            foreach (CTaxFormPageRv3_53 page in pages)
            {
                i++;

                Stream detailPageStream = assembly.GetManifestResourceStream(detailName);

                MemoryStream n = new MemoryStream();
                PdfStamper pdfDetailStamper = new PdfStamper(new PdfReader(detailPageStream), n);
                AcroFields pdfDetailFormFields = pdfDetailStamper.AcroFields;

                populateDetailFormValue(pdfDetailFormFields, 2, i, pages.Count, page);

                pdfDetailStamper.FormFlattening = true;
                pdfDetailStamper.Close();

                pdf.AddDocument(new PdfReader(new MemoryStream(n.ToArray())));

                UpdateProgress(i, max);
            }

            pdfDoc.Close();
            pdf.Close();
        }

        private void populateCoverFormValue(AcroFields pdfFormFields, int category)
        {
            pdfFormFields.SetField("Text1.0", CTaxDocumentUtil.FormatTaxIDField(company.TaxID));
            pdfFormFields.SetField("Text1.2", company.RegistrationName);
            pdfFormFields.SetField("Text1.3", company.BuildingName);
            pdfFormFields.SetField("Text1.4", company.RoomNo);
            pdfFormFields.SetField("Text1.5", company.FloorNo);
            pdfFormFields.SetField("Text1.6", company.VillageName);
            pdfFormFields.SetField("Text1.7", company.HomeNo);
            pdfFormFields.SetField("Text1.8", company.Moo);
            pdfFormFields.SetField("Text1.9", company.Soi);
            pdfFormFields.SetField("Text1.11", company.Road);
            pdfFormFields.SetField("Text1.12", company.District);
            pdfFormFields.SetField("Text1.13", company.Town);
            pdfFormFields.SetField("Text1.14", company.Province);
            pdfFormFields.SetField("Text1.15", company.Zip);
            pdfFormFields.SetField("Text1.16", company.Telephone);

            int idx = 0;

            if (taxDocType == TaxDocumentType.TaxDocRev3)
            {
                pdfFormFields.SetField("Radio Button0", "0", true);
                //pdfFormFields.SetField("Radio Button2", "0", true);
                pdfFormFields.SetField("Radio Button3", "0", true);

                pdfFormFields.SetField("Text1.18", actualView.TaxYearBD);
                idx = 1;
            }
            else
            {
                //53
                //pdfFormFields.SetField("Radio Button0", "0", true);
                pdfFormFields.SetField("Radio Button2", "0", true);
                pdfFormFields.SetField("Radio Button3", "0", true);

                pdfFormFields.SetField("Text1.17", actualView.TaxYearBD);
                idx = 0;
            }

            int month = CUtil.StringToInt(actualView.TaxMonth);
            int monthIdx = monthMap[month][idx];
            pdfFormFields.SetField("Radio Button10", monthIdx.ToString(), true);            


            pdfFormFields.SetField("Text1.19", suppliers.Count.ToString());
            pdfFormFields.SetField("Text1.20", pages.Count.ToString());

            pdfFormFields.SetField("Text2.1", CTaxDocumentUtil.FormatNumberField(actualView.ExpenseAmount));
            pdfFormFields.SetField("Text2.2", CTaxDocumentUtil.FormatNumberField(actualView.WhAmount));
            pdfFormFields.SetField("Text2.4", CTaxDocumentUtil.FormatNumberField(actualView.WhAmount));
        }

        private void MonitorProgress()
        {
            if (taxDocType == TaxDocumentType.TaxDocRev3)
            {
                registerPdfFields_3();
            }
            else
            {
                //53
                registerPdfFields_53();
            }

            pages = constructItems();
            RenderPDF(pages);

            this.Dispatcher.Invoke(delegate
            {
                this.Close();
            });
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            t = new Thread(MonitorProgress);

            t.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            t.Abort();
        }

        private void WinBase_ContentRendered(object sender, EventArgs e)
        {

        }

        private void WinBase_Closed(object sender, EventArgs e)
        {

        }
    }
}
