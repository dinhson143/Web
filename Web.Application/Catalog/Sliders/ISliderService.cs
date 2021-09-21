using System.Collections.Generic;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Sliders;

namespace Web.Application.Catalog.Sliders
{
    public interface ISliderService
    {
        public Task<List<SliderViewModel>> GetAll();
    }
}