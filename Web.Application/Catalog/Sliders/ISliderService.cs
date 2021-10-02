using System.Collections.Generic;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Sliders;

namespace Web.Application.Catalog.Sliders
{
    public interface ISliderService
    {
        public Task<List<SliderViewModel>> GetAll();

        public Task<ResultApi<SliderViewModel>> GetSliderById(int sliderId);

        public Task<ResultApi<string>> CreateSlider(SliderCreate request);

        public Task<int> UpdateSlider(SliderUpdateRequest request);

        public Task<int> Delete(int id);
    }
}