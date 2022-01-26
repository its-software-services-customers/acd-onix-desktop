using System;
using System.Windows;
using System.Windows.Controls;
using Wis.WsClientAPI;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.ClientCenter.Windows;
using Onix.ClientCenter.Criteria;
using Onix.Client.Pricing;
using Onix.Client.Model;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.UControls
{
    public partial class UStandardPackage : UserControl
    {
        private MCompanyPackage vw = null;

        public String Caption = "";
        public String Mode = "";

        private ObservableCollection<MBaseModel> itemsSourceGrid = new ObservableCollection<MBaseModel>();
        private ObservableCollection<MSelectedItem> items = new ObservableCollection<MSelectedItem>();
        private MBillSimulate billSim = null;

        private MCompanyPackage currentViewObj = null;
        private String CompanyID = "1";
        private double[] ratios = new double[9] { 0.05, 0.05, 0.125, 0.32, 0.12, 0.10, 0.10, 0.045, 0.09 };

        public UStandardPackage()
        {
            InitializeComponent();
        }

        public void QueryData(MCompanyPackage vcp)
        {
            CUtil.EnableForm(false, this);
            MCompanyPackage cp = CMasterReference.GetCompanyPackage(true);
            if (cp != null)
            {
                vcp.SetDbObject(cp.GetDbObject().Clone());
                vcp.InitChildItems();
                vcp.NotifyAllPropertiesChanged();
            }
            CUtil.EnableForm(true, this);

            vw.IsModified = false;
        }

        public void InitUserControl()
        {
            CTable t = new CTable("COMPANY_PACKAGE");

            vw = new MCompanyPackage(t);
            (vw as MCompanyPackage).CreateDefaultValue();

            DataContext = vw;
            vw.CompanyID = CompanyID;
            QueryData(vw);
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("SALE_COMPANYPACKAGE_EDIT"))
            {
                return;
            }

            if (vw.IsModified)
            {
                CUtil.EnableForm(false, this);
                vw.CompanyID = CompanyID;
                CTable t = OnixWebServiceAPI.UpdateCompanyPackage(vw.GetDbObject());
                CUtil.EnableForm(true, this);
                if (t != null)
                {
                    cmdRefresh_Click(sender, e);
                }
                else
                {
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_EDIT", null);
                }
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            if (vw.IsModified)
            {
                cmdRefresh_Click(sender, e);
            }
        }

        private void grdViewInventoryItems_SubMenuItemClick(object sender, EventArgs e)
        {
        }


        private void cbkRemoveSelected_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            MCompanyPackageSelected vw = (MCompanyPackageSelected)chk.Tag;
            vw.EnableFlag = "Y";
            vw.updateFlag();
            vw.IsModified = true;
            
        }

        private void cbkRemoveSelected_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            MCompanyPackageSelected vw = (MCompanyPackageSelected)chk.Tag;
            vw.EnableFlag = "N";
            vw.updateFlag();
            vw.IsModified = true;
        }

        private void cbkRemove_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbkRemove_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MCompanyPackage) btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void mnuPackageView_Click(object sender, RoutedEventArgs e)
        {
            MPackage p = new MPackage(new CTable("PACKAGE"));
            p.CopyPackageInfo(currentViewObj);

            WinAddEditPackage c = new WinAddEditPackage(p.PackageGroup, "E");
            c.ViewData = p;
            c.Title = CLanguage.getValue("edit") + " " + CUtil.PackageTypeToString(p.PackageType);
            c.ShowDialog();

            if (c.IsOK)
            {
                //Will be reloade later
                MPackage mp = new MPackage(p.GetDbObject());
                CPriceProcessor.UnloadPackage(mp);
            }            
        }

        private void ReArrangeOrder(ObservableCollection<MCompanyPackage> items, MCompanyPackage vw, Boolean isDown)
        {
            ArrayList arr = new ArrayList();

            int currIdx = -1;
            int min = 99999999;
            int max = 0;
            int idx = 0;
            foreach (MCompanyPackage v in items)
            {
                int seq = int.Parse(v.SeqNo);
                if (seq < min)
                {
                    min = seq;
                }

                if (seq > max)
                {
                    max = seq;
                }

                if (v.SeqNo.Equals(vw.SeqNo))
                {
                    currIdx = idx;
                }

                arr.Add(v);
                idx++;
            }

            int cnt = idx++;
            MCompanyPackage swap = null;

            if (isDown)
            {
                if (currIdx >= cnt - 1)
                {
                    //Do nothing, this is the last item in the rows
                    return;
                }

                swap = (MCompanyPackage) arr[currIdx + 1];
            }
            else
            {
                //Up
                if (currIdx <= 0)
                {
                    //Do nothing, this is the first item 
                    return;
                }

                swap = (MCompanyPackage)arr[currIdx - 1];
            }

            CTable o1 = vw.GetDbObject();
            CTable o2 = swap.GetDbObject();

            String tmp = swap.SeqNo;
            swap.SeqNo = vw.SeqNo;
            vw.SeqNo = tmp;

            vw.SetDbObject(o2);
            swap.SetDbObject(o1);

            vw.updateFlag();                     
            swap.updateFlag();

            vw.NotifyAllPropertiesChanged();
            swap.NotifyAllPropertiesChanged();
        }

        private ObservableCollection<MCompanyPackage> getSelectedItems()
        {
            TabItem ti = (TabItem) tabMain.SelectedItem;
            String name = (String) ti.Tag;

            return (vw.GetItemByName(name));
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MCompanyPackage v = (MCompanyPackage)btn.Tag;

            ReArrangeOrder(getSelectedItems(), v, false);

            vw.IsModified = true;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MCompanyPackage v = (MCompanyPackage)btn.Tag;
            ReArrangeOrder(getSelectedItems(), v, true);

            vw.IsModified = true;
        }

        private void lsvBonusPackage_SizeChanged(object sender, SizeChangedEventArgs e)
        {            
            double w = (lsvBonusPackage.ActualWidth * 1) - 35;
            CUtil.ResizeGridViewColumns(lsvBonusPackage.View as GridView, ratios, w);
        }

        private void lsvDiscountPackage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvDiscountPackage.ActualWidth * 1) - 35;
            CUtil.ResizeGridViewColumns(lsvDiscountPackage.View as GridView, ratios, w);
        }

        private void lsvItemPackage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvItemPackage.ActualWidth * 1) - 35;
            CUtil.ResizeGridViewColumns(lsvItemPackage.View as GridView, ratios, w);
        }

        private void lsvVoucherPackage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvVoucherPackage.ActualWidth * 1) - 35;
            CUtil.ResizeGridViewColumns(lsvVoucherPackage.View as GridView, ratios, w);
        }

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            vw.CompanyID = CompanyID;
            QueryData(vw);
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            this.AddOrEdit_Click("A");
        }

        private void AddOrEdit_Click(String mode)
        {
            TabItem ti = (TabItem)tabMain.SelectedItem;
            String name = (String)ti.Tag;
            String gid = getPackageGroupByTag(name);

            CCriteriaPromotion cr = new CCriteriaPromotion();
            cr.SetActionEnable(false);
            cr.Init(gid);



            WinLookupSearch2 w = new WinLookupSearch2(cr, CLanguage.getValue(name));
            w.ShowDialog();

            MCompanyPackage v = new MCompanyPackage(new CTable(""));

            if (vw != null)
            {                
                vw.PackageGroup = gid;
                v.SetDbObject(vw.GetDbObject().Clone());
            }

            if (w.IsOK)
            {
                vw.Addtems(name, (MCompanyPackage) v, (MPackage) w.ReturnedObj);
                vw.IsModified = true;
            }            
        }

        private string getPackageGroupByTag(String nameTag)
        {
            String packageGroup = "1";

            if (nameTag.Equals("pkg_group_grouping"))
            {
                packageGroup = "1";
            }
            else if (nameTag.Equals("pkg_group_pricing"))
            {
                packageGroup = "2";
            }
            else if (nameTag.Equals("pkg_group_discount"))
            {
                packageGroup = "3";
            }
            else if (nameTag.Equals("pkg_group_final_discount"))
            {
                packageGroup = "4";
            }
            else if (nameTag.Equals("post_gift"))
            {
                packageGroup = "5";
            }
            else if (nameTag.Equals("tray_package_price"))
            {
                packageGroup = "6";
            }
            else if (nameTag.Equals("tray_package_group"))
            {
                packageGroup = "7";
            }
            
            return packageGroup;
        }


        private void cmdTest_Click(object sender, RoutedEventArgs e)
        {
            WinPackageTest w = new WinPackageTest(vw, billSim, CLanguage.getValue("test"));
            w.ShowDialog();
            billSim = w.BillSimulate;
        }

        private void lsvItemPackage_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListView lv = sender as ListView;

            if (lv.SelectedItems.Count == 1)
            {
                currentViewObj = (MCompanyPackage) lv.SelectedItems[0];
                mnuPackageView_Click(null, null);
            }
        }

        private void cmdXXX_Click(object sender, RoutedEventArgs e)
        {
        }

        private void lsvPostGiftPackage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvPostGiftPackage.ActualWidth * 1) - 35;
            CUtil.ResizeGridViewColumns(lsvPostGiftPackage.View as GridView, ratios, w);
        }

        private void lsvTrayPricePackage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvTrayPricePackage.ActualWidth * 1) - 35;
            CUtil.ResizeGridViewColumns(lsvTrayPricePackage.View as GridView, ratios, w);
        }

        private void lsvTrayGroupPackage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvTrayGroupPackage.ActualWidth * 1) - 35;
            CUtil.ResizeGridViewColumns(lsvTrayGroupPackage.View as GridView, ratios, w);
        }
    }
}
