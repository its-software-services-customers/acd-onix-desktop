using System;
using Wis.WsClientAPI;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Helper;
using System.Windows;

namespace Onix.Client.Model
{
    public class MGlobalVariable : MBaseModel
    {
        private ObservableCollection<MGlobalVariable> items = new ObservableCollection<MGlobalVariable>();
        private int seq = 0;

        public MGlobalVariable(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("note", VariableDesc);
            ttItems.Add(ct);
        }

        public void CreateDefaultValue()
        {
        }

        public ObservableCollection<MGlobalVariable> Items
        {
            get
            {
                return (items);
            }
        }

        public void InitItem(GenericStringTypeFilterCallback requireFunc)
        {
            items.Clear();

            CTable o = GetDbObject();
            ArrayList narr = o.GetChildArray("GLOBAL_VARIABLE_ITEM");
            if (narr == null)
            {
                narr = new ArrayList();
                o.AddChildArray("GLOBAL_VARIABLE_ITEM", narr);
            }

            foreach (CTable t in narr)
            {                
                MGlobalVariable v = new MGlobalVariable(t);

                if (!requireFunc(v.VariableName))
                {
                    continue;
                }

                items.Add(v);
                v.ExtFlag = "I";
            }
        }

        public String GlobalVariableID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("GLOBAL_VARIABLE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("GLOBAL_VARIABLE_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String VariableName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VARIABLE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("VARIABLE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String VariableNameDesc
        {
            get
            {
                return (CLanguage.getValue(VariableName));
            }
        }

        public String VariableType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VARIABLE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("VARIABLE_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public Visibility ComboBoxVisibility
        {
            get
            {
                if (VariableType.Equals("3"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility TextBoxVisibility
        {
            get
            {
                if (!VariableType.Equals("3"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public String VariableTypeName
        {
            get
            {
                String type = "";
                if (VariableType.Equals("1"))
                {
                    type = "dataType_integer";
                }
                else if (VariableType.Equals("2"))
                {
                    type = "dataType_text";
                }
                else if (VariableType.Equals("3"))
                {
                    type = "dataType_boolean";
                }
                else if (VariableType.Equals("4"))
                {
                    type = "dataType_double";
                }
                return (CLanguage.getValue(type));
            }
        }

        public String VariableValue
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VARIABLE_VALUE"));
            }

            set
            {
                GetDbObject().SetFieldValue("VARIABLE_VALUE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String VariableDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VARIABLE_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("VARIABLE_DESC", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public int Seq
        {
            get
            {
                return (seq);
            }

            set
            {
                GetDbObject().SetFieldValue("INTERNAL_SEQ", value.ToString());
                seq = value;
            }
        }

    }
}
