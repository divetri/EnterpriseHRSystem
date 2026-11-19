USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_GetPagedActiveEmployees')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_GetPagedActiveEmployees
GO

USE EnterpriseHRDB;
GO

CREATE PROCEDURE dbo.sp_GetPagedActiveEmployees
    @DepartmentId INT = NULL,
    @Page INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF @Page < 1 SET @Page = 1;
    IF @PageSize < 1 SET @PageSize = 10;

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    SELECT COUNT(*)
    FROM dbo.Employees e
    WHERE e.IsActive = 1
      AND (@DepartmentId IS NULL OR e.DepartmentId = @DepartmentId);

    SELECT
        e.Id, e.EmployeeNumber, e.FullName, e.Email, e.DepartmentId,
        e.PositionId, e.ManagerId, e.Salary, e.JoinDate, e.IsActive,
        d.Id, d.DepartmentCode, d.DepartmentName, d.Budget,
        p.Id, p.Title, p.MinSalary, p.MaxSalary,
        m.Id, m.EmployeeNumber, m.FullName, m.Email, m.DepartmentId,
        m.PositionId, m.ManagerId, m.Salary, m.JoinDate, m.IsActive
    FROM dbo.Employees e
    INNER JOIN dbo.Departments d ON e.DepartmentId = d.Id
    INNER JOIN dbo.Positions p ON e.PositionId = p.Id
    LEFT JOIN dbo.Employees m ON e.ManagerId = m.Id
    WHERE e.IsActive = 1
      AND (@DepartmentId IS NULL OR e.DepartmentId = @DepartmentId)
    ORDER BY e.Id
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;
GO