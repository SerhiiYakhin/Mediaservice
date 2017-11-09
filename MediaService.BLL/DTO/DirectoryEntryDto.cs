namespace MediaService.BLL.DTO
{
    public sealed class DirectoryEntryDto : ObjectEntryDto
    {
        //public DirectoryEntryDto()
        //{
        //    Discriminator = "DirectoryEntry";
        //}

        public short NodeLevel { get; set; }
    }
}
