using EnterpriseHR.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnterpriseHR.WebApp.Models;

public class EmployeeFormViewModel
{
    public Employee Employee { get; set; } = new()
    {
        JoinDate = DateTime.Today,
        IsActive = true
    };

    public IEnumerable<SelectListItem> Departments { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Positions { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Managers { get; set; } = Enumerable.Empty<SelectListItem>();
}