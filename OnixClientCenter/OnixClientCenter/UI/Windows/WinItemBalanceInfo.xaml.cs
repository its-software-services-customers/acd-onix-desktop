using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinItemBalanceInfo : Window
    {
        private MInventoryItem vw = null;
        private MInventoryItem actualView = null;

        private ObservableCollection<MBaseModel> parentItemsSource = null;
        private Boolean isModified = false;
        private MInventoryCurrentBalance currentViewObj = null;

        public String Caption = "";
        public String Mode = "";

        public WinItemBalanceInfo()
        {            
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public MInventoryItem ViewData
        {
            set
            {
                actualView = value;
            }
        }

        public ObservableCollection<MBaseModel> ParentItemSource
        {
            set
            {
                parentItemsSource = value;
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = true;

            return (result);
        }

        private Boolean SaveToView( )
        {
            if (!ValidateData())
            {
                return(false);
            }

            return (true);
        }

        private Boolean SaveData(String approveFlag)
        {
            return (true);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveData("N");
            if (r)
            {
                isModified = false;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isModified)
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
            String type = "";

            this.Title = Caption;

            CTable t = new CTable("INVENTORY_ITEM");
            vw = new MInventoryItem(t);
            vw.CreateDefaultValue();            

            DataContext = vw;

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable m = OnixWebServiceAPI.GetInventoryItemBalanceInfo(actualView.GetDbObject());
                if (m != null)
                {
                    //actualView.GetDbObject().AddChildArray("CURRENT_BALANCE_ITEM", m.GetChildArray("CURRENT_BALANCE_ITEM"));
                    vw.SetDbObject(m);
                    vw.NotifyAllPropertiesChanged();

                    type = vw.ItemType;
                    lkupItem.SelectedObject = vw;
                    vw.InitBalanceItem();
                }              
            }
            
            lsvCurrentBalance.ItemsSource = vw.BalanceItems;

            //CUtil.LoadMasterRefCombo(cboType, true, MasterRefEnum.MASTER_ITEM_TYPE, type);

            isModified = false;

            CUtil.EnableForm(true, this);
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
                WinItemLotInfo w = new WinItemLotInfo(currentViewObj);
                w.Title = (String)mnu.Header;
                w.Mode = "E";
                w.ShowDialog();
            }
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MInventoryCurrentBalance)btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void cmdNavigate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lsvCurrentBalance_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvCurrentBalance.ActualWidth * 1) - 35;
            double[] ratios = new double[7] { 0.05, 0.05, 0.15, 0.24, 0.18, 0.18, 0.15 };
            CUtil.ResizeGridViewColumns(lsvCurrentBalance.View as GridView, ratios, w);
        }

        private void lsvCurrentBalance_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvCurrentBalance.SelectedItems.Count == 1)
            {
                currentViewObj = (MInventoryCurrentBalance)lsvCurrentBalance.SelectedItems[0];
                ShowEditWindow();
            }
        }

        private void ShowEditWindow()
        {
            String caption = (String)tbibalance.Header;
            WinItemLotInfo w = new WinItemLotInfo(currentViewObj);
            w.Title = CLanguage.getValue("edit") + " " + caption;
            w.Mode = "E";
            w.ShowDialog();


        }
    }
}
