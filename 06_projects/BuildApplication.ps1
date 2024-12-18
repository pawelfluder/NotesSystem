$shortcutFilePath = "./WpfNotesSystemProg3.lnk"

if (Test-Path -Path $shortcutFilePath)
{
    remove-item $shortcutFile.FullName
}
else
{
    $solutionFile = get-item ".\03_projects\WpfNotesSystem3\WpfNotesSystemProg3\WpfNotesSystemProg.sln"
    dotnet build $solutionFile.FullName
}
$shell = New-Object -comObject WScript.Shell
$shortcut = $shell.CreateShortcut($shell.CurrentDirectory + "/" + "WpfNotesSystemProg3.lnk")
$projectExeFile = get-item ".\03_projects\WpfNotesSystem3\WpfNotesSystemProg3\bin\Debug\net6.0-windows\WpfNotesSystemProg3.exe"
$shortcut.TargetPath = $projectExeFile.FullName
$shortcut.WorkingDirectory = $projectExeFile.Directory.FullName
#$shortcut.Arguments = """\\machine\share\folder"""
$shortcut.Save()

$shortcutFile = get-item $shortcutFilePath
Start-Process $shortcutFile.FullName

Write-Host "Finish"
Read-Host -Prompt "Press Enter to exit"
