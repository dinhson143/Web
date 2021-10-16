using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Common
{
    public class ResultApi<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T ResultObj { get; set; }
    }
}