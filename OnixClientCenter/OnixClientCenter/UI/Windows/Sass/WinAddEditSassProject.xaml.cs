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
    public partial class WinAddEditSassProject : Window
    {
        private MProject vw = null;
        private MProject actualView = null;
        private Hashtable projectGroupMap = null;
        private MMasterRef projectGroup = null;
        private ObservableCollection<MBaseModel> parentItemsSource = null;
        private String Mode = "";

        public String Caption = "";        
        public Boolean DialogOK = false;

        public WinAddEditSassProject(String mode)
        {
            Mode = mode;

            ObservableCollection<MMasterRef> projectGroups = CMasterReference.Instance.ProjectGroups;
            projectGroupMap = CUtil.ObserableCollectionToHash(projectGroups, "Code");
            projectGroup = (MMasterRef) projectGroupMap["SASS"];

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

        public Boolean IsEditable
        {
            get
            {
                return(Mode.Equals("A"));
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            if (!projectGroupMap.ContainsKey("SASS"))
            {
                CMessageBox.Show(CLanguage.getValue("ERROR_NO_SASS_PROJECT_FOUND"), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return (false);
            }

            result = CHelper.ValidateComboBox(lblCode, cboProduct, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblCode, txtCBU, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblCode, txtSystem, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblName, txtName, false);
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

            if (OnixWebServiceAPI.IsObjectExistAPI("IsProjectExist", uv.GetDbObject()))
            {
                CHelper.ShowKeyExist(lblCode, txtName);
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
            if (!CHelper.VerifyAccessRight("SASS_PROJECT_EDIT"))
            {
                return (false);
            }

            vw.ProjectGroup = projectGroup.MasterID;
            vw.ProjectGroupName = projectGroup.Description;

            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    CUtil.EnableForm(false, this);
                    CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("CreateProject", vw.GetDbObject());
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
                        CTable t = OnixWebServiceAPI.SubmitObjectAPI("UpdateProject", vw.GetDbObject());
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
            cboProduct.Focus();

            CTable t = new CTable("");
            vw = new MProject(t);
            vw.CreateDefaultValue();
            DataContext = vw;

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable m = OnixWebServiceAPI.SubmitObjectAPI("GetProjectInfo", actualView.GetDbObject());
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
    }
}
