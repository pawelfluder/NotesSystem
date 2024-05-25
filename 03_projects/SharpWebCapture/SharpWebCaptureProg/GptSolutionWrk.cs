using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Drawing.Imaging;
using System.Drawing;

namespace SharpWebCaptureProg
{
    public class GptSolutionWrk
    {
        static IWebDriver driver;
        private readonly GoogleProfile googleProfile;
        private readonly JsWorker jsWorker;

        public GptSolutionWrk()
        {
            googleProfile = new GoogleProfile();
            jsWorker = new JsWorker();
        }

        public void ScreenShot()
        {
            // Ustawienia ChromeDriver
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            ChromeOptions chromeCapabilities = new ChromeOptions();
            chromeCapabilities.EnableMobileEmulation(ChromeEmulations.IphoneX);
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

            // Inicjalizacja przeglądarki
            IWebDriver driver = new ChromeDriver(chromeCapabilities);
            try
            {
                // Przejście do strony
                driver.Navigate().GoToUrl("https://www.instagram.com/direct/t/116544883068511");

                // Pobranie rozmiaru strony
                var totalHeight = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollHeight");
                var totalWidth = (long)((IJavaScriptExecutor)driver).ExecuteScript("return document.body.scrollWidth");

                // Ustawienie rozmiaru okna przeglądarki na pełny obszar strony
                driver.Manage().Window.Size = new Size((int)totalWidth, (int)totalHeight);

                // Zapisujemy zrzut ekranu jako bazowy obraz
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var base64 = screenshot.AsByteArray;
                var bitmap = new Bitmap(new MemoryStream(base64));

                // Ustawienie początkowej pozycji przewijania
                long initialScroll = 0;
                long currentScroll = 0;
                long viewportHeight = driver.Manage().Window.Size.Height;

                while (currentScroll < totalHeight)
                {
                    // Przewinięcie do następnej pozycji
                    ((IJavaScriptExecutor)driver).ExecuteScript($"window.scrollTo(0, {currentScroll})");
                    currentScroll += viewportHeight;

                    // Czekamy na załadowanie strony
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                    // Wykonanie zrzutu ekranu i złączenie z poprzednimi
                    screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    base64 = screenshot.AsByteArray;
                    using (var stream = new MemoryStream(base64))
                    {
                        var image = new Bitmap(stream);
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.DrawImage(image, new Point(0, (int)initialScroll));
                        }
                    }

                    initialScroll += viewportHeight;
                }

                bitmap.Save("../../../baseScreenshot.png", ImageFormat.Png);
                Console.WriteLine("Zrzut ekranu został zapisany jako baseScreenshot.png");
            }
            finally
            {
                // Zamknięcie przeglądarki
                driver.Quit();
            }
        }
    }
}