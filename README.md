# CodeCapture

![CodeCapture-Banner](https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/CodeCapture-Banner-JPG.jpg/1024px-CodeCapture-Banner-JPG.jpg)

## About CodeCapture

**CodeCapture** is a cross-platform app that aims to assist remote learning experiences in Computer Science for students without computers by taking code from paper to mobile. CodeCapture has the functionality to extract code (C++, Java, JavaScript, or Python) from images of handwritten text and then allow the user to edit, compile, and share the result via email/SMS. We also have various lessons on C++ (more languages coming soon) to assist with students' Computer Science education especially in times like these, where the COVID-19 pandemic and enforced lockdown have kept students away from school.

*Try the Android version by downloading the* [APK file from here](https://github.com/SimranMakhija7/HackMIT-2020/releases/tag/v1.0-beta) 

*Check out the demo video of the application* [here on YouTube](https://youtu.be/fWh1jjOQNMM)

## Inspiration

While in recent times, the world has started moving towards pro-CS education, the fact is that buying computers is a distant dream for most students and educational institutions across the globe even today. In most developing countries, the ratio of CS students versus the number of computers available is highly skewed and most students are still learning programming via pen-and-paper. At the same time, however, the number of people who own mobile phones has significantly increased. Bridging this gap between pen-and-paper coding and coding on a computer by using a technology that people already own can bring a significant difference in the adoption of Computer Science education today.

## Technologies Used

* Application: Xamarin.Forms (C#, XAML, .NET)
* Extracting Code: Azure Computer Vision (Read API)
* Compiling Code: JDoodle Compiler API
* Result SMS: Twilio Programmable SMS

## Dependencies

The following **NuGet** packages are necessary to build and run this project:

* [Xamarin.Forms](https://www.nuget.org/packages/Xamarin.Forms/4.8.0.1364?_src=template)

* [Xamarin.Essentials](https://www.nuget.org/packages/Xamarin.Essentials/1.5.3.2?_src=template)

* [Microsoft.NETCore.UniversalWindowsPlatform](https://www.nuget.org/packages/Microsoft.NETCore.UniversalWindowsPlatform/6.2.10?_src=template)

* [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/12.0.3?_src=template)

* [Xam.Plugin.Media](https://www.nuget.org/packages/Xam.Plugin.Media/5.0.1?_src=template)

* [Twilio](https://www.nuget.org/packages/Twilio/)

* [NETStandard.Library](https://www.nuget.org/packages/NETStandard.Library/2.0.3?_src=template) (SDK)


## Steps To Run

Please follow the following steps to run the project in **Visual Studio Community 2019**

1. Clone the repository to a location of your preference

2. Open the project in Visual Studio 2019 by clicking on **CodeCapture.sln**

3. Install all the dependencies mentioned above using the **NuGet Package Manager** 

4. Go to the location **../CodeCapture/CodeCapture/Models** and create a **Secrets.cs** file based on the template in the **Readme.md** file
   (For more information on how to create the resources for Azure Computer Vision Read API and JDoodle Compiler API, scroll below)

5. Build the project
 
6. Run and enjoy :)

## UI Of The Application 

| ![MainPage](https://upload.wikimedia.org/wikipedia/commons/thumb/5/54/H-MainPage.jpg/280px-H-MainPage.jpg) | ![AboutUs](https://upload.wikimedia.org/wikipedia/commons/thumb/d/d5/H-AboutUs.jpg/280px-H-AboutUs.jpg) | ![LessonsMenu](https://upload.wikimedia.org/wikipedia/commons/thumb/e/e9/H-LessonsMenu.jpg/280px-H-LessonsMenu.jpg) | ![LessonView](https://upload.wikimedia.org/wikipedia/commons/thumb/d/d2/H-LessonView.jpg/280px-H-LessonView.jpg) | ![PracticeView](https://upload.wikimedia.org/wikipedia/commons/thumb/d/d6/H-PracticeView.jpg/280px-H-PracticeView.jpg) | ![ImageCapture](https://upload.wikimedia.org/wikipedia/commons/thumb/2/21/H-ImageCapture.jpg/280px-H-ImageCapture.jpg) | ![ExtractText](https://upload.wikimedia.org/wikipedia/commons/thumb/4/4c/H-ExtractText.jpg/280px-H-ExtractText.jpg) | ![CompileCode](https://upload.wikimedia.org/wikipedia/commons/thumb/5/51/H-OutputCode.jpg/280px-H-OutputCode.jpg) |
|-|-|-|-|-|-|-|-|
| Entering the App | About CodeCapture | Lessons Menu | Viewing A Lesson | Practice The Concept | Capturing An Image | Extracting The Code | Displaying The Output |

The application consists of the various Content Pages that the user sees when they use the app. You can check out the [demo video of the app here too](https://youtu.be/fWh1jjOQNMM).


## Future Plans

We do have various plans for the future, such as:
1.  Gamifying the progress for users
2.  Support for more languages
3.  Custom lessons on more technologies
4.  A web-based learning platform designed for educators
5.  Improvements in UI

## Special Credits

Special thanks to [Aman Dhattarwal](https://www.youtube.com/channel/UCmXZxX_qexEZxhb5_vQKPCw) for the amazing [C++ Course](https://www.youtube.com/playlist?list=PLKKfKV1b9e8qBh_LG02V8R8qGRQjloeBj) he has published on YouTube, which we have utilised in the application.
