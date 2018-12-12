using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIMocker.Models.MockModels
{
    public class RoleModel
    {
        public string id { get; set; }
        public string role { get; set; }

        public int usercount { get; set; }

        public string type { get; set; }
    }
}