using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using SEALMobile.Services;
using ZXing.Mobile;



[assembly: Dependency(typeof(SEALMobile.iOS.Services.QrScanningService))]


namespace SEALMobile.iOS.Services
{
    public class QrScanningService : IQrScanningService
    {
        public QrScanningService()
        {
        }

        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Scan the QR Code",
                BottomText = "Please Wait",
            };

            var scanResult = await scanner.Scan(optionsCustom);
            return scanResult.Text;
        }
    }
}
