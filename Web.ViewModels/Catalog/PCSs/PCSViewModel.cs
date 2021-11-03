using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Catalog.PCSs
{
    public class PCSViewModel
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public decimal OriginalPrice { get; set; }
    }
}