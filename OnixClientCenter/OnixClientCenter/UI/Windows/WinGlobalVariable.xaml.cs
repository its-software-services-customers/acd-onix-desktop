using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinGlobalVariable : Window
    {
        private MGlobalVariable vw = new MGlobalVariable(new CTable(""));

        private String cption = "";

        public WinGlobalVariable(String caption)
        {
            cption = caption;

            DataContext = vw;
            InitializeComponent();
        }
        public String Caption
        {
            get
            {
                return (cption);
            }
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (!vw.IsModified)
            {
                this.Close();
                return;
            }

            Boolean r = SaveData("N");
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = true;

            ObservableCollection<MGlobalVariable> items = vw.Items;
            foreach (MGlobalVariable v in items)
            {
                Label lbl = new Label();
                lbl.Content = v.VariableTypeName;
                if (v.VariableType == "1") //Int
                {
                    TextBox txt = new TextBox();
                    txt.Text = v.VariableValue;

                    result = CHelper.ValidateTextBox(lbl, txt, false, InputDataType.InputTypeZeroPossitiveInt);
                    if (!result)
                    {
                        return (result);
                    }
                }
                else if (v.VariableType == "2") //Text
                {
                    TextBox txt = new TextBox();
                    txt.Text = v.VariableValue;

                    result = CHelper.ValidateTextBox(lbl, txt, false);
                    if (!result)
                    {
                        return (result);
                    }
                }
                else if (v.VariableType == "3") //Boolean
                {
                    ComboBox cbo = new ComboBox();
                    cbo.SelectedValue = v.VariableValue;

                    result = CHelper.ValidateComboBox(lbl, cbo, false);
                    if (!result)
                    {
                        return (result);
                    }
                }
                else if (v.VariableType == "4") //Double
                {
                    TextBox txt = new TextBox();
                    txt.Text = v.VariableValue;

                    result = CHelper.ValidateTextBox(lbl, txt, false, InputDataType.InputTypeZeroPossitiveDecimal);
                    if (!result)
                    {
                        return (result);
                    }
                }
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

        private Boolean SaveData(String approveFlag)
        {
            if (!CHelper.VerifyAccessRight("GENERAL_VARIABLE_EDIT"))
            {
                return (false);
            }

            //Edit mode, always

            if (vw.IsModified)
            {
                Boolean result = SaveToView();
                if (result)
                {
                    CUtil.EnableForm(false, this);
                    CTable t = OnixWebServiceAPI.UpdateGlobalVariable(vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (t != null)
                    {
                        CGlobalVariable.InitGlobalVariables();                        

                        return (true);
                    }
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_EDIT", null);
                }
                return (false);
            }

            return (true);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vw.IsModified)
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
            CUtil.EnableForm(false, this);
            CTable o = OnixWebServiceAPI.GetGlobalVariableInfo(vw.GetDbObject());
            vw.SetDbObject(o);
            vw.InitItem(CProductFilter.IsGlobalVaribleRequired);
            vw.NotifyAllPropertiesChanged();

            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
            vw.IsModified = false;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cboBoolean_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbo = sender as ComboBox;
            
            if (cbo.SelectedIndex == 0)
            {
                vw.VariableValue = "0";
            }
            else if (cbo.SelectedIndex == 1)
            {
                vw.VariableValue = "1";
            }
            vw.IsModified = true;
        }

        private void rootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
