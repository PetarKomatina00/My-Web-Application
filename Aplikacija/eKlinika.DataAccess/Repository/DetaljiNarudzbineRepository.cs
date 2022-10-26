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
    public class DetaljiNarudzbineRepository : Repository<DetaljiNarudzbine>, IDetaljiNarudzbineRepository
    {
        private readonly ApplicationDbContext Context;
        public DetaljiNarudzbineRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(DetaljiNarudzbine entity)
        {
            Context.DetaljiNarudzbine.Update(entity);
        }
    }
}
