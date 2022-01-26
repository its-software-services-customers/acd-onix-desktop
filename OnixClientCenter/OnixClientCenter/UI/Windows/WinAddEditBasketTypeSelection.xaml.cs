using System;
using System.Windows.Controls;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditBasketTypeSelection : Window
    {
        private MBasketTypeConfig cfg = new MBasketTypeConfig(new CTable(""));
        private Boolean isOK = false;
        private String t = "P";

        public WinAddEditBasketTypeSelection(String cfgString, String itemName, String type)
        {
            cfg.DeserializeConfig(cfgString, type);
            cfg.IsModified = false;

            t = type;

            this.DataContext = cfg;
            InitializeComponent();
        }

        public String ConfigString
        {
            get
            {
                return (cfg.ConfigString);
            }
        }

        public Boolean IsOK
        {
            get
            {
                return (isOK);
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = true;

            //result = CHelper.ValidateTextBox(lblDesc, txtDesc, false);
            //if (!result)
            //{
            //    return (result);
            //}

            //result = CHelper.ValidateTextBoxEscape(lblDesc, txtDesc, "|");
            //if (!result)
            //{
            //    return (result);
            //}

            return (result);
        }

        private void RootElement_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cfg.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    Boolean r = ValidateData();
                    isOK = r;
                    e.Cancel = !r;
                }
                else
                {
                    isOK = false;
                }
            }
        }

        private void RootElement_ContentRendered(object sender, EventArgs e)
        {
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = ValidateData();
            if (r)
            {
                cfg.IsModified = false;
                isOK = true;
                this.Close();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            isOK = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            cfg.IsModified = true;
        }

        private void lsvBasketType_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvBasketType.View as GridView;

            double w = (lsvBasketType.ActualWidth * 1) - 35;
            double[] ratios = new double[3] { 0.05, 0.20, 0.75 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void cbxBasketType_Checked(object sender, RoutedEventArgs e)
        {
            cfg.IsModified = true;
        }

        private void cbxBasketType_Unchecked(object sender, RoutedEventArgs e)
        {
            cfg.IsModified = true;
        }

        private void RootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
