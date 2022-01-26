using System;
using System.Windows;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Commons.Utils
{
    public enum EntryType
    {
        ENTRY_TEXT_BOX = 1,
        ENTRY_DATE_MIN,
        ENTRY_DATE_MAX,
        ENTRY_COMBO_BOX,
        ENTRY_MONTH_YEAR,
        ENTRY_CHECK_BOX
    }

    public class CEntry
    {
        public String LabelContentKey = "";
        public int EntryWidth = 100;
        public int EntryHeight = 40;
        public EntryType EntryType = EntryType.ENTRY_TEXT_BOX;
        public Boolean NullAllowed = true;
        public String FieldName = "";

        public UIElement ActualUI = null;
        public UIElement ActualLabel = null;

        public LoadReportComboDelegate ComboLoaderFunction;
        public SetupReportComboDelegate ComboSetupFunction;
        public ObjectToIDDelegate ObjectToIndexFunction;

        public CEntry(String label, EntryType type, int width, Boolean nullallow, String fieldName)
        {
            NullAllowed = nullallow;
            LabelContentKey = label;
            EntryWidth = width;
            EntryType = type;
            FieldName = fieldName;

            if ((type == EntryType.ENTRY_DATE_MIN) || (type == EntryType.ENTRY_DATE_MAX))
            {
                SetHeight(45);
            }
            else if (type == EntryType.ENTRY_CHECK_BOX)
            {
                SetHeight(30);
            }
        }

        public void SetHeight(int h)
        {
            EntryHeight = h;
        }

        public void SetComboLoadAndInit(LoadReportComboDelegate loadFunc, SetupReportComboDelegate setupFunc,
            ObjectToIDDelegate mappingFunc)
        {
            ComboLoaderFunction = loadFunc;
            ComboSetupFunction = setupFunc;
            ObjectToIndexFunction = mappingFunc;
        }
    }
}
