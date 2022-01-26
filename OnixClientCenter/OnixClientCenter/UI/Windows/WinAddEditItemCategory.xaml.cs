using System;
using System.Windows.Controls;
using System.Windows;
using Wis.WsClientAPI;
using System.Data;
using System.Collections;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditItemCategory : Window
    {
        private MItemCategory vw = null;
        private MItemCategory actualView = null;

        private Boolean isModified = false;

        public String Caption = "";
        public String Mode = "";

        public DataRowView RowView = null; //For Delete

        public WinAddEditItemCategory()
        {
            InitializeComponent();
        }

        public MBaseModel ViewData
        {
            set
            {
                actualView = (MItemCategory)value;
            }
        }


        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string CapOpt = string.Empty;
            txtItemCategoryName.Focus();

            CTable t = new CTable("ITEM_CATEGORY");
            vw = new MItemCategory(t);
            vw.CreateDefaultValue();

            DataContext = vw;

            CUtil.EnableForm(false, this);

            if (actualView != null)
            {               
                if (Mode.Equals("A"))
                {
                    vw.ParentID = actualView.ItemCategoryID.ToString();
                    vw.ItemCount = "0".ToString();

                    CTable MItemCategory = new CTable("ITEM_CATEGORY");
                    MItemCategory.SetFieldValue("ITEM_CATEGORY_ID", actualView.ItemCategoryID.ToString());
                    ArrayList arr = OnixWebServiceAPI.GetItemCategoryList(MItemCategory);

                    foreach (CTable a in arr)
                    {
                        vw.TempNameParentID = a.GetFieldValue("CATEGORY_NAME");
                        vw.ChildCount = a.GetFieldValue("CHILD_COUNT");
                    }
                    CapOpt = "(" +  vw.TempNameParentID + ")";
                }
                else
                {
                    vw.ItemCategoryID = actualView.ItemCategoryID.ToString();
                    vw.ItemCount = actualView.ItemCount.ToString();
                    vw.CategoryName = actualView.CategoryName.ToString();
                }
            }
            this.Title = Caption + CapOpt;

            isModified = false;

            CUtil.EnableForm(true, this);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean IsSave = SaveData();
            if (IsSave)
            {
                isModified = false;
                CUtil.EnableForm(true, this);

                DialogResult = true;

                this.Close();
            }
        }

        private Boolean SaveData()
        {
            if (!CHelper.VerifyAccessRight("ITEM_CATEGORY_EDIT"))
                return (false);

            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    CUtil.EnableForm(false, this);
                    CTable newobj = OnixWebServiceAPI.CreateItemCategory(vw.GetDbObject());
                    
                    if (newobj != null)
                    {
                        vw.SetDbObject(newobj);
                        vw.ChildCount = "0"; //Always
                        CMasterReference.Instance.AddCategoryToTree(vw);

                        CMasterReference.LoadItemCategoryPathList(true, null);
                        CUtil.EnableForm(true, this);
                        return (true);
                    }

                    //Error here
                    CUtil.EnableForm(true, this);
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                    return (false);
                }
            }
            else if (Mode.Equals("E"))
            {
                if (isModified)
                {
                    Boolean result = SaveToView();
                    if (result)
                    {
                        CUtil.EnableForm(false, this);
                        actualView.CategoryName = vw.CategoryName.ToString();
                        CTable t = OnixWebServiceAPI.UpdateItemCategory(actualView.GetDbObject());                        
                        if (t != null)
                        {
                            actualView.SetDbObject(t);
                            CMasterReference.Instance.EditCategoryInTree(actualView);

                            CMasterReference.LoadItemCategoryPathList(true, null);
                            CUtil.EnableForm(true, this);
                            return (true);
                        }

                        CUtil.EnableForm(true, this);
                        CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_EDIT", null);
                    }

                    return (false);
                }

                return (true);
            }

            return (false);
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isModified = true;
        }

        private Boolean SaveToView()
        {
            if (!ValidateData())
                return (false);

            return (true);
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblItemCategoryName, txtItemCategoryName, false);
            if (!result)
            {
                return (result);
            }

            CTable ug = new CTable("ITEM_CATEGORY");
            MItemCategory uv = new MItemCategory(ug);
            if (vw != null)
            {
                uv.ItemCategoryID = (vw as MItemCategory).ItemCategoryID;
                uv.CategoryName = (vw as MItemCategory).CategoryName;
            }

            if (OnixWebServiceAPI.IsItemCategoryExist(uv.GetDbObject()))
            {
                CHelper.ShowKeyExist(lblItemCategoryName, txtItemCategoryName);
                return (false);
            }

            return (result);
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
