using System;
using System.Collections.Generic;

#nullable disable

namespace E_CommerceStoreCountry.Models
{
    public partial class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? ProductId { get; set; }
        public int? Qty { get; set; }
        public decimal? Price { get; set; }
        public string Country { get; set; }

        public virtual Product Product { get; set; }
    }
}
