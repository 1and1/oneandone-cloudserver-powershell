Import-Module ..\bin\Debug\OneAndOne.dll

$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

"Listing logs in the last 24 hours"
$logs=Get-OAOLogs -Period LAST_24H
$logs

"Get  more information about a single log"
Get-OAOLogs -LogId $logs[0].Id

