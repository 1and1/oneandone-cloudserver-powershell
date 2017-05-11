using OneAndOne.Client;
using OneAndOne.POCO.Response.PrivateNetworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OneAndOne
{
    #region Main Operations
    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of your private networks, or one private network if the NetworkId is provided , if the ID parameter is provided all the optional paramters are ignored</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOPrivateNetwork -NetworkId [UUID] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOPrivateNetwork")]
    [OutputType(typeof(PrivateNetworksResponse))]
    public class GetPrivateNetwork : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Network ID. If this parameters is not passed, the commandlet will return a list of all private networks.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Network Id", Mandatory = false, ValueFromPipeline = true)]
        public string NetworkId { get; set; }

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
                client = OneAndOneClient.Instance(Helper.Configuration);
                var pnApi = client.PrivateNetworks;

                if (string.IsNullOrEmpty(NetworkId))
                {
                    var lbs = pnApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(lbs);
                }
                else
                {
                    var lb = pnApi.Show(NetworkId);
                    WriteObject(lb);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will create a new Private network.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOPrivateNetwork -Name [name] -Description -DatacenterId [UUID] -NetworkAddress [networkaddress] -SubnetMask [subnetmask]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOPrivateNetwork")]
    [OutputType(typeof(PrivateNetworksResponse))]
    public class NewPrivateNetwork : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Name.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Name.", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Description</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Description", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Datacenter</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Datacenter", Mandatory = false, ValueFromPipeline = true)]
        public string DatacenterId { get; set; }

        /// <summary>
        /// <para type="description">Private network address (valid IP).</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Private network address (valid IP).", Mandatory = false, ValueFromPipeline = true)]
        public string NetworkAddress { get; set; }

        /// <summary>
        /// <para type="description">Subnet mask (valid subnet for the given IP)</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "Subnet mask (valid subnet for the given IP)", Mandatory = false, ValueFromPipeline = true)]
        public string SubnetMask { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var pnApi = client.PrivateNetworks;
                var result = pnApi.Create(new POCO.Requests.PrivateNetworks.CreatePrivateNetworkRequest
                {
                    Name = Name,
                    Description = Description ?? null,
                    DatacenterId = DatacenterId ?? null,
                    SubnetMask = SubnetMask ?? null,
                    NetworkAddress = NetworkAddress ?? null
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
    /// <para type="synopsis">This commandlet removes a PrivateNetowrk.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOPrivateNetwork -NetworkId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOPrivateNetwork")]
    [OutputType(typeof(PrivateNetworksResponse))]
    public class RemovePrivateNetwork : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Private Network ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Network Id", ValueFromPipeline = true)]
        public string NetworkId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var pnApi = client.PrivateNetworks;
                var resp = pnApi.Delete(NetworkId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies a PrivateNetwork.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOPrivateNetwork -Name [name] -Description -NetworkAddress [networkaddress] -SubnetMask [subnetmask] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOPrivateNetwork")]
    [OutputType(typeof(PrivateNetworksResponse))]
    public class SetPrivateNetwork : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description"> name.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Name.", Mandatory = false, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Description</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Description", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Type of the health check. At the moment, HTTP is not allowed</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Private network address (valid IP).", Mandatory = false, ValueFromPipeline = true)]
        public string NetworkAddress { get; set; }

        /// <summary>
        /// <para type="description">Description of the Private network</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Subnet mask (valid subnet for the given IP)", Mandatory = false, ValueFromPipeline = true)]
        public string SubnetMask { get; set; }

        /// <summary>
        /// <para type="description">Balancer ID.</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Network Id", Mandatory = true, ValueFromPipeline = true)]
        public string NetworkId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Helper.Configuration);
            var pnApi = client.PrivateNetworks;

            var resp = pnApi.Update(new POCO.Requests.PrivateNetworks.UpdatePrivateNetworkRequest
            {
                Name = Name ?? null,
                Description = Description ?? null,
                SubnetMask = SubnetMask ?? null,
                NetworkAddress = SubnetMask ?? null
            }, NetworkId);
            WriteObject(resp);
        }
    }

    #endregion

    #region Privatenetwork servers

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of the servers attached to a private network., or one server if the ServerId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOPrivateNetworkServer -Network [UUID] -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOPrivateNetworkServer")]
    [OutputType(typeof(PrivateNetworkServerResponse))]
    public class GetPrivateNetworkServer : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Network ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Network Id", ValueFromPipeline = true, Mandatory = true)]
        public string NetworkId { get; set; }

        /// <summary>
        /// <para type="description">Server ID. If this parameters is not passed, the commandlet will return a list of all servers attached.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Server Id", ValueFromPipeline = true, Mandatory = false)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var pnApi = client.PrivateNetworks;

                if (string.IsNullOrEmpty(ServerId))
                {
                    var pns = pnApi.GetPrivateNetworkServers(NetworkId);
                    WriteObject(pns);
                }
                else
                {
                    var pn = pnApi.ShowPrivateNetworkServer(NetworkId, ServerId);
                    WriteObject(pn);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will assigns servers/IPs to a PrivateNetwork.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOPrivateNetworkServer -NetworkId [UUID] -Servers -[array] </para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOPrivateNetworkServer")]
    [OutputType(typeof(PrivateNetworksResponse))]
    public class NewPrivateNetworkServer : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Network ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Network Id", ValueFromPipeline = true, Mandatory = true)]
        public string NetworkId { get; set; }

        /// <summary>
        /// <para type="description">List of servers to add to the PrivateNetwork.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "List of Ip ids to add to the PrivateNetwork", Mandatory = true, ValueFromPipeline = true)]
        public string[] Servers { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var pnApi = client.PrivateNetworks;
                var result = pnApi.CreatePrivateNetworkServers(new POCO.Requests.PrivateNetworks.AttachPrivateNetworkServersRequest
                {
                    Servers = new List<string>(Servers)
                }, NetworkId);

                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a server from the private network.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOPrivateNetworkServer -NetworkId [UUID] -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOPrivateNetworkServer")]
    [OutputType(typeof(PrivateNetworksResponse))]
    public class RemovePrivateNetworkServer : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Network ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Network Id", Mandatory = true, ValueFromPipeline = true)]
        public string NetworkId { get; set; }

        /// <summary>
        /// <para type="description">Server ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Server Id", ValueFromPipeline = true, Mandatory = false)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var pnApi = client.PrivateNetworks;
                var resp = pnApi.DeletePrivateNetworksServer(NetworkId, ServerId);
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
