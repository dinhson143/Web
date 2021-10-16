using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;

namespace Web.ViewModels.Catalog.Congtys
{
    public class CongTyPhieuNhapRequest
    {
        public int Id { get; set; }
        public List<SelectItems> Congtys { get; set; } = new List<SelectItems>();
    }
}