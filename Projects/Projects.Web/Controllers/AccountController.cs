using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Projects.BLL.DTO;
using Projects.BLL.Infrastructure.Exceptions;
using Projects.BLL.Interfaces;
using Projects.Web.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Projects.Web.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService _accountService;
        private IAuthenticationManager _autenticationManager => System.Web.HttpContext.Current.GetOwinContext().Authentication;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Roles = "admin")]
        public ActionResult Register()
        {
            ViewBag.Role = new SelectList(
                _accountService.GetAllRoles().ToList(),
                "Name",
                "Description");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            ViewBag.Role = new SelectList(
                _accountService.GetAllRoles().ToList(),
                "Name",
                "Description");

            if (ModelState.IsValid)
            {
                try
                {
                    UserDTO userDTO = new UserDTO
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Password = model.Password,
                        Role = model.Role
                    };
                    await _accountService.CreateUser(userDTO);

                    return RedirectToAction("Index", "Home");
                }
                catch (UserAlreadyExistsException)
                {
                    ModelState.AddModelError("UserName", $"Пользователь с логином {model.UserName} уже существует.");
                }
                catch (InsecurePasswordException)
                {
                    ModelState.AddModelError("Password", "Пароль должен содержать 8 знаков, включая строчные буквы, цифры и специальный символ.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Произошла ошибка. Попробуйте еще раз.");
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login([Bind(Include = "UserName,Password,RememberMe")]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDTO = new UserDTO
                {
                    UserName = model.UserName,
                    Password = model.Password
                };

                ClaimsIdentity claim = await _accountService.AuthenticateUser(userDTO);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    _autenticationManager.SignOut();
                    _autenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    }, claim);

                    return RedirectToRoute(new
                    {
                        controller = "Home",
                        action = "Index"
                    });
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ChangeEmail()
        {
            UserDTO userDTO = await _accountService.GetUser(User.Identity.GetUserId());
            if (userDTO == null)
                return HttpNotFound();

            ChangeEmailModel model = new ChangeEmailModel
            {
                Email = userDTO.Email
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail([Bind(Include = "UserName,Email")] ChangeEmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserDTO userDTO = new UserDTO
                    {
                        Id = Guid.Parse(User.Identity.GetUserId()),
                        Email = model.Email
                    };
                    await _accountService.UpdateEmail(userDTO);

                    TempData["success"] = "Изменения сохранены.";
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Что-то пошло не так. Попытайтесь еще раз.");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ChangePasswordDTO changePasswordDTO = new ChangePasswordDTO
                    {
                        UserId = User.Identity.GetUserId(),
                        OldPassword = model.OldPassword,
                        NewPassword = model.NewPassword
                    };
                    await _accountService.UpdatePassword(changePasswordDTO);

                    TempData["success"] = "Изменения сохранены.";
                }
                catch (OldPasswordIsWrongException)
                {
                    ModelState.AddModelError("OldPassword", "Старый пароль неверный.");
                }
                catch (InsecurePasswordException)
                {
                    ModelState.AddModelError("NewPassword", "Пароль должен содержать 8 знаков, включая строчные буквы, цифры и специальный символ.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Что-то пошло не так. Попытайтесь еще раз.");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            _autenticationManager.SignOut();
            return RedirectToRoute(new
            {
                controller = "Home",
                action = "Index"
            });
        }
    }
}