using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using TRPO_08_1.Data;
using TRPO_08_1.Domain;
using TRPO_08_1.Models;
using TRPO_08_1.Sevices;

namespace TRPO_08_1.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly EncryptingService _encryptingService;
        private readonly RestarauntContext _dbContext;

        public AuthorizationController()
        {
            _encryptingService = new EncryptingService();
            _dbContext = new RestarauntContext();
        }

        public ActionResult Register()
        {
            ViewBag.Message = "Регистрация";

            return View(new RegistrationViewModel());
        }

        [HttpPost]
        public ActionResult Register(FormCollection form, RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                 View(model);

            if (_dbContext.Customers.Any(c => c.Email == model.Email))
            {

                View(model);
            }

            var customer = new Customer()
            {
                Email = model.Email,
                Name = model.Name,
                PasswordHash = _encryptingService.GetPasswordHash(model.Password),
                Address = model.Address,
                Phone = model.Phone,
                CustomerRoleid = (int)CustomerRole.User
            };

            _dbContext.Customers.Add(customer);

            return Register();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Авторизация";

            return View(new RegistrationViewModel());
        }

        [HttpPost]
        public ActionResult Login(FormCollection form, RegistrationViewModel model)
        {
            var passwordHash = _encryptingService.GetPasswordHash(model.Password);
            var customer =
                _dbContext.Customers.FirstOrDefault(c => c.Email == model.Email && c.PasswordHash == passwordHash);

            if (customer == null)
            {
                return View(new RegistrationViewModel());
            }

            switch (customer.CustomerRole)
            {
                case CustomerRole.Manager:
                    break;
                case CustomerRole.Admin:

                    break;
                case CustomerRole.Director:

                    break;
                case CustomerRole.User:
                default:

                    break;
            }
        }
    }
}