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
    public class PacijentRepository : Repository<Pacijent>, IPacijentRepository
    {
        private readonly ApplicationDbContext Context;
        public PacijentRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(Pacijent entity)
        {
            Context.Pacijents.Update(entity);
        }
    }
}
