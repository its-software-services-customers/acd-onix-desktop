using System;

namespace Onix.ClientCenter.Commons.CriteriaConfig
{
    public enum CriteriaEntryType
    {
        ENTRY_TEXT_BOX = 1,
        ENTRY_DATE_MIN,
        ENTRY_DATE_MAX,
        ENTRY_COMBO_BOX,
        ENTRY_CHECK_BOX,
        ENTRY_LABEL
    }

    public class CCriteriaEntry
    {
        private CriteriaEntryType typ = CriteriaEntryType.ENTRY_LABEL;
        private String fldName = "";
        private String cption;
        private String cboDisplayItem = "";
        private String itemSourceName = "";
        private Boolean isEnable = true;

        public CCriteriaEntry(CriteriaEntryType type, String field, String caption)
        {
            typ = type;
            fldName = field;
            cption = caption;
        }

        public void SetComboItemSources(String collName, String display)
        {
            cboDisplayItem = display;
            itemSourceName = collName;
        }

        public CriteriaEntryType Type
        {
            get
            {
                return (typ);
            }
        }

        public String FieldName
        {
            get
            {
                return (fldName);
            }
        }

        public String CaptionKey
        {
            get
            {
                return (cption);
            }
        }

        public String ComboCollectionName
        {
            get
            {
                return (itemSourceName);
            }
        }

        public String ComboDisplayItem
        {
            get
            {
                return (cboDisplayItem);
            }
        }

        public Boolean IsEnable
        {
            get
            {
                return (isEnable);
            }

            set
            {
                isEnable = value;
            }
        }
    }
}
