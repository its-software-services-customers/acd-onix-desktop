using System;
using System.Windows;
using System.Threading;
using Onix.Client.Helper;
using Onix.Client.Report;

namespace Onix.ClientCenter.Windows
{
    public partial class WinProgressRenderer : Window
    {
        private Thread t = null;
        private CExcelRenderer tc = null;

        public WinProgressRenderer(CExcelRenderer tc)
        {
            this.tc = tc;
            InitializeComponent();
        }

        public void UpdateDone(Boolean flag, Boolean isFailed)
        {
            prgProgress.Dispatcher.Invoke(delegate
            {
                prgProgress.Value = prgProgress.Maximum;
            });
        }

        public void UpdateProgress(int c, int m)
        {
            prgProgress.Dispatcher.Invoke(delegate
            {
                prgProgress.Value = c;
                prgProgress.Maximum = m;
            });
        }

        private void MonitorProgress()
        {
            tc.Init();
            tc.Render(UpdateProgress, UpdateDone);
            tc.SaveFile();
            tc.CleanUp();

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

        }
    }
}
