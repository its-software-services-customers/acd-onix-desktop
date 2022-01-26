using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Windows
{
    /// <summary>
    /// Interaction logic for WinLogImport.xaml
    /// </summary>
    public partial class WinLogImport : Window
    {
        private MLogImportIssue vw = null;
        private MLogImportIssue actualView = null;

        private ObservableCollection<MBaseModel> parentItemsSource = null;

        private String Caption = "";
        private String Mode = "";
        //private Boolean isOK = false;
        //private Boolean isInLoadData = false;

        public WinLogImport(String mode, ObservableCollection<MBaseModel> parentSources, MLogImportIssue data, String caption)
        {
            Mode = mode;
            parentItemsSource = parentSources;
            actualView = data;
            Caption = caption;

            vw = new MLogImportIssue(new CTable(""));
            DataContext = vw;

            //dtImportDate.IsDropDownOpen = false;

            InitializeComponent();
        }

        private void LoadData()
        {
            //isInLoadData = true;

            this.Title = Caption;
            vw.CreateDefaultValue();
            CUtil.EnableForm(false, this);

            if (Mode.Equals("V"))
            {
                CTable m = OnixWebServiceAPI.GetLogImportIssueInfo(actualView.GetDbObject());
                if (m != null)
                {
                    vw.SetDbObject(m);
                    vw.InitBranchPOSError();
                    vw.NotifyAllPropertiesChanged();
                }
            }

            vw.IsModified = false;
            //isInLoadData = false;

            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            //Boolean r = SaveData();
            //if (r)
            //{
            //    vw.IsModified = false;
            //    CUtil.EnableForm(true, this);

            //    this.Close();
            //}

            this.Close();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmdOK.Focus();
            //if (vw.IsModified)
            //{
            //    Boolean result = CHelper.AskConfirmSave();
            //    if (result)
            //    {
            //        //Yes, save it
            //        Boolean r = SaveData();
            //        e.Cancel = !r;
            //    }
            //}
        }

        private void dtImportDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cboBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dtDocDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
