using System;
using System.Windows;
using System.Threading;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Windows
{
    public partial class WinProgress : Window
    {
        private Thread t = null;
        private CThreadSync tc = null;

        public WinProgress(CThreadSync tc)
        {
            this.tc = tc;
            InitializeComponent();
        }

        private void MonitorProgress()
        {
            while (!tc.GetIsDone())
            {
                prgProgress.Dispatcher.Invoke(delegate
                {
                    prgProgress.Value = tc.GetCurrent();
                    prgProgress.Maximum = tc.GetMax();
                });
            }

            this.Dispatcher.Invoke(delegate
            {
                this.Close();
            });
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            t = new Thread(MonitorProgress);

            t.Start();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            t.Abort();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
