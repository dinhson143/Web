using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.Data.Entities
{
    public class Language
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public Status Status { get; set; }

        public bool IsDefault { get; set; }

        public List<CategoryTranslation> CategoryTranslations { get; set; }
        public List<ProductTranslation> ProductTranslations { get; set; }
    }
}