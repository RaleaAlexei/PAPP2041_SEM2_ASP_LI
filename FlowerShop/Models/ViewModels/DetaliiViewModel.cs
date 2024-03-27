namespace Boutique.Models.ViewModels
{
    public class DetaliiViewModel
    {
        public DetaliiViewModel()
        {
            Produs = new Produs();
        }

        public Produs Produs { get; set; }
        public List<Produs> OtherProductsInCategory { get; set; }
        public bool ExistaInCos { get; set; }
    }
}
