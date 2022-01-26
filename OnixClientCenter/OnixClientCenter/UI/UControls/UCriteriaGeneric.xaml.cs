using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.Client.Helper;
using Onix.Client.Model;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.UControls
{
    public partial class UCriteriaGeneric : UserControl
    {
        private CCriteriaBase criteria = null;

        private CTable lastObjectReturned = null;
        private int rowCount = 0;
        private ObservableCollection<MBaseModel> itemSources = null;
        private String caption = "";
        private FrameworkElement elm = null;

        public UCriteriaGeneric(CCriteriaBase cr, String cption)
        {
            caption = cption;
            criteria = cr;
            criteria.ParentControl = this;
            criteria.ItemAddedEvent = itemAdded_Handler;

            DataContext = criteria.Model;

            InitializeComponent();
            renderCriteriaEntries();
            renderTopCriteriaEntries();
            renderAddButton();

            cmdDelete.IsEnabled = criteria.IsDeleteEnable();
        }

        private void renderAddButton()
        {
            Button btn = criteria.GetButton();
            if (btn != null)
            {
                pnlAddButton.Children.Add(btn);
                btn.IsEnabled = criteria.IsAddEnable();
            }
        }

        private void renderGridColumns()
        {
            double width = lsvMain.ActualWidth - 35;
            GridView gv = new GridView();
            gv.Columns.CollectionChanged += gridView_CollectionChanged;

            ArrayList columns = criteria.GridColumns;
            foreach (CCriteriaColumnBase en in columns)
            {
                en.AddCriteriaColumnPropertyChangedHandler(new PropertyChangedEventHandler(widthChanged));
                double w = (width * en.PctWidth) / 100.00;
                gv.Columns.Add(en.GetGridViewColumn(w));
            }

            lsvMain.View = gv;
        }

        private void renderTopCriteriaEntries()
        {
            int left = 10;
            int width = 200;

            ArrayList entries = criteria.TopCriteriaEntries;
            foreach (CCriteriaEntry en in entries)
            {
                if (en.Type == CriteriaEntryType.ENTRY_LABEL)
                {
                    Label label = new Label();

                    Binding myBinding = new Binding();
                    myBinding.Source = CTextLabel.Instance;
                    myBinding.Path = new PropertyPath(en.CaptionKey);
                    BindingOperations.SetBinding(label, Label.ContentProperty, myBinding);

                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.HorizontalContentAlignment = HorizontalAlignment.Left;
                    label.Margin = new Thickness(left, 0, 0, 0);
                    pnlCriteriaTop.Children.Add(label);
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
                    dt.Margin = new Thickness(left, 0, 0, 0);
                    dt.Width = width;

                    pnlCriteriaTop.Children.Add(dt);
                }
            }
        }

        private void renderCriteriaEntries()
        {
            int left = 10;
            int width = 200;

            ArrayList entries = criteria.CriteriaEntries;
            foreach (CCriteriaEntry en in entries)
            {
                if (en.Type == CriteriaEntryType.ENTRY_LABEL)
                {
                    Label label = new Label();

                    Binding myBinding = new Binding();
                    myBinding.Source = CTextLabel.Instance;
                    myBinding.Path = new PropertyPath(en.CaptionKey);
                    BindingOperations.SetBinding(label, Label.ContentProperty, myBinding);

                    label.VerticalAlignment = VerticalAlignment.Center;
                    label.VerticalContentAlignment = VerticalAlignment.Center;
                    label.HorizontalContentAlignment = HorizontalAlignment.Left;
                    label.Margin = new Thickness(left, 0, 0, 5);
                    pnlCriteria.Children.Add(label);
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

                    if (elm == null)
                    {
                        elm = tbx;
                    }

                    pnlCriteria.Children.Add(tbx);
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

                    if (elm == null)
                    {
                        elm = dt;
                    }

                    pnlCriteria.Children.Add(dt);
                }
                else if (en.Type == CriteriaEntryType.ENTRY_CHECK_BOX)
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
                    cbx.Margin = new Thickness(left, 5, 0, 5);

                    if (elm == null)
                    {
                        elm = cbx;
                    }

                    pnlCriteria.Children.Add(cbx);
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

                    if (elm == null)
                    {
                        elm = cbo;
                    }

                    pnlCriteria.Children.Add(cbo);
                }
            }
        }

        public void SetFirstFocus()
        {
            if (elm != null)
            {
                //elm.Focus();
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void itemAdded_Handler(object sender, RoutedEventArgs e)
        {
            itemSources.Insert(0, (MBaseModel) sender);
            rowCount = rowCount + 1;
            lblTotal.Content = CUtil.FormatInt(rowCount.ToString());
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            rowCount = criteria.DeleteData(rowCount);
            lblTotal.Content = CUtil.FormatInt(rowCount.ToString());
        }

        private void lsvMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lsvMain.SelectedItems.Count == 1)
            {
                MBaseModel m = (MBaseModel)lsvMain.SelectedItems[0];
                criteria.DoubleClickData(m);
            }
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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            CTable tb = criteria.Model.GetDbObject();
            tb.SetFieldValue("EXT_CHUNK_NO", "");
            query(tb);
        }

        private void lsvMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            renderGridColumns();
        }

        private void cmdImport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lsvMain_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = e.OriginalSource as GridViewColumnHeader;
            //String s = "DEBUG";
        }

        static void widthChanged(object sender, PropertyChangedEventArgs e)
        {
            GridViewColumn column = sender as GridViewColumn;
            if (e.PropertyName == "Width")
            {
                //String s = "DEBUG";
            }
        }

        private void gridView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                string msg = string.Format("Column moved from position {0} to position {1}", e.OldStartingIndex, e.NewStartingIndex);
                //MessageBox.Show(msg);
            }
        }

        private void cmdScreenConfig_Click(object sender, RoutedEventArgs e)
        {
            WinScreenConfig wc = new WinScreenConfig(criteria, caption);
            wc.ShowDialog();
        }

        private void lsvMain_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lsvMain_MouseDoubleClick(sender, null);
            }
        }

        private void cmdFormConfig_Click(object sender, RoutedEventArgs e)
        {
            //Forms.PackageLabel.CFormPackageLabel01 rp = new Forms.PackageLabel.CFormPackageLabel01();
            //rp.CreateDefaultFormConfigValues();

            //MFormConfig fc = rp.GetFormConfig();
            //fc.ReportName = "Test Report";
            //fc.InitFormConfig();

            //WinFormConfig wc = new WinFormConfig(fc.ReportName, fc);
            //wc.ShowDialog();
        }
    }
}
