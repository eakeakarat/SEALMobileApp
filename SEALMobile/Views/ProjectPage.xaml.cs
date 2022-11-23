using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using SEALMobile.Models;
using Xamarin.Forms;

namespace SEALMobile.Views
{
    public partial class ProjectPage : ContentPage
    {
        Project project;
        public ProjectPage(Project pj)
        {
            InitializeComponent();
            project = pj;
            Title = project.projectname;

            //EdgesViewModel edgesViewModel = new EdgesViewModel(project);
            //Edge cloudEdge = edgesViewModel.getCloud();

            //EgdeDetailViewModel edgeDetailViewModel = new EgdeDetailViewModel(cloudEdge);
            //EdgeDevice cloud = edgeDetailViewModel.GetEdgeDevice();

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, project.projectid);

            if (!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }

            var cloudPath = Path.Combine(directoryname, "Dashboard.txt");

            CheckCloud(cloudPath);

        }

        async void CheckCloud(string cloudPath)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documents, "UserInfo", "access_token.txt");
            var token = File.ReadAllText(path);

            var graphQLHttp = new GraphQLHttpClient("http://fhe.netpie.io:30010/", new NewtonsoftJsonSerializer());
            graphQLHttp.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var edgesREQ = new GraphQLRequest
            {
                Query = @"query ($pjid: String!, $dname: String) { deviceList(filter:{projectid: $pjid, alias: $dname} )
                            { alias, deviceid, description }
                        }",
                Variables = new
                {
                    pjid = project.projectid,
                    dname = "FHE-CLOUD"
                }
            };

            var graphQLResponse = await graphQLHttp.SendQueryAsync<dataEdge>(edgesREQ);
            var res = graphQLResponse.Data.deviceList;
            Edge[] edgesList = res;
            Edge edge = edgesList[0];

            foreach (Edge e in edgesList)
            {
                Console.WriteLine(e.alias);
                if (e.alias == "FHE-CLOUD")
                {
                    edge = e;
                }
            }

            CloudID(cloudPath, edge, graphQLHttp);

        }

        private async void CloudID(string cloudPath, Edge edge, GraphQLHttpClient graphQLHttp)
        {
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
            EdgeDevice cloud = res;
            File.WriteAllText(cloudPath, cloud.deviceid + ":" + cloud.devicetoken[0]);

        }

        public void Handle_EdgeList(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EdgesListPage(project), true);
        }
        public void Handle_KeyGenerator(object sender, EventArgs e)
        {
            Navigation.PushAsync(new KeyGeneratorPage(project), true);
        }
        public void Handle_Dashboards(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DashboardPage(project), true);
        }
    }
}
