using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Entities;

namespace EnterpriseHR.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync(string? search = null, int? departmentId = null, bool activeOnly = false);
    Task<PagedResultDTO<EmployeeDTO>> GetPagedActiveEmployeesAsync(int? departmentId, int page, int pageSize);
    Task<Employee?> GetByIdAsync(int id);
    Task<int> CreateAsync(Employee employee);
    Task<bool> UpdateAsync(Employee employee);
    Task UpdateEmployeeSalaryAsync(int employeeId, UpdateSalaryRequestDTO request);
    Task DeleteEmployeeAsync(int employeeId);
}