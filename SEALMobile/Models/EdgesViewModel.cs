using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Xamarin.CommunityToolkit.ObjectModel;

namespace SEALMobile.Models
{
    public class EdgesViewModel
    {
        public ObservableCollection<Edge> Edges { get; set; }
        public Edge[] edgesList;

        public EdgesViewModel(Project project)
        {
            Edges = new ObservableRangeCollection<Edge>();
            LoadMore(project.projectid);
        }

        async void LoadMore(string id)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documents, "UserInfo", "access_token.txt");
            var token = File.ReadAllText(path);

            var graphQLHttp = new GraphQLHttpClient("http://fhe.netpie.io:30010/", new NewtonsoftJsonSerializer());
            graphQLHttp.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var edgesREQ = new GraphQLRequest
            {
                Query = @"query ($pjid: String!) { deviceList(filter:{projectid: $pjid} )
                            { alias, deviceid, description }
                        }",
                Variables = new
                {
                    pjid = id
                }
            };

            var graphQLResponse = await graphQLHttp.SendQueryAsync<dataEdge>(edgesREQ);
            var res = graphQLResponse.Data.deviceList;
            edgesList = res;

            foreach (Edge p in res)
            {
                Edges.Add(p);
            }

        }
        public Edge findEdge(string id)
        {
            Edge edge = new Edge();
            foreach (Edge e in edgesList)
            {
                if (e.deviceid == id)
                {
                    edge = e;
                }
            }
            return edge;
        }
    }

    public class dataEdge
    {
        public Edge[] deviceList { get; set; }
    }

    public class Edge
    {
        public string alias { get; set; }
        public string deviceid { get; set; }
        public string description { get; set; }
    }
}
