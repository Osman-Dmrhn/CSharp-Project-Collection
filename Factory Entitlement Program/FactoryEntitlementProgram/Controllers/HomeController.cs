using FactoryEntitlementProgram.Interfaces;
using FactoryEntitlementProgram.Models;
using FactoryEntitlementProgram.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FactoryEntitlementProgram.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserService _appUserService;
        private readonly IEmployeeService _employeeService;
        private readonly IWorkDayService _workDayService;
        private readonly IMonthlyEarningsService _monthlyEarningsService;
        private readonly IHolidayService _holidayService;

        public HomeController(
            SignInManager<AppUser> signInManager,
            IUserService appUserService,
            IEmployeeService employeeService,
            IWorkDayService workDayService,
            IMonthlyEarningsService monthlyEarningsService,IHolidayService holidayService)
        {
            _signInManager = signInManager;
            _appUserService = appUserService;
            _employeeService = employeeService;
            _workDayService = workDayService;
            _monthlyEarningsService = monthlyEarningsService;
            _holidayService=holidayService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            var user = await _appUserService.FindByUsernameAsync(username);

            if (user != null)
            {
                // Doðrulama username ile yapýlmalý
                var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Dashboard");
                }
            }

            ViewBag.Error = "Kullanýcý adý veya þifre hatalý.";
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _appUserService.GetCurrentUserAsync(User);
            if (user == null || user.EmployeeId == null)
                return RedirectToAction("Index");

            var employee = await _employeeService.GetEmployeeByIdAsync(user.EmployeeId.Value);
            var earnings = await _monthlyEarningsService.GetByEmployeeAndMonthAsync(employee.Id, DateTime.Now.Year, DateTime.Now.Month);
            var workDays = await _workDayService.GetByEmployeeAndMonthAsync(employee.Id, DateTime.Now.Year, DateTime.Now.Month);
            var holiday = await _holidayService.GetAllHolidaysAsync();

            var model = new DashboardViewModel
            {
                FullName = user.FullName,
                EmployeeCode = employee.Id.ToString(),
                AttendedDays = workDays
                .Where(x => !x.IsAbsent)
                .Select(x => x.WorkDate)
                .ToList(),

                ExcusedDays = workDays
                .Where(x => x.IsAbsent && x.IsExcused)
                .Select(x => x.WorkDate)
                .ToList(),

                UnexcusedDays = workDays
                .Where(x => x.IsAbsent && !x.IsExcused)
                .Select(x => x.WorkDate)
                .ToList(),

                HolidayDays = holiday.Select(x=>x.Date).ToList(),

                TotalNormalHours = earnings?.TotalNormalHours ?? 0,
                TotalOvertimeHours = earnings?.TotalOvertimeHours ?? 0,
                UnexcusedAbsenceCount = earnings?.UnexcusedAbsenceCount ?? 0,
                AbsencePenaltyHours = earnings?.AbsencePenaltyHours ?? 0,
                AbsencePenaltyAmount = earnings?.AbsencePenaltyAmount ?? 0,
                NetPayment = earnings?.NetPayment ?? 0,
                NormalHoursPay = employee.SaatlikUcret,
                OvertimeHoursPay=employee.SaatlikUcret*1.5m,
            };

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}

