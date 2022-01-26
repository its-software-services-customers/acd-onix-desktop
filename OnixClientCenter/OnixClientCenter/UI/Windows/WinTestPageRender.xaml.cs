using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.ClientCenter.Commons.Utils;
using System.Collections;
using Onix.Client.Helper;
using Onix.Client.Model;

namespace Onix.ClientCenter
{
    public partial class WinTestPageRender : Window
    {
        private CCriteriaBase criteria = null;
        private String caption = "";
        private MScreenConfig vw = new MScreenConfig(new CTable(""));        

        public int selectedCount = 0;
        public Boolean IsOK = false;
        public MBaseModel ReturnedObj = null;

        public WinTestPageRender(CCriteriaBase cr, String configName)
        {
            criteria = cr;
            caption = configName;            
            InitializeComponent();
        }

        public String WindowTitle
        {
            get
            {
                return (CLanguage.getValue("sorting_config") + " " + caption);
            }
        }

        public String TabItemCaption
        {
            get
            {
                return (caption);
            }
        }

        private void resizeGridViewColumns(GridView grdv, double[] ratios, double w)
        {
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            CUtil.EnableForm(false, this);

            vw.InitSortingColumns(criteria.GetSortableColumns());
            DataContext = vw;
            vw.NotifyAllPropertiesChanged();

            vw.IsModified = false;
            CUtil.EnableForm(true, this);
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
            if (!vw.IsModified)
            {
                IsOK = true;
                this.Close();
                return;
            }

            Boolean r = SaveData();
            if (r)
            {
                IsOK = true;

                vw.IsModified = false;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReArrangeOrder(ObservableCollection<MBaseModel> items, MCriteriaColumn vw, Boolean isDown)
        {
            ArrayList arr = new ArrayList();

            int currIdx = -1;
            int min = 99999999;
            int max = 0;
            int idx = 0;
            foreach (MCriteriaColumn v in items)
            {
                int seq = int.Parse(v.ItemSeqNo);
                if (seq < min)
                {
                    min = seq;
                }

                if (seq > max)
                {
                    max = seq;
                }

                if (v.SeqNo.Equals(vw.ItemSeqNo))
                {
                    currIdx = idx;
                }

                arr.Add(v);
                idx++;
            }

            int cnt = idx++;
            MCriteriaColumn swap = null;

            if (isDown)
            {
                if (currIdx >= cnt - 1)
                {
                    //Do nothing, this is the last item in the rows
                    return;
                }

                swap = (MCriteriaColumn)arr[currIdx + 1];
            }
            else
            {
                //Up
                if (currIdx <= 0)
                {
                    //Do nothing, this is the first item 
                    return;
                }

                swap = (MCriteriaColumn)arr[currIdx - 1];
            }

            CTable o1 = vw.GetDbObject();
            CTable o2 = swap.GetDbObject();

            String tmp = swap.ItemSeqNo;
            swap.ItemSeqNo = vw.ItemSeqNo;
            vw.ItemSeqNo = tmp;

            vw.SetDbObject(o2);
            swap.SetDbObject(o1);

            vw.updateFlag();
            swap.updateFlag();

            vw.NotifyAllPropertiesChanged();
            swap.NotifyAllPropertiesChanged();
        }

        private void lsvMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //double w = (lsvMain.ActualWidth * 1) - 35;
            //double[] ratios = new double[4] { 0.1, 0.5, 0.30, 0.1 };
            //CUtil.ResizeGridViewColumns(lsvMain.View as GridView, ratios, w);
        }

        private void cbxUse_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            MCriteriaColumn o = (MCriteriaColumn) (sender as Button).Tag;
            ReArrangeOrder(vw.SortingColumns, o, false);
            vw.IsModified = true;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            MCriteriaColumn o = (MCriteriaColumn)(sender as Button).Tag;
            ReArrangeOrder(vw.SortingColumns, o, true);
            vw.IsModified = true;
        }

        private void radAsc_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private Boolean SaveData()
        {
            CUtil.EnableForm(false, this);
            criteria.SaveCriteriaConfig(vw);
            CUtil.EnableForm(true, this);
            return (true);
        }

        private void rootElement_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vw.IsModified)
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
    }
}
