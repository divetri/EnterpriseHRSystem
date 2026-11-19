USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_GetAllProjects')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_GetAllProjects
GO

CREATE PROCEDURE dbo.sp_GetAllProjects
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.Id,
        p.ProjectCode,
        p.ProjectName,
        p.DepartmentId,
        d.DepartmentName,
        p.StartDate,
        p.EndDate,
        p.Status,
        COUNT(CASE WHEN e.IsActive = 1 THEN pa.EmployeeId END) AS ActiveMemberCount
    FROM dbo.Projects p
    INNER JOIN dbo.Departments d ON p.DepartmentId = d.Id
    LEFT JOIN dbo.ProjectAssignments pa ON p.Id = pa.ProjectId
    LEFT JOIN dbo.Employees e ON pa.EmployeeId = e.Id
    GROUP BY
        p.Id, p.ProjectCode, p.ProjectName, p.DepartmentId,
        d.DepartmentName, p.StartDate, p.EndDate, p.Status
    ORDER BY p.Id DESC;
END;
GO