using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Viternus.DeliveryAutomation;

namespace Viternus.Service
{
    public class FFmpegEncoder
    {
        public static string EncodeToWmv(string filePath, string contentType)
        {
            string outputFileName = String.Empty;

            try
            {
                //We want to ignore Windows Media Files because they don't need encoded (video/x-ms-wmv)
                if ("video/x-ms-wmv" == contentType)
                    return filePath;

                outputFileName = filePath.Substring(0, filePath.LastIndexOf(".") + 1) + "wmv";

                //-i "myfile.mpg"  -vcodec wmv2 -acodec wmav2 "myfile.wmv"
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.Arguments = String.Format(@" -i ""{0}"" -vcodec wmv2 -acodec wmav2 ""{1}"" ", filePath, outputFileName);
                psi.FileName = filePath.Substring(0, filePath.LastIndexOf("Upload")) + @"bin\ffmpeg.exe";
                //psi.RedirectStandardError = true;
                //psi.RedirectStandardOutput = true;
                //psi.RedirectStandardInput = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                Process ffmpeg = Process.Start(psi);

                //StreamReader outputReader = ffmpeg.StandardOutput;
                //StreamReader errorReader = ffmpeg.StandardError;
                //StreamWriter inputWrite = ffmpeg.StandardInput;

                while(!ffmpeg.HasExited)
                {
                    ffmpeg.WaitForExit(3000);

                    //string thumbFile = Page.MapPath(@"~\Lib\ffmpeg.rev12665\tmp") +
                    //    @"\" + temp.ToString() + ".jpg";
                    // ...
                }
            }
            catch (Exception ex)
            {
                outputFileName = String.Empty;

                System.Net.Mail.MailMessage mailItem = new System.Net.Mail.MailMessage();
                mailItem.To.Add("Stullern@Yahoo.com");
                mailItem.From = new System.Net.Mail.MailAddress("MessageDelivery@Viternus.com");
                mailItem.Subject = "Error";
                mailItem.Body = ex.Message;

                Emailer.SendEmail(mailItem, null, EmailEntityType.None);
            }

            //Guid temp = Guid.NewGuid();

            //// just throw our ffmpeg commands at cmd.exe
            //psi.WorkingDirectory = Page.MapPath(@"~\Lib\ffmpeg.rev12665");

            //// uses extra cheap logging facility
            //inputWrite.WriteLine("echo \"Ripping " + copiedFile + " " +
            //    temp.ToString() + "\" >> log.txt");

            //inputWrite.WriteLine("ffmpeg.exe -i \"" + copiedFile +
            //    "\" -f image2 -vframes 1 -y -ss 2 tmp\\" + temp.ToString() +
            //    ".jpg");

            //inputWrite.WriteLine("exit");

            return outputFileName;
        }
    }
}
