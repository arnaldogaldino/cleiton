using System;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Reflection;
using System.Web.Mvc;

namespace ProjectMaster.Core
{
    public class ControllerMaster : System.Web.Mvc.Controller
    {

        protected override void Initialize(RequestContext requestContext)
        {
            if (Context.UsuarioOnLine == null)
            {
                requestContext.HttpContext.Response.Redirect("/Usuario/Entrar");
                return;
            }

            base.Initialize(requestContext);
        }

        protected override ViewResult View(IView view, object model)
        {
            if (string.IsNullOrEmpty(ViewBag.Title))
                ViewBag.Title = "::.. SVC - Sistema de Vendas Ceagesp  ..::";
            else
                ViewBag.Title = "::.. SVC - Sistema de Vendas Ceagesp - " + ViewBag.Title + " ..::";

            return base.View(view, model);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if(string.IsNullOrEmpty(ViewBag.Title))
                ViewBag.Title = "::.. SVC - Sistema de Vendas Ceagesp  ..::";
            else
                ViewBag.Title = "::.. SVC - Sistema de Vendas Ceagesp - " + ViewBag.Title + " ..::";

            base.OnActionExecuted(filterContext);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            if (string.IsNullOrEmpty(ViewBag.Title))
                ViewBag.Title = "::.. SVC - Sistema de Vendas Ceagesp  ..::";
            else
                ViewBag.Title = "::.. SVC - Sistema de Vendas Ceagesp - " + ViewBag.Title + " ..::";

            return base.View(viewName, masterName, model);
        }

        public string Title
        {
            get
            {
                var result = string.Empty;

                if (string.IsNullOrEmpty(this.Title))
                    result = "::.. SVC - Sistema de Vendas Ceagesp  ..::";
                else
                    result = "::.. SVC - Sistema de Vendas Ceagesp - " + this.Title + " ..::";
                
                ViewBag.Title = result;
                
                return result;
            }
            set
            {
                var result = string.Empty;

                if (string.IsNullOrEmpty(value))
                    result = "::.. SVC - Sistema de Vendas Ceagesp  ..::";
                else
                    result = "::.. SVC - Sistema de Vendas Ceagesp - " + value + " ..::";

                ViewBag.Title = result;
            }
        }

        public ControllerMaster()
        {
        }

    }
}
