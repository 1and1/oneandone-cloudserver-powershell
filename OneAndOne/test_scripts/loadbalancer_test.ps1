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

function waitLoadBalancer
{
    Param ([String]$balancerId)
    start-sleep -seconds 5
	Do{
		"Waiting on Load balancer to deploy"

		$balancerStatus = Get-OAOLoadBalancer -BalancerId $balancerId

		start-sleep -seconds 10
	}While($balancerStatus.State -ne "ACTIVE")

}



$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

$serverAppliance =Get-OAOServerAppliance -Query "centos"

$hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})

$server=New-OAOServer -Name "test ps load balancer" -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverAppliance[0].Id  `
-PowerOn $false 

#wait on server to deploy and get ready
waitServer -serverId $server.Id

$rules =@([OneAndOne.POCO.Requests.LoadBalancer.LoadBalancerRuleRequest]@{PortBalancer=80;PortServer=80;Protocol="TCP";Source="0.0.0.0"})
$balancer=New-OAOLoadBalancer  -Name "ps test LoadBalancer" -Description "desc" -Rules $rules -HealthCheckTest NONE -HealthCheckInterval 1 -Method ROUND_ROBIN -Persistence $true -PersistenceTime 30

waitLoadBalancer $balancer.Id


"listing load balancers"
Get-OAOLoadBalancer

$updateBalancer=Set-OAOLoadBalancer -BalancerId $balancer.Id -Name "ps updated name"

waitLoadBalancer $balancer.Id

if($updateBalancer.Name -eq "ps updated name")
{
    "name changed"
}else{
    "failed to update name"
}

"add a server to the loadbalancer"
$server=Get-OAOServer -ServerId $server.Id

"ip id"
$server.Ips[0].Id

$ips =@($server.Ips[0].Id)
$addedServer=New-OAOLoadBalancerServerIps -BalancerId $balancer.Id -Ips $ips 

waitLoadBalancer $balancer.Id

$attachedIps=Get-OAOLoadBalancerServerIps -BalancerId $balancer.Id

"List of attached ips"
$attachedIps

Remove-OAOLoadBalancerServerIps -BalancerId $balancer.Id -IpId $server.Ips[0].Id

waitLoadBalancer $balancer.Id

"List of attached ips should be empty"

$attachedServers=Get-OAOLoadBalancerServerIps -BalancerId $balancer.Id

$attachedServers

"load balancer rules"

$newRules =@([OneAndOne.POCO.Requests.LoadBalancer.RuleRequest]@{PortBalancer=3030;PortServer=3030;Protocol="TCP";Source="0.0.0.0"})
$addedRules=New-OAOLoadBalancerRules -BalancerId $balancer.Id -Rules $newRules

waitLoadBalancer $balancer.Id

"listing all rules attached to the load balancer"
Get-OAOLoadBalancerRules -BalancerId $balancer.Id

"remove one rule from the balancer"
Remove-OAOLoadBalancerRules -BalancerId $balancer.Id -RuleId $addedRules[0].Rules[0].Id

"test clean up"

waitLoadBalancer $balancer.Id
Remove-OAOLoadBalancer -BalancerId $balancer.Id

waitServer -serverId $server.Id
Remove-OAOServer $server.Id
