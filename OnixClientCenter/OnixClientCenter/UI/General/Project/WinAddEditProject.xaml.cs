using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.General.Project
{
    public partial class WinAddEditProject : WinBase
    {
        private MProject mv = null;

        public WinAddEditProject(CWinLoadParam param) : base(param)
        {
            accessRightName = "GENERAL_PROJECT_EDIT";

            createAPIName = "CreateProject";
            updateAPIName = "UpdateProject";
            getInfoAPIName = "GetProjectInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblCode, txtCode, false);
            registerValidateControls(lblName, txtName, false);
        }

        protected override MBaseModel createObject()
        {
            mv = new MProject(new CTable(""));
            mv.CreateDefaultValue();
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
    }
}
