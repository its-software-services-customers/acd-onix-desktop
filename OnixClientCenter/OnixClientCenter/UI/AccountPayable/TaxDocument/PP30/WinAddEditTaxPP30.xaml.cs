using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using iTextSharp.text.pdf;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.UI.Cash.Cheque;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PP30
{
    public partial class WinAddEditTaxPP30 : WinBase
    {
        class Tupple
        {
            public double VatAmt { get; set; }
            public double Amount { get; set; }

            public Tupple()
            {
                VatAmt = 0.00;
                Amount = 0.00;
            }
        }

        private int[] monthMap = null;

        public WinAddEditTaxPP30(CWinLoadParam param) : base(param)
        {
            accessRightName = "PURCHASE_TAXDOC_EDIT";

            createAPIName = "CreateTaxDoc";
            updateAPIName = "UpdateTaxDoc";
            getInfoAPIName = "GetTaxDocInfo";
            approveAPIName = "ApproveTaxDoc";

            InitializeComponent();
            initMonthMap();

            //Need to be after InitializeComponent
            registerValidateControls(lblYear, txtYear, false);
            registerValidateControls(lblMonth, cboMonth, false);
            registerValidateControls(lblSaleAmount, txtSaleAmount, false);
            registerValidateControls(lblSaleZeroPctAmt, txtSaleZeroPctAmt, false);
            registerValidateControls(lblSaleExemptAmt, txtSaleExemptAmt, false);
            registerValidateControls(lblSaleEligibleAmt, txtSaleEligibleAmt, false);
            registerValidateControls(lblSaleVatAmt, txtSaleVatAmt, false);
            registerValidateControls(lblPurchaseEligibleAmt, txtPurchaseEligibleAmt, false);
            registerValidateControls(lblPurchaseVatAmt, txtPurchaseVatAmt, false);
            registerValidateControls(lblVatClaimAmt, txtVatClaimAmt, false);
            registerValidateControls(lblVatExtraAmt, txtVatExtraAmt, false);
            registerValidateControls(lblVatPreviousFwdAmt, txtVatPreviousFwdAmt, false);
            registerValidateControls(lblVatClaimTotalAmt, txtVatClaimTotalAmt, false);
            registerValidateControls(lblVatExtraTotalAmt, txtVatExtraTotalAmt, false);
            registerValidateControls(lblAdditionalAmt, txtAdditionalAmt, false);
            registerValidateControls(lblPenaltyAmt, txtPenaltyAmt, false);
            registerValidateControls(lblVatClaimGrandAmt, txtVatClaimGrandAmt, false);
            registerValidateControls(lblVatExtraGrandAmt, txtVatExtraGrandAmt, false);
        }

        private void initMonthMap()
        {
            monthMap = new int[13] { -1, 0, 4, 8, 1, 5, 9, 2, 6, 10, 3, 7, 11 }; //Index 0 is dummy
        }

        protected override bool isEditable()
        {
            MVTaxDocument mv = (MVTaxDocument)loadParam.ActualView;
            if (mv != null)
            {
                return (mv.IsEditable);
            }

            return (true);
        }

        protected override MBaseModel createObject()
        {
            MVTaxDocument mv = new MVTaxDocument(new CTable(""));
            mv.DocumentType = ((int)TaxDocumentType.TaxDocPP30).ToString();
            mv.DocumentStatus = "1";

            mv.DocumentDate = DateTime.Now;

            if (loadParam.Mode.Equals("A"))
            {
                mv.AddDefaultTaxPP30();
            }

            return (mv);
        }

        private void CmdCalculate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Boolean result = CHelper.ValidateTextBox(lblYear, txtYear, false);
            if (!result)
            {
                return;
            }

            result = CHelper.ValidateComboBox(lblMonth, cboMonth, false);
            if (!result)
            {
                return;
            }

            MVTaxDocument vm = (MVTaxDocument) vw;

            String mm = "";
            int m = CUtil.StringToInt(vm.TaxMonth);
            if (m < 10)
            {
                mm = "0" + m;
            }
            else
            {
                mm = "" + m;
            }

            String yyyymm = vm.TaxYear + mm;

            CTable t = new CTable("");
            t.SetFieldValue("DOCUMENT_STATUS", "2");
            t.SetFieldValue("DOCUMENT_YYYY", vm.TaxYear);
            t.SetFieldValue("DOCUMENT_YYYYMM", yyyymm);
            CUtil.EnableForm(false, this);
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetVatAmountByDocTypeInMonth", "DOCTYPE_SUMMARY_LIST", t);
            populateAmount(arr);
            CUtil.EnableForm(true, this);
        }

        private void populateAmount(ArrayList arr)
        {
            int[] types = new int[20];
            //ซื้อ - 1, ขาย - 0, อื่น ๆ ไม่คิดคำนวณ - 2
            types[(int) AccountDocumentType.AcctDocCashSale] = 0;
            types[(int) AccountDocumentType.AcctDocDebtSale] = 0;
            types[(int) AccountDocumentType.AcctDocCrNote] = 0;
            types[(int) AccountDocumentType.AcctDocDrNote] = 0;
            types[(int) AccountDocumentType.AcctDocCashPurchase] = 1;
            types[(int) AccountDocumentType.AcctDocDebtPurchase] = 1;
            types[(int) AccountDocumentType.AcctDocCrNotePurchase] = 1; 
            types[(int) AccountDocumentType.AcctDocDrNotePurchase] = 1;
            types[(int)AccountDocumentType.AcctDocArReceipt] = 2;
            types[(int) AccountDocumentType.AcctDocApReceipt] = 2;            
            types[(int) AccountDocumentType.AcctDocMiscRevenue] = 0;
            types[(int) AccountDocumentType.AcctDocMiscExpense] = 1;
            types[(int) AccountDocumentType.AcctDocCashDepositAr] = 2;
            types[(int) AccountDocumentType.AcctDocCashDepositAp] = 2;
            types[(int) AccountDocumentType.AcctDocSaleOrder] = 2;
            types[(int) AccountDocumentType.AcctDocBillSummary] = 2;

            Tupple sale = new Tupple();
            Tupple purchase = new Tupple();
            Tupple others = new Tupple();

            Tupple[] temps = new Tupple[3];

            temps[0] = sale;
            temps[1] = purchase;
            temps[2] = others;

            foreach (CTable dat in arr)
            {
                int dt = CUtil.StringToInt(dat.GetFieldValue("DOCUMENT_TYPE"));
                double vat = CUtil.StringToDouble(dat.GetFieldValue("VAT_AMT"));
                double amt = CUtil.StringToDouble(dat.GetFieldValue("REVENUE_EXPENSE_FOR_VAT_AMT"));

                int factor = 1;
                if  ((dt == (int)AccountDocumentType.AcctDocCrNote) || (dt == (int)AccountDocumentType.AcctDocCrNotePurchase))
                {
                    factor = -1;
                }

                int idx = types[dt];

                Tupple t = temps[idx];
                t.VatAmt = t.VatAmt + factor * vat;
                t.Amount = t.Amount + factor * amt;
            }

            MVTaxDocument vm = (MVTaxDocument)vw;
            MVTaxFormPP30 pp30 = vm.TaxFormPP30;

            pp30.SaleAmt = sale.Amount.ToString();
            pp30.SaleVatAmt = sale.VatAmt.ToString();
            pp30.PurchaseEligibleAmt = purchase.Amount.ToString();
            pp30.PurchaseVatAmt = purchase.VatAmt.ToString();

            pp30.NotifyPopulatedFields();
        }

        private void CmdOK_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }

        private void CmdSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!vw.IsModified)
            {
                return;
            }

            Boolean r = saveData();
            if (r)
            {
                loadParam.ActualView = vw;
                loadParam.Mode = "E";

                loadData();
                vw.IsModified = false;
            }
        }

        private void CmdVerify_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void CmdApprove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MVTaxDocument mv = (MVTaxDocument)vw;

            if (mv.ChequeID.Equals(""))
            {
                CHelper.ShowErorMessage("", "ERROR_CREATE_CHEQUE_FIRST", null);
                return;
            }

            Boolean r = approveData();
            if (r)
            {
                //Approve cheque as well
                CUtil.EnableForm(false, this);
                CTaxDocumentUtil.ApproveChequeFromTaxDoc(mv);
                CUtil.EnableForm(true, this);

                vw.IsModified = false;
                this.Close();
            }
        }

        private void populateFormValue(AcroFields pdfFormFields)
        {
            MCompanyProfile cmp = CMasterReference.Instance.CompanyProfile;
            MVTaxDocument vm = (MVTaxDocument) vw;
            MVTaxFormPP30 pp30 = vm.TaxFormPP30;

            //pdfFormFields.SetField("Text1.0", "0 1155 48000 18 6");
            pdfFormFields.SetField("Text1.0", CTaxDocumentUtil.FormatTaxIDField(cmp.TaxID));
            pdfFormFields.SetField("Text1.01", cmp.RegistrationName);
            pdfFormFields.SetField("Text1.3", cmp.RegistrationAddress);
            pdfFormFields.SetField("Text1.4", cmp.BuildingName);
            pdfFormFields.SetField("Text1.5", cmp.RoomNo);
            pdfFormFields.SetField("Text1.6", cmp.FloorNo);
            pdfFormFields.SetField("Text1.7", cmp.VillageName);
            pdfFormFields.SetField("Text1.8", cmp.HomeNo);
            pdfFormFields.SetField("Text1.9", cmp.Moo);
            pdfFormFields.SetField("Text1.10", cmp.Soi);
            pdfFormFields.SetField("Text1.11", cmp.Road);
            pdfFormFields.SetField("Text1.12", cmp.District);
            pdfFormFields.SetField("Text1.13", cmp.Town);
            pdfFormFields.SetField("Text1.14", cmp.Province);
            pdfFormFields.SetField("Text1.15", cmp.Zip);
            pdfFormFields.SetField("Text1.16", cmp.Telephone);

            pdfFormFields.SetField("Text1.22", vm.TaxYearBD);

            pdfFormFields.SetField("Text2.1", CTaxDocumentUtil.FormatNumberField(pp30.SaleAmt));
            pdfFormFields.SetField("Text2.2", CTaxDocumentUtil.FormatNumberField(pp30.SaleZeroPctAmt, "0 00"));
            pdfFormFields.SetField("Text2.3", CTaxDocumentUtil.FormatNumberField(pp30.SaleExemptAmt, "0 00"));
            pdfFormFields.SetField("Text2.4", CTaxDocumentUtil.FormatNumberField(pp30.SaleEligibleAmt, "0 00"));
            pdfFormFields.SetField("Text2.5", CTaxDocumentUtil.FormatNumberField(pp30.SaleVatAmt, "0 00"));

            pdfFormFields.SetField("Text2.6", CTaxDocumentUtil.FormatNumberField(pp30.PurchaseEligibleAmt));
            pdfFormFields.SetField("Text2.7", CTaxDocumentUtil.FormatNumberField(pp30.PurchaseVatAmt));

            pdfFormFields.SetField("Text2.8", CTaxDocumentUtil.FormatNumberField(pp30.VatClaimAmt));
            pdfFormFields.SetField("Text2.9", CTaxDocumentUtil.FormatNumberField(pp30.VatExtraAmt));
            pdfFormFields.SetField("Text2.10", CTaxDocumentUtil.FormatNumberField(pp30.VatPreviousFwdAmt));

            pdfFormFields.SetField("Text2.11", CTaxDocumentUtil.FormatNumberField(pp30.VatClaimTotalAmt));
            pdfFormFields.SetField("Text2.12", CTaxDocumentUtil.FormatNumberField(pp30.VatExtraTotalAmt));

            pdfFormFields.SetField("Text2.13", CTaxDocumentUtil.FormatNumberField(pp30.AdditionalAmt));
            pdfFormFields.SetField("Text2.14", CTaxDocumentUtil.FormatNumberField(pp30.PenaltyAmt));

            pdfFormFields.SetField("Text2.15", CTaxDocumentUtil.FormatNumberField(pp30.VatClaimGrandAmt));
            pdfFormFields.SetField("Text2.16", CTaxDocumentUtil.FormatNumberField(pp30.VatExtraGrandAmt));

            int month = CUtil.StringToInt(vm.TaxMonth);
            int monthIdx = monthMap[month];
            pdfFormFields.SetField("Radio Button3", monthIdx.ToString(), true);
        }

        private String constructPDFForm()
        {
            String name = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.Resources.PP30_310757K.pdf";
            String newFile = String.Format("{0}{1}", Path.GetTempPath(), "pp30.pdf");
            if (File.Exists(newFile))
            {
                File.Delete(newFile);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream pdfStream = assembly.GetManifestResourceStream(name);
            PdfReader pdfReader = new PdfReader(pdfStream);
            
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));

            AcroFields pdfFormFields = pdfStamper.AcroFields;
            populateFormValue(pdfFormFields);

            pdfStamper.FormFlattening = false;

            pdfStamper.Close();
            pdfReader.Close();
            pdfStream.Close();

            return (newFile);
        }

        private void CmdPreview_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CUtil.EnableForm(false, this);
            String name = constructPDFForm();
            CUtil.EnableForm(true, this);

            WinPDFViewer wp = new WinPDFViewer(name, "ภ.พ.30");
            wp.ShowDialog();
            wp = null;
        }        

        private void CmdCheque_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MVTaxDocument vm = (MVTaxDocument)vw;
            MVTaxFormPP30 pp30 = vm.TaxFormPP30;
            MAccountDoc ad = CTaxDocumentUtil.TaxDocToAccountDoc(vm, this);

            if (ad == null)
            {
                return;
            }

            if (vm.ChequeID.Equals(""))
            {
                ad.ArApAmt = pp30.VatClaimGrandAmt;

                ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();
                CCriteriaCheque.ShowAddChequeWindow("2", arr, ad);

                if (arr.Count > 0)
                {
                    MCheque cq = (MCheque)arr[0];
                    vm.ChequeID = cq.ChequeID;
                    vm.ChequeNo = cq.ChequeNo;
                    vm.IsModified = true;
                }
            }
            else
            {
                CCriteriaCheque.ShowEditWindow("2", null, ad);
            }
        }
    }
}
