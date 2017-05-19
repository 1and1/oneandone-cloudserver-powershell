Import-Module ..\bin\Debug\OneAndOne.dll

$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

$ip=New-OAOPublicIP  -ReverseDns "pstest.rev" -Type IPV4

"listing Public IP"
Get-OAOPublicIP

$updatedIP=Set-OAOPublicIP -IpId $ip.Id -ReverseDns "psupdatedrever.com"

"test clean up"

Remove-OAOPublicIP $ip.Id
