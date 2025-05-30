﻿using System.Diagnostics;
using SharpButtonActionsProg.AAPublic;
using SharpButtonActionsProg.Workers;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpButtonActionsProg.Service;

public class SystemActionsService : ISystemActionsService
{
    static char space = ' ';
    private readonly IOperationsService operationsService;
    private MacWorker mac;
    private WindowsWorker windows;

    public SystemActionsService(IOperationsService operationsService)
    {
        this.operationsService = operationsService;
        mac = new MacWorker(operationsService);
        windows = new WindowsWorker();
    }

    public void OpenFolder(string path)
    {
        windows.TryOpenFolder(path);
        mac.TryOpenFolder(path);
    }

    public void OpenFile(string path)
    {
        windows.TryOpenFile(path);
        mac.TryOpenFile(path);
    }

    public void OpenTerminal(string path)
    {
        mac.TryOpenTerminal(path);
        windows.TryOpenTerminal(path);
    }

    public void Run(string[] args)
    {
        //var fileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
        //var fileName = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        //var arguments = "D:\02_Xampp\htdocs\Notki\01\02\lista.txt";

        var fileName = @"C:\Program Files\Notepad++\notepad++.exe";

        //var arguments = @"https://facebook.com";
        //var arguments = "https://www.google.com";

        string arguments = string.Empty;


        var argsList = args.Any() ? args.ToList() : new List<string>();


        if (argsList.Count == 0)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            Process.Start("explorer.exe", currentDirectory);
        }
        else if (argsList.Count == 1)
        {
            arguments = args[0];

            if (Directory.Exists(arguments))
            {
                Process.Start("explorer.exe", arguments);
            }
            else
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = System.Text.Encoding.UTF8,
                    FileName = fileName,
                    Arguments = arguments,
                };
                process.Start();
            }
        }
    }
}
