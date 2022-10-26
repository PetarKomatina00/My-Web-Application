using System.ComponentModel.DataAnnotations;

namespace eKlinika.Models
{
    public class Direktor
    {
        [Key]
        public int DirektorID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(100)]
        public string Prezime { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int BrojRadova { get; set; }
    }
}
