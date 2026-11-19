using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Entities;

namespace EnterpriseHR.Domain.Interfaces;

public interface IMasterDataRepository
{
    Task<IEnumerable<Department>> GetAllDepartmentsAsync();
    Task<IEnumerable<Position>> GetAllPositionsAsync();
    Task<IEnumerable<DepartmentSummaryDTO>> GetDepartmentSummaryAsync();
}