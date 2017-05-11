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

function waitImage
{
    Param ([String]$imageId)
    start-sleep -seconds 5
	Do{
		"Waiting on image to deploy"

		$imageStatus = Get-OAOImage -ImageOd $imageId

		start-sleep -seconds 10
	}While($imageStatus.State -ne "ACTIVE")

}



$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials
$serverAppliance =Get-OAOServerAppliance -Query "centos"

$hdds =@([OneAndOne.POCO.Requests.Servers.HddRequest]@{Size=20;IsMain=$true})

$server=New-OAOServer -Name "test ps images" -Vcore 4 -CoresPerProcessor 2 -Ram 4 -Hdds $hdds -ApplianceId $serverAppliance[0].Id `
-PowerOn $false 

#wait on server to deploy and get ready
waitServer -serverId $server.Id

$image=New-OAOImage -ServerId $server.Id -Name "ps test image" -NumImages 2 -Source server -Frequency ONCE

waitImage -imageId $image.Id

waitServer -serverId $server.Id

$getImage=Get-OAOImage -ImageId $image.Id

waitServer -serverId $server.Id

"update image"
 
$updateImage=Set-OAOImage -ImageId $getImage.Id -Name "updated image ps test name" -Frequency ONCE

waitImage -imageId $image.Id

Start-Sleep -s 10

waitServer -serverId $server.Id
Remove-OAOImage -ImageId $image.Id

waitServer -serverId $server.Id
Remove-OAOServer -ServerId $server.Id
