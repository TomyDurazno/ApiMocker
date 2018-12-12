using System.Linq;
using System.Web.Http;
using APIMocker.Configs;
using APIMocker.Models.MockModels;

namespace APIMocker.Controllers
{
    public class RoleController : MockAPIController<RoleModel>
    {
        public override MockApiConfig<RoleModel> Config
        {
            get => MockApiConfig<RoleModel>.Make(r => r.id, "Roles", Seed: "roles.json");
        }

        /// <summary>
        /// redefined GET, 
        /// using object references, you can mutate state only by accesor, and then re-set it.
        /// SWEET !
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get(string id, string type)
        {
            var state = Store.State;

            var item = state.FirstOrDefault(s => s.id == id);

            item.type = type;

            Store.State = state;

            return Ok(Store.State);
        }
    }
}
