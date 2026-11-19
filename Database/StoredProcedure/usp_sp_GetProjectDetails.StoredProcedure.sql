USE EnterpriseHRDB;
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_GetProjectDetails')
                    AND type IN ( N'P', N'PC' ) ) 
DROP PROCEDURE dbo.sp_GetProjectDetails
GO

CREATE PROCEDURE dbo.sp_GetProjectDetails
    @ProjectId INT
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
        p.Status
    FROM dbo.Projects p
    INNER JOIN dbo.Departments d ON p.DepartmentId = d.Id
    WHERE p.Id = @ProjectId;

    SELECT
        pa.Id AS AssignmentId,
        e.Id AS EmployeeId,
        e.EmployeeNumber,
        e.FullName,
        pa.RoleInProject,
        pa.AllocationPercentage,
        pa.AssignedDate
    FROM dbo.ProjectAssignments pa
    INNER JOIN dbo.Employees e ON pa.EmployeeId = e.Id
    WHERE pa.ProjectId = @ProjectId
    ORDER BY pa.AssignedDate DESC;
END;
GO