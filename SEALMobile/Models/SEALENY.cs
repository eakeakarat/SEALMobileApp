using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Research.SEAL;

namespace SEALMobile.Models
{
    public class SEALENY
    {
        EncryptionParameters parms;
        SEALContext context;
        SecretKey secretKey;
        CKKSEncoder encoder;
        Decryptor decryptor;
        Project project;

        int scale;

        PublicKey publicKey;

        public SEALENY(Project pj)
        {

            project = pj;
            var pjName = project.projectid;

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, pjName);

            if (!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }

            var skPath = Path.Combine(documents, pjName, "secretKey.txt");
            var cotPath = Path.Combine(documents, pjName, "context.txt");
            var pkPath = Path.Combine(documents, pjName, "publicKey.txt");
            var scalePath = Path.Combine(documents, pjName, "scale.txt");


            var cotBase64 = File.ReadAllText(cotPath);
            var skBase64 = File.ReadAllText(skPath);
            var pkBase64 = File.ReadAllText(pkPath);
            var scaleBase64 = File.ReadAllText(scalePath);


            MemoryStream skStream = ToMemoryStream(skBase64);
            MemoryStream cotStream = ToMemoryStream(cotBase64);
            MemoryStream pkStream = ToMemoryStream(pkBase64);

            parms = new EncryptionParameters();
            parms.Load(cotStream);
            context = new SEALContext(parms, true, SecLevelType.None);


            secretKey = new SecretKey();
            secretKey.Load(context, skStream);

            encoder = new CKKSEncoder(context);
            decryptor = new Decryptor(context, secretKey);

            publicKey = new PublicKey();
            publicKey.Load(context, pkStream);

            scale = int.Parse(scaleBase64);

        }

        public string decryptText(string text)
        {
            List<double> result = new List<double>();
            Ciphertext cipher = new Ciphertext();
            Plaintext plain = new Plaintext();
            MemoryStream textStream = ToMemoryStream(text);
            cipher.Load(context, textStream);

            decryptor.Decrypt(cipher, plain);
            encoder.Decode(plain, result);

            return result[0].ToString();
        }

        public string encryptText()
        {
            float a = 1;
            float b = 1;

            encoder = new CKKSEncoder(context);
            var evaluator = new Evaluator(context);
            var encryptor = new Encryptor(context, publicKey);
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

            MemoryStream resultStream = new MemoryStream();
            resultCipher.Save(resultStream);
            var x = ToBase64(resultStream);

            return x;
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

    }
}
