
using System;
using Onix.Client.Model;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.Commons.CriteriaConfig
{
    public class MCriteriaColumn : MBaseModel
    {
        public MCriteriaColumn() : base(new CTable(""))
        {
        }

        public MCriteriaColumn(CTable obj) : base(obj)
        {
        }

        public String SeqNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SEQ_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("SEQ_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String ColumnName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COLUMN_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("COLUMN_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String ColumnKey
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COLUMN_KEY"));
            }

            set
            {
                GetDbObject().SetFieldValue("COLUMN_KEY", value);
                NotifyPropertyChanged();
            }
        }

        public String ColumnText
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COLUMN_TEXT"));
            }

            set
            {
                GetDbObject().SetFieldValue("COLUMN_TEXT", value);
                NotifyPropertyChanged();
            }
        }

        public String IsUsedForSortingFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IS_USED_FOR_SORTING"));
            }

            set
            {
                GetDbObject().SetFieldValue("IS_USED_FOR_SORTING", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsUsedForSorting
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (IsUsedForSortingFlag.Equals("Y"));
            }

            set
            {
                if (value)
                {
                    IsUsedForSortingFlag = "Y";
                }
                else
                {
                    IsUsedForSortingFlag = "N";
                }

                NotifyPropertyChanged();
            }
        }

        public String SortingOrderBy
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ORDER_BY"));
            }

            set
            {
                GetDbObject().SetFieldValue("ORDER_BY", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsSortByDesc
        {
            get
            {
                return (SortingOrderBy.Equals("DESC"));
            }

            set
            {
                if (value)
                {
                    SortingOrderBy = "DESC";
                }
            }
        }

        public Boolean IsSortByAsc
        {
            get
            {
                return (SortingOrderBy.Equals("ASC"));
            }

            set
            {
                if (value)
                {
                    SortingOrderBy = "ASC";
                }
            }
        }
    }
}
