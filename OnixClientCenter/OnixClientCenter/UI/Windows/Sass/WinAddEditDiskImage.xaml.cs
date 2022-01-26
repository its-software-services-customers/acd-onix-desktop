using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;
using System.Collections;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows.Sass
{
    public partial class WinAddEditDiskImage : Window
    {
        private MProject vw = null;
        private MProject actualView = null;
        private ObservableCollection<MBaseModel> parentItemsSource = null;

        public String Caption = "";
        public String Mode = "";

        public Boolean DialogOK = false;

        public WinAddEditDiskImage()
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
                actualView = (MProject)value;
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

            result = CHelper.ValidateTextBox(lblDesc, txtDesc, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateComboBox(lblRole, cboRole, false);
            if (!result)
            {
                return (result);
            }

            CTable ug = new CTable("");
            MProject uv = new MProject(ug);

            if (vw != null)
            {
                uv.ProjectID = (vw as MProject).ProjectID;
                uv.ProjectCode = (vw as MProject).ProjectCode;
            }

            if (OnixWebServiceAPI.IsObjectExistAPI("SassIsDiskImageExist", uv.GetDbObject()))
            {
                CHelper.ShowKeyExist(lblName, txtName);
                return (false);
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
            if (!CHelper.VerifyAccessRight("SASS_IMAGE_EDIT"))
            {
                return (false);
            }

            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    CUtil.EnableForm(false, this);
                    CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("SassCreateDiskImage", vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (newobj != null)
                    {
                        vw.SetDbObject(newobj);
                        parentItemsSource.Insert(0, vw);
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
                        CTable t = OnixWebServiceAPI.SubmitObjectAPI("SassUpdateDiskImage", vw.GetDbObject());
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
                DialogOK = true;
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
                    Boolean r = SaveData();
                    e.Cancel = !r;
                }
            }
        }

        private void LoadData()
        {
            //txtProduct.Focus();

            CTable t = new CTable("");
            vw = new MProject(t);
            vw.CreateDefaultValue();
            DataContext = vw;

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable m = OnixWebServiceAPI.SubmitObjectAPI("SassGetDiskImageInfo", actualView.GetDbObject());
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

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboProjectGroup_SelectedObjectChanged(object sender, EventArgs e)
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

        private void uProject_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
