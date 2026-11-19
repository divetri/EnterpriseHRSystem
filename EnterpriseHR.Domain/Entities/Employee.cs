using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseHR.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public int? ManagerId { get; set; }
    public decimal Salary { get; set; }
    public DateTime JoinDate { get; set; }
    public bool IsActive { get; set; } = true;
}