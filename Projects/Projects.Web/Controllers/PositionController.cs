using AutoMapper;
using PagedList;
using Projects.BLL.DTO;
using Projects.BLL.Infrastructure.Exceptions;
using Projects.BLL.Interfaces;
using Projects.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Projects.Web.Controllers
{
    public class PositionController : Controller
    {
        private const int _itemsPerPage = 10;
        private IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        public ActionResult AjaxPositionList(string sortOrder, int? page)
        {
            IEnumerable<PositionDTO> positionDTOs = _positionService
              .GetListOrderedByName()
              .ToList();

            IEnumerable<PositionVM> positionVMs;

            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "name_desc":
                    positionVMs = Mapper.Map<IEnumerable<PositionVM>>(positionDTOs.OrderByDescending(n => n.Name));
                    break;
                default:
                    positionVMs = Mapper.Map<IEnumerable<PositionVM>>(positionDTOs);
                    break;
            }

            return PartialView(positionVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        public ActionResult Index(string sortOrder, int? page)
        {

            IEnumerable<PositionDTO> positionDTOs = _positionService
                .GetListOrderedByName()
                .ToList();

            IEnumerable<PositionVM> positionVMs;

            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CurrentSort = sortOrder;
            switch (sortOrder)
            {
                case "name_desc":
                    positionVMs = Mapper.Map<IEnumerable<PositionVM>>(positionDTOs.OrderByDescending(n => n.Name));
                    break;
                default:
                    positionVMs = Mapper.Map<IEnumerable<PositionVM>>(positionDTOs);
                    break;
            }

            return View(positionVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        public ActionResult Details(Guid? id)
        {
            try
            {
                PositionDTO positionDTO = _positionService.Get(id);
                PositionVM positionVM = Mapper.Map<PositionVM>(positionDTO);

                return View(positionVM);
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Name")] PositionVM positionVM)
        {
            if (ModelState.IsValid)
            {
                PositionDTO positionDTO = Mapper.Map<PositionDTO>(positionVM);
                _positionService.Add(positionDTO);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            try
            {
                PositionDTO positionDTO = _positionService.Get(id);
                PositionVM positionVM = Mapper.Map<PositionVM>(positionDTO);
                return View(positionVM);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Name")] PositionVM positionVM)
        {
            if (ModelState.IsValid)
            {
                PositionDTO positionDTO = Mapper.Map<PositionDTO>(positionVM);
                _positionService.Update(positionDTO);
                return RedirectToAction("Index");
            }
            return View(positionVM);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _positionService.Delete(id);
            }
            catch (NotFoundException)
            {
                return HttpNotFound();
            }
            catch (HasRelationsException)
            {
                return Content("Удаление невозможно.");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}