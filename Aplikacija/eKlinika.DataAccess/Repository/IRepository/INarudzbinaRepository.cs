using eKlinika.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.DataAccess.Repository.IRepository
{
    public interface INarudzbinaRepository : IRepository<Narudzbina>
    {
        void Update(Narudzbina entity);

        void UpdateStatus(int id, string statusIsporuke, string? statusPlacanja = null);

        void UpdateStripePaymentID(int id, string sessionID, string paymentID);
    }
}
