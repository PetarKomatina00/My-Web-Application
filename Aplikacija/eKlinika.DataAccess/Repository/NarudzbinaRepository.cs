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
    public class NarudzbinaRepository : Repository<Narudzbina>, INarudzbinaRepository
    {
        private readonly ApplicationDbContext Context;
        public NarudzbinaRepository(ApplicationDbContext context) : base(context)
        {
            Context = context;
        }

        public void Update(Narudzbina entity)
        {
            Context.Narudzbine.Update(entity);
        }

        public void UpdateStatus(int id, string statusIsporuke, string? statusPlacanja = null)
        {
            var narudzbinaFromDB = Context.Narudzbine.First(x => x.ID == id);
            if (narudzbinaFromDB != null)
            {
                narudzbinaFromDB.statusIsporuke = statusIsporuke;
                if (statusPlacanja != null)
                {
                    narudzbinaFromDB.statusPlacanja = statusPlacanja;
                }
            }
        }

        public void UpdateStripePaymentID(int id, string sessionID, string paymentID)
        {
            var narudzbinaFromDB = Context.Narudzbine.FirstOrDefault(x => x.ID == id);

            narudzbinaFromDB.DatumPlacanja = DateTime.Now;
            narudzbinaFromDB.SessionID = sessionID;
            narudzbinaFromDB.PaymentIntentID = paymentID;
        }
    }
}
