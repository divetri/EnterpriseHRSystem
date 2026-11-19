using Dapper;
using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Entities;
using EnterpriseHR.Domain.Exceptions;
using EnterpriseHR.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EnterpriseHR.DataAccess.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public EmployeeRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync(
    string? search = null,
    int? departmentId = null,
    bool activeOnly = false)
    {
        using var db = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("Search", string.IsNullOrWhiteSpace(search) ? null : search.Trim());
        parameters.Add("DepartmentId", departmentId);
        parameters.Add("ActiveOnly", activeOnly ? 1 : 0);

        return await db.QueryAsync<Employee, Department, Position, Employee, EmployeeDTO>(
            "dbo.sp_GetAllEmployees",
            (emp, dept, pos, mgr) => new EmployeeDTO
            {
                Id = emp.Id,
                EmployeeNumber = emp.EmployeeNumber,
                FullName = emp.FullName,
                Email = emp.Email,
                DepartmentId = emp.DepartmentId,
                DepartmentName = dept.DepartmentName,
                PositionId = emp.PositionId,
                PositionTitle = pos.Title,
                ManagerId = emp.ManagerId,
                ManagerName = mgr?.FullName,
                Salary = emp.Salary,
                JoinDate = emp.JoinDate,
                IsActive = emp.IsActive
            },
            parameters,
            splitOn: "Id,Id,Id",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<PagedResultDTO<EmployeeDTO>> GetPagedActiveEmployeesAsync(
    int? departmentId,
    int page,
    int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        using var db = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("DepartmentId", departmentId);
        parameters.Add("Page", page);
        parameters.Add("PageSize", pageSize);

        using var multi = await db.QueryMultipleAsync(
            "dbo.sp_GetPagedActiveEmployees",
            parameters,
            commandType: CommandType.StoredProcedure);

        var totalCount = await multi.ReadFirstAsync<int>();

        var items = multi.Read<Employee, Department, Position, Employee, EmployeeDTO>(
            (emp, dept, pos, mgr) => new EmployeeDTO
            {
                Id = emp.Id,
                EmployeeNumber = emp.EmployeeNumber,
                FullName = emp.FullName,
                Email = emp.Email,
                DepartmentId = emp.DepartmentId,
                DepartmentName = dept.DepartmentName,
                PositionId = emp.PositionId,
                PositionTitle = pos.Title,
                ManagerId = emp.ManagerId,
                ManagerName = mgr?.FullName,
                Salary = emp.Salary,
                JoinDate = emp.JoinDate,
                IsActive = emp.IsActive
            },
            splitOn: "Id,Id,Id").ToList();

        return new PagedResultDTO<EmployeeDTO>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        using var db = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        return await db.QueryFirstOrDefaultAsync<Employee>(
            "dbo.sp_GetEmployeeById",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(Employee employee)
    {
        using var db = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("EmployeeNumber", employee.EmployeeNumber);
        parameters.Add("FullName", employee.FullName);
        parameters.Add("Email", employee.Email);
        parameters.Add("DepartmentId", employee.DepartmentId);
        parameters.Add("PositionId", employee.PositionId);
        parameters.Add("ManagerId", employee.ManagerId);
        parameters.Add("Salary", employee.Salary);
        parameters.Add("JoinDate", employee.JoinDate);
        parameters.Add("IsActive", employee.IsActive);

        return await db.ExecuteScalarAsync<int>(
            "dbo.sp_CreateEmployee",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> UpdateAsync(Employee employee)
    {
        using var db = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();
        parameters.Add("Id", employee.Id);
        parameters.Add("EmployeeNumber", employee.EmployeeNumber);
        parameters.Add("FullName", employee.FullName);
        parameters.Add("Email", employee.Email);
        parameters.Add("DepartmentId", employee.DepartmentId);
        parameters.Add("PositionId", employee.PositionId);
        parameters.Add("ManagerId", employee.ManagerId);
        parameters.Add("Salary", employee.Salary);
        parameters.Add("JoinDate", employee.JoinDate);
        parameters.Add("IsActive", employee.IsActive);

        var affected = await db.ExecuteScalarAsync<int>(
            "dbo.sp_UpdateEmployee",
            parameters,
            commandType: CommandType.StoredProcedure);

        return affected > 0;
    }
    public async Task UpdateEmployeeSalaryAsync(int employeeId, UpdateSalaryRequestDTO request)
    {
        using var db = _connectionFactory.CreateConnection();
        db.Open();
        using var tx = db.BeginTransaction();

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("EmployeeId", employeeId);
            parameters.Add("NewSalary", request.NewSalary);
            parameters.Add("NewPositionId", request.NewPositionId);
            parameters.Add("Reason", string.IsNullOrWhiteSpace(request.Reason)
                ? "Salary adjustment"
                : request.Reason.Trim());

            await db.ExecuteAsync(
                "dbo.sp_UpdateEmployeeSalary",
                parameters,
                transaction: tx,
                commandType: CommandType.StoredProcedure);

            tx.Commit();
        }
        catch (SqlException ex)
        {
            tx.Rollback();
            throw MapSalarySqlException(ex);
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public async Task DeleteEmployeeAsync(int employeeId)
    {
        using var db = _connectionFactory.CreateConnection();
        db.Open();
        using var tx = db.BeginTransaction();

        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("EmployeeId", employeeId);

            await db.ExecuteAsync(
                "dbo.sp_SoftDeleteEmployee",
                parameters,
                transaction: tx,
                commandType: CommandType.StoredProcedure);

            tx.Commit();
        }
        catch (SqlException ex)
        {
            tx.Rollback();
            throw MapDeleteSqlException(ex);
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }
    private static Exception MapSalarySqlException(SqlException ex) => ex.Number switch
    {
        50001 => new CommonExceptions(ex.Message, 404), // Employee not found
        50002 or 50003 => new CommonExceptions(ex.Message, 400), // Position not found / salary out of range
        _ => ex
    };

    private static Exception MapDeleteSqlException(SqlException ex) => ex.Number switch
    {
        50004 => new CommonExceptions(ex.Message, 404), // Employee not found
        50005 or 50006 => new CommonExceptions(ex.Message, 409), // Still active manager / active project
        _ => ex
    };
}