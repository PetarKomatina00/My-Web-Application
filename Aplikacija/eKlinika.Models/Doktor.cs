using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eKlinika.Models
{
    public class Doktor
    {

        [Key]
        public int DoktorID { get; set; }

        [Required]
        [MaxLength(20)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(20)]
        public string Prezime { get; set; }

        [Required]
        [MaxLength(20)]
        public string Profesija { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [ValidateNever]
        public List<DoktorPacijentSpoj> ListaPacijenata { get; set; }

        public int? DoktorsIDNumID { get; set; }

        [ValidateNever]
        public DoktorsIDNum DoktorsNum { get; set; }

    }
}