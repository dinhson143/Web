using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.ViewModels.Catalog.Congtys
{
    public class CongtyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Masothue { get; set; }
        public string Diachi { get; set; }
        public string Sdt { get; set; }
        public Status Status { get; set; }
    }
}