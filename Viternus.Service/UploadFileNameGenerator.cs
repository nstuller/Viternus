using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krystalware.SlickUpload.Providers;
using Krystalware.SlickUpload;
using System.IO;

namespace Viternus.Service
{
    class UploadFileNameGenerator : IFileNameGenerator
    {

        #region IFileNameGenerator Members

        public string GenerateFileName(UploadedFile file)
        {
                //string savedFileDirectory = String.Format("{0}{1}", DateTime.Today.Year.ToString(), DateTime.Today.DayOfYear.ToString());

                string fileName = String.Format("{0}{1}", Guid.NewGuid(), file.ClientName.Substring(file.ClientName.LastIndexOf(".")));

                //return Path.Combine(savedFileDirectory, fileName);
                return fileName;
        }

        #endregion
    }
}
