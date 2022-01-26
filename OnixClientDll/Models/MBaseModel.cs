using System;
using System.Collections;
using System.ComponentModel;
using Wis.WsClientAPI;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using System.Windows.Media;

namespace Onix.Client.Model
{
    public class MBaseModel : INotifyPropertyChanged
    {
        private CTable dbobj = null;
        private int rindex = 0;
        private bool forDelete = false;
        private bool forAdd = false;
        private Boolean isModified = false;

        protected ObservableCollection<CToolTipItem> ttItems = new ObservableCollection<CToolTipItem>();

        public event PropertyChangedEventHandler PropertyChanged;

        public MBaseModel(CTable obj)
        {
            dbobj = obj;
        }

        public virtual void NotifyAllPropertiesChanged()
        {
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                this.NotifyPropertyChanged(prop.Name);
            }
        }

        public virtual Boolean IsAddMode
        {
            get
            {
                return (ExtFlag.Equals("A"));
            }

            set
            {
            }
        }

        public virtual Boolean IsModified
        {
            get
            {
                return (isModified);
            }

            set
            {
                isModified = value;
                NotifyPropertyChanged();
            }
        }

        public virtual void updateFlag()
        {
            if (!ExtFlag.Equals("A") && !ExtFlag.Equals("D"))
            {
                ExtFlag = "E";
            }
        }

        public virtual String ExtFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXT_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXT_FLAG", value);
                NotifyPropertyChanged("StateIcon");
            }
        }

        public Object StateIcon
        {
            get
            {
                if (ExtFlag.Equals("A"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/add-icon-16.png");
                }
                else if (ExtFlag.Equals("E"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/edit-icon-16.png");
                }
                else if (ExtFlag.Equals("D"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/delete-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/normal-icon-16.png");
            }

            set
            {
            }
        }

        public virtual String ItemSeqNo
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

        public virtual String ChunkNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXT_CHUNK_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXT_CHUNK_NO", value);
                NotifyPropertyChanged();
            }
        }

        public virtual String ChunkCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXT_CHUNK_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXT_CHUNK_COUNT", value);
                NotifyPropertyChanged();
            }
        }

        public virtual void createToolTipItems()
        {
        }

        public ObservableCollection<CToolTipItem> ToolTipItems
        {
            get
            {
                createToolTipItems();
                return (ttItems);
            }
        }

        public CTable GetDbObject()
        {
            return (dbobj);
        }

        public void SetDbObject(CTable obj)
        {
            dbobj = obj;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int RowIndex
        {
            get
            {
                return (rindex);
            }

            set
            {
                rindex = value;
            }
        }

        public MBaseModel ObjSelf
        {
            get
            {
                return (this);
            }
        }

        public bool IsSelectedForDelete
        {
            get
            {
                return (forDelete);
            }

            set
            {
                forDelete = value;
                NotifyPropertyChanged();
            }
        }

        public virtual bool IsEmpty
        {
            get
            {
                return (true);
            }

            set
            {

            }
        }

        public virtual bool IsEnabled
        {
            get; set;
        }

        public bool IsSelectedForAdd
        {
            get
            {
                return (forAdd);
            }

            set
            {
                forAdd = value;
            }
        }

        public virtual String ID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ID", value);
            }
        }

        public virtual SolidColorBrush RowColor
        {
            get
            {
                return (new SolidColorBrush(Colors.Blue));
            }
        }

        protected void SetFieldValue(String name, String value)
        {
            if (dbobj == null)
            {
                return;
            }

            dbobj.SetFieldValue(name, value);
        }

        protected String GetFieldValue(String name)
        {
            if (dbobj == null)
            {
                return ("");
            }

            return (dbobj.GetFieldValue(name));
        }

        public void AddChildArray(String arrName, CTable data)
        {
            ArrayList arr = dbobj.GetChildArray(arrName);
            if (arr == null)
            {
                arr = new ArrayList();
                dbobj.AddChildArray(arrName, arr);
            }

            arr.Add(data);
        }

        public ArrayList GetChildArray(String arrName)
        {
            ArrayList arr = dbobj.GetChildArray(arrName);
            if (arr == null)
            {
                arr = new ArrayList();
                dbobj.AddChildArray(arrName, arr);
            }

            return (arr);
        }

        public virtual void InitializeAfterLoaded()
        {
        }

        public virtual void InitializeAfterNotified()
        {
        }


        protected void ClearAssociateItems(String arrName)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray(arrName);

            if (arr == null)
            {
                return;
            }

            arr.Clear();
        }

        protected void removeAssociateItems(MBaseModel vp, String arrName, String seqField, String idField)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray(arrName);

            if (arr == null)
            {
                return;
            }

            CTable obj = vp.GetDbObject();
            String seq = obj.GetFieldValue(seqField);

            foreach (CTable t in arr)
            {
                String q = t.GetFieldValue(seqField);
                if (q.Equals(seq))
                {
                    if (obj.GetFieldValue(idField).Equals(""))
                    {
                        arr.Remove(t);
                        break;
                    }
                    else
                    {
                        obj.SetFieldValue("EXT_FLAG", "D");
                    }
                }
            }
        }
    }
}
