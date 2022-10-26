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
    public class LekRepository : Repository<Lek>, ILekRepository
    {
        private readonly ApplicationDbContext Context;
        public LekRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(Lek entity)
        {
            var lekFromDb = Context.Lekovi.FirstOrDefault(x => x.LekID == entity.LekID);
            if(lekFromDb != null)
            {
                lekFromDb.Ime = entity.Ime;
                lekFromDb.Description = entity.Description;
                lekFromDb.ISBN = entity.ISBN;
                lekFromDb.Cena = entity.Cena;
                lekFromDb.Cena3 = entity.Cena3;
                lekFromDb.Proizvodjac = entity.Proizvodjac;
                lekFromDb.PakovanjeID = entity.PakovanjeID;
                if(lekFromDb.ImageUrl != null)
                {
                    lekFromDb.ImageUrl = entity.ImageUrl;
                }
            }
        }
    }
}
