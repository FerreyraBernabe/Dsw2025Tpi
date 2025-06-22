using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Product : EntityBase
    {
        public Product() { }
        public Product(string sku, string name, decimal price, int stock, string internalCode, bool isActive)
        {
            Sku = sku;
            Name = name;
            Price = price;
            Stock = stock;
            internalCode = internalCode;
            isActive = true;
        }
        public string Sku { get; set; }
        public string Name { get; set; }
        public decimal Price

        {
        get => Price;
        set
         {
            if (value <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0.");
        }
        }

        public int Stock
        {
            get => Stock;
            set
            {
                if (value < 0)
                    throw new ArgumentException("La cantidad de stock no puede ser negativa.");
            }
        }

        public string InternalCode { get; set; }
        public bool IsActive { get; set; }

        public ICollection<OrderItem>? Items { get; set; }
    }
}
