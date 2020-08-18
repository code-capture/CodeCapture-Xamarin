using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeCapture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtractText : ContentPage
    {
        string subscriptionKey = "ffc60f43d45049b185f8ab5c9b79c2d3";
        string extract = "";
        string endpoint = "https://centralindia.api.cognitive.microsoft.com/";
        public ExtractText(string imagePath)
        {
            InitializeComponent();
            ReadText(imagePath);
        }


        //Uses the Read API on the image to extract text from images images 
        public async void ReadText(string imageFilePath)
        {
            try
            {
                string uriBase = endpoint + "/vision/v3.0/read/analyze";
                await DisplayAlert("Start Read API", "Best Of Luck", "Thanks");
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string url = uriBase;

                HttpResponseMessage response;

                string operationLocation;

                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    response = await client.PostAsync(url, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("POST Request Successful", "", "OK");
                    operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();
                }
                else
                {
                    string errorString = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine("\n\nResponse:\n{0}\n", JToken.Parse(errorString).ToString(), "OK");
                    await DisplayAlert("\n\nResponse:\n{0}\n", JToken.Parse(errorString).ToString(), "OK");
                    return;
                }

                string contentString;
                int i = 0;
                do
                {
                    System.Threading.Thread.Sleep(1000);
                    response = await client.GetAsync(operationLocation);
                    contentString = await response.Content.ReadAsStringAsync();
                    ++i;
                }
                while (i < 60 && contentString.IndexOf("\"status\":\"succeeded\"") == -1);

                if (i == 60 && contentString.IndexOf("\"status\":\"succeeded\"") == -1)
                {
                    //Debug.WriteLine("\nTimeout error.\n","","OK");
                    await DisplayAlert("\nTimeout error.\n", "", "OK");
                    return;
                }

                //await DisplayAlert("JSON Response", JToken.Parse(contentString).ToString(), "OK");
                await DisplayTextAsync(JObject.Parse(contentString).ToString());
            }
            catch (Exception e)
            {
                //Debug.WriteLine("Error","\n" + e.Message,"OK");
                await DisplayAlert("Error", "\n" + e.Message, "OK");
            }
        }


        async Task DisplayTextAsync(string content)
        {
            ReadModels.Root response = JsonConvert.DeserializeObject<ReadModels.Root>(content);
            foreach (var result in response.analyzeResult.readResults)
            {
                foreach (var line in result.lines)
                {
                    extract += line.text + "\n";
                }
            }
            await DisplayAlert("Extracted Text", extract, "Show In Editor");
            editor.Text = extract;
        }

        //Converts Image to Byte Array and return it
        public byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        private async void returnButton_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void emailButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("codecapture.sea@gmail.com");
                mail.To.Add(email.Text);
                mail.Subject = "CodeCapture Scan";
                mail.Body = "Thanks for using CodeCapture.\nHere's your scan:\n\n"+extract;

                SmtpServer.Port = 587;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("codecapture.sea@gmail.com", "CodeCapture245");

                SmtpServer.Send(mail);
                await DisplayAlert("Email Sent Succesfully!!!", "Please check your email" , "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failed To Send Email", ex.Message, "OK");
            }
        }
    }
}
