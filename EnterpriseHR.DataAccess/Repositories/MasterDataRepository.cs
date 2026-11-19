using System.Data;
using Dapper;
using EnterpriseHR.Domain.DTOs;
using EnterpriseHR.Domain.Entities;
using EnterpriseHR.Domain.Interfaces;

namespace EnterpriseHR.DataAccess.Repositories;

public class MasterDataRepository : IMasterDataRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public MasterDataRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        using var db = _connectionFactory.CreateConnection();
        const string sql = @"
            SELECT Id, DepartmentCode, DepartmentName, Budget
            FROM dbo.Departments
            ORDER BY DepartmentName;";

        return await db.QueryAsync<Department>(sql);
    }

    public async Task<IEnumerable<Position>> GetAllPositionsAsync()
    {
        using var db = _connectionFactory.CreateConnection();
        const string sql = @"
            SELECT Id, Title, MinSalary, MaxSalary
            FROM dbo.Positions
            ORDER BY Title;";

        return await db.QueryAsync<Position>(sql);
    }

    public async Task<IEnumerable<DepartmentSummaryDTO>> GetDepartmentSummaryAsync()
    {
        using var db = _connectionFactory.CreateConnection();
        return await db.QueryAsync<DepartmentSummaryDTO>(
            "dbo.sp_GetDepartmentSummary",
            commandType: CommandType.StoredProcedure);
    }
}