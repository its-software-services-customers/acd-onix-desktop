using System;
using System.Windows.Controls;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinDocumentConfig : Window
    {
        private MDocumentNumber vw = new MDocumentNumber(new CTable(""));
        private String cption = "";

        public WinDocumentConfig(String caption)
        {
            cption = caption;

            DataContext = vw;
            InitializeComponent();
        }

        public String Caption
        {
            get
            {
                return (cption);
            }
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (!vw.IsModified)
            {
                this.Close();
                return;
            }

            Boolean r = SaveData("N");
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = true;

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

        private Boolean SaveData(String approveFlag)
        {
            //if (!CHelper.VerifyAccessRight("GENERIC_PACKAGE_ADD"))
            //{
            //    return (false);
            //}

            //Edit mode, always

            if (vw.IsModified)
            {
                Boolean result = SaveToView();
                if (result)
                {
                    CUtil.EnableForm(false, this);
                    CTable t = OnixWebServiceAPI.UpdateDocumentNumber(vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (t != null)
                    {
                        return (true);
                    }

                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_EDIT", null);
                }

                return (false);
            }

            return (true);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    Boolean r = SaveData("N");
                    e.Cancel = !r;
                }
            }
        }

        private void LoadData()
        {
            CUtil.EnableForm(false, this);
            CTable o = OnixWebServiceAPI.GetDocumentNumberInfo(vw.GetDbObject());
            vw.SetDbObject(o);
            vw.InitItem(CProductFilter.IsDocTypeRequired);
            vw.NotifyAllPropertiesChanged();

            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
            vw.IsModified = false;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbxCustomSeq_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxCustomSeq_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
