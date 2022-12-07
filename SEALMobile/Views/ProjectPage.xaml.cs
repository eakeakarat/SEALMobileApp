using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, project.projectid);

            if (!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }

            SaveDashboardToken();



        }

        async void SaveDashboardToken()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documents, "UserInfo", "access_token.txt");
            var token = File.ReadAllText(path);

            var cloudPath = Path.Combine(documents, project.projectid, "Dashboard.txt");

            var graphQLHttp = new GraphQLHttpClient("http://fhe.netpie.io:30010/", new NewtonsoftJsonSerializer());
            graphQLHttp.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var edgesREQ = new GraphQLRequest
            {
                Query = @"query ($pjid: String!, $htag: [String]) { deviceList(filter:{projectid: $pjid, hashtag: $htag}) 
                            { alias, deviceid, description, hashtag }
                        }",
                Variables = new
                {
                    pjid = project.projectid,
                    htag = "cloud",
                }
            };

            try
            {
                var graphQLResponse = await graphQLHttp.SendQueryAsync<dataEdge>(edgesREQ);
                Edge res = graphQLResponse.Data.deviceList[0];
                //Console.WriteLine(res.alias);

                var cloudREQ = new GraphQLRequest
                {
                    Query = @"query($did:String!) {device(deviceid: $did){
                            alias,description,deviceid,devicetoken,hashtag,devicesecret,projectid
                        }}",
                    Variables = new
                    {
                        did = res.deviceid
                    }
                };

                var cloudRes = await graphQLHttp.SendQueryAsync<dataDevice>(cloudREQ);
                var edgeDevice = cloudRes.Data.device;
                //Console.WriteLine("Dashboard token = " + "#" + edgeDevice.deviceid + ":" + edgeDevice.devicetoken[0]);

                File.WriteAllText(cloudPath, "#" + edgeDevice.deviceid + ":" + edgeDevice.devicetoken[0]);

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }

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
            //SaveDashboardToken();
            Navigation.PushAsync(new DashboardPage(project), true);
        }
    }
}
