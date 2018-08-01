# oneandone-cloudserver-powershell
1&amp;1 Cloud Server module for PowerShell

For more information on the 1&amp;1 Cloud Server module for PowerShell see the [1&1 Community Portal](https://www.1and1.com/cloud-community/).

# Table of Contents

* [Concepts](#concepts)
* [Getting Started](#getting-started)
* [Installation](#installation)
* [Operations](#operations)
  * [Servers](#servers)
  * [Images](#images)
  * [Shared Storages](#shared-storages)
  * [Firewall Policies](#firewall-policies)
  * [Load Balancers](#load-balancers)
  * [Public IPs](#public-ips)
  * [Private Networks](#private-networks)
  * [VPN](#vpn)
  * [Monitoring Center](#monitoring-center)
  * [Monitoring Policies](#monitoring-policies)
  * [Logs](#logs)
  * [Users](#users)
  * [Roles](#roles)
  * [Usages](#usages)
  * [Server Appliances](#server-appliances)
  * [DVD ISO](#dvd-iso)
  * [Ping](#ping)
  * [Pricing](#pricing)
  * [Datacenters](#datacenters)
  * [List All Commandlets](#list-all-commandlets)
  * [Get Help for a Commandlet](#get-help-for-a-commandlet)

## Concepts

The OneAndOne Powershell module wraps the [1&1 Cloud API](https://cloudpanel-api.1and1.com/documentation/1and1/v1/en/documentation.html), allowing you to interact with it from a command-line interface.

## Getting Started

Before you begin you will need to have signed up for a 1&1 account. The credentials you create during sign-up will be used to authenticate against the API.

To create a user and generate an API token that will be used to authenticate against the REST API, log into your 1&1 control panel. Go to the Server section -> Management -> Users.

## Installation

OneAndOne Powershell Module requires .NET Framework 4.5 or higher.  You can download .NET Framework version 4.5 from [here](https://www.microsoft.com/en-us/download/details.aspx?id=40779).

Download the [OneAndOne.zip](https://github.com/1and1/oneandone-powershell/releases/download/v1.0.0/OneAndOne.zip) file and extract all. Use one of the following options to make the module available inside PowerShell:

1. Place the resulting folder `OneAndOne` in `%USERPROFILE%\Documents\WindowsPowerShell\Modules\` to auto-load the module on PowerShell start for a specific user.
2. Place the resulting folder `OneAndOne` in `%SYSTEMROOT%\System32\WindowsPowerShell\v1.0\Modules\` to make the module available system-wide (not recommended).
3. Place the resulting folder `OneAndOne` in the folder of your choice and add that folder to the environment variable `PSModulePath` to make the module available system-wide.

**Note** In case the module does not autoload, you can explicitly import the module using `Import-Module OneAndOne` 

## Configuration

Before using the OneAndOne Powershell module to perform any operations, we need to set our credentials:

```
$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

Authorization successful
```

You will be notified with the following message if you have provided incorrect credentials:

```
Set-OneAndOne : Authentication failed
At line:9 char:1
+ Set-OneAndOne $credentials
+ ~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : AuthenticationError: (:) [Set-OneAndOne], Exception
    + FullyQualifiedErrorId : {"type":"UNAUTHORIZED","message":"The Token you are using is not valid","errors":null},OneAndOne.SetOneAndOne
```

After successful authentication the credentials will be stored locally for the duration of the PowerShell session.


## Operations

- [Servers](#servers)
- [Images](#images)
- [Shared Storages](#shared-storages)
- [Firewall Policies](#firewall-policies)
- [Load Balancers](#load-balancers)
- [Public IPs](#public-ips)
- [Private Networks](#private-networks)
- [VPN](#vpn)
- [Monitoring Center](#monitoring-center)
- [Monitoring Policies](#monitoring-policies)
- [Logs](#logs)
- [Users](#users)
- [Roles](#roles)
- [Usages](#usages)
- [Server Appliances](#server-appliances)
- [DVD ISO](#dvd-iso)
- [Ping](#ping)
- [Pricing](#pricing)
- [Datacenters](#datacenters)
  

## Servers

**List all servers:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOServer`

**Show a single server:**

`Get-OAOServer -ServerId $serverId`

**List available server flavors:**

`Get-OAOHardwareFlavor`

**Show a single server flavor:**

`Get-OAOHardwareFlavor -FlavorId $falvorId`

**List baremetal server models:**

`Get-OAOBaremetalModels`

**Show a single server flavor:**

`Get-OAOBaremetalModels -BaremetalModelId $modelId`

**Create a server:**

```
$hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})
New-OAOServer -Name "server name" -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverApplianceId -PowerOn $true 
```

**Create a baremetal server:**

```
New-OAOBaremetalServer -Name "ps baremetal serverhardware" -BaremetalModelId $baremetalModelsId -ApplianceId $serverApplianceId -PowerOn $false 
```

**Update a server:**		
	
`Set-OAOServer -ServerId $serverId -Name $new_name -Description "desc"`
			
**Delete a server:**

`Remove-OAOServer -ServerId $serverId`

**Show a server's hardware:**

`Get-OAOServerHardware -ServerId $serverId`

**Update a server's hardware:**

`Set-OAOServerHardware -ServerId $serverId -Vcore 4 -Ram 8`
					
**Get server's hard drives:**

`Get-OAOServerHdds -ServerId $serverId`

**Show a server's hard drive:**

`Get-OAOServerHdds -ServerId $serverId -HddId $hddId`

**Add a hard drive to a server:**

```
$newhdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=40;IsMain=$false})
New-OAOServerHdds -ServerId $serverId -Hdds $newhdds
```
					
**Update a server’s hard drive size:**

`Set-OAOServerHdds -ServerId $serverId -HddId $hddId -Size 60`
				
**Remove a hard drive from server:**

`Remove-OAOServerHdd -ServerId $serverId -HddId $hddId`

**Show the server's loaded DVD:**

`Get-OAOServerDvd -ServerId $serverId`

**Load a DVD into the server's unit:**

`Set-OAOServerDvd -ServerId $serverId -DvdId $dvdId`

**Unload the server's loaded DVD:**

`Remove-OAOServerDVD -ServerId $serverId`

**Show the server's image:**

`Get-OAOServerImage -ServerId $serverId`

**Reinstall a new image into a server:**

`Set-OAOServerImages -ServerId $serverId -ImageId $imageId -Password "Testing!123"`
					
**List the server's IP addresses:**

`Get-OAOServerIps -ServerId $serverId`

**Show a server's IP address:**

`Get-OAOServerIps -ServerId $serverId -IpId $ipId`

**Assign an IP to the server:**

`New-OAOServerIps -ServerId $serverId`
				
**Un-Assign an IP from the server:**

`//the bool parameter Set true for releasing the IP without removing it
Remove-OAOServerIP -ServerId $serverId -IpId $ipId -KeepIp $false`

**List server's firewall policies:**

`Get-OAOServerIpsFirewalls -ServerId $serverId -IpId $ipId`

**Adds a new firewall policy to the server's IP:**

`$rules=@([OneAndOne.POCO.Requests.FirewallPolicies.CreateFirewallPocliyRule]@{PortTo=80;PortFrom=80;Protocol="TCP";Source="0.0.0.0"})
New-OAOFirewallPolicy -Name "firewall policy" -Rules $rules`

**Remove a firewall policy from server's IP:**

`Remove-OAOServerIPFirewall -ServerId $serverId -IpId $ipId`

**List all server's IP address load balancers:**

`Get-OAOServerIpsLoadBalancers -ServerId $serverId -IpId $ipId`

**Add a new load balancer to the IP:**

`New-OAOServerIpsLoadBalancers -ServerId $serverId -IpId $ipId -LoadBalancerId $balancerId`

**Remove load balancer from the IP:**

`Remove-OAOServerIPLoadBalancer -ServerId $serverId -IpId $ipId -LoadBalancerId $loadBalancerId`

**Get server status:**

`Get-OAOServerStatus -ServerId $serverId`

**Change server status:**

```
Action can be POWER_OFF,POWER_ON or REBOOT, Method can be SOFTWARE or HARDWARE
Set-OAOServerStatus -ServerId $serverId -Action [action] -Method [Method]
```
			
**List server's private networks:**

`Get-OAOServerPrivateNetwork -ServerId $serverId`

**Show a server's private network:**

`Get-OAOServerPrivateNetwork -ServerId $serverId -PrivateNetworkId $privateNetworkId`

**Add a server's private network:**

`New-OAOServerPrivateNetwork -ServerId $serverId -PrivateNetworkId $privateNetworkId`

**Remove a server's private network:**

`Remove-OAOServerPrivateNetwork -ServerId $serverId -PrivateNetworkId $privateNetworkId`

**List server's snapshots:**

`Get-OAOServerSnapshot -ServerId $serverId`

**Creates a new snapshot of the server:**

`New-OAOServerSnapshot -ServerId $serverId`

**Restore a snapshot into server:**

`Set-OAOServerSnapshot -ServerId $serverId -SnapshotId $snapshotId`

**Remove a snapshot from server:**

`Remove-OAOServerSnapshot -ServerId $serverId -SnapshotId $snapshotId`

**Create a server clone:**

`New-OAOServerClone -ServerId $serverId -Name "image name"`
 
	
## Images

**List all images:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOImage`

**Get a single image:**

`Get-OAOImage -ImageId $imageId`

**Create an image:**

`New-OAOImage -ServerId $serverId -Name "image" -NumImages 2 -Source server -Frequency ONCE`

**Update an image:**


`Set-OAOImage -ImageId $imageId -Name "updated imagename" -Frequency ONCE`

**Delete an image:**

`Remove-OAOImage -ImageId $imageId`


## Shared Storages

**List shared storages:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOSharedStorage`

**Get a single shared storage:**

`Get-OAOSharedStorage -StorageId $storageId`

**Create a shared storage:**

`New-OAOSharedStorage -Name "storage name" -DatacenterId $datacenterId -Size 50 -Description "desc"`
				
**Update a shared storage:**

`Set-OAOSharedStorage -StorageId $sharedStorageId -Name "updated name"`
			
**Remove a shared storage:**

`Remove-OAOSharedStorage -StorageId $sharedStorageId`

**List a shared storage servers:**

`Get-OAOSharedStorageServer -StorageId $sharedStorageId`

**Get a shared storage single server:**

`Get-OAOSharedStorageServer -StorageId $sharedStorageId -ServerId $serverId`

**Attaches servers to a shared storage:**

`//servers is a list of server object that represents the server id with access rights
$servers =@([OneAndOne.POCO.Requests.SharedStorages.Server]@{Id=$server.Id;Rights="RW"})
New-OAOSharedStorageServer -StorageId $sharedStorage.Id -Servers $servers`
				
**Unattaches a server from a shared storage:**

`Remove-OAOSharedStorageServer -StorageId $sharedStorageId -ServerId $serverId`

**Return the credentials for accessing the shared storages:**

`Get-OAOSharedStorageAccess`

**Change the password for accessing the shared storages:**

`Set-OAOSharedStorageAccess -Password "Test123!1"`



## Firewall Policies

**List firewall policies:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOFirewall`

**Get a single firewall policy:**

`Get-OAOFirewall -FirewallId $firewallId`

**Create a firewall policy:**

```
$rules=@([OneAndOne.POCO.Requests.FirewallPolicies.CreateFirewallPocliyRule]@{Port="80";Action="Allow";Protocol="TCP";Source="0.0.0.0"})
$firewallPolicy=New-OAOFirewallPolicy -Name "name" -Rules $rules
```
			
**Update a firewall policy:**

`Set-OAOFirewallPolicy -FirewallId $firewallPolicyId -Name "ps updated name"`
			
**Delete a firewall policy:**

`Remove-OAOFirewallPolicy -FirewallId $firewallPolicyId`

**Return a list of the servers/IPs attached to a firewall policy:**

`Get-OAOFirewallPolicyServerIps -FirewallId $firewallPolicyId`

**Return information about a server/IP assigned to a firewall policy:**

`Get-OAOFirewallPolicyServerIps -FirewallId $firewallPolicyId -IpId $ipId`

**Assign servers/IPs to a firewall policy:**

`// ServerIps is a string list of server id
$ips =@($serverId)
New-OAOFirewallPolicyServerIps -FirewallId $firewallPolicyId -Ips $ips `

**Unassign a server/IP from a firewall policy:**

`Remove-OAOFirewallPolicyServerIps -FirewallId $firewallPolicyId -IpId $ipId`

**Return a list of the rules of a firewall policy:**

`Get-OAOFirewallPolicyRules -FirewallId $firewallPolicyId`

**Return information about a rule of a firewall policy:**

`Get-OAOFirewallPolicyRules -FirewallId $firewallPolicyId -RuleId $ruleId`

**Adds new rules to a firewall policy:**

```
$newRules=@([OneAndOne.POCO.Requests.FirewallPolicies.RuleRequest]@{Port="3030";Action="Allow";Protocol="TCP";Source="0.0.0.0"})
New-OAOFirewallPolicyRules -FirewallId $firewallPolicyId -Rules $newRules
```
				
**Remove a rule from a firewall policy:**

`Remove-OAOFirewallPolicyRules -FirewallId $firewallPolicyId -RuleId $ruleId`



## Load Balancers

**Return a list of your load balancers:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOLoadBalancer`

**Return information about a load balancer:**

`Get-OAOLoadBalancer -BalancerId $loadBalancerId`

**Create a new load balancer:**

```
$rules=@([OneAndOne.POCO.Requests.LoadBalancer.LoadBalancerRuleRequest]@{PortBalancer=80;PortServer=80;Protocol="TCP";Source="0.0.0.0"})
New-OAOLoadBalancer  -Name "name" -Description "desc" -Rules $rules -HealthCheckTest NONE -HealthCheckInterval 1 -Method ROUND_ROBIN -Persistence $true -PersistenceTime 30
```
				
**Modify a load balancer:**

`Set-OAOLoadBalancer -BalancerId $balancerId -Name "updated name"`

**Removes a load balancer:**	

`Remove-OAOLoadBalancer -BalancerId $balancerId`

**Return a list of the servers/IPs attached to a load balancer:**	

`Get-OAOLoadBalancerServerIps -BalancerId $balancerId`

**Return information about a server/IP assigned to a load balancer:**	

`Get-OAOLoadBalancerServerIps -BalancerId $balancer.Id -IpId $ipId`

**Assign servers/IPs to a load balancer:**	

```// iptoAdd is a list of string contains IDs of server Ips
$ips =@($server.Ips[0].Id)
New-OAOLoadBalancerServerIps -BalancerId $balancerId -Ips $ips 
```

**Unassign a server/IP from a load balancer:**	

`Remove-OAOLoadBalancerServerIps -BalancerId $balancerId -IpId $ipId`

**Return a list of the rules of a load balancer:**	

`Get-OAOLoadBalancerRules -BalancerId $balancerId`

**Returns information about a rule of a load balancer:**	

`Get-OAOLoadBalancerRules -BalancerId $balancerId -RuleId $ruleId`

**Add new rules to a load balancer:**	

```
$newRules=@([OneAndOne.POCO.Requests.LoadBalancer.RuleRequest]@{PortBalancer=3030;PortServer=3030;Protocol="TCP";Source="0.0.0.0"})
New-OAOLoadBalancerRules -BalancerId $balancerId -Rules $newRules
```

**Removes a rule from a load balancer:**	

`Remove-OAOLoadBalancerRules -BalancerId $balancer.Id -RuleId $addedRules[0].Rules[0].Id`	

## Public IPs

**Return a list of your public IPs:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOPublicIP`

**Return information about a public IP:**

`Get-OAOPublicIP -IpId $ipId`

**Creates a new public IP:**

`New-OAOPublicIP  -ReverseDns "dns.com" -Type IPV4`

**Modify the reverse DNS of a public IP:**

`Set-OAOPublicIP -IpId $ipId -ReverseDns "dns.com"`

**Remove a public IP:**

`Remove-OAOPublicIP $ipId`


## Private Networks

**Return a list of your private networks:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOPrivateNetwork`

**Return information about a private network:**

`Get-OAOPrivateNetwork -NetworkId $networkId`

**Create a new private network:**

`New-OAOPrivateNetwork -Name "network test" -Description "desc" -SubnetMask "255.255.255.0" -DatacenterId $datacenterId -NetworkAddress "192.168.1.0"`

**Modify a private network:**

`Set-OAOPrivateNetwork -NetworkId $networkId -Name "updated name" SubnetMask "255.255.255.0" -NetworkAddress "192.168.1.0"`

**Remove a private network:**

`Remove-OAOPrivateNetwork -NetworkId $networkId`

## VPN

**Return a list of your vpns:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOVpn`

**Return information about a vpn:**

`Get-OAOVpn -VpnId $vpnId`

**Download your VPN configuration file, a string with base64 format with the configuration for OpenVPN. It is a zip file:**

`Get-OAOVpnConfiguration -VpnId $vpnId`

**Create a new vpn:**

`New-OAOVpn -Name "test"`

**Modify a vpn:**

`Set-OAOVpn -VpnId $vpnId -Name "update ps name"`

**Remove a vpn:**

`Remove-OAOVpn $vpnId`


## Monitoring Center

**List usages and alerts of monitoring servers:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOMonitoringCenter`


**Return the usage of the resources for the specified time range:**

`Get-OAOMonitoringCenterServer -ServerId $serverId -Period CUSTOM -StartDate ([System.DateTime]::Now).AddDays(-2)   -EndDate ([System.DateTime]::Now)`



## Monitoring Policies

**Return a list of your monitoring policies:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOMonitoringPolicy`

**Return information about a monitoring policy:**

`Get-OAOMonitoringPolicy -PolicyId $policyId`

**Create a new monitoring policy:**

```
$ports=@([OneAndOne.POCO.Requests.MonitoringPolicies.Ports]@{AlertIf="NOT_RESPONDING";EmailNotification=$false;Port=22;Protocol="TCP"})
$processes=@([OneAndOne.POCO.Requests.MonitoringPolicies.Processes]@{AlertIf="NOT_RUNNING";EmailNotification=$false;Process="test"})
```
```
New-OAOMonitoringPolicy -Name "ps test MP" -Email "email@test.com" -Agent $true -CpuWarningValue 90 -CpuWarningAlert $false -CpuCriticalValue 95 -CpuCriticalAlert $false
-RamWarningValue 90 -RamWarningAlert $false -RamCriticalValue 95 -RamCriticalAlert $false -DiskWarningValue 80 -DiskWarningAlert $false -DiskCriticalValue 90 -DiskCriticalAlert $false
-InternalPingWarningValue 50 -InternalPingWarningAlert $false -InternalPingCriticalValue 100 -InternalPingCriticalAlert $false -TransferWarningValue 1000 -TransferWarningAlert $false
-TransferCriticalValue 2000 -TransferCriticalAlert $false -Description "desc" -Ports $ports -Processes $processes
```

**Modifiy a monitoring policy:**

`Set-OAOMonitoringPolicy -PolicyId $mpId -Name "update ps name" CpuWarningValue 90 -CpuWarningAlert $false -CpuCriticalValue 95 -CpuCriticalAlert $false
-RamWarningValue 90 -RamWarningAlert $false -RamCriticalValue 95 -RamCriticalAlert $false -DiskWarningValue 80 -DiskWarningAlert $false -DiskCriticalValue 90 -DiskCriticalAlert $false
-InternalPingWarningValue 50 -InternalPingWarningAlert $false -InternalPingCriticalValue 100 -InternalPingCriticalAlert $false -TransferWarningValue 1000 -TransferWarningAlert $false
-TransferCriticalValue 2000 -TransferCriticalAlert $false -Description "desc"`    

**Remove a monitoring policy:**

`Remove-OAOMonitoringPolicy -PolicyId $mpId`

**Return a list of the ports of a monitoring policy:**

`Get-OAOMonitoringPolicyPort -PolicyId $mpId`

**Returns information about a port of a monitoring policy:**

`Get-OAOMonitoringPolicyPort -PolicyId $mpId -PortId portId`

**Add new ports to a monitoring policy:**

```
$portsToAdd=[OneAndOne.POCO.Requests.MonitoringPolicies.Ports]@{AlertIf="NOT_RESPONDING";EmailNotification=$false;Port=25;Protocol="TCP"}
New-OAOMonitoringPolicyPort -PolicyId $mpId -Ports $portsToAdd
```

**Modify a port from a monitoring policy:**

```
$portsToUpdate=[OneAndOne.POCO.Requests.MonitoringPolicies.Ports]@{AlertIf="NOT_RESPONDING";EmailNotification=$true;Port=25;Protocol="TCP"}
Set-OAOMonitoringPolicyPort -PolicyId $mpId -PortId $portId -Ports $portsToUpdate
```

**Remove a port from a monitoring policy:**

`Remove-OAOMonitoringPolicyPort -PolicyId $mpId -PortsId $portId`

**Return a list of the processes of a monitoring policy:**

`Get-OAOMonitoringPolicyProcess -PolicyId $mpId`

**Return information about a process of a monitoring policy:**

`Get-OAOMonitoringPolicyProcess -PolicyId $mpId -ProcessId $processId`

**Add new processes to a monitoring policy:**

```
$processesToAdd=[OneAndOne.POCO.Requests.MonitoringPolicies.Processes]@{AlertIf="NOT_RUNNING";EmailNotification=$false;Process="init"}
New-OAOMonitoringPolicyProcess -PolicyId $mpId -Processes $processes 
```

**Modify a process from a monitoring policy:**

```
$processesToUpdate=[OneAndOne.POCO.Requests.MonitoringPolicies.Processes]@{AlertIf="NOT_RUNNING";EmailNotification=$true;Process="init"}
$updatedProcesses=Set-OAOMonitoringPolicyProcess -PolicyId $mpId -ProcessId $processId -Processes $processesToUpdate
```

**Remove a process from a monitoring policy:**

`Remove-OAOMonitoringPolicyProcess -PolicyId $mpId -ProcessId $processId`

**Return a list of the servers attached to a monitoring policy:**

`Get-OAOMonitoringPolicyServers -PolicyId $mpId`

**Return information about a server attached to a monitoring policy:**

`Get-OAOMonitoringPolicyServers -PolicyId $mpId -ServerId $serverId`

**Attach servers to a monitoring policy:**

`//serverToAdd are a list of string of the servers IDs
$serverToAdd =@($server.Id)
New-OAOMonitoringPolicyServers -PolicyId $mpId -Servers $serverToAdd`

**Unattach a server from a monitoring policy:**

`Remove-OAOMonitoringPolicyServers -PolicyId $mpId -ServerId $serverId`


## Logs

**Return a list with logs:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOLogs -Period LAST_24H`

**Return information about a log:**

`Get-OAOLogs -LogId $logId`


## Users

**Return a list with all users:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOUser`

**Return information about a user:**

`Get-OAOUser -UserId $userId`

**Creates a new user:**

`New-OAOUser -Name "PSTestUser" -Password "test123!AA" -Description "delete me"`

**Modify user information:**

`Set-OAOUser -UserId $userId -State ACTIVE`

**Remove a user:**

`Remove-OAOUser -UserId $userId`

**Information about API:**

`Get-OAOUserAPI -UserId $userId`

**Allow to enable or disable the API:**

`Set-OAOUserAPI -UserId $user.Id -Active $true`

**Show the API key:**

`Get-OAOUserAPIKey -UserId $userId`

**Change the API key:**

`Set-OAOUserAPIKey -UserId $userId`

**Return IPs from which access to API is allowed:**

`var result = client.UserAPI.GetUserIps(UserId);`

**Allow a new IP:**

`New-OAOUserAllowedIps -UserId $userId -Ips @("127.0.0.1")`

**Delete an IP and forbid API access for it:**

`Remove-OAOUserAllowedIps -UserId $user.Id -IP $ip`

## Roles

**Return a list with all roles:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAORole`

**Return information about a role:**

`Get-OAORole -RoleId $roleId`

**Creates a new role:**

```
New-OAORole -Name "test role"
```

**Modify role information:**

`Set-OAORole -RoleId $roleId -Name "updated name" -State ACTIVE`

**Remove a role:**

`Remove-OAORole -RoleId $roleId`

**Lists role's permissions:**

`Get-OAORolePermissions -RoleId $roleId`

**Adds permissions to the role:**

```
$permissionsServer=[OneAndOne.POCO.Response.Roles.Servers]@{Show=$true;SetName=$false;Shutdown=$true}
Set-OAORolePermissions -RoleId $roleId -Servers $permissionsServer
```

**Returns users assigned to role:**

`Get-OAORoleUser -RoleId $roleId`

**Add users to role:**

`New-OAORoleUser -RoleId $roleId -Users @($userId);`

**Returns information about a user:**

`Get-OAORoleUser -RoleId $roleId -UserId $userId`

**Removes user from role:**

```
Remove-OAORoleUser -RoleId $roleId -UserId $userId
```

**Clones a role:**

`New-OAORoleClone -RoleId $roleId -Name "clone role"`

## Usages

**Return a list of your usages:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOUsage -Period LAST_24H`


## Server Appliances

**Return a list of all the appliances that you can use for creating a server:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAOServerAppliance`

**Return Information about specific appliance:**

`Get-OAOServerAppliance -ApplianceId $applianceId`


## DVD ISO

**Return a list of all the operative systems and tools that you can load into your virtual DVD unit:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAODvd`

**Information about specific ISO image:**

`Get-OAODvd -DvdId $dvdId`


## Ping

**Ping the API, returns true if the API is running:**

`Get-OAOPingApi`

**Ping the API Authentication, returns true if the API token is valid**

`Get-OAOPingAuthentication`

## Pricing

**Returns prices for all available resources in Cloud Panel:**

`Get-OAOPricing;`

## Datacenters

**Returns information about available datacenters to create your resources:**
*You can use any of the optional paramters provided from the API (Page, PerPage, Query, Sort, Fields) to filter the list operation*

`Get-OAODatacenter`

**Returns information about a datacenter:**

`Get-OAODatacenter -DcId $dc.Id`


## List Commandlets

Now we have had a taste of working with the OneAndOne Powershell module. To get more details on every commandlet contained in OneAndOne Powershell module you can do this:

```
CommandType     Name                                               Version    Source                                                                                                          
-----------     ----                                               -------    ------                                                                                                          
Cmdlet          Get-OAODatacenter                                  1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAODvd                                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOFirewall                                    1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOFirewallPolicyRules                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOFirewallPolicyServerIps                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOHardwareFlavor                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOImage                                       1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOLoadBalancer                                1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOLoadBalancerRules                           1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOLoadBalancerServerIps                       1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOLogs                                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOMonitoringCenter                            1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOMonitoringCenterServer                      1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOMonitoringPolicy                            1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOMonitoringPolicyPort                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOMonitoringPolicyProcess                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOMonitoringPolicyServers                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOPingApi                                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOPingAuthentication                          1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOPricing                                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOPrivateNetwork                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOPrivateNetworkServer                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOPublicIP                                    1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAORole                                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAORolePermissions                             1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAORoleUser                                    1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServer                                      1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerAppliance                             1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerDvd                                   1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerHardware                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerHdds                                  1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerImage                                 1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerIps                                   1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerIpsFirewalls                          1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerIpsLoadBalancers                      1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerPrivateNetwork                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerSnapshot                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOServerStatus                                1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOSharedStorage                               1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOSharedStorageAccess                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOSharedStorageServer                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOUsage                                       1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOUser                                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOUserAllowedIps                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOUserAPI                                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOUserAPIKey                                  1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOVpn                                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Get-OAOVpnConfiguration                            1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOFirewallPolicy                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOFirewallPolicyRules                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOFirewallPolicyServerIps                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOImage                                       1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOLoadBalancer                                1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOLoadBalancerRules                           1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOLoadBalancerServerIps                       1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOMonitoringPolicy                            1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOMonitoringPolicyPort                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOMonitoringPolicyProcess                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOMonitoringPolicyServers                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOPrivateNetwork                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOPrivateNetworkServer                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOPublicIP                                    1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAORole                                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAORoleClone                                   1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAORoleUser                                    1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOServer                                      1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOServerClone                                 1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOServerHdds                                  1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOServerIps                                   1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOServerIpsLoadBalancers                      1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOServerPrivateNetwork                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOServerSnapshot                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOServerWithFlavor                            1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOSharedStorage                               1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOSharedStorageServer                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOUser                                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOUserAllowedIps                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          New-OAOVpn                                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOFirewallPolicy                           1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOFirewallPolicyRules                      1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOFirewallPolicyServerIps                  1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOImage                                    1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOLoadBalancer                             1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOLoadBalancerRules                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOLoadBalancerServerIps                    1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOMonitoringPolicy                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOMonitoringPolicyPort                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOMonitoringPolicyProcess                  1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOMonitoringPolicyServers                  1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOPrivateNetwork                           1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOPrivateNetworkServer                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOPublicIP                                 1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAORole                                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAORoleUser                                 1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOServer                                   1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOServerDVD                                1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOServerHdd                                1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOServerIP                                 1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOServerIPFirewall                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOServerIPLoadBalancer                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOServerPrivateNetwork                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOServerSnapshot                           1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOSharedStorage                            1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOSharedStorageServer                      1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOUser                                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOUserAllowedIps                           1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Remove-OAOVpn                                      1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOFirewallPolicy                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOImage                                       1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOLoadBalancer                                1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOMonitoringPolicy                            1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOMonitoringPolicyPort                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOMonitoringPolicyProcess                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOPrivateNetwork                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOPublicIP                                    1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAORole                                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAORolePermissions                             1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOServer                                      1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOServerDvd                                   1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOServerHardware                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOServerHdds                                  1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOServerImages                                1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOServerIpsFirewalls                          1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOServerSnapshot                              1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOServerStatus                                1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOSharedStorage                               1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOSharedStorageAccess                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOUser                                        1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOUserAPI                                     1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOUserAPIKey                                  1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OAOVpn                                         1.0.0.0    OneAndOne                                                                                                       
Cmdlet          Set-OneAndOne                                      1.0.0.0    OneAndOne 
```

## Get Help for a Commandlet

To get help for a specific commandlet do the following:

```
Get-Help Set-OneAndOne -Full

NAME
    Set-OneAndOne
    
SYNTAX
    Set-OneAndOne [-Credential] <pscredential>  [<CommonParameters>]
    
    
PARAMETERS
    -Credential <pscredential>
        1&1 Authentication Token
        
        Required?                    true
        Position?                    0
        Accept pipeline input?       true (ByValue)
        Parameter set name           (All)
        Aliases                      None
        Dynamic?                     false
        
    <CommonParameters>
        This cmdlet supports the common parameters: Verbose, Debug,
        ErrorAction, ErrorVariable, WarningAction, WarningVariable,
        OutBuffer, PipelineVariable, and OutVariable. For more information, see 
        about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216). 
    
    
INPUTS
    System.Management.Automation.PSCredential
    
    
OUTPUTS
    System.Object
    
ALIASES
    Set OneAndOne Token
    

REMARKS
    None
```
	
