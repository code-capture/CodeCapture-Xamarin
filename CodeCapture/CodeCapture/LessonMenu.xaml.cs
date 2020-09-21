using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace CodeCapture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LessonMenu : ContentPage
    {
        public LessonMenu()
        {
            InitializeComponent();

            string menuHeading = "We have our curated selection of\nC++ lessons that will help get beginners started. We will bring learning support for more languages soon.";
            menuLabel.Text = menuHeading;

            var lessonList = new List<string>
            {
                "1: Setup (For Computer) And Tokens in C++",
                "2: User Input and Common Output",
                "3: Conditional Statements",
                "4: For Loops",
                "5: While And Do-While Loops",
                "6: Arrays",
                "7: Functions",
                "Want To Learn Further?"
            };

            lessonPicker.ItemsSource = lessonList;
        }

        private async void playgroundButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ImageCapture());
        }

        private async void viewLessonButton_Clicked(object sender, EventArgs e)
        {
            if (lessonPicker.SelectedItem == null) await DisplayAlert("Select A Lesson", "", "OK");
            else
            {
                string lessonTitle = lessonPicker.SelectedItem.ToString(), lessonLink, question;

                switch (lessonPicker.SelectedIndex)
                {
                    case 0:
                        lessonLink = "https://www.youtube.com/embed/o6dT3kAWg6s";
                        question = "Just to try your hands on the platform, write your first \"Hello World\" program! Write a program to display the phrase \"Hello World!\" on the console/\n\n\n Once you're comfortable with the concept of tokens, write a program to display your name, weight, height(in meters), and BMI. Use weight and height to compute BMI and Store all four values in appropriate datatypes.";
                        break;
                    case 1:
                        lessonLink = "https://www.youtube.com/embed/uzvE7dpMjLQ";
                        question = "Write a program to take the name and age (in years) of a user as input from the console. Compute and display the user's age in months and days.";
                        break;
                    case 2:
                        lessonLink = "https://www.youtube.com/embed/cD4Vauq4lpw";
                        question = "Write a program to check whether a given number is a multiple of 5 or not.";
                        break;
                    case 3:
                        lessonLink = "https://www.youtube.com/embed/mKGCkZt8I_0";
                        question = "Use a for loop to display all odd numbers between 1-100 (excluding 1 and 100)";
                        break;
                    case 4:
                        lessonLink = "https://www.youtube.com/embed/xBgJmy-trAc";
                        question = "Write a program to compute and display the factorial of a number (taken as input) using while or do while loop(s)";
                        break;
                    case 5:
                        lessonLink = "https://www.youtube.com/embed/QAKmX809ePA";
                        question = "Write a program to store the marks of all students in a class in an array and compute the average score. Take as input the number of students in the class and the scores of the students.";
                        break;
                    case 6:
                        lessonLink = "https://www.youtube.com/embed/YGA1Sbd2c08";
                        question = "Create a function to check whether a user-inputted string is a palindrome or not";
                        break;
                    default:
                        await Browser.OpenAsync("https://www.youtube.com/playlist?list=PLKKfKV1b9e8qBh_LG02V8R8qGRQjloeBj", BrowserLaunchMode.SystemPreferred);
                        lessonLink = "https://www.youtube.com/playlist?list=PLKKfKV1b9e8qBh_LG02V8R8qGRQjloeBj";
                        question = "No questions for this selection";
                        break;
                }

                if (lessonPicker.SelectedIndex != 7)
                {
                    lessonTitle = "Concept " + lessonTitle;
                    question = "Question:\n\n" + question;
                }

                await Navigation.PushModalAsync(new LessonView(lessonTitle, lessonLink, question));
            }
        }
    }
}