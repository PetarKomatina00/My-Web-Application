using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models.ViewModels
{
    public class KupovinaVM
    {
        public IEnumerable<Kupovina> listaKupovine { get; set; }

        public Narudzbina Narudzbina { get; set; }
    }
}
