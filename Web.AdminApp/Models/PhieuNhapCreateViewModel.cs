using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Congtys;

namespace Web.AdminApp.Models
{
    public class PhieuNhapCreateViewModel
    {
        public List<CongtyViewModel> Congtys { get; set; }
        public string CurrentCongtyId { get; set; }
    }
}