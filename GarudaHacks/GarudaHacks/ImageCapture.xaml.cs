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

            //await MakeRequestAsync();
            
            await ReadText(imagePath);
        }

        public async Task ReadText(string imageFilePath)
        {
            try
            {
                string uriBase = endpoint + "/vision/v3.0/read/analyze";
                await DisplayAlert("Start Read API", "Best Of Luck", "Thanks");
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                string url = uriBase;

                HttpResponseMessage response;

                // Two REST API methods are required to extract text.
                // One method to submit the image for processing, the other method
                // to retrieve the text found in the image.

                // operationLocation stores the URI of the second REST API method,
                // returned by the first REST API method.
                string operationLocation;

                // Reads the contents of the specified local image
                // into a byte array.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                // Adds the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // The first REST API method, Batch Read, starts
                    // the async process to analyze the written text in the image.
                    response = await client.PostAsync(url, content);
                }

                // The response header for the Batch Read method contains the URI
                // of the second method, Read Operation Result, which
                // returns the results of the process in the response body.
                // The Batch Read operation does not return anything in the response body.
                if (response.IsSuccessStatusCode)
                    operationLocation =
                        response.Headers.GetValues("Operation-Location").FirstOrDefault();
                else
                {
                    // Display the JSON error data.
                    string errorString = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("\n\nResponse:\n{0}\n",
                        JToken.Parse(errorString).ToString());
                    return;
                }

                // If the first REST API method completes successfully, the second 
                // REST API method retrieves the text written in the image.
                //
                // Note: The response may not be immediately available. Text
                // recognition is an asynchronous operation that can take a variable
                // amount of time depending on the length of the text.
                // You may need to wait or retry this operation.
                //
                // This example checks once per second for ten seconds.
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

                // Display the JSON response.
                Debug.WriteLine("\nResponse:\n\n{0}\n",
                    JToken.Parse(contentString).ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine("\n" + e.Message);
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        public byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
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