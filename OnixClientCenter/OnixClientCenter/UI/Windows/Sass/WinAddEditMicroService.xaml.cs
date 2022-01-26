using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.Client.Model.Sass;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows.Sass
{
    public partial class WinAddEditMicroService : Window
    {
        private MMicroService vw = null;
        private MMicroService actualView = null;

        private ObservableCollection<MBaseModel> parentItemsSource = null;

        public String Caption = "";
        public String Mode = "";
        public Boolean DialogueOK = false;

        public WinAddEditMicroService()
        {            
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public MBaseModel ViewData
        {
            set
            {
                actualView = (MMicroService) value;
            }
        }

        public ObservableCollection<MBaseModel> ParentItemSource
        {
            set
            {
                parentItemsSource = value;
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblCode, txtCode, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblDesc, txtDesc, false);
            if (!result)
            {
                return (result);
            }

            CTable ug = new CTable("");
            MMicroService uv = new MMicroService(ug);
            uv.ServiceCode = txtCode.Text;
            if (vw != null)
            {
                uv.ServiceID = (vw as MMicroService).ServiceID;
            }
            if (OnixWebServiceAPI.IsObjectExistAPI("SassIsMicroServiceExist", uv.GetDbObject()))
            {
                CHelper.ShowKeyExist(lblCode, txtCode);
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
            if (!CHelper.VerifyAccessRight("SASS_SERVICE_EDIT"))
            {
                return(false);
            }

            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    CUtil.EnableForm(false, this);
                    CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("SassCreateMicroService", vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (newobj != null)
                    {
                        vw.SetDbObject(newobj);
                        parentItemsSource.Insert(0, vw);
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
                        CTable t = OnixWebServiceAPI.SubmitObjectAPI("SassUpdateMicroService", vw.GetDbObject());
                        CUtil.EnableForm(true, this);
                        if (t != null)
                        {
                            actualView.SetDbObject(t);
                            actualView.NotifyAllPropertiesChanged();

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

                DialogueOK = true;
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
            this.Title = Caption;
            txtCode.SetFocus();

            CTable t = new CTable("");
            vw = new MMicroService(t);
            vw.CreateDefaultValue();

            DataContext = vw;

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable m = OnixWebServiceAPI.SubmitObjectAPI("SassGetMicroServiceInfo", actualView.GetDbObject());
                if (m != null)
                {
                    vw.SetDbObject(m);
                    vw.NotifyAllPropertiesChanged();                   
                }              
            }

            vw.IsModified = false;

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

        private void cboBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboType_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
