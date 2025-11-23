using System;
using System.Collections.Generic;

#nullable disable

namespace E_CommerceStoreCountry.Models
{
    public partial class AddOrdersDetail
    {
        public int Id { get; set; }
        public int? Productid { get; set; }
        public decimal? Price { get; set; }
        public int? Qty { get; set; }
        public decimal? Totalprice { get; set; }
        public int? Orderid { get; set; }
        public DateTime? DataTime { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
