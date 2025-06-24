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

        public Product(string sku, string name, decimal price, int stock, string internalCode)
        {
            Id = Guid.NewGuid();
            Sku = sku;
            Name = name;
            Price = price;
            Stock = stock;
            InternalCode = internalCode;
            IsActive = true;
        }

        public string Sku { get; set; }
        public string Name { get; set; }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("El precio debe ser mayor a 0.");
                _price = value;
            }
        }

        private int _stock;
        public int Stock
        {
            get => _stock;
            set
            {
                if (value < 0)
                    throw new ArgumentException("La cantidad de stock no puede ser negativa.");
                _stock = value;
            }
        }

        public string InternalCode { get; set; }
        public bool IsActive { get; set; }

        public ICollection<OrderItem>? Items { get; set; }
    }

}
