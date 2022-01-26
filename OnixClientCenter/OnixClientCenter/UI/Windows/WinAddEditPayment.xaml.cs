using System;
using System.Windows.Controls;
using System.Collections;
using System.Windows;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditPayment : Window
    {
        private MAccountDoc doc = null;
        private MAccountDoc orgDoc = null;
        private Boolean isOK = false;
        private String amt = "";
        private String paid = "";
        private String remain = "";
        private Boolean isInLoad = true;
        private Boolean isAr = false;

        public WinAddEditPayment(String rcptAmt, MAccountDoc acctDoc, Boolean arFlag)
        {
            orgDoc = acctDoc;
            isAr = arFlag;

            doc = new MAccountDoc(acctDoc.GetDbObject().CloneAll());
            doc.InitAccountDocPayment();
            doc.IsModified = false;

            populateCashType(doc);

            DataContext = doc;

            amt = rcptAmt;
            InitializeComponent();
        }

        private void populateCashType(MAccountDoc d)
        {
            Hashtable hash = new Hashtable();

            hash[(int) AccountDocumentType.AcctDocCashSale] = "SALE_CASH_CHANGE_TYPE";
            hash[(int) AccountDocumentType.AcctDocArReceipt] = "SALE_DEBT_CHANGE_TYPE";
            hash[(int) AccountDocumentType.AcctDocCashPurchase] = "PURCHASE_CASH_CHANGE_TYPE";
            hash[(int) AccountDocumentType.AcctDocApReceipt] = "PURCHASE_DEBT_CHANGE_TYPE";

            if (d.PaymentItems.Count > 0)
            {
                return;
            }

            int dt = CUtil.StringToInt(d.DocumentType);
            String key = (String) hash[dt];
            d.ChangeType = "1"; // CGlobalVariable.GetGlobalVariableValue(key); - ปิด feature ทอนด้วยเครดิต
        }

        public Boolean IsEditable
        {
            get
            {
                return (doc.IsEditable);
            }
        }

        public ObservableCollection<MAccountDocPayment> PaymentItems
        {
            get
            {
                return (doc.PaymentItemsNoChange);
            }
        }

        public String ReceiptAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(amt));
            }

            set
            {
            }
        }

        public String PaidAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(paid));
            }

            set
            {
            }
        }

        public String RemainAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(remain));
            }

            set
            {
            }
        }

        public Boolean IsOK
        {
            get
            {
                return (isOK);
            }
        }

        public Boolean IsAr
        {
            get
            {
                return (isAr);
            }
        }

        public Boolean IsAp
        {
            get
            {
                return (!IsAr);
            }
        }

        public TextSearchNameSpace PaymentNs
        {
            get
            {
                if (IsAr)
                {
                    return (TextSearchNameSpace.ArChequeNS);
                }

                return (TextSearchNameSpace.ApChequeNS);
            }
        }

        private Boolean validatePayment<T>(ObservableCollection<T> collection) where T : MBaseModel
        {
            int idx = 0;
            foreach (MBaseModel c in collection)
            {
                if (c.ExtFlag.Equals("D"))
                {
                    continue;
                }

                if ((c as MAccountDocPayment).Direction.Equals("2"))
                {
                    //Changes
                    continue;
                }

                idx++;
                MAccountDocPayment bi = (MAccountDocPayment)c;

                if (bi.PaymentType.Equals(""))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    return (false);
                }

                if (!bi.PaymentType.Equals("1"))
                {
                    if (bi.BankID.Equals(""))
                    {
                        CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                        return (false);
                    }

                    if (!bi.PaymentType.Equals("4"))
                    {
                        if (bi.CashAccountID.Equals(""))
                        {
                            CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                            return (false);
                        }
                    }
                }

                if (bi.PaidAmount.Equals(""))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    return (false);
                }
            }

            return (true);
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            double chgAmt = CUtil.StringToDouble(txtLeftAmount.Text);
            if (doc.IsChangeByCash && (chgAmt < 0))
            {
                CHelper.ShowErorMessage(txtLeftAmount.Text, "ERROR_CHANGE_BY_CASH_NEGATIVE", null);
                return (false);
            }

            result = validatePayment(doc.PaymentItems);

            return (result);
        }

        private void RootElement_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (doc.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    Boolean r = ValidateData();
                    if (r)
                    {
                        saveData();
                    }

                    isOK = r;
                    e.Cancel = !r;
                }
                else
                {
                    isOK = false;
                }
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            MAccountDocPayment pmt = new MAccountDocPayment(new CTable(""));
            pmt.PaymentType = "1";
            pmt.Direction = "1";
            pmt.RefundStatus = "1";
            if (IsAr)
            {
                pmt.Category = "1";
            }
            else
            {
                pmt.Category = "2";
            }

            doc.AddAccountDocPayment(pmt);
            
            calculateAssociateAmount();
            doc.IsModified = true;
        }

        private void createChangeTransaction()
        {
            //ArrayList arr2 = doc.GetDbObject().GetChildArray("ACCOUNT_DOC_PAYMENTS");
            //foreach (CTable t in arr2)
            //{
            //    MAccountDocPayment p = new MAccountDocPayment(t);
            //    if (p.Direction.Equals("2"))
            //    {
            //        //Only one Change transaction
            //        p.PaidAmount = remain.ToString();
            //        p.ChangeType = doc.ChangeType;

            //        return;
            //    }
            //}

            ////Create new one
            //MAccountDocPayment pmt = new MAccountDocPayment(new CTable(""));
            //pmt.PaidAmount = remain.ToString();
            //pmt.Direction = "2";
            //pmt.PaymentType = "1";
            //pmt.ChangeType = doc.ChangeType;

            //doc.AddAccountDocPayment(pmt);
        }

        private void saveData()
        {
            orgDoc.ChangeType = doc.ChangeType;
            createChangeTransaction();

            ArrayList arr1 = new ArrayList();
            CTable o = orgDoc.GetDbObject();            

            ArrayList arr2 = doc.GetDbObject().GetChildArray("ACCOUNT_DOC_PAYMENTS");
            if (arr2 == null)
            {
                return;
            }

            foreach (CTable t in arr2)
            {
                arr1.Add(t);
            }

            o.RemoveChildArray("ACCOUNT_DOC_PAYMENTS");
            o.AddChildArray("ACCOUNT_DOC_PAYMENTS", arr1);
            orgDoc.PaymentItems.Clear();

            foreach (MAccountDocPayment pmt in doc.PaymentItems)
            {
                orgDoc.PaymentItems.Add(pmt);
            }          
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = ValidateData();
            if (r)
            {
                isOK = true;
                doc.IsModified = false;
                saveData();

                this.Close();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            isOK = false;
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            MAccountDocPayment v = (MAccountDocPayment)(sender as Button).Tag;
            doc.RemoveAccountDocPayment(v);

            calculateAssociateAmount();
            doc.IsModified = true;
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            MAccountDocPayment pmt = (MAccountDocPayment) tb.Tag;

            if (pmt == null)
            {
                return;
            }

            //pmt.PaidAmount = tb.Text;
            calculateAssociateAmount();

            doc.IsModified = true;
        }

        private void cboStepUnitType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            doc.IsModified = true;
        }

        private void RootElement_ContentRendered(object sender, EventArgs e)
        {
            calculateAssociateAmount();
            isInLoad = false;
        }

        private void calculateAssociateAmount()
        {
            double total = 0;

            foreach (MAccountDocPayment pmt in doc.PaymentItemsNoChange)
            {
                if (!pmt.ExtFlag.Equals("D"))
                {
                    total = total + CUtil.StringToDouble(pmt.PaidAmount) + CUtil.StringToDouble(pmt.FeeAmount);
                }
            }

            paid = total.ToString();
            remain = (CUtil.StringToDouble(paid) - CUtil.StringToDouble(amt)).ToString();

            txtPaidAmount.Text = CUtil.FormatNumber(paid);
            txtLeftAmount.Text = CUtil.FormatNumber(remain);
        }

        private void radByCash_Checked(object sender, RoutedEventArgs e)
        {
            if (isInLoad)
            {
                return;
            }

            doc.IsModified = true;
        }

        private void txtChequeNo_TextChanged(object sender, EventArgs e)
        {
            doc.IsModified = true;
        }

        private MCheque getCheque(String cheqNo)
        {
            MCheque cheq = new MCheque(new CTable(""));
            cheq.ChequeNo = cheqNo;

            CUtil.EnableForm(false, this);
            ArrayList arr = OnixWebServiceAPI.GetChequeList(cheq.GetDbObject());
            CUtil.EnableForm(true, this);
            if ((arr == null) || (arr.Count <= 0))
            {
                return (null);
            }

            CTable t = (CTable) arr[0];
            return (new MCheque(t));
        }

        private MCheque getChequeByID(String cheqID)
        {
            MCheque cheq = new MCheque(new CTable(""));
            cheq.ChequeID = cheqID;

            CUtil.EnableForm(false, this);
            CTable cq = OnixWebServiceAPI.GetChequeInfo(cheq.GetDbObject());
            CUtil.EnableForm(true, this);
            if (cq == null)
            {
                return (null);
            }

            return (new MCheque(cq));
        }

        private void txtChequeNo_TextSelected(object sender, EventArgs e)
        {
            UTextBox txt = sender as UTextBox;
            MAccountDocPayment pmt = (MAccountDocPayment)txt.Tag;
            String code = txt.Text;

            MCheque cheq = getCheque(code);
            if (cheq == null)
            {
                pmt.ChequeNo = "";
                pmt.PaidAmount = "0.00";
                pmt.ChequeID = "";
            }
            else
            {
                pmt.BankID = cheq.ChequeBankID;
                pmt.PaidAmount = cheq.ChequeAmount;
                pmt.ChequeID = cheq.ChequeID;
            }            
        }

        private void txtChequeNo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (doc.ChequeID.Equals(""))
            {
                return;
            }

            UTextBox txt = sender as UTextBox;
            MAccountDocPayment pmt = (MAccountDocPayment)txt.Tag;

            if (pmt.ChequeID.Equals(doc.ChequeID))
            {
                return;
            }

            MCheque cheq = getChequeByID(doc.ChequeID);
            if (cheq == null)
            {
                pmt.ChequeNo = "";
                pmt.PaidAmount = "0.00";
                pmt.ChequeID = "";
                pmt.ChequeNo = "";
            }
            else
            {
                pmt.ChequeNo = cheq.ChequeNo;
                pmt.BankID = cheq.ChequeBankID;
                pmt.PaidAmount = cheq.ChequeAmount;
                pmt.ChequeID = cheq.ChequeID;
            }
        }

        private void RootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
