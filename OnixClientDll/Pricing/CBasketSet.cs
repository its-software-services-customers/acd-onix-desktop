using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using Onix.Client.Helper;

namespace Onix.Client.Pricing
{
    public class CBasketSet
    {
        private Hashtable baskets = new Hashtable();
        private ArrayList types = new ArrayList();
        private ObservableCollection<CBasket> childs = new ObservableCollection<CBasket>();
        private String direction = "";

        public CBasketSet()
        {
        }

        public ObservableCollection<CBasket> Items
        {
            get
            {
                return (childs);
            }

            set
            {
            }
        }

        public String Direction
        {
            get
            {
                return (direction);
            }

            set
            {
                direction = value;
            }
        }

        public int BasketSetSize
        {
            get
            {
                return (types.Count);
            }
        }

        public void AddBasket(CBasket b)
        {
            BasketTypeEnum type = b.BasketType;
            ArrayList arr = (ArrayList) baskets[type];
            if (arr == null)
            {
                arr = new ArrayList();
                baskets[type] = arr;
            }

            arr.Add(b);

            if (!types.Contains(type))
            {
                types.Add(type);
            }            
        }

        public CBasket GetBasket(BasketTypeEnum type, int idx)
        {
            ArrayList arr = (ArrayList)baskets[type];
            if (arr == null)
            {
                return (null);
            }

            CBasket b = (CBasket) arr[idx];

            return (b);
        }

        public ArrayList GetBasketTypes()
        {
            return(types);
        }

        public ArrayList GetAllBasketByType(BasketTypeEnum type)
        {
            ArrayList arr = (ArrayList)baskets[type];
            return (arr);
        }

        public void AddByArray(ArrayList baskets)
        {
            if (baskets == null)
            {
                return;
            }

            foreach (CBasket b in baskets)
            {
                AddBasket(b);
            }
        }

        public CBasketSet Clone()
        {
            CBasketSet output = new CBasketSet();
            
            foreach (BasketTypeEnum bt in types)
            {
                ArrayList arr = (ArrayList) baskets[bt];
                output.AddByArray(arr);
            }

            return (output);
        }

        public String Icon
        {
            get
            {
                if (direction.Equals("INPUT"))
                {

                    return ("pack://application:,,,/OnixClient;component/Images/X11-icon.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/Letter-I-icon.png");
            }
        }

        #region Method

        public void AddItem(CBasket bs)
        {
            childs.Add(bs);
        }

        public void ClearItem()
        {
            childs.Clear();
        }

        public Boolean ExpandSubTree
        {
            get
            {
                return (false);
            }

            set
            {
            }
        }

        #endregion
    }
}
