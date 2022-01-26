using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.UControls;
using System.Collections.ObjectModel;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditQuotationItemComplex : Window
    {
        private MAuxilaryDocItem vw = null;
        private MAuxilaryDocItem actualView = null;
        private MAuxilaryDoc parentView = null;

        public String Caption = "";
        public String Mode = "";
        public Boolean HasModified = false;

        public MAuxilaryDocItem ViewData
        {
            set
            {
                actualView = value;
            }
        }

        public MAuxilaryDoc ParentView
        {
            set
            {
                parentView = value;
            }
        }

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
                GridLength l = new GridLength(42, GridUnitType.Star);
                return (l);
            }
        }

        public GridLength Column2Width
        {
            get
            {
                GridLength l = new GridLength(8, GridUnitType.Star);
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

        public GridLength Column7Width
        {
            get
            {
                GridLength l = new GridLength(5, GridUnitType.Star);
                return (l);
            }
        }

        private void AddHotKeys()
        {
            RoutedCommand firstSettings = new RoutedCommand();
            firstSettings.InputGestures.Add(new KeyGesture(Key.Add, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(firstSettings, mnuAddHotKey_Click));
        }

        public WinAddEditQuotationItemComplex()
        {
            InitializeComponent();
            AddHotKeys();
        }

        private void mnuAddHotKey_Click(object sender, RoutedEventArgs e)
        {
            int idx = vw.ItemDetails.Count;

            MAuxilaryDocSubItem mi = new MAuxilaryDocSubItem(new CTable(""));

            vw.InsertItemDetail(idx, mi);
            vw.CalculateSubItemTotal();

            vw.IsModified = true;
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private LookupSearchType2 selectionTypeToLookup(String selType)
        {
            LookupSearchType2 lkup;

            if (selType.Equals("1"))
            {
                lkup = LookupSearchType2.ServiceRegularSaleLookup;
            }
            else
            {
                lkup = LookupSearchType2.InventoryItemLookup;
            }

            return (lkup);
        }

        private void LoadData()
        {
            //isInLoad = true;
            this.Title = Caption;

            CTable t = new CTable("");
            vw = new MAuxilaryDocItem(t);
            vw.CreateDefaultValue();

            DataContext = vw;
            vw.SelectType = "3";

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
            //isInLoad = false;
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
            Boolean result = CHelper.ValidateTextBox(lblDesc, txtDesc, false);
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
                    parentView.AddAuxilaryDocItem(vw);
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
    }
}
