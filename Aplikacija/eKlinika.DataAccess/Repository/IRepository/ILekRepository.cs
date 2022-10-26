using eKlinika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.DataAccess.Repository.IRepository
{
    public interface ILekRepository : IRepository<Lek>
    {
        void Update(Lek entity);
    }
}
