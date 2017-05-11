Import-Module ..\bin\Debug\OneAndOne.dll

$SecurePassword = [Environment]::GetEnvironmentVariable("OAO_TOKEN") | ConvertTo-SecureString -AsPlainText -Force

$Credentials = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList "OAO", $SecurePassword

$credentials = Get-Credential $Credentials

Set-OneAndOne $credentials

"Create test role"

$role=New-OAORole -Name "test role"

$testUser=New-OAOUser -Name "PSTestUser" -Password "test123!AA" -Description "delete me"

"Listing roles"
Get-OAORole

"Listing our test role"
Get-OAORole -RoleId $role.Id

$permissionsServer=[OneAndOne.POCO.Response.Roles.Servers]@{Show=$true;SetName=$false;Shutdown=$true}

"Updating permissions"
Set-OAORolePermissions -RoleId $role.Id -Servers $permissionsServer

$updated=Get-OAORolePermissions -RoleId $role.Id

if($updated.Servers.Show -eq $true -and $updated.Servers.SetName -eq $false -and $updated.Servers.Shutdown -eq $true)
{
    "Update permissions passed"
}else{
    "Update permissions failed"
}

"creating clone role"

$clone=New-OAORoleClone -RoleId $role.Id -Name "clone role"

"adding a user to the role"

$addedUser=New-OAORoleUser -RoleId $role.Id -Users @($testUser.Id)

"listing role users"

Get-OAORoleUser -RoleId $role.Id

"Showing one user"
Get-OAORoleUser -RoleId $role.Id -UserId $testUser.Id

"update role name"

Set-OAORole -RoleId $role.Id -Name "updated name" -State ACTIVE

"test cleanup"

Remove-OAORoleUser -RoleId $role.Id -UserId $testUser.Id
Remove-OAORole -RoleId $role.Id
Remove-OAORole -RoleId $clone.Id




