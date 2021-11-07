﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;

namespace Web.ViewModels.Catalog.Categories
{
    public class CategoryCreate
    {
        public string Name { get; set; }

        public string SeoDescription { get; set; }
        public int? ParentId { get; set; }
        public string SeoTitle { get; set; }
        public string SeoAlias { get; set; }
        public string LanguageId { set; get; }
        public List<SelectItems> Categories { get; set; } = new List<SelectItems>();
    }
}