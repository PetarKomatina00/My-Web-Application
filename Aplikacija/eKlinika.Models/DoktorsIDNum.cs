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
    public class DoktorsIDNum
    {
        public int ID { get; set; }

        [Required]
        [Range(10000000, 100000000, ErrorMessage = "Mora biti 8 cifara")]
        public long SpecialIDNum { get; set; }

        public bool? isFree { get; set; }

        public int? DoktorID { get; set; }

        [ValidateNever]
        [ForeignKey("DoktorID")]
        public Doktor SpecificDoktor { get; set; }

        //[ValidateNever]
        //[ForeignKey("ApplicationID")]
        //public ApplicationIdentity AppUser { get; set; }
    }
}
