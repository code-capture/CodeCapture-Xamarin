using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.Media;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace GarudaHacks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageCapture : ContentPage
    {
        private string imagePath;

        string subscriptionKey = "ffc60f43d45049b185f8ab5c9b79c2d3";

        string endpoint = "https://centralindia.api.cognitive.microsoft.com/";
        public ImageCapture()
        {
            InitializeComponent();
        }
        private async void clickImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera Available", "Function To Be Built Yet", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                SaveToAlbum = true,
                //Directory = "Sample",
                //Name = "test.jpg",
                AllowCropping = false
            });

            if (file == null)
                return;

            imagePath = file.Path;

            //await DisplayAlert("File Location", file.Path, "OK");

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        private async void selectImage_Clicked(object sender, EventArgs e)
        {
            //Debug.WriteLine("Select The Image Button Works!!!");
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported",
                           "Sorry! Permission not granted to photos.", "OK");
                //return null;
            }

            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new
                Plugin.Media.Abstractions.PickMediaOptions
            { });

            //await DisplayAlert("File Location", file.Path, "OK");

            if (file == null)
                return;

            imagePath = file.Path;

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        private async void sendToCodeSpace_Clicked(object sender, EventArgs e)
        {
            if (langPicker.SelectedItem == null)
            {
                await DisplayAlert("Language Not Selected!!!", "Go Pick A Language!!!", "OK");
                return;
            }

            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await DisplayAlert("No Network Available", "Please Connect To Your Wifi Or Turn on Mobile Data", "OK");
            }

            //await DisplayAlert("Send To CodeSpace Button Works!!!","Function To Be Built Yet\n\nLanguage Selected => "+langPicker.SelectedItem,"OK");

            await ReadText(imagePath);
        }

        public async Task ReadText(string imageFilePath)
        {
            try
            {
                string uriBase = endpoint + "/vision/v3.0/read/analyze";
                await DisplayAlert("Start Read API", "Best Of Luck", "Thanks");
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string url = uriBase;

                HttpResponseMessage response;

                string operationLocation;

                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    response = await client.PostAsync(url, content);
                }

                if (response.IsSuccessStatusCode)
                    operationLocation =
                        response.Headers.GetValues("Operation-Location").FirstOrDefault();
                else
                {
                    string errorString = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("\n\nResponse:\n{0}\n",
                        JToken.Parse(errorString).ToString());
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
                    Debug.WriteLine("\nTimeout error.\n");
                    return;
                }

                await DisplayAlert("JSON Response", JToken.Parse(contentString).ToString(), "OK");
            }
            catch (Exception e)
            {
                Debug.WriteLine("\n" + e.Message);
            }
        }

        public byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}