using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Model;
using Onix.Client.Model.Sass;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.ClientCenter.Windows.Sass;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Criteria.Sass
{
    public class CCriteriaSassVM : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaSassVM() : base(new MInstance(new CTable("")), "CCriteriaSassVM")
        {
        }

        public override void Init(String type)
        {
            createCriteriaEntries();
            createGridColumns();
        }

        public override void SetActionEnable(Boolean en)
        {
            isActionEnable = en;
        }

        public override void SetCheckUncheckHandler(RoutedEventHandler chdler, RoutedEventHandler uhdler)
        {
            checkHandler = chdler;
            unCheckHandler = uhdler;
        }

        #region Criteria Configure

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);

            //CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuCopy", "copy", new RoutedEventHandler(mnuContextMenu_Click), 2);
            //contexts.Add(ct2);

            return (contexts);
        }

        private void cbxSelect_Checked(object sender, RoutedEventArgs e)
        {
            if (checkHandler != null)
            {
                checkHandler(sender, e);
            }
        }

        private void cbxSelect_UnChecked(object sender, RoutedEventArgs e)
        {
            if (unCheckHandler != null)
            {
                unCheckHandler(sender, e);
            }
        }

        private void createGridColumns()
        {
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left, cbxSelect_Checked, cbxSelect_UnChecked);
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            c0_1.SetButtonEnable(isActionEnable);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colVmName", "vm_name", "VmName", 25, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c1_1 = new CCriteriaColumnText("colProject", "project_name", "ProjectName", 15, HorizontalAlignment.Left);
            AddGridColumn(c1_1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colMachineType", "vm_type", "MachineType", 15, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colVmIP", "ip_address", "IPAddress", 10, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colImageFile", "vm_image_name", "ImageName", 15, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colStatus", "vm_status", "Status", 10, HorizontalAlignment.Left);
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "vm_name"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "VmName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "project_name"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ProjectName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "vm_image_name"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ImageName", ""));
        }

        private ArrayList createAddContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "ADMIN_EDIT", new RoutedEventHandler(cmdAdd_Click), 1);
            contexts.Add(ct1);

            return (contexts);
        }

        #endregion;

        #region Private

        private void showEditWindow()
        {
            if (!CHelper.VerifyAccessRight("SASS_VM_VIEW"))
            {
                return;
            }

            MProject v = (MProject)currentObj;

            WinAddEditInstance w = new WinAddEditInstance();
            w.Mode = "E";
            w.ViewData = v;
            w.Title = CLanguage.getValue("edit") + " " + CLanguage.getValue("project");
            w.ShowDialog();
        }

        #endregion

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, null);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            items = OnixWebServiceAPI.GetListAPI("SassGetInstanceList", "INSTANCE_LIST", model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MProject v = new MProject(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("SASS_VM_DELETE"))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteAPI, rc.ToString(), "SassDeleteInstance");

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("SASS_VM_ADD"))
            {
                return;
            }

            WinAddEditInstance w = new WinAddEditInstance();
            w.ParentItemSource = itemSources;
            w.Mode = "A";
            w.Title = (String)(sender as Button).Content + " " + CLanguage.getValue("project");
            w.ShowDialog();
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            currentObj = (MBaseModel)(sender as UActionButton).Tag;
        }

        private void mnuContextMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuEdit"))
            {
                showEditWindow();
            }
            else if (name.Equals("mnuCopy"))
            {
                //CUtil.EnableForm(false, ParentControl);
                //CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("CopyProject", currentObj.GetDbObject());

                //if (newobj != null)
                //{
                //    MProject ivd = new MProject(newobj);
                //    itemSources.Insert(0, ivd);
                //}
                //else
                //{
                //    //Error here
                //    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                //}

                //CUtil.EnableForm(true, ParentControl);
            }
        }
#endregion
    }
}
