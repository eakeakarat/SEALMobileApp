using System;
using System.Collections.Generic;
using SEALMobile.Models;
using Xamarin.Forms;

namespace SEALMobile.Views
{
    public partial class EdgesListPage : ContentPage
    {
        Project project;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new EdgesViewModel(project);
        }

        public EdgesListPage(Project pj)
        {
            InitializeComponent();
            project = pj;
            Title = project.projectname;
            BindingContext = new EdgesViewModel(project);
            base.OnAppearing();


        }

        void ListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            Edge edge = (Edge)EdgeListView.SelectedItem;
            Navigation.PushAsync(new EdgeDetailPage(edge,project.projectid), true);

        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new CreateEdgePage(project), true);
        }
    }
}
