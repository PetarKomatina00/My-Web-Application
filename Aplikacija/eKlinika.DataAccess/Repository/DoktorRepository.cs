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
    public class DoktorRepository : Repository<Doktor>, IDoktorRepository
    {
        private readonly ApplicationDbContext Context;
        public DoktorRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(Doktor entity)
        {
            Context.Doktori.Update(entity);
        }
    }
}
