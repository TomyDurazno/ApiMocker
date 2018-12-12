using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIMocker.Models
{
    public class HomeIndexViewModel
    {
        public string Version { get; set; }

        public List<ControllerActionElement> ControllerActions { get; set; }
    }
}