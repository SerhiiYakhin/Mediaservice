namespace MediaService.BLL.Models.Enums
{
    public enum OperationType : byte
    {
        None,
        GenerateThumbnail,
        DownloadFiles,
        DownloadFolder,
        DeleteFolder
    }
}