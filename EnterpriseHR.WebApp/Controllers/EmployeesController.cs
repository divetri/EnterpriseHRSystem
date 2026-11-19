using EnterpriseHR.Domain.Entities;
using EnterpriseHR.Domain.Exceptions;
using EnterpriseHR.Domain.Interfaces;
using EnterpriseHR.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnterpriseHR.WebApp.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMasterDataRepository _masterDataRepository;

    public EmployeesController(
        IEmployeeRepository employeeRepository,
        IMasterDataRepository masterDataRepository)
    {
        _employeeRepository = employeeRepository;
        _masterDataRepository = masterDataRepository;
    }

    public async Task<IActionResult> Index(string? search, int? departmentId)
    {
        var departments = await _masterDataRepository.GetAllDepartmentsAsync();
        ViewBag.Departments = new SelectList(departments, "Id", "DepartmentName", departmentId);
        ViewBag.Search = search;
        ViewBag.SelectedDepartmentId = departmentId;

        var employees = await _employeeRepository.GetAllEmployeesAsync(search, departmentId, activeOnly: false);
        return View(employees);
    }

    public async Task<IActionResult> Create()
    {
        var vm = await BuildFormViewModelAsync(new Employee
        {
            JoinDate = DateTime.Today,
            IsActive = true
        });
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var vm = await BuildFormViewModelAsync(model.Employee);
            return View(vm);
        }

        try
        {
            await _employeeRepository.CreateAsync(model.Employee);
            TempData["SuccessMessage"] = "Employee created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var vm = await BuildFormViewModelAsync(model.Employee);
            return View(vm);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee is null)
        {
            return NotFound();
        }

        var vm = await BuildFormViewModelAsync(employee, excludeEmployeeId: id);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmployeeFormViewModel model)
    {
        if (id != model.Employee.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            var vm = await BuildFormViewModelAsync(model.Employee, excludeEmployeeId: id);
            return View(vm);
        }

        try
        {
            await _employeeRepository.UpdateAsync(model.Employee);
            TempData["SuccessMessage"] = "Employee updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var vm = await BuildFormViewModelAsync(model.Employee, excludeEmployeeId: id);
            return View(vm);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _employeeRepository.DeleteEmployeeAsync(id);
            TempData["SuccessMessage"] = "Employee deactivated (soft deleted) successfully.";
        }
        catch (CommonExceptions ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<EmployeeFormViewModel> BuildFormViewModelAsync(Employee employee, int? excludeEmployeeId = null)
    {
        var departments = await _masterDataRepository.GetAllDepartmentsAsync();
        var positions = await _masterDataRepository.GetAllPositionsAsync();
        var allEmployees = await _employeeRepository.GetAllEmployeesAsync(activeOnly: true);

        var managerOptions = allEmployees
            .Where(e => !excludeEmployeeId.HasValue || e.Id != excludeEmployeeId.Value)
            .Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.EmployeeNumber} - {e.FullName}",
                Selected = employee.ManagerId == e.Id
            });

        return new EmployeeFormViewModel
        {
            Employee = employee,
            Departments = new SelectList(departments, "Id", "DepartmentName", employee.DepartmentId),
            Positions = new SelectList(positions, "Id", "Title", employee.PositionId),
            Managers = managerOptions
        };
    }
}