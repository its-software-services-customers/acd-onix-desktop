using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using QRCoder;
using Onix.ClientCenter.Commons.Utils.QR;

namespace Onix.ClientCenter.Commons.UControls
{
    public partial class UQRPromptPay : UserControl
    {
        public static readonly DependencyProperty PropmtPayIDProperty =
            DependencyProperty.Register("PropmtPayID",
                typeof(string),
                typeof(UQRPromptPay),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedPromptPayEvtChanged));

        public static readonly DependencyProperty AmountProperty =
            DependencyProperty.Register("Amount",
                typeof(string),
                typeof(UQRPromptPay),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedAmountEvtChanged));

        #region PropmtPayID
        public string PropmtPayID
        {
            get { return (string)GetValue(PropmtPayIDProperty); }
            set { SetValue(PropmtPayIDProperty, value); }
        }

        private static void OnSelectedPromptPayEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UQRPromptPay ctrl = obj as UQRPromptPay;
        }
        #endregion

        #region Amount
        public string Amount
        {
            get { return (string)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }

        private static void OnSelectedAmountEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UQRPromptPay ctrl = obj as UQRPromptPay;
        }
        #endregion

        public UQRPromptPay()
        {
            InitializeComponent();
        }

        public void GenerateQR()
        {
            string payload = PromptPayUtils.QRCodePayload(PropmtPayID, Amount);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(200);

            var ms = new MemoryStream(qrCodeAsPngByteArr);
            var imageSource = new BitmapImage();

            imageSource.BeginInit();
            imageSource.StreamSource = ms;
            imageSource.EndInit();

            imgQR.Source = imageSource;
        }
    }
}
