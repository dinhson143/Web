using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Sizes;

namespace Web.Application.Catalog.Sizes
{
    public interface ISizeService
    {
        public Task<List<SizeViewModel>> GetAll();
    }
}