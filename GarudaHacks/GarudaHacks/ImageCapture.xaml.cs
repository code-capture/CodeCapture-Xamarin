using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.Media;

namespace GarudaHacks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageCapture : ContentPage
    {
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

            await DisplayAlert("File Location", file.Path, "OK");

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            //Debug.WriteLine(image.Source);
        }

        private async void sendToCodeSpace_Clicked(object sender, EventArgs e)
        {
            if(langPicker.SelectedItem==null)
            {
                await DisplayAlert("Language Not Selected!!!","Go Pick A Language!!!", "OK");
                return;
            }
            await DisplayAlert("Send To CodeSpace Button Works!!!","Function To Be Built Yet\nLanguage Selected => "+langPicker.SelectedItem,"OK");
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
                Plugin.Media.Abstractions.PickMediaOptions{ });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }
    }
}