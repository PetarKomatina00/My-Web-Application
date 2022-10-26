using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eKlinika.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        public string Adresa { get; set; }

        public string Email { get; set; }

        [Required]
        public string? BrojTelefona { get; set; }

        public string? NazivGrada { get; set; }

        [Required]
        public string PostanskiBroj { get; set; }

        public int? DoktorsIDNumID { get; set; }

        public string? ImageUrl { get; set; }

        [MaxLength(30)]
        public string? Profesija { get; set; }

    }
}
