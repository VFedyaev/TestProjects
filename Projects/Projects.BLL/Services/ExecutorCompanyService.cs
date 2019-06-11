using AutoMapper;
using Projects.BLL.DTO;
using Projects.BLL.Infrastructure.Exceptions;
using Projects.BLL.Interfaces;
using Projects.DAL.Entities;
using Projects.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.BLL.Services
{
    public class ExecutorCompanyService : IExecutorCompanyService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public ExecutorCompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ExecutorCompanyDTO Get(Guid id)
        {
            ExecutorCompany executorCompany = _unitOfWork.ExecutorCompanies.Get(id);

            return Mapper.Map<ExecutorCompanyDTO>(executorCompany);
        }

        public ExecutorCompanyDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            ExecutorCompany executorCompany = _unitOfWork.ExecutorCompanies.Get(id);
            if (executorCompany == null)
                throw new NotFoundException();

            return Mapper.Map<ExecutorCompanyDTO>(executorCompany);
        }


        public IEnumerable<ExecutorCompanyDTO> GetAll()
        {
            List<ExecutorCompany> executorCompany = _unitOfWork.ExecutorCompanies.GetAll().ToList();

            return Mapper.Map<IEnumerable<ExecutorCompanyDTO>>(executorCompany);
        }

        public IEnumerable<ExecutorCompanyDTO> GetListOrderedByName()
        {
            List<ExecutorCompany> executorCompany = _unitOfWork.ExecutorCompanies.GetAll().OrderBy(n => n.Name).ToList();

            return Mapper.Map<IEnumerable<ExecutorCompanyDTO>>(executorCompany);
        }

        public void Add(ExecutorCompanyDTO executorCompanyDTO)
        {
            ExecutorCompany executorCompany = Mapper.Map<ExecutorCompany>(executorCompanyDTO);
            executorCompany.Id = Guid.NewGuid();

            _unitOfWork.ExecutorCompanies.Create(executorCompany);
            _unitOfWork.Save();
        }

        public void Update(ExecutorCompanyDTO executorCompanyDTO)
        {
            ExecutorCompany executorCompany = Mapper.Map<ExecutorCompany>(executorCompanyDTO);

            _unitOfWork.ExecutorCompanies.Update(executorCompany);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            ExecutorCompany executorCompany = _unitOfWork.ExecutorCompanies.Get(id);
            if (executorCompany == null)
                throw new NotFoundException();

            _unitOfWork.ExecutorCompanies.Delete(id);
            _unitOfWork.Save();
        }

        public bool HasRelations(Guid id)
        {
            var relations = _unitOfWork.Employees.Find(h => h.ExecutorCompanyId == id);

            return relations.Count() > 0;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
