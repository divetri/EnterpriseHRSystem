using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseHR.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveEmployees(
        [FromQuery] int? departmentId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest(APIResponse<object>.Fail("Query parameters 'page' and 'pageSize' must be greater than 0."));
        }

        var result = await _employeeRepository.GetPagedActiveEmployeesAsync(departmentId, page, pageSize);
        return Ok(APIResponse<PagedResultDTO<EmployeeDTO>>.Ok(result, "Active employees retrieved successfully."));
    }

    [HttpPatch("{id:int}/salary")]
    public async Task<IActionResult> UpdateSalary(int id, [FromBody] UpdateSalaryRequestDTO request)
    {
        if (id <= 0)
        {
            return BadRequest(APIResponse<object>.Fail("Invalid employee ID."));
        }

        await _employeeRepository.UpdateEmployeeSalaryAsync(id, request);
        return Ok(APIResponse<object>.Ok(null, "Employee salary updated and recorded in audit history."));
    }
}