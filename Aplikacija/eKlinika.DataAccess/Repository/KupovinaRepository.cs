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
    public class KupovinaRepository : Repository<Kupovina>, IKupovinaRepository
    {
        private readonly ApplicationDbContext Context;
        public KupovinaRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public uint DecrementCount(Kupovina shoppingCart, uint count)
        {
            shoppingCart.Kolicina -= count;
            return shoppingCart.Kolicina;
        }

        public uint IncrementCount(Kupovina shoppingCart, uint count)
        {
            shoppingCart.Kolicina += count;
            return shoppingCart.Kolicina;
        }
    }
}
