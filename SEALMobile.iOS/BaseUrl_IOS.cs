using Xamarin.Forms;
using SEALMobile.iOS;
using Foundation;
using SEALMobile.Views;

[assembly: Dependency (typeof (BaseUrl_iOS))]

namespace SEALMobile.iOS 
{
    public class BaseUrl_iOS : IBaseUrl
    {
		public string Get () 
		{
			return NSBundle.MainBundle.BundlePath;
		}
	}
}