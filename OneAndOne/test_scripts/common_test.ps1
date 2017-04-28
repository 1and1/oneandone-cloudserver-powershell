Import-Module ..\bin\Debug\OneAndOne.dll

$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

"listing usages"
Get-OAOUsage -Period LAST_24H

"List servera appliances"
$appliances=Get-OAOServerAppliance
$appliances

"Single server appliance"
Get-OAOServerAppliance -ApplianceId $appliances[0].Id

"List dvd isos"
$dvds =Get-OAODvd
$dvds

"single dvd iso"
Get-OAODvd -DvdId $dvds[0].Id

"list Data centers"

$dcs=Get-OAODatacenter
$dcs

"single data center"

Get-OAODatacenter -DcId $dcs[0].Id

"Listing pricing"

Get-OAOPricing

"ping the API"

Get-OAOPingApi

"ping the token"

Get-OAOPingAuthentication



