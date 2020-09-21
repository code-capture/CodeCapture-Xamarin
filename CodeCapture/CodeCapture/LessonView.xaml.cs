using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeCapture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LessonView : ContentPage
    {
        public string Link;
        public string question;
        public LessonView(string title, string link, string ques)
        {
            InitializeComponent();
            lessonTitle.Text = title;
            viewer.Source = link;
            Link = link;
            question = ques;
        }

        private async void returnBack()
        {
            await Navigation.PopModalAsync();
        }

        private void returnButton_Clicked(object sender, EventArgs e)
        {
            viewer.Source = "";
            returnBack();
        }

        private async void practiceButton_Clicked(object sender, EventArgs e)
        {
            viewer.Source = "";
            await Navigation.PushModalAsync(new PracticeView(question));
        }

        private async void youtubeButton_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync(Link, BrowserLaunchMode.SystemPreferred);
        }
    }
}