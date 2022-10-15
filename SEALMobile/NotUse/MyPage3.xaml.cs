using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using Microsoft.Research.SEAL;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace SEALMobile
{

    public partial class MyPage3 : ContentPage
    {
        EncryptionParameters parms;
        SEALContext context;
        KeyGenerator keygen;
        PublicKey publicKey;
        RelinKeys relinKeys;
        SecretKey secretKey;
        string parmsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "parms.txt");
        string skPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "secretKey.txt");
        string rlkPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "relinKey.txt");
        string pkPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "publicKey.txt");
        string username = "";
        //string jsonRes = "";
        string jsonRes = "{ \"hostname\":\"3555961e-0ec9-4818-861e-a5cf7161b249\", \"hostIP\":\"api.netpie.io/v2/device\", \"hostPort\":0, \"endPoint\":\"shadow/data\", \"oneTimePassword\":\"YffZjZVuqYbmQzkHVaLxqNcfaPvErdxS\" }";

        public MyPage3(string name)
        {
            InitializeComponent();
            username = name;
            Title = "Hello " + username;
        }
        // create parms object
        public class ParmsReq
        {
            public string parms { get; set; }
        }

        // create publicKey object
        public class PublicKeyReq
        {
            public string pkBase64 { get; set; }
        }

        // create relinKeys object
        public class RelinKeyReq
        {
            public string rlkBase64 { get; set; }
        }

        public class NetpiePacking
        {
            public object data { get; set; }
        }

        public class Host
        {
            public string hostname { get; set; } = null;
            public string hostIP { get; set; } = null;
            public int hostPort { get; set; } = 0;
            public string endpoint { get; set; } = null;
            public string oneTimePassword { get; set; } = null;

            public override string ToString()
            {
                return "hostname =>\t" + hostname.ToString() +
                    "\nhostIP =>\t" + hostIP.ToString() +
                    "\nhostPort =>\t" + hostPort.ToString() +
                    "\nendpoint =>\t" + endpoint.ToString() +
                    "\nOTP =>\t\t" + oneTimePassword.ToString();
            }
            public string getURI()
            {
                string uri = "https://";

                if (hostIP.Contains("://"))
                {
                    hostIP = hostIP.Remove(0, hostIP.IndexOf("://") + 3);
                }
                uri += hostIP;

                if (hostPort != 0)
                {
                    uri += ":" + hostPort;
                }

                if (!endpoint[0].Equals('/'))
                {
                    uri += "/";
                }

                return uri += endpoint;

            }
            public string getAuthentication()
            {
                return hostname + ":" + oneTimePassword;
            }

        }


        async void Handle_Scanner(object sender, System.EventArgs e)
        {
            //Navigation.ShowPopup(new MyScanner());

            var result = await Navigation.ShowPopupAsync(new MyScanner());
            string text = result.ToString();
            jsonRes = text;
            x_label.Text = text;
        }

        public async void Handle_KeyGenerator(object sender, System.EventArgs e)
        {
            var httpClient = new HttpClient();

            // Data transfer via HTTP to NETPIE
            //string uri = "https://api.netpie.io/v2/device/shadow/data";
            //string authentication = "3555961e-0ec9-4818-861e-a5cf7161b249:YffZjZVuqYbmQzkHVaLxqNcfaPvErdxS";

            // QR Scaning
            JObject hostJson = JObject.Parse(jsonRes);
            Host host = hostJson.ToObject<Host>();
            string uri = host.getURI();
            string authentication = host.getAuthentication();
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authentication));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);


            // initialize stream
            using MemoryStream pkStream = new MemoryStream();
            using MemoryStream rlkStream = new MemoryStream();
            using MemoryStream skStream = new MemoryStream();

            // receive parms to configure context
            parms = new EncryptionParameters();
            var response = await (await httpClient.GetAsync(uri)).Content.ReadAsStringAsync();
            JObject resObj = JObject.Parse(response);
            string resStr = resObj.ToObject<NetpiePacking>().data.ToString();
            string parmsRes = (JObject.Parse(resStr)).ToObject<ParmsReq>().parms.ToString();
            //string parmsRes = "XqEQBAACAABTAAAAAAAAACi1L/0gidUBADQCAgAgAAQAXqEQGAAAAYD+/////w/A9P//wP3/AAAAAAAAAAAIAEv4xM4aT0WTnRNjynW5Awy+ABE=";
            MemoryStream parmsStream = ToMemoryStream(parmsRes);
            parmsStream.Seek(0, SeekOrigin.Begin);
            parms.Load(parmsStream);
            context = new SEALContext(parms, true, SecLevelType.None);

            // create key generator to create keys(SecretKey, PublicKey, RelinKeys)
            keygen = new KeyGenerator(context);
            secretKey = keygen.SecretKey;
            keygen.CreatePublicKey(out publicKey);
            keygen.CreateRelinKeys(out relinKeys);

            // save publicKey and relinKeys to stream and convert to stringBase64
            publicKey.Save(pkStream);
            var pkBase64 = ToBase64(pkStream);
            relinKeys.Save(rlkStream);
            var rlkBase64 = ToBase64(rlkStream);

            secretKey.Save(skStream);
            var skBase64 = ToBase64(skStream);
            File.WriteAllText(parmsPath, parmsRes);
            File.WriteAllText(skPath, skBase64);
            File.WriteAllText(pkPath, pkBase64);
            File.WriteAllText(rlkPath, rlkBase64);

            // assign data to key object
            PublicKeyReq pkReq = new PublicKeyReq { pkBase64 = pkBase64 };
            RelinKeyReq rlkReq = new RelinKeyReq { rlkBase64 = rlkBase64 };

            // Test data string (origin keyBase64 are longer !! [[ unsolve ]] )
            //pkReq = new PublicKeyReq { pkBase64 = "PK from Mobile KeyGenBtn" };
            //rlkReq = new RelinKeyReq { rlkBase64 = "RLK from Mobile KeyGenBtn" };

            // Packing with object list
            //NetpiePacking test = new NetpiePacking();
            //test.data = new object[] { pkReq, rlkReq };
            //string npJson = JsonConvert.SerializeObject(test, Formatting.Indented);

            // Packing obj then post and put a new field to [ same obj in NETPIE ]
            NetpiePacking pkNetpie = new NetpiePacking { data = pkReq };
            string pkJsonNp = JsonConvert.SerializeObject(pkNetpie, Formatting.Indented);
            StringContent pkNpContent = new StringContent(pkJsonNp, Encoding.UTF8, "application/json");

            var pkNpRes = await httpClient.PutAsync(uri, pkNpContent);

            NetpiePacking rlkNetpie = new NetpiePacking { data = rlkReq };
            string rlkJsonNp = JsonConvert.SerializeObject(rlkNetpie, Formatting.Indented);
            StringContent rlkNpContent = new StringContent(rlkJsonNp, Encoding.UTF8, "application/json");
            // Put to insert new filed in device [ same obj ]
            var rlkNpRes = await httpClient.PutAsync(uri, rlkNpContent);


            x_label.Text = "SEND to NETPIE\nPK: " + pkNpRes.StatusCode.ToString() + ", RLK: " + rlkNpRes.StatusCode.ToString();

        }

        public async void Handle_NP(object sender, System.EventArgs e)
        {
            // initialize uri and authentication
            JObject hostJson = JObject.Parse(jsonRes);
            Host host = hostJson.ToObject<Host>();
            //Console.WriteLine(host.ToString() + "\n" + host.getURI() + "\n" + host.getAuthentication());


            var http = new HttpClient();
            // authentication = "clientID:Token";
            //string uri = "https://api.netpie.io/v2/device/shadow/data";
            //string authentication = "3555961e-0ec9-4818-861e-a5cf7161b249:YffZjZVuqYbmQzkHVaLxqNcfaPvErdxS";

            // FROM jsonRes
            string uri = host.getURI();
            string authentication = host.getAuthentication();

            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authentication));
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

            string input = entry.Text;
            NetpiePacking netpiePacking = new NetpiePacking { data = new PublicKeyReq { pkBase64 = input } };
            String json = JsonConvert.SerializeObject(netpiePacking, Formatting.Indented);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await http.PutAsync(uri, stringContent);
            k_label.Text = "Post: " + httpResponse.StatusCode.ToString();

            //k_label.Text = "URI: " + uri + "\nAUT: " + authentication;

        }

        public void Handle_callSK(object sender, System.EventArgs e)
        {
            string l = File.ReadAllText(skPath);
            k_label.Text = "Head20: " + l.Substring(0, 20) + "\nLast20: " + l.Substring(l.Length - 20) + "\nLength: " + l.Length;

        }
        public void Handle_callPK(object sender, System.EventArgs e)
        {
            string l = File.ReadAllText(pkPath);
            k_label.Text = "Head20: " + l.Substring(0, 20) + "\nLast20: " + l.Substring(l.Length - 20) + "\nLength: " + l.Length;

        }
        public void Handle_callRLK(object sender, System.EventArgs e)
        {
            string l = File.ReadAllText(rlkPath);
            k_label.Text = "Head20: " + l.Substring(0, 20) + "\nLast20: " + l.Substring(l.Length - 20) + "\nLength: " + l.Length;

        }

        public EncryptionParameters GetLocalEncryptionParameters()
        {
            EncryptionParameters encryptionParameters = new EncryptionParameters();

            string tmp = File.ReadAllText(parmsPath);
            MemoryStream memoryStream = ToMemoryStream(tmp);
            memoryStream.Seek(0, SeekOrigin.Begin);

            encryptionParameters.Load(memoryStream);
            return encryptionParameters;
        }

        public SecretKey GetLocalSecretKey(SEALContext context)
        {
            SecretKey secretKey = new SecretKey();

            string tmp = File.ReadAllText(skPath);
            MemoryStream memoryStream = ToMemoryStream(tmp);
            memoryStream.Seek(0, SeekOrigin.Begin);

            secretKey.Load(context, memoryStream);
            return secretKey;

        }

        public PublicKey GetLocalPublicKey(SEALContext context)
        {
            PublicKey publicKey = new PublicKey();

            string tmp = File.ReadAllText(pkPath);
            MemoryStream memoryStream = ToMemoryStream(tmp);
            memoryStream.Seek(0, SeekOrigin.Begin);

            publicKey.Load(context, memoryStream);
            return publicKey;

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
            var dataAsStream = new MemoryStream(bytes);
            return dataAsStream;
        }

        void Handle_DashboardPg(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MyPage4(), true);
        }
    }
}
