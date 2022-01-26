using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.ClientCenter.Windows;
using Onix.ClientCenter.UControls;
using System.Text.RegularExpressions;
using Onix.Client.Model;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.ClientCenter.Reports;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.Commons.Utils
{ 
    public static class CHelper
    {
        private static Hashtable masterHash = new Hashtable();

        public static void ShowErorMessage(String msg, String errcd, CTable param)
        {
            String str = CLanguage.getValue(errcd);
            String fmt = String.Format(str, msg);
           
            CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static Boolean AskConfirmMessage(String cd)
        {
            String str = CLanguage.getValue(cd);
            MessageBoxResult result = CMessageBox.Show(str, "CONFIRM", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                return (true);
            }

            return (false);
        }

        public static Boolean AskConfirmMessage(String param, String cd)
        {
            String str = CLanguage.getValue(cd);
            String msg = String.Format(str, param);

            MessageBoxResult result = CMessageBox.Show(msg, "CONFIRM", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                return (true);
            }

            return (false);
        }

        public static void ShowKeyExist(Label lbl, TextBox tb)
        {
            String str = CLanguage.getValue("ERROR_DUPLICATE_KEY");
            String fmt = String.Format(str, lbl.Content, tb.Text);

            CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowKeyExist(Label lbl, UTextBox tb)
        {
            String str = CLanguage.getValue("ERROR_DUPLICATE_KEY");
            String fmt = String.Format(str, lbl.Content, tb.Text);

            CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowKeyExist(Label lbl, ComboBox tb)
        {
            String str = CLanguage.getValue("ERROR_DUPLICATE_KEY");
            String fmt = String.Format(str, lbl.Content, tb.Text);

            CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }


        public static Boolean AskConfirmSave()
        {
            String str = CLanguage.getValue("CONFIRM_SAVE");
            MessageBoxResult result = CMessageBox.Show(str, "CONFIRM", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                return (true);
            }

            return (false);
        }

        public static Boolean AskConfirmDelete(int cnt)
        {
            String str = CLanguage.getValue("CONFIRM_DELETE");
            String msg = String.Format(str, cnt);
            MessageBoxResult result = CMessageBox.Show(msg, "CONFIRM", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                return (true);
            }

            return (false);
        }

        public static Boolean AskConfirmDeleteItem(string cnt)
        {
            String str = CLanguage.getValue("CONFIRM_DELETE_ITEM");
            String msg = String.Format(str, cnt);
            MessageBoxResult result = CMessageBox.Show(msg, "CONFIRM", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                return (true);
            }

            return (false);
        }


        public static Boolean ValidateTextBox(Label lbl, TextBox tb, Boolean AllowEmpty)
        {
            String str = CLanguage.getValue("ERROR_TEXT_VALIDATE");
            String fmt = String.Format(str, lbl.Content);

            if (tb.IsEnabled && (tb.Visibility == Visibility.Visible))
            {
                if (AllowEmpty)
                {
                    return (true);
                }

                if (tb.Text.Trim().Equals(""))
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    tb.Focus();
                    return (false);
                }
            }
            
            return (true);
        }

        public static Boolean ValidateTextBox(Label lbl, UTextBox tb, Boolean AllowEmpty)
        {
            String str = CLanguage.getValue("ERROR_TEXT_VALIDATE");
            String fmt = String.Format(str, lbl.Content);

            if (tb.IsEnabled)
            {
                if (AllowEmpty)
                {
                    return (true);
                }

                if (tb.Text.Trim().Equals(""))
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    tb.Focus();
                    return (false);
                }
            }

            return (true);
        }

        public static Boolean ValidateTextBox(Label lbl, PasswordBox tb, Boolean AllowEmpty)
        {
            String str = CLanguage.getValue("ERROR_TEXT_VALIDATE");
            String fmt = String.Format(str, lbl.Content);

            if (tb.IsEnabled)
            {
                if (AllowEmpty)
                {
                    return (true);
                }

                if (tb.Password.Trim().Equals(""))
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    tb.Focus();
                    return (false);
                }
            }

            return (true);
        }

        public static Boolean ValidateTextBoxEscape(Label lbl, TextBox tb, String esc)
        {
            String str = CLanguage.getValue("ERROR_TEXT_ESCAPE");
            String fmt = String.Format(str, esc);

            if (tb.IsEnabled)
            {
                if (tb.Text.Trim().Contains(esc))
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    tb.Focus();
                    return (false);
                }
            }

            return (true);
        }

        public static Boolean ValidateTextBox(Label lbl, TextBox tb, Boolean AllowEmpty, InputDataType dt)
        {
            String str = CLanguage.getValue("ERROR_TEXT_VALIDATE");
            String fmt = String.Format(str, lbl.Content);

            if (tb.IsEnabled)
            {
                if (AllowEmpty)
                {
                    return (true);
                }

                if (tb.Text.Trim().Equals(""))
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    tb.Focus();
                    return (false);
                }

                Regex regex = null;
                if (dt == InputDataType.InputTypeZeroPossitiveDecimal)
                {
                    regex = new Regex(@"^\d+(\.\d{1,2})?$");
                }
                else if (dt == InputDataType.InputTypeZeroPossitiveInt)
                {
                    regex = new Regex(@"^\d+$");
                }

                Boolean result = regex.IsMatch(tb.Text);
                if (!result)
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    tb.Focus();
                    return (false);
                }
                
            }

            return (true);
        }

        public static Boolean ValidateLookup(Label lbl, ULookupSearch2 ulk, Boolean AllowEmpty)
        {
            String str = CLanguage.getValue("ERROR_TEXT_VALIDATE");
            String fmt = String.Format(str, lbl.Content);

            if (ulk.IsEnabled)
            {
                if (AllowEmpty)
                {
                    return (true);
                }

                if (ulk.IsEmpty())
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    ulk.Focus();
                    return (false);
                }
            }

            return (true);
        }

        public static Boolean ValidateComboBox(Label lbl, UComboBox cb, Boolean AllowEmpty)
        {
            String lb = "";
            if (lbl != null)
                lb = lbl.Content.ToString();

            String str = CLanguage.getValue("ERROR_TEXT_VALIDATE");
            String fmt = String.Format(str, lbl.Content);

            if (cb.IsEnabled)
            {
                if (AllowEmpty)
                {
                    return (true);
                }

                if ((cb.SelectedObject == null) || (cb.IsEmpty()))
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    cb.Focus();
                    return (false);
                }
            }

            return (true);
        }

        public static Boolean ValidateComboBox( Label lbl, ComboBox cb, Boolean AllowEmpty)
        {
            return (ValidateComboBox("", lbl, cb, AllowEmpty));
        }

        public static Boolean ValidateComboBox(String strMsg, ComboBox cb, Boolean AllowEmpty)
        {
            return (ValidateComboBox(strMsg, null, cb, AllowEmpty));
        }

        private static Boolean ValidateComboBox(String strMsg, Label lbl, ComboBox cb, Boolean AllowEmpty)
        {
            String lb = "";
            if (lbl != null)
                lb = lbl.Content.ToString();

            String str = CLanguage.getValue("ERROR_TEXT_VALIDATE");
            String fmt = "";
            if (!lb.Equals(""))
            {
                fmt = String.Format(str, lbl.Content);
            }
            else if (!strMsg.Equals(""))
            {
                fmt = String.Format(str, strMsg);
            }

            if (cb.IsEnabled)
            {
                if (AllowEmpty)
                {
                    return (true);
                }

                if (cb.Text.Trim().Equals(""))
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    cb.Focus();
                    return (false);
                }
            }

            return (true);
        }

        public static Boolean ValidatePasswordBox(Label lbl, PasswordBox tb, Boolean AllowEmpty)
        {
            String str = CLanguage.getValue("ERROR_TEXT_VALIDATE");
            String fmt = String.Format(str, lbl.Content);

            if (tb.IsEnabled)
            {
                if (AllowEmpty)
                {
                    return (true);
                }

                if (tb.Password.Trim().Equals(""))
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    tb.Focus();
                    return (false);
                }
            }

            return (true);
        }

        public static Boolean ValidateConfirmPassword(Label lbl, PasswordBox tb, Label clbl, PasswordBox ctb)
        {
            String str = CLanguage.getValue("ERROR_PASSWORD_VALIDATE");
            String fmt = String.Format(str, lbl.Content, clbl.Content);

            if (tb.IsEnabled)
            {
                if (!tb.Password.Equals(ctb.Password))
                {
                    CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    tb.Focus();
                    return (false);
                }
            }

            return (true);
        }

        public static Boolean VerifyAccessRight(String permName)
        {
            Boolean flag = CAccessValidator.VerifyAccessRight(permName);
            if (flag)
            {
                return (true);
            }

            String str = CLanguage.getValue("ERROR_VERIFY_ACCESS");
            String fmt = String.Format(str, "");            

            CMessageBox.Show(fmt, permName, MessageBoxButton.OK, MessageBoxImage.Error);

            return (false);
        }

        public static Boolean VerifyVersion()
        {
            String api = CConfig.APIVersion;
            String prog = CConfig.Version;

            String[] apibuilt = api.Split('-');
            String[] progbuilt = prog.Split('-');

            String str = CLanguage.getValue("version_mismatch");
            String strwarn = CLanguage.getValue("version_mismatch_warn");
            String fmt = String.Format(str, api, prog);
            String fmtwarn = String.Format(strwarn, api, prog);

            if (!apibuilt[0].Equals(progbuilt[0]))
            {
                if (progbuilt[0].Equals("$ONIX_BUILD_LABEL_VAR$"))
                {
                    //Run from source code is allow for version mismatch for the sake of debugging

                    CMessageBox.Show(fmtwarn, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return (true);
                }

                CMessageBox.Show(fmt, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return (false);
            }

            return (true);
        }

        public static CTable GetComboSelectedObject(ComboBox cbo)
        {
            CTable o = null;

            MBaseModel v = (MBaseModel)cbo.SelectedItem;
            if (v != null)
            {
                o = v.GetDbObject();
            }

            return (o);
        }

        public static String GetComboSelectedValue(ComboBox cbo, String fldName)
        {
            String result = "";

            MBaseModel v = (MBaseModel) cbo.SelectedItem;
            if (v != null)
            {
                CTable o = v.GetDbObject();
                if (o != null)
                {
                    result = o.GetFieldValue(fldName);
                }
            }

            return (result);
        }

        public static String CheckBoxToString(CheckBox cbx, String defValue)
        {
            if (cbx.IsChecked == null)
            {
                return (defValue);
            }

            if ((bool) cbx.IsChecked)
            {
                return ("Y");
            }

            return ("N");
        }

        public static void DeleteSelectedItems(ObservableCollection<MBaseModel> items, DeleteFunction df)
        {
            DeleteSelectedItems(items, df, null);
            return;
        }

        public static int DeleteSelectedItems(ObservableCollection<MBaseModel> items, DeleteAPIFunction df, String RecordCount, String ApiName)
        {
            int rrt = CUtil.StringToInt(RecordCount);
            int cnt = 0;

            foreach (MBaseModel v in items)
            {
                if (v.IsSelectedForDelete)
                {
                    cnt++;
                }
            }

            if (cnt <= 0)
            {
                ShowErorMessage("", "ERROR_NO_ITEM_SELECTED", null);
                return rrt;
            }

            WinDeleteProgress w = new WinDeleteProgress();
            w.ItemsSource = items;
            w.SetFileCount(cnt);
            w.DeleteAPIDelegate = df;
            w.APIName = ApiName;
            w.ShowDialog();

            if (RecordCount != null)
            {
                rrt = CUtil.StringToInt(RecordCount) - cnt;
            }

            return rrt;

        }

        public static int DeleteSelectedItems(ObservableCollection<MBaseModel> items, DeleteFunction df, String RecordCount)
        {
            int rrt = CUtil.StringToInt(RecordCount);
            int cnt = 0;

            foreach (MBaseModel v in items)
            {
                if (v.IsSelectedForDelete)
                {
                    cnt++;
                }
            }

            if (cnt <= 0)
            {
                ShowErorMessage("", "ERROR_NO_ITEM_SELECTED", null);
                return rrt;
            }

            WinDeleteProgress w = new WinDeleteProgress();
            w.ItemsSource = items;
            w.SetFileCount(cnt);
            w.DeleteDelegate = df;
            w.ShowDialog();
            
            if (RecordCount != null)
            {
                rrt = CUtil.StringToInt(RecordCount) - cnt;
            }

            return rrt;
            
        }

        public static void LoadUserGroupCombo(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("USER_GROUP");
            ArrayList arr = OnixWebServiceAPI.GetUserGroupList(obj);
            List<MUserGroup> items = new List<MUserGroup>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MUserGroup v = new MUserGroup(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }
            
            foreach (CTable o in arr)
            {
                MUserGroup v = new MUserGroup(o);
                v.RowIndex = idx;
                items.Add(v);

                if (v.GroupID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadTheme(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("MASTER_REF");
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;
            int themeIndex = CProductFilter.GetThemeIndex();

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            for (int i = themeIndex; i <= themeIndex; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = ThemeToString(i);

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static String ThemeToString(int themeid)
        {
            String key = "";

            if (themeid == 1)
            {
                key = "RainierOrange";
            }
            else if (themeid == 2)
            {
                key = "BubbleCreme";
            }
            else if (themeid == 3)
            {
                key = "ShinyBlue";
            }
            else if (themeid == 4)
            {
                key = "ExpressionDark";
            }
            else if (themeid == 5)
            {
                key = "LightingBlueUI";
            }
            else if (themeid == 6)
            {
                key = "LightingPinkUI";
            }

            return (key);
        }

        public static void LoadReportName(ReportGroupEnum grp, ComboBox cbo)
        {
            List<MMasterRef> items = new List<MMasterRef>();

            MMasterRef v = new MMasterRef(new CTable(""));
            items.Add(v);

            ArrayList arr = CReportFactory.GetReportArray(grp);
            foreach (MMasterRef mr in arr)
            {
                items.Add(mr);
            }

            cbo.ItemsSource = items;
        }

        public static CTable ApproveChequeFromAccountDoc(MAccountDoc doc)
        {
            CTable dat = new CTable("");

            Boolean approveCheque = CGlobalVariable.GetGlobalVariableValue("CHEQUE_APPROVE_IMMEDIATE_FLAG").Equals("Y");
            if (!approveCheque)
            {
                return(dat);
            }
            
            dat.SetFieldValue("CHEQUE_ID", doc.ChequeID);

            CTable cheque = OnixWebServiceAPI.SubmitObjectAPI("GetChequeInfo", dat);
            if (cheque != null)
            {
                cheque = OnixWebServiceAPI.SubmitObjectAPI("ApproveCheque", cheque);
            }

            return (cheque);
        }
    }
}
