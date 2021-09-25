using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public DateTime Dob { get; set; }

        public List<Cart> Carts { get; set; }
        public List<Order> Orders { get; set; }

        //public List<Transaction> Transactions { get; set; }
        public List<Comment> Comments { get; set; }
    }
}