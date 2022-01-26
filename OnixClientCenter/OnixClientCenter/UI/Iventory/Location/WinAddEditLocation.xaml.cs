using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.Inventory.Location
{
    public partial class WinAddEditLocation : WinBase
    {
        private MLocation mv = null;

        public WinAddEditLocation(CWinLoadParam param) : base(param)
        {
            accessRightName = "INVENTORY_LOCATION_EDIT";

            createAPIName = "CreateLocation";
            updateAPIName = "UpdateLocation";
            getInfoAPIName = "GetLocationInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblCode, txtCode, false);
            registerValidateControls(lblDesc, txtDesc, false);
            registerValidateControls(lblDesc, txtDesc, false);
        }

        protected override MBaseModel createObject()
        {
            mv = new MLocation(new CTable(""));
            mv.CreateDefaultValue();
            mv.IsForSale = false;
            mv.IsForBorrow = false;

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
    }
}
