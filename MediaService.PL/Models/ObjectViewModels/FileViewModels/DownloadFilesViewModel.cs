#region usings

using System;
using System.Collections.Generic;

#endregion

namespace MediaService.PL.Models.ObjectViewModels.FileViewModels
{
    public class DownloadFilesViewModel
    {
        public IEnumerable<Guid> FilesIds { get; set; }
    }
}