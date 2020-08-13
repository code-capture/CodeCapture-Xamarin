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
                Debug.WriteLine("No Camera Available");
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
            Debug.WriteLine(image.Source);
        }

        private void sendToCodeSpace_Clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Send To CodeSpace Button Works!!!");
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
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
            });

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