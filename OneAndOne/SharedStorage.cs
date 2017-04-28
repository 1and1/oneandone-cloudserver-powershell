using OneAndOne.Client;
using OneAndOne.POCO.Response.SharedStorages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OneAndOne
{

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of all Shared Storages, or one Shared Storage if the StorageId is provided, if the ID parameter is provided all the optional paramters are ignored.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOSharedStorage -StorageId [UUID] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOSharedStorage")]
    [OutputType(typeof(SharedStoragesResponse))]
    public class GetSharedStorage : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Storage ID. If this parameters is not passed, the commandlet will return a list of all Shared Storages.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Storage Id", ValueFromPipeline = true)]
        public string StorageId { get; set; }

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
                client = OneAndOneClient.Instance(Constants.Configuration);
                var storageApi = client.SharedStorages;

                if (string.IsNullOrEmpty(StorageId))
                {
                    var storages = storageApi.Get(Page, PerPage, Sort, Query, Fields);
                    WriteObject(storages);
                }
                else
                {
                    var storage = storageApi.Show(StorageId);
                    WriteObject(storage);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will add a new shared storage</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOSharedStorage -Name [name] -Description -[description] -Size [size]  -DatacenterId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOSharedStorage")]
    [OutputType(typeof(SharedStoragesResponse))]
    public class NewSharedStorage : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Shared storage name.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Shared storage name.", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Datacenter ID of the shared storage</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Datacenter ID of the shared storage", Mandatory = true, ValueFromPipeline = true)]
        public string DatacenterId { get; set; }

        /// <summary>
        /// <para type="description">Description of the shared storage</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Description of the shared storage", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Size of shared storage","minimum": "50","maximum": "2000","multipleOf": "50".</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Size of shared storage minimum :  50 ,  maximum : 2000, multipleOf: 50.", Mandatory = true, ValueFromPipeline = true)]
        public int Size { get; set; }



        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Constants.Configuration);
                var storageApi = client.SharedStorages;
                var result = storageApi.Create(new POCO.Requests.SharedStorages.CreateSharedStorage
                {
                    Name = Name,
                    Description = Description ?? null,
                    Size = Size,
                    DatacenterId = DatacenterId ?? null
                });

                WriteVerbose("Creating the shared storage...");
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a shared storage.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOSharedStorage -StorageId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOSharedStorage")]
    [OutputType(typeof(SharedStoragesResponse))]
    public class RemoveSharedStorage : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Storage ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Storage Id", Mandatory = true, ValueFromPipeline = true)]
        public string StorageId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var storageApi = client.SharedStorages;
                var resp = storageApi.Delete(StorageId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies a shared storage.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOSharedStorage -Name [name] -Description -[description] -Size [size]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOSharedStorage")]
    [OutputType(typeof(SharedStoragesResponse))]
    public class SetSharedStorage : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Shared storage name.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Shared storage name.", Mandatory = false, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Description of the shared storage</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Description of the shared storage", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Size of shared storage","minimum": "50","maximum": "2000","multipleOf": "50".</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Size of shared storage minimum :  50 ,  maximum : 2000, multipleOf: 50.", Mandatory = false, ValueFromPipeline = true)]
        public int? Size { get; set; }

        /// <summary>
        /// <para type="description">Storage ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Storage Id", Mandatory = true, ValueFromPipeline = true)]
        public string StorageId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Constants.Configuration);
            var storageApi = client.SharedStorages;
            var storage = client.SharedStorages.Show(StorageId);

            var resp = storageApi.Update(new POCO.Requests.SharedStorages.UpdateSharedStorageRequest { Name = Name ?? null, Description = Description ?? null, Size = Size ?? storage.Size }, StorageId);
            WriteObject(resp);
        }
    }


    #region Shared storage servers 

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of all Shared Storages servers, or one Shared Storage server if the ServerId is provided</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOSharedStorageServer -StorageId [UUID] -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOSharedStorageServer")]
    [OutputType(typeof(SharedStorageServerResponse))]
    public class GetSharedStorageServers : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Storage ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Storage Id", ValueFromPipeline = true, Mandatory = true)]
        public string StorageId { get; set; }

        /// <summary>
        /// <para type="description">Server ID. If this parameters is not passed, the commandlet will return a list of all Shared Storage's servers.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Server Id", ValueFromPipeline = true, Mandatory = false)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var storageApi = client.SharedStorages;

                if (string.IsNullOrEmpty(ServerId))
                {
                    var storageServers = storageApi.GetSharedStorageServers(StorageId);
                    WriteObject(storageServers);
                }
                else
                {
                    var storageServer = storageApi.ShowSharedStoragesServer(StorageId, ServerId);
                    WriteObject(storageServer);
                }

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet will add a server to the provided shared storage</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOSharedStorageServer -Servers [array]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOSharedStorageServer")]
    [OutputType(typeof(SharedStoragesResponse))]
    public class NewSharedStorageServer : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Storage ID.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Storage Id", ValueFromPipeline = true, Mandatory = true)]
        public string StorageId { get; set; }

        /// <summary>
        /// <para type="description">List of servers to add to the storage.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "List of servers to add to the storage", Mandatory = true, ValueFromPipeline = true)]
        public POCO.Requests.SharedStorages.Server[] Servers { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Constants.Configuration);
                var storageApi = client.SharedStorages;
                var result = storageApi.CreateServerSharedStorages(new POCO.Requests.SharedStorages.AttachSharedStorageServerRequest
                {
                    Servers = new List<POCO.Requests.SharedStorages.Server>(Servers)
                }, StorageId);

                WriteVerbose("Adding a server to the shared storage...");
                WriteObject(result);
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes a server from the shared storage.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOSharedStorageServer -StorageId [UUID] -ServerId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOSharedStorageServer")]
    [OutputType(typeof(SharedStoragesResponse))]
    public class RemoveSharedStorageServer : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Storage ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Storage Id", Mandatory = true, ValueFromPipeline = true)]
        public string StorageId { get; set; }

        /// <summary>
        /// <para type="description">Server ID. If this parameters is not passed.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Server Id", ValueFromPipeline = true, Mandatory = false)]
        public string ServerId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var storageApi = client.SharedStorages;
                var resp = storageApi.DeleteSharedStoragesServer(StorageId, ServerId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    #endregion

    #region shared storage access

    /// <summary>
    /// <para type="synopsis">This commandlet returns the credentials for accessing the shared storages.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOSharedStorageAccess</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOSharedStorageAccess")]
    [OutputType(typeof(SharedStorageAccessResponse))]
    public class GetSharedStorageAccess : Cmdlet
    {

        private static OneAndOneClient client;

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Constants.Configuration);
                var storageApi = client.SharedStorages;

                var storageServer = storageApi.ShowSharedStorageAccess();
                WriteObject(storageServer);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet changes the password for accessing the shared storages..</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOSharedStorageAccess -Password [password] </para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOSharedStorageAccess")]
    [OutputType(typeof(SharedStorageAccessResponse))]
    public class SetSharedStorageAccess : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">Shared storage name.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Shared storage access Password.", Mandatory = true, ValueFromPipeline = true)]
        public string Password { get; set; }

        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Constants.Configuration);
            var storageApi = client.SharedStorages;

            var resp = storageApi.UpdateSharedStorageAccess(Password);
            WriteObject(resp);
        }
    }

    #endregion

}
