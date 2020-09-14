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
using CodeCapture.Models.CompileModels;

namespace CodeCapture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompileCode : ContentPage
    {
        public CompileCode(string code, string stdin)
        {
            InitializeComponent();
            compile(code, stdin);
        }

        public async void compile(string code, string stdin)
        {
            string contentString;
            Models.CompileModels.Input input = new Models.CompileModels.Input()
            {
                clientId = "1818dc6a88b6566b8086e3d9f9c8eea3",
                clientSecret = "b3403af090e64ac8a18ed627d65736d14055a5d10f914688408e74d5f30039f4",
                script = code,
                stdin = stdin,
                language="java",
                versionIndex = "0",
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
            String result = output.output;
            await DisplayAlert("Compiled Code", result, "Show In Editor");
            editor.Text = result;
        }
    }
}