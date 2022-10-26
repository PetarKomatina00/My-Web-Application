using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models
{
    public class Grad
    {
        public int GradID { get; set; }

        [Required]
        [MaxLength(100)]
        public string NazivGrada { get; set; }
        
        [Required]
        [Range(10000,99999)]
        public int PostanskiBroj { get; set; }
    }
}
