using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using CodeCapture.Models;
using CodeCapture.Models.CompileModels;
using System.Diagnostics;

namespace CodeCapture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtractText : ContentPage
    {
        string extract = "";
        public ExtractText(string imagePath)
        {
            InitializeComponent();

            var langList = new List<string>();
            langList.Add("C++ (GCC 9.1.0)");
            langList.Add("Java (JDK 11.0.4)");
            langList.Add("JavaScript(Node.js v12.11.1)");
            langList.Add("Python (v3.7.4)");

            langPicker.ItemsSource = langList;

            ReadText(imagePath);
        }


        //Uses the Read API on the image to extract text from images images 
        public async void ReadText(string imageFilePath)
        {
            try
            {
                Models.Secrets secrets = new Models.Secrets();
                string subscriptionKey = secrets.READ_subscriptionKey;
                string uriBase = secrets.READ_endpoint + "/vision/v3.0/read/analyze";
                
                //await DisplayAlert("Start Read API", "Best Of Luck", "Thanks");
                
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string url = uriBase;

                HttpResponseMessage response;

                string operationLocation;

                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    response = await client.PostAsync(url, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    //await DisplayAlert("POST Request Successful", "", "OK");
                    operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();
                }
                else
                {
                    string errorString = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("\n\nResponse:\n{0}\n", JToken.Parse(errorString).ToString(), "OK");
                    return;
                }

                string contentString;
                int i = 0;
                do
                {
                    System.Threading.Thread.Sleep(1000);
                    response = await client.GetAsync(operationLocation);
                    contentString = await response.Content.ReadAsStringAsync();
                    ++i;
                }
                while (i < 60 && contentString.IndexOf("\"status\":\"succeeded\"") == -1);

                if (i == 60 && contentString.IndexOf("\"status\":\"succeeded\"") == -1)
                {
                    await DisplayAlert("\nTimeout error.\n", "", "OK");
                    return;
                }

                await DisplayTextAsync(JObject.Parse(contentString).ToString());
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", "\n" + e.Message, "OK");
            }
        }


        async Task DisplayTextAsync(string content)
        {
            Models.ReadModels.Root response = JsonConvert.DeserializeObject<Models.ReadModels.Root>(content);
            foreach (var result in response.analyzeResult.readResults)
            {
                foreach (var line in result.lines)
                {
                    extract += line.text + "\n";
                }
            }
            await DisplayAlert("Extracted Text", extract, "Show In Editor");
            editor.Text = extract;
            await DisplayAlert("Special Instructions", "For C++:\n\n\tDon't forget to add: \n\t\t#include <iostream>\n\t\tusing namespace std;\n\nFor Java:\n\t\tMake sure that only the Main class is 'public'\n\t\tYou don't need to add a custom package name", "OK");
        }

        //Converts Image to Byte Array and return it
        public byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        private async void returnButton_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void compileButton_Clicked(object sender, EventArgs e)
        {
            string lang = "", vers = "";

            if(langPicker.SelectedItem!=null)
            {
                if (langPicker.SelectedIndex == 0)
                {
                    lang = "cpp";
                    vers = "4";
                }

                else if (langPicker.SelectedIndex == 1)
                {
                    lang = "java";
                    vers = "3";
                }
                else if (langPicker.SelectedIndex == 2)
                {
                    lang = "nodejs";
                    vers = "3";
                }
                else if (langPicker.SelectedIndex == 3)
                {
                    lang = "python3";
                    vers = "3";
                }

                if (Connectivity.NetworkAccess == NetworkAccess.None)
                {
                    await DisplayAlert("No Network Available", "Please Connect To Your Wifi Or Turn on Mobile Data", "OK");
                }

                else await Navigation.PushModalAsync(new CompileCode(editor.Text, stdin.Text, lang, vers));
            }
            else
            {
                await DisplayAlert("Please select a language","", "OK");
            }
        }
    }
}
