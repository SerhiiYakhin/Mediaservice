#region usings
using System;using System.ComponentModel.DataAnnotations;
using System.Web;

#endregion
namespace MediaService.PL.Utils.Attributes{    //todo: Make it work or delete    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FileContentAttribute : ValidationAttribute
    {
        private readonly (int maxSize, string[] allowedTypes)[] ValidationRules;

        public FileContentAttribute(params (int maxSize, string[] allowedTypes)[] rules)
        {
            ValidationRules = rules;
        }

        //public override bool IsValid(object value)
        //{
        //    if (value == null)
        //    {
        //        return true;
        //    }
        //    var file = value as HttpPostedFileBase; 
        //    return (value as HttpPostedFileBase).ContentLength <= _maxSize;
        //}

        //public override string FormatErrorMessage(string name)
        //{
        //    return string.Format("The file size should not exceed {0}", _maxSize);
        //}
    }}