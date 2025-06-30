using BlogApplication.Areas.AdminArea.Models;
using BlogApplication.Areas.AdminArea.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Areas.AdminArea.Controllers
{

    [Area("AdminArea")]
    [Authorize(AuthenticationSchemes = "AdminAuth",Roles = "Admin")]   
    public class ManagerController : Controller
    {
        private readonly IAdminServices _adminservices;
        public ManagerController(IAdminServices adminService) { 
            _adminservices = adminService;
        }
        public IActionResult Index()
        {
            var admins=_adminservices.GetAdmins();
            return View(admins);
        }

        public IActionResult AddMan()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMan(RegisterAdminModel admin)
        {
            if (ModelState.IsValid)
            {
                _adminservices.addUser(admin);
                TempData["SuccessMessage"] = "Yeni yonetici eklendi!";
                return RedirectToAction("Index");
            }
            return View(admin);
        }


        public IActionResult EditMan(Guid id)
        {
            var admin = _adminservices.GetAdmin(id);
            return View(admin);
        }

        [HttpPost]
        public IActionResult EditMan(Admin admin)
        {
            if (ModelState.IsValid)
            {
                _adminservices.updateUser(admin);
                TempData["SuccessMessage"] = "Yonetıcı bilgileri guncellendi!";
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        public IActionResult Delete(Guid id)
        {
            
            if (_adminservices.removeUser(id))
            {
                TempData["SuccessMessage"] = "Yonetıcı silindi!";
            }
            return RedirectToAction("Index");
        }
    }
}
