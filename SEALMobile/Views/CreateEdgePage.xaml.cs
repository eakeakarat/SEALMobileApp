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
    public partial class CreateEdgePage : ContentPage
    {
        Project project;
        public CreateEdgePage(Project p)
        {
            InitializeComponent();
            project = p;
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (name_entry.Text != null || name_entry.Text != "")
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var path = Path.Combine(documents, "UserInfo", "access_token.txt");
                var token = File.ReadAllText(path);

                var graphQLHttp = new GraphQLHttpClient("http://fhe.netpie.io:30010/", new NewtonsoftJsonSerializer());
                graphQLHttp.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var createedgeREQ = new GraphQLRequest
                {
                    Query = @"mutation ($pid:String!, $dname:String!, $ddes:String!) { 
                                createDevice(projectid: $pid, deviceinfo:{alias: $dname, description: $ddes}) {
                                    alias,deviceid,description
                                } 
                            }",
                    Variables = new
                    {
                        pid = project.projectid,
                        dname = name_entry.Text,
                        ddes = desc_entry.Text
                    }
                };


                try
                {
                    var graphQLResponse = await graphQLHttp.SendQueryAsync<dataCreateEdge>(createedgeREQ);
                    var res = graphQLResponse.Data.createDevice;
                    Edge edge = res;
                    await Navigation.PopAsync();
                }
                catch
                {
                    await DisplayAlert("Alert", "Can't create project", "Close");
                }

            }
        }
    }
    public class dataCreateEdge
    {
        public Edge createDevice { get; set; }
    }
}
