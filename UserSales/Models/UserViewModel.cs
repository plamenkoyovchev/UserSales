using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserSales.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}