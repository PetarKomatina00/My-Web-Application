using eKlinika.DataAccess.Data;
using eKlinika.DataAccess.Repository.IRepository;
using eKlinika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext Context;
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(ApplicationUser entity)
        {
            Context.Users.Update(entity);
        }
    }
}
