using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.Client.Controller;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.Commons.Windows
{
    public partial class WinTextIntellisense : Window
    {
        private double tp = 0.00;
        private double lf = 0.00;
        private int currIdx = 0;
        private TextSearchNameSpace nameSpace = TextSearchNameSpace.CustomerCodeNS;

        private ArrayList allItems = new ArrayList();
        private ObservableCollection<MMasterRef> items = new ObservableCollection<MMasterRef>();

        private Hashtable dicts = new Hashtable();
        private MasterRefEnum mrType = MasterRefEnum.MASTER_REF_UNDEF;
        private Boolean isShowDetail = false;
        private UTextBox parentCaller = null;
        private MBaseModel extraParam = null;

        public WinTextIntellisense(String position, TextBox source, Window win, TextSearchNameSpace ns, MasterRefEnum mrt, Boolean showDetail, UTextBox caller, MBaseModel extParam)
        {
            int offset = 10;

            parentCaller = caller;
            nameSpace = ns;
            mrType = mrt;
            isShowDetail = showDetail;
            extraParam = extParam;

            //https://social.msdn.microsoft.com/Forums/vstudio/en-US/281a8cdd-69a9-4a4a-9fc3-c039119af8ed/absolute-screen-coordinates-of-wpf-user-control?forum=wpf
            Point locationFromScreen = source.PointToScreen(new Point(0, 0));
            PresentationSource s = PresentationSource.FromVisual(win);
            Point targetPoints = s.CompositionTarget.TransformFromDevice.Transform(locationFromScreen);

            if (position.Equals("R"))
            {
                tp = targetPoints.Y;
                lf = targetPoints.X + source.ActualWidth + offset;
            }
            else if (position.Equals("B"))
            {
                tp = targetPoints.Y + source.ActualHeight + offset;
                lf = targetPoints.X;
            }

            InitializeComponent();
        }

        public double WindowWidth
        {
            get
            {
                if (isShowDetail)
                {
                    return (600);
                }

                return (300);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> FilterItems
        {
            get
            {
                return (items);
            }
        }

        public double T
        {
            get
            {
                return (tp);
            }
        }

        public double L
        {
            get
            {
                return (lf);
            }
        }

        public Boolean IsCheque
        {
            get
            {
                Boolean flag = (nameSpace == TextSearchNameSpace.ApChequeNS) || (nameSpace == TextSearchNameSpace.ArChequeNS);
                return (flag);
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
        }

        private void lsvFilter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvFilter.ActualWidth * 1) - 30;

            double[] ratios;
            if (isShowDetail)
            {
                if (IsCheque)
                {
                    ratios = new double[3] { 0.3, 0.5, 0.2 };
                }
                else
                {
                    ratios = new double[2] { 0.2, 0.8 };
                }
            }
            else
            {
                ratios = new double[2] { 1, 0 };
            }

            CUtil.ResizeGridViewColumns(lsvFilter.View as GridView, ratios, w);
        }

        private void createFilterArray(String text, ArrayList allItems)
        {
            items.Clear();

            foreach (MMasterRef m in allItems)
            {
                if (m.Code.Contains(text))
                {
                    items.Add(m);
                }
            }

            if (items.Count > 0)
            {
                //Assum the first one is the best match

                currIdx = 0;
                lsvFilter.SelectedIndex = currIdx;
                lsvFilter.ScrollIntoView(lsvFilter.SelectedItem);
            }
        }

        private void getFilterTextFromServer(String key, String text)
        {
            ArrayList arr = null;

            CUtil.EnableForm(false, this);

            if ((nameSpace == TextSearchNameSpace.ApChequeNS) || (nameSpace == TextSearchNameSpace.ArChequeNS))
            {
                MMasterRef bank = extraParam as MMasterRef;

                MCheque cheque = new MCheque(new CTable(""));
                cheque.ChequeNo = key;
                cheque.BankID = bank.MasterID;
                cheque.ChequeStatus = "1";

                //cheque.Direction = "1";
                //if (nameSpace == TextSearchNameSpace.ApChequeNS)
                //{
                //    cheque.Direction = "2";
                //}

                arr = OnixWebServiceAPI.GetChequeList(cheque.GetDbObject());
            }
            else if (nameSpace == TextSearchNameSpace.MicroServiceCodeNS)
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.Code = key;
                mr.Description = nameSpace.ToString();

                arr = OnixWebServiceAPI.GetListAPI("SassGetSearchTextList", "SEARCH_TEXT_LIST", mr.GetDbObject());
            }
            else
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.Code = key;
                mr.Description = nameSpace.ToString();
                mr.RefType = ((int)mrType).ToString();

                CTable t = mr.GetDbObject();
                arr = OnixWebServiceAPI.GetSearchTextList(t);    
            }

            CUtil.EnableForm(true, this);

            if (arr == null)
            {
                return;
            }

            ArrayList filters = new ArrayList();
            foreach (CTable o in arr)
            {
                MMasterRef m;
                if ((nameSpace == TextSearchNameSpace.ApChequeNS) || (nameSpace == TextSearchNameSpace.ArChequeNS))
                {
                    MCheque c = new MCheque(o);
                    m = new MMasterRef(new CTable(""));

                    m.Code = c.ChequeNo;
                    m.Description = c.PayeeName;
                    m.DescriptionEng = c.ChequeAmountFmt;
                }
                else
                {
                    m = new MMasterRef(o);
                }

                filters.Add(m);
            }

            dicts.Add(key, filters);
            createFilterArray(text, filters);
        }

        private void getFilterText(String text)
        {
            String ch = text.Substring(0, 1);
            if (!dicts.ContainsKey(ch))
            {
                //retrieve from server by creating another thread
                getFilterTextFromServer(ch, text);
                return;
            }

            ArrayList arr = (ArrayList) dicts[ch];
            //arr should not be null
            createFilterArray(text, arr);
        }

        public void RegisterText(String text)
        {            
            txtCurrentText.Text = text;
            if (text.Equals(""))
            {
                return;
            }

            getFilterText(text);
        }

        public String GetCurrentText()
        {
            MMasterRef mr = (MMasterRef) lsvFilter.SelectedItem;
            if (mr == null)
            {
                return ("");
            }

            return (mr.Code);
        }

        public void MoveUp()
        {
            //Decrease
            if (currIdx < 0)
            {
                return;
            }
            
            lsvFilter.SelectedIndex = currIdx;
            lsvFilter.ScrollIntoView(lsvFilter.SelectedItem);

            if (currIdx > 0)
            {
                currIdx--;
            }
        }

        public void MoveDown()
        {
            int max = items.Count;

            //Increase
            if (currIdx >= max)
            {
                return;
            }
            
            lsvFilter.SelectedIndex = currIdx;
            lsvFilter.ScrollIntoView(lsvFilter.SelectedItem);

            if (currIdx < max-1)
            {
                currIdx++;
            }
        }

        private void lsvFilter_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvFilter.SelectedItems.Count == 1)
            {
                MBaseModel m = (MBaseModel)lsvFilter.SelectedItems[0];
                parentCaller.CloseIntellisense();
            }
        }

        private void lsvFilter_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                lsvFilter_MouseDoubleClick(sender, null);
            }
        }

        private void rootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
