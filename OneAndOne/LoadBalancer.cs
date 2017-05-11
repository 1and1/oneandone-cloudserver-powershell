using OneAndOne.Client;
using OneAndOne.POCO.Requests.LoadBalancer;
using OneAndOne.POCO.Response.LoadBalancers;
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
    /// <para type="synopsis">This commandlet returns a list of your shared storages., or one loadbalancer if the BalancerId is provided , if the ID parameter is provided all the optional paramters are ignored</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOLoadBalancer -BalancerId [UUID] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOLoadBalancer")]
    [OutputType(typeof(LoadBalancerResponse))]
    public class GetLoadBalancer : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Balancer ID. If this parameters is not passed, the commandlet will return a list of all Loadbalancers.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Balancer Id", Mandatory = true, ValueFromPipeline = true)]
        public string BalancerId { get; set; }

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
                var balancerApi = client.LoadBalancer;

                if (string.IsNullOrEmpty(BalancerId))
                {
                    var lbs = balancerApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(lbs);
                }
                else
                {
                    var lb = balancerApi.Show(BalancerId);
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
    /// <para type="synopsis">This commandlet will create a new load balancer.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOLoadBalancer -Name [name] -Description -DatacenterId [UUID] -HealthCheckTest [enum] -HealthCheckInterval [interval] -HealthCheckPath [path] -HealthCheckPathParser [parser] -Persistence [bool] -PersistenceTime [seconds] -Method [enum] -Rules [rules]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOLoadBalancer")]
    [OutputType(typeof(LoadBalancerResponse))]
    public class NewLoadBalancer : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Load balancer name.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Load balancer name.", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Description of the Loadbalancer</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Description of the Loadbalancer", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Datacenter of the Loadbalancer</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Datacenter of the Loadbalancer", Mandatory = false, ValueFromPipeline = true)]
        public string DatacenterId { get; set; }

        /// <summary>
        /// <para type="description">Type of the health check. At the moment, HTTP is not allowed.</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Type of the health check. At the moment, HTTP is not allowed.", Mandatory = true, ValueFromPipeline = true)]
        public HealthCheckTestTypes HealthCheckTest { get; set; }

        /// <summary>
        /// <para type="description">Required:Health check period in secondsminimum: 5, maximum: 300, multipleOf: 1</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "Required:Health check period in secondsminimum: 5, maximum: 300, multipleOf: 1", Mandatory = true, ValueFromPipeline = true)]
        public int HealthCheckInterval { get; set; }

        /// <summary>
        /// <para type="description">Url to call for cheking. Required for HTTP health check. max 1000 char</para>
        /// </summary>
        [Parameter(Position = 6, HelpMessage = "Url to call for cheking. Required for HTTP health check. max 1000", Mandatory = false, ValueFromPipeline = true)]
        public string HealthCheckPath { get; set; }

        /// <summary>
        /// <para type="description">Regular expression to check. Required for HTTP health check, max 64 char.</para>
        /// </summary>
        [Parameter(Position = 7, HelpMessage = "Regular expression to check. Required for HTTP health check, max 64 char.", Mandatory = false, ValueFromPipeline = true)]
        public string HealthCheckPathParser { get; set; }

        /// <summary>
        /// <para type="description">Persistence</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "Persistence", Mandatory = true, ValueFromPipeline = true)]
        public bool Persistence { get; set; }

        /// <summary>
        /// <para type="description">Persistence time in seconds. Required if persistence is enabled.minimum: 30,maximum: 1200, multipleOf: 1</para>
        /// </summary>
        [Parameter(Position = 9, HelpMessage = "Persistence time in seconds. Required if persistence is enabled.minimum: 30,maximum: 1200, multipleOf: 1", Mandatory = true, ValueFromPipeline = true)]
        public int PersistenceTime { get; set; }

        /// <summary>
        /// <para type="description">Balancing procedure</para>
        /// </summary>
        [Parameter(Position = 10, HelpMessage = "Balancing procedure", Mandatory = true, ValueFromPipeline = true)]
        public LoadBalancerMethod Method { get; set; }

        /// <summary>
        /// <para type="description">Rules to add { PortTo,PortFrom,Protocol,Source}.</para>
        /// </summary>
        [Parameter(Position = 11, HelpMessage = "Rules to add { PortBalancer,PortServer,Protocol,Source}", Mandatory = true, ValueFromPipeline = true)]
        public LoadBalancerRuleRequest[] Rules { get; set; }



        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var lbApi = client.LoadBalancer;
                var result = lbApi.Create(new CreateLoadBalancerRequest
                {
                    DatacenterId = DatacenterId ?? null,
                    Description = Description ?? null,
                    HealthCheckInterval = HealthCheckInterval,
                    HealthCheckPath = HealthCheckPath ?? null,
                    HealthCheckPathParser = HealthCheckPathParser ?? null,
                    HealthCheckTest = HealthCheckTest,
                    Method = Method,
                    Name = Name,
                    Persistence = Persistence,
                    PersistenceTime = PersistenceTime,
                    Rules = new List<LoadBalancerRuleRequest>(Rules)

                });

                WriteVerbose("Creating the load balancer...");
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a Loadbalancer.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOLoadBalancer -BalancerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOLoadBalancer")]
    [OutputType(typeof(LoadBalancerResponse))]
    public class RemoveLoadBalancer : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Loadbalancer ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Balancer Id", ValueFromPipeline = true)]
        public string BalancerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var lbApi = client.LoadBalancer;
                var resp = lbApi.Delete(BalancerId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies a LoadBalancer.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOLoadBalancer -Name [name] -Description -HealthCheckTest [enum] -HealthCheckInterval [interval] -HealthCheckPath [path] -HealthCheckPathParser [parser] -Persistence [bool] -PersistenceTime [seconds] -Method [enum] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOLoadBalancer")]
    [OutputType(typeof(LoadBalancerResponse))]
    public class SetLoadBalancer : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Loadbalancer name.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Loadbalancer name.", Mandatory = false, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Description of the Loadbalancer</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Description of the Loadbalancer", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Type of the health check. At the moment, HTTP is not allowed</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Type of the health check. At the moment, HTTP is not allowed", Mandatory = false, ValueFromPipeline = true)]
        public HealthCheckTestTypes? HealthCheckTest { get; set; }

        /// <summary>
        /// <para type="description">Description of the Loadbalancer</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Description of the Loadbalancer", Mandatory = false, ValueFromPipeline = true)]
        public int? HealthCheckInterval { get; set; }

        /// <summary>
        /// <para type="description">Persistence</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Persistence", Mandatory = false, ValueFromPipeline = true)]
        public bool? Persistence { get; set; }

        /// <summary>
        /// <para type="description">Persistence time in seconds</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "Persistence time in seconds", Mandatory = false, ValueFromPipeline = true)]
        public int? PersistenceTime { get; set; }

        /// <summary>
        /// <para type="description">Balancing procedure</para>
        /// </summary>
        [Parameter(Position = 6, HelpMessage = "Balancing procedure", Mandatory = false, ValueFromPipeline = true)]
        public LoadBalancerMethod? Method { get; set; }

        /// <summary>
        /// <para type="description">Url to call for checking. Required for HTTP health check.</para>
        /// </summary>
        [Parameter(Position = 7, HelpMessage = "Url to call for checking. Required for HTTP health check.", Mandatory = false, ValueFromPipeline = true)]
        public string HealthCheckPath { get; set; }

        /// <summary>
        /// <para type="description">Regular expression to check. Required for HTTP health check.</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "Regular expression to check. Required for HTTP health check.", Mandatory = false, ValueFromPipeline = true)]
        public string HealthCheckPathParse { get; set; }

        /// <summary>
        /// <para type="description">Balancer ID.</para>
        /// </summary>
        [Parameter(Position = 9, HelpMessage = "Balancer Id", Mandatory = true, ValueFromPipeline = true)]
        public string BalancerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Helper.Configuration);
            var lbApi = client.LoadBalancer;
            var lb = client.LoadBalancer.Show(BalancerId);

            var resp = lbApi.Update(new UpdateLoadBalancerRequest
            {
                Description = Description ?? null,
                HealthCheckInterval = HealthCheckInterval ?? lb.HealthCheckInterval,
                HealthCheckPath = HealthCheckPath ?? null,
                HealthCheckPathParse = HealthCheckPathParse ?? null,
                HealthCheckTest = HealthCheckTest ?? lb.HealthCheckTest,
                Method = Method ?? lb.Method,
                Name = Name ?? lb.Name,
                Persistence = Persistence ?? lb.Persistence,
                PersistenceTime = PersistenceTime ?? lb.PersistenceTime,
            }, BalancerId);
            WriteObject(resp);
        }
    }

    #endregion

    #region Loadbalancer servers Ips

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of the servers/IPs attached to a load balancer, or one server ip if the ServerIPId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOLoadBalancerServerIps -BalancerId [UUID] -IpId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOLoadBalancerServerIps")]
    [OutputType(typeof(LoadBalancerServerIpsResponse))]
    public class GetLoadBalancerServerIps : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Balancer ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Balancer Id", ValueFromPipeline = true, Mandatory = true)]
        public string BalancerId { get; set; }

        /// <summary>
        /// <para type="description">Ip ID. If this parameters is not passed, the commandlet will return a list of all servers ips.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Ip Id", ValueFromPipeline = true, Mandatory = false)]
        public string IpId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var lbApi = client.LoadBalancer;

                if (string.IsNullOrEmpty(IpId))
                {
                    var lbIps = lbApi.GetLoadBalancerServerIps(BalancerId);
                    WriteObject(lbIps);
                }
                else
                {
                    var lbIp = lbApi.ShowLoadBalancerServerIp(BalancerId, IpId);
                    WriteObject(lbIp);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will assigns servers/IPs to a Loadbalancer.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-LoadBalancerServerIps -BalancerId [UUID] -Ips -[array] </para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOLoadBalancerServerIps")]
    [OutputType(typeof(LoadBalancerResponse))]
    public class NewLoadBalancerServerIps : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Balancer ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Balancer Id", ValueFromPipeline = true, Mandatory = true)]
        public string BalancerId { get; set; }

        /// <summary>
        /// <para type="description">List of servers to add to the Loadbalancer.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "List of Ip ids to add to the Loadbalancer", Mandatory = true, ValueFromPipeline = true)]
        public string[] Ips { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var lbApi = client.LoadBalancer;
                var result = lbApi.CreateLoadBalancerServerIPs(new AssignLoadBalancerServerIpsRequest
                {
                    ServerIps = new List<string>(Ips)
                }, BalancerId);

                WriteVerbose("Adding a server ip to the Loadbalancer...");
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a server ip from the Loadbalancer.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-LoadBalancerServerIps -BalancerId [UUID] -IpId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOLoadBalancerServerIps")]
    [OutputType(typeof(LoadBalancerResponse))]
    public class RemoveLoadBalancerServerIps : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Balancer ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Balancer Id", Mandatory = true, ValueFromPipeline = true)]
        public string BalancerId { get; set; }

        /// <summary>
        /// <para type="description">IP ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "IP Id", ValueFromPipeline = true, Mandatory = false)]
        public string IpId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var lbApi = client.LoadBalancer;
                var resp = lbApi.DeleteLoadBalancerServerIP(BalancerId, IpId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
    #endregion

    #region Balancer rules

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of the rules of a LoadBalancer., or one rule if the RuleId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOLoadBalancerRules -BalancerId [UUID] -RuleId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOLoadBalancerRules")]
    [OutputType(typeof(LoadBalancerRule))]
    public class GetLoadBalancerRules : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Balancer ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Balancer Id", ValueFromPipeline = true, Mandatory = true)]
        public string BalancerId { get; set; }

        /// <summary>
        /// <para type="description">Rule ID. If this parameters is not passed, the commandlet will return a list of all rules.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Rule Id", ValueFromPipeline = true, Mandatory = false)]
        public string RuleId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var lbApi = client.LoadBalancer;

                if (string.IsNullOrEmpty(RuleId))
                {
                    var rules = lbApi.GetLoadBalancerRules(BalancerId);
                    WriteObject(rules);
                }
                else
                {
                    var rule = lbApi.ShowLoadBalancerRule(BalancerId, RuleId);
                    WriteObject(rule);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will adds new rules to a Loadbalancer.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOLoadBalancerRules -BalancerId [UUID] -Rules -[array] </para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOLoadBalancerRules")]
    [OutputType(typeof(LoadBalancerResponse))]
    public class NewLoadBalancerRules : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Balancer ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Balancer Id", ValueFromPipeline = true, Mandatory = true)]
        public string BalancerId { get; set; }

        /// <summary>
        /// <para type="description">Rules to add to the LoadBalancer.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Rules to add to the LoadBalancer", Mandatory = true, ValueFromPipeline = true)]
        public RuleRequest[] Rules { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var lbApi = client.LoadBalancer;
                var result = lbApi.CreateLoadBalancerRule(new AddLoadBalancerRuleRequest
                {
                    Rules = new List<RuleRequest>(Rules)
                }, BalancerId);
                WriteVerbose("Adding rules to the LoadBalancer...");
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a rule id from the Loadbalancer.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOLoadBalancerRules -BalancerId [UUID] -RuleId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOLoadBalancerRules")]
    [OutputType(typeof(LoadBalancerResponse))]
    public class RemoveLoadBalancerRule : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Balancer ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Balancer Id", Mandatory = true, ValueFromPipeline = true)]
        public string BalancerId { get; set; }

        /// <summary>
        /// <para type="description">Rule ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Rule Id", ValueFromPipeline = true, Mandatory = false)]
        public string RuleId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var lbApi = client.LoadBalancer;
                var resp = lbApi.DeleteLoadBalancerRules(BalancerId, RuleId);
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
