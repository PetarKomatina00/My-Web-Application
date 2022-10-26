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
    public class DoktorsNumRepository : Repository<DoktorsIDNum>, IDoktorsNumRepository
    {
        private readonly ApplicationDbContext Context;
        public DoktorsNumRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(DoktorsIDNum entity)
        {
            Context.DoktorsIDNums.Update(entity);
        }
    }
}
