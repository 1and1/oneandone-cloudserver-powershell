Import-Module ..\bin\Debug\OneAndOne.dll

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

function waitMonitoringPolicy
{
    Param ([String]$mpId)
    start-sleep -seconds 5
	Do{
		"Waiting on MonitoringPolicy to deploy"

		$mpStatus = Get-OAOMonitoringPolicy -PolicyId $mpId

		start-sleep -seconds 10
	}While($mpStatus.State -ne "ACTIVE")

}



$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

$serverAppliance =Get-OAOServerAppliance -Query "centos"

$ports =@([OneAndOne.POCO.Requests.MonitoringPolicies.Ports]@{AlertIf="NOT_RESPONDING";EmailNotification=$false;Port=22;Protocol="TCP"})
$processes =@([OneAndOne.POCO.Requests.MonitoringPolicies.Processes]@{AlertIf="NOT_RUNNING";EmailNotification=$false;Process="test"})

$mp=New-OAOMonitoringPolicy -Name "ps test MP2" -Email "email@test.com" -Agent $true -CpuWarningValue 90 -CpuWarningAlert $false -CpuCriticalValue 95 -CpuCriticalAlert $false `
-RamWarningValue 90 -RamWarningAlert $false -RamCriticalValue 95 -RamCriticalAlert $false -DiskWarningValue 80 -DiskWarningAlert $false -DiskCriticalValue 90 -DiskCriticalAlert $false `
-InternalPingWarningValue 50 -InternalPingWarningAlert $false -InternalPingCriticalValue 100 -InternalPingCriticalAlert $false -TransferWarningValue 1000 -TransferWarningAlert $false `
-TransferCriticalValue 2000 -TransferCriticalAlert $false -Description "desc" -Ports $ports -Processes $processes

waitMonitoringPolicy -mpId $mp.Id

$hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})

$server=New-OAOServer -Name "test ps Monitoringcenter"  -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverAppliance[0].Id `
-PowerOn $true 

#wait on server to deploy and get ready
waitServer -serverId $server.Id

"add new server"
$serverToAdd =@($server.Id)
$addedServer=New-OAOMonitoringPolicyServers -PolicyId $mp.Id -Servers $serverToAdd

waitMonitoringPolicy -mpId $mp.Id
waitServer -serverId $server.Id

"Listing monitoring centers"
Get-OAOMonitoringCenter

"Getting Monitoring Center Server"
Get-OAOMonitoringCenterServer -ServerId $server.Id -Period LAST_24H

"starting test clean up"

Remove-OAOMonitoringPolicy -PolicyId $mp.Id

waitServer -serverId $server.Id
Remove-OAOServer -ServerId $server.Id -KeepIps $false
