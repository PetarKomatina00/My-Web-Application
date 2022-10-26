using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKlinika.Models
{
    public class IDSpecial
    {
        public int ID { get; set; }

        [Required]
        [Range(10000000,100000000, ErrorMessage = "Mora biti 8 cifara")]
        public long SpecialIDNum { get; set; }

    }
}
