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
    public class PositionService : IPositionService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public PositionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PositionDTO Get(Guid id)
        {
            Position position = _unitOfWork.Positions.Get(id);

            return Mapper.Map<PositionDTO>(position);
        }

        public PositionDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            Position position = _unitOfWork.Positions.Get(id);
            if (position == null)
                throw new NotFoundException();

            return Mapper.Map<PositionDTO>(position);
        }

        public IEnumerable<PositionDTO> GetAll()
        {
            List<Position> position = _unitOfWork.Positions.GetAll().ToList();

            return Mapper.Map<IEnumerable<PositionDTO>>(position);
        }

        public IEnumerable<PositionDTO> GetListOrderedByName()
        {
            List<Position> position = _unitOfWork.Positions.GetAll().OrderBy(n => n.Name).ToList();

            return Mapper.Map<IEnumerable<PositionDTO>>(position);
        }

        public void Add(PositionDTO positionDTO)
        {
            Position position = Mapper.Map<Position>(positionDTO);
            position.Id = Guid.NewGuid();

            _unitOfWork.Positions.Create(position);
            _unitOfWork.Save();
        }

        public void Update(PositionDTO positionDTO)
        {
            Position position = Mapper.Map<Position>(positionDTO);

            _unitOfWork.Positions.Update(position);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            if (HasRelations(id))
                throw new HasRelationsException();

            Position position = _unitOfWork.Positions.Get(id);
            if (position == null)
                throw new NotFoundException();

            _unitOfWork.Positions.Delete(id);
            _unitOfWork.Save();
        }

        public bool HasRelations(Guid id)
        {
            var relations = _unitOfWork.Employees.Find(h => h.PositionId == id);

            return relations.Count() > 0;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
