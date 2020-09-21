using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.Media;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using CodeCapture.Models.ReadModels;

namespace CodeCapture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageCapture : ContentPage
    {
        private string imagePath;

        public ImageCapture()
        {
            InitializeComponent();

            extractText.IsEnabled = false;
            extractText.IsVisible = false;
        }

        //Captures images from user's camera
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
                AllowCropping = false
            });

            if (file == null)
                return;

            imagePath = file.Path;

            extractText.IsEnabled = true;
            extractText.IsVisible = true;

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        //Selects images from the user's device
        private async void selectImage_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", "Sorry! Permission not granted to photos.", "OK");
                return;
            }

            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new
                Plugin.Media.Abstractions.PickMediaOptions
            { 
            });

            if (file == null)
                return;

            imagePath = file.Path;

            extractText.IsEnabled = true;
            extractText.IsVisible = true;

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        //Sends the image to the Read API for the text to be extracted
        private async void extractText_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await DisplayAlert("No Network Available", "Please Connect To Your Wifi Or Turn on Mobile Data", "OK");
            }
            else await Navigation.PushModalAsync(new ExtractText(imagePath));
        }

        private async void menuButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LessonMenu());
        }
    }
}