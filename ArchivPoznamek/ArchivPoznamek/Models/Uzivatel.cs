using System.ComponentModel.DataAnnotations;

namespace ArchivPoznamek.Models
{
    public class Uzivatel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Jmeno { get; set; } = String.Empty;
        [Required]
        public string Heslo { get; set; } = String.Empty;
        [Required]
        public string Email { get; set; } = String.Empty;

        //[Required]
        //virtual public List<Ukol>? Ukoly { get; set; }
    }
}