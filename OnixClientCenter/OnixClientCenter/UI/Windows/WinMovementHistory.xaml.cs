using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.ClientCenter.Commons.UControls;
using System.Collections;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinMovementHistory : Window
    {
        private CCriteriaBase criteria = null;
        private String caption = "";
        private CTable lastObjectReturned = null;
        private int rowCount = 0;
        private ObservableCollection<MBaseModel> itemSources = null;

        private MBaseModel prev = null;
        private MBaseModel curr = null;

        public int selectedCount = 0;
        public Boolean IsOK = false;
        public MBaseModel ReturnedObj = null;

        public WinMovementHistory(CCriteriaBase cr, String ction)
        {
            criteria = cr;
            criteria.SetCheckUncheckHandler(cbxSelect_Checked, cbxSelect_Unchecked);
            caption = ction;

            DataContext = criteria.Model;
            InitializeComponent();
            renderInfoEntries();
            renderCriteriaEntries();
        }

        public String WindowTitle
        {
            get
            {
                return (caption);
            }
        }

        public String TabItemCaption
        {
            get
            {
                return (caption);
            }
        }

        private Label createLabel(CCriteriaEntry en, double l, double t, double r, double b)
        {
            Label label = new Label();

            Binding myBinding = new Binding();
            myBinding.Source = CTextLabel.Instance;
            myBinding.Path = new PropertyPath(en.CaptionKey);
            BindingOperations.SetBinding(label, Label.ContentProperty, myBinding);

            label.VerticalAlignment = VerticalAlignment.Center;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.HorizontalContentAlignment = HorizontalAlignment.Right;
            label.Margin = new Thickness(l, t, r, b);
            label.IsEnabled = en.IsEnable;

            return (label);
        }

        private TextBox createTextBox(CCriteriaEntry en, double l, double t, double r, double b, double w)
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
            tbx.Margin = new Thickness(l, t, r, b);
            tbx.Width = w;
            tbx.IsEnabled = en.IsEnable;

            return (tbx);
        }

        private UDatePicker createDatePicker(CCriteriaEntry en, double l, double t, double r, double b, double w)
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
            dt.Margin = new Thickness(l, 0, 0, 0);
            dt.Width = w;
            dt.IsEnabled = en.IsEnable;

            return (dt);
        }

        private CheckBox createCheckBox(CCriteriaEntry en, double l, double t, double r, double b, double w)
        {
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
            cbx.Margin = new Thickness(l, t, r, b);
            cbx.IsEnabled = en.IsEnable;

            return (cbx);
        }

        private ComboBox createComboBox(CCriteriaEntry en, double l, double t, double r, double b, double w)
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
            cbo.Margin = new Thickness(l, t, r, b);
            cbo.Width = w;
            cbo.IsEnabled = en.IsEnable;

            return (cbo);
        }

        private void renderCriteriaEntries()
        {
            //pnlCriteria
            int left = 5;
            int width = 200;

            ArrayList entries = criteria.CriteriaEntries;
            foreach (CCriteriaEntry en in entries)
            {
                if (en.Type == CriteriaEntryType.ENTRY_LABEL)
                {
                    Label label = createLabel(en, left, 0, 0, 0);
                    pnlCriteria.Children.Add(label);
                }
                else if (en.Type == CriteriaEntryType.ENTRY_TEXT_BOX)
                {
                    TextBox tbx = createTextBox(en, left, 0, 0, 0, width);
                    pnlCriteria.Children.Add(tbx);
                }
                else if ((en.Type == CriteriaEntryType.ENTRY_DATE_MIN) || (en.Type == CriteriaEntryType.ENTRY_DATE_MAX))
                {
                    UDatePicker dt = createDatePicker(en, left, 0, 0, 0, width);
                    pnlCriteria.Children.Add(dt);
                }
                else if (en.Type == CriteriaEntryType.ENTRY_CHECK_BOX)
                {
                    CheckBox cbx = createCheckBox(en, left, 0, 0, 0, width);
                    pnlCriteria.Children.Add(cbx);
                }
                else if (en.Type == CriteriaEntryType.ENTRY_COMBO_BOX)
                {
                    ComboBox cbo = createComboBox(en, left, 0, 0, 0, width);
                    pnlCriteria.Children.Add(cbo);
                }
            }
        }

        private void renderInfoEntries()
        {
            ColumnDefinition cd1 = new ColumnDefinition();
            cd1.Width = new GridLength(15, GridUnitType.Auto);
            grdInfo.ColumnDefinitions.Add(cd1);

            ColumnDefinition cd2 = new ColumnDefinition();
            cd2.Width = new GridLength(65, GridUnitType.Star);
            grdInfo.ColumnDefinitions.Add(cd2);

            ColumnDefinition cd3 = new ColumnDefinition();
            cd3.Width = new GridLength(20, GridUnitType.Star);
            grdInfo.ColumnDefinitions.Add(cd3);

            int left = 10;
            int width = 400;
            int row = 0; //Count row by label

            Boolean labelCreated = false;

            ArrayList entries = criteria.InfoEntries;
            foreach (CCriteriaEntry en in entries)
            {
                if (en.Type == CriteriaEntryType.ENTRY_LABEL)
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = new GridLength(45, GridUnitType.Auto);
                    grdInfo.RowDefinitions.Add(rd);

                    Label label = createLabel(en, left, 0, 0, 5);

                    Grid.SetColumn(label, 0);
                    Grid.SetRow(label, row);
                    grdInfo.Children.Add(label);

                    labelCreated = true;
                }
                else if (en.Type == CriteriaEntryType.ENTRY_TEXT_BOX)
                {
                    TextBox tbx = createTextBox(en, left, 0, 0, 5, width);

                    Grid.SetColumn(tbx, 1);
                    Grid.SetRow(tbx, row);
                    grdInfo.Children.Add(tbx);
                }
                else if ((en.Type == CriteriaEntryType.ENTRY_DATE_MIN) || (en.Type == CriteriaEntryType.ENTRY_DATE_MAX))
                {
                    UDatePicker dt = createDatePicker(en, left, 0, 0, 5, width);

                    Grid.SetColumn(dt, 1);
                    Grid.SetRow(dt, row);
                    grdInfo.Children.Add(dt);
                }
                else if (en.Type == CriteriaEntryType.ENTRY_CHECK_BOX)
                {
                    if (!labelCreated)
                    {
                        //Create grid row, Check box has no associate label
                        RowDefinition rd = new RowDefinition();
                        rd.Height = new GridLength(45, GridUnitType.Auto);
                        grdInfo.RowDefinitions.Add(rd);
                    }

                    CheckBox cbx = createCheckBox(en, left, 5, 0, 5, width);

                    Grid.SetColumn(cbx, 1);
                    Grid.SetRow(cbx, row);
                    grdInfo.Children.Add(cbx);
                }
                else if (en.Type == CriteriaEntryType.ENTRY_COMBO_BOX)
                {
                    ComboBox cbo = createComboBox(en, left, 0, 0, 5, width);

                    Grid.SetColumn(cbo, 1);
                    Grid.SetRow(cbo, row);
                    grdInfo.Children.Add(cbo);
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
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.ContextMenu.IsOpen = true;
        }

        private void renderGridColumns()
        {
            double width = lsvMovement.ActualWidth - 35;
            GridView gv = new GridView();

            ArrayList columns = criteria.GridColumns;
            foreach (CCriteriaColumnBase en in columns)
            {
                double w = (width * en.PctWidth) / 100.00;
                gv.Columns.Add(en.GetGridViewColumn(w));
            }

            lsvMovement.View = gv;
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
            //if (forSelected)
            //{
            if (selectedCount <= 0)
            {
                String str = CLanguage.getValue("ERROR_NO_SELECTED");

                CMessageBox.Show(str, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ReturnedObj = curr;
            //}

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

        private void lsvMovement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderGridColumns();
        }

        private void query(CTable tb)
        {
            CUtil.EnableForm(false, this);
            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = criteria.QueryData();
            CUtil.EnableForm(true, this);

            itemSources = tuple.Item2;
            lastObjectReturned = tuple.Item1;

            lsvMovement.ItemsSource = itemSources;

            rowCount = CUtil.StringToInt(lastObjectReturned.GetFieldValue("EXT_RECORD_COUNT"));
            lblTotal.Content = CUtil.FormatInt(rowCount.ToString());
            CUtil.LoadChunkNavigateCombo(cboNavigate, lastObjectReturned, tb.GetFieldValue("EXT_CHUNK_NO"));
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void rootElement_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

    }
}
