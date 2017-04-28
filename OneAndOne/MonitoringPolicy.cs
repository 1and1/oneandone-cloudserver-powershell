using OneAndOne.Client;
using OneAndOne.POCO.Response.MonitoringPolicies;
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
    /// <para type="synopsis">This commandlet returns a list of your monitoring policies, or one monitoring policy if the PolicyId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOMonitoringPolicy -PolicyId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOMonitoringPolicy")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class GetMonitoringPolicy : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Policy ID. If this parameters is not passed, the commandlet will return a list of all monitoring policies.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Policy Id", Mandatory = false, ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var policyApi = client.MonitoringPolicies;

                if (string.IsNullOrEmpty(PolicyId))
                {
                    var policy = policyApi.Get();
                    WriteObject(policy);
                }
                else
                {
                    var policy = policyApi.Show(PolicyId);
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
    /// <para type="synopsis">This commandlet will create a new monitoring policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOMonitoringPolicy -Name [name] -Description -Email [email] -CpuWarningValue [int]  -CpuWarningAlert [bool] -CpuCriticalValue [int]  -CpuCriticalAlert [bool]-RamWarningValue [int]  -RamWarningAlert [bool]-RamCriticalValue [int]  -RamCriticalAlert [bool] -DiskWarningValue [int]  -DiskWarningAlert [bool] -DiskCriticalValue [int]  -DiskCriticalAlert [bool] -TransferWarningValue [int]  -TransferWarningAlert [bool] -TransferCriticalValue [int]  -TransferCriticalAlert [bool] -InternalPingWarningValue [int]  -InternalPingWarningAlert [bool] -InternalPingCriticalValue [int]  -InternalPingCriticalAlert [bool] -Ports [array] -Processes [array] -Agent [bool]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOMonitoringPolicy")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class NewMonitoringPolicy : Cmdlet
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
        [Parameter(Position = 2, HelpMessage = "Description", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">User's email</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "User's email", Mandatory = true, ValueFromPipeline = true)]
        public string Email { get; set; }


        /// <summary>
        /// <para type="description">Set true for using agent</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Set true for using agent", Mandatory = true, ValueFromPipeline = true)]
        public bool Agent { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%)"minimum": 1,"maximum": 95,</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "Advise when this value is exceeded (%)minimum: 1,maximum: 95,", Mandatory = true, ValueFromPipeline = true)]
        public int CpuWarningValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 6, HelpMessage = "Enable alert", Mandatory = true, ValueFromPipeline = true)]
        public bool CpuWarningAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%) "maximum": 100</para>
        /// </summary>
        [Parameter(Position = 7, HelpMessage = "Advise when this value is exceeded (%) maximum: 100", Mandatory = true, ValueFromPipeline = true)]
        public int CpuCriticalValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "Enable alert", Mandatory = true, ValueFromPipeline = true)]
        public bool CpuCriticalAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%)minimum: 1,maximum: 95,</para>
        /// </summary>
        [Parameter(Position = 9, HelpMessage = "Advise when this value is exceeded (%)minimum: 1,maximum: 95,", Mandatory = true, ValueFromPipeline = true)]
        public int RamWarningValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 10, HelpMessage = "Enable alert", Mandatory = true, ValueFromPipeline = true)]
        public bool RamWarningAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%) "maximum": 100</para>
        /// </summary>
        [Parameter(Position = 11, HelpMessage = "Advise when this value is exceeded (%) maximum: 100", Mandatory = true, ValueFromPipeline = true)]
        public int RamCriticalValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 12, HelpMessage = "Enable alert", Mandatory = true, ValueFromPipeline = true)]
        public bool RamCriticalAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%)"minimum": 1,"maximum": 95,</para>
        /// </summary>
        [Parameter(Position = 13, HelpMessage = "Advise when this value is exceeded (%)minimum: 1,maximum: 95,", Mandatory = true, ValueFromPipeline = true)]
        public int DiskWarningValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 14, HelpMessage = "Enable alert", Mandatory = true, ValueFromPipeline = true)]
        public bool DiskWarningAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%) "maximum": 100</para>
        /// </summary>
        [Parameter(Position = 15, HelpMessage = "Advise when this value is exceeded (%) maximum: 100", Mandatory = true, ValueFromPipeline = true)]
        public int DiskCriticalValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 16, HelpMessage = "Enable alert", Mandatory = true, ValueFromPipeline = true)]
        public bool DiskCriticalAlert { get; set; }

        /// <summary>
        /// <para type="description">Thresholds</para>
        /// </summary>
        [Parameter(Position = 17, HelpMessage = "Advise when this value is exceeded (kbps)", Mandatory = true, ValueFromPipeline = true)]
        public int TransferWarningValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 18, HelpMessage = "Enable alert", Mandatory = true, ValueFromPipeline = true)]
        public bool TransferWarningAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%) "maximum": 100</para>
        /// </summary>
        [Parameter(Position = 19, HelpMessage = "Advise when this value is exceeded (kbps) maximum: 2000", Mandatory = true, ValueFromPipeline = true)]
        public int TransferCriticalValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 20, HelpMessage = "Enable alert", Mandatory = true, ValueFromPipeline = true)]
        public bool TransferCriticalAlert { get; set; }

        /// <summary>
        /// <para type="description">"Advise when this value is exceeded (ms)</para>
        /// </summary>
        [Parameter(Position = 21, HelpMessage = "Advise when this value is exceeded (ms)", Mandatory = true, ValueFromPipeline = true)]
        public int InternalPingWarningValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 22, HelpMessage = "Enable alert", Mandatory = true, ValueFromPipeline = true)]
        public bool InternalPingWarningAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%) "maximum": 100</para>
        /// </summary>
        [Parameter(Position = 23, HelpMessage = "dvise when this value is exceeded (ms) maximum: 100", Mandatory = true, ValueFromPipeline = true)]
        public int InternalPingCriticalValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 24, HelpMessage = "Enable alert", Mandatory = true, ValueFromPipeline = true)]
        public bool InternalPingCriticalAlert { get; set; }

        /// <summary>
        /// <para type="description">Array of ports that will be monitoring</para>
        /// </summary>
        [Parameter(Position = 25, HelpMessage = "Array of ports that will be monitoring", Mandatory = true, ValueFromPipeline = true)]
        public OneAndOne.POCO.Requests.MonitoringPolicies.Ports[] Ports { get; set; }

        /// <summary>
        /// <para type="description">Array of processes that will be monitoring</para>
        /// </summary>
        [Parameter(Position = 26, HelpMessage = "Array of processes that will be monitoring", Mandatory = true, ValueFromPipeline = true)]
        public OneAndOne.POCO.Requests.MonitoringPolicies.Processes[] Processes { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var mpApi = client.MonitoringPolicies;
                var result = mpApi.Create(new POCO.Requests.MonitoringPolicies.CreateMonitoringPolicyRequest
                {
                    Name = Name,
                    Agent = Agent,
                    Description = Description ?? null,
                    Email = Email,
                    Ports = new List<POCO.Requests.MonitoringPolicies.Ports>(Ports),
                    Processes = new List<POCO.Requests.MonitoringPolicies.Processes>(Processes),
                    Thresholds = new POCO.Requests.MonitoringPolicies.Thresholds
                    {
                        Cpu = new POCO.Requests.MonitoringPolicies.Cpu
                        {
                            Critical = new POCO.Requests.MonitoringPolicies.Critical
                            {
                                Alert = CpuCriticalAlert,
                                Value = CpuCriticalValue
                            },
                            Warning = new POCO.Requests.MonitoringPolicies.Warning
                            {
                                Alert = CpuWarningAlert,
                                Value = CpuWarningValue
                            }
                        },
                        Ram = new POCO.Requests.MonitoringPolicies.Ram
                        {
                            Critical = new POCO.Requests.MonitoringPolicies.Critical
                            {
                                Alert = RamCriticalAlert,
                                Value = RamCriticalValue
                            },
                            Warning = new POCO.Requests.MonitoringPolicies.Warning
                            {
                                Alert = RamWarningAlert,
                                Value = RamWarningValue
                            }
                        },
                        Disk = new POCO.Requests.MonitoringPolicies.Disk
                        {
                            Critical = new POCO.Requests.MonitoringPolicies.DiskCritical
                            {
                                Alert = DiskCriticalAlert,
                                Value = DiskCriticalValue
                            },
                            Warning = new POCO.Requests.MonitoringPolicies.DiskWarning
                            {
                                Alert = DiskWarningAlert,
                                Value = DiskWarningValue
                            }
                        },
                        InternalPing = new POCO.Requests.MonitoringPolicies.InternalPing
                        {
                            Critical = new POCO.Requests.MonitoringPolicies.InternalPingCritical
                            {
                                Alert = InternalPingCriticalAlert,
                                Value = InternalPingCriticalValue
                            },
                            Warning = new POCO.Requests.MonitoringPolicies.InternalPingWarning
                            {
                                Alert = InternalPingWarningAlert,
                                Value = InternalPingWarningValue
                            }
                        },
                        Transfer = new POCO.Requests.MonitoringPolicies.Transfer
                        {
                            Critical = new POCO.Requests.MonitoringPolicies.TransferCritical
                            {
                                Value = TransferCriticalValue,
                                Alert = TransferCriticalAlert

                            },
                            Warning = new POCO.Requests.MonitoringPolicies.Warning
                            {
                                Alert = TransferWarningAlert,
                                Value = TransferWarningValue
                            }
                        }
                    }
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
    /// <para type="synopsis">This commandlet removes a monitoring policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOMonitoringPolicy -PolicyId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOMonitoringPolicy")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class RemoveMonitoringPolicy : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Policy ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Policy Id", ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var mpApi = client.MonitoringPolicies;
                var resp = mpApi.Delete(PolicyId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies a Monitoring Policy.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOMonitoringPolicy -PolicyId [UUID] -Name [name] -Email [email] -CpuWarningValue [int] -CpuWarningAlert [bool] -CpuCriticalValue [int] -CpuCriticalAlert [bool]-RamWarningValue [int] -RamWarningAlert [bool] -RamCriticalValue [int] -RamCriticalAlert [bool] -DiskWarningValue [int] -DiskWarningAlert [bool] -DiskCriticalValue [int] -DiskCriticalAlert [bool] -TransferWarningValue [int] -TransferWarningAlert [bool] -TransferCriticalValue [int] -TransferCriticalAlert [bool] -InternalPingWarningValue [int]  -InternalPingWarningAlert [bool] -InternalPingCriticalValue [int] -InternalPingCriticalAlert [bool]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOMonitoringPolicy")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class SetMonitoringPolicy : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters


        /// <summary>
        /// <para type="description">Name</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Name", Mandatory = false, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Policy ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Policy Id", Mandatory = true, ValueFromPipeline = true)]
        public string PolicyId { get; set; }

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
        /// <para type="description">Advise when this value is exceeded (%)"minimum": 1,"maximum": 95,</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "Advise when this value is exceeded (%)minimum: 1,maximum: 95,", Mandatory = false, ValueFromPipeline = true)]
        public int? CpuWarningValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 6, HelpMessage = "Enable alert", Mandatory = false, ValueFromPipeline = true)]
        public bool? CpuWarningAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%) "maximum": 100</para>
        /// </summary>
        [Parameter(Position = 7, HelpMessage = "Advise when this value is exceeded (%) maximum: 100", Mandatory = false, ValueFromPipeline = true)]
        public int? CpuCriticalValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "Enable alert", Mandatory = false, ValueFromPipeline = true)]
        public bool? CpuCriticalAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%)minimum: 1,maximum: 95,</para>
        /// </summary>
        [Parameter(Position = 9, HelpMessage = "Advise when this value is exceeded (%)minimum: 1,maximum: 95,", Mandatory = false, ValueFromPipeline = true)]
        public int? RamWarningValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 10, HelpMessage = "Enable alert", Mandatory = false, ValueFromPipeline = true)]
        public bool? RamWarningAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%) "maximum": 100</para>
        /// </summary>
        [Parameter(Position = 11, HelpMessage = "Advise when this value is exceeded (%) maximum: 100", Mandatory = false, ValueFromPipeline = true)]
        public int? RamCriticalValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 12, HelpMessage = "Enable alert", Mandatory = false, ValueFromPipeline = true)]
        public bool? RamCriticalAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%)"minimum": 1,"maximum": 95,</para>
        /// </summary>
        [Parameter(Position = 13, HelpMessage = "Advise when this value is exceeded (%)minimum: 1,maximum: 95,", Mandatory = false, ValueFromPipeline = true)]
        public int? DiskWarningValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 14, HelpMessage = "Enable alert", Mandatory = false, ValueFromPipeline = true)]
        public bool? DiskWarningAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%) "maximum": 100</para>
        /// </summary>
        [Parameter(Position = 15, HelpMessage = "Advise when this value is exceeded (%) maximum: 100", Mandatory = false, ValueFromPipeline = true)]
        public int? DiskCriticalValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 16, HelpMessage = "Enable alert", Mandatory = false, ValueFromPipeline = true)]
        public bool? DiskCriticalAlert { get; set; }

        /// <summary>
        /// <para type="description">Thresholds</para>
        /// </summary>
        [Parameter(Position = 17, HelpMessage = "Advise when this value is exceeded (kbps)", Mandatory = false, ValueFromPipeline = true)]
        public int? TransferWarningValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 18, HelpMessage = "Enable alert", Mandatory = false, ValueFromPipeline = true)]
        public bool? TransferWarningAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%) "maximum": 100</para>
        /// </summary>
        [Parameter(Position = 19, HelpMessage = "Advise when this value is exceeded (kbps) maximum: 2000", Mandatory = false, ValueFromPipeline = true)]
        public int? TransferCriticalValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 20, HelpMessage = "Enable alert", Mandatory = false, ValueFromPipeline = true)]
        public bool? TransferCriticalAlert { get; set; }

        /// <summary>
        /// <para type="description">"Advise when this value is exceeded (ms)</para>
        /// </summary>
        [Parameter(Position = 21, HelpMessage = "Advise when this value is exceeded (ms)", Mandatory = false, ValueFromPipeline = true)]
        public int? InternalPingWarningValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 22, HelpMessage = "Enable alert", Mandatory = false, ValueFromPipeline = true)]
        public bool? InternalPingWarningAlert { get; set; }

        /// <summary>
        /// <para type="description">Advise when this value is exceeded (%) "maximum": 100</para>
        /// </summary>
        [Parameter(Position = 23, HelpMessage = "dvise when this value is exceeded (ms) maximum: 100", Mandatory = false, ValueFromPipeline = true)]
        public int? InternalPingCriticalValue { get; set; }

        /// <summary>
        /// <para type="description">Enable alert</para>
        /// </summary>
        [Parameter(Position = 24, HelpMessage = "Enable alert", Mandatory = false, ValueFromPipeline = true)]
        public bool? InternalPingCriticalAlert { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Constants.Configuration);
            var mpApi = client.MonitoringPolicies;
            var mp = client.MonitoringPolicies.Show(PolicyId);
            var resp = mpApi.Update(new POCO.Requests.MonitoringPolicies.UpdateMonitoringPolicyRequest
            {
                Name = Name ?? null,
                Description = Description ?? null,
                Email = Email ?? null,
                Thresholds = new POCO.Requests.MonitoringPolicies.Thresholds
                {
                    Cpu = new POCO.Requests.MonitoringPolicies.Cpu
                    {
                        Critical = new POCO.Requests.MonitoringPolicies.Critical
                        {
                            Alert = CpuCriticalAlert ?? mp.Thresholds.Cpu.Critical.Alert,
                            Value = CpuCriticalValue ?? mp.Thresholds.Cpu.Critical.Value
                        },
                        Warning = new POCO.Requests.MonitoringPolicies.Warning
                        {
                            Alert = CpuWarningAlert ?? mp.Thresholds.Cpu.Warning.Alert,
                            Value = CpuWarningValue ?? mp.Thresholds.Cpu.Warning.Value
                        }
                    },
                    Ram = new POCO.Requests.MonitoringPolicies.Ram
                    {
                        Critical = new POCO.Requests.MonitoringPolicies.Critical
                        {
                            Alert = RamCriticalAlert ?? mp.Thresholds.Ram.Critical.Alert,
                            Value = RamCriticalValue ?? mp.Thresholds.Ram.Critical.Value
                        },
                        Warning = new POCO.Requests.MonitoringPolicies.Warning
                        {
                            Alert = RamWarningAlert ?? mp.Thresholds.Ram.Warning.Alert,
                            Value = RamWarningValue ?? mp.Thresholds.Ram.Warning.Value
            
                        }
                    },
                    Disk = new POCO.Requests.MonitoringPolicies.Disk
                    {
                        Critical = new POCO.Requests.MonitoringPolicies.DiskCritical
                        {
                            Alert = DiskCriticalAlert ?? mp.Thresholds.Disk.Critical.Alert,
                            Value = DiskCriticalValue ?? mp.Thresholds.Disk.Critical.Value
                        },
                        Warning = new POCO.Requests.MonitoringPolicies.DiskWarning
                        {
                            Alert = DiskWarningAlert ?? mp.Thresholds.Disk.Warning.Alert,
                            Value = DiskWarningValue ?? mp.Thresholds.Disk.Warning.Value
                        }
                    },
                    InternalPing = new POCO.Requests.MonitoringPolicies.InternalPing
                    {
                        Critical = new POCO.Requests.MonitoringPolicies.InternalPingCritical
                        {
                            Alert = InternalPingCriticalAlert ?? mp.Thresholds.InternalPing.Critical.Alert,
                            Value = InternalPingCriticalValue ?? mp.Thresholds.InternalPing.Critical.Value
                        },
                        Warning = new POCO.Requests.MonitoringPolicies.InternalPingWarning
                        {
                            Alert = InternalPingWarningAlert ?? mp.Thresholds.InternalPing.Warning.Alert,
                            Value = InternalPingWarningValue ?? mp.Thresholds.InternalPing.Warning.Value
                        }
                    },
                    Transfer = new POCO.Requests.MonitoringPolicies.Transfer
                    {
                        Critical = new POCO.Requests.MonitoringPolicies.TransferCritical
                        {
                            Value = TransferCriticalValue ?? mp.Thresholds.Transfer.Critical.Value,
                            Alert = TransferCriticalAlert ?? mp.Thresholds.Transfer.Critical.Alert

                        },
                        Warning = new POCO.Requests.MonitoringPolicies.Warning
                        {
                            Alert = TransferWarningAlert ?? mp.Thresholds.Transfer.Warning.Alert,
                            Value = TransferWarningValue ?? mp.Thresholds.Transfer.Warning.Value
                        }
                    }
                }
            }, PolicyId);
            WriteObject(resp);
        }
    }

    #endregion

    #region policy servers

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of your monitoring policy's servers, or one server if the ServerId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOMonitoringPolicyServers -PolicyId [UUID] -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOMonitoringPolicyServers")]
    [OutputType(typeof(Server))]
    public class GetMonitoringPolicyServers : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Policy ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Policy Id", Mandatory = true, ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        /// <summary>
        /// <para type="description">Server ID. If this parameters is not passed, the commandlet will return a list of all servers.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Server Id", Mandatory = false, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var policyApi = client.MonitoringPoliciesServers;

                if (string.IsNullOrEmpty(ServerId))
                {
                    var policy = policyApi.Get(PolicyId);
                    WriteObject(policy);
                }
                else
                {
                    var policy = policyApi.Show(PolicyId, ServerId);
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
    /// <para type="synopsis">This commandlet will add a new server to the policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOMonitoringPolicyServers -Name [name] -Description -Email [email] -Thresholds [Thresholds] -Ports [array] -Processes [array] -Agent [bool]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOMonitoringPolicyServers")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class NewMonitoringPolicyServers : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">PolicyId</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "PolicyId", Mandatory = true, ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        /// <summary>
        /// <para type="description">Servers array of ids</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Servers array of ids", Mandatory = true, ValueFromPipeline = true)]
        public string[] Servers { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Constants.Configuration);
                var mpApi = client.MonitoringPoliciesServers;
                var result = mpApi.Create(new List<string>(Servers), PolicyId);

                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a server from the monitoring policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOMonitoringPolicyServers -PolicyId [UUID] -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOMonitoringPolicyServers")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class RemoveMonitoringPolicyServer : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Policy ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Policy Id", Mandatory = true, ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        /// <summary>
        /// <para type="description">Server ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = false, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var mpApi = client.MonitoringPoliciesServers;
                var resp = mpApi.Delete(PolicyId, ServerId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #endregion

    #region policy processes

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of your monitoring policies proceses, or one monitoring policy process if the ProcessId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOMonitoringPolicyProcess -PolicyId [UUID] -ProcessId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOMonitoringPolicyProcess")]
    [OutputType(typeof(Processes))]
    public class GetMonitoringPolicyProcess : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Policy ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Policy Id", Mandatory = true, ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        /// <summary>
        /// <para type="description">Process ID. If this parameters is not passed, the commandlet will return a list of all monitoring policies.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Process Id", Mandatory = false, ValueFromPipeline = true)]
        public string ProcessId { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var policyApi = client.MonitoringPoliciesProcesses;

                if (string.IsNullOrEmpty(ProcessId))
                {
                    var policy = policyApi.Get(PolicyId);
                    WriteObject(policy);
                }
                else
                {
                    var policy = policyApi.Show(PolicyId, ProcessId);
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
    /// <para type="synopsis">This commandlet will add a new process to the  monitoring policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOMonitoringPolicyProcess -PolicyId [UUID] -Processes [array]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOMonitoringPolicyProcess")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class NewMonitoringPolicyProcess : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Name</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Name", Mandatory = true, ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        /// <summary>
        /// <para type="description">Array of processes ids</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Array of processes ids", Mandatory = true, ValueFromPipeline = true)]
        public OneAndOne.POCO.Requests.MonitoringPolicies.Processes[] Processes { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Constants.Configuration);
                var mpApi = client.MonitoringPoliciesProcesses;
                var result = mpApi.Create(new List<POCO.Requests.MonitoringPolicies.Processes>(Processes), PolicyId);

                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a process from the monitoring policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOMonitoringPolicyProcess -PolicyId [UUID] -ProcessId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOMonitoringPolicyProcess")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class RemoveMonitoringPolicyProcess : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Policy ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Policy Id", ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        /// <summary>
        /// <para type="description">Process ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Process Id", ValueFromPipeline = true)]
        public string ProcessId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var mpApi = client.MonitoringPoliciesProcesses;
                var resp = mpApi.Delete(PolicyId, ProcessId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies a Monitoring Policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOMonitoringPolicyProcess -PolicyId [UUID] -Processes [Processes] -ProcessId [UUID] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOMonitoringPolicyProcess")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class SetMonitoringPolicyProcess : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Description</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Description", Mandatory = true, ValueFromPipeline = true)]
        public OneAndOne.POCO.Requests.MonitoringPolicies.Processes Processes { get; set; }

        /// <summary>
        /// <para type="description">Policy ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Policy Id", Mandatory = true, ValueFromPipeline = true)]
        public string PolicyId { get; set; }


        /// <summary>
        /// <para type="description">Process ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Process Id", ValueFromPipeline = true)]
        public string ProcessId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Constants.Configuration);
            var mpApi = client.MonitoringPoliciesProcesses;
            var resp = mpApi.Update(Processes, PolicyId, ProcessId);
            WriteObject(resp);
        }
    }

    #endregion

    #region policy ports

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of your monitoring policies ports, or one monitoring policy process if the PortId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOMonitoringPolicyPort -PolicyId [UUID] -PortId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOMonitoringPolicyPort")]
    [OutputType(typeof(Ports))]
    public class GetMonitoringPolicyPort : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Policy ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Policy Id", Mandatory = true, ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        /// <summary>
        /// <para type="description">Port ID. If this parameters is not passed, the commandlet will return a list of all monitoring policies ports.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Port Id", Mandatory = false, ValueFromPipeline = true)]
        public string PortId { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var policyApi = client.MonitoringPoliciesPorts;

                if (string.IsNullOrEmpty(PortId))
                {
                    var policy = policyApi.Get(PolicyId);
                    WriteObject(policy);
                }
                else
                {
                    var policy = policyApi.Show(PolicyId, PortId);
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
    /// <para type="synopsis">This commandlet will add a new ports to the monitoring policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOMonitoringPolicyPort -PolicyId [UUID] -Ports [array]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOMonitoringPolicyPort")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class NewMonitoringPolicyPort : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">PolicyId</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "PolicyId", Mandatory = true, ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        /// <summary>
        /// <para type="description">Array of Ports ids</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Array of Ports ids", Mandatory = true, ValueFromPipeline = true)]
        public OneAndOne.POCO.Requests.MonitoringPolicies.Ports[] Ports { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Constants.Configuration);
                var mpApi = client.MonitoringPoliciesPorts;
                var result = mpApi.Create(new List<POCO.Requests.MonitoringPolicies.Ports>(Ports), PolicyId);

                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a port from the monitoring policy.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOMonitoringPolicyPort -PolicyId [UUID] -PortId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOMonitoringPolicyPort")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class RemoveMonitoringPolicyPort : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Policy ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Policy Id", ValueFromPipeline = true)]
        public string PolicyId { get; set; }

        /// <summary>
        /// <para type="description">Port ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Port Id", ValueFromPipeline = true)]
        public string PortsId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var mpApi = client.MonitoringPoliciesPorts;
                var resp = mpApi.Delete(PolicyId, PortsId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies a Monitoring Policy port.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOMonitoringPolicyPort -PolicyId [UUID] -Ports [ports] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOMonitoringPolicyPort")]
    [OutputType(typeof(MonitoringPoliciesResponse))]
    public class SetMonitoringPolicyPorts : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Ports</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Ports", Mandatory = true, ValueFromPipeline = true)]
        public OneAndOne.POCO.Requests.MonitoringPolicies.Ports Ports { get; set; }

        /// <summary>
        /// <para type="description">Policy ID.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Policy Id", Mandatory = true, ValueFromPipeline = true)]
        public string PolicyId { get; set; }


        /// <summary>
        /// <para type="description">Process ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Port Id", ValueFromPipeline = true)]
        public string PortId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Constants.Configuration);
            var mpApi = client.MonitoringPoliciesPorts;
            var resp = mpApi.Update(Ports, PolicyId, PortId);
            WriteObject(resp);
        }
    }

    #endregion
}
