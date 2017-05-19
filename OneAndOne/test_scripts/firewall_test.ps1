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



$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials
$serverAppliance =Get-OAOServerAppliance -Query "centos"

$hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})

$server=New-OAOServer -Name "test ps firewall policy" -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverAppliance[0].Id `
-PowerOn $false 

#wait on server to deploy and get ready
waitServer -serverId $server.Id

$rules =@([OneAndOne.POCO.Requests.FirewallPolicies.CreateFirewallPocliyRule]@{PortTo=80;PortFrom=80;Protocol="TCP";Source="0.0.0.0"})
$firewallPolicy=New-OAOFirewallPolicy -Name "ps test firewall policy" -Rules $rules

waitFirewallPolicy $firewallPolicy.Id


"listing firewall policy"
Get-OAOFirewall

$updatePolicy=Set-OAOFirewallPolicy -FirewallId $firewallPolicy.Id -Name "ps updated name"

waitFirewallPolicy $firewallPolicy.Id

if($updatePolicy.Name -eq "ps updated name")
{
    "name changed"
}else{
    "failed to update name"
}

"add a server to the firewall policy"
$server=Get-OAOServer -ServerId $server.Id

"ip id"
$server.Ips[0].Id

$ips =@($server.Ips[0].Id)
$addedServer=New-OAOFirewallPolicyServerIps -FirewallId $firewallPolicy.Id -Ips $ips 

waitFirewallPolicy $firewallPolicy.Id

$attachedIps=Get-OAOFirewallPolicyServerIps -FirewallId $firewallPolicy.Id

"List of attached ips"
$attachedIps

Remove-OAOFirewallPolicyServerIps -FirewallId $firewallPolicy.Id -IpId $server.Ips[0].Id

waitFirewallPolicy $firewallPolicy.Id

"List of attached ips should be empty"

$attachedServers=Get-OAOFirewallPolicyServerIps -FirewallId $firewallPolicy.Id

$attachedServers

"firewall policy rules"

$newRules =@([OneAndOne.POCO.Requests.FirewallPolicies.RuleRequest]@{PortTo=3030;PortFrom=3030;Protocol="TCP";Source="0.0.0.0"})
$addedRules=New-OAOFirewallPolicyRules -FirewallId $firewallPolicy.Id -Rules $newRules

waitFirewallPolicy $firewallPolicy.Id

"listing all rules attached to the firewall policy"
Get-OAOFirewallPolicyRules -FirewallId $firewallPolicy.Id

"remove one rule from the policy"
Remove-OAOFirewallPolicyRules -FirewallId $firewallPolicy.Id -RuleId $addedRules[0].Rules[0].Id

"test clean up"

waitFirewallPolicy $firewallPolicy.Id
Remove-OAOFirewallPolicy -FirewallId $firewallPolicy.Id

waitServer -serverId $server.Id
Remove-OAOServer $server.Id
