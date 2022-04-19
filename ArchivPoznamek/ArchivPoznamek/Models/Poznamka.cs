using System.ComponentModel.DataAnnotations;

namespace ArchivPoznamek.Models
{
    public class Poznamka
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nadpis { get; set; } = String.Empty;

        public string Obsah { get; set; } = String.Empty;

        public DateTime DatumVlozeni { get; set; } = DateTime.Now;

        public bool Dulezitost { get; set; }

        [Required]
        virtual public Uzivatel? Autor { get; set; }
    }
}