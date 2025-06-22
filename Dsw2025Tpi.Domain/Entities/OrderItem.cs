using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class OrderItem : EntityBase
    {
        public OrderItem() { }
        public OrderItem(Guid orderId, Guid productId, int quantity, decimal price)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity
        {
            get => Quantity;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("La cantidad debe ser mayor a 0");
                }
            }
        }
        public decimal Price
        {
            get => Price;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("El precio unitario debe ser mayor a 0");
                }
            }
        }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }

}



