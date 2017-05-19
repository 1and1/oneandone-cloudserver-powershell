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

function waitStorageReady
{
    Param ([String]$storageId)
    start-sleep -seconds 5
	Do{
		"Waiting on SharedStorage to deploy"

		$storageStatus = Get-OAOSharedStorage -StorageId $storageId

		start-sleep -seconds 10
	}While($storageStatus.State -ne "ACTIVE")

}



$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

$serverAppliance =Get-OAOServerAppliance -Query "centos"

$hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})

$server=New-OAOServer -Name "test ps shared storage1" -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverAppliance[0].Id `
-PowerOn $false 

#wait on server to deploy and get ready
waitServer -serverId $server.Id
$dcs=Get-OAODatacenter

$sharedStorage=New-OAOSharedStorage -Name "ps test storage1" -DatacenterId $dcs[0].Id -Size 50 -Description "desc"

waitStorageReady $sharedStorage.Id


"listing shared storages"
Get-OAOSharedStorage

$updatedStorage=Set-OAOSharedStorage -StorageId $sharedStorage.Id -Name "ps updated name1"

waitStorageReady $sharedStorage.Id

if($updatedStorage.Name -eq "ps updated name1")
{
    "name changed"
}else{
    "failed to update name"
}

"Access tests"

Get-OAOSharedStorageAccess

$updateAccess=Set-OAOSharedStorageAccess -Password "Test123!1"

"add a server to the shared storage"

$servers =@([OneAndOne.POCO.Requests.SharedStorages.Server]@{Id=$server.Id;Rights="RW"})
$addedServer=New-OAOSharedStorageServer -StorageId $sharedStorage.Id -Servers $servers

waitStorageReady $sharedStorage.Id

$attachedServers=Get-OAOSharedStorageServer -StorageId $sharedStorage.Id

"List of attached servers"
$attachedServers

Remove-OAOSharedStorageServer -StorageId $sharedStorage.Id -ServerId $server.Id

waitStorageReady $sharedStorage.Id

"List of attached servers should be empty"
$attachedServers=Get-OAOSharedStorageServer -StorageId $sharedStorage.Id

$attachedServers

"test clean up"

waitStorageReady $sharedStorage.Id
Remove-OAOSharedStorage -StorageId $sharedStorage.Id

waitServer -serverId $server.Id
Remove-OAOServer $server.Id
