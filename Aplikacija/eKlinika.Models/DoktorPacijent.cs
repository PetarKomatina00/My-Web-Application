namespace eKlinika.Models
{
    public class DoktorPacijent
    {
        public int DoktorPacijentID { get; set; }

        public int DoktorID { get; set; }

        public Doctor Doktor { get; set; }

        public int PacijentID { get; set; }

        public Pacijent Pacijent { get; set; }
    }
}
