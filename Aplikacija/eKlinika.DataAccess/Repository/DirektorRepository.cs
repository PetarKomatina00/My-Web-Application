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
    public class DirektorRepository : Repository<Direktor>, IDirektorRepository
    {
        private ApplicationDbContext Context;

        public DirektorRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(Direktor entity)
        {
            Context.Direktor.Update(entity);
        }
    }
}
