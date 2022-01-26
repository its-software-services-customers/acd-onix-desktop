using System;
using System.Windows;
using System.Threading;
using System.Collections;
using System.Windows.Controls;
using Wis.WsClientAPI;
using Onix.ClientCenter.UControls;
using Onix.Client.Helper;
using Onix.Client.Report;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Reports;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.Windows
{
    public partial class WinReportParam : Window
    {
        private MMasterRef dat = null;
        private CBaseReport paginator = null;
        MReportConfig rptCfg = null;
        private String xml = "";
        private Boolean isDone = false;
        private CThreadSync tc = null;

        private ArrayList entries = null;

        private void constructFieldEntry()
        {
            entries = paginator.GetReportInputEntries();
        }

        private void constructUI()
        {
            String value = "";

            foreach (CEntry en in entries)
            {
                Label label = new Label();
                label.Height = en.EntryHeight;
                label.Content = CLanguage.getValue(en.LabelContentKey);
                label.VerticalAlignment = VerticalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.HorizontalContentAlignment = HorizontalAlignment.Right;
                label.Margin = new Thickness(20, 0, 0, 5);

                pnlLabel.Children.Add(label);

                FrameworkElement ctrl = new TextBlock();
                if ((en.EntryType == EntryType.ENTRY_DATE_MIN) || (en.EntryType == EntryType.ENTRY_DATE_MAX))
                {
                    ctrl = new UDatePicker();
                    ctrl.HorizontalAlignment = HorizontalAlignment.Left;
                    ctrl.VerticalAlignment = VerticalAlignment.Center;

                    value = rptCfg.GetConfigValue(en.FieldName, "");
                    if (!value.Equals(""))
                    {
                        (ctrl as UDatePicker).SelectedDate = CUtil.InternalDateToDate(value);
                    }                       
                }
                else if (en.EntryType == EntryType.ENTRY_MONTH_YEAR)
                {
                    ctrl = new UDateEntry();
                    (ctrl as UDateEntry).ButtonVisible = true;
                    (ctrl as UDateEntry).OnlyMonth = true;
                    ctrl.HorizontalAlignment = HorizontalAlignment.Left;
                    ctrl.VerticalAlignment = VerticalAlignment.Center;

                    value = rptCfg.GetConfigValue(en.FieldName, "");
                    if (!value.Equals(""))
                    {
                        (ctrl as UDateEntry).SelectedDate = CUtil.InternalDateToDate(value);
                    }
                    else
                    {
                        (ctrl as UDateEntry).SelectedDate = DateTime.Now;
                    }
                }
                else if (en.EntryType == EntryType.ENTRY_TEXT_BOX)
                {
                    ctrl = new TextBox();
                    ctrl.HorizontalAlignment = HorizontalAlignment.Left;
                    ctrl.VerticalAlignment = VerticalAlignment.Center;                    
                    (ctrl as TextBox).VerticalContentAlignment = VerticalAlignment.Center;

                    value = rptCfg.GetConfigValue(en.FieldName, "");
                    (ctrl as TextBox).Text = value;
                }
                else if (en.EntryType == EntryType.ENTRY_CHECK_BOX)
                {
                    ctrl = new CheckBox();
                    ctrl.HorizontalAlignment = HorizontalAlignment.Left;
                    ctrl.VerticalAlignment = VerticalAlignment.Center;
                    (ctrl as CheckBox).VerticalContentAlignment = VerticalAlignment.Top;
                    (ctrl as CheckBox).Content = label.Content; //เอาค่าจาก label มาแสดงที่ checkbox แทน

                    label.Content = "";
                    value = rptCfg.GetConfigValue(en.FieldName, "");
                    (ctrl as CheckBox).IsChecked = flagToBoolean(value);
                }
                else if (en.EntryType == EntryType.ENTRY_COMBO_BOX)
                {
                    ctrl = new ComboBox();
                    ctrl.HorizontalAlignment = HorizontalAlignment.Left;
                    ctrl.VerticalAlignment = VerticalAlignment.Center;
                    (ctrl as ComboBox).VerticalContentAlignment = VerticalAlignment.Center;

                    value = rptCfg.GetConfigValue(en.FieldName, "");

                    en.ComboSetupFunction((ctrl as ComboBox));
                    en.ComboLoaderFunction((ctrl as ComboBox), value);
                }

                ctrl.Margin = new Thickness(5, 0, 0, 5);
                ctrl.Width = en.EntryWidth;
                ctrl.Height = en.EntryHeight;

                en.ActualUI = ctrl;
                en.ActualLabel = label;

                pnlEntry.Children.Add(ctrl);

                value = rptCfg.GetConfigValue((String)txtFromPage.Tag, "");
                txtFromPage.Text = value;

                value = rptCfg.GetConfigValue((String)txtToPage.Tag, "");
                txtToPage.Text = value;

                value = rptCfg.GetConfigValue((String)txtLeft.Tag, "");
                txtLeft.Text = value;

                value = rptCfg.GetConfigValue((String)txtTop.Tag, "");               
                txtTop.Text = value;

                value = rptCfg.GetConfigValue((String)txtBottom.Tag, "");
                txtBottom.Text = value;

                value = rptCfg.GetConfigValue((String)txtRight.Tag, "");
                txtRight.Text = value;

                cbxPageRange.IsEnabled = paginator.IsNewVersion();
            }
        }

        private Boolean flagToBoolean(String flag)
        {
            if (flag.Equals("Y"))
            {
                return (true);
            }

            return (false);
        }

        private String booleanToFlag(Boolean value)
        {
            if (value)
            {
                return ("Y");
            }

            return ("N");
        }

        private CTable verifyAndConstructObject()
        {
            foreach (CEntry en in entries)
            {
                UIElement elm = en.ActualUI;
                String value = "";

                if (en.EntryType == EntryType.ENTRY_DATE_MIN)
                {
                    UDatePicker dt = (UDatePicker) elm;
                    if (dt.SelectedDate == null)
                    {
                        value = "";
                    }
                    else
                    {
                        value = CUtil.DateTimeToDateStringInternalMin((DateTime) dt.SelectedDate);
                    }
                }
                else if (en.EntryType == EntryType.ENTRY_DATE_MAX)
                {
                    UDatePicker dt = (UDatePicker)elm;
                    if (dt.SelectedDate == null)
                    {
                        value = "";
                    }
                    else
                    {
                        value = CUtil.DateTimeToDateStringInternalMax((DateTime)dt.SelectedDate);
                    }
                }
                else if (en.EntryType == EntryType.ENTRY_MONTH_YEAR)
                {
                    UDateEntry dt = (UDateEntry)elm;
                    if (dt.SelectedDate == null)
                    {
                        value = "";
                    }
                    else
                    {
                        value = CUtil.DateTimeToDateStringInternal((DateTime)dt.SelectedDate);
                    }
                }
                else if (en.EntryType == EntryType.ENTRY_TEXT_BOX)
                {
                    TextBox txt = (TextBox)elm;

                    if (!CHelper.ValidateTextBox((Label) en.ActualLabel, txt, en.NullAllowed))
                    {
                        return (null);
                    }
                    
                    value = txt.Text;
                }
                else if (en.EntryType == EntryType.ENTRY_CHECK_BOX)
                {
                    CheckBox cbx = (CheckBox)elm;

                    value = booleanToFlag((Boolean) cbx.IsChecked);
                }
                else if (en.EntryType == EntryType.ENTRY_COMBO_BOX)
                {
                    ComboBox cbo = (ComboBox)elm;

                    if (!CHelper.ValidateComboBox((Label)en.ActualLabel, cbo, en.NullAllowed))
                    {
                        return (null);
                    }

                    MBaseModel v = (MBaseModel) cbo.SelectedItem;
                    value = en.ObjectToIndexFunction(v);
                }

                String fld = en.FieldName;
                rptCfg.SetConfigValue(fld, value, "String", "");
            }

            if (!CHelper.ValidateComboBox(lblPaperType, cboPaperType, false))
            {
                return (null);
            }

            MMasterRef mr = (MMasterRef) cboPaperType.SelectedItem;

            rptCfg.SetConfigValue((String)cboPaperType.Tag, mr.MasterID, "Integer", "");


            if (radPotrait.IsChecked == true)
            {
                rptCfg.SetConfigValue((String)lblPaperType.Tag, (String) radPotrait.Tag, "String", "");
            }
            else if (radLandScape.IsChecked == true)
            {
                rptCfg.SetConfigValue((String)lblPaperType.Tag, (String)radLandScape.Tag, "String", "");
            }

            if (cbxPageRange.IsChecked == true)
            {
                paginator.isPageRange = true;

                if (!CHelper.ValidateTextBox(lblFromPage, txtFromPage, false, InputDataType.InputTypeZeroPossitiveInt))
                {
                    return (null);
                }
                paginator.fromPage = CUtil.StringToInt(txtFromPage.Text);

                if (!CHelper.ValidateTextBox(lblToPage, txtToPage, false, InputDataType.InputTypeZeroPossitiveInt))
                {
                    return (null);
                }
                paginator.toPage = CUtil.StringToInt(txtToPage.Text);

                if (paginator.toPage < paginator.fromPage)
                {
                    CHelper.ShowErorMessage("", "ERROR_PAGE_RANGE", null);
                    txtFromPage.Focus();

                    return (null);
                }
            }

            if (!CHelper.ValidateTextBox(lblMarginLeft, txtLeft, false, InputDataType.InputTypeZeroPossitiveDecimal))
            {
				return (null);
			}
            if (!CHelper.ValidateTextBox(lblMarginTop, txtTop, false, InputDataType.InputTypeZeroPossitiveDecimal))
            {
				return (null);
			}
            if (!CHelper.ValidateTextBox(lblMarginRight, txtRight, false, InputDataType.InputTypeZeroPossitiveDecimal))
			{
				return (null);
			}
			if (!CHelper.ValidateTextBox(lblMarginBottom, txtBottom, false, InputDataType.InputTypeZeroPossitiveDecimal))
			{
				return (null);
			}

            rptCfg.SetConfigValue((String) txtFromPage.Tag, txtFromPage.Text, "String", "");
            rptCfg.SetConfigValue((String) txtToPage.Tag, txtToPage.Text, "String", "");
            rptCfg.SetConfigValue((String) txtLeft.Tag, txtLeft.Text, "String", "");
            rptCfg.SetConfigValue((String) txtTop.Tag, txtTop.Text, "String", "");
            rptCfg.SetConfigValue((String) txtRight.Tag, txtRight.Text, "String", "");
            rptCfg.SetConfigValue((String) txtBottom.Tag, txtBottom.Text, "String", "");
            CUtil.EnableForm(false, this);
            CReportConfigs.SaveReportConfig(null, rptCfg);
            CUtil.EnableForm(true, this);

            CTable tb = rptCfg.GetParamObject();
            return (tb);
        }

        public WinReportParam(MMasterRef mr)
        {
            dat = mr;
            InitializeComponent();
        }

        public CBaseReport Paginator
        {
            get
            {
                return (paginator);
            }
        }

        public Boolean IsDone
        {
            get
            {
                return (isDone);
            }
        }

        private void UpdateProgress(int current, int max)
        {
            tc.UpdateProgress(current, max);
        }


        private void UpdateDone(Boolean done, Boolean fail)
        {
             tc.UpdateDone(done);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void ShowProgWin()
        {
            WinProgress wp = new WinProgress(tc);
            wp.ShowDialog();
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            CTable param = verifyAndConstructObject();

            if (param == null)
            {
                return;
            }

            tc = new CThreadSync();

            Thread t = new Thread(ShowProgWin);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            CReportFactory.UpdateExtendedParam(dat, param);

            paginator.UpdateReportConfig(rptCfg);
            paginator.SetReportParam(param);
            paginator.SetProgressUpdateFunc(UpdateProgress);
            paginator.SetProgressDoneFunc(UpdateDone);
            if (paginator.IsNewVersion())
            {
                paginator.CreateReportFixedDocument();
            }
            else
            {
                paginator.CreateFixedDocument();
            }

            isDone = true;
            this.Close();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            String tmp = xml;

            isDone = false;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CReportFactory.InitReports();
            paginator = CReportFactory.GetReportObject(dat);
            this.Title = this.Title + " : " + paginator.GetType().Name;                 
            CUtil.EnableForm(false, this);
            rptCfg = CReportConfigs.GetReportConfig(null, dat.Code);
            CUtil.EnableForm(true, this);

            String paperType = "1";
            if (rptCfg == null)
            {
                radLandScape.IsChecked = true;
                rptCfg = new MReportConfig(new CTable(""));
                rptCfg.ReportName = dat.Code;
            }
            else
            {
                paperType = rptCfg.PaperType;
                if (rptCfg.PageOrientation.Equals("LANDSCAPE"))
                {
                    radLandScape.IsChecked = true;
                }
                else
                {
                    radPotrait.IsChecked = true;
                }
            }

            CUtil.LoadPaperType(cboPaperType, false, paperType);

            constructFieldEntry();
            constructUI();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
