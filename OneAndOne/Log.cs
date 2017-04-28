using OneAndOne.Client;
using OneAndOne.POCO;
using OneAndOne.POCO.Response.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OneAndOne
{
    /// <summary>
    /// <para type="synopsis">This commandlet returns a list with logs.If the logId is provided it will return an information about one log</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOLogs -LogId [UUID] -Period [enum] -StartDate [date] -EndDate [date] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOLogs")]
    [OutputType(typeof(LogsResponse))]
    public class GetLogs : Cmdlet
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
        /// <para type="description">required (one of LAST_HOUR,LAST_24H,LAST_7D,LAST_30D,LAST_365D,CUSTOM ),Time range whose logs will be shown.</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "required (one of LAST_HOUR,LAST_24H,LAST_7D,LAST_30D,LAST_365D,CUSTOM ),Time range whose logs will be shown.", Mandatory = false, ValueFromPipeline = true)]
        public PeriodType Period { get; set; }

        /// <summary>
        /// <para type="description">(date) The first date in a custom range. Required only if selected period is "CUSTOM".</para>
        /// </summary>
        [Parameter(Position = 6, HelpMessage = "(date) The first date in a custom range. Required only if selected period is CUSTOM.", Mandatory = false, ValueFromPipeline = true)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// <para type="description">(date) The second date in a custom range. Required only if selected period is "CUSTOM"..</para>
        /// </summary>
        [Parameter(Position = 7, HelpMessage = "(date) The second date in a custom range. Required only if selected period is CUSTOM.", Mandatory = false, ValueFromPipeline = true)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// <para type="description">Log ID</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "Log ID", Mandatory = false, ValueFromPipeline = true)]
        public string LogId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var logsApi = client.Logs;
                if (String.IsNullOrEmpty(LogId))
                {
                    var logs = logsApi.Get(Period, Page, PerPage, Sort, Query, Fields, StartDate, EndDate);
                    WriteObject(logs);

                }
                else
                {
                    var log = logsApi.Show(LogId);
                    WriteObject(log);
                }


            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
}
