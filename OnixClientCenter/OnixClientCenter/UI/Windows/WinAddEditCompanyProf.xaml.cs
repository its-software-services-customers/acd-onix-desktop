using System;
using System.Windows;
using System.Windows.Controls;
using Wis.WsClientAPI;
using System.Collections;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditCompanyProf : Window
    {
        private MCompanyProfile vw = null;

        public String Caption = "";
        public String Mode = "";
        public Boolean DialogOK = false;

        public WinAddEditCompanyProf()
        {
            InitializeComponent();
        }

        private void txtCompanyCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblCompanyCode, txtCompanyCode, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblTaxID, txtTaxID, true);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblCompanyNameThai, txtCompanyNameThai, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblCompanyNameEng, txtCompanyNameEng, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblOperatorName, txtOperatorName, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblOperatorNameEng, txtOperatorNameEng, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblAddress, txtAddress, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblAddressEng, txtAddressEng, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblTel, txtTel, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblFax, txtFax, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblEmail, txtEmail, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblWebsite, txtWebsite, false);
            if (!result)
            {
                return (result);
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
            if (!CHelper.VerifyAccessRight("GENERAL_COMPANY_EDIT"))
            {
                return (false);
            }

            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    CUtil.EnableForm(false, this);
                    CTable newobj = OnixWebServiceAPI.CreateCompanyProfile(vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (newobj != null)
                    {
                        vw.SetDbObject(newobj);
                        return (true);
                    }

                    //Error here
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
                        CTable t = OnixWebServiceAPI.UpdateCompanyProfile(vw.GetDbObject());
                        CUtil.EnableForm(true, this);
                        if (t != null)
                        {
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
                DialogOK = true;
                CUtil.EnableForm(true, this);

                this.Close();
            }
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

        private void LoadData()
        {
            txtCompanyCode.Focus();

            CTable t = new CTable("COMPANY_PROFILE");
            vw = new MCompanyProfile(t);
            (vw as MCompanyProfile).CreateDefaultValue();

            DataContext = vw;

            CUtil.EnableForm(false, this);

            ArrayList arr = OnixWebServiceAPI.GetCompanyProfileList(t);
            Mode = "A";
            if (arr.Count > 0)
            {
                Mode = "E";
            }

            if (Mode.Equals("E"))
            {
                CTable o = (CTable) arr[0];
                CTable m = OnixWebServiceAPI.SubmitObjectAPI("GetCompanyProfileInfo", o);

                if (m != null)
                {
                    vw.SetDbObject(m);
                    vw.InitCompanyImage();
                    (vw as MCompanyProfile).NotifyAllPropertiesChanged();
                }
            }

            vw.IsModified = false;
            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadData();
        }

        private void cboBranch_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void CmdUpload_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImgLogo_SelectedFileChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
