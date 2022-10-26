using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models
{
    public class Pregled
    {
        [Key]
        public int PregledID { get; set; }

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Telefon { get; set; }

        //[Required]
        //public string Email { get; set; }

        [Required]
        public string Datum { get; set; }

        public int VremeID { get; set; }

        [ValidateNever]
        [ForeignKey("VremeID")]
        public Vreme Vreme { get; set; }

        public int OdeljenjeID { get; set; }

        [ValidateNever]
        [ForeignKey("OdeljenjeID")]
        public Odeljenje Odeljenje { get; set; }

        public string ApplicationUserID { get; set; }

        [ValidateNever]
        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
