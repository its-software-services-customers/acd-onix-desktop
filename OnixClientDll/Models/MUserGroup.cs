using System;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Helper;

using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MUserGroup : MBaseModel
    {
        public MUserGroup(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("user_group", GroupName);
            ttItems.Add(ct);

            ct = new CToolTipItem("description", Description);
            ttItems.Add(ct);
        }

        public String GroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("GROUP_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String Description
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DESCRIPTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DESCRIPTION", value);
                NotifyPropertyChanged();
            }
        }

        public String GroupID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("GROUP_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("GROUP_ID", value);
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<MModule> createTreeViewItemsTree(ArrayList items)
        {
            String prevKey = "";
            ObservableCollection<MModule> oitems = new ObservableCollection<MModule>();
            string[] stringSeparators = new string[] { "_" };
            MModule gp = null;

            foreach (CTable t in items)
            {
                String perm = t.GetFieldValue("PERM_NAME");
                string[] result = perm.Split(stringSeparators, StringSplitOptions.None);
                String key = result[0];

                if (!prevKey.Equals(key))
                {
                    gp = new MModule(t);
                    gp.ModuleName = key;

                    oitems.Add(gp);

                    prevKey = key;
                }

                MGroupPermission pm = new MGroupPermission(t);
                gp.AddGroupPermission(pm);
            }

            return (oitems);
        }

        public ObservableCollection<MModule> PermissionItems
        {
            get
            {
                ObservableCollection<MModule> items = new ObservableCollection<MModule>();
                CTable o = GetDbObject();
                if (o == null)
                {
                    return (items);
                }

                ArrayList perms = o.GetChildArray("PERM_ITEM");
                if (perms != null)
                {
                    items = createTreeViewItemsTree(perms);
                }
                return (items);
            }

            set
            {
            }
        }
    }
}
