using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using Microsoft.Research.SEAL;
using Newtonsoft.Json;
using System.IO;

namespace SEALMobile

{
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


    public partial class MainPage : ContentPage
    {
        // initialize variable
        EncryptionParameters parms;
        SEALContext context;
        KeyGenerator keygen;
        PublicKey publicKey;
        RelinKeys relinKeys;
        SecretKey secretKey;
        Encryptor encryptor;
        Evaluator evaluator;
        Decryptor decryptor;
        CKKSEncoder encoder;
        double scale = Math.Pow(2.0, 30);

        public MainPage()
        {
            InitializeComponent();
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

        // generate key button
        async void HandleKeyGenBtn(object sender, System.EventArgs e)
        {
            // initialize stream
            using MemoryStream pkStream = new MemoryStream();
            using MemoryStream rlkStream = new MemoryStream();

            // receive parms to configure context
            parms = new EncryptionParameters();
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:9000/parms");
            string parmsRes = await response.Content.ReadAsStringAsync();
            //string parmsRes = "XqEQBAACAABTAAAAAAAAACi1L/0gidUBADQCAgAgAAQAXqEQGAAAAYD+/////w/A9P//wP3/AAAAAAAAAAAIAEv4xM4aT0WTnRNjynW5Awy+ABE=";
            //string parmsRes = "XqEQBAACAABjAAAAAAAAACi1L/0g6VUCALQCAgAgAAgAXqEQBBgAAAABgP7/////D8DW//9A5wDowPP0/f8AAAAAAAAAAAwAK/MJqA/UDNQNKG2n0qai885xFGYsKwPELSAC";
            MemoryStream parmsStream = ToMemoryStream(parmsRes);
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

            // assign data to key object
            PublicKeyReq pkReq = new PublicKeyReq { pkBase64 = pkBase64 };
            RelinKeyReq rlkReq = new RelinKeyReq { rlkBase64 = rlkBase64 };

            // send publicKey to Edge
            string pkJson = JsonConvert.SerializeObject(pkReq, Formatting.Indented);
            StringContent pkContent = new StringContent(pkJson, Encoding.UTF8, "application/json");
            var pkRes = await httpClient.PostAsync("http://localhost:9000/pk", pkContent);

            // send relinKey to Evaluator
            string rlkJson = JsonConvert.SerializeObject(rlkReq, Formatting.Indented);
            StringContent rlkContent = new StringContent(rlkJson, Encoding.UTF8, "application/json");
            var rlkRes = await httpClient.PostAsync("http://localhost:9000/rlk", rlkContent);

            k_Label.Text = "Key Generated";
        }

        // compute button { do a^2 + b^2 from input field (in app compute) }
        // encode encrypt compute decrypt and decode in app
        void HandleComputeBtn(object sender, System.EventArgs e)
        {
            // receive data from input field
            float a = float.Parse(a_value.Text);
            float b = float.Parse(b_value.Text);

            // configure encoder evaluator encryptor decryptor
            encoder = new CKKSEncoder(context);
            evaluator = new Evaluator(context);
            encryptor = new Encryptor(context, publicKey);
            decryptor = new Decryptor(context, secretKey);

            // encode received data to plainText
            using Plaintext plain1 = new Plaintext();
            using Plaintext plain2 = new Plaintext();
            encoder.Encode(a, scale, plain1);
            encoder.Encode(b, scale, plain2);

            // encrypt plainText to cipherText
            using Ciphertext cipher1 = new Ciphertext();
            using Ciphertext cipher2 = new Ciphertext();
            encryptor.Encrypt(plain1, cipher1);
            encryptor.Encrypt(plain2, cipher2);

            using Ciphertext resultCipher = new Ciphertext();
            using Ciphertext resultCipher2 = new Ciphertext();

            // compute A^2 + B^2
            evaluator.Multiply(cipher1, cipher1, cipher1);
            evaluator.Multiply(cipher2, cipher2, cipher2);
            evaluator.Add(cipher1, cipher2, resultCipher);

            //ulong t = 2;
            //evaluator.Exponentiate(resultCipher, t, relinKeys, resultCipher2);

            // decrypt and decode to get result
            using Plaintext resultPlain = new Plaintext();
            List<double> result = new List<double>();
            decryptor.Decrypt(resultCipher, resultPlain);
            encoder.Decode(resultPlain, result);
            Console.WriteLine(result[0]);
            
            cr_Label.Text = "A^2 + B^2 = " + (result[1]).ToString("0.000000");
        }

        // receive cipher result (already compute)
        // decrypt and decode in app
        async void HandleDecryptedBtn(object sender, System.EventArgs e)
        {
            using Ciphertext resultCipher = new Ciphertext();
            using Plaintext resultPlain = new Plaintext();

            // receive computed result from cloud
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:9000/result");
            string resultRes = await response.Content.ReadAsStringAsync();
            MemoryStream resultStream = ToMemoryStream(resultRes);
            resultCipher.Load(context, resultStream);

            // configure decryptor and decoder
            decryptor = new Decryptor(context, secretKey);
            encoder = new CKKSEncoder(context);

            // decrypt and decode to get result
            decryptor.Decrypt(resultCipher, resultPlain);
            List<double> result = new List<double>();
            encoder.Decode(resultPlain, result);
            Console.WriteLine(result[0]);

            dr_Label.Text = "" + (result[1]).ToString("0.000000");
        }
    }
}
