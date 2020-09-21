using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeCapture
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new CodeCapture.MainPage());
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
