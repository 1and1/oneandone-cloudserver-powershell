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

$hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})

$server=New-OAOServer -Name "test ps MonitoringPolicy 13"  -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverAppliance[0].Id `
-PowerOn $false 

#wait on server to deploy and get ready
waitServer -serverId $server.Id

$ports =@([OneAndOne.POCO.Requests.MonitoringPolicies.Ports]@{AlertIf="NOT_RESPONDING";EmailNotification=$false;Port=22;Protocol="TCP"})
$processes =@([OneAndOne.POCO.Requests.MonitoringPolicies.Processes]@{AlertIf="NOT_RUNNING";EmailNotification=$false;Process="test"})

$mp=New-OAOMonitoringPolicy -Name "ps test MP" -Email "email@test.com" -Agent $true -CpuWarningValue 90 -CpuWarningAlert $false -CpuCriticalValue 95 -CpuCriticalAlert $false `
-RamWarningValue 90 -RamWarningAlert $false -RamCriticalValue 95 -RamCriticalAlert $false -DiskWarningValue 80 -DiskWarningAlert $false -DiskCriticalValue 90 -DiskCriticalAlert $false `
-InternalPingWarningValue 50 -InternalPingWarningAlert $false -InternalPingCriticalValue 100 -InternalPingCriticalAlert $false -TransferWarningValue 1000 -TransferWarningAlert $false `
-TransferCriticalValue 2000 -TransferCriticalAlert $false -Description "desc" -Ports $ports -Processes $processes


waitMonitoringPolicy -mpId $mp.Id

"listing monitoring policies"
Get-OAOMonitoringPolicy

$updatedMP=Set-OAOMonitoringPolicy -PolicyId $mp.Id -Name "update ps name"

"Processes tests"
"add new process"
$processesToAdd =[OneAndOne.POCO.Requests.MonitoringPolicies.Processes]@{AlertIf="NOT_RUNNING";EmailNotification=$false;Process="init"}
$addedProcesses=New-OAOMonitoringPolicyProcess -PolicyId $mp.Id -Processes $processes 

waitMonitoringPolicy -mpId $mp.Id

"update existing process"

$processesToUpdate =[OneAndOne.POCO.Requests.MonitoringPolicies.Processes]@{AlertIf="NOT_RUNNING";EmailNotification=$true;Process="init"}
$updatedProcesses=Set-OAOMonitoringPolicyProcess -PolicyId $mp.Id -ProcessId $addedProcesses.Processes[0].Id -Processes $processesToUpdate

"getting updated process" 
waitMonitoringPolicy -mpId $mp.Id
Get-OAOMonitoringPolicyProcess -PolicyId $mp.Id -ProcessId $mp.Processes[0].Id

"removing a process"
$prcsNotFound=$false
Remove-OAOMonitoringPolicyProcess -PolicyId $mp.Id -ProcessId $mp.Processes[0].Id
waitMonitoringPolicy -mpId $mp.Id
$listing=@(Get-OAOMonitoringPolicyProcess -PolicyId $mp.Id)
foreach ($prcs in $listing) {
	if($prcs.Id -eq $mp.Processes[0].Id)
    {
        $prcsNotFound=$false
        break
    }else{
        $prcsNotFound=$true
    }
}
if($prcsNotFound)
{
    "remove process passed"
}else{
    "remove process failed"
}

waitMonitoringPolicy -mpId $mp.Id

"listing processes"
Get-OAOMonitoringPolicyProcess -PolicyId $mp.Id


"Ports tests"
"add new port"
$portsToAdd =[OneAndOne.POCO.Requests.MonitoringPolicies.Ports]@{AlertIf="NOT_RESPONDING";EmailNotification=$false;Port=25;Protocol="TCP"}
$addedPorts=New-OAOMonitoringPolicyPort -PolicyId $mp.Id -Ports $portsToAdd

waitMonitoringPolicy -mpId $mp.Id

"update existing port"

$portsToUpdate =[OneAndOne.POCO.Requests.MonitoringPolicies.Ports]@{AlertIf="NOT_RESPONDING";EmailNotification=$true;Port=25;Protocol="TCP"}
$updatedPorts=Set-OAOMonitoringPolicyPort -PolicyId $mp.Id -PortId $mp.Ports[0].Id -Ports $portsToUpdate

"getting updated port" 
waitMonitoringPolicy -mpId $mp.Id
Get-OAOMonitoringPolicyPort -PolicyId $mp.Id -PortId $mp.Ports[0].Id

"removing a prot"
$prtNotFound=$false
Remove-OAOMonitoringPolicyPort -PolicyId $mp.Id -PortsId $mp.Ports[0].Id
waitMonitoringPolicy -mpId $mp.Id

$listing=@(Get-OAOMonitoringPolicyPort -PolicyId $mp.Id)
foreach ($prts in $listing) {
	if($prts.Id -eq $mp.Ports[0].Id)
    {
        $prtNotFound=$false
        break
    }else{
        $prtNotFound=$true
    }
}
if($prtNotFound)
{
    "remove port passed"
}else{
    "remove port failed"
}

waitMonitoringPolicy -mpId $mp.Id


"listing ports"
Get-OAOMonitoringPolicyPort -PolicyId $mp.Id



"Servers tests"
"add new server"
$serverToAdd =@($server.Id)
$addedServer=New-OAOMonitoringPolicyServers -PolicyId $mp.Id -Servers $serverToAdd

waitMonitoringPolicy -mpId $mp.Id

"getting added server " 
waitMonitoringPolicy -mpId $mp.Id
Get-OAOMonitoringPolicyServers -PolicyId $mp.Id -ServerId $server.Id


"removing a server"
$serverNotFound=$false
Remove-OAOMonitoringPolicyServers -PolicyId $mp.Id -ServerId $server.Id
waitMonitoringPolicy -mpId $mp.Id

$listing=@(Get-OAOMonitoringPolicyServers -PolicyId $mp.Id)
foreach ($prcs in $listing) {
	if($prcs.Id -eq $server.Id)
    {
        $serverNotFound=$false
        break
    }else{
        $serverNotFound=$true
    }
}
if($serverNotFound)
{
    "remove server passed"
}else{
    "remove server failed"
}


waitMonitoringPolicy -mpId $mp.Id

"listing servers shoud show notihng"
Get-OAOMonitoringPolicyServers -PolicyId $mp.Id



"starting test clean up"

Remove-OAOMonitoringPolicy -PolicyId $mp.Id

waitServer -serverId $server.Id
Remove-OAOServer -ServerId $server.Id -KeepIps $false
