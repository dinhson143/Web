﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Entities
{
    public class Color
    {
        public int Id { get; set; }
        public string Mamau { get; set; }
        public string Name { get; set; }
        public List<Product_Size> PCS { get; set; }
    }
}