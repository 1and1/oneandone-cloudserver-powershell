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

For ($i=0; $i -le 2; $i++) {
        $hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})

        $server=New-OAOServer -Name "test ps privatenetwork $($i)"  -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverAppliance[0].Id `
        -PowerOn $false 

        #wait on server to deploy and get ready
        waitServer -serverId $server.Id

        $serverIds+=$server.Id
}

$pn=New-OAOPrivateNetwork -Name "ps private network test"

waitPrivateNetwork $pn.Id


"listing Private networks"
Get-OAOPrivateNetwork

$updatedPN=Set-OAOPrivateNetwork -NetworkId $pn.Id -Name "ps updated name"

waitPrivateNetwork $pn.Id

if($updatedPN.Name -eq "ps updated name")
{
    "name changed"
}else{
    "failed to update name"
}

"add a server to the Private network"

$servers =@($serverIds[0])
$addedServer=New-OAOPrivateNetworkServer -NetworkId $pn.Id -Servers $servers 

waitPrivateNetwork $pn.Id

$attachedServers=Get-OAOPrivateNetworkServer -NetworkId $pn.Id

"List of servers"
$attachedServers

Remove-OAOPrivateNetworkServer -NetworkId $pn.Id -ServerId $serverIds[0]

waitPrivateNetwork $pn.Id

"List of attached servers should be empty"

Get-OAOPrivateNetworkServer -NetworkId $pn.Id

"test clean up"
foreach ($id in $serverIds) {
    waitServer -serverId $id
    Remove-OAOServer $id
}



waitPrivateNetwork $pn.Id
Remove-OAOPrivateNetwork -NetworkId $pn.Id


