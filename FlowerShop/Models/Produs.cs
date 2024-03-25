using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Boutique.Models
{
    public class Produs
    {
        [Key]
        public int Id { get; set; } = -1;
        [Required]
        [Display(Name = "Nume")]
        public string Name { get; set; }
        [Display(Name = "Descriere")]
        public string Description { get; set; }
        [Display(Name = "Preț")]
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Display(Name = "Categoria")]
        public int CategorieId { get; set; }
        [ForeignKey("CategorieId")]
        public virtual Categorie? Categorie { get; set; }
    }
}
