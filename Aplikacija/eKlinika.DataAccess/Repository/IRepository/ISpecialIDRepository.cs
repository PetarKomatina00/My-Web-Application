using eKlinika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.DataAccess.Repository.IRepository
{
    public interface ISpecialIDRepository : IRepository<IDSpecial>
    {
        void Update(IDSpecial entity);
    }
}
