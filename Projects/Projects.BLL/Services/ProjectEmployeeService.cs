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
    public class ProjectEmployeeService : IProjectEmployeeService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public ProjectEmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProjectEmployeeDTO Get(Guid id)
        {
            ProjectEmployee projectEmployee = _unitOfWork.ProjectEmployees.Get(id);
            return Mapper.Map<ProjectEmployeeDTO>(projectEmployee);
        }

        public IEnumerable<ProjectEmployeeDTO> GetAll()
        {
            List<ProjectEmployee> projectEmployees = _unitOfWork.ProjectEmployees.GetAll().ToList();
            return Mapper.Map<IEnumerable<ProjectEmployeeDTO>>(projectEmployees);
        }

        public void Create(Guid projectId, Guid employeeId)
        {
            ProjectEmployeeDTO projectEmployeeDTO = new ProjectEmployeeDTO
            {
                ProjectId = projectId,
                EmployeeId = employeeId
            };

            this.Add(projectEmployeeDTO);
        }

        public void Add(ProjectEmployeeDTO projectEmployeeDTO)
        {
            ProjectEmployee projectEmployee = Mapper.Map<ProjectEmployee>(projectEmployeeDTO);
            projectEmployee.Id = Guid.NewGuid();

            _unitOfWork.ProjectEmployees.Create(projectEmployee);
            _unitOfWork.Save();
        }

        public void Update(ProjectEmployeeDTO projectEmployeeDTO)
        {
            throw new NotImplementedException();
        }

        public void UpdateProjectRelations(Guid? projId, string[] employeeIds)
        {
            if (projId == null)
                throw new ArgumentNullException();
            Guid projectId = (Guid)projId;

            if (employeeIds.Length <= 0)
            {
                DeleteRelationsByProjectId(projectId);
                return;
            }

            UpdateProjectRelations(projectId, employeeIds);
        }

        public void DeleteRelationsByProjectId(Guid id)
        {
            IEnumerable<Guid> relationIds = _unitOfWork
                .ProjectEmployees
                .Find(r => r.ProjectId == id)
                .Select(r => r.Id)
                .ToList();

            foreach (Guid relationId in relationIds)
                Delete(relationId);
        }

        private void UpdateProjectRelations(Guid projectId, string[] employeeIds)
        {
            List<Guid> projectEmployeeIds = _unitOfWork
                .ProjectEmployees
                .Find(r => r.ProjectId == projectId)
                .Select(r => r.EmployeeId)
                .ToList();

            List<Guid> guidEmployeeIds = employeeIds
                .Select(id => Guid.Parse(id))
                .ToList();

            foreach (Guid employeeId in guidEmployeeIds)
                if (!projectEmployeeIds.Contains(employeeId))
                    Create(projectId, employeeId);

            foreach (Guid projectEmployeeId in projectEmployeeIds)
                if (!guidEmployeeIds.Contains(projectEmployeeId))
                    DeleteProjectRelation(projectId, projectEmployeeId);
        }

        private void DeleteProjectRelation(Guid projectId, Guid employeeId)
        {
            ProjectEmployee projectEmployee = _unitOfWork
                .ProjectEmployees
                .Find(r => r.ProjectId == projectId && r.EmployeeId == employeeId)
                .FirstOrDefault();

            if (projectEmployee != null)
                Delete(projectEmployee.Id);
        }

        public void Delete(Guid id)
        {
            ProjectEmployee projectEmployee = _unitOfWork.ProjectEmployees.Get(id);
            if (projectEmployee == null)
                throw new NotFoundException();

            _unitOfWork.ProjectEmployees.Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
