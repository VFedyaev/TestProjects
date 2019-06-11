﻿using AutoMapper;
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
    public class ExecutorCompanyController : Controller
    {
        private const int _itemsPerPage = 10;
        private IExecutorCompanyService _executorCompanyService;

        public ExecutorCompanyController(IExecutorCompanyService executorCompanyService)
        {
            _executorCompanyService = executorCompanyService;
        }

        public ActionResult AjaxExecutorCompanyList(int? page)
        {
            IEnumerable<ExecutorCompanyDTO> executorCompanyDTOs = _executorCompanyService
                .GetListOrderedByName()
                .ToList();
            IEnumerable<ExecutorCompanyVM> executorCompanyVMs = Mapper.Map<IEnumerable<ExecutorCompanyVM>>(executorCompanyDTOs);

            return PartialView(executorCompanyVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        // GET: ExecutorCompany
        public ActionResult Index(int? page)
        {
            IEnumerable<ExecutorCompanyDTO> executorCompanyDTOs = _executorCompanyService
              .GetListOrderedByName()
              .ToList();
            IEnumerable<ExecutorCompanyVM> executorCompanyVMs = Mapper.Map<IEnumerable<ExecutorCompanyVM>>(executorCompanyDTOs);

            return View(executorCompanyVMs.ToPagedList(page ?? 1, _itemsPerPage));
        }

        public ActionResult Details(Guid? id)
        {
            try
            {
                ExecutorCompanyDTO executorCompanyDTO = _executorCompanyService.Get(id);
                ExecutorCompanyVM executorCompanyVM = Mapper.Map<ExecutorCompanyVM>(executorCompanyDTO);

                return View(executorCompanyVM);
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
        public ActionResult Create([Bind(Include = "Name, Address, Phone, Email, Fax")] ExecutorCompanyVM executorCompanyVM)
        {
            if (ModelState.IsValid)
            {
                ExecutorCompanyDTO executorCompanyDTO = Mapper.Map<ExecutorCompanyDTO>(executorCompanyVM);
                _executorCompanyService.Add(executorCompanyDTO);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            try
            {
                ExecutorCompanyDTO executorCompanyDTO = _executorCompanyService.Get(id);
                ExecutorCompanyVM executorCompanyVM = Mapper.Map<ExecutorCompanyVM>(executorCompanyDTO);
                return View(executorCompanyVM);
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
        public ActionResult Edit([Bind(Include = "Id, Name, Address, Phone, Email, Fax")] ExecutorCompanyVM executorCompanyVM)
        {
            if (ModelState.IsValid)
            {
                ExecutorCompanyDTO executorCompanyDTO = Mapper.Map<ExecutorCompanyDTO>(executorCompanyVM);
                _executorCompanyService.Update(executorCompanyDTO);
                return RedirectToAction("Index");
            }
            return View(executorCompanyVM);
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _executorCompanyService.Delete(id);
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