using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebAppOrder.Models;

[assembly: OwinStartupAttribute(typeof(WebAppOrder.Startup))]
namespace WebAppOrder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            // InitUserRole();
           

        }
        private void InitUserRole()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //tao role Admin
            if (!roleManager.RoleExists("Admin")) //chua co moi tao
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                //tao user
                var user = new ApplicationUser();
                user.UserName = "admin@gmail.com";
                string pass = "123456"; //sẽ thay đổi pass khi login
                var chkUser = userManager.Create(user, pass);
                //đưa user trên vào admin
                if (chkUser.Succeeded)
                    userManager.AddToRole(user.Id, "Admin");
            }
        }
    }
}
