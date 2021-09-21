using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Sliders;

namespace Web.ServiceApi_Admin_User.Service.Sliders
{
    public interface ISliderApi
    {
        public Task<ResultApi<List<SliderViewModel>>> GetAll();
    }
}