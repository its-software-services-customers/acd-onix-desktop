using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using System.Collections.ObjectModel;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditMiscItemComplex : Window
    {
        private MAccountDocItem vw = null;
        private MAccountDocItem actualView = null;
        private MAccountDoc parentView = null;

        public String Caption = "";
        public String Mode = "";
        public Boolean HasModified = false;

        public MAccountDocItem ViewData
        {
            set
            {
                actualView = value;
            }
        }

        public MAccountDoc ParentView
        {
            set
            {
                parentView = value;
            }
        }

        #region Columns
        public GridLength Column0Width
        {
            get
            {
                GridLength l = new GridLength(5, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column1Width
        {
            get
            {
                GridLength l = new GridLength(13, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column2Width
        {
            get
            {
                GridLength l = new GridLength(42, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column3Width
        {
            get
            {
                GridLength l = new GridLength(10, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column4Width
        {
            get
            {
                GridLength l = new GridLength(10, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column5Width
        {
            get
            {
                GridLength l = new GridLength(10, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column6Width
        {
            get
            {
                GridLength l = new GridLength(10, GridUnitType.Star);
                return (l);
            }
        }
        #endregion

        public WinAddEditMiscItemComplex()
        {
            InitializeComponent();
            AddHotKeys();
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void LoadData()
        {
            this.Title = Caption;

            CTable t = new CTable("");
            vw = new MAccountDocItem(t);
            vw.CreateDefaultValue();

            DataContext = vw;
            vw.SelectType = "1";

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable newDB = actualView.GetDbObject().Clone();
                vw.SetDbObject(newDB);
                vw.InitItemDetails();
            }
            else
            {
                for (int i = 1; i <= 1; i++)
                {
                    MAuxilaryDocSubItem mi = new MAuxilaryDocSubItem(new CTable(""));
                    vw.AddItemDetail(mi);
                }
            }

            vw.NotifyAllPropertiesChanged();

            vw.IsModified = false;
            CUtil.EnableForm(true, this);
        }

        private Boolean validateQuotationItems<T>(ObservableCollection<T> collection, Boolean chkCnt) where T : MBaseModel
        {
            int idx = 0;
            int cnt = 0;
            foreach (MBaseModel c in collection)
            {
                idx++;

                if (c.IsEmpty)
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    return (false);
                }

                cnt++;
            }

            if ((cnt <= 0) && chkCnt)
            {
                CHelper.ShowErorMessage(idx.ToString(), "ERROR_ITEM_COUNT", null);
                return (false);
            }

            return (true);
        }

        private Boolean ValidateData()
        {
            Boolean result = CHelper.ValidateLookup(lblDesc, lkup, false);
            if (!result)
            {
                return (result);
            }

            result = validateQuotationItems(vw.ItemDetails, true);
            if (!result)
            {
                return (result);
            }

            return (true);
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveData();
            if (r)
            {
                HasModified = true;

                vw.IsModified = false;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private Boolean SaveData()
        {
            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    vw.SerializeItemDetails();
                    parentView.AddAccountDocItem(vw);
                    return (true);
                }

                return (false);
            }
            else if (Mode.Equals("E"))
            {
                if (vw.IsModified)
                {
                    Boolean result = SaveToView();
                    if (result)
                    {
                        vw.SerializeItemDetails();
                        CTable o = actualView.GetDbObject();
                        o.CopyFrom(vw.GetDbObject());

                        actualView.NotifyAllPropertiesChanged();

                        return (true);
                    }

                    return (false);
                }
            }

            return (true);
        }

        private Boolean SaveToView()
        {
            if (!ValidateData())
            {
                return (false);
            }

            return (true);
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.CalculateSubItemTotal();
            vw.IsModified = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void cbxAllow_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxAllow_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void rootElement_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rootElement_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    Boolean r = SaveData();
                    if (r)
                    {
                        HasModified = true;
                        CUtil.EnableForm(true, this);
                    }
                }
            }
        }

        private void rootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void constructMenuItems(Button btn)
        {
            MAuxilaryDocSubItem adi = (MAuxilaryDocSubItem)btn.Tag;
            ContextMenu btnContext = new ContextMenu();

            MenuItem mi1 = new MenuItem();
            mi1.Name = "mnuAdd";
            mi1.Tag = adi;
            mi1.Click += mnuAdd_Click;
            mi1.Header = CLanguage.getValue("add");

            MenuItem mi2 = new MenuItem();
            mi2.Name = "mnuDelete";
            mi2.Tag = adi;
            mi2.Click += mnuDelete_Click;
            mi2.Header = CLanguage.getValue("delete");

            btnContext.Items.Add(mi1);
            btnContext.Items.Add(new Separator());
            btnContext.Items.Add(mi2);

            btn.ContextMenu = btnContext;
        }

        private void cmdOption_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button) sender;            

            constructMenuItems(btn);
            btn.ContextMenu.IsOpen = true;
        }

        private void AddHotKeys()
        {
            RoutedCommand firstSettings = new RoutedCommand();
            firstSettings.InputGestures.Add(new KeyGesture(Key.Add, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(firstSettings, mnuAddHotKey_Click));
        }

        private void mnuAddHotKey_Click(object sender, RoutedEventArgs e)
        {
            int idx = vw.ItemDetails.Count;

            MAuxilaryDocSubItem mi = new MAuxilaryDocSubItem(new CTable(""));

            vw.InsertItemDetail(idx, mi);
            vw.CalculateSubItemTotal();

            vw.IsModified = true;
        }

        private void mnuAdd_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mni = (MenuItem)sender;
            MAuxilaryDocSubItem itm = (MAuxilaryDocSubItem)mni.Tag;
            int idx = CUtil.StringToInt(itm.Index);

            MAuxilaryDocSubItem mi = new MAuxilaryDocSubItem(new CTable(""));

            vw.InsertItemDetail(idx, mi);
            vw.CalculateSubItemTotal();

            vw.IsModified = true;
        }

        private void mnuDelete_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mni = (MenuItem)sender;
            MAuxilaryDocSubItem itm = (MAuxilaryDocSubItem)mni.Tag;

            if (vw.ItemDetails.Count <= 1)
            {
                CHelper.ShowErorMessage("", "ERROR_MUST_KEEP", null);
                return;
            }

            Boolean result = CHelper.AskConfirmMessage(itm.Index, "CONFIRM_DELETE_DATA");
            if (!result)
            {
                return;
            }

            vw.RemoveItemDetail(itm);
            vw.CalculateSubItemTotal();

            vw.IsModified = true;
        }

        private void txtDesc_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void dtFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void lkup_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void uProject_SelectedObjectChanged(object sender, EventArgs e)
        {

        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            
            string[] lines = System.IO.File.ReadAllLines(@"D:\tollfree.txt");
            int cnt = lines.Length;

            for (int i = 0; i < cnt; i++)
            {
                if (i == 0)
                {
                    //Comment line
                    continue;
                }

                String line = lines[i];
                MAuxilaryDocSubItem mi = new MAuxilaryDocSubItem(new CTable(""));

                string[] fields = line.Split('|');
                String date = fields[0];
                String desc = fields[1];
                String invoice = fields[2];
                String amtBeforeVat = fields[5];
                String vat = fields[6];
                String amtAfterVat = fields[7];

                string[] dateComponents = date.Split('/');

                int year = 2000 + CUtil.StringToInt(dateComponents[2]);
                int month = CUtil.StringToInt(dateComponents[0]);
                int day = CUtil.StringToInt(dateComponents[1]);

                mi.SubItemDate = new DateTime(year, month, day);
                mi.UnitPrice = amtAfterVat; //(CUtil.StringToDouble(vat) + CUtil.StringToDouble(amtAfterVat)).ToString();
                mi.Quantity = "1";                
                mi.Description = "ค่าทางด่วน" + " (" + invoice + ")";
                mi.ExtFlag = "A";

                vw.AddItemDetail(mi);
            }

            vw.IsModified = true;
        }
    }
}
