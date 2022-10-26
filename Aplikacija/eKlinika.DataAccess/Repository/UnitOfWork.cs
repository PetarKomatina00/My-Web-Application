using eKlinika.DataAccess.Data;
using eKlinika.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext Context;
        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;
            Doktor = new DoktorRepository(Context);
            Admin = new AdminRepository(Context);
            Direktor = new DirektorRepository(Context);
            Pacijent = new PacijentRepository(Context);
            Grad = new GradRepository(Context);
            DoktorsIDNum = new DoktorsNumRepository(Context);
            Lekovi = new LekRepository(Context);   
            Pakovanje = new PakovanjeRepository(Context);
            ApplicationUsers = new ApplicationUserRepository(Context);
            Kupovina = new KupovinaRepository(Context);
            Narudzbina = new NarudzbinaRepository(Context);
            DetaljiNarudzbine = new DetaljiNarudzbineRepository(Context);
            Pregledi = new PregledRepository(Context);
            Odeljenja = new OdeljenjeRepository(Context);
            Vreme = new VremeRepository(Context);
        }
        public IDoktorRepository Doktor { get; private set;}

        public IAdminRepository Admin { get; private set; }

        public IDirektorRepository Direktor { get; private set; }

        public IPacijentRepository Pacijent { get; private set; }

        public IGradRepository Grad { get; private set; }

        public IDoktorsNumRepository DoktorsIDNum { get; private set; }

        public ILekRepository Lekovi { get; private set; }

        public IPakovanjeRepository Pakovanje { get; private set; }

        public IApplicationUserRepository ApplicationUsers { get; private set; }

        public IKupovinaRepository Kupovina { get; private set; }

        public INarudzbinaRepository Narudzbina { get; private set; }

        public IDetaljiNarudzbineRepository DetaljiNarudzbine { get; private set; }

        public IPregledRepository Pregledi { get; private set; }

        public IOdeljenjeRepository Odeljenja { get; private set; }

        public IVremeRepository Vreme { get; private set; }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
