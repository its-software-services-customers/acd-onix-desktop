using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.Utils;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.Commons.Windows
{
    public class ValidationParam
    {
        public Control Label { get; set; }
        public Control Control { get; set; }
        public Boolean IsEmptyAllow { get; set; }
    }

    public class WinBase : Window
    {
        private ArrayList validationRegistered = new ArrayList();
        private Hashtable listViewColumnSizes = new Hashtable();

        protected MBaseModel vw = null;
        protected CWinLoadParam loadParam = null;
        protected String createAPIName = "";
        protected String updateAPIName = "";
        protected String getInfoAPIName = "";
        protected String accessRightName = "";
        protected String approveAPIName = "";
        protected String verifyAPIName = "";
        protected Boolean isInLoad = true;

        public Boolean IsOKClick = false;

        public WinBase()
        {
        }

        public WinBase(CWinLoadParam param)
        {
            loadParam = param;
        }

        public String Caption
        {
            get
            {
                return (loadParam.Caption);
            }
        }

        protected void registerValidateControls(Control label, Control ctrl, Boolean allowEmpty)
        {
            ValidationParam vp = new ValidationParam();
            vp.Label = label;
            vp.Control = ctrl;
            vp.IsEmptyAllow = allowEmpty;

            validationRegistered.Add(vp);
        }

        protected void registerListViewSize(String name, double[] ratios)
        {
            listViewColumnSizes.Add(name, ratios);
        }

        protected void registerModelView(MBaseModel mv)
        {
            vw = mv;
        }

        protected Boolean approveData()
        {
            if (!CHelper.VerifyAccessRight(accessRightName))
            {
                return (false);
            }

            if (!validateData())
            {
                return (false);
            }

            CUtil.EnableForm(false, this);
            CTable newobj = OnixWebServiceAPI.SubmitObjectAPI(approveAPIName, vw.GetDbObject());
            CUtil.EnableForm(true, this);

            if (newobj == null)
            {
                //Error here
                CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                return (false);
            }

            MInventoryDoc vcd = new MInventoryDoc(newobj);
            vcd.InitErrorItem();
            if (vcd.ErrorItems.Count > 0)
            {
                WinErrorDetails w = new WinErrorDetails(vcd.ErrorItems, "InventoryDoc");
                w.Title = CLanguage.getValue("approve_error");
                w.ShowDialog();

                return (false);
            }

            if (loadParam.Mode.Equals("A"))
            {
                vw.SetDbObject(newobj);
                loadParam.ParentItemSources.Insert(0, vw);
            }
            else
            {
                loadParam.ActualView.SetDbObject(newobj);
                loadParam.ActualView.NotifyAllPropertiesChanged();
            }

            return (true);
        }


        protected Boolean verifyData()
        {
            if (!validateData())
            {
                return (false);
            }

            CUtil.EnableForm(false, this);
            CTable newobj = OnixWebServiceAPI.SubmitObjectAPI(verifyAPIName, vw.GetDbObject());
            CUtil.EnableForm(true, this);

            if (newobj == null)
            {
                //Error here
                CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                return (false);
            }

            MInventoryDoc vcd = new MInventoryDoc(newobj);
            vcd.InitErrorItem();
            if (vcd.ErrorItems.Count > 0)
            {
                WinErrorDetails w = new WinErrorDetails(vcd.ErrorItems, "InventoryDoc");
                w.Title = CLanguage.getValue("approve_error");
                w.ShowDialog();

                return (false);
            }
            else
            {
                String msg = CLanguage.getValue("VERIFY_SUCCESS");
                CMessageBox.Show(msg, "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            return (true);
        }

        protected virtual void addSubItem()
        {
        }

        protected virtual void beforeSaveItem()
        {
        }

        protected Boolean saveDataItem()
        {
            if (!CHelper.VerifyAccessRight(accessRightName))
            {
                return (false);
            }

            if (!vw.IsModified)
            {
                return (true);
            }

            if (!validateData())
            {
                return (false);
            }

            beforeSaveItem();

            CUtil.EnableForm(false, this);
            if (loadParam.Mode.Equals("A"))
            {
                addSubItem();
            }
            else if (loadParam.Mode.Equals("E"))
            {
                CTable o = loadParam.ActualView.GetDbObject();
                o.CopyFrom(vw.GetDbObject());
                loadParam.ActualView.NotifyAllPropertiesChanged();
            }
            CUtil.EnableForm(true, this);

            return (true);
        }

        protected Boolean saveData()
        {
            if (!CHelper.VerifyAccessRight(accessRightName))
            {
                return (false);
            }

            if (!vw.IsModified)
            {
                return (true);
            }

            if (loadParam.Mode.Equals("A"))
            {
                if (validateData())
                {
                    CUtil.EnableForm(false, this);
                    CTable newobj = OnixWebServiceAPI.SubmitObjectAPI(createAPIName, vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (newobj != null)
                    {
                        vw.SetDbObject(newobj);
                        loadParam.ParentItemSources.Insert(0, vw);
                        return (true);
                    }

                    //Error here
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                    return (false);
                }
            }
            else if (loadParam.Mode.Equals("E"))
            {
                Boolean result = validateData();
                
                if (result)
                {
                    CUtil.EnableForm(false, this);
                    CTable t = OnixWebServiceAPI.SubmitObjectAPI(updateAPIName, vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (t != null)
                    {
                        loadParam.ActualView.SetDbObject(t);
                        loadParam.ActualView.NotifyAllPropertiesChanged();

                        return (true);
                    }

                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_EDIT", null);
                }

                return (false);
            }

            return (false);
        }

        protected Boolean validateData()
        {
            Boolean result = false;
           
            foreach (ValidationParam vp in validationRegistered)
            {
                if (vp.Control is TextBox)
                {
                    result = CHelper.ValidateTextBox((Label) vp.Label, (TextBox) vp.Control, vp.IsEmptyAllow);
                }
                if (vp.Control is PasswordBox)
                {
                    result = CHelper.ValidateTextBox((Label)vp.Label, (PasswordBox)vp.Control, vp.IsEmptyAllow);
                }
                else if (vp.Control is UTextBox)
                {
                    result = CHelper.ValidateTextBox((Label)vp.Label, (UTextBox)vp.Control, vp.IsEmptyAllow);
                }
                else if (vp.Control is ComboBox)
                {
                    result = CHelper.ValidateComboBox((Label)vp.Label, (ComboBox)vp.Control, vp.IsEmptyAllow);
                }
                else if (vp.Control is UComboBox)
                {
                    result = CHelper.ValidateComboBox((Label)vp.Label, (UComboBox)vp.Control, vp.IsEmptyAllow);
                }
                else if (vp.Control is ULookupSearch2)
                {
                    result = CHelper.ValidateLookup((Label)vp.Label, (ULookupSearch2)vp.Control, vp.IsEmptyAllow);
                }

                if (!result)
                {
                    return (result);
                }
            }

            result = postValidate();
            if (!result)
            {
                return (false);
            }

            return (true);
        }

        protected virtual Boolean postValidate()
        {
            return (true);
        }

        protected void loadDataItem()
        {
            CTable t = new CTable("");
            vw = createObject();

            DataContext = vw;

            CUtil.EnableForm(false, this);

            if (loadParam.Mode.Equals("E"))
            {
                CTable newDB = loadParam.ActualView.GetDbObject().Clone();
                vw.SetDbObject(newDB);
                vw.InitializeAfterLoaded();
                vw.NotifyAllPropertiesChanged();
            }
            else
            {
            }

            vw.IsModified = false;
            isInLoad = false;

            CUtil.EnableForm(true, this);
        }

        protected void loadData()
        {
            CTable t = new CTable("");
            vw = createObject();

            DataContext = vw;

            CUtil.EnableForm(false, this);

            if (loadParam.Mode.Equals("E"))
            {
                CTable m = OnixWebServiceAPI.SubmitObjectAPI(getInfoAPIName, loadParam.ActualView.GetDbObject());
                if (m != null)
                {
                    vw.SetDbObject(m);
                    vw.InitializeAfterLoaded();
                    vw.NotifyAllPropertiesChanged();
                    vw.InitializeAfterNotified();
                }
            }

            vw.IsModified = false;
            isInLoad = false;

            CUtil.EnableForm(true, this);
        }

        protected virtual MBaseModel createObject()
        {
            return (null);
        }

        protected virtual Boolean isEditable()
        {
            return (true);
        }

        #region Event Handler

        protected void DefaultListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView lsv = (ListView)sender;
            String name = lsv.Name;

            double[] ratios = (double[]) listViewColumnSizes[name];
            double w = (e.NewSize.Width * 1) - 35;

            CUtil.ResizeGridViewColumns(lsv.View as GridView, ratios, w);
        }

        private void DefaultWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isEditable())
            {
                return;
            }

            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    Boolean r = saveData();
                    e.Cancel = !r;
                }
            }
        }

        private void DefaultItemWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isEditable())
            {
                return;
            }

            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    Boolean r = saveDataItem();
                    e.Cancel = !r;
                }
            }
        }

        public void DefaultPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!isInLoad)
            {
                vw.IsModified = true;
            }
        }

        protected void DefaultComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (!isInLoad)
            {
                vw.IsModified = true;
            }
        }

        protected void DefaultTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!isInLoad)
            {
                vw.IsModified = true;
            }
        }

        protected void DefaultUTxtbox_TextChanged(object sender, EventArgs e)
        {
            if (!isInLoad)
            {
                vw.IsModified = true;
            }
        }

        protected void DefaultUCbobox_SelectedObjectChanged(object sender, EventArgs e)
        {
            if (!isInLoad)
            {
                vw.IsModified = true;
            }
        }

        protected void DefaultTextBoxNumber_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        protected void DefaultWindow_ContentRendered(object sender, EventArgs e)
        {
            loadData();
        }

        protected void DefaultItemWindow_ContentRendered(object sender, EventArgs e)
        {
            loadDataItem();
        }

        protected void DefaultImageFile_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (!isInLoad)
            {
                vw.IsModified = true;
            }
        }

        protected void DefaultRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInLoad)
            {
                vw.IsModified = true;
            }
        }

        protected void DefaultCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (!isInLoad)
            {
                vw.IsModified = true;
            }
        }

        private void DefaultUDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isInLoad)
            {
                vw.IsModified = true;
            }
        }

        protected void DefaultWindow_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void DefaultULookup_SelectedObjectChanged(object sender, EventArgs e)
        {
            if (!isInLoad)
            {
                vw.IsModified = true;
            }
        }

        #endregion
    }
}
