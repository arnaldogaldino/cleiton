using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectMaster.Data;
using System.Data;

namespace ProjectMaster.Bussiness
{
    public class Menu
    {
        private PMEntities entities = new PMEntities();

        public IQueryable<pm_menu> GetMenuGrid()
        {            
            return (from m in entities.pm_menu
                    select m);
        }

        public pm_menu GetMenuByID(long id_menu)
        {
            return (from m in entities.pm_menu
                    where m.id_menu == id_menu
                    orderby m.id_menu_pai, m.id_menu
                    select m).FirstOrDefault();
        }
        
        public void DeleteMenu(long id_menu)
        {
            pm_menu menu = entities.pm_menu.First(i => i.id_menu == id_menu);

            entities.pm_menu.Detach(menu);
            //entities.pm_menu.DeleteObject(menu);
            entities.SaveChanges();
        }
        
        public bool MenuCadastrar(ref pm_menu adoMenu)
        {
            try
            {
                entities.AddTopm_menu(adoMenu);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool MenuEditar(ref pm_menu adoMenu)
        {
            try
            {
                EntityKey key = entities.CreateEntityKey("pm_menu", adoMenu);
                object originalItem;

                if (entities.TryGetObjectByKey(key, out originalItem))
                {
                    entities.ApplyCurrentValues(key.EntitySetName, adoMenu);
                }
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

        public bool MenuExcluir(pm_menu adoMenu)
        {
            try
            {
                entities.DeleteObject(adoMenu);
                entities.SaveChanges();
            }
            catch { return false; }

            return true;
        }

    }
}
