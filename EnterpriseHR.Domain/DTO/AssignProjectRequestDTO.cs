using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseHR.Domain.DTOs;

public class AssignProjectRequestDTO
{
    public int EmployeeId { get; set; }
    public string RoleInProject { get; set; } = string.Empty;
    public int AllocationPercentage { get; set; }
}