namespace SharpWebCaptureProg
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var worker = new SeleniumWorker();
            worker.ScreenShot();
        }
    }
}