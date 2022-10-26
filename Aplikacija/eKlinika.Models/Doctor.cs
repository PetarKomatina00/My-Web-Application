using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eKlinika.Models
{
    public class Doctor
    {

        [Key]
        public int DoctorID { get; set; }

        [Required]
        [MaxLength(20)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(20)]
        public string Prezime { get; set; }

        [Required]
        [Display(Name = "Srednje Ime")]
        [MaxLength(20)]
        public string SrednjeIme { get; set; }

        [Required]
        [MaxLength(20)]
        public string Profesija { get; set; }

        [Required]
        [Display(Name = "Godine iskustva")]
        public int GodineIskustva { get; set; }

        [Required]
        [Range(1,5)]
        public double Ocena { get; set; }

        [Required]
        [MaxLength(40)]
        public string Email { get; set; }


        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [ValidateNever]
        public List<DoktorPacijent> ListaPacijenata { get; set; }

    }
}
