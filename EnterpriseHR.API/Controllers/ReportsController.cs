using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseHR.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMasterDataRepository _masterDataRepository;

    public ReportsController(IMasterDataRepository masterDataRepository)
    {
        _masterDataRepository = masterDataRepository;
    }

    [HttpGet("department-summary")]
    public async Task<IActionResult> GetDepartmentSummary()
    {
        var summary = await _masterDataRepository.GetDepartmentSummaryAsync();
        return Ok(APIResponse<IEnumerable<DepartmentSummaryDTO>>.Ok(
            summary,
            "Department budget summary retrieved successfully."));
    }
}