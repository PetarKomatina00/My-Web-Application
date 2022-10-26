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
    public class OdeljenjeRepository : Repository<Odeljenje>, IOdeljenjeRepository
    {
        private readonly ApplicationDbContext Context;
        public OdeljenjeRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(Odeljenje entity)
        {
            Context.Odeljenja.Update(entity);
        }
    }
}
