using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.ClientCenter.UControls;
using System.Collections;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter
{
    public partial class WinLookupSearch1 : Window
    {
        private CCriteriaBase criteria = null;
        private String caption = "";
        private CTable lastObjectReturned = null;
        private int rowCount = 0;
        private ObservableCollection<MBaseModel> itemSources = null;
        private Boolean forSelected = false;

        private MBaseModel prev = null;
        private MBaseModel curr = null;

        public int selectedCount = 0;
        public Boolean IsOK = false;
        public MBaseModel ReturnedObj = null;

        public WinLookupSearch1(CCriteriaBase cr, String ction)
        {
            criteria = cr;
            criteria.SetCheckUncheckHandler(cbxSelect_Checked, cbxSelect_Unchecked);
            caption = ction;

            DataContext = criteria.Model;
            InitializeComponent();
            renderCriteriaEntries();
        }

        public String WindowTitle
        {
            get
            {
                return (CLanguage.getValue("search") + " " + caption);
            }
        }

        public String TabItemCaption
        {
            get
            {
                return (caption);
            }
        }

        private Grid getGridReference(int logicalRow)
        {
            if ((logicalRow % 2) == 0)
            {
                return (grdMain1);
            }

            return (grdMain2);
        }

        private int logicalRowToRow(int logicalRow)
        {
            int d = logicalRow / 2;
            int r = logicalRow % 2;

            return (d);
        }

        private void renderCriteriaEntries()
        {
            ColumnDefinition cd1 = new ColumnDefinition();
            cd1.Width = new GridLength(15, GridUnitType.Auto);
            grdMain1.ColumnDefinitions.Add(cd1);

            ColumnDefinition cd1_2 = new ColumnDefinition();
            cd1_2.Width = new GridLength(15, GridUnitType.Auto);
            grdMain2.ColumnDefinitions.Add(cd1_2);

            ColumnDefinition cd2 = new ColumnDefinition();
            cd2.Width = new GridLength(65, GridUnitType.Star);
            grdMain1.ColumnDefinitions.Add(cd2);

            ColumnDefinition cd2_2 = new ColumnDefinition();
            cd2_2.Width = new GridLength(65, GridUnitType.Star);
            grdMain2.ColumnDefinitions.Add(cd2_2);

            ColumnDefinition cd3 = new ColumnDefinition();
            cd3.Width = new GridLength(20, GridUnitType.Star);
            grdMain1.ColumnDefinitions.Add(cd3);

            ColumnDefinition cd3_1 = new ColumnDefinition();
            cd3_1.Width = new GridLength(20, GridUnitType.Star);
            grdMain2.ColumnDefinitions.Add(cd3_1);

            int left = 10;
            int width = 300;
            int row = 0; //Count row by label

            Boolean labelCreated = false;

            ArrayList entries = criteria.CriteriaEntries;
            foreach (CCriteriaEntry en in entries)
            {
                if (en.Type == CriteriaEntryType.ENTRY_LABEL)
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = new GridLength(45, GridUnitType.Auto);
                    getGridReference(row).RowDefinitions.Add(rd);

                    Label label = new Label();

                    Binding myBinding = new Binding();
                    myBinding.Source = CTextLabel.Instance;
                    myBinding.Path = new PropertyPath(en.CaptionKey);
                    BindingOperations.SetBinding(label, Label.ContentProperty, myBinding);

                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.HorizontalContentAlignment = HorizontalAlignment.Right;
                    label.Margin = new Thickness(left, 0, 0, 5);

                    Grid.SetColumn(label, 0);
                    Grid.SetRow(label, logicalRowToRow(row));
                    getGridReference(row).Children.Add(label);

                    labelCreated = true;
                }
                else if (en.Type == CriteriaEntryType.ENTRY_TEXT_BOX)
                {
                    TextBox tbx = new TextBox();

                    Binding myBinding = new Binding();
                    myBinding.Source = DataContext;
                    myBinding.Path = new PropertyPath(en.FieldName);
                    myBinding.Mode = BindingMode.TwoWay;
                    myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    BindingOperations.SetBinding(tbx, TextBox.TextProperty, myBinding);

                    tbx.HorizontalAlignment = HorizontalAlignment.Left;
                    tbx.VerticalAlignment = VerticalAlignment.Center;
                    tbx.VerticalContentAlignment = VerticalAlignment.Center;
                    tbx.Margin = new Thickness(left, 0, 0, 5);
                    tbx.Width = width;

                    Grid.SetColumn(tbx, 1);
                    Grid.SetRow(tbx, logicalRowToRow(row));
                    getGridReference(row).Children.Add(tbx);
                }
                else if ((en.Type == CriteriaEntryType.ENTRY_DATE_MIN) || (en.Type == CriteriaEntryType.ENTRY_DATE_MAX))
                {
                    UDatePicker dt = new UDatePicker();

                    Binding myBinding = new Binding();
                    myBinding.Source = DataContext;
                    myBinding.Path = new PropertyPath(en.FieldName);
                    myBinding.Mode = BindingMode.TwoWay;
                    myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    BindingOperations.SetBinding(dt, UDatePicker.SelectedDateProperty, myBinding);

                    dt.HorizontalAlignment = HorizontalAlignment.Left;
                    dt.VerticalAlignment = VerticalAlignment.Center;
                    dt.VerticalContentAlignment = VerticalAlignment.Center;
                    dt.Margin = new Thickness(left, 0, 0, 5);
                    dt.Width = width;

                    Grid.SetColumn(dt, 1);
                    Grid.SetRow(dt, logicalRowToRow(row));
                    getGridReference(row).Children.Add(dt);
                }
                else if (en.Type == CriteriaEntryType.ENTRY_CHECK_BOX)
                {
                    if (!labelCreated)
                    {
                        //Create grid row, Check box has no associate label
                        RowDefinition rd = new RowDefinition();
                        rd.Height = new GridLength(45, GridUnitType.Auto);
                        getGridReference(row).RowDefinitions.Add(rd);
                    }

                    CheckBox cbx = new CheckBox();
                    cbx.IsThreeState = true;

                    Binding ctntBinding = new Binding();
                    ctntBinding.Source = CTextLabel.Instance;
                    ctntBinding.Path = new PropertyPath(en.CaptionKey);
                    BindingOperations.SetBinding(cbx, CheckBox.ContentProperty, ctntBinding);

                    Binding valueBinding = new Binding();
                    valueBinding.Source = DataContext;
                    valueBinding.Path = new PropertyPath(en.FieldName);
                    BindingOperations.SetBinding(cbx, CheckBox.IsCheckedProperty, valueBinding);

                    cbx.HorizontalAlignment = HorizontalAlignment.Left;
                    cbx.VerticalAlignment = VerticalAlignment.Center;
                    cbx.VerticalContentAlignment = VerticalAlignment.Center;
                    cbx.Margin = new Thickness(left, 5, 0, 5);

                    Grid.SetColumn(cbx, 1);
                    Grid.SetRow(cbx, logicalRowToRow(row));
                    getGridReference(row).Children.Add(cbx);
                }
                else if (en.Type == CriteriaEntryType.ENTRY_COMBO_BOX)
                {
                    ComboBox cbo = new ComboBox();

                    Binding ctntBinding = new Binding();
                    ctntBinding.Source = CMasterReference.Instance;
                    ctntBinding.Path = new PropertyPath(en.ComboCollectionName);
                    BindingOperations.SetBinding(cbo, ComboBox.ItemsSourceProperty, ctntBinding);

                    cbo.DisplayMemberPath = en.ComboDisplayItem;
                    cbo.SelectedValuePath = "ObjSelf";

                    Binding selectedValueBinding = new Binding();
                    selectedValueBinding.Path = new PropertyPath(en.FieldName);
                    BindingOperations.SetBinding(cbo, ComboBox.SelectedValueProperty, selectedValueBinding);

                    cbo.HorizontalAlignment = HorizontalAlignment.Left;
                    cbo.VerticalAlignment = VerticalAlignment.Center;
                    cbo.VerticalContentAlignment = VerticalAlignment.Center;
                    cbo.Margin = new Thickness(left, 0, 0, 5);
                    cbo.Width = width;

                    Grid.SetColumn(cbo, 1);
                    Grid.SetRow(cbo, logicalRowToRow(row));
                    getGridReference(row).Children.Add(cbo);
                }

                if (en.Type != CriteriaEntryType.ENTRY_LABEL)
                {
                    row++;
                    labelCreated = false;
                }
            }
        }

        private void resizeGridViewColumns(GridView grdv, double[] ratios, double w)
        {
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            CUtil.EnableForm(false, this);
            criteria.LoadScreenConfig();
            CUtil.EnableForm(true, this);
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.ContextMenu.IsOpen = true;
        }

        private void renderGridColumns()
        {
            double width = lsvMain.ActualWidth - 35;
            GridView gv = new GridView();

            ArrayList columns = criteria.GridColumns;
            foreach (CCriteriaColumnBase en in columns)
            {
                double w = (width * en.PctWidth) / 100.00;
                gv.Columns.Add(en.GetGridViewColumn(w));
            }

            lsvMain.View = gv;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            CTable tb = criteria.Model.GetDbObject();
            tb.SetFieldValue("EXT_CHUNK_NO", "");
            query(tb);
        }

        private void cmdNavigate_Click(object sender, RoutedEventArgs e)
        {
            CTable tb = criteria.Model.GetDbObject();

            int idx = cboNavigate.SelectedIndex;
            if (idx < 0)
            {
                return;
            }

            MChunkNavigate v = (MChunkNavigate)cboNavigate.SelectedItem;
            tb.SetFieldValue("EXT_CHUNK_NO", v.ChunkNo);
            query(tb);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (forSelected)
            {
                if (selectedCount <= 0)
                {
                    String str = CLanguage.getValue("ERROR_NO_SELECTED");

                    CMessageBox.Show(str, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ReturnedObj = curr;
            }

            IsOK = true;          
            this.Close();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbxSelect_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cbx = (CheckBox)sender;
            MBaseModel cv = (MBaseModel)cbx.Tag;

            prev = curr;
            curr = cv;
            if (prev != null)
            {
                prev.IsSelectedForDelete = false;
            }

            selectedCount++;
        }

        private void cbxSelect_Unchecked(object sender, RoutedEventArgs e)
        {
            selectedCount--;
        }

        private void lsvMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderGridColumns();
        }

        private void query(CTable tb)
        {
            CUtil.EnableForm(false, this);

            MBaseModel md = criteria.GetModel();
            CTable t = md.GetDbObject();
            criteria.PopulateQuerySortSetting(t);

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = criteria.QueryData();
            CUtil.EnableForm(true, this);

            itemSources = tuple.Item2;
            lastObjectReturned = tuple.Item1;

            lsvMain.ItemsSource = itemSources;

            rowCount = CUtil.StringToInt(lastObjectReturned.GetFieldValue("EXT_RECORD_COUNT"));
            lblTotal.Content = CUtil.FormatInt(rowCount.ToString());
            CUtil.LoadChunkNavigateCombo(cboNavigate, lastObjectReturned, tb.GetFieldValue("EXT_CHUNK_NO"));
        }

        private void cmdScreenConfig_Click(object sender, RoutedEventArgs e)
        {
            WinScreenConfig wc = new WinScreenConfig(criteria, caption);
            wc.ShowDialog();
        }
    }
}
