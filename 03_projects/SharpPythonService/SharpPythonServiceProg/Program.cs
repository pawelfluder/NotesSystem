using PythonNetEngine;

namespace SharpPythonServiceProg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var gg = new PythonNet();
            var gg2 = gg.ExecuteCommand("gg");
        }
    }
}