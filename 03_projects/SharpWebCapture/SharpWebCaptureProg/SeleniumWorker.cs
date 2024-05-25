using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WDSE.Decorators;
using WDSE.ScreenshotMaker;
using OpenQA.Selenium.Support.Extensions;
using WDSE;


//https://www.linkedin.com/pulse/capturing-full-webpage-screenshot-using-selenium-c-test-choudhary-1c/
//https://www.nuget.org/packages/Noksa.WebDriver.ScreenshotsExtensions/
// https://www.youtube.com/watch?v=MUVCE550yw8

namespace SharpWebCaptureProg
{
    public class SeleniumWorker
    {
        static IWebDriver driver;
        private readonly GoogleProfile googleProfile;
        private readonly JsWorker jsWorker;

        public SeleniumWorker()
        {
            googleProfile = new GoogleProfile();
            jsWorker = new JsWorker();
        }

        public void ScreenShot()
        {
            ChromeOptions chromeCapabilities = new ChromeOptions();
            //chromeCapabilities.EnableMobileEmulation(ChromeEmulations.IphoneX);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var arg1 = $"--user-data-dir={googleProfile.TryGetUserDataDir()}";
                var arg2 = $"--profile-directory={googleProfile.TryGetProfileDir()}";
                chromeCapabilities.AddArgument(arg1);
                chromeCapabilities.AddArgument(arg2);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var arg = $"--user-data-dir={googleProfile.TryGetUserDataDir()}";
                chromeCapabilities.AddArgument(arg);
            }

            var weburl = "https://www.instagram.com/direct/t/116544883068511/";
            
            try
            {
                driver = new ChromeDriver(chromeCapabilities);


                driver.Url = "https://www.selenium.dev/";

                // ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile("oldFirefox.png");

                VerticalCombineDecorator vcd = new VerticalCombineDecorator(new ScreenshotMaker().RemoveScrollBarsWhileShooting());
                var gg = driver.TakeScreenshot(vcd);
                //.ToMagickImage().ToBitmap().Save("newChrome.png");

                driver.Navigate().GoToUrl(weburl);

                System.Threading.Thread.Sleep(100);

                var screenShotDriver = driver as ITakesScreenshot;
                var imageFolderPath = "./../../../shot/";
                Directory.CreateDirectory(imageFolderPath);

                var vcs = new VerticalCombineDecorator(new ScreenshotMaker());
                var webDrive = driver as IWebDriver;
                var screen = webDrive.TakeScreenshot(vcs);



                var js = driver as IJavaScriptExecutor;



                jsWorker.DefineSetBox01(js);
                js.ExecuteScript("SetBox01()");

                jsWorker.DefineGetDeepestNestedDiv(js);
                js.ExecuteScript("var box02 = getDeepestNestedDiv(box01)");

                


                //var tmp3 = js.ExecuteScript(script01);

                var tmp4 = js.ExecuteScript("return box01");

                //var gg = "return document.querySelector('[aria-label^=\"Wiadomości\"]');";
                //var gg2 = js.ExecuteScript(gg);

                var isNotTop = true;
                var previousHeight = 0;
                var currentHeight = 0;
                var maxHeight = 0;
                while (isNotTop)
                {
                    previousHeight = currentHeight;
                    js.ExecuteScript("return testingBar.scrollTop = 0;");
                    System.Threading.Thread.Sleep(1000);
                    js.ExecuteScript("return testingBar.scrollTop = 0;");
                    System.Threading.Thread.Sleep(1000);
                    js.ExecuteScript("return testingBar.scrollTop = 0;");
                    System.Threading.Thread.Sleep(1000);
                    currentHeight = int.Parse(js.ExecuteScript("return testingBar.scrollHeight").ToString());

                    if (currentHeight == previousHeight)
                    {
                        isNotTop = false;
                        maxHeight = currentHeight;
                    }
                }

                js.ExecuteScript($"return testingBar.scrollTop = {maxHeight};");
                currentHeight = maxHeight;
                // 1600 - katwocice sie czuje kawiarnie
                // 1300 - minimalnei za duzo

                currentHeight = 0;

                var diff = 1000; // 1360

                while (currentHeight < maxHeight)
                {
                    
                    js.ExecuteScript($"return testingBar.scrollTop = {currentHeight};");
                    //js.ExecuteScript($"return testingBar.scrollTop = {currentHeight};");
                    System.Threading.Thread.Sleep(1000);
                    var screenShot = screenShotDriver.GetScreenshot();
                    System.Threading.Thread.Sleep(1000);
                    var imageFileName = imageFolderPath + "item_" + currentHeight + ".png";
                    screenShot.SaveAsFile(imageFileName);

                    currentHeight += diff;
                }

                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            driver.Quit();
        }
    }
}