using Microsoft.AspNetCore.Mvc;
using eUseControl.BusinessLogic;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.Web.Models;
using eUseControl.Domain.Entities.User;
using System;

namespace eUseControl.Web.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly eUseControl.BusinessLogic.Interfaces.ISession _session;

        public LoginController()
        {
            var bl = new BussinesLogic();
            _session = bl.GetSessionBL();
        }

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken] // Отключено для Swagger
        public ActionResult Index(UserLogin login)
        {
            if (ModelState.IsValid)
            {
                ULoginData data = new ULoginData
                {
                    Credential = login.Credential,
                    Password = login.Password,
                    LoginIp = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    LoginDateTime = DateTime.Now
                };

                var userLogin = _session.UserLogin(data);
                if (userLogin.Status)
                {
                    // ADD COOKIE
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", userLogin.StatusMsg);
                    return View();
                }
            }

            return View();
        }
    }
}
