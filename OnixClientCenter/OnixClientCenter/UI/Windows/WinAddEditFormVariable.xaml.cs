using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditFormVariable : Window
    {
        private MFormConfigVariable vw = new MFormConfigVariable(new CTable(""));
        private MFormConfigVariable actualView = null;
        private MFormConfig parentObj = null;
        private String mode = "";

        public String Caption = "";
        
        public Boolean HasModified = false;

        public WinAddEditFormVariable(String md, MBaseModel parent, MBaseModel actObj)
        {
            actualView = (MFormConfigVariable) actObj;
            parentObj = (MFormConfig) parent;
            mode = md;

            DataContext = vw;

            InitializeComponent();
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public Boolean IsEditable
        {
            get
            {
                if (mode.Equals("A"))
                {
                    return (true);
                }

                if (mode.Equals("E"))
                {
                    return (parentObj.Language.Equals(actualView.Scope));
                }

                return (false);
            }
        }

        public Boolean IsValueEditable
        {
            get
            {
                if (mode.Equals("O"))
                {
                    return (true);
                }

                return (IsEditable);
            }
        }

        private void LoadData()
        {
            this.Title = Caption;

            CUtil.EnableForm(false, this);

            if (mode.Equals("E") || mode.Equals("O"))
            {
                CTable newDB = actualView.GetDbObject().Clone();
                vw.SetDbObject(newDB);
                vw.NotifyAllPropertiesChanged();
            }

            vw.IsModified = false;
            CUtil.EnableForm(true, this);
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateComboBox(lblVariableType, cboVariableType, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblVariableName, txtVariableName, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblValue, txtValue, false);
            if (!result)
            {
                return (result);
            }

            result = parentObj.IsVariableExist(vw);
            if (result)
            {
                CHelper.ShowKeyExist(lblVariableName, txtVariableName);
                return (false);
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
            if (!vw.IsModified)
            {
                return (true);
            }

            Boolean result = SaveToView();
            if (!result)
            {
                return (false);
            }

            if (mode.Equals("A"))
            {
                parentObj.AddConfigVariable(vw);
            }
            else if (mode.Equals("E"))
            {
                CTable o = actualView.GetDbObject();
                o.CopyFrom(vw.GetDbObject());
                actualView.NotifyAllPropertiesChanged();
            }
            else if (mode.Equals("O"))
            {
                parentObj.OverrideConfigVariable(vw);
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

        private void lkup_SelectedItemChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
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

        private void cboVariableType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxOverrided_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void rootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
