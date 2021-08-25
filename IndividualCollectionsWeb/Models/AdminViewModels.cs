using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndividualCollectionsWeb.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool Blocked { get; set; }
        public bool IsAdministrator { get; set; }
    }
}
