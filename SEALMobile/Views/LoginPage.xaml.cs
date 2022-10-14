using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace SEALMobile.Views
{
    public partial class LoginPage : ContentPage
    {
        public class Login
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
        }

        public class Data
        {
            public Login login;
        }

        string dir = "UserInfo";

        string documents;
        string directoryname;

        public LoginPage()
        {
            InitializeComponent();
            user_value.Text = "nataset.t@ku.th";
            pass_value.Text = "121bd4a9";

            documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            directoryname = Path.Combine(documents, dir);

            if (!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }
        }

        async void Handle_Login(object sender, System.EventArgs e)
        {
            var graphQLHttp = new GraphQLHttpClient("http://fhe.netpie.io:30010/", new NewtonsoftJsonSerializer());
            var loginTokenRequest = new GraphQLRequest
            {
                Query = @"mutation ($usr: String!, $pwd: String!) {login (username: $usr, password: $pwd) {access_token, token_type}}",
                Variables = new
                {
                    usr = user_value.Text,
                    pwd = pass_value.Text
                }
            };

            try
            {
                var graphQLResponse = await graphQLHttp.SendQueryAsync<Data>(loginTokenRequest);
                var res = graphQLResponse.Data.login;
                var path = Path.Combine(documents, dir, "access_token.txt");
                File.WriteAllText(path, res.access_token);
                var home = new UserHomePage();
                await Navigation.PushAsync(home, true);
            }
            catch
            {
                await DisplayAlert("Alert", "invalid username/password", "Close");
            }


        }

    }
}
