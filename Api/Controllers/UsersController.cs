using System;
using System.Linq;
using System.Web.Http;
using Api.Models;
using APIMocker.Configs;
using APIMocker.Controllers;
using APIMocker.Extensiones;

namespace Api.Controllers
{
    public class UsersController : MockAPIController<User>
    {
        public override MockApiConfig<User> Config
        {
             get => MockApiConfig<User>.Make(r => r.userId.ToString(), "Users", Seed: "users.json");
        }

        public override IHttpActionResult Post(User model)
        {
            //Auto-incremental primary key
            var lastId = Store.GetAll()
                              .Select(Config.Key)
                              .Max()
                              .Project(n => Convert.ToInt32(n) + 1);

            model.userId = lastId;
            return base.Post(model);    
        }
    }
}