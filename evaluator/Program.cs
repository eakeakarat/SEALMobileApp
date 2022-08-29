using System;
using System.IO;
using Microsoft.Research.SEAL;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;

namespace evaluator
{
    class Program
    {
        // create result object
        public class ResultReq
        {
            public string result { get; set; }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("EVALUATOR service DEMO!");

            // receive parms and load to local parms
            EncryptionParameters parms = new EncryptionParameters();
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:9000/parms");
            string parmsRes = await response.Content.ReadAsStringAsync();
            MemoryStream parmsStream = ToMemoryStream(parmsRes);
            parms.Load(parmsStream);

            // configure context from parms
            SEALContext context = new SEALContext(parms);

            // receive relinKeys
            response = await httpClient.GetAsync("http://localhost:9000/rlk");
            string rlkRes = await response.Content.ReadAsStringAsync();
            MemoryStream rlkStream = ToMemoryStream(rlkRes);

            // receive publicKey
            response = await httpClient.GetAsync("http://localhost:9000/pk");
            string pkRes = await response.Content.ReadAsStringAsync();
            MemoryStream pkStream = ToMemoryStream(pkRes);

            // create local puclicKey and relinKeys
            // load received key to configure local keys
            PublicKey publicKey = new PublicKey();
            RelinKeys relinKeys = new RelinKeys();
            publicKey.Load(context, pkStream);
            relinKeys.Load(context, rlkStream);

            // configure evaluator encoder and encryptor
            Evaluator evaluator = new Evaluator(context);
            CKKSEncoder encoder = new CKKSEncoder(context);
            Encryptor encryptor = new Encryptor(context, publicKey);

            // create plainText and cipherText
            Plaintext plaintext1 = new Plaintext();
            Plaintext plaintext2 = new Plaintext();
            Ciphertext ciphertext1 = new Ciphertext();
            Ciphertext ciphertext2 = new Ciphertext();
            Ciphertext ciphertextResult = new Ciphertext();

            //simulate A and B
            long a = 2;
            long b = 2;

            // configure scale to encode
            double scale = Math.Pow(2.0, 30);
            encoder.Encode(a,scale, plaintext1);
            encoder.Encode(b,scale, plaintext2);

            // encrypt plainText to cipherText
            encryptor.Encrypt(plaintext1, ciphertext1);
            encryptor.Encrypt(plaintext2, ciphertext2);

            // compute A^2 + B^2
            evaluator.Multiply(ciphertext1, ciphertext1, ciphertext1);
            evaluator.Multiply(ciphertext2, ciphertext2, ciphertext2);
            evaluator.Add(ciphertext1, ciphertext2, ciphertextResult);

            // relinearize cipherText
            evaluator.Relinearize(ciphertextResult, relinKeys, ciphertextResult);

            // keep resultCipher to Base64
            MemoryStream resultStream = new MemoryStream();
            ciphertextResult.Save(resultStream);
            string resultBase64 = ToBase64(resultStream);

            // assign data to object
            ResultReq resultReq = new ResultReq{result = resultBase64};
            // ResultReq resultReq = new ResultReq();
            // resultReq.result = resultBase64
            
            // convert resultCipher object to json
            // post resultCipher to cloud(or somewhere)
            string resultJson = JsonConvert.SerializeObject(resultReq, Formatting.Indented);
            StringContent resultContent = new StringContent(resultJson, Encoding.UTF8, "application/json");
            var resultRes = await httpClient.PostAsync("http://localhost:9000/result", resultContent);
            // Console.WriteLine(resultJson)
            Console.WriteLine("Result Cipher has POST (cloud)");

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
