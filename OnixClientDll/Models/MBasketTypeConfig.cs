using System;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MBasketTypeConfig : MBaseModel
    {
        private ObservableCollection<MMasterRef> intervals = new ObservableCollection<MMasterRef>();
        BasketTypeEnum[] types1 = new BasketTypeEnum[6] {BasketTypeEnum.Priced, BasketTypeEnum.Discounted, BasketTypeEnum.Bundled,
            BasketTypeEnum.PricedTray, BasketTypeEnum.DiscountedTray, BasketTypeEnum.BundledTray};

        BasketTypeEnum[] types2 = new BasketTypeEnum[4] {BasketTypeEnum.Priced, BasketTypeEnum.Discounted,
            BasketTypeEnum.PricedTray, BasketTypeEnum.DiscountedTray};

        BasketTypeEnum[] types3 = new BasketTypeEnum[7] { BasketTypeEnum.Priced, BasketTypeEnum.Discounted, BasketTypeEnum.Bundled, BasketTypeEnum.FinalDiscounted,
            BasketTypeEnum.PricedTray, BasketTypeEnum.DiscountedTray, BasketTypeEnum.BundledTray};

        BasketTypeEnum[] types4 = new BasketTypeEnum[5] { BasketTypeEnum.Priced, BasketTypeEnum.Discounted, BasketTypeEnum.FinalDiscounted,
            BasketTypeEnum.PricedTray, BasketTypeEnum.DiscountedTray};

        public MBasketTypeConfig(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

        }

        public String BasketTypeConfigID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BASKET_TYPE_CONFIG_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BASKET_TYPE_CONFIG_ID", value);
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

        public ObservableCollection<MMasterRef> SelectedBaskets
        {
            get
            {
                return (intervals);
            }

            set
            {

            }
        }

        #region Method

        private void populateTypes(String cfgString, BasketTypeEnum[] types)
        {
            Hashtable tempHash = new Hashtable();
            string[] words = cfgString.Split('|');

            intervals.Clear();

            foreach (string r in words)
            {
                if (r.Equals(""))
                {
                    continue;
                }

                string[] flds = r.Split(':');

                MMasterRef vi = new MMasterRef(new CTable(""));
                vi.Code = flds[0];
                vi.Description = flds[1];
                vi.EnabledFlag = flds[2];

                tempHash.Add(vi.Code, vi);
            }

            foreach (BasketTypeEnum bt in types)
            {
                String key = bt.ToString();

                MMasterRef vi = null;
                if (tempHash.ContainsKey(key))
                {
                    vi = (MMasterRef)tempHash[key];
                    intervals.Add(vi);
                }
                else
                {
                    vi = new MMasterRef(new CTable(""));
                    vi.Code = key;
                    vi.Description = key;
                    vi.EnabledFlag = "N";

                    intervals.Add(vi);
                }
            }
        }

        public void DeserializeConfig(String cfgString, String type)
        {
            if (type.Equals("1"))
            {
                populateTypes(cfgString, types1);
            }
            else if (type.Equals("2"))
            {
                populateTypes(cfgString, types2);
            }
            else if (type.Equals("3"))
            {
                populateTypes(cfgString, types3);
            }
            else if (type.Equals("4"))
            {
                populateTypes(cfgString, types4);
            }
        }

        private String serializeConfig()
        {
            String details = "";
            int idx = 0;
            foreach (MMasterRef iv in intervals)
            {
                String row = String.Format("{0}:{1}:{2}", iv.Code, iv.Description, iv.EnabledFlag);

                if (idx == 0)
                {
                    details = row;
                }
                else
                {
                    details = String.Format("{0}|{1}", details, row);
                }

                idx++;
            }

            return (details);
        }

        #endregion
    }
}
