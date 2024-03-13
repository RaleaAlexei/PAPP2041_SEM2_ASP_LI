namespace Boutique.Models.ViewModels
{
    public class ProdusUtilizatorViewModel
    {
        public ProdusUtilizatorViewModel()
        {
            ListaProduse = new List<Produs>();
        }
        public Utilizator Utilizator { get; set; }
        public IList<Produs> ListaProduse { get; set; }
    }
}
