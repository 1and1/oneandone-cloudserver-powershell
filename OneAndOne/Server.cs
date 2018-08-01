using OneAndOne.Client;
using OneAndOne.Client.Endpoints.PublicIPs;
using OneAndOne.Client.Endpoints.ServerAppliances;
using OneAndOne.POCO;
using OneAndOne.POCO.Requests.Servers;
using OneAndOne.POCO.Response.PublicIPs;
using OneAndOne.POCO.Response.ServerAppliances;
using OneAndOne.POCO.Response.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OneAndOne
{
    /// <summary>
    /// <para type="synopsis">This commandlet will get one or a list of servers,Or one server if the ID parameter is provided,in that case all the optional paramters are ignored</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServer -ServerId [UUID] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServer")]
    [OutputType(typeof(ServerResponse))]
    public class GetServer : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. If this parameters is not passed, the commandlet will return a list of all servers.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", ValueFromPipeline = true)]
        public string ServerId { get; set; }

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
                var serverApi = client.Servers;

                if (string.IsNullOrEmpty(ServerId))
                {
                    var servers = serverApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(servers);
                }
                else
                {
                    var servers = serverApi.Show(ServerId);
                    WriteObject(servers);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will get one or a list of available hardware flavors.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOHardwareFlavor -FlavorId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOHardwareFlavor")]
    [OutputType(typeof(AvailableHardwareFlavour))]
    public class GetHardwareFlavors : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. If this parameters is not passed, the commandlet will return a list of all flavors.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", ValueFromPipeline = true)]
        public string FlavorId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;

                if (string.IsNullOrEmpty(FlavorId))
                {
                    var flavors = serverApi.GetAvailableFixedServers();
                    WriteObject(flavors);
                }
                else
                {
                    var falvor = serverApi.GetFlavorInformation(FlavorId);
                    WriteObject(falvor);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }


    /// <summary>
    /// <para type="synopsis">This commandlet will get one or a list of available baremetal hardware models.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOBaremetalModels -BaremetalModelId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOBaremetalModels")]
    [OutputType(typeof(BaremetalResponse))]
    public class GetBaremetalModel : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">BaremetalModel ID. If this parameters is not passed, the commandlet will return a list of all models.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "BaremetalModelId", ValueFromPipeline = true)]
        public string BaremetalModelId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;

                if (string.IsNullOrEmpty(BaremetalModelId))
                {
                    var models = serverApi.GetBaremetal();
                    WriteObject(models);
                }
                else
                {
                    var model = serverApi.ShowBaremetal(BaremetalModelId);
                    WriteObject(model);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }


    /// <summary>
    /// <para type="synopsis">This commandlet will create a server with custom hardware settings</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOServer -Name [name] -Vcore [Vcore] -CoresPerProcessor [CoresPerProcessor] -Ram [ram] -Hdds [HddRequest[]] -ApplianceId [UUID] -RegionId [UUID] -Password [password] -PowerOn [poweron] -IpId [UUID] -LoadrBalancerId [UUID] -MonitoringPolicyId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOServer")]
    [OutputType(typeof(CreateServerResponse))]
    public class NewServer : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">The hostname of the server. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The hostname of the server.", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">The hostname of the server. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The Description of the server.", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Total amount of processors minimum: '1',maximum: '16',multipleOf: '1'.</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Total amount of processors minimum: '1',maximum: '16',multipleOf: '1'.", Mandatory = true, ValueFromPipeline = true)]
        public int Vcore { get; set; }

        /// <summary>
        /// <para type="description">Number of cores per processor minimum: '1',maximum: '16',multipleOf: '1'.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Number of cores per processor minimum: '1',maximum: '16',multipleOf: '1'", Mandatory = true, ValueFromPipeline = true)]
        public int CoresPerProcessor { get; set; }

        /// <summary>
        /// <para type="description">RAM memory size minimum: "1",maximum: "128",multipleOf: "0.5",.</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "RAM memory size minimum: '1',maximum: '128',multipleOf: '0.5'.", Mandatory = true, ValueFromPipeline = true)]
        public decimal Ram { get; set; }


        /// <summary>
        /// <para type="description">Array of hard disk drives</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "Array of hard disk drives", Mandatory = true, ValueFromPipeline = true)]
        public HddRequest[] Hdds { get; set; }

        /// <summary>
        /// <para type="description">Image will be installed on server.</para>
        /// </summary>
        [Parameter(Position = 6, HelpMessage = "Image will be installed on server.", Mandatory = true, ValueFromPipeline = true)]
        public string ApplianceId { get; set; }

        /// <summary>
        /// <para type="description">Password of the server.</para>
        /// </summary>
        [Parameter(Position = 7, HelpMessage = "Password of the server. Password must contain more than 8 characters using uppercase letters, numbers and other special symbols. minLength: 8,maxLength: 64.", Mandatory = false, ValueFromPipeline = true)]
        public string Password { get; set; }

        /// <summary>
        /// <para type="description">Region id</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "ID of the region where the server will be created", Mandatory = false, ValueFromPipeline = true)]
        public string RegionId { get; set; }

        /// <summary>
        /// <para type="description">Power on server after creation</para>
        /// </summary>
        [Parameter(Position = 9, HelpMessage = "Power on server after creation", Mandatory = false, ValueFromPipeline = true)]
        public bool PowerOn { get; set; }

        /// <summary>
        /// <para type="description">Firewall policy's ID.</para>
        /// </summary>
        [Parameter(Position = 10, HelpMessage = "Firewall policy's ID to attach", Mandatory = false, ValueFromPipeline = true)]
        public string FirewallPolicyId { get; set; }


        /// <summary>
        /// <para type="description">IP's ID.</para>
        /// </summary>
        [Parameter(Position = 11, HelpMessage = "IP's ID", Mandatory = false, ValueFromPipeline = true)]
        public string IpId { get; set; }

        /// <summary>
        /// <para type="description">Load balancer's ID</para>
        /// </summary>
        [Parameter(Position = 12, HelpMessage = "Load balancer's ID ", Mandatory = false, ValueFromPipeline = true)]
        public string LoadrBalancerId { get; set; }

        /// <summary>
        /// <para type="description">Monitoring policy's ID</para>
        /// </summary>
        [Parameter(Position = 13, HelpMessage = "Monitoring policy's ID ", Mandatory = false, ValueFromPipeline = true)]
        public string MonitoringPolicyId { get; set; }



        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;
                ServerAppliancesResponse serverAppliance = null;
                PublicIPsResponse publicIp = null;
                if (!string.IsNullOrEmpty(ApplianceId))
                {
                    serverAppliance = client.ServerAppliances.Show(ApplianceId);
                }
                if (!string.IsNullOrEmpty(IpId))
                {
                    publicIp = client.PublicIPs.Show(IpId);
                }

                var hardware = new POCO.Requests.Servers.HardwareRequest
                {
                    CoresPerProcessor = CoresPerProcessor,
                    Ram = Ram,
                    Vcore = Vcore,
                    Hdds = new List<HddRequest>(Hdds),

                };

                var result = client.Servers.Create(new POCO.Requests.Servers.CreateServerRequest()
                {
                    ApplianceId = serverAppliance != null ? serverAppliance.Id : null,
                    Name = Name,
                    Description = Description ?? null,
                    Hardware = hardware,
                    FirewallPolicyId = FirewallPolicyId ?? null,
                    LoadrBalancerId = LoadrBalancerId ?? null,
                    MonitoringPolicyId = MonitoringPolicyId ?? null,
                    RegionId = RegionId ?? null,
                    PowerOn = PowerOn,
                    Password = Password ?? null,
                    IpId = publicIp != null ? publicIp.Id : null,

                });

                WriteVerbose("Creating the server...");

                WriteObject(result);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }


    /// <summary>
    /// <para type="synopsis">This commandlet will create a server with a flavored hardware profile.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOServerWithFlavor -Name [name] -FixedInstanceSizeId [UUID] -ApplianceId [UUID] -RegionId [UUID] -Password [password] -PowerOn [poweron] -IpId [UUID] -LoadrBalancerId [UUID] -MonitoringPolicyId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOServerWithFlavor")]
    [OutputType(typeof(CreateServerResponse))]
    public class NewServerWithFlavor : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">The hostname of the server. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The hostname of the server.", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">The hostname of the server. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The Description of the server.", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }


        /// <summary>
        /// <para type="description">Size of the ID desired for the server</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "The id of the desired hardware flavour", Mandatory = true, ValueFromPipeline = true)]
        public string FixedInstanceSizeId { get; set; }

        /// <summary>
        /// <para type="description">Image will be installed on server.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Image will be installed on server.", Mandatory = true, ValueFromPipeline = true)]
        public string ApplianceId { get; set; }

        /// <summary>
        /// <para type="description">Password of the server.</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Password of the server. Password must contain more than 8 characters using uppercase letters, numbers and other special symbols. minLength: 8,maxLength: 64.", Mandatory = false, ValueFromPipeline = true)]
        public string Password { get; set; }

        /// <summary>
        /// <para type="description">Region id</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "ID of the region where the server will be created", Mandatory = false, ValueFromPipeline = true)]
        public string RegionId { get; set; }

        /// <summary>
        /// <para type="description">Power on server after creation</para>
        /// </summary>
        [Parameter(Position = 6, HelpMessage = "Power on server after creation", Mandatory = false, ValueFromPipeline = true)]
        public bool PowerOn { get; set; }

        /// <summary>
        /// <para type="description">Firewall policy's ID.</para>
        /// </summary>
        [Parameter(Position = 7, HelpMessage = "Firewall policy's ID to attach", Mandatory = false, ValueFromPipeline = true)]
        public string FirewallPolicyId { get; set; }


        /// <summary>
        /// <para type="description">IP's ID.</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "IP's ID", Mandatory = false, ValueFromPipeline = true)]
        public string IpId { get; set; }

        /// <summary>
        /// <para type="description">Load balancer's ID</para>
        /// </summary>
        [Parameter(Position = 9, HelpMessage = "Load balancer's ID ", Mandatory = false, ValueFromPipeline = true)]
        public string LoadrBalancerId { get; set; }

        /// <summary>
        /// <para type="description">Monitoring policy's ID</para>
        /// </summary>
        [Parameter(Position = 10, HelpMessage = "Monitoring policy's ID ", Mandatory = false, ValueFromPipeline = true)]
        public string MonitoringPolicyId { get; set; }



        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;
                ServerAppliancesResponse serverAppliance = null;
                PublicIPsResponse publicIp = null;
                AvailableHardwareFlavour availabeFixedImage = null;
                if (!string.IsNullOrEmpty(ApplianceId))
                {
                    serverAppliance = client.ServerAppliances.Show(ApplianceId);
                }
                if (!string.IsNullOrEmpty(IpId))
                {
                    publicIp = client.PublicIPs.Show(IpId);
                }

                if (!string.IsNullOrEmpty(FixedInstanceSizeId))
                {
                    availabeFixedImage = client.Servers.GetFlavorInformation(FixedInstanceSizeId);
                }

                var hardware = new POCO.Requests.Servers.HardwareFlavorRequest
                {
                    FixedInstanceSizeId = availabeFixedImage.Id

                };

                var result = client.Servers.CreateServerFromFlavor(new POCO.Requests.Servers.CreateServerWithFlavorRequest()
                {
                    ApplianceId = serverAppliance != null ? serverAppliance.Id : null,
                    Name = Name,
                    Description = Description ?? null,
                    Hardware = hardware,
                    FirewallPolicyId = FirewallPolicyId ?? null,
                    LoadrBalancerId = LoadrBalancerId ?? null,
                    MonitoringPolicyId = MonitoringPolicyId ?? null,
                    RegionId = RegionId ?? null,
                    PowerOn = PowerOn,
                    Password = Password ?? null,
                    IpId = publicIp != null ? publicIp.Id : null
                });

                WriteVerbose("Creating the server...");

                WriteObject(result);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet will create a baremetal server with a flavored baremetal hardware profile.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOBaremetalServer -Name [name] -BaremetalModelId [UUID] -ApplianceId [UUID] -RegionId [UUID] -Password [password] -PowerOn [poweron] -IpId [UUID] -LoadrBalancerId [UUID] -MonitoringPolicyId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOBaremetalServer")]
    [OutputType(typeof(CreateServerResponse))]
    public class NewBaremetalServer : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">The hostname of the server. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The hostname of the server.", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">The hostname of the server. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The Description of the server.", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }


        /// <summary>
        /// <para type="description">Size of the ID desired for the server</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "The id of the desired baremetal hardware model", Mandatory = true, ValueFromPipeline = true)]
        public string BaremetalModelId { get; set; }

        /// <summary>
        /// <para type="description">Image will be installed on server.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Image will be installed on server.", Mandatory = true, ValueFromPipeline = true)]
        public string ApplianceId { get; set; }

        /// <summary>
        /// <para type="description">Password of the server.</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Password of the server. Password must contain more than 8 characters using uppercase letters, numbers and other special symbols. minLength: 8,maxLength: 64.", Mandatory = false, ValueFromPipeline = true)]
        public string Password { get; set; }

        /// <summary>
        /// <para type="description">Region id</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "ID of the region where the server will be created", Mandatory = false, ValueFromPipeline = true)]
        public string RegionId { get; set; }

        /// <summary>
        /// <para type="description">Power on server after creation</para>
        /// </summary>
        [Parameter(Position = 6, HelpMessage = "Power on server after creation", Mandatory = false, ValueFromPipeline = true)]
        public bool PowerOn { get; set; }

        /// <summary>
        /// <para type="description">Firewall policy's ID.</para>
        /// </summary>
        [Parameter(Position = 7, HelpMessage = "Firewall policy's ID to attach", Mandatory = false, ValueFromPipeline = true)]
        public string FirewallPolicyId { get; set; }


        /// <summary>
        /// <para type="description">IP's ID.</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "IP's ID", Mandatory = false, ValueFromPipeline = true)]
        public string IpId { get; set; }

        /// <summary>
        /// <para type="description">Load balancer's ID</para>
        /// </summary>
        [Parameter(Position = 9, HelpMessage = "Load balancer's ID ", Mandatory = false, ValueFromPipeline = true)]
        public string LoadrBalancerId { get; set; }

        /// <summary>
        /// <para type="description">Monitoring policy's ID</para>
        /// </summary>
        [Parameter(Position = 10, HelpMessage = "Monitoring policy's ID ", Mandatory = false, ValueFromPipeline = true)]
        public string MonitoringPolicyId { get; set; }



        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;
                ServerAppliancesResponse serverAppliance = null;
                PublicIPsResponse publicIp = null;
                BaremetalResponse baremetalmodel = null;
                if (!string.IsNullOrEmpty(ApplianceId))
                {
                    serverAppliance = client.ServerAppliances.Show(ApplianceId);
                }
                if (!string.IsNullOrEmpty(IpId))
                {
                    publicIp = client.PublicIPs.Show(IpId);
                }

                if (!string.IsNullOrEmpty(BaremetalModelId))
                {
                    baremetalmodel = client.Servers.ShowBaremetal(BaremetalModelId);
                }

                var hardware = new POCO.Requests.Servers.HardwareRequest
                {
                    BaremetalModelId = baremetalmodel.Id

                };

                var result = client.Servers.Create(new POCO.Requests.Servers.CreateServerRequest()
                {
                    ApplianceId = serverAppliance != null ? serverAppliance.Id : null,
                    Name = Name,
                    Description = Description ?? null,
                    Hardware = hardware,
                    FirewallPolicyId = FirewallPolicyId ?? null,
                    LoadrBalancerId = LoadrBalancerId ?? null,
                    MonitoringPolicyId = MonitoringPolicyId ?? null,
                    RegionId = RegionId ?? null,
                    PowerOn = PowerOn,
                    Password = Password ?? null,
                    IpId = publicIp != null ? publicIp.Id : null,
                    ServerType = ServerType.baremetal
                });

                WriteVerbose("Creating the server...");

                WriteObject(result);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }


    /// <summary>
    /// <para type="synopsis">This commandlet will remove the specified server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOServer -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOServer")]
    [OutputType(typeof(DeleteServerResponse))]
    public class RemoveServer : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Set true for keeping server IPs after deleting a server (false by default).</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Set true for keeping server IPs after deleting a server (false by default).", Mandatory = false, ValueFromPipeline = true)]
        public bool KeepIps { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;
                var resp = serverApi.Delete(this.ServerId, this.KeepIps);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will update server properties.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOServer -ServerId [UUID] -Name [name]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOServer")]
    [OutputType(typeof(UpdateServerResponse))]
    public class SetServer : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">The hostname of the server.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The anme of the server.", Mandatory = false, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">The hostname of the server.</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "The Description of the server.", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }


        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Helper.Configuration);
            var serverApi = client.Servers;

            var resp = serverApi.Update(new UpdateServerRequest { Name = Name ?? null, Description = Description ?? null }, ServerId);
            WriteObject(resp);
        }
    }


    #region Server Hardware Operations


    /// <summary>
    /// <para type="synopsis">This commandlet will get the hardware profile of a server</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerHardware -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServerHardware")]
    [OutputType(typeof(HardwareBase))]
    public class GetServerHardware : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var hardwareApi = client.ServersHardware;

                if (string.IsNullOrEmpty(ServerId))
                {
                    throw new ArgumentNullException("ServerId");
                }
                else
                {
                    var hardware = hardwareApi.Show(ServerId);
                    WriteObject(hardware);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will update hardware properties of a server.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOServerHardware -ServerId [UUID] -Vcore [vcore] -CoresPerProcessor [CoresPerProcessor] -Ram [ram] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOServerHardware")]
    [OutputType(typeof(ServerResponse))]
    public class SetServerHardware : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Total amount of virtual cores. It is only possible to decrease the number of vCore if the server is powered off. Some operating systems don't allow to increase the CPU if they are powered </para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Total amount of virtual cores. It is only possible to decrease the number of vCore if the server is powered off. Some operating systems don't allow to increase the CPU if they are powered ", ValueFromPipeline = true)]
        public int? Vcore { get; set; }

        /// <summary>
        /// <para type="description">Number of cores per processor. It is only possible to modify the cores per processor if the server is powered off. The valid values are 1, a even number and the total amount of cores. </para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Number of cores per processor. It is only possible to modify the cores per processor if the server is powered off. The valid values are 1, a even number and the total amount of cores. ", ValueFromPipeline = true)]
        public int? CoresPerProcessor { get; set; }

        /// <summary>
        /// <para type="description">RAM memory size. It is only possible to decrease the RAM if the server is powered off. Some operating systems don't allow to increase the RAM if they are powered on.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "RAM memory size. It is only possible to decrease the RAM if the server is powered off. Some operating systems don't allow to increase the RAM if they are powered on.", ValueFromPipeline = true)]
        public int? Ram { get; set; }


        #endregion

        protected override void BeginProcessing()
        {

            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.ServersHardware;

                var resp = serverApi.Update(new UpdateHardwareRequest { CoresPerProcessor = CoresPerProcessor, Ram = Ram, Vcore = Vcore }, ServerId);
                WriteObject(resp);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }


    #endregion

    #region HDD operations

    /// <summary>
    /// <para type="synopsis">This commandlet will get the list of Hdds of a server</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerHdds -ServerId [UUID] -HddId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServerHdds")]
    [OutputType(typeof(Hdd))]
    public class GetServerHdd : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Unique hard disk's identifier.If this parameters is not passed, the commandlet will return a list of all hdds associated with one server.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Hdd Id", ValueFromPipeline = true)]
        public string HddId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var hddApi = client.ServerHdds;

                if (string.IsNullOrEmpty(HddId))
                {
                    var hdds = hddApi.Get(ServerId);
                    WriteObject(hdds);
                }
                else
                {
                    var hdds = hddApi.Show(ServerId, HddId);
                    WriteObject(hdds);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will Add a list of hdds to a server provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOServerHdds -ServerId [UUID] -Hdds [hdds]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOServerHdds")]
    [OutputType(typeof(ServerResponse))]
    public class NewServerHdds : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Hdd Array </para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Hdd Array", Mandatory = true, ValueFromPipeline = true)]
        public HddRequest[] Hdds { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverHddsApi = client.ServerHdds;
                var resp = serverHddsApi.Create(new AddHddRequest { Hdds = new List<HddRequest>(Hdds) }, ServerId);

                WriteVerbose("Adding hdds to the server...");

                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet will update server hdds.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOServerHdds -ServerId [UUID] -HddId [UUID] -Size [size]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOServerHdds")]
    [OutputType(typeof(ServerResponse))]
    public class SetServerHdds : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">HddId ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "HddId", Mandatory = true, ValueFromPipeline = true)]
        public string HddId { get; set; }

        /// <summary>
        /// <para type="description">Size of the disk</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Size of the disk", Mandatory = true, ValueFromPipeline = true)]
        public int Size { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.ServerHdds;

                var resp = serverApi.Update(new UpdateHddRequest { Size = Size }, ServerId, HddId);
                WriteObject(resp);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will remove the specified hdd from its server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOServerHdd -ServerId [UUID] -HddId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOServerHdd")]
    [OutputType(typeof(ServerResponse))]
    public class RemoveServerHdd : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Hdd ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Hdd Id", Mandatory = true, ValueFromPipeline = true)]
        public string HddId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.ServerHdds;
                var resp = serverApi.Delete(ServerId, HddId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #endregion

    #region ServerImages

    /// <summary>
    /// <para type="synopsis">This commandlet returns information about a server's image.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerImage -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServerImage")]
    [OutputType(typeof(Hdd))]
    public class GetServerImage : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var imageApi = client.ServerImage;

                if (string.IsNullOrEmpty(ServerId))
                {
                    throw new ArgumentNullException("ServerId");
                }
                else
                {
                    var image = imageApi.Show(ServerId);
                    WriteObject(image);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet reinstalls a new image into a server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOServerImages -ServerId [UUID] -ImageId [UUID] -Password [password]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOServerImages")]
    [OutputType(typeof(ServerResponse))]
    public class SetServerImage : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">ImageId. Image ID</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "ImageId", Mandatory = true, ValueFromPipeline = true)]
        public string ImageId { get; set; }

        /// <summary>
        /// <para type="description">Password of the server. Password must contain more than 8 characters using uppercase letters, numbers and other special symbols.</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Password of the server. Password must contain more than 8 characters using uppercase letters, numbers and other special symbols.", Mandatory = false, ValueFromPipeline = true)]
        public string Password { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var imageApi = client.ServerImage;

                var resp = imageApi.Update(new UpdateServerImageRequest { Id = ImageId, Password = Password ?? null }, ServerId);
                WriteObject(resp);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #endregion

    #region Server IP

    /// <summary>
    /// <para type="synopsis">This commandlet will get the list of IPS of a server</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerIps -ServerId [UUID] -IpId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServerIps")]
    [OutputType(typeof(IpAddress))]
    public class GetServerIPs : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Unique IP's identifier.If this parameters is not passed, the commandlet will return a list of all IPS associated with one server.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Ip Id", ValueFromPipeline = true)]
        public string IpId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var ipApi = client.ServerIps;

                if (string.IsNullOrEmpty(IpId))
                {
                    var ips = ipApi.Get(ServerId);
                    WriteObject(ips);
                }
                else
                {
                    var ip = ipApi.Show(ServerId, IpId);
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
    /// <para type="synopsis">This commandlet Adds a new IP to the server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOServerIps -ServerId [UUID] </para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOServerIps")]
    [OutputType(typeof(ServerResponse))]
    public class NewServerIps : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverIpApi = client.ServerIps;
                var resp = serverIpApi.Create(new CreateServerIPRequest { Type = IPType.Ipv4 }, ServerId);

                WriteVerbose("Adding ips to the server...");

                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet release the specified ip from its server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOServerIP -ServerId [UUID] -IpId [UUID] -KeepIp [bool]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOServerIP")]
    [OutputType(typeof(ServerResponse))]
    public class RemoveServerIP : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">IP ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "IP Id", Mandatory = true, ValueFromPipeline = true)]
        public string IpId { get; set; }

        /// <summary>
        /// <para type="description">Set true for releasing the IP without removing it</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Set true for releasing the IP without removing it", Mandatory = false, ValueFromPipeline = true)]
        public bool KeepIp { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var ipApi = client.ServerIps;
                var resp = ipApi.Delete(ServerId, IpId, KeepIp);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #region ip firewalls

    /// <summary>
    /// <para type="synopsis">This commandlet Lists firewall policies assigned to the IP</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerIpsFirewalls -ServerId [UUID] -IpId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServerIpsFirewalls")]
    [OutputType(typeof(FirewallPolicyResponse))]
    public class GetServerIPsFirewalls : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Unique IP's identifier.If this parameters is not passed.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Ip Id", ValueFromPipeline = true)]
        public string IpId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var ipApi = client.ServerIps;

                if (string.IsNullOrEmpty(IpId))
                {
                    throw new ArgumentNullException("ServerId");
                }
                else
                {
                    var firewalls = ipApi.GetFirewallPolicies(ServerId, IpId);
                    WriteObject(firewalls);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet Adds a new firewall policy to the IP</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOServerIpsFirewalls -ServerId [UUID] -IpId [UUID] -FirewallId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOServerIpsFirewalls")]
    [OutputType(typeof(ServerResponse))]
    public class SetServerIpsFirewalls : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">IpId. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "IpId", Mandatory = true, ValueFromPipeline = true)]
        public string IpId { get; set; }

        /// <summary>
        /// <para type="description">ID of the firewall policy that will be assigned</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "ID of the firewall policy that will be assigned", Mandatory = true, ValueFromPipeline = true)]
        public string FirewallId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var ipApi = client.ServerIps;

                var resp = ipApi.UpdateFirewallPolicy(ServerId, IpId, FirewallId);
                WriteObject(resp);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #endregion

    #region ip loadbalancer

    /// <summary>
    /// <para type="synopsis">This commandlet lists all load balancers assigned to the IP</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerIpsLoadBalancers -ServerId [UUID] -IpId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServerIpsLoadBalancers")]
    [OutputType(typeof(LoadBalancers))]
    public class GetServerIPsLoadBalancers : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Unique IP's identifier..</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Ip Id", ValueFromPipeline = true)]
        public string IpId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var ipApi = client.ServerIps;

                var loadBalancers = ipApi.GetLoadBalancer(ServerId, IpId);
                WriteObject(loadBalancers);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }


    /// <summary>
    /// <para type="synopsis">This commandlet Adds a new load balancer to the IP.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOServerIpsLoadBalancers -ServerId [UUID] -IpId [UUID] -LoadBalancerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOServerIpsLoadBalancers")]
    [OutputType(typeof(ServerResponse))]
    public class NewServerIpsLoadBalancers : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Ip ID. </para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Ip Id", Mandatory = true, ValueFromPipeline = true)]
        public string IpId { get; set; }

        /// <summary>
        /// <para type="description">LoadBalancerId . </para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "LoadBalancerId ", Mandatory = true, ValueFromPipeline = true)]
        public string LoadBalancerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverIpApi = client.ServerIps;
                var resp = serverIpApi.CreateLoadBalancer(ServerId, IpId, LoadBalancerId);

                WriteObject(resp);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

        /// <summary>
        /// <para type="synopsis">This commandlet Removes load balancer from the IP.</para>
        /// </summary>
        /// <example>
        /// <para type="description">Remove-OAOServerIPLoadBalancer -ServerId [UUID] -IpId [UUID] -LoadBalancerId [UUID]</para>
        /// </example>
        [Cmdlet(VerbsCommon.Remove, "OAOServerIPLoadBalancer")]
        [OutputType(typeof(ServerResponse))]
        public class RemoveServerIPLoadBalancer : Cmdlet
        {

            private static OneAndOneClient client;

            #region Parameters

            /// <summary>
            /// <para type="description">Server ID. Mandatory parameter.</para>
            /// </summary>
            [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
            public string ServerId { get; set; }

            /// <summary>
            /// <para type="description">IP ID. Mandatory parameter.</para>
            /// </summary>
            [Parameter(Position = 1, HelpMessage = "IP Id", Mandatory = true, ValueFromPipeline = true)]
            public string IpId { get; set; }

            /// <summary>
            /// <para type="description">LoadBalancerId.  Mandatory parameter.</para>
            /// </summary>
            [Parameter(Position = 2, HelpMessage = "LoadBalancer Id ", Mandatory = true, ValueFromPipeline = true)]
            public string LoadBalancerId { get; set; }

            #endregion

            protected override void BeginProcessing()
            {
                try
                {
                    client = OneAndOneClient.Instance(Helper.Configuration);
                    var ipApi = client.ServerIps;
                    var resp = ipApi.DeleteLoadBalancer(ServerId, IpId, LoadBalancerId);
                    WriteObject(resp);

                }
                catch (Exception ex)
                {
                    WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
                }
            }
        }

    }

    #endregion

    #endregion

    #region Server Status and Operations

    /// <summary>
    /// <para type="synopsis">This commandlet returns information about a server's status.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerStatus -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServerStatus")]
    [OutputType(typeof(Status))]
    public class GetServerStatus : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;

                var loadBalancers = serverApi.GetStatus(ServerId);
                WriteObject(loadBalancers);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }


    /// <summary>
    /// <para type="synopsis">This commandlet will Change server status POWERNON,POWEROFF,REBOOT</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOServerStatus -ServerId [UUID] -Action [action] -Method [Method]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOServerStatus")]
    [OutputType(typeof(ServerResponse))]
    public class SetServerStatus : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Action  POWER_ON = 0,POWER_OFF = 1,REBOOT = 2.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Action  POWER_ON = 0,POWER_OFF = 1,REBOOT = 2", Mandatory = true, ValueFromPipeline = true)]
        public ServerAction Action { get; set; }

        /// <summary>
        /// <para type="description">Method can be either SOFTWARE = 0,HARDWARE = 1</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Method can be either SOFTWARE = 0,HARDWARE = 1", Mandatory = true, ValueFromPipeline = true)]
        public ServerActionMethod Method { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;

                var resp = serverApi.UpdateStatus(new UpdateStatusRequest { Action = Action, Method = Method }, ServerId);
                WriteObject(resp);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
    #endregion

    #region dvd 

    /// <summary>
    /// <para type="synopsis">This commandlet returns information about the DVD loaded into the virtual DVD unit of a server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerDvd -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServerDvd")]
    [OutputType(typeof(POCO.Response.Servers.Dvd))]
    public class GetServerDvd : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var hardwareApi = client.ServersHardware;

                var dvd = hardwareApi.ShowDVD(ServerId);
                WriteObject(dvd);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet Loads a DVD into the virtual DVD unit of a server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOServerDvd -ServerId [UUID] -DvdId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOServerDvd")]
    [OutputType(typeof(ServerResponse))]
    public class SetServerDvd : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">DvdId. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "DvdId", Mandatory = true, ValueFromPipeline = true)]
        public string DvdId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var hardwareApi = client.ServersHardware;

                var resp = hardwareApi.UpdateDVD(ServerId, DvdId);
                WriteObject(resp);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet Unloads a DVD from the virtual DVD unit of a server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOServerDVD -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOServerDVD")]
    [OutputType(typeof(ServerResponse))]
    public class RemoveServerDvd : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var hardwareApi = client.ServersHardware;
                var resp = hardwareApi.DeleteDVD(ServerId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #endregion

    #region Server Private Network

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of the server's private networks.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerPrivateNetwork -ServerId [UUID] -PrivateNetworkId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServerPrivateNetwork")]
    [OutputType(typeof(PrivateNetworks))]
    public class GetServerPrivateNetwork : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">PrivateNetwork ID.If this parameters is not passed, the commandlet will return a list of all private networks. </para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "PrivateNetwork Id", Mandatory = true, ValueFromPipeline = true)]
        public string PrivateNetworkId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;

                if (string.IsNullOrEmpty(PrivateNetworkId))
                {
                    var pn = serverApi.GetPrivateNetworks(ServerId);
                    WriteObject(pn);
                }
                else
                {

                    var pn = serverApi.ShowPrivateNetworks(ServerId, PrivateNetworkId);
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
    /// <para type="synopsis">This commandlet Assigns a private network to the server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOServerPrivateNetwork -ServerId [UUID] -PrivateNetworkId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOServerPrivateNetwork")]
    [OutputType(typeof(ServerResponse))]
    public class NewServerPrivateNetwork : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">PrivateNetwork ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "PrivateNetwork Id", Mandatory = true, ValueFromPipeline = true)]
        public string PrivateNetworkId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;
                var resp = serverApi.CreatePrivateNetwork(ServerId, PrivateNetworkId);

                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet unassigns a private network from the server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOServerPrivateNetwork -ServerId [UUID] -PrivateNetworkId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOServerPrivateNetwork")]
    [OutputType(typeof(ServerResponse))]
    public class RemoveServerPrivateNetwork : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">PrivateNetwork ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "PrivateNetwork Id", Mandatory = true, ValueFromPipeline = true)]
        public string PrivateNetworkId { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;
                var resp = serverApi.DeletePrivateNetwork(ServerId, PrivateNetworkId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
    #endregion

    #region snapshots

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of the server's snapshots.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerSnapshot -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOServerSnapshot")]
    [OutputType(typeof(Snapshots))]
    public class GetServerSnapshot : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;
                var snapshot = serverApi.GetSnapshots(ServerId);
                WriteObject(snapshot);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet creates a new snapshot of the server..</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOServerSnapshot -ServerId [UUID] </para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOServerSnapshot")]
    [OutputType(typeof(ServerResponse))]
    public class NewServerSnapshot : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;
                var resp = serverApi.CreateSnapshot(ServerId);

                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a snapshot.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOServerSnapshot -ServerId [UUID] -SnapshotId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOServerSnapshot")]
    [OutputType(typeof(ServerResponse))]
    public class RemoveServerSnapshot : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Snapshot ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Snapshot Id", Mandatory = true, ValueFromPipeline = true)]
        public string SnapshotId { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;
                var resp = serverApi.DeleteSnapshot(ServerId, SnapshotId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet restores a snapshot into the server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOServerSnapshot -ServerId [UUID] -SnapshotId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOServerSnapshot")]
    [OutputType(typeof(ServerResponse))]
    public class SetServerSnapshot : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Snapshot ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Snapshot Id", Mandatory = true, ValueFromPipeline = true)]
        public string SnapshotId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;

                var resp = serverApi.UpdateSnapshot(ServerId, SnapshotId);
                WriteObject(resp);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #endregion


    /// <summary>
    /// <para type="synopsis">This commandlet Clones a server.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOServerClone -ServerId [UUID] -Name [name] </para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOServerClone")]
    [OutputType(typeof(ServerResponse))]
    public class NewServerClone : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Server ID. </para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Clone Name </para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Clone Name", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var serverApi = client.Servers;
                var resp = serverApi.CreateClone(ServerId, Name);

                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

}
