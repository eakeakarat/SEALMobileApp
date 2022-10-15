using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SEALMobile.Models;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace SEALMobile.Views
{
    public partial class EdgeDetailPage : ContentPage
    {
        //        {
        //oneTimepassword: ส่งค่าเดิมกลับไปเลยครับ,
        //publicKey: pk base64,
        //context : context Base64
        //netpieDevice: {
        //          client_id: cilent_id ของ edge,
        //          token: token ของ edge,
        //          secret: secret ของ edge
        //    }
        //}

        string qrRes;
        string projectid;
        Edge edge;
        EgdeDetailViewModel model;

        NetpieDevice netpieDevice;
        EdgeReq edgeReq;


        public EdgeDetailPage(Edge e, string id)
        {
            InitializeComponent();
            edge = e;
            projectid = id;
            Title = edge.alias;
            detail_label.Text = edge.description;

            model = new EgdeDetailViewModel(edge);



        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var result = await Navigation.ShowPopupAsync(new MyScanner());
            string text = result.ToString();
            qrRes = text;
            JObject hostJson = JObject.Parse(qrRes);
            Host host = hostJson.ToObject<Host>();

            string uri = host.hostIP + host.endpoint;

            HttpClient client = new HttpClient();

            var edgeReq = prepareEdgeREQ();
            edgeReq.oneTimepassword = host.oneTimePassword;

            dataPacking dataPacking = new dataPacking { data = edgeReq };
            string jsonPacking = JsonConvert.SerializeObject(dataPacking, Formatting.Indented);
            StringContent content = new StringContent(jsonPacking, Encoding.UTF8, "application/json");

            var responseMessage = await client.PostAsync(uri, content);

            Console.WriteLine(responseMessage.StatusCode.ToString());

        }

        private EdgeReq prepareEdgeREQ()
        {
            EdgeReq edgeReq = new EdgeReq();
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var cotPath = Path.Combine(documents, projectid, "context.txt");
            var pkPath = Path.Combine(documents, projectid, "publicKey.txt");

            edgeReq.context = File.ReadAllText(cotPath);
            edgeReq.publicKey = File.ReadAllText(pkPath);

            EdgeDevice device = model.device;

            edgeReq.netpieDevice = new NetpieDevice
            {
                client_id = device.deviceid,
                secret = device.devicesecret,
                token = device.devicetoken
            };

            //Console.WriteLine(edgeReq.context.ToString());
            return edgeReq;
        }
    }

    public class Host
    {
        public string hostname { get; set; }
        public string hostIP { get; set; }
        public string endpoint { get; set; }
        public string oneTimePassword { get; set; }
    }

    public class EdgeReq
    {
        public string oneTimepassword { get; set; }
        public string publicKey { get; set; }
        public string context { get; set; }
        public NetpieDevice netpieDevice { get; set; }

    }

    public class NetpieDevice
    {
        public string client_id { get; set; }
        public string[] token { get; set; }
        public string secret { get; set; }
    }
    public class dataPacking
    {
        public EdgeReq data { get; set; }
    }
}
