using eKlinika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.DataAccess.Repository.IRepository
{
    public interface IKupovinaRepository : IRepository<Kupovina>
    {
        public uint DecrementCount(Kupovina shoppingCart, uint count);

        public uint IncrementCount(Kupovina shoppingCart, uint count);
    }
}
