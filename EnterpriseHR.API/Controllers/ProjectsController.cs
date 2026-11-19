using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseHR.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    public ProjectsController(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [HttpGet("{id:int}/details")]
    public async Task<IActionResult> GetDetails(int id)
    {
        var details = await _projectRepository.GetProjectDetailsAsync(id);
        if (details is null)
        {
            return NotFound(APIResponse<object>.Fail($"Project with ID {id} was not found."));
        }

        return Ok(APIResponse<ProjectDetailDTO>.Ok(details, "Project details retrieved successfully."));
    }

    [HttpPost("{id:int}/assign")]
    public async Task<IActionResult> AssignEmployee(int id, [FromBody] AssignProjectRequestDTO request)
    {
        if (request.EmployeeId <= 0 || string.IsNullOrWhiteSpace(request.RoleInProject))
        {
            return BadRequest(APIResponse<object>.Fail("EmployeeId and RoleInProject are required."));
        }

        await _projectRepository.AssignEmployeeToProjectAsync(id, request);
        return StatusCode(
            StatusCodes.Status201Created,
            APIResponse<object>.Ok(null, "Employee assigned to project successfully."));
    }
}