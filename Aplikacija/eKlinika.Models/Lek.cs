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
    public class Lek
    {
        [Key]
        public int LekID { get; set; }

        [Required]
        public string Ime { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Proizvodjac { get; set; }

        [Required]
        [Range(10,10000)]
        public double Cena { get; set; }

        [Required]
        [Range(10, 10000)]
        [Display(Name = "Cena za 3+")]
        public double Cena3 { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        public int PakovanjeID { get; set; }

        [ValidateNever]
        [ForeignKey("PakovanjeID")]
        public Pakovanje Pakovanje { get; set; }
    }
}
