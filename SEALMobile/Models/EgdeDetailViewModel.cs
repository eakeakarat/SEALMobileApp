using System;
using System.IO;
using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace SEALMobile.Models
{
    public class EgdeDetailViewModel
    {
        Edge edge;
        public EdgeDevice device { get; set; }
        public EgdeDetailViewModel(Edge e)
        {
            edge = e;
            device = new EdgeDevice();
            LoadMore();
        }
        async void LoadMore()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documents, "UserInfo", "access_token.txt");
            var token = File.ReadAllText(path);

            var graphQLHttp = new GraphQLHttpClient("http://fhe.netpie.io:30010/", new NewtonsoftJsonSerializer());
            graphQLHttp.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var deviceREQ = new GraphQLRequest
            {
                Query = @"query($did:String!) {device(deviceid: $did){
                            alias,description,deviceid,devicetoken,devicesecret,projectid
                        }}",
                Variables = new
                {
                    did = edge.deviceid
                }
            };

            var graphQLResponse = await graphQLHttp.SendQueryAsync<dataDevice>(deviceREQ);
            var res = graphQLResponse.Data.device;
            device = res;
            Console.WriteLine(device.deviceid);

        }
    }

    public class dataDevice
    {
        public EdgeDevice device { get; set; }
    }

    public class EdgeDevice
    {
        public string alias { get; set; }
        public string description { get; set; }
        public string deviceid { get; set; }
        public string[] devicetoken { get; set; }
        public string devicesecret { get; set; }
        public string projectid { get; set; }
    }
}
