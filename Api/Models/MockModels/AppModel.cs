using Api.Models;
using System.Collections.Generic;

namespace APIMocker.Models.MockModels
{
    public class AppModel
    {
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<RoleModel> Roles { get; set; }
    }
}