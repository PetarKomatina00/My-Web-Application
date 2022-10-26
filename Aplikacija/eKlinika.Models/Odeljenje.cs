using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models
{
    public class Odeljenje
    {
        [Key]
        public int OdeljenjeID { get; set; }

        [Required]
        [MaxLength(30)]
        public string NazivOdeljenja { get; set; }
    }
}
