using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Samples.ServiceHosting.StorageClient;
using System.Collections.Specialized;
using Krystalware.SlickUpload;

namespace Viternus.Service
{
    public class AzureUtility
    {
        public static BlobProperties CollectBlobMetadata(UploadedFile file, string fileNameOnly, string userName)
        {
            // Create metadata to be associated with the blob
            NameValueCollection metadata = new NameValueCollection();
            string desc = file.FormValues["fileDescription"];
            metadata["FileDescription"] = string.IsNullOrEmpty(desc) ? "{empty}" : desc;
            metadata["Submitter"] = userName;
            BlobProperties properties = new BlobProperties(fileNameOnly);
            properties.Metadata = metadata;
            
            //This will always be .wmv because we just encoded it
            properties.ContentType = "video/x-ms-wmv";
            return properties;
        }

        public  static BlobContainer GetAzureContainer(string containerName)
        {
            StorageAccountInfo accountInfo = StorageAccountInfo.GetAccountInfoFromConfiguration("BlobStorageEndpoint");
            BlobStorage blobStorage = BlobStorage.Create(accountInfo);
            //The default timeout of 30 seconds is far too short, make it 6 hours.
            blobStorage.Timeout = new TimeSpan(6, 0, 0);

            if (String.IsNullOrEmpty(containerName))
            {
                // Default name for new container; Container names have the same restrictions as DNS names
                containerName = String.Format("media{0}{1}", DateTime.Now.Year, DateTime.Now.DayOfYear);
            }
            else
            {
                //We have received a path from a media file
                containerName = containerName.Substring(0, containerName.IndexOf("/"));
            }

            BlobContainer container = blobStorage.GetBlobContainer(containerName);
            //If the Container already exists, false is returned, so go get it
            if (!container.DoesContainerExist())
                container.CreateContainer(null, ContainerAccessControl.Private);
            return container;
        }
    }
}
