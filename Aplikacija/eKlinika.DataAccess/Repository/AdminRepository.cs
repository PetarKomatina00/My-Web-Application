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
    public class AdminRepository : Repository<Admin>, IAdminRepository
    {
        private readonly ApplicationDbContext Context;
        public AdminRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }
        public void Update(Admin entity)
        {
            Context.Admin.Update(entity);
        }
    }
}
