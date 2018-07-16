#Import-Module ..\bin\Debug\OneAndOne.dll

function waitServer
{
    Param ([String]$serverId)
    start-sleep -seconds 5
	Do{
		"Waiting on server to deploy"

		$serverstatus = Get-OAOServerStatus -ServerId $serverId

		start-sleep -seconds 10
	}While($serverstatus -ne $null -and (($serverstatus.State -ne "POWERED_ON" -and  $serverstatus.State -ne "POWERED_OFF") -Or `
                                     ($serverstatus.Percent -ne 0 -and $serverstatus.Percent -ne 99)))

}

$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

$serverAppliance =Get-OAOServerAppliance -Query "baremetal"
$serverIds=@()

$baremetalModels = Get-OAOBaremetalModels

$server=New-OAOBaremetalServer -Name "ps baremetal serverhardware" -BaremetalModelId $baremetalModels[0].Id -ApplianceId $serverAppliance[0].Id `
-PowerOn $false 

#wait on server to deploy and get ready
waitServer -serverId $server.Id

$new_name="updated Name ps"

$updatedSever=Set-OAOServer -ServerId $server.Id -Name $new_name -Description "desc"


#wait on server to deploy and get ready
waitServer -serverId $server.Id

$updatedSever=Get-OAOServer -ServerId $server.Id

waitServer -serverId $server.Id
Remove-OAOServer -ServerId $server.Id
