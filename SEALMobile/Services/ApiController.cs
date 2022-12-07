using System;
using System.IO;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using SEALMobile.Models;
using SEALMobile.Views;

namespace SEALMobile.Services
{
    public class ApiController : WebApiController
    {
        string projectid { get; set; }
        SEALENY seal;
        string path;
        string webtoken;
        public ApiController()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(documents, "selectedProject.txt");
            webtoken = Path.Combine(documents, "UserInfo", "access_token.txt");

        }

        private void setProjectid(string id)
        {
            projectid = id;
            Console.WriteLine("PRID ___ " + projectid);
        }

        [Route(HttpVerbs.Get, "/cipher")]
        public string GetCipher()
        {
            Console.WriteLine("GET CIPHER RES");
            return "Cipher";
        }

        [Route(HttpVerbs.Post, "/projectid")]
        public async Task PostProjectid()
        {
            var data = HttpContext.GetRequestBodyAsStringAsync().Result;

            //var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var path = Path.Combine(documents, "selectedProject.txt");

            File.WriteAllText(path, data.ToString());



            //seal = new SEALENY(projectid);

            Console.WriteLine("POST projectid");
            Console.WriteLine(projectid + " ____TYPE" + projectid.GetType());

        }


        [Route(HttpVerbs.Post, "/cipher")]
        public async Task<string> PostJsonData()
        {
            var data = HttpContext.GetRequestBodyAsStringAsync().Result;

            Console.WriteLine("POST CIPHER");
            Console.WriteLine(data.ToString());
            Console.WriteLine(projectid);

            var projectida = File.ReadAllText(path);
            Console.WriteLine(projectida);

            //SEAL DECRYPTED
            var seal = new SEALENY(projectida);
            var encrypt = seal.getEncryptText();
            var decrypt = seal.decryptText(encrypt);
            //var decrypt = data + " was DECRYPTED [SEAL]";

            Console.WriteLine(decrypt);

            return decrypt;
        }


        [Route(HttpVerbs.Get, "/webtoken")]
        public string GetJwtToken()
        {
            string token = File.ReadAllText(webtoken);

            Console.WriteLine("GET JWT token RES");
            return token;
        }



    }
}
