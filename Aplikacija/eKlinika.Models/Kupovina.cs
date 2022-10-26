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
    public class Kupovina
    {
        public int KupovinaID { get; set; }
        public int LekID { get; set; }

        [ForeignKey("LekID")]
        [ValidateNever]
        public Lek Lek { get; set; }

        [Range(1,7, ErrorMessage = "Unesite kolicinu izmedju 1 i 7")]
        public uint Kolicina { get; set; }

        public string ApplicationUserID { get; set; }

        [ForeignKey("ApplicationUserID")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public double Cena { get; set; }
    }
}
