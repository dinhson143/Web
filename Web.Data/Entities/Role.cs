using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Enums;

namespace Web.Data.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public string Description { get; set; }
        public Status Status { get; set; }
    }
}