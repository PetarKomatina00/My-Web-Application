using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IDoktorRepository Doktor { get; }

        IAdminRepository Admin { get; }   

        IDirektorRepository Direktor { get; }

        IPacijentRepository Pacijent { get; }

        IGradRepository Grad { get; }

        IDoktorsNumRepository DoktorsIDNum { get; }

        ILekRepository Lekovi { get; }

        IPakovanjeRepository Pakovanje { get; }

        IApplicationUserRepository ApplicationUsers { get; }

        IKupovinaRepository Kupovina { get; }

        INarudzbinaRepository Narudzbina { get; }

        IDetaljiNarudzbineRepository DetaljiNarudzbine { get; }

        IPregledRepository Pregledi { get; }

        IOdeljenjeRepository Odeljenja { get; }

        IVremeRepository Vreme { get; }
        void Save();
    }
}
