using System;
using System.Collections.Generic;

#nullable disable

namespace E_CommerceStoreCountry.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string Description { get; set; }
    }
}
