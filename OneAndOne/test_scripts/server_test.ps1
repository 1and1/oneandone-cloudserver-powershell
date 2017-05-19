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


function waitPrivateNetwork
{
    Param ([String]$networkId)
    start-sleep -seconds 5
	Do{
		"Waiting on PrivateNetowrk to deploy"

		$networkStatus = Get-OAOPrivateNetwork -NetworkId $networkId

		start-sleep -seconds 10
	}While($networkStatus.State -ne "ACTIVE")

}

$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

$serverAppliance =Get-OAOServerAppliance -Query "centos"
$serverIds=@()

$hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})

$server=New-OAOServer -Name "test ps serverhardware" -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverAppliance[0].Id `
-PowerOn $false 

#wait on server to deploy and get ready
waitServer -serverId $server.Id

$serverIds+=$server.Id

For ($i=0; $i -le 1; $i++) {
        $hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})

        $additional=New-OAOServer -Name "test ps privatenetwork $($i)"  -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverAppliance[0].Id `
        -PowerOn $false 

        #wait on server to deploy and get ready
        waitServer -serverId $additional.Id

        $serverIds+=$additional.Id
}

$new_name="updated Name ps"

$updatedSever=Set-OAOServer -ServerId $server.Id -Name $new_name -Description "desc"


#wait on server to deploy and get ready
waitServer -serverId $server.Id

$updatedSever=Get-OAOServer -ServerId $server.Id

$listFlavors=Get-OAOHardwareFlavor

$flavoredServer=New-OAOServerWithFlavor -Name "flavor ps created" -FixedInstanceSizeId $listFlavors[0].Id -ApplianceId $serverAppliance[0].Id -PowerOn $false

 


#hardware tests
Write-Host "Begin to test hardware ops"

$serverHardware=Get-OAOServerHardware -ServerId $server.Id

$updatedHardware= Set-OAOServerHardware -ServerId $server.Id -Vcore 4 -Ram 8 

#wait on server hardware to get updated
waitServer -serverId $server.Id

#hdd tests
Write-Host "Begin to test hdd ops"

$serverHdds=Get-OAOServerHdds -ServerId $server.Id
$serverHdd=Get-OAOServerHdds -ServerId $server.Id -HddId $serverHdds[0].Id

Write-Host "Found HDD " $serverHdd.Size

waitServer -serverId $server.Id

$newhdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=40;IsMain=$false})
$newHdd =New-OAOServerHdds -ServerId $server.Id -Hdds $newhdds

#wait on server hdd to get added
waitServer -serverId $server.Id

$serverHdds=Get-OAOServerHdds -ServerId $server.Id
Set-OAOServerHdds -ServerId $server.Id -HddId $serverHdds[1].Id -Size 60

#wait on server hdd to get updated
waitServer -serverId $server.Id

$withMoreHdd=Get-OAOServer -ServerId $server.Id

$serverHdds=Get-OAOServerHdds -ServerId $server.Id

if($withMoreHdd.Hardware.Hdds.Count -ne 1 )
{
    "Successfully added  hdd"
}else{
    "Failed to add hdd"
}

Remove-OAOServerHdd -ServerId $server.Id -HddId $serverHdds[1].Id

#wait on server hdd to get updated
waitServer -serverId $server.Id

$lessHdd=Get-OAOServer -ServerId $server.Id

if($lessHdd.Hardware.Hdds.Count -eq 1)
{
    "Successfully removed hdd"
}else{
    "Failed to remove hdd"
}

$serverImage=Get-OAOServerImage -ServerId $server.Id

Set-OAOServerImages -ServerId $server.Id -ImageId $server.Image.Id -Password "Testing!123"

waitServer -serverId $flavoredServer.Id

Write-Host "Begin to test private network ops"

"creating test pn"

$pn=New-OAOPrivateNetwork -Name "ps private network test"

waitPrivateNetwork $pn.Id

"add server to a private network"

$addToPN=New-OAOServerPrivateNetwork -ServerId $flavoredServer.Id -PrivateNetworkId $pn.Id

waitServer -serverId $flavoredServer.Id

waitPrivateNetwork -pnId $pn.Id

$withPn=Get-OAOServer -ServerId $flavoredServer.Id

if($withPn.PrivateNetworks.Count -ne 0)
{
    "Private network added succesfully"
}else{
    "failed to add private network"
}

"remove server from a private network"

Remove-OAOServerPrivateNetwork -ServerId $flavoredServer.Id -PrivateNetworkId $withPn.PrivateNetworks[0].Id

waitServer -serverId $flavoredServer.Id
waitPrivateNetwork -pnId $pn.Id

$withoutPn=Get-OAOServer -ServerId $flavoredServer.Id

if($withoutPn.PrivateNetworks -eq $null -or $withoutPn.PrivateNetworks.Count -eq 0)
{
    "Private network removed succesfully"
}else{
    "failed to remove private network"
}


Write-Host "Begin to test snapshot ops"
"create snapshot"
waitServer -serverId $server.Id

New-OAOServerSnapshot -ServerId $server.Id

waitServer -serverId $server.Id

$withSnapshot= Get-OAOServer -ServerId $server.Id

"restore snapshot"
Set-OAOServerSnapshot -ServerId $withSnapshot.Id -SnapshotId $withSnapshot.Snapshot.Id

$serverSnapshot=Get-OAOServerSnapshot -ServerId $withSnapshot.Id

waitServer -serverId $withSnapshot.Id

"remove snapshot"

Remove-OAOServerSnapshot -ServerId $withSnapshot.Id -SnapshotId $serverSnapshot.Id

"create clone"
$clone=New-OAOServerClone -ServerId $flavoredServer.Id -Name "server ps clone from flavor test"

Start-Sleep -s 10

foreach ($id in $serverIds) {
    waitServer -serverId $id
    Remove-OAOServer $id
}

waitServer -serverId $flavoredServer.Id
Remove-OAOServer -ServerId $flavoredServer.Id

waitServer -serverId $clone.Id
Remove-OAOServer -ServerId $clone.Id

waitPrivateNetwork -networkId $pn.Id
Remove-OAOPrivateNetwork -NetworkId $pn.Id
