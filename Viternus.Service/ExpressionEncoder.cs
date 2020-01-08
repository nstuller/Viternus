using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Expression.Encoder;

//This class was used to interface with the Microsoft Expression Encoder 3 SDK.
//It worked pretty well until we found out we need to install the full Expression application on any deployed machine
namespace Viternus.Service
{
    public class ExpressionEncoder
    {
        public static string EncodeToWmv(string filePath, string contentType)
        {
            string outputFileName = String.Empty;

            //We want to ignore Windows Media Files because they don't need encoded (video/x-ms-wmv)
            if ("video/x-ms-wmv" == contentType)
                return filePath;

            //TODO: should try to use the SDK to determine if the filetype can be converted
            //TODO: As we know, there could be an error with certain .mp4 files. Has to do with using .ffdshow?
            if ("video/mp4" == contentType || "video/quicktime" == contentType || "video/mpeg" == contentType || "video/avi" == contentType)
            {
                try
                {
                    //MediaItem mediaItem = new MediaItem(filePath);

                    //create job and media item for video to encode
                    //Job job = new Job();
                    //job.MediaItems.Add(mediaItem);

                    //set output directory
                    //job.OutputDirectory = filePath.Substring(0, filePath.LastIndexOf("Upload") + 7);
                    //job.CreateSubfolder = false;

                    //job.Encode();
                    //outputFileName = mediaItem.ActualOutputFileFullPath;
                }
                catch (/*EncodeError*/Exception eeex)
                {
                    //TODO: Catch only relevant types of exceptions
                }
            }

            return outputFileName;
        }
    }
}
