using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Common
{
    public class ResultErrorApi<T> : ResultApi<T>
    {
        public string[] ValidationErrors { get; set; }

        public ResultErrorApi()
        {
        }

        public ResultErrorApi(string message)
        {
            IsSuccess = false;
            Message = message;
        }

        public ResultErrorApi(string[] validationErrors)
        {
            IsSuccess = false;
            ValidationErrors = validationErrors;
        }
    }
}