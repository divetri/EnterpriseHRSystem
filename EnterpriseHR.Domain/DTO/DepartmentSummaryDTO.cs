using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseHR.Domain.DTOs;

public class DepartmentSummaryDTO
{
    public int DepartmentId { get; set; }
    public string DepartmentCode { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public int ActiveEmployeeCount { get; set; }
    public decimal TotalMonthlySalary { get; set; }
    public decimal RemainingBudget { get; set; }
    public int ActiveProjectCount { get; set; }
}