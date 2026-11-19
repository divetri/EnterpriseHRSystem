using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseHR.Domain.DTOs;

public class UpdateSalaryRequestDTO
{
    public decimal NewSalary { get; set; }
    public int? NewPositionId { get; set; }
    public string Reason { get; set; } = string.Empty;
}