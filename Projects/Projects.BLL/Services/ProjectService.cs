using AutoMapper;
using Projects.BLL.DTO;
using Projects.BLL.Interfaces;
using Projects.DAL.Entities;
using Projects.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Projects.BLL.Infrastructure.Exceptions;

namespace Projects.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProjectDTO Get(Guid id)
        {
            Project project = _unitOfWork.Projects.Get(id);

            return Mapper.Map<ProjectDTO>(project);
        }

        public ProjectDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            Project project = _unitOfWork.Projects.Get(id);
            if (project == null)
                throw new NotFoundException();

            return Mapper.Map<ProjectDTO>(project);
        }

        public IEnumerable<ProjectDTO> GetAll()
        {
            List<Project> projects = _unitOfWork.Projects.GetAll().ToList();

            return Mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }

        public IEnumerable<ProjectDTO> GetListOrderedByName()
        {
            List<Project> projects = _unitOfWork.Projects.GetAll().OrderBy(n => n.Name).ToList();

            return Mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }

        public void Add(ProjectDTO projectDTO)
        {
            Project project = Mapper.Map<Project>(projectDTO);
            project.Id = Guid.NewGuid();

            _unitOfWork.Projects.Create(project);
            _unitOfWork.Save();
        }

        public void Update(ProjectDTO projectDTO)
        {
            Project project = Mapper.Map<Project>(projectDTO);

            _unitOfWork.Projects.Update(project);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            Project project = _unitOfWork.Projects.Get(id);
            if (project == null)
                throw new NotFoundException();

            _unitOfWork.Projects.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<EmployeeDTO> GetEmployees(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            IEnumerable<Guid> projectEmployerIds = _unitOfWork
                .ProjectEmployees
                .Find(e => e.ProjectId == id)
                .Select(com => com.EmployeeId);

            if (projectEmployerIds.Count() < 0)
                return Enumerable.Empty<EmployeeDTO>();

            IEnumerable<EmployeeDTO> employees = (
                from
                    relation in _unitOfWork.ProjectEmployees.GetAll()
                join
                    employee in _unitOfWork.Employees.GetAll()
                on
                    relation.EmployeeId equals employee.Id
                join
                    position in _unitOfWork.Positions.GetAll()
                on
                    employee.PositionId equals position.Id
                where
                    relation.ProjectId == id
                select new EmployeeDTO
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    Position = new PositionDTO
                    {
                        Id = position.Id,
                        Name = position.Name
                    }
                });

            return employees;
        }

        public bool HasRelations(Guid id)
        {
            var relations = _unitOfWork.ProjectEmployees.Find(h => h.ProjectId == id);

            return relations.Count() > 0;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
