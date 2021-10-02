using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Sliders
{
    public class SliderCreate
    {
        public string Name { set; get; }
        public string Description { set; get; }
        public string Url { set; get; }
        public IFormFile Image { set; get; }
    }
}