using Web.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public bool IsShowonHome { get; set; }
        public int? ParentId { get; set; }
        public Status Status { get; set; }

        public List<CategoryTranslation> CategoryTranslations { get; set; }
        public List<ProductInCategory> ProductInCategories { get; set; }
    }
}