using OneAndOne.Client;
using OneAndOne.POCO.Response.Vpn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OneAndOne
{
    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of your VPN, or one VPN if the VpnId is provided , if the ID parameter is provided all the optional paramters are ignored</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOVpn -VpnId [UUID] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOVpn")]
    [OutputType(typeof(VpnResponse))]
    public class GetVpn : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Vpn ID. If this parameters is not passed, the commandlet will return a list of all vpns.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "VPN Id", Mandatory = false, ValueFromPipeline = true)]
        public string VpnId { get; set; }

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
                var vpnApi = client.Vpn;

                if (string.IsNullOrEmpty(VpnId))
                {
                    var ips = vpnApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(ips);
                }
                else
                {
                    var ip = vpnApi.Show(VpnId);
                    WriteObject(ip);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will download your VPN configuration file</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOVpnConfiguration -VpnId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOVpnConfiguration")]
    [OutputType(typeof(VpnResponse))]
    public class GetVpnConfiguration : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Vpn ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "VPN Id", Mandatory = false, ValueFromPipeline = true)]
        public string VpnId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var vpnApi = client.Vpn;

                var vpn = vpnApi.ShowConfiguration(VpnId);
                WriteObject(vpn);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet Creates a new vpn.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOVpn -ReverseDns [reversedns] -DatacenterId [UUID] -Type [type]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOVpn")]
    [OutputType(typeof(VpnResponse))]
    public class NewVpn : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Name</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Name", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">ID of the datacenter</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "ID of the datacenter ", Mandatory = false, ValueFromPipeline = true)]
        public string DatacenterId { get; set; }

        /// <summary>
        /// <para type="description">Description</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Description", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var vpnApi = client.Vpn;
                var result = vpnApi.Create(new POCO.Requests.Vpn.CreateVpnRequest
                {
                    Name = Name,
                    Datacenterid = DatacenterId ?? null,
                    Description = Description ?? null
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
    /// <para type="synopsis">This commandlet removes a vpn.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOVpn -VpnId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOVpn")]
    [OutputType(typeof(VpnResponse))]
    public class RemoveVpn : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Vpn ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Vpn Id", ValueFromPipeline = true)]
        public string VpnId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var vpn = client.Vpn;
                var resp = vpn.Delete(VpnId);
                WriteObject(resp);
            }

            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies a vpn.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOVpn -VpnId [UUID] -Name [name] -Description [description] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOVpn")]
    [OutputType(typeof(VpnResponse))]
    public class SetVpn : Cmdlet
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
        /// <para type="description">Vpn ID.</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Vpn Id", ValueFromPipeline = true)]
        public string VpnId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Helper.Configuration);
            var vpnApi = client.Vpn;
            var resp = vpnApi.Update(new POCO.Requests.Vpn.UpdateVpnRequest
            {
                Name = Name ?? null,
                Description = Description ?? null
            }, VpnId);
            WriteObject(resp);
        }
    }
}
