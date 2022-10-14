using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using SEALMobile.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace SEALMobile.Views
{
    public partial class UserHomePage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new ProjectsViewModel();
        }

        public UserHomePage()
        {
            InitializeComponent();
            Title = "HOME PAGE";
            BindingContext = new ProjectsViewModel();
            base.OnAppearing();

        }

        void Handle_CreateProject(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new CreateProjectPage(), true);
        }

        void ListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            Project project = (Project)ProjectListView.SelectedItem;
            Navigation.PushAsync(new ProjectPage(project), true);
        }
    }
}
