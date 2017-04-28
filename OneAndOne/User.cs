using OneAndOne.Client;
using OneAndOne.POCO.Requests.Users;
using OneAndOne.POCO.Response.Users;
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
    /// <para type="synopsis">This commandlet returns a list with all users, or one user if the UserId is provided , if the ID parameter is provided all the optional paramters are ignored</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOUser -UserId [UUID] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOUser")]
    [OutputType(typeof(UserResponse))]
    public class GetUser : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">User ID. If this parameters is not passed, the commandlet will return a list of all users.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "User Id", Mandatory = false, ValueFromPipeline = true)]
        public string UserId { get; set; }

        /// <summary>
        /// <para type="description">Allows to use pagination. Sets the number of servers that will be shown in each page.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Allows to use pagination. Sets the number of servers that will be shown in each page.", Mandatory = false, ValueFromPipeline = true)]
        public int? Page { get; set; }

        /// <summary>
        /// <para type="description">Current page to show.</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Current page to show.", Mandatory = false, ValueFromPipeline = true)]
        public int? PerPage { get; set; }

        /// <summary>
        /// <para type="description">Allows to sort the result by priority:sort=name retrieves a list of elements ordered by their names.sort=-creation_date retrieves a list of elements ordered according to their creation date in descending order of priority.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Allows to sort the result by priority:sort=name retrieves a list of elements ordered by their names.sort=-creation_date retrieves a list of elements ordered according to their creation date in descending order of priority.", Mandatory = false, ValueFromPipeline = true)]
        public string Sort { get; set; }

        /// <summary>
        /// <para type="description">Allows to search one string in the response and return the elements that contain it. In order to specify the string use parameter q:    q=My server.</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Allows to search one string in the response and return the elements that contain it. In order to specify the string use parameter q:    q=My server", Mandatory = false, ValueFromPipeline = true)]
        public string Query { get; set; }

        /// <summary>
        /// <para type="description">Returns only the parameters requested: fields=id,name,description,hardware.ram</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "Returns only the parameters requested: fields=id,name,description,hardware.ram", Mandatory = false, ValueFromPipeline = true)]
        public string Fields { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var userApi = client.Users;

                if (string.IsNullOrEmpty(UserId))
                {
                    var policy = userApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(policy);
                }
                else
                {
                    var policy = userApi.Show(UserId);
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
    /// <para type="synopsis">This commandlet will create a new user.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOUser -Name [name] -Description -Email [email] -Password [password]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOUser")]
    [OutputType(typeof(UserResponse))]
    public class NewUser : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Name</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Name", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Description</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Description", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">User's email</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "User's email", Mandatory = false, ValueFromPipeline = true)]
        public string Email { get; set; }

        /// <summary>
        /// <para type="description">User's password. Pass must contain at least 8 characters using uppercase letters, numbers and other special symbols.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "User's password. Pass must contain at least 8 characters using uppercase letters, numbers and other special symbols.", Mandatory = true, ValueFromPipeline = true)]
        public string Password { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var userApi = client.Users;
                var result = userApi.Create(new POCO.Requests.Users.CreateUserRequest
                {
                    Name = Name,
                    Password = Password,
                    Description = Description,
                    Email = Email

                });

                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a user.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOUser -UserId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOUser")]
    [OutputType(typeof(UserResponse))]
    public class RemoveUser : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">User ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "User Id", Mandatory = true, ValueFromPipeline = true)]
        public string UserId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var usersApi = client.Users;
                var resp = usersApi.Delete(UserId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies user information.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOUser -UserId [UUID] -State [enum] -Description -Email [email] -Password [password]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOUser")]
    [OutputType(typeof(UserResponse))]
    public class SetUser : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters


        /// <summary>
        /// <para type="description">Allows to enable or disable users</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Allows to enable or disable users", Mandatory = false, ValueFromPipeline = true)]
        public UserState? State { get; set; }

        /// <summary>
        /// <para type="description">User ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "User Id", Mandatory = true, ValueFromPipeline = true)]
        public string UserId { get; set; }

        /// <summary>
        /// <para type="description">Description</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Description", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">User's email</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "User's email", Mandatory = false, ValueFromPipeline = true)]
        public string Email { get; set; }

        /// <summary>
        /// <para type="description">User's password. Pass must contain at least 8 characters using uppercase letters, numbers and other special symbols.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "User's password. Pass must contain at least 8 characters using uppercase letters, numbers and other special symbols.", Mandatory = false, ValueFromPipeline = true)]
        public string Password { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Constants.Configuration);
            var usersApi = client.Users;
            var user = client.Users.Show(UserId);
            var resp = client.Users.Update(new UpdateUserRequest
            {
                State = State ?? user.State,
                Email = Email,
                Description = Description,
                Password = Password

            }, UserId);

            WriteObject(resp);
        }
    }

    #endregion

    #region api operations
    /// <summary>
    /// <para type="synopsis">This commandlet returns information about API.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOUserAPI -UserId [UUID]</para>
    /// </example>

    [Cmdlet(VerbsCommon.Get, "OAOUserAPI")]
    [OutputType(typeof(ApiResponse))]
    public class GetUserAPI : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">User ID..</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "User Id", Mandatory = false, ValueFromPipeline = true)]
        public string UserId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var userApi = client.UserAPI;

                var info = userApi.ShowUserAPI(UserId);
                WriteObject(info);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies user information.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOUser -UserId [UUID] -Name [name] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOUserAPI")]
    [OutputType(typeof(UserResponse))]
    public class SetUserAPI : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters


        /// <summary>
        /// <para type="description">User ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "User Id", Mandatory = true, ValueFromPipeline = true)]
        public string UserId { get; set; }

        /// <summary>
        /// <para type="description">Description</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Description", Mandatory = false, ValueFromPipeline = true)]
        public bool Active { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Constants.Configuration);
            var usersApi = client.UserAPI;
            var resp = client.UserAPI.UpdateUserAPI(UserId, Active);

            WriteObject(resp);
        }
    }

    [Cmdlet(VerbsCommon.Get, "OAOUserAPIKey")]
    [OutputType(typeof(UserAPIKeyResponse))]
    public class GetUserAPIKey : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">User ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "User Id", Mandatory = true, ValueFromPipeline = true)]
        public string UserId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var userApi = client.UserAPI;
                var userKey = userApi.ShowUserAPIKey(UserId);
                WriteObject(userKey);


            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet changes the API key.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOUserAPIKey -UserId [UUID] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOUserAPIKey")]
    [OutputType(typeof(UserResponse))]
    public class SetUserAPIKey : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters


        /// <summary>
        /// <para type="description">User ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "User Id", Mandatory = true, ValueFromPipeline = true)]
        public string UserId { get; set; }


        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Constants.Configuration);
            var usersApi = client.UserAPI;
            var resp = usersApi.UpdateAPIKey(UserId);
            WriteObject(resp);
        }
    }


    /// <summary>
    /// <para type="synopsis">This commandlet shows IP's from which access to API is allowed.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOUserAllowedIps -UserId [UUID] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOUserAllowedIps")]
    [OutputType(typeof(String))]
    public class GetUserAllowedIps : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">User ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "User Id", Mandatory = true, ValueFromPipeline = true)]
        public string UserId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var userApi = client.UserAPI;

                var ips = userApi.GetUserIps(UserId);
                WriteObject(ips);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet allows a new IP.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOUserAllowedIps -UserId [UUID] -Ips [array] </para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOUserAllowedIps")]
    [OutputType(typeof(UserResponse))]
    public class NewUserAllowedIP : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">User ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "User Id", Mandatory = true, ValueFromPipeline = true)]
        public string UserId { get; set; }

        /// <summary>
        /// <para type="description">Array of new IPs from which access to API will be available.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Array of new IPs from which access to API will be available.", Mandatory = true, ValueFromPipeline = true)]
        public string[] Ips { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var userApi = client.UserAPI;
                var result = userApi.UpdateAPIIps(new List<string>(Ips), UserId);

                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet deletes an IP and forbides API access for it.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOUserAllowedIps -UserId [UUID] -IP [ip]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOUserAllowedIps")]
    [OutputType(typeof(UserResponse))]
    public class RemoveUserAllowedIP : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">User ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "User Id", Mandatory = true, ValueFromPipeline = true)]
        public string UserId { get; set; }

        /// <summary>
        /// <para type="description">Desired IP.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Desired IP", Mandatory = true, ValueFromPipeline = true)]
        public string IP { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var usersApi = client.UserAPI;
                var resp = usersApi.DeleteUserIp(UserId, IP);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #endregion
}
