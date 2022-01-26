using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Threading;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public class CDeleteProgress : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int fcount = 0;
        private int success = 0;
        private int error = 0;
        private int deleted = 0;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public int FileCount
        {
            get
            {
                return (fcount);
            }

            set
            {
                fcount = value;
                NotifyPropertyChanged();
            }
        }

        public int SuccessCount
        {
            get
            {
                return (success);
            }

            set
            {
                success = value;
                NotifyPropertyChanged();
            }
        }

        public int FailedCount
        {
            get
            {
                return (error);
            }

            set
            {
                error = value;
                NotifyPropertyChanged();
            }
        }

        public int DeletedCount
        {
            get
            {
                return (deleted);
            }

            set
            {
                deleted = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Percent");
            }
        }

        public double Percent
        {            
            get
            {
                double pctg;

                if (fcount == 0)
                {
                    pctg = 0;
                    return (pctg);
                }

                double d = deleted;
                double f = fcount;

                pctg = (d / f) * 100;
                if (pctg > 100)
                {
                    pctg = 100;
                }

                return (System.Math.Ceiling(pctg));
            }

            set { }
        }
    }

    public partial class WinDeleteProgress : Window
    {
        public ObservableCollection<MBaseModel> ItemsSource = null;
        public DeleteFunction DeleteDelegate = null;
        public DeleteAPIFunction DeleteAPIDelegate = null;
        public String APIName = "";

        private Collection<MBaseModel> deletedItems = new Collection<MBaseModel>();
        private int fileToDeleteCount = 0;
        private CDeleteProgress mv = null;

        public WinDeleteProgress()
        {
            mv = new CDeleteProgress();
            DataContext = mv;

            InitializeComponent();
        }

        public void SetFileCount(int fc)
        {
            fileToDeleteCount = fc;
            mv.FileCount = fc;
        }

        private void DeleteFiles()
        {
            foreach (MBaseModel o in ItemsSource)
            {
                if (o.IsSelectedForDelete)
                {
                    CTable t = o.GetDbObject();
                    Boolean r = false;
                    if (DeleteDelegate != null)
                    {
                        r = DeleteDelegate(t);
                    }
                    else
                    {
                        r = DeleteAPIDelegate(APIName, t);
                    }

                    if (r)
                    {
                        deletedItems.Add(o);
                        mv.SuccessCount++;
                    }
                    else
                    {
                        mv.FailedCount++;
                    }

                    mv.DeletedCount++;                    
                }
            }            
        }

        private void MonitorDeleted()
        {
            while (mv.Percent < 100)
            {
            }

            System.Threading.Thread.Sleep(1000);
            foreach (MBaseModel b in deletedItems)
            {
                this.Dispatcher.Invoke(delegate { ItemsSource.Remove(b); });
            }
            this.Dispatcher.Invoke(delegate { this.Close(); });
        }

        private void Window_ContentRendered(object sender, System.EventArgs e)
        {
            Boolean result = CHelper.AskConfirmDelete(mv.FileCount);
            if (!result)
            {
                this.Close();
                return;
            }

            Thread t = new Thread(DeleteFiles);
            t.Start();

            Thread m = new Thread(MonitorDeleted);
            m.Start();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
