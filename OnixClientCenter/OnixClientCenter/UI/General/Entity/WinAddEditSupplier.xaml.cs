using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.General.Entity
{
    public partial class WinAddEditSupplier : WinBase
    {
        private MEntity mv = null;

        public WinAddEditSupplier(CWinLoadParam param) : base(param)
        {
            accessRightName = "GENERAL_SUPPLIER_EDIT";

            createAPIName = "CreateEntity";
            updateAPIName = "UpdateEntity";
            getInfoAPIName = "GetEntityInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblCode, txtCode, false);
            registerValidateControls(lblName, txtName, false);
            registerValidateControls(lblType, cboType, false);
            registerValidateControls(lblGroup, cboGroup, false);
            registerValidateControls(lblCredit, cboCreditTerm, false);
        }

        protected override Boolean postValidate()
        {
            mv.CreditTerm = mv.CreditTermDay;

            Boolean result1 = validateAddress(mv.AddressItems, tbiAddress);
            Boolean result2 = validateBankAccount(mv.BankAccounts, tbiBankAccount);
            Boolean result = result1 && result2;

            return (result);
        }

        protected override MBaseModel createObject()
        {
            mv = new MEntity(new CTable(""));
            mv.CreateDefaultValue();
            mv.Category = "2";
            mv.CreateDefaultValue();
            mv.IsRvType3 = true;

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

        private void cmdAddressAdd_Click(object sender, RoutedEventArgs e)
        {
            mv.AddAddress();
            vw.IsModified = true;
        }
        
        private void cmdAddressDelete_Click(object sender, RoutedEventArgs e)
        {
            MEntityAddress pp = (MEntityAddress)(sender as Button).Tag;
            mv.RemoveAddressItem(pp);

            mv.IsModified = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private Boolean validateAddress<T>(ObservableCollection<T> collection, TabItem titem) where T : MBaseModel
        {
            int idx = 0;
            foreach (MBaseModel c in collection)
            {
                idx++;
                MEntityAddress bi = (MEntityAddress)c;

                if (bi.AddressType.Equals(""))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    titem.IsSelected = true;

                    return (false);
                }

                if (bi.Address.Equals(""))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    titem.IsSelected = true;

                    return (false);
                }
            }

            return (true);
        }

        private Boolean validateBankAccount<T>(ObservableCollection<T> collection, TabItem titem) where T : MBaseModel
        {
            int idx = 0;
            foreach (MBaseModel c in collection)
            {
                idx++;
                MEntityBankAccount bi = (MEntityBankAccount)c;

                if (bi.BankID.Equals("") || bi.AccountType.Equals("") || bi.AccountNo.Equals(""))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    titem.IsSelected = true;

                    return (false);
                }
            }

            return (true);
        }
       
        private void cmdAccountAdd_Click(object sender, RoutedEventArgs e)
        {
            mv.AddBankAccount();
            mv.IsModified = true;
        }

        private void cmdBankAccountDelete_Click(object sender, RoutedEventArgs e)
        {
            MEntityBankAccount pp = (MEntityBankAccount)(sender as Button).Tag;
            mv.RemoveBankAccount(pp);

            mv.IsModified = true;
        }
    }
}
