
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
                                     $serverstatus.Percent -ne 0))

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

function waitFirewallPolicy
{
    Param ([String]$firewallId)
    start-sleep -seconds 5
	Do{
		"Waiting on firewall policy to deploy"

		$firewallStatus = Get-OAOFirewall -FirewallId $firewallId

		start-sleep -seconds 10
	}While($firewallStatus.State -ne "ACTIVE")

}

function waitIPRemoved
{
	Param ([String]$ipId)

    start-sleep -seconds 480

}

#setting up token and creating test server

$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

$serverAppliance =Get-OAOServerAppliance -Query "centos"

$hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})

$server=New-OAOServer -Name "test ps server ip1" -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverAppliance[0].Id `
-PowerOn $false 

#wait on server to deploy and get ready
waitServer -serverId $server.Id


#being IP test operations

$addIp=New-OAOServerIps -ServerId $server.Id


#wait on server to deploy and get ready
waitServer -serverId $server.Id

$getIps=Get-OAOServerIps -ServerId $server.Id

$server=Get-OAOServer -ServerId $server.Id

$rules =@([OneAndOne.POCO.Requests.LoadBalancer.LoadBalancerRuleRequest]@{PortBalancer=80;PortServer=80;Protocol="TCP";Source="0.0.0.0"})
$balancer=New-OAOLoadBalancer  -Name "ps test LoadBalancer" -Description "desc" -Rules $rules -HealthCheckTest NONE -HealthCheckInterval 1 -Method ROUND_ROBIN -Persistence $true -PersistenceTime 30

waitLoadBalancer $balancer.Id

$addLoadBalancer=New-OAOServerIpsLoadBalancers -ServerId $server.Id -IpId $server.Ips[0].Id -LoadBalancerId $balancer.Id


#wait on server to deploy and get ready
waitServer -serverId $server.Id

$loadBalancer=Get-OAOServerIpsLoadBalancers -ServerId $server.Id -IpId $server.Ips[0].Id

$removeLB=Remove-OAOServerIPLoadBalancer -ServerId $server.Id -IpId $server.Ips[0].Id -LoadBalancerId $loadBalancer.Id

#wait on server to deploy and get ready
waitServer -serverId $server.Id

$rules =@([OneAndOne.POCO.Requests.FirewallPolicies.CreateFirewallPocliyRule]@{PortTo=80;PortFrom=80;Protocol="TCP";Source="0.0.0.0"})
$firewallPolicy=New-OAOFirewallPolicy -Name "ps test firewall policy" -Rules $rules

waitFirewallPolicy $firewallPolicy.Id

$updateFirewall=Set-OAOServerIpsFirewalls -ServerId $server.Id -IpId $server.Ips[0].Id -FirewallId $firewallPolicy.Id

#wait on server to deploy and get ready
waitServer -serverId $server.Id

$getFW=Get-OAOServerIpsFirewalls -ServerId $server.Id -IpId $server.Ips[0].Id

if($getFW.Id -ne $null)
{
   write-host "Found FW" $getFW.Name
}

if($server.Ips.Count -gt 1)
{
    $removeIP=Remove-OAOServerIP -ServerId $server.Id -IpId $server.Ips[1].Id
    waitIPRemoved -ipId $server.Ips[1].Id
}

Start-Sleep -s 60

waitFirewallPolicy -firewallId $firewallPolicy.Id
Remove-OAOFirewallPolicy -FirewallId $firewallPolicy.Id

waitLoadBalancer -balancerId $balancer.Id
Remove-OAOLoadBalancer -BalancerId $balancer.Id

waitServer -serverId $server.Id
Remove-OAOServer -ServerId $server.Id






