using BlogAngApi.Areas.AdminArea.Models;
using BlogAngApi.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAngApi.Controllers
{
    [Route("api/admin/managers")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ManagerController : ControllerBase
    {
        private readonly IAdminServices _adminServices;

        public ManagerController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpGet("GetManagers")]
        public IActionResult GetManagers()
        {
            var admins = _adminServices.GetAdmins();
            return Ok(admins);
        }

        [HttpPost("AddManager")]
        public IActionResult AddManager([FromBody] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _adminServices.addUser(admin);
                return Ok(new { success = true, message = "Yeni yönetici başarıyla eklendi!" });
            }
            return BadRequest(new { success = false, message = "Geçersiz veri!" });
        }

        [HttpGet("{id}")]
        public IActionResult GetManagerById(Guid id)
        {
            var admin = _adminServices.GetAdmin(id);
            if (admin == null)
            {
                return NotFound(new { success = false, message = "Yönetici bulunamadı!" });
            }
            return Ok(admin);
        }

        [HttpPut("EditManager/{id}")]
        public IActionResult EditManager(Guid id, [FromBody] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _adminServices.updateUser(admin);
                return Ok(new { success = true, message = "Yönetici bilgileri güncellendi!" });
            }
            return BadRequest(new { success = false, message = "Güncelleme sırasında hata oluştu!" });
        }

        [HttpDelete("DeleteManager/{id}")]
        public IActionResult DeleteManager(Guid id)
        {
            if (_adminServices.removeUser(id))
            {
                return Ok(new { success = true, message = "Yönetici başarıyla silindi!" });
            }
            return NotFound(new { success = false, message = "Yönetici bulunamadı!" });
        }
    }
}
