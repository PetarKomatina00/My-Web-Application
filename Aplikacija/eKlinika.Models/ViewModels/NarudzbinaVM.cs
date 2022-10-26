using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models.ViewModels
{
    public class NarudzbinaVM
    {
        public Narudzbina Narudzbina { get; set; }

        public IEnumerable<DetaljiNarudzbine> DetaljiNarudzbine { get; set; }
    }
}
