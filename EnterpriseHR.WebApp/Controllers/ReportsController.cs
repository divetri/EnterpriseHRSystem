using EnterpriseHR.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseHR.WebApp.Controllers;

public class ReportsController : Controller
{
    private readonly IMasterDataRepository _masterDataRepository;

    public ReportsController(IMasterDataRepository masterDataRepository)
    {
        _masterDataRepository = masterDataRepository;
    }

    public async Task<IActionResult> BudgetSummary()
    {
        var summary = await _masterDataRepository.GetDepartmentSummaryAsync();
        return View(summary);
    }
}