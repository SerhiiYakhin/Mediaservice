#region usings

using System.Linq;
using System.Web;
using MediaService.BLL.DTO.Enums;

#endregion

namespace MediaService.BLL.BusinessModels
{
    public static class FileValidation
    {
        private static readonly (int maxFileSize, string[] allowedFileTypes, FileType fileType) PicturesAllowRule;

        private static readonly (int maxFileSize, string[] allowedFileTypes, FileType fileType) VidieosAllowRule;

        private static readonly (int maxFileSize, string[] allowedFileTypes, FileType fileType)[] FileValidationRules =
        {
            PicturesAllowRule,
            VidieosAllowRule
        };

        static FileValidation()
        {
            PicturesAllowRule = (2_097_152, new[] {"image/png", "image/jpg", "image/jpeg"}, FileType.Image);
            VidieosAllowRule =
                (52_428_800, new[] {"video/quicktime", "video/x-msvideo", "video/x-matroska"}, FileType.Video);
            FileValidationRules = new[] {PicturesAllowRule, VidieosAllowRule};
        }

        public static bool FileIsValid(HttpPostedFileBase file)
        {
            if (file != null)
            {
                foreach (var fileValidationRule in FileValidationRules)
                {
                    if (fileValidationRule.allowedFileTypes.Contains(file.ContentType))
                    {
                        return fileValidationRule.maxFileSize > file.ContentLength;
                    }
                }
            }
            return false;
        }

        public static FileType GetFileTypeValidation(HttpPostedFileBase file)
        {
            if (file != null)
            {
                foreach (var fileValidationRule in FileValidationRules)
                {
                    if (fileValidationRule.allowedFileTypes.Contains(file.ContentType))
                    {
                        return fileValidationRule.maxFileSize > file.ContentLength
                            ? fileValidationRule.fileType
                            : FileType.Unallowed;
                    }
                }
            }

            return FileType.Unallowed;
        }

        public static FileType GetFileType(HttpPostedFileBase file)
        {
            if (file != null)
            {
                foreach (var fileValidationRule in FileValidationRules)
                {
                    if (fileValidationRule.allowedFileTypes.Contains(file.ContentType))
                    {
                        return fileValidationRule.fileType;
                    }
                }
            }

            return FileType.Unallowed;
        }
    }
}