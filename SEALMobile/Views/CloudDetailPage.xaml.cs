using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using SEALMobile.Models;
using Xamarin.Forms;

namespace SEALMobile.Views
{
    public partial class CloudDetailPage : ContentPage
    {

        string qrRes;
        string projectid;
        Edge edge;
        EdgeDetailViewModel model;
        public CloudDetailPage(Edge e, string id)
        {
            InitializeComponent();
            edge = e;
            projectid = id;
            Title = edge.alias;
            detail_label.Text = edge.description;

            model = new EdgeDetailViewModel(edge);
        }


        async void SendKey_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var rlkPath = Path.Combine(documents, projectid, "relinKeys.txt");
            var rlkBase64 = File.ReadAllText(rlkPath);

            SendRlkToCloud(rlkBase64);

        }



        private async void SendRlkToCloud(string rlkKey)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documents, "UserInfo", "access_token.txt");
            var token = File.ReadAllText(path);

            var graphQLHttp = new GraphQLHttpClient("http://fhe.netpie.io:30010/", new NewtonsoftJsonSerializer());
            graphQLHttp.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var sendRLKtoCloud = new GraphQLRequest
            {
                Query = @"mutation ($pid:String!, $rlk:String!) { 
                                uploadRelinKey(projectid: $pid, relinKeyBase64: $rlk){
                                    success
                                } 
                            }",
                Variables = new
                {
                    pid = projectid,
                    rlk = rlkKey
                }
            };

            try
            {
                var graphQLResponse = await graphQLHttp.SendQueryAsync<data>(sendRLKtoCloud);
                var res = graphQLResponse.Data.uploadRelinKey.success;

                Console.WriteLine(res);

                //await Navigation.PushAsync(new UserHomePage(), true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private class data
        {
            public uploadRelinKey uploadRelinKey { get; set; }
        }
        public class uploadRelinKey
        {
            public bool success { get; set; }
        }

    }
}
