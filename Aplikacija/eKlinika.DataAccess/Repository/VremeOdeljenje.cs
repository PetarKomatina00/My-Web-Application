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
    public class VremeRepository : Repository<Vreme>, IVremeRepository
    {
        private readonly ApplicationDbContext Context;
        public VremeRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }
        public void Update(Vreme entity)
        {
            Context.Vremena.Update(entity);
        }
    }
}
