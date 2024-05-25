using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace SharpWebCaptureProg
{
    public class SeleniumWorker
    {
        static IWebDriver driver;

        public void ScreenShot()
        {
            //driver = new ChromeDriver();
            //var chromePath = @"C:\03_synch\02_programs_portable\07_pawelfluder\GoogleChromePortable.exe";
            //chromeCapabilities.BinaryLocation = chromePath;
            //var path = @"C:\03_synch\02_programs_portable\02_chrome\01_pawelfluder\Data\profile";
            // chrome
            // firefox
            //var firefoxCapabilities = new FirefoxOptions();
            //driver = new FirefoxDriver();

            ChromeOptions chromeCapabilities = new ChromeOptions();
            chromeCapabilities.EnableMobileEmulation(ChromeEmulations.IphoneX);
            var path = @"C:\03_synch\02_programs_portable\07_pawelfluder\Data\profile";
            chromeCapabilities.AddArgument($"--user-data-dir={path}");

            var weburl = "https://www.instagram.com/direct/t/116544883068511/";
            
            try
            {
                driver = new ChromeDriver(chromeCapabilities);
                driver.Navigate().GoToUrl(weburl);

                System.Threading.Thread.Sleep(100);

                var screenShotDriver = driver as ITakesScreenshot;
                var imageFolderPath = "./../../../shot/";
                Directory.CreateDirectory(imageFolderPath);

                var js = driver as IJavaScriptExecutor;

                var gg = "return document.querySelector('[aria-label^=\"Wiadomości\"]');";
                var gg2= js.ExecuteScript(gg);



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