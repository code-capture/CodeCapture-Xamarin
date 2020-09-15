using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CodeCapture.Models;
using CodeCapture.Models.CompileModels;
using Xamarin.Essentials;

namespace CodeCapture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompileCode : ContentPage
    {
        public string saveCode = "";
        public string saveStdin = "";
        public string result = "";
        public CompileCode(string code, string Stdin, string lang, string vers)
        {
            InitializeComponent();
            saveCode = code;
            saveStdin = Stdin;
            compile(code, Stdin, lang, vers);
        }

        //Compile the code using JDoodle API
        public async void compile(string code, string Stdin, string lang, string vers)
        {
            string contentString;
            Models.Secrets secrets = new Models.Secrets();
            Models.CompileModels.Input input = new Models.CompileModels.Input()
            {
                clientId = secrets.JDOODLE_clientId,
                clientSecret = secrets.JDOODLE_clientSecret,
                script = code,
                stdin = Stdin,
                language = lang,
                versionIndex = vers,
            };
            HttpClient client = new HttpClient();
            string Url = "https://api.jdoodle.com/execute";
            
            var response = await client.PostAsync(Url,new StringContent(JsonConvert.SerializeObject(input),Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("POST Request Successful", "", "OK");
                contentString = await response.Content.ReadAsStringAsync();
                await DisplayTextAsync(JObject.Parse(contentString).ToString());
            }
        }

        async Task DisplayTextAsync(string content)
        {
            Models.CompileModels.Output output = JsonConvert.DeserializeObject<Models.CompileModels.Output>(content);
            result = output.output;
            await DisplayAlert("Compiled Code", result, "Show In Editor");
            editor.Text = result;
        }

        private async void returnButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void captureNewImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
            //Does not work on Windows
        }

        private async void emailButton_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await DisplayAlert("No Network Available", "Please Connect To Your Wifi Or Turn on Mobile Data", "OK");
            }
            else await Navigation.PushModalAsync(new EmailResult(saveCode, saveStdin, result));
        }
    }
}