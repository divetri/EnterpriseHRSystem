using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseHR.Domain.Entities;

public class ProjectAssignment
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int EmployeeId { get; set; }
    public string RoleInProject { get; set; } = string.Empty;
    public int AllocationPercentage { get; set; }
    public DateTime AssignedDate { get; set; }
}