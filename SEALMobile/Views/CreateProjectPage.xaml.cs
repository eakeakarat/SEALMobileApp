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
    public partial class CreateProjectPage : ContentPage
    {
        public CreateProjectPage()
        {
            InitializeComponent();
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

                var createprojectsREQ = new GraphQLRequest
                {
                    Query = @"mutation ($name:String!, $desc:String!) { 
                                createProject(projectname: $name, description: $desc) {
                                    projectid,projectname,description
                                } 
                            }",
                    Variables = new
                    {
                        name = name_entry.Text,
                        desc = desc_entry.Text
                    }
                };


                try
                {
                    var graphQLResponse = await graphQLHttp.SendQueryAsync<dataCreateProject>(createprojectsREQ);
                    var res = graphQLResponse.Data.createproject;
                    Project project = res;

                    //await Navigation.PushAsync(new UserHomePage(), true);
                    await Navigation.PopAsync();

                }
                catch
                {
                    await DisplayAlert("Alert", "Can't create project", "Close");
                }


            }
        }
    }
    public class dataCreateProject
    {
        public Project createproject { get; set; }
    }
}
