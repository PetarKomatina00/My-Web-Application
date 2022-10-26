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
    public class GradRepository : Repository<Grad>, IGradRepository
    {
        private readonly ApplicationDbContext Context;
        public GradRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(Grad entity)
        {
            Context.Gradovi.Update(entity);
        }
    }
}
