using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.Common
{
    public class SelectItems
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}