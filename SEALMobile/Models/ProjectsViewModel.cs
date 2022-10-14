using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms.Internals;
using System.Linq;
using SEALMobile.Models;
using Xamarin.Forms;
using System.Net.Http.Headers;
using GraphQL.Client.Serializer.Newtonsoft;
using System.IO;
using GraphQL.Client.Http;
using GraphQL;

namespace SEALMobile.Models
{
    public class ProjectsViewModel
    {
        public ObservableCollection<Project> Projects { get; set; }
        public Project[] projectsList;

        public ProjectsViewModel()
        {
            Projects = new ObservableRangeCollection<Project>();
            LoadMore();
        }
        async void LoadMore()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documents, "UserInfo", "access_token.txt");
            var token = File.ReadAllText(path);

            var graphQLHttp = new GraphQLHttpClient("http://fhe.netpie.io:30010/", new NewtonsoftJsonSerializer());
            graphQLHttp.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var projectsREQ = new GraphQLRequest
            {
                Query = @"query {
                    projectList(limit: 5){
                        projectid,projectname,description
                    }
                }"
            };

            var graphQLResponse = await graphQLHttp.SendQueryAsync<dataProject>(projectsREQ);
            var res = graphQLResponse.Data.projectList;
            projectsList = res;

            foreach (Project p in res)
            {
                Projects.Add(p);
            }

        }

        public Project findProject(string id)
        {
            Project project = new Project();
            foreach (Project p in projectsList)
            {
                if (p.projectid == id)
                {
                    project = p;
                }
            }
            return project;
        }


    }
    public class dataProject
    {
        public Project[] projectList { get; set; }
    }

    public class Project
    {
        public string projectid { get; set; }
        public string projectname { get; set; }
        public string description { get; set; }
    }
}
