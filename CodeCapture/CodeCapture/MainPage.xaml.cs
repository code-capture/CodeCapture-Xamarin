using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        }

        private async void menuButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LessonMenu());
        }

        private async void aboutButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AboutUs());
        }
    }
}