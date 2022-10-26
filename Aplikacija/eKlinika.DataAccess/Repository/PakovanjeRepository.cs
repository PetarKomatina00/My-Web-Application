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
    public class PakovanjeRepository : Repository<Pakovanje>, IPakovanjeRepository
    {
        private readonly ApplicationDbContext Context;
        public PakovanjeRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(Pakovanje entity)
        {
            Context.Pakovanja.Update(entity);
        }
    }
}
