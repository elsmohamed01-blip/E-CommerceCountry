using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_CommerceStoreCountry.Models
{
    public class IndexVM
    {
        public IndexVM()
        {
            Categories = new List<Catoegry>();
            Products = new List<Product>();
            Reviews = new List<Review>();
            LatestProducts = new List<Product>();

        }

        public string SelectedCountry { get; set; } // الدولة المختارة
        public List<string> Countries { get; set; } = new List<string> { "مصر", "السعودية", "الإمارات", "العراق" };
        public List<Catoegry> Categories { get; set; } = new List<Catoegry>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Product> LatestProducts { get; set; } = new List<Product>();
        public List<Review> Reviews { get; set; } = new List<Review>();


    }
}
