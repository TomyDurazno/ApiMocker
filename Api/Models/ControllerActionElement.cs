using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace APIMocker.Models
{
    public class ControllerActionElement
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ReturnType { get; set; }
        public string Attributes { get; set; }

        public ControllerActionElement(MethodInfo x)
        {
            Controller = x.DeclaringType.Name;
            Action = x.Name;
            ReturnType = x.ReturnType.Name;
            Attributes = string.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")));
        }
    }
}