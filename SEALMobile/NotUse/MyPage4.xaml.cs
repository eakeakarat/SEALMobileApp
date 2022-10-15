using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Research.SEAL;
using Xamarin.Forms;


namespace SEALMobile
{
    public partial class MyPage4 : ContentPage
    {
        SEALContext context;
        CKKSEncoder CKKSEncoder;
        Decryptor decryptor;

        public interface IBaseUrl { string Get(); }

        public MyPage4()
        {
            InitializeComponent();
            //webView.Source = URL;
            //var webView = new WebView();

            //webView.Source = "https://www.google.co.th/?hl=th";

            var htmlSource = new HtmlWebViewSource();

            var baseUrl = DependencyService.Get<IBaseUrl>().Get();
            var fileURL = Path.Combine(baseUrl, "local.html");
            htmlSource.BaseUrl = baseUrl;
            htmlSource.Html = File.ReadAllText(fileURL);

            webView.Source = htmlSource;

            //string jsonString = JsonSerializer.Serialize(vm);
            //await WebView.EvaluateJavaScriptAsync($"jsFunction({jsonString})");
            //string re = await webView.EvaluateJavaScriptAsync($"updatetexttowebview('HELLO FORM Xamarin')");
            //Content = webView;

            context = new SEALContext(new MyPage3("").GetLocalEncryptionParameters(), true, SecLevelType.None);
            CKKSEncoder = new CKKSEncoder(context);
            decryptor = new Decryptor(context, new MyPage3("").GetLocalSecretKey(context));


        }

        public async void Handle_Hello(object sender, EventArgs e)
        {
            string send = "HELLO FROM Xamarin";
            if (!string.IsNullOrWhiteSpace(entry.Text))
            {
                send = entry.Text;
            }
            await webView.EvaluateJavaScriptAsync($"updatetexttowebview('{send}')");

        }

        public Ciphertext MockingEncryptData(ulong a , ulong b)
        {
            double scale = Math.Pow(2.0, 30);
            Evaluator evaluator = new Evaluator(context);
            Encryptor encryptor = new Encryptor(context, new MyPage3("").GetLocalPublicKey(context));

            // encode received data to plainText
            Plaintext plain1 = new Plaintext();
            Plaintext plain2 = new Plaintext();
            CKKSEncoder.Encode(a, scale, plain1);
            CKKSEncoder.Encode(b, scale, plain2);

            // encrypt plainText to cipherText
            Ciphertext cipher1 = new Ciphertext();
            Ciphertext cipher2 = new Ciphertext();
            encryptor.Encrypt(plain1, cipher1);
            encryptor.Encrypt(plain2, cipher2);

            Ciphertext resultCipher = new Ciphertext();

            // compute A^2 + B^2
            evaluator.Multiply(cipher1, cipher1, cipher1);
            evaluator.Multiply(cipher2, cipher2, cipher2);
            evaluator.Add(cipher1, cipher2, resultCipher);

            return resultCipher;

        }

        public async void Handle_Decrypt(object sender, EventArgs e)
        {
            Ciphertext tmp = MockingEncryptData(2, 3);
            Ciphertext ciphertext = new Ciphertext();
            Plaintext plaintext = new Plaintext();
            List<double> result = new List<double>();

            //MemoryStream stream = new MemoryStream();
            //tmp.Save(stream);

            //ciphertext.Load(context, stream);

            ciphertext = tmp;
            decryptor.Decrypt(ciphertext, plaintext);
            CKKSEncoder.Decode(plaintext, result);
            name_label.Text = result[0].ToString();
            await webView.EvaluateJavaScriptAsync($"updateDecryptedText('{result[0]}')");
        }

        public async void Handle_Refresh(object sender, EventArgs e)
        {
            //await webView.EvaluateJavaScriptAsync($"");
            webView.Reload();
        }

        async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }
            else
            {
                await Navigation.PopAsync();
            }
        }

        void OnForwardButtonClicked(object sender, EventArgs e)
        {
            if (webView.CanGoForward)
            {
                webView.GoForward();
            }
        }

        // convert Stream to StringBase64
        public static string ToBase64(MemoryStream data)
        {
            var dataAsString = Convert.ToBase64String(data.ToArray());
            return dataAsString;
        }

        // convert StringBase64 to Stream
        public static MemoryStream ToMemoryStream(string data)
        {
            var bytes = Convert.FromBase64String(data);
            var dataAsStream = new MemoryStream(bytes);
            return dataAsStream;
        }
    }
}