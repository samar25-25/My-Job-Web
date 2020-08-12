using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using MyJobWeb.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyJobWeb.Startup))]
namespace MyJobWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUserAndRoles();
        }
        public void CreateUserAndRoles()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                //create superAdmin role
                var role = new IdentityRole("SuperAdmin");
                roleManager.Create(role);
                //create Default User
                var user = new ApplicationUser();
                user.UserName = "sa@domain.com";
                user.Email = "sa@domain.com";
                string pwd = "Password@2017";
                var newuser = userManager.Create(user, pwd);
                if (newuser.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, "SuperAdmin");
                }
            }

        }
    }
}
