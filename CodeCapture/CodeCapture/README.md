# User Interface of the Application

The application consists of the various Content Pages that the user sees when they use the app. The details and purpose of each page that 

You can check out the [demo video here](https://drive.google.com/file/d/1Dso3XfNttYl8mPMorpG0CSG9iAkEF5rS/view?usp=sharing) as well.

## 1. Entering the App
 
![MainPage](https://upload.wikimedia.org/wikipedia/commons/thumb/1/17/MainPage.jpg/216px-MainPage.jpg)

* Page: MainPage.xaml
* Code: MainPage.xaml.cs

The **MainPage.xaml** page is the first page that a user will see when they start the app. Tapping on the logo will allow them to proceed further.

## 2. Capturing An Image

![ImageCapture](https://upload.wikimedia.org/wikipedia/commons/thumb/e/ef/ImageCapture.jpg/216px-ImageCapture.jpg)

* Page: ImageCapture.xaml
* Code: ImageCapture.xaml.cs

The **ImageCapture.xaml** page is where the user will capture (click or select) the image of handwritten code that they want to check using the app.

## 3. Extracting The Code

![ExtractText](https://upload.wikimedia.org/wikipedia/commons/thumb/e/e0/ExtractText.jpg/216px-ExtractText.jpg)

* Page: ExtractText.xaml
* Code: ExtractText.xaml.cs

The **ExtractText.xaml** page is where the code extracted from the image using the **Azure Computer Vision Read API** shall be displayed. The user can edit the code here in the in-built editor to correct any errors in the extracted code. The user will also have to select the programming language of the code here and can add any "Standard Input" they might want to.

## 4. Compiling The Code And Displaying Output

![CompileCode](https://upload.wikimedia.org/wikipedia/commons/thumb/e/e4/CompileCode.jpg/216px-CompileCode.jpg)

* Page: CompileCode.xaml
* Code: CompileCode.xaml.cs

The **CompileCode.xaml** page is where the result of the code is returned by the **JDoodle Compiler API**. In case there are any errors in the code. Those shall be displayed here as well.

## Emailing The Result

![EmailResult](https://upload.wikimedia.org/wikipedia/commons/thumb/9/93/EmailResult.jpg/216px-EmailResult.jpg)

* Page: EmailResult.xaml
* Code: EmailResult.xaml.cs

The **EmailResult.xaml** page allows the user to email the extracted code, Standard Input, and result to any email address of their preference.