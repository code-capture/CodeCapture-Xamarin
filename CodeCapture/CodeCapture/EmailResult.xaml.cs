using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using CodeCapture.Models;

namespace CodeCapture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailResult : ContentPage
    {
        public EmailResult(string code, string stdin, string result)
        {
            InitializeComponent();

            body.Text = "Thanks for using CodeCapture.\n\n-----\n\nThe Code you scanned was:\n\n" + code + "\n\n-----\n\nYour Standard Input was:\n\n" + stdin + "\n\n-----\n\nThe Result you got was:\n\n" + result;
        }
        private async void captureNewImageButton_Clicked(object sender, EventArgs e)
        {
            var captureimage = new ImageCapture();
            await Navigation.PushModalAsync(captureimage);
        }

        private async void emailButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                Models.Secrets secrets = new Models.Secrets();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(secrets.EMAIL_address);
                mail.To.Add(emailID.Text);
                mail.Subject = "CodeCapture Scan";
                mail.Body = body.Text;
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

        private async void returnButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}