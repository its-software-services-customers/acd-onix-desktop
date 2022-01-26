using System;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using System.Reflection;

namespace Onix.Client.Model
{
    public class MFormConfig : MBaseModel
    {
        private Hashtable languageVarHash = new Hashtable();
        private ObservableCollection<MFormConfigVariable> defaultVars = new ObservableCollection<MFormConfigVariable>();

        public MFormConfig(CTable obj) : base(obj)
        {
        }

        #region Private
        private ObservableCollection<T> populateGlobalObject<T>(ObservableCollection<T> dests, String keyName, Hashtable hash)
        {
            ObservableCollection<T> items = (ObservableCollection<T>)hash["ALL"];
            if (items == null)
            {
                return (null);
            }

            Hashtable varHash = CUtil.ObserableCollectionToHash(dests, keyName);
            Hashtable commonHash = new Hashtable();

            ObservableCollection<T> nitems = new ObservableCollection<T>();

            int idx = 0;
            foreach (T v in items)
            {
                String key = (String)v.GetType().GetProperty(keyName).GetValue(v, null);

                if (!varHash.ContainsKey(key))
                {
                    nitems.Add(v);
                    idx++;
                }
                else
                {
                    nitems.Add((T)varHash[key]);
                    commonHash.Add(key, "");
                }
            }

            //Add the remainder from dests
            foreach (T v in dests)
            {
                String key = (String)v.GetType().GetProperty(keyName).GetValue(v, null);

                if (!commonHash.ContainsKey(key))
                {
                    nitems.Add(v);
                }
            }

            return (nitems);
        }

        private void init<T>(ArrayList childArrNames, ArrayList langNames, Hashtable hash, ObservableCollection<T> defVars, String keyFieldName)
        {
            hash.Clear();

            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            int idx = 0;
            foreach (String arrName in childArrNames)
            {
                String key = (String)langNames[idx];
                ObservableCollection<T> items = new ObservableCollection<T>();
                hash.Add(key, items);
                idx++;

                ArrayList arr = o.GetChildArray(arrName);
                if (arr != null)
                {
                    foreach (CTable obj in arr)
                    {
                        T v = (T) Activator.CreateInstance(typeof(T), new object[] { obj });
                        items.Add(v);
                    }
                }

                if (!key.Equals("ALL"))
                {
                    continue;
                }

                //Populate default global variables

                Hashtable varHash = CUtil.ObserableCollectionToHash(items, keyFieldName);

                int cnt = 0;
                foreach (T v in defVars)
                {
                    String k = (String)v.GetType().GetProperty(keyFieldName).GetValue(v, null);

                    if (!varHash.ContainsKey(k))
                    {
                        items.Insert(cnt, v);
                        cnt++;
                    }
                }
            }
        }

        private ObservableCollection<T> getCombinedCollection<T>(Hashtable hash, String fieldKeyName)
        {
            ObservableCollection<T> items = (ObservableCollection<T>)hash[Language];
            if (items == null)
            {
                items = new ObservableCollection<T>();
                hash.Add(Language, items);
            }

            ObservableCollection<T> nitems = items;
            if (!Language.Equals("ALL"))
            {
                nitems = populateGlobalObject(items, fieldKeyName, hash);
            }

            int i = 1;
            foreach (T v in nitems)
            {
                String scope = (String)v.GetType().GetProperty("Scope").GetValue(v, null);
                v.GetType().GetProperty("IsEditable").SetValue(v, scope.Equals(Language));
                v.GetType().GetProperty("ID").SetValue(v, i.ToString());
                i++;
            }

            return (nitems);
        }

        private Boolean isItemExist<T>(ObservableCollection<T> items, T item, String uniqKeyField)
        {
            foreach (T v in items)
            {
                String name = (String)v.GetType().GetProperty(uniqKeyField).GetValue(v, null);
                String itemName = (String)item.GetType().GetProperty(uniqKeyField).GetValue(item, null);

                String id = (String)v.GetType().GetProperty("ID").GetValue(v, null);
                String itemID = (String)item.GetType().GetProperty("ID").GetValue(item, null);

                if (name.Equals(itemName) && (!id.Equals(itemID)))
                {
                    return (true);
                }
            }

            return(false);
        }

        private void addConfigItem<T>(Hashtable hash, T v, String notifyProperty)
        {
            ObservableCollection<T> items = (ObservableCollection<T>)hash[Language];
            if (items == null)
            {
                items = new ObservableCollection<T>();
                hash.Add(Language, items);
            }

            items.Add(v);

            NotifyPropertyChanged(notifyProperty);
        }

        public void CopyProperties<T>(T source, T dest)
        {
            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                var value = source.GetType().GetProperty(prop.Name).GetValue(source, null);

                MethodInfo mi = dest.GetType().GetProperty(prop.Name).SetMethod;
                if (mi != null)
                {
                    //Set method available
                    dest.GetType().GetProperty(prop.Name).SetValue(dest, value);
                }
            }
        }

        private void overrideConfigItem<T>(Hashtable hash, T item, String fieldName, String notifyProperty)
        {
            ObservableCollection<T> items = (ObservableCollection<T>)hash[Language];

            item.GetType().GetProperty("OVerridedFlag").SetValue(item, "Y");
            String name = (String)item.GetType().GetProperty(fieldName).GetValue(item, null);

            T curr = default(T);
            Boolean isExist = false;
            foreach (T v in items)
            {
                String n = (String)v.GetType().GetProperty(fieldName).GetValue(v, null);
                if (n.Equals(name))
                {
                    curr = v;
                    isExist = true;
                    break;
                }
            }

            if (!isExist)
            {
                //Not exist
                items.Add(item);
            }
            else
            {
                CopyProperties<T>(item, curr);
            }
            
            NotifyPropertyChanged(notifyProperty);
        }

        private void removeConfigItem<T>(Hashtable hash, T v, String notifyProperty)
        {
            ObservableCollection<T> items = (ObservableCollection<T>)hash[Language];
            if (items == null)
            {
                return;
            }

            items.Remove(v);

            NotifyPropertyChanged(notifyProperty);
        }
        #endregion Private

        public void InitFormConfig()
        {
            ArrayList arrNames = new ArrayList() { "VARIABLES_ITEM_ALL", "VARIABLES_ITEM_EN", "VARIABLES_ITEM_TH" };
            ArrayList langNames = new ArrayList() { "ALL", "EN", "TH" };
            init<MFormConfigVariable>(arrNames, langNames, languageVarHash, defaultVars, "VariableName");
        }

        #region Variables
        public void SetConfigVariable(String varName, VariableType varType, String varValue)
        {
            MFormConfigVariable v = new MFormConfigVariable(new CTable(""));

            v.VariableName = varName;
            v.VariableType = varType.ToString();
            v.VariableValue = varValue;
            v.SystemVariableFlag = "Y";
            v.Scope = "ALL";

            defaultVars.Add(v);
        }

        public void AddConfigVariable(MFormConfigVariable v)
        {
            v.Scope = Language;
            addConfigItem(languageVarHash, v, "Variables");
        }

        public Boolean IsVariableExist(MFormConfigVariable v)
        {
            Boolean result = isItemExist(Variables, v, "VariableName");
            return (result);
        }

        public void RemoveConfigVariable(MFormConfigVariable v)
        {
            removeConfigItem(languageVarHash, v, "Variables");
        }

        public void OverrideConfigVariable(MFormConfigVariable v)
        {
            overrideConfigItem(languageVarHash, v, "VariableName", "Variables");
        }

        public ObservableCollection<MFormConfigVariable> Variables
        {
            get
            {
                ObservableCollection<MFormConfigVariable> items = getCombinedCollection<MFormConfigVariable>(languageVarHash, "VariableName");
                return (items);
            }
        }
        #endregion Variables
        
        #region Property

        public String ReportName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REPORT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("REPORT_NAME", value);
            }
        }

        public String ReportID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REPORT_CONFIG_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("REPORT_CONFIG_ID", value);
            }
        }

        public String Language
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("ALL");
                }

                String s = GetDbObject().GetFieldValue("LANGUAGE");
                if (s.Equals(""))
                {
                    s = "ALL";
                }

                return (s);
            }

            set
            {
                String s = value;
                if (s.Equals(""))
                {
                    s = "ALL";
                }

                GetDbObject().SetFieldValue("LANGUAGE", s);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsThai
        {
            get
            {
                return (Language.Equals("TH"));
            }

            set
            {
                if (value)
                {
                    Language = "TH";
                    NotifyPropertyChanged("Variables");
                }
            }
        }

        public Boolean IsEng
        {
            get
            {
                return (Language.Equals("EN"));
            }

            set
            {
                if (value)
                {
                    Language = "EN";
                    NotifyPropertyChanged("Variables");
                }
            }
        }

        public Boolean IsAll
        {
            get
            {
                return (Language.Equals("ALL"));
            }

            set
            {
                if (value)
                {
                    Language = "ALL";
                    NotifyPropertyChanged("Variables");
                }
            }
        }

        #endregion

    }
}
