using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaInteractive.Domain.Entities
{
    public class ProductCategory
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
