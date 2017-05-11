using OneAndOne.Client;
using OneAndOne.POCO.Requests.Servers;
using OneAndOne.POCO.Response.PublicIPs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OneAndOne
{
    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of your public IPs, or one publicIp if the IpId is provided , if the ID parameter is provided all the optional paramters are ignored</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOPublicIP -IpId [UUID] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOPublicIP")]
    [OutputType(typeof(PublicIPsResponse))]
    public class GetPublicIP : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Ip ID. If this parameters is not passed, the commandlet will return a list of all public Ips.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Balancer Id", Mandatory = false, ValueFromPipeline = true)]
        public string IpId { get; set; }

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
                var ipApi = client.PublicIPs;

                if (string.IsNullOrEmpty(IpId))
                {
                    var ips = ipApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(ips);
                }
                else
                {
                    var ip = ipApi.Show(IpId);
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
    /// <para type="synopsis">This commandlet Creates a new public IP.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOPublicIP -ReverseDns [reversedns] -DatacenterId [UUID] -Type [type]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOPublicIP")]
    [OutputType(typeof(PublicIPsResponse))]
    public class NewPublicIp : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Reverse DNS name</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Reverse DNS name", Mandatory = true, ValueFromPipeline = true)]
        public string ReverseDns { get; set; }

        /// <summary>
        /// <para type="description">ID of the datacenter where the IP will be created (only for unassigned IPs)</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "ID of the datacenter where the IP will be created (only for unassigned IPs)", Mandatory = false, ValueFromPipeline = true)]
        public string DatacenterId { get; set; }

        /// <summary>
        /// <para type="description">Type of IP. Currently, only IPV4 is available.</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Datacenter", Mandatory = false, ValueFromPipeline = true)]
        public IPType Type { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var ipApi = client.PublicIPs;
                var result = ipApi.Create(new POCO.Requests.PublicIPs.CreatePublicIPRequest
                {
                    ReverseDns = ReverseDns ?? null,
                    Type = Type,
                    DatacenterId = DatacenterId ?? null
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
    /// <para type="synopsis">This commandlet removes a public ip.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOPublicIP -IpId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOPublicIP")]
    [OutputType(typeof(PublicIPsResponse))]
    public class RemovePublicIp : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">IP ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Ip Id", ValueFromPipeline = true)]
        public string IpId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var ipId = client.PublicIPs;
                var resp = ipId.Delete(IpId);
                WriteObject(resp);
            }

            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies a public ip.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOPublicIP -IpId [UUID] -ReverseDns [reversedns] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOPublicIP")]
    [OutputType(typeof(PublicIPsResponse))]
    public class SetPublicIp : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Reverse DNS name</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Reverse DNS name", Mandatory = true, ValueFromPipeline = true)]
        public string ReverseDns { get; set; }

        /// <summary>
        /// <para type="description">IP ID.</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Ip Id", ValueFromPipeline = true)]
        public string IpId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Helper.Configuration);
            var ipApi = client.PublicIPs;
            var resp = ipApi.Update(ReverseDns, IpId);
            WriteObject(resp);
        }
    }
}
