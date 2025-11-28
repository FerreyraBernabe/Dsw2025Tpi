using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Domain.Entities
{
    public class Customer : EntityBase
    {
        public Customer() {} 
       
        public Customer(string name, string email, string phoneNumber)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string GeneratePhoneNumber()
        {
            var random = new Random();
            return $"+54 9 11 {random.Next(1000, 9999)}-{random.Next(1000, 9999)}";
        }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
    
}

