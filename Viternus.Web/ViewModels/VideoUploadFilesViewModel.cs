using Krystalware.SlickUpload.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Viternus.Data;
using Viternus.Data.Repository;

namespace Viternus.Web.ViewModels
{
    public class VideoUploadFilesViewModel
    {
        public AppUser User { get; private set; }
        public UploadStatus SlickUploadStatus { get; private set; }

        public VideoUploadFilesViewModel(string userName, UploadStatus result)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                AppUserRepository userRepository = new AppUserRepository();
                User = userRepository.GetByUserName(userName);
            }

            SlickUploadStatus = result;
        }
    }
}
