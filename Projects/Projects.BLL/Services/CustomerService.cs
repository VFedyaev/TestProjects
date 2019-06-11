using AutoMapper;
using Projects.BLL.DTO;
using Projects.BLL.Infrastructure.Exceptions;
using Projects.BLL.Interfaces;
using Projects.DAL.Entities;
using Projects.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projects.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CustomerDTO Get(Guid id)
        {
            Customer customer = _unitOfWork.Customers.Get(id);

            return Mapper.Map<CustomerDTO>(customer);
        }

        public CustomerDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            Customer customer = _unitOfWork.Customers.Get(id);
            if (customer == null)
                throw new NotFoundException();

            return Mapper.Map<CustomerDTO>(customer);
        }

        public IEnumerable<CustomerDTO> GetAll()
        {
            List<Customer> customers = _unitOfWork.Customers.GetAll().ToList();

            return Mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public IEnumerable<CustomerDTO> GetListOrderedByName()
        {
            List<Customer> customers = _unitOfWork.Customers.GetAll().OrderBy(n => n.FullName).ToList();

            return Mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public void Add(CustomerDTO customerDTO)
        {
            Customer customer = Mapper.Map<Customer>(customerDTO);
            customer.Id = Guid.NewGuid();

            _unitOfWork.Customers.Create(customer);
            _unitOfWork.Save();
        }

        public void Update(CustomerDTO customerDTO)
        {
            Customer customer = Mapper.Map<Customer>(customerDTO);

            _unitOfWork.Customers.Update(customer);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            Customer customer = _unitOfWork.Customers.Get(id);
            if (customer == null)
                throw new NotFoundException();

            _unitOfWork.Customers.Delete(id);
            _unitOfWork.Save();
        }

        public bool HasRelations(Guid id)
        {
            var relations = _unitOfWork.Projects.Find(p => p.CustomerId == id);

            return relations.Count() > 0;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
