using System.Data;
using Dapper;
using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Entities;
using EnterpriseHR.Domain.Exceptions;
using EnterpriseHR.Domain.Interfaces;
using Microsoft.Data.SqlClient;

namespace EnterpriseHR.DataAccess.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public ProjectRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ProjectSummaryDTO>> GetAllProjectsAsync()
    {
        using var db = _connectionFactory.CreateConnection();
        return await db.QueryAsync<ProjectSummaryDTO>(
            "dbo.sp_GetAllProjects",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ProjectDetailDTO?> GetProjectDetailsAsync(int projectId)
    {
        using var db = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("ProjectId", projectId);

        using var multi = await db.QueryMultipleAsync(
            "dbo.sp_GetProjectDetails",
            parameters,
            commandType: CommandType.StoredProcedure);

        var detail = await multi.ReadFirstOrDefaultAsync<ProjectDetailDTO>();
        if (detail is null)
        {
            return null;
        }

        var members = (await multi.ReadAsync<ProjectMemberDTO>()).ToList();
        detail.Members = members;
        return detail;
    }

    public async Task<int> CreateAsync(Project project)
    {
        using var db = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("ProjectCode", project.ProjectCode);
        parameters.Add("ProjectName", project.ProjectName);
        parameters.Add("DepartmentId", project.DepartmentId);
        parameters.Add("StartDate", project.StartDate);
        parameters.Add("EndDate", project.EndDate);
        parameters.Add("Status", project.Status);

        return await db.ExecuteScalarAsync<int>(
            "dbo.sp_CreateProject",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task AssignEmployeeToProjectAsync(int projectId, AssignProjectRequestDTO request)
    {
        using var db = _connectionFactory.CreateConnection();
        db.Open();
        using var tx = db.BeginTransaction();

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("ProjectId", projectId);
            parameters.Add("EmployeeId", request.EmployeeId);
            parameters.Add("RoleInProject", request.RoleInProject.Trim());
            parameters.Add("AllocationPercentage", request.AllocationPercentage);

            await db.ExecuteAsync(
                "dbo.sp_AssignEmployeeToProject",
                parameters,
                transaction: tx,
                commandType: CommandType.StoredProcedure);

            tx.Commit();
        }
        catch (SqlException ex)
        {
            tx.Rollback();
            throw MapAssignSqlException(ex);
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    private static Exception MapAssignSqlException(SqlException ex) => ex.Number switch
    {
        50011 or 50013 => new CommonExceptions(ex.Message, 404),
        50010 or 50012 or 50014 => new CommonExceptions(ex.Message, 400),
        50015 or 50016 => new CommonExceptions(ex.Message, 409),
        _ => ex
    };
}