using System;
using System.IO;
using Microsoft.Research.SEAL;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;

namespace edge
{
    class Program
    {
        // create parms object
        public class ParmsReq
        {
            public string parms { get; set; }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("EDGE service DEMO!");
            
            // create EncryptionParameters
            EncryptionParameters parms = CreateParams();
            using MemoryStream parmsStream = new MemoryStream();
            
            // save to stream and convert to base64
            parms.Save(parmsStream);
            var parmsBase64 = ToBase64(parmsStream);

            // assign data to object
            // ParmsReq req = new ParmsReq{parms = parmsBase64};
            ParmsReq req = new ParmsReq();
            req.parms = parmsBase64;

            // convert parms object to json
            string json = JsonConvert.SerializeObject(req);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            // Console.WriteLine(json);

            // post parms to mobile
            var httpClient = new HttpClient();
            var parmsRes = await httpClient.PostAsync("http://localhost:9000/parms", content);

            Console.WriteLine("Parms has POST (edge to mobile)");

        }

        // configure encryption parameter
        public static EncryptionParameters CreateParams(){
            EncryptionParameters parms = new EncryptionParameters(SchemeType.CKKS);
            ulong polyModulusDegree = 8192;
            parms.PolyModulusDegree = polyModulusDegree;
            parms.CoeffModulus = CoeffModulus.Create(polyModulusDegree, new int[] { 60, 40, 40, 60 });
            return parms;
        }

        // convert Stream to StringBase64
        public static string ToBase64(MemoryStream data)
        {
            var dataAsString = Convert.ToBase64String(data.ToArray());
            return dataAsString;
        }

        // convert StringBase64 to Stream
        public static MemoryStream ToMemoryStream(string data)
        {
            var bytes = Convert.FromBase64String(data);
            var dataAsStream = (new MemoryStream(bytes));
            return dataAsStream;
        }

    }
}
