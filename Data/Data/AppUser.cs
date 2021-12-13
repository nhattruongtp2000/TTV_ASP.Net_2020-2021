using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Data.Data
{
    public class AppUser :  IdentityUser
    {
        public string Name { get; set; }

        public string Address { get; set; }
    }
}
