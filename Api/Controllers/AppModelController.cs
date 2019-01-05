using APIMocker.Configs;
using APIMocker.Models.MockModels;
using ServiceCore.Recursos;
using System.Web.Http;
using System;
using System.Linq;

namespace APIMocker.Controllers
{
    public class AppModelController : BaseModelMockAPIController<AppModel>
    {
        public override MockModelAPIConfig Config => MockModelAPIConfig.Make("AppModel", "AppModel.json");

        public IHttpActionResult GetUsers()
        {
            return Ok(Store.State.Users);
        }

        [Route("api/model")]
        [HttpGet]
        public IHttpActionResult Model()
        {
            return Ok(Store.State);
        }

        [Route("setnames")]
        public IHttpActionResult SetNames()
        {
            var state = Store.State;

            foreach (var user in state.Users)
            {
                user.lastName = user.lastName + Guid.NewGuid().ToString();
            }

            Store.State = state;

            return Ok(state.Users.Select(u => u.username));
        }
    }
}
