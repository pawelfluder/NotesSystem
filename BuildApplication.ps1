$shell = New-Object -comObject WScript.Shell
$shortcut = $shell.CreateShortcut(".\WpfNotesSystemProg3.lnk")
$projectExeFile = get-item ".\03_projects\WpfNotesSystem3\WpfNotesSystemProg3\bin\Debug\net6.0-windows\WpfNotesSystemProg3.exe"
$shortcut.TargetPath = $projectExeFile.FullName
$shortcut.WorkingDirectory = $projectExeFile.Directory.FullName
#$shortcut.Arguments = """\\machine\share\folder"""
$shortcut.Save()

Write-Host "gg"