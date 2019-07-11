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
                              .Pipe(e => e.Select(k => Convert.ToInt32(k)).Max() + 1);                         

            model.userId = lastId;
            return base.Post(model);    
        }
    }
}