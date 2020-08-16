using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.Media;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Xml.XPath;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;

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

            await MakeRequestAsync();
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

        private async Task MakeRequestAsync()
        {
            await DisplayAlert("Endpoint", endpoint, "OK");
            try
            {
                RunAsync(endpoint, subscriptionKey).Wait(5000);
                Debug.WriteLine(endpoint);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        public async Task RunAsync(string endpoint, string Key)
        {
            await DisplayAlert("Endpoint", endpoint, "OK");
            ComputerVisionClient computerVision = new ComputerVisionClient(new ApiKeyServiceClientCredentials(Key))
            {
                Endpoint = endpoint
            };
            await OCRFromStreamAsync(computerVision);
        }

        private async Task OCRFromStreamAsync(ComputerVisionClient computerVision)
        {
            if (!File.Exists(imagePath))
            {
                Debug.WriteLine("\nUnable to open or read local image path:\n{0} \n", imagePath);
                return;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                OcrResult analysis = await computerVision.RecognizePrintedTextInStreamAsync(true, imageStream);
                Debug.WriteLine(imagePath);
                DisplayResults(analysis);
            }
        }

        private void DisplayResults(OcrResult analysis)
        {
            //text
            Debug.WriteLine("Text:");
            Debug.WriteLine("Language: " + analysis.Language);
            Debug.WriteLine("Text Angle: " + analysis.TextAngle);
            Debug.WriteLine("Orientation: " + analysis.Orientation);
            Debug.WriteLine("Text regions: ");
            foreach (var region in analysis.Regions)
            {
                Debug.WriteLine("Region bounding box: " + region.BoundingBox);
                foreach (var line in region.Lines)
                {
                    Debug.WriteLine("Line bounding box: " + line.BoundingBox);

                    foreach (var word in line.Words)
                    {
                        Debug.WriteLine("Word bounding box: " + word.BoundingBox);
                        Debug.WriteLine("Text: " + word.Text);
                    }
                    Debug.WriteLine("\n");
                }
                Debug.WriteLine("\n \n");
            }
        }
    }
}