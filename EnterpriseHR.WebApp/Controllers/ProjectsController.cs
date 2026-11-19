using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Exceptions;
using EnterpriseHR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnterpriseHR.WebApp.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectRepository _projectRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ProjectsController(
        IProjectRepository projectRepository,
        IEmployeeRepository employeeRepository)
    {
        _projectRepository = projectRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<IActionResult> Index()
    {
        var projects = await _projectRepository.GetAllProjectsAsync();
        return View(projects);
    }

    public async Task<IActionResult> Details(int id)
    {
        var details = await _projectRepository.GetProjectDetailsAsync(id);
        if (details is null)
        {
            return NotFound();
        }

        await PopulateActiveEmployeesSelectListAsync();
        ViewBag.AssignmentInput = new AssignProjectRequestDTO();
        return View(details);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(int id, AssignProjectRequestDTO input)
    {
        if (input.EmployeeId <= 0 || string.IsNullOrWhiteSpace(input.RoleInProject))
        {
            ModelState.AddModelError(string.Empty, "Please select an employee and enter a role in project.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _projectRepository.AssignEmployeeToProjectAsync(id, input);
                TempData["SuccessMessage"] = "Team member assigned to project successfully.";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (CommonExceptions ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        var details = await _projectRepository.GetProjectDetailsAsync(id);
        if (details is null)
        {
            return NotFound();
        }

        await PopulateActiveEmployeesSelectListAsync(input.EmployeeId);
        ViewBag.AssignmentInput = input;
        return View(nameof(Details), details);
    }

    private async Task PopulateActiveEmployeesSelectListAsync(int? selectedEmployeeId = null)
    {
        var activeEmployees = await _employeeRepository.GetAllEmployeesAsync(activeOnly: true);
        ViewBag.ActiveEmployees = activeEmployees.Select(e => new SelectListItem
        {
            Value = e.Id.ToString(),
            Text = $"{e.EmployeeNumber} - {e.FullName} ({e.DepartmentName})",
            Selected = selectedEmployeeId == e.Id
        });
    }
}