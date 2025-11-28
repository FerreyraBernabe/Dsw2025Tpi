using Dsw2025Tpi.Application.Dtos;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Application.Validation;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Services
{
    public class CustomersService: ICustomerService
    {
        private readonly IRepository _repository;

        public CustomersService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<CustomerModel.CustomerResponse> CreateCustomerAsync(CustomerModel.CustomerRequest request)
        {

            CustomerValidator.Validate(request);

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            await _repository.Add(customer);

            return new CustomerModel.CustomerResponse(
                customer.Id,
                customer.Name,
                customer.Email,
                customer.PhoneNumber
            );
        }

        public async Task<CustomerModel.CustomerResponse?> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _repository.First<Customer>(c => c.Id == id);

            if (customer == null)
                return null;

            return new CustomerModel.CustomerResponse(
                customer.Id,
                customer.Name,
                customer.Email,
                customer.PhoneNumber
            );
        }

        public async Task<CustomerModel.CustomerResponse?> GetByEmailAsync(string email)
        {
            var entity = await _repository.First<Customer>(c => c.Email == email);
            if (entity == null) return null;

            return new CustomerModel.CustomerResponse(
                entity.Id, entity.Name, entity.Email, entity.PhoneNumber
            );
        }

        public async Task<CustomerModel.CustomerResponse?> GetByUsernameAsync(string username)
        {
            var entity = await _repository.First<Customer>(c => c.Name == username);

            if (entity == null) return null;

            return new CustomerModel.CustomerResponse(
                entity.Id,
                entity.Name,
                entity.Email,
                entity.PhoneNumber
            );
        }


    }
}
