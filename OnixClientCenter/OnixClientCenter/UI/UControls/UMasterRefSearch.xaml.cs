using System;
using System.Windows.Controls;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.UControls
{

    public partial class UMasterRefSearch : UserControl
    {
        private UserControl provider = null;
        private MMasterRef param = new MMasterRef(new CTable("MASTER_REF"));        

        public UMasterRefSearch()
        {
            DataContext = param;
            InitializeComponent();            
        }

        public void SetProvider(UserControl u)
        {
            provider = u;
        }

        public void LoadData()
        {
            CUtil.LoadMasterRefType(cboRefType, true, "", CProductFilter.IsRefTypeRequired);
            (provider as UMasterRef).SetLabelCombobox(lblRefType, cboRefType);
        }

        private void cmdSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Boolean result = CHelper.ValidateComboBox(lblRefType, cboRefType, false);
            if (!result)
            {
                return;
            }

            this.IsEnabled = false;

            CUtil.EnableForm(false, this);
            (provider as UMasterRef).QueryData(param.GetDbObject());
            CUtil.EnableForm(true, this);

            this.IsEnabled = true;
        }
    }
}
