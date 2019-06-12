using AutoMapper;
using PagedList;
using Projects.BLL.DTO;
using Projects.BLL.Interfaces;
using Projects.Web.Models.Account;
using Projects.Web.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Projects.Web.Controllers
{
    public class UserController : Controller
    {
        private const int _itemsPerPage = 10;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;

        public UserController(IUserService userService, IAccountService accountService)
        {
            _userService = userService;
            _accountService = accountService;

        }
        [Authorize(Roles = "admin")]
        public ActionResult Index(int? page)
        {
            var userDTOs = _userService.GetAllUsers().ToList();
            var userVMs = Mapper.Map<IEnumerable<UserVM>>(userDTOs).ToPagedList(page ?? 1, _itemsPerPage);

            return View(userVMs);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ChangeRole(string userId)
        {
            if (userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var role = await _userService.GetUserRole(userId);
            ChangeRoleModel model = new ChangeRoleModel
            {
                UserId = userId,
                OldRole = role,
                Role = role
            };

            ViewBag.Role = GetRoleNameSelectList(model.Role);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeRole([Bind(Include = "UserId,OldRole,Role")] ChangeRoleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ChangeRoleDTO changeRoleDTO = new ChangeRoleDTO
                    {
                        UserId = model.UserId,
                        OldRole = model.OldRole,
                        Role = model.Role
                    };
                    await _userService.ChangeUserRole(changeRoleDTO);
                    TempData["success"] = "Изменения сохранены.";
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Произошла ошибка. Попробуйте еще раз либо обратитесь к администратору.");
                }
            }
            ViewBag.Role = GetRoleNameSelectList(model.Role);

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _userService.DeleteUser(id);
            }
            catch (ArgumentNullException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                TempData["fail"] = "Произошла ошибка.";
            }

            return RedirectToAction("Index");
        }

        private SelectList GetRoleNameSelectList(string selectedValue = null)
        {
            return new SelectList(_accountService.GetAllRoles().ToList(), "Name", "Description", selectedValue);
        }
    }
}