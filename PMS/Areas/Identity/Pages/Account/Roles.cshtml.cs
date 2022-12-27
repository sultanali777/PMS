using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PMS.Areas.Identity.Pages.Account
{
    public class RolesModel : PageModel
    {
        public List<IdentityRole> Roles { get; set; }
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            this.Roles = new List<IdentityRole>();
        }
        public async Task OnGet()
        {
            Roles = await _roleManager.Roles.ToListAsync();
        }
        public async Task OnPostAsync(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            await OnGet();
        }
    }
}
