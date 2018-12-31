using APIMocker.Controllers;
using APIMocker.Models;
using ServiceCore;
using ServiceCore.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Api.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        public ActionResult Index()
        {            
            var model = new HomeIndexViewModel();

            model.Version = "1.0";
            model.ControllerActions = GetControllerActions();

            return View(model);
        }

        public List<ControllerActionElement> GetControllerActions()
        {
            var asm = Assembly.GetExecutingAssembly();

            var elements = asm.GetTypes()
                    .Where(type => typeof(MockAPIController<>).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods())
                  //  .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new ControllerActionElement(x))
                    .OrderBy(x => x.Controller)
                    .ThenBy(x => x.Action)
                    .ToList();

            return elements;
        }
    }
}
