using OneAndOne.Client;
using OneAndOne.POCO;
using OneAndOne.POCO.Response.MonitoringCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OneAndOne
{

    /// <summary>
    /// <para type="synopsis">This commandlet lists usages and alerts of monitoring servers.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOMonitoringCenter -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOMonitoringCenter")]
    [OutputType(typeof(MonitoringCenterResponse))]
    public class GetMonitoringCenter : Cmdlet
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

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var centerApi = client.MonitoringCenter;

                var center = centerApi.Get(Page, PerPage, Sort, Query, Fields);
                WriteObject(center);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }


    /// <summary>
    /// <para type="synopsis">This commandlet lists usages and alerts of monitoring servers.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOMonitoringCenterServer -ServerId [UUID] -Period [enum] -StartDate [date] -EndDate [date]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOMonitoringCenterServer")]
    [OutputType(typeof(MonitoringCenterResponse))]
    public class GetMonitoringCenterServer : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">required (one of LAST_HOUR,LAST_24H,LAST_7D,LAST_30D,LAST_365D,CUSTOM ),Time range whose logs will be shown.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "required (one of LAST_HOUR,LAST_24H,LAST_7D,LAST_30D,LAST_365D,CUSTOM ),Time range whose logs will be shown.", Mandatory = true, ValueFromPipeline = true)]
        public PeriodType Period { get; set; }

        /// <summary>
        /// <para type="description">(date) The first date in a custom range. Required only if selected period is "CUSTOM".</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "(date) The first date in a custom range. Required only if selected period is CUSTOM.", Mandatory = false, ValueFromPipeline = true)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// <para type="description">(date) The second date in a custom range. Required only if selected period is "CUSTOM"..</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "(date) The second date in a custom range. Required only if selected period is CUSTOM.", Mandatory = false, ValueFromPipeline = true)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// <para type="description">ServerId</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "ServerId", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var centerApi = client.MonitoringCenter;

                var centerServer = centerApi.Show(ServerId, Period, StartDate, EndDate);
                WriteObject(centerServer);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
}
