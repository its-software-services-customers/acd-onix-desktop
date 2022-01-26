using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditVoidedDoc : Window
    {
        private MVoidDoc vw = new MVoidDoc(new CTable(""));
        private MAccountDoc accDoc = null;

        public String Caption = "";
        public String Mode = "";

        public Boolean DialogOK = false;

        public WinAddEditVoidedDoc(MAccountDoc doc)
        {
            this.Title = CLanguage.getValue("void_document");

            accDoc = doc;
            vw.DocumentDate = doc.DocumentDate;
            vw.DocumentNo = doc.DocumentNo;
            vw.AccountDocID = doc.AccountDocId;

            DataContext = vw;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblCode, txtCode, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateComboBox(lblReason, cboVoidReason, false);
            if (!result)
            {
                return (result);
            }

            return (result);
        }

        private Boolean SaveToView()
        {
            if (!ValidateData())
            {
                return (false);
            }

            return (true);
        }

        private Boolean SaveData()
        {
            if (!CHelper.VerifyAccessRight("SALE_VOID_EDIT"))
            {
                return (false);
            }

            //if (!vw.IsModified)
            //{
            //    return (true);
            //}

            Boolean result = SaveToView();
            if (!result)
            {
                return (false);
            }

            CUtil.EnableForm(false, this);
            CTable t = OnixWebServiceAPI.ApproveVoidedDoc(vw.GetDbObject());
            CUtil.EnableForm(true, this);
            if (t != null)
            {
                MInventoryDoc ivd = new MInventoryDoc(t);
                ivd.InitErrorItem();
                if (ivd.ErrorItems.Count > 0)
                {
                    WinErrorDetails w = new WinErrorDetails(ivd.ErrorItems, "InventoryDoc");
                    w.Title = CLanguage.getValue("approve_error");
                    w.ShowDialog();

                    return (false);
                }
                else
                {
                    accDoc.SetDbObject(t);
                    accDoc.NotifyAllPropertiesChanged();

                    return (true);
                }
            }

            CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_EDIT", null);
            CUtil.EnableForm(true, this);

            return (false);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveData();
            if (r)
            {
                vw.IsModified = false;
                DialogOK = true;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmdOK.Focus();
            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    Boolean r = SaveData();
                    e.Cancel = !r;
                }
            }
        }

        private void LoadData()
        {
            vw.IsModified = false;

            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboCycleID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxEnable_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxEnable_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboCycleID_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void dtFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdVerify_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveToView();
            if (!r)
            {
                return;
            }

            CUtil.EnableForm(false, this);
            CTable t = OnixWebServiceAPI.VerifyVoidedDoc(vw.GetDbObject().CloneAll());
            CUtil.EnableForm(true, this);

            if (t != null)
            {
                MInventoryDoc ivd = new MInventoryDoc(t);
                ivd.InitErrorItem();
                if (ivd.ErrorItems.Count > 0)
                {
                    WinErrorDetails w = new WinErrorDetails(ivd.ErrorItems, "InventoryDoc");
                    w.Title = CLanguage.getValue("approve_error");
                    w.ShowDialog();
                }
                else
                {
                    String msg = CLanguage.getValue("VERIFY_SUCCESS");
                    CMessageBox.Show(msg, "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }           
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
