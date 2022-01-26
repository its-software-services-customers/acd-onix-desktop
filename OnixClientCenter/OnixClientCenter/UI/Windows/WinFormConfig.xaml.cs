using System;
using System.Windows.Controls;
using System.Windows;
using Onix.ClientCenter.Windows;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinFormConfig : Window
    {
        private String reportName = "";
        public Boolean IsOK = false;
        public MBaseModel ReturnedObj = null;
        public MFormConfig fc = null;

        public WinFormConfig(String prtName, MFormConfig formConfig)
        {
            reportName = prtName;
            fc = formConfig;

            DataContext = fc;
            InitializeComponent();
        }
        
        public String WindowTitle
        {
            get
            {
                return (reportName);
            }
        }

        private void resizeGridViewColumns(GridView grdv, double[] ratios, double w)
        {
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            fc.IsModified = false;
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.ContextMenu.IsOpen = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (!fc.IsModified)
            {
                IsOK = true;
                this.Close();
                return;
            }

            Boolean r = SaveData();
            if (r)
            {
                IsOK = true;

                fc.IsModified = false;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lsvMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvMain.ActualWidth * 1) - 35;
            double[] ratios = new double[4] { 0.2, 0.2, 0.50, 0.10 };
            CUtil.ResizeGridViewColumns(lsvMain.View as GridView, ratios, w);
        }

        private Boolean SaveData()
        {
            return (true);
        }

        private void rootElement_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (fc.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    Boolean r = SaveData();
                    e.Cancel = !r;

                    if (r)
                    {
                        IsOK = true;
                    }
                }
            }
        }

        private void cmdVariableRemove_Click(object sender, RoutedEventArgs e)
        {
            MFormConfigVariable o = (MFormConfigVariable) (sender as Button).Tag;
            fc.RemoveConfigVariable(o);
            fc.IsModified = true;
        }

        private void cmdVariableAdd_Click(object sender, RoutedEventArgs e)
        {
            WinAddEditFormVariable wf = new WinAddEditFormVariable("A", fc, null);
            String caption = CLanguage.getValue("variable");
            wf.Caption = CLanguage.getValue("add") + " " + caption;
            wf.ShowDialog();

            if (wf.HasModified)
            {
                fc.IsModified = wf.HasModified;
            }
        }

        private void txtVariableSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void lsvMain_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvMain.SelectedItems.Count == 1)
            {
                MFormConfigVariable o = (MFormConfigVariable)lsvMain.SelectedItems[0];

                String mode = "O";
                if (o.Scope.Equals(fc.Language))
                {
                    mode = "E";
                }

                WinAddEditFormVariable wf = new WinAddEditFormVariable(mode, fc, o);
                String caption = CLanguage.getValue("variable");
                wf.Caption = CLanguage.getValue("edit") + " " + caption;
                wf.ShowDialog();

                if (wf.HasModified)
                {
                    fc.IsModified = wf.HasModified;
                }
            }
        }

        private void cmdVariableOverride_Click(object sender, RoutedEventArgs e)
        {
            MFormConfigVariable o = (MFormConfigVariable)(sender as Button).Tag;
            fc.RemoveConfigVariable(o);
            fc.IsModified = true;
        }

        private void lsvMain_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                lsvMain_MouseDoubleClick(sender, null);
            }
        }
    }
}
