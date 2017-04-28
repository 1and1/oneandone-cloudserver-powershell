using OneAndOne.Client;
using OneAndOne.POCO.Requests.Users;
using OneAndOne.POCO.Response.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OneAndOne
{
    #region main operations

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list with all roles, or one role if the RoleId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAORole -RoleId [UUID] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAORole")]
    [OutputType(typeof(RoleResponse))]
    public class GetRole : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Allows to use pagination. Sets the number of servers that will be shown in each page.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Allows to use pagination. Sets the number of servers that will be shown in each page.", Mandatory = false, ValueFromPipeline = true)]
        public int? Page { get; set; }

        /// <summary>
        /// <para type="description">Current page to show.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Current page to show.", Mandatory = false, ValueFromPipeline = true)]
        public int? PerPage { get; set; }

        /// <summary>
        /// <para type="description">Allows to sort the result by priority:sort=name retrieves a list of elements ordered by their names.sort=-creation_date retrieves a list of elements ordered according to their creation date in descending order of priority.</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Allows to sort the result by priority:sort=name retrieves a list of elements ordered by their names.sort=-creation_date retrieves a list of elements ordered according to their creation date in descending order of priority.", Mandatory = false, ValueFromPipeline = true)]
        public string Sort { get; set; }

        /// <summary>
        /// <para type="description">Allows to search one string in the response and return the elements that contain it. In order to specify the string use parameter q:    q=My server.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Allows to search one string in the response and return the elements that contain it. In order to specify the string use parameter q:    q=My server", Mandatory = false, ValueFromPipeline = true)]
        public string Query { get; set; }

        /// <summary>
        /// <para type="description">Returns only the parameters requested: fields=id,name,description,hardware.ram</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Returns only the parameters requested: fields=id,name,description,hardware.ram", Mandatory = false, ValueFromPipeline = true)]
        public string Fields { get; set; }

        /// <summary>
        /// <para type="description">Role ID. If this parameters is not passed, the commandlet will return a list of all roles.</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "Role Id", Mandatory = false, ValueFromPipeline = true)]
        public string RoleId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var roleApi = client.Roles;

                if (string.IsNullOrEmpty(RoleId))
                {
                    var policy = roleApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(policy);
                }
                else
                {
                    var policy = roleApi.Show(RoleId);
                    WriteObject(policy);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will create a new role.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAORole -Name [name] -Description -Email [email] -Password [password]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAORole")]
    [OutputType(typeof(RoleResponse))]
    public class NewRole : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Name</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Name", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var roleApi = client.Roles;
                var result = roleApi.Create(Name);
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet will clone a role.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAORoleClone -RoleId [UUID] -Name [name]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAORoleClone")]
    [OutputType(typeof(RoleResponse))]
    public class NewRoleClone : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Clone Name</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Clone Name", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Role ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Role Id", Mandatory = true, ValueFromPipeline = true)]
        public string RoleId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var roleApi = client.Roles;
                var result = roleApi.CreateRoleClone(Name, RoleId);
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a role.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAORole -RoleId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAORole")]
    [OutputType(typeof(RoleResponse))]
    public class RemoveRole : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Role ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Role Id", Mandatory = true, ValueFromPipeline = true)]
        public string RoleId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var usersApi = client.Roles;
                var resp = usersApi.Delete(RoleId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies role information.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAORole -RoleId [UUID] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAORole")]
    [OutputType(typeof(RoleResponse))]
    public class SetRole : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters


        /// <summary>
        /// <para type="description">Allows to enable or disable the role</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Allows to enable or disable users", Mandatory = false, ValueFromPipeline = true)]
        public UserState? State { get; set; }

        /// <summary>
        /// <para type="description">Role ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Role Id", Mandatory = true, ValueFromPipeline = true)]
        public string RoleId { get; set; }

        /// <summary>
        /// <para type="description">Name</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Name", Mandatory = false, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Description</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Description", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Constants.Configuration);
            var roleApi = client.Roles;
            var role = roleApi.Show(RoleId);
            var resp = roleApi.Update(Name, Description, State ?? role.State, RoleId);
            WriteObject(resp);
        }
    }

    #endregion

    #region permissions

    /// <summary>
    /// <para type="synopsis">This commandlet lists role's permissions.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAORolePermissions -RoleId [UUID]</para>
    /// </example>

    [Cmdlet(VerbsCommon.Get, "OAORolePermissions")]
    [OutputType(typeof(Permissions))]
    public class GetRolePermissions : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Role ID..</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Role Id", Mandatory = true, ValueFromPipeline = true)]
        public string RoleId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var roleApi = client.Roles;

                var permissions = roleApi.GetPermissions(RoleId);
                WriteObject(permissions);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet adds permissions to the role.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAORolePermissions -RoleId [UUID] -Servers [Servers] -Images [Images] -SharedStorages [SharedStorages] -FirewallPolicies [FirewallPolicies] -LoadBalancers [LoadBalancers] -PublicIps [PublicIps] -PrivateNetwork [PrivateNetwork] -Vpn [Vpn] -MonitoringCenter [MonitoringCenter] -MonitoringPolicies [MonitoringPolicies] -Backups [Backups]      -Logs [Logs] -Users [Users] -Roles [Roles] -Usages [Usages] -InteractiveInvoices [InteractiveInvoices] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAORolePermissions")]
    [OutputType(typeof(RoleResponse))]
    public class SetRolePermissons : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Role ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Role Id", Mandatory = true, ValueFromPipeline = true)]
        public string RoleId { get; set; }

        /// <summary>
        /// <para type="description">Server Permissions.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Server Permissions", Mandatory = false, ValueFromPipeline = true)]
        public POCO.Response.Roles.Servers Servers { get; set; }

        /// <summary>
        /// <para type="description">Images Permissions</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Images Permissions", Mandatory = false, ValueFromPipeline = true)]
        public Images Images { get; set; }

        /// <summary>
        /// <para type="description">SharedStorages Permissions</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "SharedStorages Permissions", Mandatory = false, ValueFromPipeline = true)]
        public SharedStorages SharedStorages { get; set; }

        /// <summary>
        /// <para type="description">FirewallPolicies Permissions</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "FirewallPolicies Permissions", Mandatory = false, ValueFromPipeline = true)]
        public FirewallPolicies FirewallPolicies { get; set; }

        /// <summary>
        /// <para type="description">LoadBalancers Permissions</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "LoadBalancers Permissions", Mandatory = false, ValueFromPipeline = true)]
        public LoadBalancers LoadBalancers { get; set; }

        /// <summary>
        /// <para type="description">PublicIps Permissions</para>
        /// </summary>
        [Parameter(Position = 6, HelpMessage = "PublicIps Permissions", Mandatory = false, ValueFromPipeline = true)]
        public PublicIps PublicIps { get; set; }

        /// <summary>
        /// <para type="description">PrivateNetwork Permissions</para>
        /// </summary>
        [Parameter(Position = 7, HelpMessage = "PrivateNetwork Permissions", Mandatory = false, ValueFromPipeline = true)]
        public PrivateNetwork PrivateNetwork { get; set; }

        /// <summary>
        /// <para type="description">Vpn Permissions</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "Vpn Permissions", Mandatory = false, ValueFromPipeline = true)]
        public Vpn Vpn { get; set; }

        /// <summary>
        /// <para type="description">MonitoringCenter Permissions</para>
        /// </summary>
        [Parameter(Position = 9, HelpMessage = "MonitoringCenter Permissions", Mandatory = false, ValueFromPipeline = true)]
        public MonitoringCenter MonitoringCenter { get; set; }

        /// <summary>
        /// <para type="description">MonitoringPolicies Permissions</para>
        /// </summary>
        [Parameter(Position = 10, HelpMessage = "MonitoringPolicies Permissions", Mandatory = false, ValueFromPipeline = true)]
        public MonitoringPolicies MonitoringPolicies { get; set; }

        /// <summary>
        /// <para type="description">Backups Permissions</para>
        /// </summary>
        [Parameter(Position = 11, HelpMessage = "Backups Permissions", Mandatory = false, ValueFromPipeline = true)]
        public Backups Backups { get; set; }

        /// <summary>
        /// <para type="description">Logs Permissions</para>
        /// </summary>
        [Parameter(Position = 12, HelpMessage = "Logs Permissions", Mandatory = false, ValueFromPipeline = true)]
        public Logs Logs { get; set; }

        /// <summary>
        /// <para type="description">Users Permissions</para>
        /// </summary>
        [Parameter(Position = 13, HelpMessage = "Users Permissions", Mandatory = false, ValueFromPipeline = true)]
        public Users Users { get; set; }

        /// <summary>
        /// <para type="description">Roles Permissions</para>
        /// </summary>
        [Parameter(Position = 14, HelpMessage = "Roles Permissions", Mandatory = false, ValueFromPipeline = true)]
        public Roles Roles { get; set; }

        /// <summary>
        /// <para type="description">Usages Permissions</para>
        /// </summary>
        [Parameter(Position = 15, HelpMessage = "Usages Permissions", Mandatory = false, ValueFromPipeline = true)]
        public Usages Usages { get; set; }

        /// <summary>
        /// <para type="description">InteractiveInvoices Permissions</para>
        /// </summary>
        [Parameter(Position = 16, HelpMessage = "InteractiveInvoices Permissions", Mandatory = false, ValueFromPipeline = true)]
        public InteractiveInvoices InteractiveInvoices { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Constants.Configuration);
            var rolesApi = client.Roles;
            var resp = rolesApi.UpdatePermissions(new Permissions
            {
                Servers = Servers,
                SharedStorages = SharedStorages,
                Backups = Backups,
                FirewallPolicies = FirewallPolicies,
                Images = Images,
                InteractiveInvoices = InteractiveInvoices,
                LoadBalancers = LoadBalancers,
                Logs = Logs,
                MonitoringCenter = MonitoringCenter,
                MonitoringPolicies = MonitoringPolicies,
                PrivateNetwork = PrivateNetwork,
                PublicIps = PublicIps,
                Roles = Roles,
                Usages = Usages,
                Users = Users,
                Vpn = Vpn
            }, RoleId);

            WriteObject(resp);
        }
    }

    #endregion

    #region users

    /// <summary>
    /// <para type="synopsis">This commandlet returns users assigned to role.if the UserId paramter is provided it will return information about that user</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAORoleUser -RoleId [UUID] -UserId [UUID]</para>
    /// </example>

    [Cmdlet(VerbsCommon.Get, "OAORoleUser")]
    [OutputType(typeof(User))]
    public class GetRoleUser : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Role ID..</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Role Id", Mandatory = true, ValueFromPipeline = true)]
        public string RoleId { get; set; }

        /// <summary>
        /// <para type="description">User ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "User Id", Mandatory = false, ValueFromPipeline = true)]
        public string UserId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var roleApi = client.Roles;
                if (string.IsNullOrEmpty(UserId))
                {
                    var users = roleApi.GetRoleUsers(RoleId);
                    WriteObject(users);
                }
                else
                {
                    var userInfo = roleApi.ShowRoleUser(RoleId, UserId);
                    WriteObject(userInfo);
                }



            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will add users to role</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAORoleUser -RoleId [UUID] -Users [array]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAORoleUser")]
    [OutputType(typeof(RoleResponse))]
    public class NewRoleUser : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Role ID..</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Role Id", Mandatory = true, ValueFromPipeline = true)]
        public string RoleId { get; set; }

        /// <summary>
        /// <para type="description">Array of user IDs.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Array of user IDs", Mandatory = true, ValueFromPipeline = true)]
        public string[] Users { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var roleApi = client.Roles;
                var result = roleApi.CreateRoleUsers(new List<string>(Users), RoleId);
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes user from role.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAORoleUser -RoleId [UUID] -UserId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAORoleUser")]
    [OutputType(typeof(RoleResponse))]
    public class RemoveRoleUser : Cmdlet
    {
        private static OneAndOneClient client;
        /// <summary>
        /// <para type="description">Role ID..</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Role Id", Mandatory = true, ValueFromPipeline = true)]
        public string RoleId { get; set; }

        /// <summary>
        /// <para type="description">User ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "User Id", Mandatory = true, ValueFromPipeline = true)]
        public string UserId { get; set; }


        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var roleApi = client.Roles;
                var result = roleApi.DeleteRoleUser(UserId, RoleId);
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #endregion

}
