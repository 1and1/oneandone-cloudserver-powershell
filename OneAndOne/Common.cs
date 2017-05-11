using OneAndOne.Client;
using OneAndOne.POCO;
using OneAndOne.POCO.Response.Common;
using OneAndOne.POCO.Response.DataCenters;
using OneAndOne.POCO.Response.DVDS;
using OneAndOne.POCO.Response.ServerAppliances;
using OneAndOne.POCO.Response.Usages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OneAndOne
{
    #region usages
    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of your usages.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOUsage -Period [enum] -StartDate [date] -EndDate [date] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>

    [Cmdlet(VerbsCommon.Get, "OAOUsage")]
    [OutputType(typeof(UsageResponse))]
    public class GetUsage : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

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

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var usageApi = client.Usages;

                var usage = usageApi.Get(Period, Page, PerPage, Sort, Query, Fields, StartDate, EndDate);
                WriteObject(usage);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #endregion

    #region server appliances
    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of all the appliances that you can use for creating a server.if the ApplianceId is provided that single appliance will be returned</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOServerAppliance -ApplianceId [UUID]
    /// </example>

    [Cmdlet(VerbsCommon.Get, "OAOServerAppliance")]
    [OutputType(typeof(ServerAppliancesResponse))]
    public class GetServerAppliance : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

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
        /// <para type="description">Appliance ID</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "Appliance ID", Mandatory = false, ValueFromPipeline = true)]
        public string ApplianceId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var applianceApi = client.ServerAppliances;
                if (string.IsNullOrEmpty(ApplianceId))
                {
                    var appliances = applianceApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(appliances);

                }
                else
                {
                    var appliance = applianceApi.Show(ApplianceId);
                    WriteObject(appliance);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
    #endregion

    #region dvd isos
    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of all the operative systems and tools that you can load into your virtual DVD unit.if the DvdId is provided that Dvd Iso will be returned</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAODvd -DvdId [UUID]
    /// </example>

    [Cmdlet(VerbsCommon.Get, "OAODvd")]
    [OutputType(typeof(DVDResponse))]
    public class GetDvd : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

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
        /// <para type="description">DVD ID</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "DVD ID", Mandatory = false, ValueFromPipeline = true)]
        public string DvdId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var dvdApi = client.DVDs;
                if (string.IsNullOrEmpty(DvdId))
                {
                    var dvds = dvdApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(dvds);

                }
                else
                {
                    var dvd = dvdApi.Show(DvdId);
                    WriteObject(dvd);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
    #endregion

    #region Data centers
    /// <summary>
    /// <para type="synopsis">This commandlet returns information about available datacenters to create your resources.if the DcId is provided that Data center will be returned</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAODatacenter -DcId [UUID]
    /// </example>

    [Cmdlet(VerbsCommon.Get, "OAODatacenter")]
    [OutputType(typeof(DataCenterResponse))]
    public class GetDatacenter : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

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
        /// <para type="description">Datacenter ID</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "Datacenter ID", Mandatory = false, ValueFromPipeline = true)]
        public string DcId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var dcApi = client.DataCenters;
                if (string.IsNullOrEmpty(DcId))
                {
                    var dcs = dcApi.Get(Page ?? null, PerPage ?? null, Sort ?? null, Query ?? null, Fields ?? null);
                    WriteObject(dcs);

                }
                else
                {
                    var dc = dcApi.Show(DcId);
                    WriteObject(dc);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
    #endregion

    #region pricing
    /// <summary>
    /// <para type="synopsis">This commandlet returns prices for all available resources in Cloud Panel</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOPricing
    /// </example>

    [Cmdlet(VerbsCommon.Get, "OAOPricing")]
    [OutputType(typeof(PricingResponse))]
    public class GetPricing : Cmdlet
    {

        private static OneAndOneClient client;

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var api = client.Common;
                var pricing = api.GetPricing();
                WriteObject(pricing);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
    #endregion

    #region ping operations
    /// <summary>
    /// <para type="synopsis">This commandlet returns true if the API is running and available</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOPingApi
    /// </example>

    [Cmdlet(VerbsCommon.Get, "OAOPingApi")]
    [OutputType(typeof(PricingResponse))]
    public class PingApi : Cmdlet
    {

        private static OneAndOneClient client;

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var api = client.Common;
                var ping = api.Ping();
                WriteObject(ping);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet returns true if the API authentication token is valid</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOPingAuthentication
    /// </example>

    [Cmdlet(VerbsCommon.Get, "OAOPingAuthentication")]
    [OutputType(typeof(PricingResponse))]
    public class GetPingAuthentication : Cmdlet
    {

        private static OneAndOneClient client;

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var api = client.Common;
                var ping = api.PingAuthentication();
                WriteObject(ping);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }
    #endregion
}
