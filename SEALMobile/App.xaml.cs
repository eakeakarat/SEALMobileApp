using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SEALMobile.Views;

namespace SEALMobile
{
    public partial class App : Application
    {
        public App()
        {
            DevExpress.XamarinForms.Charts.Initializer.Init();

            InitializeComponent();
            //MainPage = new MainPage();

            MainPage = new NavigationPage(new LoginPage());
            //MainPage = new NavigationPage(new UserHomePage());
            //MainPage = new NavigationPage(new ProjectPage());
            //MainPage = new NavigationPage(new CreateProjectPage());

            //MainPage = new NavigationPage(new MyPage2("test"));
            //MainPage = new NavigationPage(new MyPage3("test"));
            //MainPage = new NavigationPage(new MyPage4());




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
