using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models
{
    public class DetaljiNarudzbine
    {
        public int ID { get; set; }

        public int NarudzbinaID { get; set; }

        [ValidateNever]
        [ForeignKey("NarudzbinaID")]
        public Narudzbina Narudzbina { get; set; }

        public int LekID { get; set; }

        [ValidateNever]
        [ForeignKey("LekID")]
        public Lek Lek { get; set; }

        public uint Kolicina { get; set; }

        public double Cena { get; set; }
    }
}
