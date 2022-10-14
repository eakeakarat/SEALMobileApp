using System;
using System.Collections.Generic;
using SEALMobile.Models;
using Xamarin.Forms;

namespace SEALMobile.Views
{
    public partial class EdgesListPage : ContentPage
    {
        Project project;
        EdgesViewModel viewModel;
        public EdgesListPage(Project pj)
        {
            InitializeComponent();
            project = pj;
            Title = project.projectname;
            viewModel = new EdgesViewModel(project);
            BindingContext = viewModel;


        }

        void ListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            Edge edge = (Edge)EdgeListView.SelectedItem;
            Navigation.PushAsync(new EdgeDetailPage(edge,project.projectid), true);

        }
    }
}
