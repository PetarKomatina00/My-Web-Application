using eKlinika.DataAccess.Data;
using eKlinika.DataAccess.Repository.IRepository;
using eKlinika.Models;
using eKlinika.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace eKlinika.DataAccess.DatabaseInit
{
    public class DatabaseInit : IDatabaseInit
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext Context;

        public DatabaseInit(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext _context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            Context = _context;
        }

        public void Init()
        {
            try
            {
                if(Context.Database.GetPendingMigrations().Count() > 0)
                {
                    Context.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }

            if (!_roleManager.RoleExistsAsync(SD.Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Doktor)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Direktor)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Pacijent)).GetAwaiter().GetResult();

                var x = _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "tobygames855@gmail.com",
                    Ime = "Petar",
                    Prezime = "Komatina",
                    Email = "tobygames855@gmail.com",
                    BrojTelefona = "0665068555",
                    Adresa = "Bulevar Nemanjica 76/24",
                    NazivGrada = "Nis",
                    PostanskiBroj = "18000"
                }, "Admin123!").GetAwaiter().GetResult();

                ApplicationUser officialAdmin = Context.Users.FirstOrDefault(x => x.UserName == "tobygames855@gmail.com");
                _userManager.AddToRoleAsync(officialAdmin, SD.Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
