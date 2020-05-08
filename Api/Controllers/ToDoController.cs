using APIMocker.Configs;
using APIMocker.Models.MockModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace APIMocker.Controllers
{
    public class ToDoController : MockAPIController<ToDo>
    {
        public override MockApiConfig<ToDo> Config
        {
            get => MockApiConfig<ToDo>.Make(r => r.Title, "ToDo", Seed: "ToDo.json");
        }

        public override IHttpActionResult Post(ToDo model)
        {
            return base.Post(model);    
        }
    }
}