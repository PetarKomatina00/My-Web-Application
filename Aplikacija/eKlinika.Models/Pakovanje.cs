using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models
{
    public class Pakovanje
    {
        [Key]
        public int PakovanjeID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Vrsta Pakovanja")]
        public string VrstaPakovanja { get; set; }
    }
}
