using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Entities;

namespace EnterpriseHR.Domain.Interfaces;

public interface IProjectRepository
{
    Task<IEnumerable<ProjectSummaryDTO>> GetAllProjectsAsync();
    Task<ProjectDetailDTO?> GetProjectDetailsAsync(int projectId);
    Task<int> CreateAsync(Project project);
    Task AssignEmployeeToProjectAsync(int projectId, AssignProjectRequestDTO request);
}