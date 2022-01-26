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
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Criteria
{
    public class CCriteriaPromotion : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String groupID = "";

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaPromotion() : base(new MPackage(new CTable("")), "CCriteriaPromotion")
        {
        }

        public override void Init(String type)
        {
            groupID = type;

            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
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

        private void loadRelatedReferences()
        {
            if (!CMasterReference.IsPackageTypeLoad())
            {
                CMasterReference.LoadPackageType();
            }
        }

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

            CCriteriaColumnText c1 = new CCriteriaColumnText("colPackageCode", "package_code", "PackageCode", 13, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colPackageName", "package_name", "PackageName", 35, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnImageText c3 = new CCriteriaColumnImageText("colPackageType", "package_type", "PackageTypeName", "PackageTypeIcon", 17, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colEffectiveDate", "effective_date", "EffectiveDateFmt", 10, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colExpireDate", "expire_date", "ExpireDateFmt", 10, HorizontalAlignment.Left);
            AddGridColumn(c5);

            CCriteriaColumnImageText c6 = new CCriteriaColumnImageText("colIsEnable", "is_enable", "", "IsEnabledIcon", 5, HorizontalAlignment.Left);
            AddGridColumn(c6);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "package_code"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "PackageCode", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "package_name"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "PackageName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "package_type"));

            if (groupID.Equals(""))
            {
                CCriteriaEntry typeEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "PackageTypeObj", "");
                typeEntry.SetComboItemSources("PackageTypes", "Description");
                AddCriteriaControl(typeEntry);
            }

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsEnabled", "is_enable"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "day_effective"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "DayEffective", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "day_expire"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "DayExpire", ""));
        }

        private ArrayList createAddContextMenu()
        {
            ArrayList contexts = new ArrayList();

            RoutedEventHandler evt = new RoutedEventHandler(cmdPromotionAdd_Click);

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuTrayBonus", "tray_package_bonus", evt, 1);
            contexts.Add(ct1);
            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuTrayBundle", "tray_package_bundle", evt, 1);
            contexts.Add(ct2);

            CCriteriaContextMenu ct2_1 = new CCriteriaContextMenu("mnuTrayPrice", "tray_package_price", evt, 2);
            contexts.Add(ct2_1);

            CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuBonus", "bonus_package", evt, 3);
            contexts.Add(ct3);
            CCriteriaContextMenu ct4 = new CCriteriaContextMenu("mnuVoucher", "voucher_package", evt, 3);
            contexts.Add(ct4);
            CCriteriaContextMenu ct5 = new CCriteriaContextMenu("mnuBundle", "bundle_package", evt, 3);
            contexts.Add(ct5);

            CCriteriaContextMenu ct6 = new CCriteriaContextMenu("mnuPricing", "pricing_package", evt, 4);
            contexts.Add(ct6);

            CCriteriaContextMenu ct7 = new CCriteriaContextMenu("mnuDiscount", "discount_package", evt, 5);
            contexts.Add(ct7);

            CCriteriaContextMenu ct8 = new CCriteriaContextMenu("mnuFinalDiscount", "final_discount_package", evt, 6);
            contexts.Add(ct8);

            CCriteriaContextMenu ct9 = new CCriteriaContextMenu("mnuPostGift", "post_gift", evt, 7);
            contexts.Add(ct9);

            return (contexts);
        }

        #endregion;

        #region Private

        private void showEditWindow()
        {
            if (!CHelper.VerifyAccessRight("PROMOTION_PROMOTION_VIEW"))
            {
                return;
            }

            String pkgGroup = (currentObj as MPackage).PackageType;

            WinAddEditPackage w = new WinAddEditPackage(pkgGroup);
            w.Title = CLanguage.getValue("edit") + " " + CUtil.PackageTypeToString(pkgGroup);
            w.ViewData = (MPackage) currentObj;
            w.Mode = "E";
            w.ParentItemSource = itemSources;
            w.ShowDialog();
        }

        #endregion

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, cmdAdd_Click);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            (model as MPackage).PackageGroup = groupID;
            items = OnixWebServiceAPI.GetPackageList(model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MPackage v = new MPackage(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("PROMOTION_PROMOTION_DELETE"))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeletePackage, rc.ToString());

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }

        #region Event Handler

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

        private void cmdPromotionAdd_Click(object sender, RoutedEventArgs e)
        {
            String pkgGroup = "1";
            Boolean ifMatched = false;

            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuPricing"))
            {
                pkgGroup = "1";
                ifMatched = true;
            }
            else if (name.Equals("mnuBonus"))
            {
                pkgGroup = "2";
                ifMatched = true;
            }
            else if (name.Equals("mnuDiscount"))
            {
                pkgGroup = "3";
                ifMatched = true;
            }
            else if (name.Equals("mnuVoucher"))
            {
                pkgGroup = "4";
                ifMatched = true;
            }
            else if (name.Equals("mnuBundle"))
            {
                pkgGroup = "5";
                ifMatched = true;
            }
            else if (name.Equals("mnuFinalDiscount"))
            {
                pkgGroup = "6";
                ifMatched = true;
            }
            else if (name.Equals("mnuPostGift"))
            {
                pkgGroup = "7";
                ifMatched = true;
            }
            else if (name.Equals("mnuTray"))
            {
                pkgGroup = "8";
                ifMatched = true;
            }
            else if (name.Equals("mnuTrayBonus"))
            {
                pkgGroup = "9";
                ifMatched = true;
            }
            else if (name.Equals("mnuTrayBundle"))
            {
                pkgGroup = "10";
                ifMatched = true;
            }

            if (ifMatched)
            {
                WinAddEditPackage w = new WinAddEditPackage(pkgGroup);
                w.Title = CLanguage.getValue("add") + " " + CUtil.PackageTypeToString(pkgGroup);
                w.Mode = "A";
                w.ParentItemSource = itemSources;
                w.ShowDialog();
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
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
                CTable newobj = OnixWebServiceAPI.CopyPackage(currentObj.GetDbObject());

                if (newobj != null)
                {
                    MPackage ivd = new MPackage(newobj);
                    ItemAddedEvent(ivd, e);
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
