namespace Boutique.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Produs> Produse { get; set; }
        public IEnumerable<Categorie> Categorii { get; set; }
    }
}
