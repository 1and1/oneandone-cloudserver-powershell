using OneAndOne.Client;
using OneAndOne.POCO.Requests.Images;
using OneAndOne.POCO.Response.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace OneAndOne
{

    /// <summary>
    /// <para type="synopsis">This commandlet returns a list of your images, or one image if the imageId is provided, if the ID parameter is provided all the optional paramters are ignored</para>
    /// </summary>
    /// <example>
    /// <para type="description">Get-OAOImage -ImageId [UUID] -Page [page] -PerPage [perpage] -Sort [sort] -Query [q] -Fields [fields]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "OAOImage")]
    [OutputType(typeof(ImagesResponse))]
    public class GetImage : Cmdlet
    {

        private static OneAndOneClient client;
        #region Parameters

        /// <summary>
        /// <para type="description">Image ID. If this parameters is not passed, the commandlet will return a list of all images.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Image Id", ValueFromPipeline = true)]
        public string ImageId { get; set; }

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
                var imageApi = client.Images;

                if (string.IsNullOrEmpty(ImageId))
                {
                    var images = imageApi.Get(Page??null, PerPage ?? null, Sort ?? null, Query ?? null, Fields ?? null);
                    WriteObject(images);
                }
                else
                {
                    var image = imageApi.Show(ImageId);
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
    /// <para type="synopsis">This commandlet will add a new image</para>
    /// </summary>
    /// <example>
    /// <para type="description">New-OAOImage -ServerId [UUID] -Description -[description] -Frequency [frequency] -Name [name] -NumImages [numImages] -DatacetnerId [UUID] -Source [source] -Url [url] -OsId [osId] -Type [type]</para>
    /// </example>
    [Cmdlet(VerbsCommon.New, "OAOImage")]
    [OutputType(typeof(ImagesResponse))]
    public class NewImage : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters

        /// <summary>
        /// <para type="description">The ID of the Image. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "The ID of the Image.", Mandatory = true, ValueFromPipeline = true)]
        public string ServerId { get; set; }

        /// <summary>
        /// <para type="description">Image description.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "Image description.", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Creation policy frequency.</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "Creation policy frequency.", Mandatory = true, ValueFromPipeline = true)]
        public ImageFrequency Frequency { get; set; }

        /// <summary>
        /// <para type="description">Image name.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Image name.", Mandatory = true, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">Maximum number of images.</para>
        /// </summary>
        [Parameter(Position = 4, HelpMessage = "Maximum number of images", Mandatory = true, ValueFromPipeline = true)]
        public int NumImages { get; set; }

        /// <summary>
        /// <para type="description">ID of the datacenter where the shared storage will be created.</para>
        /// </summary>
        [Parameter(Position = 5, HelpMessage = "ID of the datacenter where the shared storage will be created.", Mandatory = false, ValueFromPipeline = true)]
        public string DatacetnerId { get; set; }

        /// <summary>
        /// <para type="description">Source of the new image: server (from an existing server), image (from an imported image) or iso (from an imported iso).</para>
        /// </summary>
        [Parameter(Position = 6, HelpMessage = "Source of the new image: server (from an existing server), image (from an imported image) or iso (from an imported iso).", Mandatory = true, ValueFromPipeline = true)]
        public ImageSource Source { get; set; }

        /// <summary>
        /// <para type="description">URL where the image can be downloaded. It is required when the source is image or iso.</para>
        /// </summary>
        [Parameter(Position = 7, HelpMessage = "URL where the image can be downloaded. It is required when the source is image or iso.", Mandatory = false, ValueFromPipeline = true)]
        public String Url { get; set; }

        /// <summary>
        /// <para type="description">ID of the Operative System you want to import. You can get a list of the available ones with the method /iamges/os..</para>
        /// </summary>
        [Parameter(Position = 8, HelpMessage = "ID of the Operative System you want to import. You can get a list of the available ones with the method /iamges/os..", Mandatory = false, ValueFromPipeline = true)]
        public string OsId { get; set; }

        /// <summary>
        /// <para type="description">Type of the ISO you want to import: os (Operative System) or app (Application).  It is required when the source is iso.</para>
        /// </summary>
        [Parameter(Position = 9, HelpMessage = "Type of the ISO you want to import: os (Operative System) or app (Application).  It is required when the source is iso.", Mandatory = false, ValueFromPipeline = true)]
        public ImageType Type { get; set; }


        #endregion

        protected override void BeginProcessing()
        {
            try
            {

                client = OneAndOneClient.Instance(Helper.Configuration);
                var imageApi = client.Images;
                var result = imageApi.Create(new CreateImageRequest
                {
                    ServerId = ServerId,
                    Description = Description,
                    Frequency = Frequency,
                    Name = Name,
                    NumIimages = NumImages,
                    Source = Source,
                    DatacetnerId = DatacetnerId ?? null,
                    OsId = OsId ?? null,
                    Type = Type,
                    Url = Url ?? null
                });


                WriteVerbose("Creating the image...");

                WriteObject(result);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }

    }

    /// <summary>
    /// <para type="synopsis">This commandlet removes an image.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Remove-OAOImage -ImageId [UUID]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Remove, "OAOImage")]
    [OutputType(typeof(ImagesResponse))]
    public class RemoveImage : Cmdlet
    {

        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Image ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Image Id", Mandatory = true, ValueFromPipeline = true)]
        public string ImageId { get; set; }

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                client = OneAndOneClient.Instance(Helper.Configuration);
                var imageApi = client.Images;
                var resp = imageApi.Delete(ImageId);
                WriteObject(resp);

            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.NotSpecified, null));
            }
        }
    }

    /// <summary>
    /// <para type="synopsis">This commandlet modifies an image.</para>
    /// <para type="synopsis">Only parameters passed in the commandlet will be updated.</para>
    /// </summary>
    /// <example>
    /// <para type="description">Set-OAOImage -ImageId [UUID] -Name [name] -Description [description] -Frequency [frequency]</para>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OAOImage")]
    [OutputType(typeof(ImagesResponse))]
    public class SetImage : Cmdlet
    {
        private static OneAndOneClient client;

        #region Parameters
        /// <summary>
        /// <para type="description">Server ID. Mandatory parameter.</para>
        /// </summary>
        [Parameter(Position = 0, HelpMessage = "Server Id", Mandatory = true, ValueFromPipeline = true)]
        public string ImageId { get; set; }

        /// <summary>
        /// <para type="description">The hostname of the Image.</para>
        /// </summary>
        [Parameter(Position = 1, HelpMessage = "The anme of the Image.", Mandatory = false, ValueFromPipeline = true)]
        public string Name { get; set; }

        /// <summary>
        /// <para type="description">The Description of the Image.</para>
        /// </summary>
        [Parameter(Position = 2, HelpMessage = "The Description of the Image.", Mandatory = false, ValueFromPipeline = true)]
        public string Description { get; set; }

        /// <summary>
        /// <para type="description">Creation policy frequency.</para>
        /// </summary>
        [Parameter(Position = 3, HelpMessage = "Creation policy frequency.", Mandatory = false, ValueFromPipeline = true)]
        public ImageFrequency? Frequency { get; set; }


        #endregion

        protected override void BeginProcessing()
        {

            client = OneAndOneClient.Instance(Helper.Configuration);
            var imageApi = client.Images;
            var image = client.Images.Show(ImageId);

            var resp = imageApi.Update(new UpdateImageRequest { Name = Name ?? null, Description = Description ?? null, Frequency = Frequency ?? image.Frequency }, ImageId);
            WriteObject(resp);
        }
    }
}
