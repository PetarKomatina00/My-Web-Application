using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace eKlinika.Models
{
    public class Pacijent
    {

        [Key]
        public int PacijentID { get; set; }

        [Required]
        public string Ime { get; set; }
        [Required]
        public string Prezime { get; set; } = String.Empty;

        [Required]
        [Display(Name = "Srednje Ime")]
        [MaxLength(20)]
        public string SrednjeIme { get; set; }

        [Required]
        [Range(1000000000000, 10000000000000, ErrorMessage =("JMBG mora imati tacno 13 cifara"))]
        public long JMBG { get; set; }

        [Required]
        [Range(10000000, 99999999, ErrorMessage = "Broj licne karte mora imati tacno 8 cifara")]
     
        public int BrojKartice { get; set; }

        [Required]
        [Range(0,120)]
        public int Godine { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(40)]
        public string Email { get; set; }

        [ValidateNever]
        public List<DoktorPacijentSpoj> ListaDoktora { get; set; }
    }
}
