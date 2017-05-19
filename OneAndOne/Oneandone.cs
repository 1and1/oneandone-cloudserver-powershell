using OneAndOne.Client;
using OneAndOne.Client.RESTHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Security;
using System.Text;

namespace OneAndOne
{
    /// <summary>
    /// <para type="synopsis">This is the cmdlet which sets 1&1 credentials.</para>
    /// </summary>
    /// <example>
    ///   <code>
    ///   $credentials = Get-Credential -Message [message text] -UserName [user_name] 
    ///   Set-OneAndOne -Credential $credential
    ///   </code>
    /// </example>
    [Cmdlet(VerbsCommon.Set, "OneAndOne")]
    [Alias("Set OneAndOne Token")]
    public class SetOneAndOne : Cmdlet
    {
        #region Parameters

        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, HelpMessage = "1&1 Authentication Token")]
        public PSCredential Credential { get; set; }

        static OneAndOneClient client;

        #endregion

        protected override void BeginProcessing()
        {
            try
            {
                var config = new Configuration
                {
                    ApiKey= Helper.SecureStringToString(Credential.Password)
                };
                client = OneAndOneClient.Instance(config);


                var appliances = client.ServerAppliances.Get();
                Helper.Configuration = config;


                WriteObject("Authorization successful");
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(new Exception("Authentication failed"), ex.Message.ToString(), ErrorCategory.AuthenticationError, null));
            }

        }
    }
}
