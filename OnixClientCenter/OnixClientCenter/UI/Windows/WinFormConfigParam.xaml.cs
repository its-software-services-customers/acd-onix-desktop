using System;
using System.Windows;
using Onix.Client.Model;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Windows
{
    public partial class WinFormConfigParam : Window
    {
        //private MBaseModel dataSource = null;
        private MReportConfig temp = new MReportConfig(new CTable(""));
        private Boolean isOK = false;

        public WinFormConfigParam(String rptGroup, MBaseModel model)
        {
            temp.CopyValues((MReportConfig) model);

            DataContext = temp;
            InitializeComponent();
        }

        public Boolean IsOK
        {
            get
            {
                return (isOK);
            }
        }

        public MReportConfig Config
        {
            get
            {
                return (temp);
            }
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            isOK = true;
            this.Close();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtValue_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void rootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
