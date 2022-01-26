using System;
using System.Windows;
using System.Threading;
using Onix.Client.Controller;
using System.Net;
using System.Text;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.Commons.Windows
{
    public partial class WinUploadProgress : Window
    {
        private Thread t = null;
        private String fileName = "";
        private Boolean uploadDone = false;
        private String uploadResultReturn = "";

        public CUploadResult UploadResult { get; set; }

        public WinUploadProgress(String fileName)
        {
            this.fileName = fileName;
            InitializeComponent();
        }

        private void uploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesSent.ToString());
            double totalBytes = double.Parse(e.TotalBytesToSend.ToString());
            double percentage = bytesIn / totalBytes * 100;

            this.Dispatcher.Invoke(() =>
            {
                prgProgress.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }

        private void uploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                prgProgress.Value = 100;
                uploadResultReturn = Encoding.UTF8.GetString(e.Result, 0, e.Result.Length);
                uploadDone = true;
            });
        }

        private void MonitorProgress()
        {
            uploadDone = false;
            OnixWebServiceAPI.Upload(fileName, uploadProgressChanged, uploadFileCompleted);

            while (!uploadDone)
            {
            }

            CUploadResult uploadResult = new CUploadResult(uploadResultReturn);

            this.Dispatcher.Invoke(delegate
            {
                UploadResult = uploadResult;
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
