using Api.Models;
using APIMocker.Configs;
using APIMocker.Controllers;

namespace Api.Controllers
{
    public class UsersController : MockAPIController<User>
    {
        public override MockApiConfig<User> Config
        {
             get => MockApiConfig<User>.Make(r => r.id.ToString(), "Users", Seed: "users.json");
        }
    }
}