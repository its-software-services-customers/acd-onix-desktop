using System;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using System.Windows.Media;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.UControls;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.Commons.UControls
{
    public partial class UImageBox : UserControl
    {
        private Boolean internalUpdate = false;
        private String extFlag = "";

        public static readonly DependencyProperty FileNameProperty =
        DependencyProperty.Register("FileName", typeof(String), typeof(UImageBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedFileEvtChanged));

        public static readonly DependencyProperty FilterProperty =
        DependencyProperty.Register("Filter", typeof(String), typeof(UImageBox),
            new FrameworkPropertyMetadata("Image files (*.png, *.jpg)|*.png;*.jpg|All files (*.*)|*.*", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedFilterEvtChanged));

        public static readonly DependencyProperty ExtFlagProperty =
        DependencyProperty.Register("ExtFlag", typeof(String), typeof(UImageBox),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedExtFlagEvtChanged));

        public static readonly DependencyProperty CaptionProperty =
        DependencyProperty.Register("Caption", typeof(String), typeof(UImageBox),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedCaptionEvtChanged));

        public static readonly DependencyProperty FileSizeLimitProperty =
        DependencyProperty.Register("FileSizeLimit", typeof(long), typeof(UImageBox),
            new FrameworkPropertyMetadata((long) 100000, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedFileSizeLimitEvtChanged));

        public event SelectionChangedEventHandler SelectedFileChanged;

        public UImageBox()
        {
            InitializeComponent();            
        }

        #region FileName
        public String FileName
        {
            get { return (String)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        private static void OnSelectedFileEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UImageBox ctrl = obj as UImageBox;

            if (ctrl.internalUpdate)
            {
                return;
            }
            
            ctrl.displayImage(ctrl.FileName, "storage");
        }
        #endregion

        #region FileSizeLimit
        public long FileSizeLimit
        {
            get { return (long)GetValue(FileSizeLimitProperty); }
            set { SetValue(FileSizeLimitProperty, value); }
        }

        private static void OnSelectedFileSizeLimitEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UImageBox ctrl = obj as UImageBox;
        }
        #endregion

        #region Filter
        public String Filter
        {
            get { return (String)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        private static void OnSelectedFilterEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UImageBox ctrl = obj as UImageBox;            
        }
        #endregion

        #region ExtFlag
        public String ExtFlag
        {
            get { return (String)GetValue(ExtFlagProperty); }
            set { SetValue(ExtFlagProperty, value); }
        }

        private static void OnSelectedExtFlagEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UImageBox ctrl = obj as UImageBox;

            if (ctrl.internalUpdate)
            {
                return;
            }

            ctrl.extFlag = (String) e.NewValue;
        }
        #endregion

        public String LoadedingImagePath
        {
            get
            {                
                return (CUtil.GetLoadingImagePath());
            }

            set
            {
            }
        }

        #region Caption
        public String Caption
        {
            get { return (String)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        private static void OnSelectedCaptionEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UImageBox ctrl = obj as UImageBox;
            ctrl.txtCaption.Text = ctrl.Caption;
        }
        #endregion

        private void updateExtFlag(String action)
        {
            //I = ไม่มีไฟล์อยู่มาก่อน
            //I = มีไฟล์อยู่แล้วจริง ๆ

            if (extFlag.Equals(""))
            {
                if (action.Equals("U"))
                {
                    extFlag = "A";
                }
            }
            else if (extFlag.Equals("A"))
            {
                if (action.Equals("D"))
                {
                    extFlag = "";
                }
            }
            else if (extFlag.Equals("I") || (extFlag.Equals("E")))
            {
                if (action.Equals("D"))
                {
                    extFlag = "D";
                }
                else if (action.Equals("U"))
                {
                    extFlag = "E";
                }
            }

            internalUpdate = true;
            ExtFlag = extFlag;
            internalUpdate = false;
        }

        private void bitmapImage_DownloadCompleted(object sender, EventArgs e)
        {
            imgUpload.Visibility = Visibility.Visible;
            loadingImage.Visibility = Visibility.Collapsed;
        }

        private void bitmapImage_DownloadFailed(object sender, ExceptionEventArgs e)
        {
            imgUpload.Visibility = Visibility.Visible;
            loadingImage.Visibility = Visibility.Collapsed;
        }

        private void displayImage(String token, String mode)
        {
            if (token.Equals(""))
            {
                return;
            }

            loadingImage.Play();

            String uri = OnixWebServiceAPI.GetUploadedFileUrl(token, mode);

            imgUpload.Visibility = Visibility.Collapsed;
            loadingImage.Visibility = Visibility.Visible;

            BitmapImage b = new BitmapImage();
            b.DownloadCompleted += new EventHandler(bitmapImage_DownloadCompleted);
            b.DownloadFailed += new EventHandler<ExceptionEventArgs>(bitmapImage_DownloadFailed);
            b.BeginInit();
            b.UriSource = new Uri(uri);
            b.EndInit();
            imgUpload.Source = b;                 
        }

        private void CmdUpload_Click(object sender, RoutedEventArgs e)
        {            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = Filter;
            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }

            String fileName = openFileDialog.FileName;

            long length = new System.IO.FileInfo(fileName).Length;
            if (length > FileSizeLimit)
            {
                CHelper.ShowErorMessage(FileSizeLimit.ToString(), "ERROR_UPLOAD_FILE_SIZE", null);
                return;
            }

            WinUploadProgress wp = new WinUploadProgress(fileName);
            wp.ShowDialog();

            CUploadResult result = wp.UploadResult;

            displayImage(result.Token, "wip");            

            internalUpdate = true;
            FileName = result.Token;
            internalUpdate = false;

            updateExtFlag("U");

            if (SelectedFileChanged != null)
            {
                SelectedFileChanged(this, null);
            }
        }

        private void CmdRemove_Click(object sender, RoutedEventArgs e)
        {
            imgUpload.Source = null;

            internalUpdate = true;
            FileName = "";
            internalUpdate = false;

            updateExtFlag("D");

            if (SelectedFileChanged != null)
            {
                SelectedFileChanged(this, null);
            }
        }

        private void LoadingImage_MediaEnded(object sender, RoutedEventArgs e)
        {
            loadingImage.Position = new TimeSpan(0, 0, 1);
            loadingImage.Play();
        }

        private void RootElement_Loaded(object sender, RoutedEventArgs e)
        {            
        }
    }
}
