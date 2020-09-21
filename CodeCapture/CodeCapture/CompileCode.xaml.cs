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
using System.Diagnostics;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

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
            editor.IsReadOnly = false;

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
                //await DisplayAlert("POST Request Successful", "", "OK");
                contentString = await response.Content.ReadAsStringAsync();
                await DisplayTextAsync(JObject.Parse(contentString).ToString());
            }
            else await DisplayAlert("POST Request Failed", "", "OK");
        }

        async Task DisplayTextAsync(string content)
        {
            Models.CompileModels.Output output = JsonConvert.DeserializeObject<Models.CompileModels.Output>(content);
            result = output.output;
            await DisplayAlert("Compiled Code", result, "Show In Editor");
            editor.Text = result;
            editor.IsReadOnly = true;
        }

        private async void returnButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void menuButton_Clicked(object sender, EventArgs e)
        {
            var menu = new LessonMenu();
            await Navigation.PushModalAsync(menu);
        }

        private async void shareButton_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await DisplayAlert("No Network Available", "Please Connect To Your Wifi Or Turn on Mobile Data", "OK");
            }
            else
            {
                string emailContentBody = "Thank you for using CodeCapture.\n\n-----\n\nThe Code you scanned was:\n\n" + saveCode + "\n\n-----\n\nYour Standard Input was:\n\n" + saveStdin + "\n\n-----\n\nThe Result you got was:\n\n" + result;
                string msgContentBody = "Thank you for using CodeCapture. The Result you got was:\n\n" + result;

                Models.Secrets secrets = new Models.Secrets();

                if (!string.IsNullOrEmpty(emailID.Text))
                {
                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress(secrets.EMAIL_address);
                        mail.To.Add(emailID.Text.Trim());
                        mail.Subject = "CodeCapture Scan";
                        mail.Body = emailContentBody;
                        SmtpServer.Port = 587;
                        SmtpServer.Host = "smtp.gmail.com";
                        SmtpServer.EnableSsl = true;
                        SmtpServer.UseDefaultCredentials = false;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(secrets.EMAIL_address, secrets.EMAIL_password);
                        SmtpServer.Send(mail);
                        await DisplayAlert("Email Sent Succesfully", "", "OK");
                    }
                    catch (Exception err)
                    {
                        await DisplayAlert("Failed To Send Email", err.Message, "OK");
                    }
                }
                if(!string.IsNullOrEmpty(phoneNumber.Text))
                {
                    try
                    {
                        string accountSid = secrets.TWILIO_accountSID;
                        string authToken = secrets.TWILIO_authToken;

                        TwilioClient.Init(accountSid, authToken);

                        var message = MessageResource.Create(
                            body: msgContentBody,
                            from: new Twilio.Types.PhoneNumber(secrets.TWILIO_phoneNumber),
                            to: new Twilio.Types.PhoneNumber(phoneNumber.Text.Trim())
                        );

                        if (!(message.ErrorCode == null))
                        {
                            await DisplayAlert("Failed To Send SMS", "", "OK");
                        }
                        else
                        {
                            await DisplayAlert("SMS Sent Succesfully", "", "OK");
                        }
                    }
                    catch(Exception err)
                    {
                        await DisplayAlert("Failed To Send SMS", err.Message, "OK");
                    }
                }
            }
        }
    }
}