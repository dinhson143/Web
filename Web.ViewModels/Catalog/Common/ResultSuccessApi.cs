using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Common
{
    public class ResultSuccessApi<T> : ResultApi<T>
    {
        public ResultSuccessApi(T result)
        {
            IsSuccess = true;
            ResultObj = result;
        }

        public ResultSuccessApi()
        {
            IsSuccess = true;
        }
    }
}