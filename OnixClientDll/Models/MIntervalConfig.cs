using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Resources;

namespace Onix.Client.Model
{
    public class MIntervalConfig : MBaseModel
    {
        private ObservableCollection<MInterval> intervals = new ObservableCollection<MInterval>();
        private ObservableCollection<MMasterRef> stepSelectionMap = new ObservableCollection<MMasterRef>();
        private ObservableCollection<MMasterRef> tierSelectionMap = new ObservableCollection<MMasterRef>();
        private ObservableCollection<MMasterRef> mappingTypeMap = new ObservableCollection<MMasterRef>();

        public MIntervalConfig(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

        }

        #region Interval
        public ObservableCollection<MInterval> IntervalItems
        {
            get
            {
                return (intervals);
            }

            set
            {
            }
        }

        private MInterval createNewInterval()
        {
            int idx = intervals.Count - 1;
            MInterval latest = intervals[idx];
            double gap = CUtil.StringToDouble(latest.ToValue) - CUtil.StringToDouble(latest.FromValue);

            MInterval v = new MInterval(new CTable(""));
            v.IsNotFirst = true;
            v.FromValue = latest.ToValue;
            double nvalue = CUtil.StringToDouble(v.FromValue) + gap;
            v.ToValue = nvalue.ToString();

            return (v);
        }

        private void arrangeInterval()
        {
            int idx = 0;
            MInterval prev = null;

            foreach (MInterval iv in intervals)
            {
                if (idx >= 1)
                {
                    double from = CUtil.StringToDouble(iv.FromValue);
                    double to = CUtil.StringToDouble(iv.ToValue);
                    double gap = to - from;

                    iv.FromValue = prev.ToValue;

                    double nfrom = CUtil.StringToDouble(iv.FromValue);

                    if (nfrom >= to)
                    {
                        iv.ToValue = (nfrom + gap).ToString();
                    }
                }

                prev = iv;

                idx++;
            }
        }

        public void AddInterval()
        {
            MInterval v = null;

            if (intervals.Count <= 0)
            {
                v = new MInterval(new CTable(""));
                v.IsNotFirst = false;
                v.FromValue = "0";
                v.ToValue = "10";
            }
            else
            {
                v = createNewInterval();
            }

            intervals.Add(v);
            arrangeInterval();
        }

        public void RemoveInterval(MInterval vi)
        {
            intervals.Remove(vi);
            arrangeInterval();
        }

        public void ArrangeInterval()
        {
            arrangeInterval();
        }

        #endregion Interval

        public ObservableCollection<MMasterRef> StepSelectionMap
        {
            set
            {
                stepSelectionMap = value;
            }
        }

        private MMasterRef getObjectByID(ObservableCollection<MMasterRef> coll, int id)
        {
            foreach (MMasterRef m in coll)
            {
                if (m.MasterID.Equals(id.ToString()))
                {
                    return (m);
                }
            }

            return (null);
        }

        public MMasterRef StepSelectedObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value;
                StepScopeType = CUtil.StringToInt(m.MasterID);
            }

            get
            {
                MMasterRef m = getObjectByID(stepSelectionMap, StepScopeType);
                return (m);
            }
        }

        public MMasterRef TierSelectedObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value;
                TierScopeType = CUtil.StringToInt(m.MasterID);
            }

            get
            {
                MMasterRef m = getObjectByID(tierSelectionMap, TierScopeType);
                return (m);
            }
        }

        public MMasterRef MappingTypeSelectedObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value;
                MappingType = CUtil.StringToInt(m.MasterID);
            }

            get
            {
                MMasterRef m = getObjectByID(mappingTypeMap, MappingType);
                return (m);
            }
        }

        public ObservableCollection<MMasterRef> TierSelectionMap
        {
            set
            {
                tierSelectionMap = value;
            }
        }

        public ObservableCollection<MMasterRef> MappingTypeMap
        {
            set
            {
                mappingTypeMap = value;
            }
        }

        public String IntervalConfigID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INTERVAL_CONFIG_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("INTERVAL_CONFIG_ID", value);
            }
        }

        public String Name
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("NAME", value);
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
            }
        }

        /* 1 = Step, 2 = tier */
        public int SelectionType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (1);
                }

                return (CUtil.StringToInt(GetDbObject().GetFieldValue("SELECTION_TYPE")));
            }

            set
            {
                GetDbObject().SetFieldValue("SELECTION_TYPE", value.ToString());
                NotifyPropertyChanged();
            }
        }

        /* 1 = By Quantity, 2 = By Amount */
        public int MappingType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (1);
                }

                return (CUtil.StringToInt(GetDbObject().GetFieldValue("MAPPING_TYPE")));
            }

            set
            {
                GetDbObject().SetFieldValue("MAPPING_TYPE", value.ToString());
                NotifyPropertyChanged();
            }
        }

        public int StepScopeType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (0);
                }

                return (CUtil.StringToInt(GetDbObject().GetFieldValue("STEP_SCOPE_TYPE")));
            }

            set
            {
                GetDbObject().SetFieldValue("STEP_SCOPE_TYPE", value.ToString());
                NotifyPropertyChanged();
            }
        }

        public int TierScopeType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (0);
                }

                return (CUtil.StringToInt(GetDbObject().GetFieldValue("TIER_SCOPE_TYPE")));
            }

            set
            {
                GetDbObject().SetFieldValue("TIER_SCOPE_TYPE", value.ToString());
                NotifyPropertyChanged();
            }
        }

        public String ConfigString
        {
            get
            {
                return (serializeConfig());
            }

            set
            {

            }
        }

        #region Method

        public void DeserializeConfig(String cfgString)
        {
            if (cfgString.Equals(""))
            {
                return;
            }

            string[] words = cfgString.Split('|');

            if (words.Length < 5)
            {
                return;
            }

            //Name = words[0];
            Description = words[1];
            SelectionType = CUtil.StringToInt(words[2]);
            StepScopeType = CUtil.StringToInt(words[3]);
            TierScopeType = CUtil.StringToInt(words[4]);

            MappingType = 1;
            if (words.Length >= 7)
            {
                MappingType = CUtil.StringToInt(words[6]);
            }

            String details = words[5];
            string[] rows = details.Split(';');

            intervals.Clear();
            if (!details.Equals(""))
            {
                foreach (string r in rows)
                {
                    string[] flds = r.Split(':');

                    MInterval vi = new MInterval(new CTable(""));
                    vi.FromValue = flds[0];
                    vi.ToValue = flds[1];
                    vi.ConfigValue = flds[2];

                    intervals.Add(vi);
                }
            }
        }

        private String serializeConfig()
        {
            String master = String.Format("{0}|{1}|{2}|{3}|{4}", Name, Description, SelectionType, StepScopeType, TierScopeType);

            String details = "";
            int idx = 0;
            foreach (MInterval iv in intervals)
            {
                if (iv.ConfigValue.Trim().Equals(""))
                {
                    iv.ConfigValue = "0.00";
                }

                String row = String.Format("{0}:{1}:{2}", iv.FromValue, iv.ToValue, iv.ConfigValue);

                if (idx == 0)
                {
                    details = row;
                }
                else
                {
                    details = String.Format("{0};{1}", details, row);
                }

                idx++;
            }

            return (String.Format("{0}|{1}|{2}", master, details, MappingType));
        }

        #endregion
    }
}
