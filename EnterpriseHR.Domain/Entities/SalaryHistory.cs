using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseHR.Domain.Entities;

public class SalaryHistory
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public decimal OldSalary { get; set; }
    public decimal NewSalary { get; set; }
    public string ChangeReason { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
}