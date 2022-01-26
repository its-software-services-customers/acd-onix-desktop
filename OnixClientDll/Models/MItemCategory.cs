using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MItemCategory : MBaseModel
    {
        private List<MItemCategory> children = new List<MItemCategory>();
        private ObservableCollection<MItemCategory> childrenNodes = new ObservableCollection<MItemCategory>();

        public MItemCategory(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }

        public ObservableCollection<MItemCategory> ChildrenNodes
        {
            get
            {
                return (childrenNodes);
            }

            set
            {
                childrenNodes = value;
            }
        }

        public List<MItemCategory> Children
        {
            get
            {
                return (children);
            }

            set
            {
                children = value;
            }
        }

        public String ItemCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_COUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String ItemCategoryID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_CATEGORY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_CATEGORY_ID", value);
                NotifyPropertyChanged();
            }
        }

        //public String ItemCategoryCode
        //{
        //    get
        //    {
        //        if (GetDbObject() == null)
        //        {
        //            return ("");
        //        }

        //        return (GetDbObject().GetFieldValue("ITEM_CATEGORY_ID"));
        //    }

        //    set
        //    {
        //        GetDbObject().SetFieldValue("ITEM_CATEGORY_ID", value);
        //        NotifyPropertyChanged();
        //    }
        //}

        public String ParentID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PARENT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PARENT_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String CategoryName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CATEGORY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CATEGORY_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String TempCategoryName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP_CATEGORY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP_CATEGORY_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String TempNameParentID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP_NAME_PARENT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP_NAME_PARENT_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String ChildCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHILD_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHILD_COUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String Caption
        {
            get
            {
                int icnt = CUtil.StringToInt(ItemCount);

                if (icnt > 0)
                {
                    return (String.Format("{0} ({1})", CategoryName, icnt));
                }

                return (CategoryName);
            }

            set
            {
            }
        }

        public Boolean IsParent
        {
            get
            {
                int ccnt = CUtil.StringToInt(ChildCount);

                return (ccnt > 0);
            }
        }

        //public static explicit operator VItemCategory(DataRowView v)
        //{
        //    throw new NotImplementedException();
        //}

        public MItemCategory ItemCategoryObj
        {
            set
            {
                MItemCategory m = value;
                if (m != null)
                {
                    ItemCategoryID = m.ItemCategoryID;
                    CategoryName = m.CategoryName;
                }
            }
        }

        public String Path
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PATH"));
            }

            set
            {
                GetDbObject().SetFieldValue("PATH", value);
                NotifyPropertyChanged();
            }
        }


        public Object IconTreeView
        {
            get
            {
                //if (ExtFlag.Equals("A"))
                //{
                //    return ("pack://application:,,,/OnixClient;component/Images/add-icon-16.png");
                //}

                return ("pack://application:,,,/OnixClient;component/Images/add-icon-16.png");

            }

            set { }
        }
    }
}
