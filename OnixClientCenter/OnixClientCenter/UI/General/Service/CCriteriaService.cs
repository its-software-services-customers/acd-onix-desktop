using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Model;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.ClientCenter.Commons.Utils;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Commons.Factories;

namespace Onix.ClientCenter.UI.General.Service
{
    public class CCriteriaService : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String category = "0";
        private String level = "1";

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        private Boolean isForMultiSelect = false;

        public CCriteriaService() : base(new MService(new CTable("")), "CCriteriaService")
        {
        }

        public Boolean IsForMultiSelect
        {
            get
            {
                return (isForMultiSelect);
            }

            set
            {
                isForMultiSelect = value;
            }
        }

        public override void Init(String type)
        {
            category = type;
            createCriteriaEntries();
            createGridColumns();
        }

        public void Init(String type, String lvel)
        {
            category = type;
            level = lvel;
            createCriteriaEntries();
            createGridColumns();
        }

        public override void Initialize(string keyword)
        {
            if (keyword.Equals("mnuService"))
            {
                Init("", ((int)ServiceLevel.ServiceLevelRegular).ToString());
            }
            else if (keyword.Equals("mnuServiceOther"))
            {
                Init("", ((int)ServiceLevel.ServiceLevelOther).ToString());
            }
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

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuCopy", "copy", new RoutedEventHandler(mnuContextMenu_Click), 2);
            contexts.Add(ct2);

            return (contexts);
        }

        private void createGridColumns()
        {
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left, cbxSelect_Checked, cbxSelect_UnChecked);
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            c0_1.SetButtonEnable(isActionEnable);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colServiceCode", "service_code", "ServiceCode", 15, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colServiceName", "service_name", "ServiceName", 45, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colServiceType", "service_type", "ServiceTypeName", 15, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colServiceUOM", "service_uom", "ServiceUOMName", 15, HorizontalAlignment.Left);
            AddGridColumn(c4);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "service_code"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ServiceCode", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "service_name"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ServiceName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "service_type"));

            if (!isForMultiSelect)
            {
                CCriteriaEntry svTypeEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "ServiceTypeObj", "");
                svTypeEntry.SetComboItemSources("ServiceTypes", "Description");
                AddCriteriaControl(svTypeEntry);

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "service_uom"));

                CCriteriaEntry svUomEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "ServiceUomObj", "");
                svUomEntry.SetComboItemSources("Uoms", "Description");
                AddCriteriaControl(svUomEntry);

                if (category.Equals("0") || category.Equals(""))
                {
                    AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "service_category"));

                    CCriteriaEntry svcCatEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "CategoryObj", "");
                    svcCatEntry.SetComboItemSources("ServiceCategories", "Description");
                    AddCriteriaControl(svcCatEntry);
                }

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsSalary", "is_salary"));
            }
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

        private void showEditWindow()
        {
            if (!CHelper.VerifyAccessRight("SALE_SERVICE_VIEW"))
            {
                return;
            }

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("service");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = level;
            param.Mode = "E";
            param.ActualView = currentObj;
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditService", param);
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
            MService svc = (MService) model;            
            if (!category.Equals("0") && !category.Equals(""))
            {
                svc.Category = category;
            }

            svc.ServiceLevel = level;

            items = OnixWebServiceAPI.GetServiceList(model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MService v = new MService(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("GENERAL_SERVICE_DELETE"))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteService, rc.ToString());

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
            if (!CHelper.VerifyAccessRight("GENERAL_SERVICE_ADD"))
            {
                return;
            }

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("service");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = level;
            param.Mode = "A";
            param.ActualView = currentObj;
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditService", param);
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
                CUtil.EnableForm(false, ParentControl);
                CTable newobj = OnixWebServiceAPI.CopyService(currentObj.GetDbObject());

                if (newobj != null)
                {
                    MService ivd = new MService(newobj);
                    itemSources.Insert(0, ivd);
                }
                else
                {
                    //Error here
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                }

                CUtil.EnableForm(true, ParentControl);
            }
        }
#endregion
    }
}
