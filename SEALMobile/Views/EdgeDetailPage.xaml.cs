using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SEALMobile.Models;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace SEALMobile.Views
{
    public partial class EdgeDetailPage : ContentPage
    {
        string qrRes;
        string projectid;
        Edge edge;
        EdgeDetailViewModel model;


        public EdgeDetailPage(Edge e, string id)
        {
            InitializeComponent();
            edge = e;
            projectid = id;
            Title = edge.alias;
            detail_label.Text = edge.description;

            model = new EdgeDetailViewModel(edge);
        }

        async void Pair_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var result = await Navigation.ShowPopupAsync(new MyScanner());
            string text = result.ToString();
            qrRes = text;
            //qrRes = "{ \"hostname\":\"eak\",\"hostIP\":\"http://localhost:9000\",\"endPoint\":\"/import/contextAndPublicKey\",\"oneTimePassword\":\"gogeEaTNdTeUYT3Wan5lSSTe8bRkUulFQ7RhsZCX8yWzl7mAwt\"} ";
            if (!result.Equals("closed") )
            {
                JObject hostJson = JObject.Parse(qrRes);
                Host host = hostJson.ToObject<Host>();

                string uri = "";

                if (!host.hostIP.Contains("http"))
                {
                    uri += "http://"; 
                }

                uri += host.hostIP;

                if (host.port != null && host.port != "")
                {
                    uri += ":" + host.port;
                }

                uri += host.endpoint;

                Console.WriteLine(uri);

                HttpClient client = new HttpClient();

                var edgeReq = prepareEdgeREQ();
                edgeReq.oneTimePassword = host.oneTimePassword;

                //dataPacking dataPacking = new dataPacking { data = edgeReq };
                //string jsonPacking = JsonConvert.SerializeObject(dataPacking, Formatting.Indented);
                string jsonPacking = JsonConvert.SerializeObject(edgeReq, Formatting.Indented);

                StringContent content = new StringContent(jsonPacking, Encoding.UTF8, "application/json");

                var responseMessage = await client.PostAsync(uri, content);

                Console.WriteLine(responseMessage.StatusCode.ToString());
                await Navigation.PopAsync();
            }
            else
            {
                await Navigation.PopAsync();
            }

        }

        private EdgeReq prepareEdgeREQ()
        {
            EdgeReq edgeReq = new EdgeReq();
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var cotPath = Path.Combine(documents, projectid, "context.txt");
            var pkPath = Path.Combine(documents, projectid, "publicKey.txt");
            var scalePath = Path.Combine(documents, projectid, "scale.txt");
            var typePath = Path.Combine(documents, projectid, "type.txt");

            edgeReq.publicKey = File.ReadAllText(pkPath);
            edgeReq.context = File.ReadAllText(cotPath);
            edgeReq.contextType = File.ReadAllText(typePath);
            edgeReq.scale = File.ReadAllText(scalePath);

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

        async void Delete_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var answer = await DisplayAlert("Delete Device ?", "Device: " + edge.alias + "\nID: " + edge.deviceid, "YES", "NO");

            if (answer)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var path = Path.Combine(documents, "UserInfo", "access_token.txt");
                var token = File.ReadAllText(path);

                var graphQLHttp = new GraphQLHttpClient("http://fhe.netpie.io:30010/", new NewtonsoftJsonSerializer());
                graphQLHttp.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var deleteEdgeREQ = new GraphQLRequest
                {
                    Query = @"mutation ($did:String!) { 
                                deleteDevice(deviceid: $did) {
                                    alias,deviceid,description
                                } 
                            }",
                    Variables = new
                    {
                        did = edge.deviceid
                    }
                };

                try
                {
                    var graphQLResponse = await graphQLHttp.SendQueryAsync<dataDeleteEdge>(deleteEdgeREQ);
                    var res = graphQLResponse;

                    if (res.Data != null)
                    {
                        //Console.WriteLine("RES ");
                        Console.WriteLine("RES " + res.Data.deleteDevice.alias);
                        await DisplayAlert("Already Delete", "Device: " + res.Data.deleteDevice.alias + "\nID: " + res.Data.deleteDevice.deviceid, "Close");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        //Console.WriteLine("ERR ");
                        Console.WriteLine("ERR " + res.Errors[0].Message);
                        await DisplayAlert("Error!", "Can not delete device cause " + res.Errors[0].Message, "Close");

                    }



                }
                catch (Exception ex)
                {
                    await DisplayAlert("Catch", ex.Message, "Close");
                }
            }

        }

    }

    public class dataDeleteEdge
    {
        public Edge deleteDevice { get; set; }
    }

    public class Host
    {
        public string hostname { get; set; }
        public string hostIP { get; set; }
        public string endpoint { get; set; }
        public string oneTimePassword { get; set; }
        public string port { get; set; }
    }

    public class EdgeReq
    {
        public string oneTimePassword { get; set; }
        public string publicKey { get; set; }
        public string context { get; set; }
        public string contextType { get; set; }
        public string scale { get; set; }
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
