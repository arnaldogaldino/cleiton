using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using System.Data.Objects.DataClasses;

namespace ProjectMaster.Bussiness
{
    public class Usuario
    {
        private PMEntities entities = new PMEntities();

        public pm_usuario GetUsuarioByUsuarioSenha(string user, string password)
        {
            return (from u in entities.pm_usuario
                   where u.usuario.Equals(user, StringComparison.OrdinalIgnoreCase) &&
                         u.senha.Equals(password, StringComparison.OrdinalIgnoreCase) &&
                         u.dm_status.Equals("A") && 
                         u.excluido == false
                  select u).FirstOrDefault();
        }

        public pm_usuario GetUsuarioByIdUsuario(long id_usuario)
        {
            return (from u in entities.pm_usuario
                    where u.id_usuario.Equals(id_usuario)
                    select u).FirstOrDefault();
        }

        public string GetMenuByIdUsuario(long id_usuario)
        {
            string result = string.Empty;

            var usuario = (from u in entities.pm_usuario
                           where u.id_usuario.Equals(id_usuario)
                           select u).FirstOrDefault();

            return GetMontarMenu(usuario.pm_menu.Where(m => m.id_menu_pai == null), id_usuario);
        }

        private string GetMontarMenu(IEnumerable<pm_menu> mnu, long id_usuario)
        {
            string result = string.Empty;

            foreach (var menu in mnu)
            {
                if (menu.pm_menu2 == null)
                {
                    string url = string.Empty;

                    if (string.IsNullOrEmpty(menu.area_name))
                        url = (!string.IsNullOrEmpty(menu.action_name) ? "/" + menu.controller_name + "/" + menu.action_name : "");
                    else
                        url = (!string.IsNullOrEmpty(menu.action_name) ? "/" + menu.area_name + "/" + menu.controller_name + "/" + menu.action_name : "");

                    if (menu.pm_usuario.Select(o => o.id_usuario == id_usuario).Count() > 0)
                    {
                        result += "<li class='top'><a href='" + url + "' class='top_link'><span>" + menu.texto + "</span></a>";
                        if (menu.pm_menu1.Count > 0)
                        {
                            result += "<ul class='sub'>";
                            result += GetMontarMenu(menu.pm_menu1, id_usuario);
                            result += "</ul>";
                        }
                        result += "</li>";
                    }
                }
                else
                {
                    if (menu.pm_menu1.Count > 0)
                    {
                        string url = string.Empty;

                        if (string.IsNullOrEmpty(menu.area_name))
                            url = (!string.IsNullOrEmpty(menu.action_name) ? "/" + menu.controller_name + "/" + menu.action_name : "");
                        else
                            url = (!string.IsNullOrEmpty(menu.action_name) ? "/" + menu.area_name + "/" + menu.controller_name + "/" + menu.action_name : "");

                        if (menu.pm_usuario.Select(o => o.id_usuario == id_usuario).Count() > 0)
                        {
                            result += "<li><a href='" + url + "' class='fly'>" + menu.nome + "</a><ul>";
                            result += GetMontarMenu(menu.pm_menu1, id_usuario);
                            result += "</ul></li>";
                        }
                    }
                    else
                    {
                        string url = string.Empty;

                        if (string.IsNullOrEmpty(menu.area_name))
                            url = (!string.IsNullOrEmpty(menu.action_name) ? "/" + menu.controller_name + "/" + menu.action_name : "");
                        else
                            url = (!string.IsNullOrEmpty(menu.action_name) ? "/" + menu.area_name + "/" + menu.controller_name + "/" + menu.action_name : "");
                        
                        if (menu.pm_usuario.Select(o => o.id_usuario == id_usuario).Count() > 0)
                        {
                            result += "<li><a href='" + url + "'>" + menu.nome + "</a></li>";
                        }
                    }
                }
            }

            return result;
        }

        public IQueryable<pm_domain> GetDomain()
        {
            return (from u in entities.pm_domain
                  select u);
        }
    }
}
