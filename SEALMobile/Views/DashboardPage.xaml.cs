using System;
using System.IO;
using System.Threading;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using SEALMobile.Models;

using Xamarin.Forms;

namespace SEALMobile.Views
{
    public interface IBaseUrl { string Get(); }



    public partial class DashboardPage : ContentPage
    {
        Project project;
        SEALENY seal;
        IMqttClient mqttClient;
        MqttClientOptions mqttClientOptions;


        public DashboardPage(Project p)
        {
            InitializeComponent();
            project = p;
            seal = new SEALENY(project);


            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, project.projectid);
            var cloudPath = Path.Combine(directoryname, "Dashboard.txt");

            string token = File.ReadAllText(cloudPath);

            //Web Session
            WebView web = new WebView();
            var urlSource = new UrlWebViewSource();
            string baseUrl = DependencyService.Get<IBaseUrl>().Get();

            token = "3a7bac62-84cd-4245-8c34- 932205f4ea3e:8WeNXJam3TWHTtqHWuUcbq1oxT3gpcdd";
            string filePathUrl = Path.Combine(baseUrl, "Freeboard", "index.html#" + token);

            //Console.WriteLine(baseUrl);
            //Console.WriteLine(filePathUrl);

            urlSource.Url = filePathUrl;
            web.Source = urlSource;
            Content = web;


        }

        public string DecryptData(string s)
        {
            Console.WriteLine("DecryptData FN");

            //string result = seal.decryptText(s);
            //return result;

            return "OK";

        }


        

    }
}
