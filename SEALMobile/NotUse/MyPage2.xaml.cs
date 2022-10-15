
using Xamarin.Forms;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using SEALMobile.Models;

namespace SEALMobile
{
    public partial class MyPage2 : ContentPage
    {
        string username = "";


        //public List<string>  

        public MyPage2(string name)
        {
            InitializeComponent();
            username = name;
            Title = username + "'s home page";

            //this.BindingContext = new[] { "Project 1", "Project 2", "Project 3" };

            //var projectsView = new ProjectsView();
            
            //Project project1 = new Project() { ID = "001", Name = "Project 001", Description = "test 1 project" };
            //Project project2 = new Project() { ID = "002", Name = "Project 002", Description = "test 2 project" };

            //projects.list = new List<Project> { project1, project2 };



            //this.BindingContext = projectsView;


        }

        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            //Debug.WriteLine("Tapped: " + e.Item);
            ((ListView)sender).SelectedItem = null; // de-select the row
        }



        void Handle_Dashboard(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MyPage4(), true);
        }
        void Handle_CreateProject(object sender, System.EventArgs e)
        {
            //Navigation.PushAsync(new MyPage3(username), true);
        }

        void Handle_PairEdge(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MyPage3(username), true);
        }
        void Handle_logout(object sender, System.EventArgs e)
        {
            Navigation.PopToRootAsync();
        }

    }
}
