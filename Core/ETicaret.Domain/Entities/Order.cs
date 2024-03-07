using ETicaret.Domain.Entities.Common;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Basket Basket { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string OrderCode { get; set; } 

        public CompletedOrder CompletedOrder { get; set; }
        //public ICollection<Product>? Products{ get; set; }
        //public Guid? CustomerId { get; set; }
        //public Customer? Customer { get; set; }
    }
}
