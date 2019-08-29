using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistem_Ventas.Library
{
    public class UsersRoles : ListObject
    {
        private List<SelectListItem> _userRoles;

        public UsersRoles()
        {
            _userRoles = new List<SelectListItem>();
        }

        public async Task<List<SelectListItem>> GetRole(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, string Id)
        {
            var users = await userManager.FindByIdAsync(Id);
            var roles = await userManager.GetRolesAsync(users);

            // De esta forma comprueba que tenga o no tenga roles asignados
            if(roles.Count.Equals(0))
            {
                _userRoles.Add(new SelectListItem
                {
                    Value = "0",
                    Text = "No role"
                });
            }
            else
            {
                var roleUser = roleManager.Roles.Where(m => m.Name.Equals(roles[0]));
                if(!roleUser.Equals(0))
                {
                    foreach (var Data in roleUser)
                    {
                        _userRoles.Add(new SelectListItem
                        {
                            Value = Data.Id,
                            Text = Data.Name
                        });
                    }
                }
                
            }

            return _userRoles;
        }
    }
}