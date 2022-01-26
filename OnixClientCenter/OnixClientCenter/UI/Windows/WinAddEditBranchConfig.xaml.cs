using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinAddEditBranchConfig : Window
    {
        private MBranchConfigCenter vw = null;
        private MBranchConfigCenter actualView = null;

        private ObservableCollection<MBaseModel> parentItemsSource = null;

        private String Caption = "";
        private String Mode = "";
        private Boolean isOK = false;
        private RoutedEventHandler itemAddedHandler = null;

        //private Boolean isInLoadData = false;

        public WinAddEditBranchConfig(String mode, ObservableCollection<MBaseModel> parentSources,MBranchConfigCenter data, String caption)
        {
            Mode = mode;
            parentItemsSource = parentSources;
            actualView = data;
            Caption = caption;

            vw = new MBranchConfigCenter(new CTable(""));
            DataContext = vw;

            InitializeComponent();
        }

        public RoutedEventHandler ItemAddedHandler
        {
            get
            {
                return (itemAddedHandler);
            }

            set
            {
                itemAddedHandler = value;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public Boolean IsOK
        {
            get
            {
                return (isOK);
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateComboBox(lblBranch, cboBranch, false);
            if (!result)
            {
                return (result);
            }


            result = CHelper.ValidateTextBox(lblKey, txtKey, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblVoidBill, txtVoidBill, false);
            if (!result)
            {
                return (result);
            }

            #region CashDocVat
            result = CHelper.ValidateTextBox(lblSaleByCashDocVat, txtSaleByCashDocVatPrefix, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblSaleByCashDocVat, txtSaleByCashDocVatPattern, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblCashVatLen, txtCashVatLen, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblCashVatOffset, txtCashVatOffset, false);
            if (!result)
            {
                return (result);
            }
            #endregion

            #region DebtDocVat
            result = CHelper.ValidateTextBox(lblSaleByDebtDocVat, txtSaleByDebtDocVatPrefix, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblSaleByDebtDocVat, txtSaleByDebtDocVatPattern, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblDebtVatLen, txtDebtVatLen, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblDebtVatOffset, txtDebtVatOffset, false);
            if (!result)
            {
                return (result);
            }
            #endregion

            #region CashDocNV
            result = CHelper.ValidateTextBox(lblSaleByCashDocNV, txtSaleByCashDocNVPrefix, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblSaleByCashDocNV, txtSaleByCashDocNVPattern, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblCashNoVatLen, txtCashNoVatLen, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblCashNoVatOffset, txtCashNoVatOffset, false);
            if (!result)
            {
                return (result);
            }
            #endregion

            #region DebtDocNV
            result = CHelper.ValidateTextBox(lblSaleByDebtDocNV, txtSaleByDebtDocNVPrefix, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblSaleByDebtDocNV, txtSaleByDebtDocNVPattern, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblDebtNoVatLen, txtDebtNoVatLen, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblDebtNoVatOffset, txtDebtNoVatOffset, false);
            if (!result)
            {
                return (result);
            }
            #endregion 

            result = CHelper.ValidateComboBox(lblLocationVat, cboLocationVat, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateComboBox(lblAccountVat, cboAccountVat, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateComboBox(lblLocationNoVat, cboLocationNoVat, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateComboBox(lblAccountNoVat, cboAccountNoVat, false);
            if (!result)
            {
                return (result);
            }

            CTable ug = new CTable("");
            MBranchConfigCenter uv = new MBranchConfigCenter(ug);
            uv.BranchID = vw.BranchID;
            uv.Key = vw.Key;
            if (!vw.BranchConfigID.Equals(""))
            {
                uv.BranchConfigID = vw.BranchConfigID;
            }

            if (OnixWebServiceAPI.IsBranchConfigExist(uv.GetDbObject()))
            {
                //String ss = ((cboBranch as ComboBox).SelectedValue as MMasterRef).MasterID as string;
                CHelper.ShowKeyExist(lblBranch, cboBranch);
                return (false);
            }


            return (result);
        }

        private Boolean SaveToView( )
        {
            if (!ValidateData())
            {
                return(false);
            }

            return (true);
        }

        private Boolean SaveData()
        {
            //if (!CHelper.VerifyAccessRight("_ITEM_EDIT"))
            //{
            //    return (false);
            //}

            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    CUtil.EnableForm(false, this);
                    CTable newobj = OnixWebServiceAPI.CreateBranchConfig(vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (newobj != null)
                    {
                        vw.SetDbObject(newobj);
                        if (itemAddedHandler != null)
                        {
                            itemAddedHandler(vw, null);
                        }
                        else
                        {
                            //Will be obsoleted soon
                            parentItemsSource.Insert(0, vw);
                        }

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
                        CTable t = OnixWebServiceAPI.UpdateBranchConfig(vw.GetDbObject());
                        CUtil.EnableForm(true, this);
                        if (t != null)
                        {
                            actualView.SetDbObject(t);
                            (actualView as MBranchConfigCenter).NotifyAllPropertiesChanged();

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

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveData();
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmdOK.Focus();
            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    Boolean r = SaveData();
                    e.Cancel = !r;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void LoadData()
        {
            //isInLoadData = true;

            this.Title = Caption;
            txtKey.Focus();

            vw.CreateDefaultValue();

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable m = OnixWebServiceAPI.GetBranchConfigInfo(actualView.GetDbObject());
                if (m != null)
                {
                    vw.SetDbObject(m);
                    vw.InitBranchPOSs();
                    vw.NotifyAllPropertiesChanged();
                }
            }
            else if (Mode.Equals("A"))
            {
                vw.DocNoCashPattern = CGlobalVariable.GetGlobalVariableValue("DOC_NO_CASH_DEFAULT");
                vw.DocNoDebtPattern = CGlobalVariable.GetGlobalVariableValue("DOC_NO_DEBT_DEFAULT");
                vw.DocNoCashNVPattern = CGlobalVariable.GetGlobalVariableValue("DOC_NO_CASH_DEFAULT_NV");
                vw.DocNoDebtNVPattern = CGlobalVariable.GetGlobalVariableValue("DOC_NO_DEBT_DEFAULT_NV");

                String SeqLength = CGlobalVariable.GetGlobalVariableValue("DOC_SEQ_LENGTH_DEFAULT");
                vw.DocNoCashSeqLength = SeqLength;
                vw.DocNoDebtSeqLength = SeqLength;
                vw.DocNoCashNVSeqLength = SeqLength;
                vw.DocNoDebtNVSeqLength = SeqLength;

                String YearOffset = CGlobalVariable.GetGlobalVariableValue("DOC_NO_YEAR_OFFSET_DEFAULT");
                vw.DocNoCashYearOffset = YearOffset;
                vw.DocNoDebtYearOffset = YearOffset;
                vw.DocNoCashNVYearOffset = YearOffset;
                vw.DocNoDebtNVYearOffset = YearOffset;

                String Reset = CGlobalVariable.GetGlobalVariableValue("DOC_NO_RESET_DEFAULT");
                if (Reset.Equals("2"))
                {
                    vw.IsCashVatResetByYear = true;
                    vw.IsDebtVatResetByYear = true;
                    vw.IsCashNoVatResetByYear = true;
                    vw.IsDebtNoVatResetByYear = true;
                }
                else if (Reset.Equals("1"))
                {
                    vw.IsCashVatResetByMonth = true;
                    vw.IsDebtVatResetByMonth = true;
                    vw.IsCashNoVatResetByMonth = true;
                    vw.IsDebtNoVatResetByMonth = true;
                }

                //String Account = CGlobalVariable.GetGlobalVariableValue("SALE_PETTY_CASH_ACCT_NO");
                //CUtil.LoadCashAccount(cboAccountVat, true, Account);
                //CUtil.LoadMasterRefCombo(cboBranch, true, MasterRefEnum.MASTER_BRANCH, branchID);
            }

            vw.IsModified = false;
            //isInLoadData = false;

            CUtil.EnableForm(true, this);
        }

		private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cbxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cboLocationVat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboAccountVat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboLocationNoVat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboAccountNoVat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdPOSAdd_Click(object sender, RoutedEventArgs e)
        {
            (vw as MBranchConfigCenter).AddBranchPOS();
            vw.IsModified = true;
        }

        private void cmdPosSerialNoDelete_Click(object sender, RoutedEventArgs e)
        {
            MBranchConfigPosCenter pp = (MBranchConfigPosCenter)(sender as Button).Tag;
            vw.RemovePOSSeriesItem(pp);

            vw.IsModified = true;
        }

        private void cboBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
            if (Mode.Equals("A"))
            {
                vw.DocNoCashPrefix = vw.BranchCode;
                vw.DocNoDebtPrefix = vw.BranchCode;
                vw.DocNoCashNVPrefix = vw.BranchCode;
                vw.DocNoDebtNVPrefix = vw.BranchCode;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
