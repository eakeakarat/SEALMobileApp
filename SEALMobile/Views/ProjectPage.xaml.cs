using System;
using System.Collections.Generic;
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
            //Navigation.PushAsync(new DashboardsPage(), true);
        }
    }
}
