using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Onix.Client.Controller;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Windows
{
    public partial class WinDebugXml : Window
    {
        private string LastSubmit = "";
        private string LastReturn = "";
        private Window parent = null;

        public WinDebugXml(Window caller)
        {
            parent = caller;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (parent != null)
            {
                this.Title = parent.GetType().Name;
            }

            LastSubmit = OnixWebServiceAPI.GetLastSubmitXML();
            LastReturn = OnixWebServiceAPI.GetLastReturnXML();

            if (!LastSubmit.Equals(""))
            {
                BCLastSubmit.NavigateToString(LastSubmit);
            }

            if (!LastReturn.Equals(""))
            {
                BCLastReturn.NavigateToString(LastReturn);
            }
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            TabItem ti = (TabItem)tabMain.SelectedItem;
            String Tag = (String)ti.Tag;
            
            if (Tag.Equals(tbiSubmit.Tag))
            {
                System.Windows.Forms.Clipboard.SetText(PrintXML(LastSubmit));
            }
            if (Tag.Equals(tbiReturn.Tag))
            {
                System.Windows.Forms.Clipboard.SetText(PrintXML(LastReturn));
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //CMessageBox.Show("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA ทดสอบ ทดสอบ","Test",
            //    MessageBoxButton.YesNo, MessageBoxImage.Information);

            this.Close();
        }

        public static String PrintXML(String XML)
        {
            String Result = "";

            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(XML);

                writer.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                String FormattedXML = sReader.ReadToEnd();

                Result = FormattedXML;
            }
            catch (XmlException)
            {
            }

            mStream.Close();
            writer.Close();

            return Result;
        }

        private void cmdTest_Click(object sender, RoutedEventArgs e)
        {
            Wis.WsClientAPI.CTable dat = new Wis.WsClientAPI.CTable("");
            dat.SetFieldValue("MAX_MIGRAGE", "500");
            OnixWebServiceAPI.SubmitObjectAPI("MoveServiceToItem", dat);
        }

        private void RootDebugXml_Closed(object sender, EventArgs e)
        {
            BCLastReturn.Dispose();
            BCLastSubmit.Dispose();
        }
    }
}
