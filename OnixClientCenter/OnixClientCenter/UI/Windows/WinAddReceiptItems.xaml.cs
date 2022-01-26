using Onix.Client.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddReceiptItems : Window
    {
        private String title = "";
        private MEntity vw = new MEntity(new CTable("CUSTOMER"));
        private MEntity actualView = null;
        private ObservableCollection<MBaseModel> parrentsItems = null;
        private String mode = "";

        public WinAddReceiptItems(String m, ObservableCollection<MBaseModel> parents, MEntity actView, String t)
        {
            title = t;
            parrentsItems = parents;
            actualView = actView;
            mode = m;

            InitializeComponent();
        }

        public String Caption
        {
            get
            {
                return (title);
            }
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            vw.InitTxArApMovement();
        }

        private GridView getGridView()
        {
            GridView gv = lsvMain.View as GridView;
            return (gv);
        }

        private void lsvMainMovement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = getGridView();

            double w = (lsvMain.ActualWidth * 1) - 35;
            double[] ratios = new double[6] { 0.05, 0.15, 0.15, 0.15, 0.10, 0.40 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadData()
        {
            //this.Title = Caption;

            //vw.CreateDefaultValue();
            //DataContext = vw;

            //CUtil.EnableForm(false, this);

            //CTable m = OnixWebServiceAPI.GetEntityInfo(actualView.GetDbObject());
            //if (m != null)
            //{
            //    vw.SetDbObject(m);
            //    vw.NotifyAllPropertiesChanged();

            //    vw.InitTxArApMovement();

            //}

            //CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void cmdNavigate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbkSelected_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void lsvSelected_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvSelected.View as GridView;

            double w = (lsvSelected.ActualWidth * 1) - 35;
            double[] ratios = new double[6] { 0.05, 0.15, 0.15, 0.15, 0.10, 0.40 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }
    }
}
