using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Onix.Client.Model;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.ClientCenter
{
    public partial class WinErrorDetails : Window
    {
        private ObservableCollection<MError> errorItems = new ObservableCollection<MError>();

        public String Caption = "";
        public String Mode = "";

        public WinErrorDetails(ObservableCollection<MError> items, String TypeDoc)
        {
            InitInitialize(items);
        }

        public WinErrorDetails(ObservableCollection<MError> items)
        {
            InitInitialize(items);
        }

        public void InitInitialize(ObservableCollection<MError> items)
        {
            normalizeError(items);
            InitializeComponent();
        }

        private void normalizeError(ObservableCollection<MError> errors)
        {
            int cnt = 0;
            foreach (MError err in errors)
            {
                cnt++;

                string[] stringSeparators = new string[] { "|" };
                string[] result;

                String errorDesc = err.ErrorDesc;
                result = errorDesc.Split(stringSeparators, StringSplitOptions.None);

                String errcd = result[0];

                MError m = new MError(new CTable(""));
                m.Seq = cnt.ToString();

                m.Description = err.ErrorNormalizeDesc;
                errorItems.Add(m);
            }
        }

        public ObservableCollection<MError> ErrorItems
        {
            get
            {
                return (errorItems);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void LoadData()
        {
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void lsvError_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvError.ActualWidth * 1) - 35;
            double[] ratios = new double[2] { 0.05, 0.95 };
            CUtil.ResizeGridViewColumns(lsvError.View as GridView, ratios, w);
        }
    }
}
