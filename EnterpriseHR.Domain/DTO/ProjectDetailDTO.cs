using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseHR.Domain.DTOs;

public class ProjectDetailDTO
{
    public int Id { get; set; }
    public string ProjectCode { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<ProjectMemberDTO> Members { get; set; } = new();
}

public class ProjectMemberDTO
{
    public int AssignmentId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string RoleInProject { get; set; } = string.Empty;
    public int AllocationPercentage { get; set; }
    public DateTime AssignedDate { get; set; }
}