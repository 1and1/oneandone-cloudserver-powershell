Import-Module ..\bin\Debug\OneAndOne.dll

$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

$vpn=New-OAOVpn -Name "vpn ps test"

"listing vpns"
Get-OAOVpn

start-sleep -seconds 20

Get-OAOVpnConfiguration -VpnId $vpn.Id

$updatedIP=Set-OAOVpn -VpnId $vpn.Id -Name "update ps name"

"test clean up"

Remove-OAOVpn $vpn.Id
