Import-Module ..\bin\Debug\OneAndOne.dll

$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

"create test user"
$user =New-OAOUser -Name "PSTestUser" -Password "test123!AA" -Description "delete me"

"Listing users"
Get-OAOUser

"show one users information"
Get-OAOUser -UserId $user.Id

"update user"
$updatedUser=Set-OAOUser -UserId $user.Id -State ACTIVE

"update API access status"
Set-OAOUserAPI -UserId $user.Id -Active $true

"show users API information"

Get-OAOUserAPI -UserId $user.Id

"show users API key"
Get-OAOUserAPIKey -UserId $user.Id

"update users API key"

Set-OAOUserAPIKey -UserId $user.Id

"add an IP to the users allowed list"

New-OAOUserAllowedIps -UserId $user.Id -Ips @((get-netadapter | get-netipaddress | ? addressfamily -eq 'IPv4').ipaddress)

"list allowed IPS"
Get-OAOUserAllowedIps -UserId $user.Id

"remove all allowedIPS"

foreach ($ip in (get-netadapter | get-netipaddress | ? addressfamily -eq 'IPv4').ipaddress) {
	Remove-OAOUserAllowedIps -UserId $user.Id -IP $ip
}

"test clean up"

Remove-OAOUser -UserId $user.Id

