using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models
{
    public class Vreme
    {
        [Key]
        public int VremeID { get; set; }

        //[Required]
        //public string Vremee { get; set; }

        [Required]
        public TimeSpan VremeTimeSpan { get; set; }
    }
}
