using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Krystalware.SlickUpload;
using Krystalware.SlickUpload.Controls;
using Krystalware.SlickUpload.Status;
using Viternus.Data;
using Viternus.Data.Repository;
using Viternus.Service;
using Microsoft.Samples.ServiceHosting.StorageClient;
using System.Net.Mime;
using Viternus.Web.Filters;
using System.Web.UI;
using Viternus.Web.ViewModels;

namespace Viternus.Web.Controllers
{
    [LogError]
    public class VideoController : ApplicationController
    {
        private const int COMPLETE_PERCENTAGE = 100;

        private VideoRepository _videoRepository = new VideoRepository();
        private NotificationRepository _notificationRepository = new NotificationRepository();
        delegate string ProcessTask(UploadStatus status, decimal total);

        //
        // GET: /Video/

        [Authorize()]
        public ActionResult Index()
        {
            return View(_videoRepository.FindAllVideosByUser(User.Identity.Name).ToList());
        }

        [Authorize]
        public ActionResult UploadFiles()
        {
            return View(new VideoUploadFilesViewModel(User.Identity.Name, null));
        }

        [Authorize]
        public ActionResult UploadResult()
        {
            UploadStatus status = SlickUpload.GetUploadStatus();
            decimal totalCount = (decimal)status.GetUploadedFiles().Count;

            if (status != null && status.State == UploadState.Complete && 0 < totalCount)
            {
                //Post Processing Step: Update Screen to 0% Done
                UpdateProgressBarOnScreen(status, 0, false);

                //At this point, we know the file has been uploaded to the server
                //I think we can just kick off an async process to do the rest.
                ProcessTask processTask = new ProcessTask(this.PostProcessVideo);
                processTask.BeginInvoke(status, totalCount, null, processTask);

                UpdateProgressBarOnScreen(status, COMPLETE_PERCENTAGE, true);
            }
            else
            {
                ErrorLogRepository errorRepos = new ErrorLogRepository();
                errorRepos.SaveErrorToDB(null, "Video upload had issues. Upload Status = " + status.State.ToString() + " totalCount = " + totalCount.ToString() + ": " + Environment.NewLine + status.Reason + Environment.NewLine + status.ErrorMessage, User.Identity.Name);

                return View("UploadFiles", new VideoUploadFilesViewModel(User.Identity.Name, status));
            }

            return RedirectToAction("UploadPending", new { contentLength = status.ContentLength });
        }

        [EmailErrorAttribute]
        private string PostProcessVideo(UploadStatus status, decimal totalCount)
        {
            string resultMessage = String.Empty;
            string localFilePath = String.Empty;
            int counter = 0;

            //Setup the Azure Storage Container
            BlobContainer container = AzureUtility.GetAzureContainer(null);

            foreach (UploadedFile file in status.GetUploadedFiles())
            {
                try
                {
                    localFilePath = file.LocationInfo["fileName"].ToString();

                    //Save a skeleton record of the upload in the database
                    Video vid = _videoRepository.New();
                    _videoRepository.AddVideoToUser(vid, User.Identity.Name);
                    vid.Description = file.FormValues["fileDescription"];
                    vid.StartProcessingDate = DateTime.Now;
                    vid.OriginalFileFormat = file.ContentType;
                    vid.UploadSize = file.ContentLength;
                    _videoRepository.Save();
                    Guid videoId = vid.Id;

                    string encodedFilePath = FFmpegEncoder.EncodeToWmv(localFilePath, file.ContentType);

                    if (!String.IsNullOrEmpty(encodedFilePath))
                    {
                        string fileNameOnly = encodedFilePath.Substring(encodedFilePath.LastIndexOf("Upload") + 7);

                        //TODO: Take a screenshot/Thumbnail of the video & upload it

                        BlobProperties properties = AzureUtility.CollectBlobMetadata(file, fileNameOnly, User.Identity.Name);

                        // Create the blob & Upload
                        long finalSize = 0;
                        using (FileStream uploadedFile = new FileStream(encodedFilePath, FileMode.Open, FileAccess.Read))
                        {
                            BlobContents fileBlob = new BlobContents(uploadedFile);
                            finalSize = fileBlob.AsStream.Length;
                            container.CreateBlob(properties, fileBlob, true);
                        }

                        //Create the database record for this video
                        vid = _videoRepository.GetById(videoId);
                        vid.CreationDate = DateTime.Now;
                        vid.FinalSize = finalSize;
                        vid.Path = String.Format("{0}/{1}", container.ContainerName, fileNameOnly);
                        _videoRepository.Save();

                        resultMessage = string.Format("Your video ({0}) is ready for you to use.", file.FormValues["fileDescription"]);

                        //Delete the local copy of the encoded file
                        System.IO.File.Delete(encodedFilePath);
                    }
                    else
                    {
                        resultMessage = "ERROR: video (" + file.FormValues["fileDescription"] + ") could not be converted to a recognizable video format.";
                    }

                    //Create a notification record so the user knows that processing is done for this video
                    Notification note = _notificationRepository.New();
                    _notificationRepository.AddNotificationToUser(note, User.Identity.Name);
                    note.UserNotified = false;
                    note.Message = resultMessage;
                    note.CreationDate = DateTime.Now;
                    _notificationRepository.Save();
                }
                catch (Exception ex)
                {
                    resultMessage = string.Format("ERROR: we tried to process the video, {0}, but it did not finish.  You might want to try again.", file.FormValues["fileDescription"]);

                    //Create a notification record so the user knows that there was an error
                    Notification note = _notificationRepository.New();
                    _notificationRepository.AddNotificationToUser(note, User.Identity.Name);
                    note.UserNotified = false;
                    note.Message = resultMessage;
                    note.CreationDate = DateTime.Now;
                    _notificationRepository.Save();

                    throw new Exception(resultMessage, ex);
                }
                finally
                {
                    //Delete the local copy of the original file
                    System.IO.File.Delete(localFilePath);

                    counter++;
                }
            }

            return resultMessage;
        }

        private static void UpdateProgressBarOnScreen(UploadStatus status, double percentComplete, bool isComplete)
        {
            percentComplete *= 100;

            Dictionary<string, string> postStatus = new Dictionary<string, string>();
            postStatus["percentComplete"] = percentComplete.ToString();
            postStatus["percentCompleteText"] = ((int)Math.Round(percentComplete, 2)).ToString() + "%";

            // Update the progress context 
            status.UpdatePostProcessStatus(postStatus, isComplete);
        }

        [Authorize]
        public ActionResult UploadPending(long contentLength)
        {
            //Estimate how long, based on upload time or size
            //Right now it seems like it takes 1 min for every 4 MB to upload
            //TODO: We can just assume that every file will be encoded (and tack on that time estimate)
            ViewData["EstimatedArrivalSpan"] = contentLength / 2000000 + 1;

            return View();
        }

        [Authorize()]
        public ActionResult Test()
        {
            return View();
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TestUpload()
        {
            //DO UPload Logic
            try
            {
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[file];
                    if (hpf.ContentLength == 0)
                        continue;
                    //string savedFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(hpf.FileName)); 
                    string savedFileName = Path.Combine(HttpContext.Server.MapPath("../Upload"), System.Guid.NewGuid().ToString());
                    savedFileName = Path.Combine(savedFileName, Path.GetFileName(hpf.FileName));

                    hpf.SaveAs(savedFileName);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Test", ex);
            }
        }

        [OutputCache(VaryByParam = "id", Duration = 2500000, Location = OutputCacheLocation.Server)]
        public FileStreamResult StreamPrivate(Guid id)
        {
            string videoSecurity = TempData["VideoSecurity"] as string;
            if (!("VideoEdit" == videoSecurity || "MessageFromUrl" == videoSecurity || "MessagePreview" == videoSecurity))
                throw new Exception("Video request is not authorized.");

            Video vid = _videoRepository.GetById(id);

            BlobContainer container = AzureUtility.GetAzureContainer(vid.Path);

            BlobContents contents = new BlobContents(new MemoryStream());
            BlobProperties blob = container.GetBlob(vid.Path.Substring(vid.Path.IndexOf("/")+1), contents, false);
            if (blob.ContentType == null)
            {
                throw new FormatException("No content type set for blob.");
            }

            Stream stream = contents.AsStream;
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, blob.ContentType);
        }

        [Authorize]
        public ActionResult Edit(Guid id)
        {
            Video vid = _videoRepository.GetById(id);
            TempData["VideoSecurity"] = "VideoEdit";

            if (null == vid)
                return View("NotFound");
            else
                return View(vid);
        }

        [Authorize()]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Guid id, FormCollection collection)
        {
            Video vid = null;
            try
            {
                vid = _videoRepository.GetById(id);
                vid.Description = Request.Form["Description"];
                _videoRepository.Save();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(vid);
            }
        }

        [Authorize]
        public ActionResult Delete(Guid id)
        {
            Video vid = _videoRepository.GetById(id);

            if (null == vid)
                return View("NotFound");
            else
                return View(vid);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Guid id, string confirmButton)
        {
            Video vid = _videoRepository.GetById(id);

            if (null == vid)
                return View("NotFound");

            //Should also delete the physical file on the server
            BlobContainer container = AzureUtility.GetAzureContainer(vid.Path);
            container.DeleteBlob(vid.Path.Substring(vid.Path.IndexOf("/") + 1));

            _videoRepository.Delete(vid);
            _videoRepository.Save();

            return View("Deleted");
        }


    }
}
