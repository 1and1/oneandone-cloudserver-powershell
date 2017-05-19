using OneAndOne.Client;
using OneAndOne.POCO.Requests.FirewallPolicies;
using OneAndOne.POCO.Response;
using OneAndOne.POCO.Response.FirewallPolicies;
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
    /// <para type="synopsis">This commandlet returns a list of your firewall policies, or one firewall policy if the firewallId is provided , if the ID parameter is provided all the optional paramters are ignored</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOFirewall -FirewallId [UUID] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOFirewall")]
    [OutputType(typeof(FirewallPolicyResponse))]
    public class GetFirewallPolicy : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Firewall ID. If this parameters is not passed, the commandlet will return a list of all firewall policies.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Firewall Id", ValueFromPipeline = true)]
        public string FirewallId { get; set; }

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
                var firewallApi = client.FirewallPolicies;

                if (string.IsNullOrEmpty(FirewallId))
                {
                    var firewalls = firewallApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(firewalls);
                }
                else
                {
                    var firewall = firewallApi.Show(FirewallId);
                    WriteObject(firewall);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will create a new firewall policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOFirewallPolicy -Name [name] -Description -Rules [rules]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOFirewallPolicy")]
    [OutputType(typeof(FirewallPolicyResponse))]
    public class NewFirewallPolicy : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Firewall policy name.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Firewall policye name.", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Description of the firewall</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Description of the firewall", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Rules to add { PortTo,PortFrom,Protocol,Source}.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Rules to add { PortTo,PortFrom,Protocol,Source}", Mandatory = true, ValueFromPipeline = true)]
        public CreateFirewallPocliyRule[] Rules { get; set; }



        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var firewallApi = client.FirewallPolicies;
                var result = firewallApi.Create(new CreateFirewallPolicyRequest
                {
                    Name = Name,
                    Description = Description,
                    Rules = new List<CreateFirewallPocliyRule>(Rules)
                });

                WriteVerbose("Creating the firewall policy...");
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a firewall policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOFirewallPolicy -FirewallId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOFirewallPolicy")]
    [OutputType(typeof(FirewallPolicyResponse))]
    public class RemoveFirewallPolicy : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Firewall ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Firewall Id", ValueFromPipeline = true)]
        public string FirewallId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var firewallApi = client.FirewallPolicies;
                var resp = firewallApi.Delete(FirewallId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies a firewall policy.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-FirewallPolicy -Name [name] -Description -[description]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOFirewallPolicy")]
    [OutputType(typeof(FirewallPolicyResponse))]
    public class SetFirewallPolicy : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">firewall policy name.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "firewall policy name.", Mandatory = false, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Description of the firewall policy</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Description of the firewall policy", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Firewall ID. If this parameters is not passed, the commandlet will return a list of all firewall policies.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Firewall Id", ValueFromPipeline = true)]
        public string FirewallId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Helper.Configuration);
            var firewallApi = client.FirewallPolicies;
            var firewall = client.FirewallPolicies.Show(FirewallId);

            var resp = firewallApi.Update(new UpdateFirewallPolicyRequest
            {
                Name = Name ?? null,
                Description = Description ?? null
            }, FirewallId);
            WriteObject(resp);
        }
    }

    #endregion

    #region Firewall policy servers Ips

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of all firewall policy server ips, or one server ip if the ServerIPId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOFirewallPolicyServerIps -FirewallId [UUID] -IpId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOFirewallPolicyServerIps")]
    [OutputType(typeof(FirewallPolicyServerIPsResponse))]
    public class GetFirewallPolicyServerIps : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Firewall ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Firewall Id", ValueFromPipeline = true, Mandatory = true)]
        public string FirewallId { get; set; }

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
                var firewallApi = client.FirewallPolicies;

                if (string.IsNullOrEmpty(IpId))
                {
                    var firewallPolicyIps = firewallApi.GetFirewallPolicyServerIps(FirewallId);
                    WriteObject(firewallPolicyIps);
                }
                else
                {
                    var firewallpolicyIp = firewallApi.ShowFirewallPolicyServerIp(FirewallId, IpId);
                    WriteObject(firewallpolicyIp);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will assigns servers/IPs to a firewall policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOFirewallPolicyServerIps -FirewallId [UUID] -Ips -[array] </para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOFirewallPolicyServerIps")]
    [OutputType(typeof(FirewallPolicyResponse))]
    public class NewFirewallPolicyServerIps : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Firewall ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Firewall Id", ValueFromPipeline = true, Mandatory = true)]
        public string FirewallId { get; set; }

        /// <summary>
        /// <para type="description">List of servers to add to the firewall Policy.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "List of Ip ids to add to the firewall policy", Mandatory = true, ValueFromPipeline = true)]
        public string[] Ips { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var firewallApi = client.FirewallPolicies;
                var result = firewallApi.CreateFirewallPolicyServerIPs(new AssignFirewallServerIPRequest { ServerIps = new List<string>(Ips) }, FirewallId);

                WriteVerbose("Adding a server ip to the firewall policy...");
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a server ip from the firewall policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOFirewallPolicyServerIps -FirewallId [UUID] -IpId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOFirewallPolicyServerIps")]
    [OutputType(typeof(FirewallPolicyResponse))]
    public class RemoveFirewallPolicyServerIps : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Firewall Policy ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Firewall Id", Mandatory = true, ValueFromPipeline = true)]
        public string FirewallId { get; set; }

        /// <summary>
        /// <para type="description">IP ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Server Id", ValueFromPipeline = true, Mandatory = false)]
        public string IpId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var firewallApi = client.FirewallPolicies;
                var resp = firewallApi.DeleteFirewallPolicyServerIP(FirewallId, IpId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
    #endregion

    #region Firewall policy rules

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of the rules of a firewall policy., or one rule if the RuleId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOFirewallPolicyRules -FirewallId [UUID] -RuleId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOFirewallPolicyRules")]
    [OutputType(typeof(FirewallRule))]
    public class GetFirewallPolicyRules : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Firewall ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Firewall Id", ValueFromPipeline = true, Mandatory = true)]
        public string FirewallId { get; set; }

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
                var firewallApi = client.FirewallPolicies;

                if (string.IsNullOrEmpty(RuleId))
                {
                    var rules = firewallApi.GetFirewallPolicyRules(FirewallId);
                    WriteObject(rules);
                }
                else
                {
                    var rule = firewallApi.ShowFirewallPolicyRule(FirewallId, RuleId);
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
    /// <para type="synopsis">This commandlet will adds new rules to a firewall policy..</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOFirewallPolicyRules -FirewallId [UUID] -Rules -[array] </para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOFirewallPolicyRules")]
    [OutputType(typeof(FirewallPolicyResponse))]
    public class NewFirewallPolicyServerRules : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Firewall ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Firewall Id", ValueFromPipeline = true, Mandatory = true)]
        public string FirewallId { get; set; }

        /// <summary>
        /// <para type="description">Rules to add to the firewall policy.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Rules to add to the firewall policy", Mandatory = true, ValueFromPipeline = true)]
        public RuleRequest[] Rules { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var firewallApi = client.FirewallPolicies;
                var result = firewallApi.CreateFirewallPolicyRule(
                    new AddFirewallPolicyRuleRequest { Rules = new List<RuleRequest>(Rules) }, FirewallId);

                WriteVerbose("Adding rules to the firewall policy...");
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a rule id from the firewall policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOFirewallPolicyRules -FirewallId [UUID] -RuleId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOFirewallPolicyRules")]
    [OutputType(typeof(FirewallPolicyResponse))]
    public class RemoveFirewallPolicyRule : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Firewall Policy ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Firewall Id", Mandatory = true, ValueFromPipeline = true)]
        public string FirewallId { get; set; }

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
                var firewallApi = client.FirewallPolicies;
                var resp = firewallApi.DeleteFirewallPolicyRules(FirewallId, RuleId);
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
