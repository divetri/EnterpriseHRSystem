USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_GetDepartmentSummary')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_GetDepartmentSummary
GO

CREATE PROCEDURE dbo.sp_GetDepartmentSummary
AS
BEGIN
    SET NOCOUNT ON;

    WITH ActiveEmployeeStats AS
    (
        SELECT
            e.DepartmentId,
            COUNT(*) AS ActiveEmployeeCount,
            COALESCE(SUM(e.Salary), 0) AS TotalMonthlySalary
        FROM dbo.Employees e
        WHERE e.IsActive = 1
        GROUP BY e.DepartmentId
    ),
    ActiveProjectStats AS
    (
        SELECT
            p.DepartmentId,
            COUNT(*) AS ActiveProjectCount
        FROM dbo.Projects p
        WHERE p.Status = 'Active'
        GROUP BY p.DepartmentId
    )
    SELECT
        d.Id AS DepartmentId,
        d.DepartmentCode,
        d.DepartmentName,
        d.Budget,
        COALESCE(emp.ActiveEmployeeCount, 0) AS ActiveEmployeeCount,
        COALESCE(emp.TotalMonthlySalary, 0) AS TotalMonthlySalary,
        d.Budget - COALESCE(emp.TotalMonthlySalary, 0) AS RemainingBudget,
        COALESCE(prj.ActiveProjectCount, 0) AS ActiveProjectCount
    FROM dbo.Departments d
    LEFT JOIN ActiveEmployeeStats emp ON d.Id = emp.DepartmentId
    LEFT JOIN ActiveProjectStats prj ON d.Id = prj.DepartmentId
    ORDER BY d.DepartmentName;
END;
GO