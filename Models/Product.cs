using System;
using System.Collections.Generic;

#nullable disable

namespace E_CommerceStoreCountry.Models
{
    public partial class Product
    {
        public Product()
        {
            AddOrdersDetails = new HashSet<AddOrdersDetail>();
            Carts = new HashSet<Cart>();
            ProductImages = new HashSet<ProductImage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public int? Quantity { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public int? CateoId { get; set; }
        public string Description { get; set; }
        public DateTime? DateSystem { get; set; }

        public virtual Catoegry Cateo { get; set; }
        public virtual ICollection<AddOrdersDetail> AddOrdersDetails { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
