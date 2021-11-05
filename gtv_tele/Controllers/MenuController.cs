using gtv_tele.CustomAuthentication;
using gtv_tele.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;

namespace gtv_tele.Controllers
{    
    public class MenuController : Controller
    {
        AuthenticationDB dbContext = new AuthenticationDB();
        // GET: Menu
        public List<Menus> GetMenus()
        {
            CustomPrincipal auth = (CustomPrincipal)System.Web.HttpContext.Current.User;
            int[] role = auth.RolesId;
            List <Menus> menus = new List<Menus>();
            var quer = dbContext.Database.SqlQuery<MenusRoles>("select * from MenusRoles where Roles_Id="+role[0]);
            var result = from a in quer.ToList()
                         join b in dbContext.Menu on a.Menus_id equals b.id 
                         where b.active == true && b.parent == 0
                         orderby b.order ascending
                         select b;

            menus = result.ToList<Menus>();            
            return menus;
        }

        public List<Menus> SubMenus(int parent)
        {            
            List<Menus> result = dbContext.Menu.Where(a => a.parent == parent).OrderBy(a => a.order).ToList();
            
            return result;
        }
    }
}