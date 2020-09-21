using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeCapture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PracticeView : ContentPage
    {
        public PracticeView(string ques)
        {
            InitializeComponent();
            question.Text = ques;
            question.IsReadOnly = true;
        }

        private async void returnButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void playgroundButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ImageCapture());
        }
    }
}