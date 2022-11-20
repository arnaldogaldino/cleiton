using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Reflection;

namespace ProjectMaster.Core
{
    public class Context
    {
        public static dynamic UsuarioOnLine
        {
            get
            {
                if(HttpContext.Current.Session["UsuariosOnLine"] != null)
                    return ((UsuarioContext)HttpContext.Current.Session["UsuariosOnLine"]).Usuario;

                return null;
            }
        }

        public static dynamic EmpresaLogin
        {
            get
            {
                if (HttpContext.Current.Session["UsuariosOnLine"] != null)
                    return ((UsuarioContext)HttpContext.Current.Session["UsuariosOnLine"]).Empresa;

                return null;
            }
        }

        public static dynamic FilialLogin
        {
            get
            {
                if (HttpContext.Current.Session["UsuariosOnLine"] != null)
                    return ((UsuarioContext)HttpContext.Current.Session["UsuariosOnLine"]).Filial;

                return null;
            }
        }

        public static long idFilial
        {
            get
            {
                return (long)Context.FilialLogin.id_filial;
            }
        }

        public static dynamic Domain
        {
            get
            {
                if (HttpContext.Current.Session["UsuariosOnLine"] != null)
                    return ((UsuarioContext)HttpContext.Current.Session["UsuariosOnLine"]).Domain;

                return null;
            }
        }

        public static string GetMenuHTML
        {
            get
            {
                return ((UsuarioContext)HttpContext.Current.Session["UsuariosOnLine"]).MenuHTML;
            }
        }

        public static void Logoff()
        {
            HttpContext.Current.Session["UsuariosOnLine"] = null;
            HttpContext.Current.Session.Remove("UsuariosOnLine");
        }

        //private static Dictionary<string, UsuarioContext> UsuariosOnLine = new Dictionary<string, UsuarioContext>();

        public static void AddUsuarioOnLine(UsuarioContext usuario)
        {
            HttpContext.Current.Session["UsuariosOnLine"] = usuario;
        }

        public class UsuarioContext
        {
            public dynamic Usuario;
            public dynamic Empresa;
            public dynamic Filial;
            public string MenuHTML;
            public dynamic Domain;
        }

        public static string GetPastaLocal()
        {
           return HttpContext.Current.Server.MapPath(@"~\");
        }
    }
}
