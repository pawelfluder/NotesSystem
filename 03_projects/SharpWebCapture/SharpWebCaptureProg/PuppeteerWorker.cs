//using PuppeteerSharp;
//using PuppeteerSharp;

namespace SharpWebCaptureProg
{
    internal class PuppeteerWorker
    {
        public void Do()
        {
            // Download the Chromium browser if needed
            //await new BrowserFetcher().DownloadAsync();

            var chromeExe = "C:\\03_synch\\02_programs_portable\\06_notki-info\\GoogleChromePortable.exe";
            var chromeExe2 = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";
            // Launch the browser
            //using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            //{
            //    HeadlessMode = HeadlessMode.False,
            //    ExecutablePath = chromeExe2,
            //});

            //// Create a new page
            //using var page = await browser.NewPageAsync();

            //// Navigate to the URL
            //await page.GoToAsync("https://www.instagram.com/direct/t/114783846582482");

            //// Set the viewport size if needed (optional)
            //await page.SetViewportAsync(new ViewPortOptions
            //{
            //    Width = 1920,
            //    Height = 1080
            //});

            //// Take the screenshot
            //await page.ScreenshotAsync("screenshot.png");

            //// Close the browser
            //await browser.CloseAsync();

            Console.WriteLine("Screenshot saved!");
        }
    }
}
