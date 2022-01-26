using System;
using System.Windows.Controls;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;

namespace Onix.ClientCenter
{
    public partial class WinItemLotInfo : Window
    {
        //private MInventoryCurrentBalance vw = null;
        private MInventoryCurrentBalance actualView = null;

        public String Mode = "";

        public WinItemLotInfo(MInventoryCurrentBalance ib)
        {
            actualView = new MInventoryCurrentBalance(ib.GetDbObject().Clone());
            DataContext = actualView;
            InitializeComponent();
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
            CUtil.EnableForm(false, this);

            
            CTable m = OnixWebServiceAPI.GetCurrentBalanceInfo(actualView.GetDbObject());
            if (m != null)
            {
                MInventoryCurrentBalance vm = new MInventoryCurrentBalance(m);
                vm.NotifyAllPropertiesChanged();

                vm.InitCurrentLotTrackings();
                vm.InitCurrentMovements();
                vm.InitMovementSummaries();


                lsvMovementAvg.ItemsSource = vm.CurrentMovements;
                lsvMovementSum.ItemsSource = vm.MovementSummaries;
            }

            CUtil.EnableForm(true, this);
        }

        private void resizeGridViewColumns(GridView grdv, double[] ratios, double w)
        {
            for (int i = 0; i < ratios.Length; i++)
            {
                GridViewColumn gvc = grdv.Columns[i];
                gvc.Width = ratios[i] * w;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void mnuLotInfo_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuLotInfo"))
            {
                //WinAddEditImportItem w = new WinAddEditImportItem();
                //w.ViewData = currentViewObj;
                //w.Caption = (String)mnu.Header;
                //w.Mode = "E";
                ////w.ParentItemSource = (vw as MInventoryDoc).TxItems;
                //w.ShowDialog();

                //if (w.HasModified)
                //{
                //    isModified = true;
                //}
            }
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            //currentViewObj = (MInventoryTransaction)btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void cmdNavigate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lsvMovementAvg_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvMovementAvg.ActualWidth * 1) - 35;
            double[] ratios = new double[10] { 0.025,0.125, 0.15, 0.10, 0.10, 0.10, 0.10, 0.10, 0.10, 0.10 };
            resizeGridViewColumns(lsvMovementAvg.View as GridView, ratios, w);
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void lsvMovementSum_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvMovementSum.ActualWidth * 1) - 35;
            double[] ratios = new double[8] { 0.16, 0.12, 0.12, 0.12, 0.12, 0.12, 0.12, 0.12 };
            resizeGridViewColumns(lsvMovementSum.View as GridView, ratios, w);
        }
    }
}
