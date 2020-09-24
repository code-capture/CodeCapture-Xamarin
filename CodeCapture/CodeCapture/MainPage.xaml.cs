using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CodeCapture
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                label.Text="No Network Available\nPlease Connect To Your Wifi Or Turn on Mobile Data";
                logo.IsEnabled = false;
            }
            else
            {
                label.Text = "Tap On The Logo to Continue";
                logo.IsEnabled = true;
            }

            logo.Clicked += (s, e) => Navigation.PushModalAsync(new ImageCapture());
        }
    }
}
