using System;
using System.Windows.Controls;
using System.Collections;
using System.Windows;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditDiscount : Window
    {
        private MAccountDoc doc = null;
        private MAccountDoc orgDoc = null;
        private Boolean isOK = false;
        private Boolean isInLoad = true;

        public WinAddEditDiscount(String rcptAmt, MAccountDoc acctDoc)
        {
            orgDoc = acctDoc;

            doc = new MAccountDoc(acctDoc.GetDbObject().CloneAll());
            doc.InitAccountDocDiscount();
            doc.IsModified = false;

            DataContext = doc;

            InitializeComponent();
        }

        public static Boolean ShowDiscountWindow(MAccountDoc doc)
        {
            WinAddEditDiscount w = new WinAddEditDiscount("", doc);
            w.ShowDialog();

            return (w.isOK);
        }

        public Boolean IsOK
        {
            get
            {
                return (isOK);
            }
        }

        private Boolean validateDiscount<T>(ObservableCollection<T> collection) where T : MBaseModel
        {
            int idx = 0;
            foreach (MBaseModel c in collection)
            {
                if (c.ExtFlag.Equals("D"))
                {
                    continue;
                }

                idx++;
                MAccountDocDiscount bi = (MAccountDocDiscount)c;

                if (bi.DiscountType.Equals(""))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    return (false);
                }

                if (bi.DiscountAmt.Equals(""))
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

            result = validateDiscount(doc.DiscountItems);

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
            MAccountDocDiscount disc = new MAccountDocDiscount(new CTable(""));
            doc.AddAccountDocDiscount(disc);
            
            calculateAssociateAmount();
            doc.IsModified = true;
        }

        private void saveData()
        {
            ArrayList arr1 = new ArrayList();
            CTable o = orgDoc.GetDbObject();            

            ArrayList arr2 = doc.GetDbObject().GetChildArray("ACCOUNT_DOC_DISCOUNTS");
            if (arr2 == null)
            {
                return;
            }

            foreach (CTable t in arr2)
            {
                arr1.Add(t);
            }

            o.RemoveChildArray("ACCOUNT_DOC_DISCOUNTS");
            o.AddChildArray("ACCOUNT_DOC_DISCOUNTS", arr1);
            orgDoc.DiscountItems.Clear();

            foreach (MAccountDocDiscount pmt in doc.DiscountItems)
            {
                orgDoc.DiscountItems.Add(pmt);
            }

            orgDoc.FinalDiscount = doc.FinalDiscount;
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
            MAccountDocDiscount v = (MAccountDocDiscount)(sender as Button).Tag;
            doc.RemoveAccountDocDiscount(v);

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
            MAccountDocDiscount disc = (MAccountDocDiscount) tb.Tag;

            if (disc == null)
            {
                return;
            }

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
            double discount = 0;

            foreach (MAccountDocDiscount disc in doc.DiscountItems)
            {
                if (!disc.ExtFlag.Equals("D"))
                {
                    discount = discount + CUtil.StringToDouble(disc.DiscountAmt);
                }
            }

            double amt = CUtil.StringToDouble(doc.PricingAmt) - discount;

            doc.FinalDiscount = discount.ToString();
            doc.RevenueExpenseAmt = amt.ToString();
        }

        private void radByCash_Checked(object sender, RoutedEventArgs e)
        {
            if (isInLoad)
            {
                return;
            }

            doc.IsModified = true;
        }

        private void RootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
