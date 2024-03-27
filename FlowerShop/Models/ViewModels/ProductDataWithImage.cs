using Microsoft.AspNetCore.Http;

namespace Boutique.Models
{
    public class ProductData
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
    }
}
