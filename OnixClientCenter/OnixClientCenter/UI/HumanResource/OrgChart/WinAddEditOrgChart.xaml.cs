using System;
using System.Windows;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.HumanResource.OrgChart
{
    public partial class WinAddEditOrgChart : WinBase
    {
        private String category = "";
        private ObservableCollection<MVOrgChart> currentPaths = null;

        public WinAddEditOrgChart(CWinOrgChartLoadParam param) : base((CWinLoadParam) param)
        {
            category = param.GenericType;
            currentPaths = param.CurrentPaths;

            accessRightName = "HR_ORGCHART_EDIT";

            createAPIName = "CreateVirtualDirectory";
            updateAPIName = "UpdateVirtualDirectory";
            getInfoAPIName = "GetVirtualDirectoryInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblName, txtName, false);
            registerValidateControls(lblDesc, txtDesc, false);
        }

        public ObservableCollection<MVOrgChart> CurrentPath
        {
            get
            {
                return (currentPaths);
            }

            set
            {
            }
        }

        public String NameLabel
        {
            get
            {
                if (category.Equals("1"))
                {
                    return (CLanguage.getValue("department"));
                }
                else if (category.Equals("2"))
                {
                    return (CLanguage.getValue("position"));
                }

                return ("");
            }

            set
            {
            }
        }

        public new String Caption
        {
            get
            {
                if (loadParam.Mode.Equals("A"))
                {
                    return (CLanguage.getValue("add") + " " + NameLabel);
                }

                return (CLanguage.getValue("edit") + " " + NameLabel);
            }

            set
            {
            }
        }

        protected override MBaseModel createObject()
        {
            MVOrgChart mv = new MVOrgChart(new CTable(""));

            int cnt = currentPaths.Count;
            if ((currentPaths != null) && (cnt > 0))
            {
                MVOrgChart p = (MVOrgChart)currentPaths[cnt-1];
                mv.ParentDirectoryID = p.DirectoryID;
            }

            mv.Category = category;

            return (mv);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }
    }
}
