using Microsoft.AspNetCore.Mvc.Rendering;

namespace Boutique.Models.ViewModels
{
    public class ProdusViewModel
    {
        public Produs? Produs { get; set; }
        public IEnumerable<SelectListItem>? CategoriaListaSelectabila { get; set; }
        public IEnumerable<SelectListItem>? TabelListaSelectabila { get; set; }
    }
}
