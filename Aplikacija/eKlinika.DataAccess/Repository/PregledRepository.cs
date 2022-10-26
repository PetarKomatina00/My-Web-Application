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
    public class PregledRepository : Repository<Pregled>, IPregledRepository
    {
        private readonly ApplicationDbContext Context;
        public PregledRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(Pregled entity)
        {
            Context.Pregledi.Update(entity);
        }
    }
}
