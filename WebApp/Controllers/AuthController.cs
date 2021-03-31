using Core.Application.Users.Dtos.Patches;
using Core.Application.Users.Dtos.Plain;
using Core.Application.Users.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;
using Shared.Security;

namespace DocSpiderTest.Controllers {
    public class AuthController : Controller {
        private readonly UserService _service;
        private readonly CurrentAuth _auth;

        public AuthController(UserService service, CurrentAuth auth) {
            _service = service;
            _auth    = auth;
        }

        public IActionResult Login() {
            return View(new UserCredentialsDto());
        }

        [HttpPost]
        public IActionResult Login([FromForm] UserCredentialsDto dto) {
            if (!ModelState.IsValid)
                return View(dto);
                
            try {
                _service.Authenticate(_auth, dto);

                return RedirectToAction("Index", "Home");
            }
            catch (ValidationFieldException e) {
                ModelState.AddModelError(e.Field, e.Error);
            }

            return View(dto);
        }

        public IActionResult Register() {
            return View(new UserDto());
        }

        [HttpPost]
        public IActionResult Register([FromForm] UserDto dto) {
            if (!ModelState.IsValid)
                return View(dto);

            try {
                _service.Register(dto);

                return RedirectToAction("AccountCreated");
            }
            catch (ValidationFieldException e) {
                ModelState.AddModelError(e.Field, e.Error);
            }

            return View(dto);
        }

        [HttpGet]
        public IActionResult Logout() {
            _auth.Logout();
            return RedirectToAction("Login");
        }

        public IActionResult AccountCreated() {
            return View();
        }
    }
}