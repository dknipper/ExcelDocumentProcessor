using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExcelDocumentProcessor.Web.Models
{
    public abstract class UploadModel
    {
        public string UploadCode { get; set; }
        public abstract void UploadFile(HttpPostedFileBase file);
        public abstract string UploadControlName { get; }
        private List<string> _validationErrors;

        [Display(Name = "Validation Errors:")]
        public List<string> ValidationErrors
        {
            get
            {
                _validationErrors = _validationErrors ?? new List<string>();
                return _validationErrors;
            }
            set { _validationErrors = value; }
        }

        protected void ProcessExcelPreFlightErrorMessages(HttpPostedFileBase file)
        {

            if (file == null)
            {
                ValidationErrors.Add(string.Format("Field: \"{0}\" is required.", UploadControlName));
            }
            else
            {
                if (file.ContentLength < 1)
                {
                    ValidationErrors.Add(string.Format("Field: \"{0}\" contains a file that could not be read.", UploadControlName));
                }
                else
                {
                    var extension = file.FileName.Split('.');
                    if (!string.Equals(extension.Last(), "xlsx", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ValidationErrors.Add(string.Format("Field: \"{0}\" contains a file that is not a .xlsx file.", UploadControlName));
                    }
                }
            }
        }
    }
}