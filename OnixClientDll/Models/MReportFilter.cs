using System;
using Wis.WsClientAPI;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Helper;
using System.Windows;

namespace Onix.Client.Model
{
    public class MReportFilter : MBaseModel
    {
        private ObservableCollection<MMasterRef> languages = new ObservableCollection<MMasterRef>();

        public MReportFilter(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();
        }

        public void CreateDefaultValue()
        {
        }

        public ObservableCollection<MMasterRef> Languages
        {
            get
            {
                return (languages);
            }
        }

        public void AddLanguage(MMasterRef m)
        {
            languages.Add(m);
        }

        public String Key
        {
            get
            {
                return (ReportGroup + "-" + ReportName);
            }
        }

        public String ReportFilterID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REPORT_FILTER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("REPORT_FILTER_ID", value);
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public String ReportGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REPORT_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("REPORT_GROUP", value);
                NotifyPropertyChanged();
            }
        }

        public String ReportSeq
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REPORT_SEQ"));
            }

            set
            {
                GetDbObject().SetFieldValue("REPORT_SEQ", value);
                NotifyPropertyChanged();
            }
        }

        public String ReportNs
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REPORT_NS"));
            }

            set
            {
                GetDbObject().SetFieldValue("REPORT_NS", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsNodeSelected { get; set; }

        public Boolean IsSelected
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (IsSelectedFlag.Equals("Y"));
            }

            set
            {
                if (value)
                {
                    IsSelectedFlag = "Y";
                }
                else
                {
                    IsSelectedFlag = "N";
                }
            }
        }

        public String IsSelectedFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IS_SELECTED"));
            }

            set
            {
                GetDbObject().SetFieldValue("IS_SELECTED", value);
                updateFlag();
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
    }
}
