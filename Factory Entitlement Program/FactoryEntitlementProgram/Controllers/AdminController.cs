using FactoryEntitlementProgram.Interfaces;
using FactoryEntitlementProgram.Models;
using FactoryEntitlementProgram.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FactoryEntitlementProgram.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmployeeService _employeeService;
        private readonly IHolidayService _holidayService;
        private readonly IWorkDayService _workDayService;
        private readonly IMonthlyEarningsService _earningsService;
        private readonly IUserService _userService;
        private readonly IWageRateService _wageRateService;

        public AdminController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmployeeService employeeService,
            IHolidayService holidayService,
            IWorkDayService workDayService,
            IMonthlyEarningsService earningsService,
            IUserService userService,
            IWageRateService wageRateService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeService = employeeService;
            _holidayService = holidayService;
            _workDayService = workDayService;
            _earningsService = earningsService;
            _userService = userService;
            _wageRateService = wageRateService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || user.Role != "Admin")
            {
                ViewBag.Error = "Geçersiz kullanıcı veya yetki yok.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Giriş başarısız.";
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // GET: AddEmployee
        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();

            var model = new EmployeeCreateViewModel
            {
                Employees = employees.ToList()
            };

            return View(model);
        }

        // POST: AddEmployee
        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = (await _employeeService.GetAllEmployeesAsync()).ToList();
                return View(model);
            }

            var existingUser = await _userService.FindByUsernameAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Bu e-posta ile zaten bir kullanıcı var.");
                model.Employees = (await _employeeService.GetAllEmployeesAsync()).ToList();
                return View(model);
            }

            var newUser = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                Role = model.Role
            };

            await _userService.AddUserAsync(newUser, model.Password);

            var employee = new Employee
            {
                Department = model.Department,
                SaatlikUcret = model.SaatlikUcret,  // Buraya eklendi
                AppUser = newUser
            };

            await _employeeService.AddEmployeeAsync(employee);

            TempData["Message"] = "Personel başarıyla eklendi.";
            return RedirectToAction(nameof(AddEmployee));
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();

            var model = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Department = employee.Department,
                FullName = employee.AppUser?.FullName,
                Email = employee.AppUser?.Email,
                Role = employee.AppUser?.Role,
                SaatlikUcret = employee.SaatlikUcret  // Buraya eklendi
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EmployeeEditViewModel model)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(model.Id);
            if (employee == null)
                return NotFound();

            // Update metoduna saatlik ücret parametresi eklendi
            await _employeeService.UpdateEmployeeAsync(employee, model.FullName!, model.Email!, model.Department!, model.Role!, model.SaatlikUcret);

            TempData["Message"] = "Personel başarıyla güncellendi.";
            return RedirectToAction("AddEmployee");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            TempData["Message"] = "Personel başarıyla silindi.";
            return RedirectToAction("AddEmployee");
        }

        [HttpGet]
        public async Task<IActionResult> AddHoliday()
        {
            var holidays = await _holidayService.GetAllHolidaysAsync();
            ViewBag.Holidays = holidays.Select(h => new
            {
                title = h.Description,
                start = h.Date.ToString("yyyy-MM-dd"),
                allDay = true,
                color = "red"
            });
            return View(new Holiday());
        }

        [HttpPost]
        public async Task<IActionResult> AddHoliday(Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                await _holidayService.AddHolidayAsync(holiday);
                TempData["Message"] = "Tatil günü eklendi.";
                return RedirectToAction("AddHoliday");
            }
            return View(holiday);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHoliday([FromBody] DeleteHolidayRequest model)
        {
            if (model == null || model.Date == default)
                return BadRequest();

            // Holiday serviste sadece id ile silme var, önce id'yi bulalım
            var holidays = await _holidayService.GetHolidaysInRangeAsync(model.Date, model.Date);
            if (holidays == null)
                return NotFound();

            foreach (var h in holidays)
            {
                await _holidayService.DeleteHolidayAsync(h.Id);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddMultipleHolidays([FromBody] List<Holiday> holidays)
        {
            if (holidays == null || holidays.Count == 0)
                return BadRequest();

            foreach (var holiday in holidays)
            {
                await _holidayService.AddHolidayAsync(holiday);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> WorkdayList()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            var holidays = await _holidayService.GetAllHolidaysAsync();

            DateTime today = DateTime.Today;
            DateTime startDate = new DateTime(today.Year, today.Month, 1);
            DateTime endDate = new DateTime(today.Year, today.Month, 30);

            var existingWorkdays = await _workDayService.GetAllAsync();
            var wageRates = await _wageRateService.GetAllAsync();

            TimeSpan normalStartTime = new TimeSpan(9, 0, 0);
            TimeSpan normalEndTime = new TimeSpan(17, 0, 0);
            double normalWorkHours = (normalEndTime - normalStartTime).TotalHours;

            // Ücret oranlarını aylık tarihe göre seç
            var wageRateForMonth = wageRates
                .Where(wr => wr.EffectiveFrom <= startDate)
                .OrderByDescending(wr => wr.EffectiveFrom)
                .FirstOrDefault();

            // Otomatik mesai günleri atama (önceki örnek gibi)
            var workdaysToAdd = new List<WorkDay>();
            foreach (var date in Enumerable.Range(0, (endDate - startDate).Days + 1).Select(offset => startDate.AddDays(offset)))
            {
                bool isHoliday = holidays.Any(h => h.Date.Date == date.Date);
                if (isHoliday)
                    continue;

                foreach (var employee in employees)
                {
                    bool exists = existingWorkdays.Any(wd => wd.EmployeeId == employee.Id && wd.WorkDate.Date == date.Date);
                    if (!exists)
                    {
                        workdaysToAdd.Add(new WorkDay
                        {
                            EmployeeId = employee.Id,
                            WorkDate = date,
                            IsHoliday = false,
                            IsAbsent = false,
                            IsExcused = false,
                            Note = "Normal Mesai",
                            StartTime = normalStartTime,
                            EndTime = normalEndTime
                        });
                    }
                }
            }
            if (workdaysToAdd.Any())
                await _workDayService.AddRangeAsync(workdaysToAdd);

            // Güncellenmiş tüm mesai kayıtlarını tekrar al
            var allWorkdays = await _workDayService.GetAllAsync();

            // ViewModel listesi oluştur
            var summaries = employees.Select(emp =>
            {
                var empWorkdays = allWorkdays.Where(wd => wd.EmployeeId == emp.Id && !wd.IsAbsent && !wd.IsHoliday && wd.StartTime.HasValue && wd.EndTime.HasValue);

                int workDaysCount = empWorkdays.Select(wd => wd.WorkDate.Date).Distinct().Count();

                double totalOvertimeHours = empWorkdays.Sum(wd =>
                {
                    var workedHours = (wd.EndTime.Value - wd.StartTime.Value).TotalHours;
                    return workedHours > normalWorkHours ? workedHours - normalWorkHours : 0;
                });

                decimal overtimePayment = 0;
                if (wageRateForMonth != null)
                {
                    overtimePayment = (decimal)totalOvertimeHours * wageRateForMonth.OvertimeHourRate;
                }

                return new EmployeeWorkSummaryViewModel
                {
                    EmployeeId = emp.Id,
                    FullName = emp.AppUser?.FullName ?? "—",
                    WorkDaysCount = workDaysCount,
                    OvertimeHours = Math.Round(totalOvertimeHours, 2),
                    OvertimePayment = overtimePayment
                };
            }).ToList();

            return View(summaries);
        }

        [HttpGet]
        public async Task<IActionResult> WorkdayDetails(int employeeId)
        {
            var workDays = await _workDayService.GetByEmployeeAsync(employeeId);
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
            var holidays = await _holidayService.GetAllHolidaysAsync();

            if (employee == null)
                return NotFound();

            var model = new EmployeeWorkdayDetailsViewModel
            {
                EmployeeId = employeeId,
                FullName = employee.AppUser.FullName,
                WorkdayDetails = workDays.Select(w => new WorkdayDetailViewModel
                {
                    Id = w.Id,
                    WorkDate = w.WorkDate,
                    IsAbsent = w.IsAbsent,
                    IsExcused = w.IsExcused,
                    StartTime = w.StartTime,
                    EndTime = w.EndTime,
                    Note = w.Note,
                    OvertimeHours = w.OvertimeHours
                }).OrderBy(w => w.WorkDate).ToList(),


                Holidays = holidays.Select(h => h.Date).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> WorkdayDetails(EmployeeWorkdayDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            foreach (var wd in model.WorkdayDetails)
            {

                if (wd.OvertimeHoursRaw.HasValue)
                {
                    wd.OvertimeHours = TimeSpan.FromHours(wd.OvertimeHoursRaw.Value);
                }
                else
                {
                    wd.OvertimeHours = null;
                }
                if (wd.Id > 0)
                {


                    var entity = await _workDayService.GetByIdAsync(wd.Id.Value);
                    if (entity == null) continue;

                    entity.IsAbsent = wd.IsAbsent;
                    entity.IsExcused = wd.IsExcused;
                    entity.StartTime = wd.StartTime;
                    entity.EndTime = wd.EndTime;
                    entity.Note = wd.Note;
                    entity.OvertimeHours = wd.OvertimeHours;
                    entity.WorkDate = wd.WorkDate;
                    entity.EmployeeId = model.EmployeeId;

                    await _workDayService.UpdateAsync(entity);
                }
                else
                {

                    var newEntity = new WorkDay
                    {
                        EmployeeId = model.EmployeeId,
                        WorkDate = wd.WorkDate,
                        IsAbsent = wd.IsAbsent,
                        IsExcused = wd.IsExcused,
                        StartTime = wd.StartTime,
                        EndTime = wd.EndTime,
                        Note = wd.Note,
                        OvertimeHours = wd.OvertimeHours,
                    };

                    await _workDayService.AddAsync(newEntity);
                }
            }

            return RedirectToAction(nameof(WorkdayDetails), new { employeeId = model.EmployeeId });
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkdaysJson(int employeeId)
        {
            var workdays = await _workDayService.GetWorkdaysByEmployeeIdAsync(employeeId);

            var result = workdays.Select(w => new
            {
                title = GetEventTitle(w),
                start = w.WorkDate.ToString("yyyy-MM-dd"),
                allDay = true,
                color = GetEventColor(w)
            });

            return Json(result);
        }

        private string GetEventTitle(WorkDay w)
        {
            if (w.IsHoliday) return "Tatil";
            if (w.IsAbsent && w.IsExcused) return "Mazeretli";
            if (w.IsAbsent && !w.IsExcused) return "Gelmedi";
            if (w.OvertimeHours.HasValue && w.OvertimeHours.Value.TotalMinutes > 0) return "Fazla Mesai";
            return "Çalıştı";
        }

        private string GetEventColor(WorkDay w)
        {
            if (w.IsHoliday) return "gray";
            if (w.IsAbsent && w.IsExcused) return "orange";
            if (w.IsAbsent && !w.IsExcused) return "red";
            if (w.OvertimeHours.HasValue && w.OvertimeHours.Value.TotalMinutes > 0) return "blue";
            return "green";
        }

        [HttpGet]
        public async Task<IActionResult> CalculateEarnings()
        {

            ViewBag.Months = Enumerable.Range(1, 12)
                .Select(m => new SelectListItem
                {
                    Value = m.ToString(),
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
                }).ToList();

            ViewBag.SelectedYear = DateTime.Now.Year;
            ViewBag.SelectedMonth = DateTime.Now.Month;

            var earnings = await _earningsService.CalculateMonthlyEarningsAsync(DateTime.Now.Year, DateTime.Now.Month);

            return View(earnings);
        }

        [HttpPost]
        public async Task<IActionResult> CalculateEarnings(int year, int month)
        {
            ViewBag.Months = Enumerable.Range(1, 12)
                .Select(m => new SelectListItem
                {
                    Value = m.ToString(),
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
                }).ToList();

            ViewBag.SelectedYear = year;
            ViewBag.SelectedMonth = month;

            var earnings = await _earningsService.CalculateMonthlyEarningsAsync(year, month);
            return View(earnings);
        }


    }
}

