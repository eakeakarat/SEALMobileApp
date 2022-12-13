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
        string encPath;
        public ApiController()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(documents, "selectedProject.txt");
            webtoken = Path.Combine(documents, "UserInfo", "access_token.txt");
            encPath = Path.Combine(documents, "encTEST.txt");

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

        
        [Route(HttpVerbs.Get, "/mockcipher")]
        public async Task<string> GetMcipher()
        {
            var projectida = File.ReadAllText(path);
            Console.WriteLine("GET:  " + projectida);

            //SEAL DECRYPTED
            var seal = new SEALENY(projectida);
            // Mocking encrypt TEXT
            string encrypt = seal.getEncryptText();
            Console.WriteLine("GET:   " + encrypt.Substring(0, 20));
            return encrypt;

        }


        [Route(HttpVerbs.Post, "/cipher")]
        public async Task<string> PostJsonData()
        {
            var data = HttpContext.GetRequestBodyAsStringAsync().Result;

            Console.WriteLine("POST CIPHER");
            //Console.WriteLine(data.ToString());

            var projectida = File.ReadAllText(path);
            //Console.WriteLine(projectida);

            //SEAL DECRYPTED
            var seal = new SEALENY(projectida);

            //Remove " from data
            //string encrypt = data.Remove(data.Length - 1, 1);
            //encrypt = encrypt.Remove(0, 1);


            string encrypt = data;
            File.WriteAllText(encPath, encrypt);

            //Console.WriteLine(encrypt);
            //Console.WriteLine("First 20 char: "+ encrypt.Substring(0, 20));


            var decrypt = seal.decryptText(encrypt);
            //var decrypt = data + " was DECRYPTED [SEAL]";
            Console.WriteLine("DECRYPT ==>> " + decrypt);

            return decrypt;
        }

        
        [Route(HttpVerbs.Get, "/webtoken")]
        public string GetJwtToken()
        {
            string token = File.ReadAllText(webtoken);

            Console.WriteLine("GET JWT token RES ==" + token.Substring(0,20));
            return token;
        }



    }
}
