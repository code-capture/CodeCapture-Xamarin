using Plugin.Media;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GarudaHacks
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new GarudaHacks.MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
