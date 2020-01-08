using System;
using System.Configuration;
using Viternus.Data.Interface;

namespace Viternus.Data
{
    public partial class Video : IEntity
    {
        //Azure Code Starts Here
        public string AzureUri
        {
            get
            {
                try
                {
                    string suffixURI = this.Path;

                    // Get the base Azure URL from Config
                    string baseUrl = ConfigurationManager.AppSettings["BlobStorageEndpoint"].ToString();
                    var isAzureEnvProduction = ConfigurationManager.AppSettings["ProductionAzureEnv"];
                    if (!bool.Parse(isAzureEnvProduction))
                    {
                        suffixURI = ConfigurationManager.AppSettings["AccountName"].ToString() + "/" + suffixURI;
                    }
                    return String.Format("{0}{1}", baseUrl, suffixURI);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error determining Azure URI: " + ex.Message);
                }
            }
        }
    }
}
