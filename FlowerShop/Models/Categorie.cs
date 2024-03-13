using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Boutique.Models
{
    public class Categorie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Nume")]
        public string Name { get; set; }
        [DisplayName("Nr. Comenzii")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Numarul comenzii trebuie sa fie mai mare de 0")]
        public int Order { get; set; }
    }
}
