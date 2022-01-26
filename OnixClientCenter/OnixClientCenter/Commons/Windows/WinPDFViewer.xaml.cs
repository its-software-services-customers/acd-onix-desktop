using System;
using System.IO;
using System.Windows;

namespace Onix.ClientCenter.Commons.Windows
{   
    public partial class WinPDFViewer : Window
    {
        private String pdfFile = "";
        private String name = "";

        public WinPDFViewer(String pdfFile, String name)
        {
            this.name = name;
            this.pdfFile = pdfFile;

            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.Title = name;
            webPdf.Navigate(pdfFile);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            webPdf.Dispose();
        }
    }
}
