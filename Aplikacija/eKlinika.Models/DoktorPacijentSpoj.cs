using System.ComponentModel.DataAnnotations;

namespace eKlinika.Models
{
    public class DoktorPacijentSpoj
    {
        [Key]
        public int DoktorPacijentID { get; set; }

        public int DoktorID { get; set; }

        public Doktor Doktor { get; set; }

        public int PacijentID { get; set; }

        public Pacijent Pacijent { get; set; }
    }
}
