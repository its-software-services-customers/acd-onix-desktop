using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.ClientCenter.Windows;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Commons.Utils;
using System.Windows.Controls;

namespace Onix.ClientCenter.UI.General.Service
{
    public partial class WinAddEditService : WinBase
    {
        public WinAddEditService(CWinLoadParam param) : base(param)
        {
            accessRightName = "GENERAL_SERVICE_EDIT";

            createAPIName = "CreateService";
            updateAPIName = "UpdateService";
            getInfoAPIName = "GetServiceInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblCode, txtCode, false);
            registerValidateControls(lblName, txtName, false);
            registerValidateControls(lblType, cboType, false);
            registerValidateControls(lblUOM, cboUOM, false);
        }

        protected override Boolean postValidate()
        {
            Label lbl = new Label();
            lbl.Content = cbxWhTax.Content;

            Boolean result = CHelper.ValidateTextBox(lbl, txtWhPCT, (bool) !cbxWhTax.IsChecked);
            if (!result)
            {
                return (false);
            }

            result = CHelper.ValidateComboBox(lblWhGroup, cboWhGroup, (bool)!cbxWhTax.IsChecked);
            if (!result)
            {
                return (false);
            }

            return (true);
        }

        protected override MBaseModel createObject()
        {
            MService mv = new MService(new CTable(""));

            mv.CreateDefaultValue();
            mv.ServiceCategory = "1"; //Sale
            mv.ServiceLevel = loadParam.GenericType;
            mv.Category = "0";
            mv.IsSalary = false;

            return (mv);
        }        

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                IsOKClick = true;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }

        private void cmdInterval_Click(object sender, RoutedEventArgs e)
        {
            MService mv = (MService) vw;

            WinAddEditIntervalConfigEx w = new WinAddEditIntervalConfigEx(mv.PricingDefinition, mv.ServiceName, "IP");
            w.ShowDialog();
            if (w.IsOK)
            {
                mv.PricingDefinition = w.ConfigString;
                mv.IsModified = true;
            }
        }
    }
}
