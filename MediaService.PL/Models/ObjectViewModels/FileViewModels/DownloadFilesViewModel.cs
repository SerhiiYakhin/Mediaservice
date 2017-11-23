using System;
using System.Collections.Generic;

namespace MediaService.PL.Models.ObjectViewModels.FileViewModels
{
    public class DownloadFilesViewModel
    {
        public IEnumerable<Guid> FilesIds { get; set; }
    }
}