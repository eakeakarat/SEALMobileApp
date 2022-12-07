using System;
using System.IO;
using System.Threading;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using SEALMobile.Models;

using Xamarin.Forms;
using System.Net.Http;
using System.Threading.Tasks;

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

            makeAPIknowIDAsync();

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, project.projectid);
            var cloudPath = Path.Combine(directoryname, "Dashboard.txt");

            var token = File.ReadAllText(cloudPath);

            //var url = "https://ntscloud.cc/freeboard/index.html";

            var url = "http://localhost:8080/freeboard/index.html" + token;
            //var url = "http://localhost:8080/index.html";

            WebView web = new WebView();
            web.Source = url;
            Console.WriteLine(url);
            Content = web;
        }

        public string DecryptData(string cipher)
        {
            //return seal.decryptText(cipher);

            Console.WriteLine("C# function");

            return "Touch a C# funciton";

        }

        private async void makeAPIknowIDAsync()
        {
            var uri = "http://localhost:8080/api/projectid";
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(project.projectid, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync(uri, content);

            Console.WriteLine("C# " + responseMessage.StatusCode.ToString());

        }
    }
}
