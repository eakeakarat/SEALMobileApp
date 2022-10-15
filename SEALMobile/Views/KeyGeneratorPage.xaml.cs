using System;
using System.Collections.Generic;
using SEALMobile.Models;
using Xamarin.Forms;
using Microsoft.Research.SEAL;
using System.IO;

namespace SEALMobile.Views
{

    public partial class KeyGeneratorPage : ContentPage
    {
        ContextSizeViewModel CSL;
        ContextSize contextSize = null;
        EncryptionParameters encParms;
        SEALContext context;

        string skPath;
        string rlkPath;
        string pkPath;
        string cotPath;
        string hashPath;
        string scalePath;

        Project project;

        public KeyGeneratorPage(Project pj)
        {
            InitializeComponent();
            CSL = new ContextSizeViewModel();
            //BindingContext = CSL;

            Dropdown.BindingContext = CSL;
            project = pj;
            var pjName = project.projectid;

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, pjName);

            if (!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }

            skPath = Path.Combine(documents, pjName, "secretKey.txt");
            rlkPath = Path.Combine(documents, pjName, "relinKeys.txt");
            pkPath = Path.Combine(documents, pjName, "publicKey.txt");
            cotPath = Path.Combine(documents, pjName, "context.txt");
            hashPath = Path.Combine(documents, pjName, "hash.txt");
            scalePath = Path.Combine(documents, pjName, "scale.txt");

        }
        void Picker_SelectedContextSize(object sender, EventArgs e)
        {

            var collection = CSL.SizeList;
            int selectedIndex = Dropdown.SelectedIndex;
            if (selectedIndex != -1)
            {
                //selected context size
                contextSize = collection[selectedIndex];
            }

        }

        async void KeyGen_Button_Clicked(object sender, System.EventArgs e)
        {
            if (contextSize != null)
            {
                encParms = new EncryptionParameters(SchemeType.CKKS);
                ulong polyModulusDegree = contextSize.PolyModulusDegree;
                encParms.PolyModulusDegree = polyModulusDegree;
                encParms.CoeffModulus = CoeffModulus.Create(polyModulusDegree, contextSize.CoeffModulus);
                context = new SEALContext(encParms, true, SecLevelType.None);
                
                KeyGenerator keyGenerator = new KeyGenerator(context);
                SecretKey secretKey = keyGenerator.SecretKey;
                PublicKey publicKey = new PublicKey();
                RelinKeys relinKeys = new RelinKeys();

                keyGenerator.CreatePublicKey(out publicKey);
                keyGenerator.CreateRelinKeys(out relinKeys);

                MemoryStream skStream = new MemoryStream();
                MemoryStream pkStream = new MemoryStream();
                MemoryStream rlkStream = new MemoryStream();
                MemoryStream cotStream = new MemoryStream();

                secretKey.Save(skStream);
                publicKey.Save(pkStream);
                relinKeys.Save(rlkStream);
                encParms.Save(cotStream);


                var skBase64 = ToBase64(skStream);
                var pkBase64 = ToBase64(pkStream);
                var rlkBase64 = ToBase64(rlkStream);
                var cotBase64 = ToBase64(cotStream);

                File.WriteAllText(skPath, skBase64);
                File.WriteAllText(pkPath, pkBase64);
                File.WriteAllText(rlkPath, rlkBase64);
                File.WriteAllText(cotPath, cotBase64);
                File.WriteAllText(scalePath, contextSize.scale + "");

                GenKeyBtn.Text = "Key Generated !";
                Dropdown.SelectedIndex = 0;

            }
            else
            {
                await DisplayAlert("Alert", "Please choose context size before generate keys", "Got it!");
            }
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
