using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models
{
    public class Narudzbina
    {
        public int ID { get; set; }

        public string ApplicationUserID { get; set; }

        [ValidateNever]
        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime DatumNarucivanja { get; set; }

        public DateTime DatumIsporuke { get; set; }

        public double ukupnaSuma { get; set; }

        public string? statusIsporuke { get; set; }

        public string? statusPlacanja { get; set; }

        public string? Dostavljac { get; set; }

        public DateTime DatumPlacanja { get; set; }

        public string? SessionID { get; set; }
        public string? PaymentIntentID { get; set; }

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Adresa { get; set; }

        [Required]
        public string Grad { get; set; }

        [Required]
        public string PostanskiBroj { get; set; }

        [Required]
        public string BrojTelefona { get; set; }
    }
}
