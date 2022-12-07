using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SEALMobile.Views;
using System.Threading.Tasks;
using EmbedIO;
using System.Reflection;
using EmbedIO.WebApi;
using SEALMobile.Services;

namespace SEALMobile
{
    public partial class App : Application
    {
        public App()
        {
            DevExpress.XamarinForms.Charts.Initializer.Init();

            InitializeComponent();

            Task.Factory.StartNew(async () =>
            {
                using (var server = new WebServer(HttpListenerMode.EmbedIO, "http://*:8080"))
                {
                    Assembly assembly = typeof(App).Assembly;
                    server.WithLocalSessionManager();
                    server.WithWebApi("/api", m => m.WithController(() => new ApiController()));
                    server.WithEmbeddedResources("/", assembly, "SEALMobile.html");
                    await server.RunAsync();
                }
            });


            //MainPage = new NavigationPage(new TestImportPage());

            MainPage = new NavigationPage(new LoginPage());
            //MainPage = new NavigationPage(new UserHomePage());
            //MainPage = new NavigationPage(new ProjectPage());
            //MainPage = new NavigationPage(new CreateProjectPage());
            //MainPage = new NavigationPage(new DashboardPage());

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
