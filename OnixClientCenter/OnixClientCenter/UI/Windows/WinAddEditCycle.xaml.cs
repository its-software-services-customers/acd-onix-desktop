using System;
using System.Windows;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using System.Windows.Controls;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditCycle : Window
    {
        private MBaseModel vw = null;
        private MBaseModel actualView = null;

        private ObservableCollection<MBaseModel> parents;

        public String Caption = "";
        public String Mode = "";
        public Boolean DialogOK = false;

        public MCycle CInCurrentRow = new MCycle(new CTable("CYCLE"));

        public WinAddEditCycle(String _mode, MBaseModel _actualView, ObservableCollection<MBaseModel> _parents)
        {
            Mode = _mode;
            actualView = _actualView;
            parents = _parents;
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void LoadData()
        {
            txtCycleCode.Focus();

            CTable t = new CTable("CYCLE");

            vw = new MCycle(t);
            (vw as MCycle).CreateDefaultValue();

            DataContext = vw;

            if (Mode.Equals("A"))
            {
            }
            else if (Mode.Equals("E"))
            {
                CTable m = OnixWebServiceAPI.GetCycleInfo(actualView.GetDbObject());
                if (m != null)
                {
                    vw.SetDbObject(m);
                    (vw as MCycle).NotifyAllPropertiesChanged();
                }
            }
            CUtil.LoadCycleType(cboCycleType, false, (vw as MCycle).CycleType);
            CUtil.LoadCycleDayWeekly(cboCycleWeekly, false, (vw as MCycle).DayOfWeek);
            CUtil.LoadCycleDayMonthly(cboCycleMonthly, false, (vw as MCycle).DayOfMonth);
            vw.IsModified = false;

            CUtil.EnableForm(true, this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmdOk.Focus();
            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    Boolean r = SaveData();
                    e.Cancel = !r;
                }
            }
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveData();
            if (r)
            {
                vw.IsModified = false;
                DialogOK = true;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblCycleCode, txtCycleCode, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblCycleDesc, txtCycleDesc, true);
            if (!result)
            {
                return (result);
            }

            CTable ug = new CTable("CYCLE");
            MCycle uv = new MCycle(ug);
            if (vw != null)
            {
                uv.CycleID = (vw as MCycle).CycleID;
                uv.CycleCode = (vw as MCycle).CycleCode;
            }
            if (OnixWebServiceAPI.IsCycleExist(uv.GetDbObject()))
            {
                CHelper.ShowKeyExist(lblCycleCode, txtCycleCode);
                return (false);
            }
            return (result);
        }

        private Boolean SaveToView()
        {
            if (!ValidateData())
            {
                return (false);
            }

            return (true);
        }

        private Boolean SaveData()
        {
            if (!CHelper.VerifyAccessRight("GENERAL_CYCLE_EDIT"))
            {
                return (false);
            }

            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    (vw as MCycle).CycleID = "";

                    CUtil.EnableForm(false, this);
                    CTable newobj = OnixWebServiceAPI.CreateCycle(vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (newobj != null)
                    {
                        vw.SetDbObject(newobj);
                        parents.Insert(0, vw);
                        return (true);
                    }

                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                    return (false);
                }
            }
            else if (Mode.Equals("E"))
            {
                if (vw.IsModified)
                {
                    Boolean result = SaveToView();
                    if (result)
                    {
                        CUtil.EnableForm(false, this);
                        CTable t = OnixWebServiceAPI.UpdateCycle(vw.GetDbObject());
                        CUtil.EnableForm(true, this);
                        if (t != null)
                        {
                            actualView.SetDbObject(t);
                            (actualView as MCycle).NotifyAllPropertiesChanged();

                            return (true);
                        }

                        CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_EDIT", null);
                    }

                    return (false);
                }

                return (true);
            }

            return (false);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboCycleType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
            string ID = ((sender as ComboBox).SelectedValue as MMasterRef).MasterID as string;
            if(ID.Equals("1"))
            {
                cboCycleWeekly.IsEnabled = true;
                cboCycleMonthly.IsEnabled = false;
            }
            else if (ID.Equals("2"))
            {
                cboCycleWeekly.IsEnabled = false;
                cboCycleMonthly.IsEnabled = true;
            }
        }

        private void cboCycleWeekly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboCycleMonthly_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
