using System;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MReportConfig : MBaseModel
    {
        private Hashtable fieldHash = new Hashtable();
        private double cm2inch = 0.393701;
        private double inch2dot = 96;

        private Hashtable defaultFields = new Hashtable();
        private ObservableCollection<MGlobalVariable> variables = new ObservableCollection<MGlobalVariable>();

        public MReportConfig(CTable obj) : base(obj)
        {
            defaultFields["Language"] = true;
            defaultFields["PageWidthCm"] = true;
            defaultFields["PageHeightCm"] = true;
            defaultFields["MarginLeftCm"] = true;
            defaultFields["MarginTopCm"] = true;
            defaultFields["MarginRightCm"] = true;
            defaultFields["MarginBottomCm"] = true;   
        }

        public MGlobalVariable GetConfig(String cfgName)
        {
            MGlobalVariable obj = (MGlobalVariable)fieldHash[cfgName];
            return (obj);
        }

        public String GetConfigValue(String cfgName)
        {
            MGlobalVariable obj = (MGlobalVariable)fieldHash[cfgName];
            if (obj == null)
            {
                return ("");
            }

            return (obj.VariableValue);
        }

        public String GetConfigValue(String cfgName, String defaultIfNull)
        {
            MGlobalVariable obj = (MGlobalVariable)fieldHash[cfgName];
            if (obj == null)
            {
                return (defaultIfNull);
            }

            return (obj.VariableValue);
        }

        public void SetConfigValue(String varName, String value, String type, String note)
        {
            MGlobalVariable obj = new MGlobalVariable(new CTable(""));

            obj.VariableName = varName;
            obj.VariableValue = value;
            obj.VariableType = type;
            obj.VariableDesc = note;

            fieldHash[varName] = obj;
        }

        public Hashtable GetFieldHash()
        {
            return (fieldHash);
        }

        public CTable GetParamObject()
        {
            CTable o = new CTable("");

            foreach (String key in fieldHash.Keys)
            {
                MGlobalVariable s = (MGlobalVariable)fieldHash[key];
                o.SetFieldValue(s.VariableName, s.VariableValue);
            }

            return (o);
        }

        public void PrepareForSaving()
        {
            CTable o = GetDbObject();
            ArrayList arr = new ArrayList();

            foreach (String key in fieldHash.Keys)
            {
                MGlobalVariable s = (MGlobalVariable) fieldHash[key];
                arr.Add(s.GetDbObject());
            }

            o.RemoveChildArray("REPORT_CONFIG_LIST");
            o.AddChildArray("REPORT_CONFIG_LIST", arr);            
        }

        public void InitReportConfig()
        {
            variables.Clear();

            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("REPORT_CONFIG_LIST");
            if (arr == null)
            {
                return;
            }

            foreach (CTable obj in arr)
            {
                MGlobalVariable v = new MGlobalVariable(obj);
                fieldHash[v.VariableName] = v;

                if (!defaultFields.ContainsKey(v.VariableName))
                {
                    variables.Add(v);
                }
            }
        }

        public void PopulateMissingValue(MReportConfig defCfg)
        {
            Hashtable hash = defCfg.GetFieldHash();

            foreach (String key in hash.Keys)
            {
                if (!fieldHash.ContainsKey(key))
                {
                    MGlobalVariable s = (MGlobalVariable)hash[key];
                    MGlobalVariable v = new MGlobalVariable(new CTable(""));

                    v.VariableName = s.VariableName;
                    v.VariableValue = s.VariableValue;
                    v.VariableType = s.VariableType;
                    v.VariableDesc = s.VariableDesc;

                    fieldHash[key] = v;
                }
            }
        }

        public void CopyValues(MReportConfig srcCfg)
        {
            Hashtable hash = srcCfg.GetFieldHash();

            ReportName = srcCfg.ReportName;
            ReportID = srcCfg.ReportID;

            variables.Clear();
            foreach (String key in hash.Keys)
            {
                MGlobalVariable s = (MGlobalVariable)hash[key];
                MGlobalVariable v = new MGlobalVariable(new CTable(""));

                v.VariableName = s.VariableName;
                v.VariableValue = s.VariableValue;
                v.VariableType = s.VariableType;
                v.VariableDesc = s.VariableDesc;

                fieldHash[key] = v;

                if (!defaultFields.ContainsKey(v.VariableName))
                {
                    variables.Add(v);
                }
            }
        }

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

        public ObservableCollection<MGlobalVariable> Variables
        {
            get
            {
                return (variables);
            }
        }

        #region Language
        public String Language
        {
            get
            {
                MGlobalVariable gb = GetConfig("Language");
                return (gb.VariableValue);
            }

            set
            {
                MGlobalVariable gb = GetConfig("Language");
                gb.VariableValue = value;
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
                }
            }
        }

        #endregion

        public String PageOrientation
        {
            get
            {
                MGlobalVariable gb = GetConfig("PAPER_ORIENTATION");
                return (gb.VariableValue);
            }

            set
            {
                MGlobalVariable gb = GetConfig("PAPER_ORIENTATION");
                gb.VariableValue = value.ToString();
            }
        }

        public String PaperType
        {
            get
            {
                MGlobalVariable gb = GetConfig("PAPER_TYPE");
                return (gb.VariableValue);
            }

            set
            {
                MGlobalVariable gb = GetConfig("PAPER_TYPE");
                gb.VariableValue = value.ToString();
            }
        }

        public double PageWidthCm
        {
            get
            {
                MGlobalVariable gb = GetConfig("PageWidthCm");
                return (CUtil.StringToDouble(gb.VariableValue));
            }

            set
            {
                MGlobalVariable gb = GetConfig("PageWidthCm");
                gb.VariableValue = value.ToString();
            }
        }

        public double PageHeightCm
        {
            get
            {
                MGlobalVariable gb = GetConfig("PageHeightCm");
                return (CUtil.StringToDouble(gb.VariableValue));
            }

            set
            {
                MGlobalVariable gb = GetConfig("PageHeightCm");
                gb.VariableValue = value.ToString();
            }
        }

        public double AreaWidthDot
        {
            get
            {
                return (PageWidthDot - MarginLeftDot - MarginRightDot);
            }
        }

        public double AreaHeightDot
        {
            get
            {
                return (PageHeightDot - MarginTopDot - MarginBottomDot);
            }
        }

        public double PageWidthDot
        {
            get
            {
                return (PageWidthCm * cm2inch * inch2dot);
            }
        }

        public double PageHeightDot
        {
            get
            {
                return (PageHeightCm * cm2inch * inch2dot);
            }
        }

        public double MarginLeftCm
        {
            get
            {
                MGlobalVariable gb = GetConfig("MarginLeftCm");
                return (CUtil.StringToDouble(gb.VariableValue));
            }

            set
            {
                MGlobalVariable gb = GetConfig("MarginLeftCm");
                gb.VariableValue = value.ToString();
            }
        }

        public double MarginTopCm
        {
            get
            {
                MGlobalVariable gb = GetConfig("MarginTopCm");
                return (CUtil.StringToDouble(gb.VariableValue));
            }

            set
            {
                MGlobalVariable gb = GetConfig("MarginTopCm");
                gb.VariableValue = value.ToString();
            }
        }

        public double MarginRightCm
        {
            get
            {
                MGlobalVariable gb = GetConfig("MarginRightCm");
                return (CUtil.StringToDouble(gb.VariableValue));
            }

            set
            {
                MGlobalVariable gb = GetConfig("MarginRightCm");
                gb.VariableValue = value.ToString();
            }
        }

        public double MarginBottomCm
        {
            get
            {
                MGlobalVariable gb = GetConfig("MarginBottomCm");
                return (CUtil.StringToDouble(gb.VariableValue));
            }

            set
            {
                MGlobalVariable gb = GetConfig("MarginBottomCm");
                gb.VariableValue = value.ToString();
            }
        }


        public double MarginLeftDot
        {
            get
            {
                return (MarginLeftCm * cm2inch * inch2dot);
            }
        }

        public double MarginTopDot
        {
            get
            {
                return (MarginTopCm * cm2inch * inch2dot);
            }
        }

        public double MarginRightDot
        {
            get
            {
                return (MarginRightCm * cm2inch * inch2dot);
            }
        }

        public double MarginBottomDot
        {
            get
            {
                return (MarginBottomCm * cm2inch * inch2dot);
            }
        }

        #region Branch
        public String BranchID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_ID", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String BranchCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_CODE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String BranchName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_NAME", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public MMasterRef BranchObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                BranchID = m.MasterID;
                BranchCode = m.Code;
                BranchName = m.Description;

                updateFlag();
                //NotifyPropertyChanged("Locations");
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> branches = CMasterReference.Instance.Branches;
                if (branches == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(branches, BranchID));
            }
        }
        #endregion Branch

        #region DateTime
        public DateTime? FromDocumentDate
        {
            get
            {
                String str = GetDbObject().GetFieldValue("FROM_DOCUMENT_DATE");
                if (str == "")
                {
                    return (null);
                }
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_DOCUMENT_DATE", str);
            }
        }

        public DateTime? ToDocumentDate
        {
            get
            {
                String str = GetDbObject().GetFieldValue("TO_DOCUMENT_DATE");
                if (str == "")
                {
                    return (null);
                }
                
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_DOCUMENT_DATE", str);
            }
        }
        #endregion
    }
}
