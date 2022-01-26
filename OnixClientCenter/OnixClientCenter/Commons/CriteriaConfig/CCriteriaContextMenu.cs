using System;

namespace Onix.ClientCenter.Commons.CriteriaConfig
{
    public class CCriteriaContextMenu
    {
        private String nm = "";
        private String cption = "";
        private Delegate func = null;
        private int grp = 0;

        public CCriteriaContextMenu(String name, String caption, Delegate clickEvent, int groupNo)
        {
            func = clickEvent;
            cption = caption;
            nm = name;
            grp = groupNo;
        }

        public String Name
        {
            get
            {
                return (nm);
            }
        }

        public String Caption
        {
            get
            {
                return (cption);
            }
        }

        public int Group
        {
            get
            {
                return (grp);
            }
        }

        public Delegate ClickHandler
        {
            get
            {
                return (func);
            }
        }
    }
}
