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

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV1
{
    public partial class WinTaxDocRv1RendererProgress : WinBase
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

        public WinTaxDocRv1RendererProgress(CWinLoadParam param) : base(param)
        {
            pdfName = String.Format("{0}{1}", Path.GetTempPath(), "rev1.pdf");
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
            CTaxformRv1PdfField field = new CTaxformRv1PdfField(group, row, pdfFieldName, logicalName);
            pdfFields.Add(field.Key, field);
        }

        private void populateDetailFormValue(AcroFields pdfFormFields, int category, int pageNo, int pageCount, CTaxFormPageRv1 page)
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

            fldName = getFormFieldName(-1, -1, "Condition");
            pdfFormFields.SetField(fldName, "0");

            int grpNo = 0;
            foreach (CTaxFormGroupByEmployeeRv1 grp in page.Groups)
            {
                currSeq++;

                fldName = getFormFieldName(grpNo, 0, "SupplierTaxID");
                pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatTaxIDField(grp.TaxID));

                String[] s = CTaxDocumentUtil.GetNameLastname(grp.Name);
                fldName = getFormFieldName(grpNo, 0, "SupplierName");
                pdfFormFields.SetField(fldName, s[0]);

                fldName = getFormFieldName(grpNo, 0, "SupplierLastName");
                pdfFormFields.SetField(fldName, s[1]);
                
                fldName = getFormFieldName(grpNo, 0, "SeqNo");
                pdfFormFields.SetField(fldName, currSeq.ToString());

                int j = 0;
                foreach (MVTaxFormPRV1 row in grp.WhItems)
                {
                    whAmt = whAmt + CUtil.StringToDouble(row.WhAmount);
                    expenseAmt = expenseAmt + CUtil.StringToDouble(row.ExpenseAmt);

                    fldName = getFormFieldName(grpNo, j, "PayrollDate");
                    pdfFormFields.SetField(fldName, "  " + row.DocumentDateFmt);

                    fldName = getFormFieldName(grpNo, j, "WhAmount");
                    pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatNumberField(row.WhAmount));

                    fldName = getFormFieldName(grpNo, j, "ExpenseAmt");
                    pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatNumberField(row.ExpenseAmt));

                    fldName = getFormFieldName(grpNo, j, "Condition");
                    pdfFormFields.SetField(fldName, "  1");

                    j++;
                }

                grpNo++;
            }

            fldName = getFormFieldName(-1, -1, "TotalExpenseAmount");
            pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatNumberField(expenseAmt.ToString()));

            fldName = getFormFieldName(-1, -1, "TotalWhAmount");
            pdfFormFields.SetField(fldName, CTaxDocumentUtil.FormatNumberField(whAmt.ToString()));
        }
        
        private void registerPdfFields_1()
        {
            //========Page Level
            registerPdfField(-1, -1, "Text1.0", "CompanyTaxID");
            registerPdfField(-1, -1, "Text1.2", "SheetNo");
            registerPdfField(-1, -1, "Text1.3", "TotalSheet");

            registerPdfField(-1, -1, "Radio Button0", "Condition");

            //registerPdfField(-1, -1, "Text6.28", "TotalExpenseAmount");
            //registerPdfField(-1, -1, "Text6.29", "TotalWhAmount");            

            //===========================================
            //Group Level "ลำดับที่"
            registerPdfField(0, 0, "Text1.4", "SeqNo");
            registerPdfField(1, 0, "Text2.1", "SeqNo");
            registerPdfField(2, 0, "Text3.1", "SeqNo");
            registerPdfField(3, 0, "Text4.1", "SeqNo");
            registerPdfField(4, 0, "Text5.1", "SeqNo");
            registerPdfField(5, 0, "Text6.1", "SeqNo");
            registerPdfField(6, 0, "Text7.1", "SeqNo");
            registerPdfField(7, 0, "Text8.1", "SeqNo");

            //Group Level "เลขที่ผู้เสียภาษี supplier"
            registerPdfField(0, 0, "Text1.5", "SupplierTaxID");
            registerPdfField(1, 0, "Text2.2", "SupplierTaxID");
            registerPdfField(2, 0, "Text3.2", "SupplierTaxID");
            registerPdfField(3, 0, "Text4.2", "SupplierTaxID");
            registerPdfField(4, 0, "Text5.2", "SupplierTaxID");
            registerPdfField(5, 0, "Text6.2", "SupplierTaxID");
            registerPdfField(6, 0, "Text7.2", "SupplierTaxID");
            registerPdfField(7, 0, "Text8.2", "SupplierTaxID");

            //Group Level "ชื่อ supplier"
            registerPdfField(0, 0, "Text1.6", "SupplierName");
            registerPdfField(1, 0, "Text2.3", "SupplierName");
            registerPdfField(2, 0, "Text3.3", "SupplierName");
            registerPdfField(3, 0, "Text4.3", "SupplierName");
            registerPdfField(4, 0, "Text5.3", "SupplierName");
            registerPdfField(5, 0, "Text6.3", "SupplierName");
            registerPdfField(6, 0, "Text7.3", "SupplierName");
            registerPdfField(7, 0, "Text8.3", "SupplierName");

            //Group Level "นามสกุล supplier"
            registerPdfField(0, 0, "Text1.7", "SupplierLastName");
            registerPdfField(1, 0, "Text2.4", "SupplierLastName");
            registerPdfField(2, 0, "Text3.4", "SupplierLastName");
            registerPdfField(3, 0, "Text4.4", "SupplierLastName");
            registerPdfField(4, 0, "Text5.4", "SupplierLastName");
            registerPdfField(5, 0, "Text6.4", "SupplierLastName");
            registerPdfField(6, 0, "Text7.4", "SupplierLastName");
            registerPdfField(7, 0, "Text8.4", "SupplierLastName");

            //Group Level "Pay date"
            registerPdfField(0, 0, "Text1.8", "PayrollDate");
            registerPdfField(1, 0, "Text2.5", "PayrollDate");
            registerPdfField(2, 0, "Text3.5", "PayrollDate");
            registerPdfField(3, 0, "Text4.5", "PayrollDate");
            registerPdfField(4, 0, "Text5.5", "PayrollDate");
            registerPdfField(5, 0, "Text6.5", "PayrollDate");
            registerPdfField(6, 0, "Text7.5", "PayrollDate");
            registerPdfField(7, 0, "Text8.5", "PayrollDate");

            //Group Level "Expense Amount"
            registerPdfField(0, 0, "Text1.9", "ExpenseAmt");
            registerPdfField(1, 0, "Text2.6", "ExpenseAmt");
            registerPdfField(2, 0, "Text3.6", "ExpenseAmt");
            registerPdfField(3, 0, "Text4.6", "ExpenseAmt");
            registerPdfField(4, 0, "Text5.6", "ExpenseAmt");
            registerPdfField(5, 0, "Text6.6", "ExpenseAmt");
            registerPdfField(6, 0, "Text7.6", "ExpenseAmt");
            registerPdfField(7, 0, "Text8.6", "ExpenseAmt");

            //Group Level "Wh Amount"
            registerPdfField(0, 0, "Text1.10", "WhAmount");
            registerPdfField(1, 0, "Text2.7", "WhAmount");
            registerPdfField(2, 0, "Text3.7", "WhAmount");
            registerPdfField(3, 0, "Text4.7", "WhAmount");
            registerPdfField(4, 0, "Text5.7", "WhAmount");
            registerPdfField(5, 0, "Text6.7", "WhAmount");
            registerPdfField(6, 0, "Text7.7", "WhAmount");
            registerPdfField(7, 0, "Text8.7", "WhAmount");

            //Condition
            registerPdfField(0, 0, "Text1.11", "Condition");
            registerPdfField(1, 0, "Text2.8", "Condition");
            registerPdfField(2, 0, "Text3.8", "Condition");
            registerPdfField(3, 0, "Text4.8", "Condition");
            registerPdfField(4, 0, "Text5.8", "Condition");
            registerPdfField(5, 0, "Text6.8", "Condition");
            registerPdfField(6, 0, "Text7.8", "Condition");
            registerPdfField(7, 0, "Text8.8", "Condition");
        }

        private String getFormFieldName(int group, int row, String logicalName)
        {
            String key = CTaxformRv1PdfField.ConstructKey(group, row, logicalName);
            CTaxformRv1PdfField field = (CTaxformRv1PdfField) pdfFields[key];

            if (field == null)
            {
                return ("");
            }

            return (field.PdfFieldName);
        }

        private MVTaxFormPRV1 fetchData()
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

                MVTaxFormPRV1 m = new MVTaxFormPRV1(o);
                return (m);
            }

            return (null);
        }

        private ArrayList constructItems()
        {
            CTaxFormPageRv1 page = null;
            CTaxFormGroupByEmployeeRv1 group = null;
            ArrayList pages = new ArrayList();

            while (true)
            {
                MVTaxFormPRV1 m = fetchData();
                if (m == null)
                {
                    break;
                }

                if ((page == null) || (page.GroupCount >= 8))
                {
                    page = new CTaxFormPageRv1();
                    pages.Add(page);
                }

                group = new CTaxFormGroupByEmployeeRv1(m);
                page.AddGroup(group);

                group.AddWhItem(m);
                suppliers[m.SupplierName] = m.SupplierName;
            }

            if (pages.Count > 0)
            {
                int lastIdx = pages.Count - 1;

                CTaxFormPageRv1 lastPage = (CTaxFormPageRv1) pages[lastIdx];
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
            String coverName = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.Resources.PP1_SUMMARY.pdf";
            String detailName = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.Resources.PP1_DETAIL.pdf";

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
            foreach (CTaxFormPageRv1 page in pages)
            {
                i++;

                Stream detailPageStream = assembly.GetManifestResourceStream(detailName);

                MemoryStream n = new MemoryStream();
                PdfStamper pdfDetailStamper = new PdfStamper(new PdfReader(detailPageStream), n);
                AcroFields pdfDetailFormFields = pdfDetailStamper.AcroFields;

                populateDetailFormValue(pdfDetailFormFields, 2, i, pages.Count, page);

                pdfDetailStamper.FormFlattening = true;
                pdfDetailStamper.Close();

                PdfReader reader = new PdfReader(new MemoryStream(n.ToArray()));
                PdfDictionary pd = reader.GetPageN(1);
                pd.Put(PdfName.ROTATE, new PdfNumber(90));

                pdf.AddDocument(reader);

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

            //pdfFormFields.SetField("Radio Button0", "0", true);
            //pdfFormFields.SetField("Radio Button3", "0", true);

            pdfFormFields.SetField("Text1.18", actualView.TaxYearBD);

            //int month = CUtil.StringToInt(actualView.TaxMonth);
            //int monthIdx = monthMap[month][idx];
            //pdfFormFields.SetField("Radio Button10", monthIdx.ToString(), true);            


            pdfFormFields.SetField("Text1.19", pages.Count.ToString());
            //pdfFormFields.SetField("Text1.20", pages.Count.ToString());

            pdfFormFields.SetField("Text2.1", CTaxDocumentUtil.FormatNumberField(actualView.ItemCount));
            pdfFormFields.SetField("Text2.2", CTaxDocumentUtil.FormatNumberField(actualView.ExpenseAmount));
            pdfFormFields.SetField("Text2.3", CTaxDocumentUtil.FormatNumberField(actualView.WhAmount));

            pdfFormFields.SetField("Text2.18", CTaxDocumentUtil.FormatNumberField(actualView.ItemCount));
            pdfFormFields.SetField("Text2.19", CTaxDocumentUtil.FormatNumberField(actualView.ExpenseAmount));
            pdfFormFields.SetField("Text2.20", CTaxDocumentUtil.FormatNumberField(actualView.WhAmount));

            pdfFormFields.SetField("Text2.22", CTaxDocumentUtil.FormatNumberField(actualView.WhAmount));
        }

        private void MonitorProgress()
        {
            registerPdfFields_1();
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
