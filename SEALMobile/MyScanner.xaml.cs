using System;
using System.Collections.Generic;
using SEALMobile.Services;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace SEALMobile
{
    public partial class MyScanner : Popup
    {
        public MyScanner()
        {
            scanner = new ZXingScannerView();
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (scanner.Result != null)
            {
                Dismiss(scanner.Result);
            }
            else
            {
                Dismiss("closed");
            }

        }

        void scanner_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (result != null)
                {
                    label.Text = "Scanning Complete";
                    label_scan.IsVisible = true;
                    label_scan.Text = result.Text;
                    scanner.IsScanning = false;
                    scanner.IsVisible = false;
                }
            });

        }
    }
}